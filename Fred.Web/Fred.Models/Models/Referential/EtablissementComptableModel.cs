using Fred.Web.Models.Organisation;
using Fred.Web.Models.Societe;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Représente un établissement comptable
  /// </summary>
  public class EtablissementComptableModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un établissement comptable.
    /// </summary>
    [Required]
    public int EtablissementComptableId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'organisation de l'établissement
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'organisation de l'établissement
    /// </summary>
    public OrganisationModel Organisation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la société de l'établissement
    /// </summary>
    public int? SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit la société de l'établissement
    /// </summary>
    public SocieteModel Societe { get; set; }

    /// <summary>
    /// Obtient ou définit le code de l'établissement comptable.
    /// </summary>
    [Required]
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé de l'établissement comptable.
    /// </summary>
    [Required]
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit l'Adresse de l'établissement comptable.
    /// </summary>
    public string Adresse { get; set; }

    /// <summary>
    /// Obtient ou définit la ville de l'établissement comptable.
    /// </summary>
    public string Ville { get; set; }

    /// <summary>
    /// Obtient ou définit le code postal de l'établissement comptable.
    /// </summary>
    public string CodePostal { get; set; }

    /// <summary>
    /// Obtient ou définit le pays de l'établissement comptable.
    /// </summary>
    public string Pays { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle => this.Code + " - " + this.Libelle;

  /// <summary>
  /// Obtient ou définit une valeur indiquant si ModuleCommandeEnabled
  /// </summary>
    public bool ModuleCommandeEnabled { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si ModuleProductionEnabled
    /// </summary>
    public bool ModuleProductionEnabled { get; set; }

    /// <summary>
    /// Date de création
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Date de modification
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Date de modification
    /// </summary>
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    /// Id de l'auteur de création
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    /// Id de l'auteur de modification
    /// </summary>
    public int? AuteurModificationId { get; set; }

    /// <summary>
    /// Id de l'auteur de suppression
    /// </summary>
    public int? AuteurSuppressionId { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si IsDeleted
    /// </summary>
    public bool IsDeleted { get; set; }
  }
}
