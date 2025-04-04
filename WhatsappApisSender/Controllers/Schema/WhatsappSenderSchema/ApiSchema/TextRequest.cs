using System.Text.Json.Serialization;

namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.ApiSchema
{
    public class TextRequest : BaseSchema
    {
        public TextContent Text { get; set; }

        public override InternalSchema.BaseSchema ToInternalSchema()
        {
            return new InternalSchema.TextRequest()
            {
                Text = new() { Body = this.Text.Body, Preview_Url = this.Text.Preview_Url },
            };
        }

        public override List<string> Validate()
        {
            var errors = base.Validate();

            if (Text == null)
            {
                errors.Add("Text content is required.");
            }
            else
            {
                if (string.IsNullOrEmpty(Text.Body))
                {
                    errors.Add("Text Body is required.");
                }
            }

            return errors;
        }

    }

    public class TextContent
    {
        public bool Preview_Url { get; set; }
        public string Body { get; set; }
    }
}
