using Fred.Web.Models.Personnel;


namespace Fred.Web.Models.Utilisateur
{
  public class UtilisateurLightModel
  {

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un utilisateur
    /// </summary>
    public int UtilisateurId { get; set; }   

    /// <summary>
    /// Obtient ou définit l'entité du membre du utilisateur ayant valider la commande
    /// </summary>
    public PersonnelLightModel Personnel { get; set; } = null;

    /// <summary>
    /// Obtient ou définit l'identifiant personnel de l'utilisateur
    /// </summary>
    public int? PersonnelId { get; set; }
  }
}

