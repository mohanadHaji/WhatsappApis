using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using WhatsappApisSender.Configurations;
using WhatsappApisSender.Controllers.Schema.Status.InternalSchema;

namespace WhatsappApisSender.Handlers
{
    public class WhatsappStatusHandlers(IHttpClientFactory httpClientFactory, IOptions<WhatsAppStatusSettings> options) : IWhatsappStatusHandlers
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly WhatsAppStatusSettings _whatsAppSettings = options.Value;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

        public async Task<(bool IsSuccess, string ResponseContent)> CheckContactAsync(CheckContactNumberRequest requestMessage, string token)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, _whatsAppSettings.BaseUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(requestMessage, _jsonSerializerOptions);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return (response.IsSuccessStatusCode, content);
        }
    }
}
