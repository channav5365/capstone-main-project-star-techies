using System.ComponentModel.DataAnnotations;

namespace POGOMVC.Models
{
    public class FileUploadModel
    {
        [Key]
        public int Id { get; set; }
        public int C1 { get; set; }
        public int C2 { get; set; }
        public DateTime? C3 { get; set; }
        public int C4 { get; set; }
    }
}
