using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POGOMVC.Models
{
    public class ProjectModel : CommonModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string ProjectDescription { get; set; }
        [Required]
        public string ProjectType { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public UserRegistrationModel? SuperUser { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Required SuperUserId.")]
        public int? SuperUserId { get; set; }
        [NotMapped]
        public List<SelectListItem>? EndUsers { get; set; }
        [NotMapped]
        //[EnsureMinimumElements(min: 1, ErrorMessage = "Select at least one End User")]
        public int[] EndUserIds { get; set; }
        [NotMapped]
        public List<SelectListItem>? SuperUsers { get; set; }
        [NotMapped]
        public string? ErrorMessage { get; set; }
        [NotMapped]
        public string? AssignedRoleToProject { get; set; }
        [NotMapped]
        public List<SelectListItem>? SelectedEndUser { get; set; }
        public UserRegistrationModel? UploadDataEndUser { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Required UploadDataEndUserId.")]
        public int? UploadDataEndUserId { get; set; }

        [NotMapped]
        public string? SelectedSuperUserName { get; set; }

        [NotMapped]
        public string? SelectedEndUserNames { get; set; }
        [NotMapped]
        public string? SelectedEndUserFileUploadName { get; set; }
    }
}

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public EnsureMinimumElementsAttribute(int min = 0, int max = int.MaxValue)
        {
            _min = min;
            _max = max;
        }

        public override bool IsValid(object value)
        {
            if (!(value is IList list))
                return false;

            return list.Count >= _min && list.Count <= _max;
        }
    }
