using Fred.Entities;
using Fred.Entities.Utilisateur;

namespace Fred.Entites
{
  /// <summary>
  ///   Initialise une nouvelle instance de la classe <see cref="AuthenticationStatus" />
  /// </summary>
  public class AuthenticationStatus
  {
    /// <summary>
    ///   Obtient ou définit une valeur indiquant si l'authentification est validé.
    /// </summary>
    public bool Success { get; set; }  

    /// <summary>
    ///   Obtient ou définit la raison d'erreur d'authentification
    /// </summary>
    public string ErrorAuthReason { get; set; }        

    /// <summary>
    ///   Obtient ou définit la fiche complète de l'utilisateur
    /// </summary>
    public UtilisateurEnt Utilisateur { get; set; }


    /// <summary>
    /// Etat de la connexion
    /// </summary>
    public ConnexionStatus ConnexionStatus { get; set; }

    /// <summary>
    /// Erreur technique
    /// </summary>
    public string TechnicalError { get; set; }
  }
 
}
