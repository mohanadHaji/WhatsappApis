namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.ApiSchema
{
    public class ResponseSchema
    {
        public string Recipient { get; set; }
        public bool IsSuccess { get; set; }
        public string? ResponseContent { get; set; }
    }
}
