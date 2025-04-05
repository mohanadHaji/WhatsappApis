namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.ApiSchema
{
    public abstract class BaseSchema
    {
        public List<string> To { get; set; }
        public string AccessToken { get; set; }
        public abstract InternalSchema.BaseSchema ToInternalSchema();
        public int DelayBetweenMessagesInMs { get; set; } = 0;
        public DateTime? ScheduledTimeInUtc { get; set; }


        public virtual List<string> Validate()
        {
            var errors = new List<string>();

            if (To == null || !To.Any())
                errors.Add("Recipient list 'To' must not be null or empty.");

            if (string.IsNullOrWhiteSpace(AccessToken))
                errors.Add("AccessToken is required.");

            if (DelayBetweenMessagesInMs < 0)
                errors.Add("delay should be > 0");

            if (ScheduledTimeInUtc != null && ScheduledTimeInUtc < DateTime.UtcNow)
                errors.Add("ScheduledTimeInUtc should be > current time");

            return errors;
        }
    }
}
