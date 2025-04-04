namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.ApiSchema
{
    public class Media
    {
        public string? Link { get; set; }
        public string? Caption { get; set; }
        public string? Filename { get; set; }

        public List<string> Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Link))
                errors.Add("Media.Link is required.");
            else if (!Uri.IsWellFormedUriString(Link, UriKind.Absolute))
                errors.Add("Media.Link must be a valid URL.");

            return errors;
        }
    }
}
