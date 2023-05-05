using System.ComponentModel.DataAnnotations;

namespace POGOMVC.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Passcode { get; set; }
    }
}
