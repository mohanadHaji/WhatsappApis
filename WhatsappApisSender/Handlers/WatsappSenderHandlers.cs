using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using WhatsappApisSender.Configurations;
using Microsoft.Extensions.Options;
using WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.InternalSchema;
using WhatsappApisSender.Storage;
using WhatsappApisSender.Models;

namespace WhatsappApisSender.Handlers
{
    public class WatsappSenderHandlers(IHttpClientFactory httpClientFactory, IOptions<WhatsAppSettings> options, IStorageManager storageManager) : IWatsappSenderHandlers
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly WhatsAppSettings _whatsAppSettings = options.Value;
        private readonly IStorageManager _storageManager = storageManager;

        public async Task<(bool IsSuccess, string ResponseContent)> SendMessageAsync(BaseSchema message, string token, string userEmail)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, _whatsAppSettings.BaseUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(message, message.GetType(), new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            AppUser? user = await _storageManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                _storageManager.UpdateUserHistory(user, new UserMessage() { PhoneNumber = message.To, RequestBody = json, IsSuccessfull = response.IsSuccessStatusCode, IsScheduled = false });
            }

            return (response.IsSuccessStatusCode, content);
        }

        public async Task<(bool IsSuccess, string ResponseContent)> SchedulMessageAsync(BaseSchema message, string token, string userEmail, DateTime dueDateUtc)
        {
            var json = JsonSerializer.Serialize(message, message.GetType(), new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            AppUser? user = await _storageManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                _storageManager.AddScheduledMessage(user, new UserScheduledMessage() { UserEmail = userEmail, RequestBody = json, Token = token, DueDateUTC = dueDateUtc, ToPhoneNumber = message.To });
            }

            return (true, "Message was Scheduled");
        }

        public async Task SendScheduledMessageAsync(string requestBody, string token, string userEmail, string toPhoneNumber)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, _whatsAppSettings.BaseUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            AppUser? user = await _storageManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                _storageManager.UpdateUserHistory(user, new UserMessage() { PhoneNumber = toPhoneNumber, RequestBody = requestBody, IsSuccessfull = response.IsSuccessStatusCode, IsScheduled = true });
            }
        }
    }
}
