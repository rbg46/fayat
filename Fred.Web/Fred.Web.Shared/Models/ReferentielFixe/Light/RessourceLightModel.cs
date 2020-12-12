using System.Collections.Generic;
using Fred.Web.Models.ReferentielEtendu.Light;

namespace Fred.Web.Models.ReferentielFixe.Light
{
  /// <summary>
  ///   représente une ressource allégée en données membres
  /// </summary>
  public class RessourceLightModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une ressource.
    /// </summary>
    public int RessourceId { get; set; }

    /// <summary>
    /// Obtient ou définit le sous-chapitre de la ressource.
    /// </summary>
    public SousChapitreLightModel SousChapitre { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique du sous-chapitre de la resource.
    /// </summary>
    public int SousChapitreId { get; set; }

    /// <summary>
    ///   Obtient ou définit le code d'un rôle
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///   Obtient ou définit le libellé d'un rôle
    /// </summary>
    public string Libelle { get; set; }
    
    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle => this.Code + " - " + this.Libelle;

    /// <summary>
    ///   Obtient ou définit l'identifiant d'un type de ressource
    /// </summary>
    public int? TypeRessourceId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'ID de la ressource Parent.
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string SousChapitreCode
    {
      get
      {
        return this.SousChapitre != null ? this.SousChapitre.Code : string.Empty;
      }
    }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string ChapitreCode
    {
      get
      {
        return (this.SousChapitre != null && this.SousChapitre.Chapitre != null) ? this.SousChapitre.Chapitre.Code : string.Empty;
      }
    }

    /// <summary>
    ///   Obtient ou définit la liste des ressources enfants
    /// </summary>
    public ICollection<RessourceLightModel> RessourcesEnfants { get; set; }

    /// <summary>
    ///   Obtient ou définit le référentiel étendu associé
    /// </summary>
    public ICollection<ReferentielEtenduLightModel> ReferentielEtendus { get; set; }
  }
}