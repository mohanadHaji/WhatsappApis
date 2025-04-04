namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.ApiSchema
{
    public class TemplateRequest : BaseSchema
    {
        public Template Template { get; set; }

        public override InternalSchema.BaseSchema ToInternalSchema()
        {
            return new InternalSchema.TemplateRequest()
            {
                Template = new()
                { 
                    Name = this.Template.Name,
                    Language = new() { Code = this.Template.Language.Code, Policy = this.Template.Language.Policy },
                    Components = this.Template.Components != null ? new InternalSchema.Component()
                    {
                        Type = this.Template.Components.Type,
                        Parameters = this.Template.Components.Parameters?
                    .Select(p => new InternalSchema.Parameter
                    {
                        Type = p.Type,
                        Text = p.Text
                    })
                    .ToList()
                    } : null
                },
            };
        }

        public override List<string> Validate()
        {
            var errors = base.Validate();

            if (Template == null)
            {
                errors.Add("Template is required.");
            }
            else
            {
                if (string.IsNullOrEmpty(Template.Name))
                {
                    errors.Add("Template Name is required.");
                }

                if (Template.Language == null)
                {
                    errors.Add("Language is required in the Template.");
                }
                else
                {
                    if (string.IsNullOrEmpty(Template.Language.Code))
                    {
                        errors.Add("Language Code is required.");
                    }
                }
            }

            return errors;
        }
    }

    public class Template
    {
        public string Name { get; set; }
        public Language Language { get; set; }
        public Component? Components { get; set; }
    }

    public class Language
    {
        public string? Policy { get; set; }
        public string Code { get; set; }
    }

    public class Component
    {
        public string Type { get; set; }
        public List<Parameter>? Parameters { get; set; }
    }

    public class Parameter
    {
        public string Type { get; set; }
        public string Text { get; set; }
    }
}
