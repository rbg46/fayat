using System.Collections.Generic;

namespace Fred.Web.Models.ReferentielFixe.Light
{
  /// <summary>
  ///   Représente un sous-chapitre allégé en données membres
  /// </summary>
  public class SousChapitreLightModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'un sous-chapitre.
    /// </summary>
    public int SousChapitreId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'objet groupe attaché à un chapitre
    /// </summary>
    public ChapitreLightModel Chapitre { get; set; }

    /// <summary>
    ///   Obtient ou définit le code d'un sous-chapitre
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///   Obtient ou définit le libellé d'un sous-chapitre
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des ressources associées au sous-chapitre
    /// </summary>
    public ICollection<RessourceLightModel> Ressources { get; set; }
  }
}