using System.ComponentModel.DataAnnotations;

namespace Fred.ImportExport.Web.Models
{

  public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
