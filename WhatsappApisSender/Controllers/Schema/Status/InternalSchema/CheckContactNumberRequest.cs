namespace WhatsappApisSender.Controllers.Schema.Status.InternalSchema
{
    public class CheckContactNumberRequest
    {
        public string Blocking {  get; set; }

        public List<string> Contacts {  get; set; }
    }
}
