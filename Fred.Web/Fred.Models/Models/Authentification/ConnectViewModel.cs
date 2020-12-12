using Fred.Business.Authentification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Models.Authentification
{
  public class ConnectViewModel
  {
    [Required(ErrorMessageResourceName = "Username_required", ErrorMessageResourceType = typeof(Fred.Models.App_LocalResources.Authentification))]
    [DisplayName("Identifiant")]
    public string UserName { get; set; }

    [Required(ErrorMessageResourceName = "Password_required", ErrorMessageResourceType = typeof(App_LocalResources.Authentification))]
    [DisplayName("Mot de passe")]
    [DataType(DataType.Password)]
    [PasswordPropertyText]
    public string Password { get; set; }

    public AuthenticationStatus Status { get; set; }
  }
}