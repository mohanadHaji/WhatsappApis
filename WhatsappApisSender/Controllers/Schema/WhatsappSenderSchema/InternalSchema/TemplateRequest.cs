namespace WhatsappApisSender.Controllers.Schema.WhatsappSenderSchema.InternalSchema
{
    public class TemplateRequest : BaseSchema
    {
        public string Type { get; set; } = "Template";
        public Template Template { get; set; }
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
