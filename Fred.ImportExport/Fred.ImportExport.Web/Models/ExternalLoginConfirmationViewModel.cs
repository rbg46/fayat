using System.ComponentModel.DataAnnotations;

namespace Fred.ImportExport.Web.Models
{
  public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
