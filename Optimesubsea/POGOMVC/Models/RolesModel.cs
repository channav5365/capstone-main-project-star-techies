using System.ComponentModel.DataAnnotations;

namespace POGOMVC.Models
{
    public class RolesModel : CommonModel
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Narration { get; set; }
        public ICollection<UserRegistrationModel> UserRegistrations { get; set; }
    }
}
