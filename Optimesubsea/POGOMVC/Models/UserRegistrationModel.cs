using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POGOMVC.Models
{
    public class UserRegistrationModel : CommonModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Passcode { get; set; }
        public string EmailId { get; set; }
        public string Narration { get; set; }
        public RolesModel? UserRoleId { get; set; }
        public int? UserRoleIdId { get; set; }
        public int? PasscodeRecoveryQuestionnaireId1Id { get; set; }
        public PasscodeRecoveryQuestionnaireModel? PasscodeRecoveryQuestionnaireId1 { get; set; }
        public string? PasscodeRecoveryAnswer1 { get; set; }
        [NotMapped]
        public List<SelectListItem>? Roles { get; set; }
        [NotMapped]
        public List<SelectListItem>? Questionnaire1 { get; set; }
        [NotMapped]
        public List<SelectListItem>? Projects { get; set; }
        [NotMapped]
        public int[]? ProjectIds { get; set; }
        [NotMapped]
        public string? ErrorMessage { get; set; }
    }
}
