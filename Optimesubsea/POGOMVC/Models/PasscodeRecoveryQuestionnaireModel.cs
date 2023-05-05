using System.ComponentModel.DataAnnotations;

namespace POGOMVC.Models
{
    public class PasscodeRecoveryQuestionnaireModel:CommonModel
    {
        [Key]
        public int Id { get; set; }
        public string QuestionName { get; set; }
        public string QuestionNarration { get; set; }
        //public UserRegistrationModel UserId { get; set; }
        public ICollection<UserRegistrationModel> UserRegistrations { get; set; }
    }
}
