using Fred.Web.Models.Budget;
using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielEtendu;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.ReferentielFixe
{
  /// <summary>
  /// Représente un ressource
  /// </summary>
  public class RessourceModel : IReferentialModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une resource.
    /// </summary>
    public int RessourceId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique du sous-chapitre de la resource.
    /// </summary>
    public int SousChapitreId { get; set; }

    /// <summary>
    /// Obtient ou définit le sous-chapitre de la ressource.
    /// </summary>
    public SousChapitreModel SousChapitre { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un rôle
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un rôle
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit la valeur active d'une ressource
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant d'un type de ressource
    /// </summary>
    public int? TypeRessourceId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant d'un type de ressource
    /// </summary>
    public TypeRessourceModel TypeRessource { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant d'un carburant
    /// </summary>
    public int? CarburantId { get; set; }

    /// <summary>
    /// Obtient ou définit le carburant
    /// </summary>
    public CarburantModel Carburant { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant d'un type de ressource
    /// </summary>
    public decimal? Consommation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de création
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'auteur de la création
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    /// Obtient ou définit la date de suppression
    /// </summary>
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'auteur de la suppression
    /// </summary>
    public int? AuteurSuppressionId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'ID de la ressource Parent.
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    ///   Obtient ou définit la ressource Parent .
    /// </summary>
    public RessourceModel Parent { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des ressources enfants
    /// </summary>
    public ICollection<RessourceModel> RessourcesEnfants { get; set; }

    /// <summary>
    /// Obtient ou définit le référentiel étendu associé
    /// </summary>
    public ICollection<ReferentielEtenduModel> ReferentielEtendus { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des associations avec la liste des tâches
    /// </summary>
    public ICollection<RessourceTacheModel> RessourceTaches { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des associations avec les CI
    /// </summary>
    public ICollection<CIRessourceModel> CIRessources { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle
    {
      get
      {
        return this.Code + " - " + this.Libelle;
      }
    }

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
    public string SousChapitreLibelle
    {
      get
      {
        return this.SousChapitre != null ? this.SousChapitre.Libelle : string.Empty;
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
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string ChapitreLibelle
    {
      get
      {
        return (this.SousChapitre != null && this.SousChapitre.Chapitre != null) ? this.SousChapitre.Chapitre.Libelle : string.Empty;
      }
    }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string ConsommationLibelle
    {
      get
      {
        return this.Consommation != null ? this.Consommation.ToString() : string.Empty;
      }
    }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CarburantCode
    {
      get
      {
        return this.Carburant != null ? this.Carburant.Code : string.Empty;
      }
    }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CarburantLibelle
    {
      get
      {
        return this.Carburant != null ? this.Carburant.Libelle : string.Empty;
      }
    }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string TypeCode
    {
      get
      {
        return this.TypeRessource != null ? this.TypeRessource.Code.ToString() : string.Empty;
      }
    }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string TypeLibelle => TypeRessource?.Libelle ?? string.Empty;

    /// <summary>
    /// Obtient ou définit l'identifiant du référentiel
    /// </summary>
    public string IdRef => this.RessourceId.ToString();

    /// <summary>
    /// Obtient ou définit le libelle du référentiel
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient ou définit le code du référentiel
    /// </summary>
    public string CodeRef => this.Code;

    public bool IsChecked { get; set; }
  }
}