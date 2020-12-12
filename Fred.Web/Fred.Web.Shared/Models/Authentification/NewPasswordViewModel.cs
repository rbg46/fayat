using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Models.Authentification
{
    public class NewPasswordViewModel
    {
        [DisplayName("Mot de Passe")]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        public string Password { get; set; }

        [DisplayName("Vérification du mot de passe")]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        public string PasswordVerify { get; set; }

        public string Guid { get; set; }

        public string Message { get; set; }

        public bool Success { get; set; }

        public bool GuidIsValid { get; set; }

        public bool GuidIsExpired { get; set; }
    }
}
