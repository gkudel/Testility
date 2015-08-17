using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Testility.WebUI.CustomValidation
{
    public class CheckReferenceExtensions : ValidationAttribute
    {
        string referenceExtensions { get; set; }

        public CheckReferenceExtensions(string referenceExtension)
        {
            this.referenceExtensions = referenceExtension;
        }

        public override bool IsValid(object value)
        {
            if (value != null) //When action delete value is null 
            {
                var str = value as String;


                if (referenceExtensions == str.Substring(str.Length - referenceExtensions.Length))
                    return true;

                return false;
            }
            return true;
        }
    }
}