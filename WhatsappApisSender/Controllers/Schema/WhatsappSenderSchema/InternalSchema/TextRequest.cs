using System.Text.Json.Serialization;

namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.InternalSchema
{
    public class TextRequest : BaseSchema
    {
        public string Type { get; set; } = "text";
        public TextContent Text { get; set; }
    }

    public class TextContent
    {
        public bool Preview_Url { get; set; }
        public string Body { get; set; }
    }
}
