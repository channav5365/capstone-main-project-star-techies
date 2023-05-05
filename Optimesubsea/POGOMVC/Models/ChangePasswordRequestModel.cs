using Microsoft.AspNetCore.Mvc.Rendering;

namespace POGOMVC.Models
{
    public class ChangePasswordRequestModel
    {
        public string UserName { get; set; }
        public List<SelectListItem>? Questionnaire1 { get; set; }
        public int? PasscodeRecoveryQuestionnaireId1Id { get; set; }
        public string? PasscodeRecoveryAnswer1 { get; set; }
    }
}
