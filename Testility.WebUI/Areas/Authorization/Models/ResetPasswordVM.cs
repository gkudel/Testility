using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Testility.WebUI.Areas.Authorization.Models
{
    public class ResetPasswordVM
    {
        [Required]
        public string Email {get;set;}
    }
}