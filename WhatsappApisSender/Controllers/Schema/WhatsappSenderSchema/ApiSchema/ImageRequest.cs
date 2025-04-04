namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.ApiSchema
{
    public class ImageRequest : BaseSchema
    {
        public Media Image {  get; set; }

        public override InternalSchema.BaseSchema ToInternalSchema()
        {
            return new InternalSchema.ImageRequest()
            {
                Image = new InternalSchema.Image() { Caption = this.Image.Caption, Link = this.Image.Link },
            };
        }

        public override List<string> Validate()
        {
            var errors = base.Validate();

            if (Image == null)
                errors.Add("Image is required.");
            else
                errors.AddRange(Image.Validate());

            return errors;
        }

    }
}
