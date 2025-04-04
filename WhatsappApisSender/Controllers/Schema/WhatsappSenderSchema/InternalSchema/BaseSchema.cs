namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.InternalSchema
{
    public abstract class BaseSchema
    {
        public string Messaging_Product { get; set; } = "whatsapp";
        public string Recipient_Type { get; set; } = "individual";
        public string To { get; set; }
    }
}
