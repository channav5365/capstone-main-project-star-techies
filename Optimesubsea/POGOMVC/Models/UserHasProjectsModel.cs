using System.ComponentModel.DataAnnotations;

namespace POGOMVC.Models
{
    public class UserHasProjectsModel : CommonModel
    {
        [Key]
        public int Id { get; set; }
        public int? UserRegistrationId { get; set; }
        public UserRegistrationModel? UserRegistration { get; set; }
        public int? ProjectId { get; set; }
        public ProjectModel? Project { get; set; }
    }
}
