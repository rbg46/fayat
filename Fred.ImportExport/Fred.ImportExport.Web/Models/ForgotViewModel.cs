﻿using System.ComponentModel.DataAnnotations;

namespace Fred.ImportExport.Web.Models
{
  public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}