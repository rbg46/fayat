using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Models.Authentification
{
    public class ConnectViewModel
    {
        [Required(ErrorMessageResourceName = "Authentification_Model_UtilisateurRequis", ErrorMessageResourceType = typeof(Shared.App_LocalResources.Authentification))]
        [DisplayName("Identifiant")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Authentification_Model_MotDePassRequis", ErrorMessageResourceType = typeof(Shared.App_LocalResources.Authentification))]
        [DisplayName("Mot de passe")]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        public string Password { get; set; }

        public string Error { get; set; }
        public string TechnicalError { get; set; }
    }
}