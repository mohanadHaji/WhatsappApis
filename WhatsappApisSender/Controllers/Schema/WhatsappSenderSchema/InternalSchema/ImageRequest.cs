namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.InternalSchema
{
    public class ImageRequest : BaseSchema
    {
        public string Type { get; set; } = "Image";
        public Image Image {  get; set; }
    }

    public class Image
    {
        public string? Link { get; set; }
        public string? Caption { get; set; }
    }
}
