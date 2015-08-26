using System.ComponentModel.DataAnnotations;

namespace Testility.WebUI.Areas.Authorization.Models
{
    public class VerifyCodeVM
    {
        [Required]
        public string token { get; set; }
    }
}