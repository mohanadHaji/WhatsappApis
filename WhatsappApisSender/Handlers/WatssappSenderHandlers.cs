using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using WhatsappApisSender.Configurations;
using Microsoft.Extensions.Options;
using WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.InternalSchema;

namespace WhatsappApisSender.Handlers
{
    public class WatssappSenderHandlers(IHttpClientFactory httpClientFactory, IOptions<WhatsAppSettings> options) : IWatssappSenderHandlers
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly WhatsAppSettings _whatsAppSettings = options.Value;

        public async Task<(bool IsSuccess, string ResponseContent)> SendMessageAsync(BaseSchema message, string token)
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

            return (response.IsSuccessStatusCode, content);
        }
    }
}
