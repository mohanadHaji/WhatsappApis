namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.ApiSchema
{
    public class DocumentRequest : BaseSchema
    {
        public Media Document { get; set; }

        public override InternalSchema.BaseSchema ToInternalSchema()
        {
            return new InternalSchema.DocumentRequest()
            {
                Document = new InternalSchema.Document() { Filename = this.Document.Filename, Link = this.Document.Link },
            };
        }

        public override List<string> Validate()
        {
            var errors = base.Validate();

            if (Document == null)
                errors.Add("Document is required.");
            else
            {
                errors.AddRange(Document.Validate());
                if (string.IsNullOrEmpty(Document.Filename))
                {
                    errors.Add("File name should not be empty");
                }
            }

            return errors;
        }
    }
}
