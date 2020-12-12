using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Personnel
{


    public class Grid : ValidationAttribute
    {
        public Grid()
        {
            
        }

        public string[] PropertyNames { get; private set; }
        public int MinLength { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            return null;
        }


    }
}