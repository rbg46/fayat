using System.ComponentModel;

namespace Fred.Web.Models.Authentification
{
    public class ResetPasswordViewModel
    {
        [DisplayName("Identifiant")]
        public string UserName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public bool Success { get; set; }
    }
}
