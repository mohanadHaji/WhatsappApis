using System.ComponentModel.DataAnnotations;

namespace WhatsappApisSender.Controllers.Schema.Status.ApiSchema
{
    public class CheckContactNumberRequest
    {
        [Required(ErrorMessage = "Blocking is required.")]
        [MinLength(1, ErrorMessage = "Blocking cannot be empty.")]
        public string Blocking {  get; set; }

        [Required(ErrorMessage = "At least one contact is required.")]
        [MinLength(1, ErrorMessage = "At least one contact is required.")]
        public List<string> Contacts {  get; set; }

        [Required(ErrorMessage = "token is required.")]
        [MinLength(1, ErrorMessage = "token can't be empty.")]
        public string StatusCheckerToken {  get; set; }

        public InternalSchema.CheckContactNumberRequest ToInternalSchema()
        {
            return new InternalSchema.CheckContactNumberRequest { Blocking = Blocking, Contacts = Contacts };
        }
    }
}
