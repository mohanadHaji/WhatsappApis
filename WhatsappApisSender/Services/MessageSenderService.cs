using WhatsappApisSender.Handlers;
using WhatsappApisSender.Storage;

namespace WhatsappApisSender.Services
{
    public class MessageSenderService(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var storageManager = scope.ServiceProvider
                        .GetRequiredService<IStorageManager>();
                var watsappSenderHandlers = scope.ServiceProvider
                    .GetRequiredService<IWatsappSenderHandlers>();

                var dueMessages = await storageManager.FindDueScheduledMessagesAsync();

                foreach (var message in dueMessages)
                {
                    try
                    {
                        await watsappSenderHandlers.SendScheduledMessageAsync(
                        message.RequestBody,
                        message.Token,
                        message.UserEmail,
                        message.ToPhoneNumber);

                        await storageManager.RemovecheduledMessage(message.Id.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
