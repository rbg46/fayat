using Fred.Entities;
using Fred.Web.Models.Fonctionnalite;

namespace Fred.Web.Shared.Models.RoleFonctionnalite
{
  public class RoleFonctionnaliteModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique de l'association
    /// </summary>  
    public int RoleFonctionnaliteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'un groupe.
    /// </summary>   
    public int RoleId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une Fonctionnalite.
    /// </summary>  
    public int FonctionnaliteId { get; set; }

    /// <summary>
    ///   Obtient ou définit le FonctionnaliteEnt associé
    /// </summary>  
    public virtual FonctionnaliteLightModel Fonctionnalite { get; set; }

    /// <summary>
    /// Code du module de la fonctionnalité
    /// </summary>
    public string ModuleCode { get; set; }

    /// <summary>
    /// Libelle du module de la fonctionnalité
    /// </summary>
    public string ModuleLibelle { get; set; }

    /// <summary>
    ///  Description du module de la fonctionnalité
    /// </summary>
    public string ModuleDescription { get; set; }


    /// <summary>
    ///  Obtient ou définit le mode de la FonctionnaliteEnt
    /// </summary>
    public FonctionnaliteTypeMode Mode { get; set; }
  }
}
