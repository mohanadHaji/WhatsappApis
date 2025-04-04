namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.InternalSchema
{
    public class DocumentRequest : BaseSchema
    {
        public string Type { get; set; } = "Document";
        public Document Document { get; set; }
    }

    public class Document
    {
        public string? Link { get; set; }
        public string? Filename { get; set; }
    }
}
