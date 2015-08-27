using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Testility.WebUI.Areas.Authorization.Models
{
    public class ConfirmPasswordResetVM
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}