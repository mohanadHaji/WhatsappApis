namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.ApiSchema
{
    public abstract class BaseSchema
    {
        public List<string> To { get; set; }
        public string AccessToken { get; set; }

        public abstract InternalSchema.BaseSchema ToInternalSchema();

        public virtual List<string> Validate()
        {
            var errors = new List<string>();

            if (To == null || !To.Any())
                errors.Add("Recipient list 'To' must not be null or empty.");

            if (string.IsNullOrWhiteSpace(AccessToken))
                errors.Add("AccessToken is required.");

            return errors;
        }
    }
}
