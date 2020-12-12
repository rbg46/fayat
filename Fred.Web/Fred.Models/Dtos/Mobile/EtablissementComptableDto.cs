using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Societe;

namespace Fred.Web.Dtos.Mobile
{
  /// <summary>
  /// Représente un établissement comptable
  /// </summary>
  public class EtablissementComptableDto
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un établissement comptable.
    /// </summary>
    [Required]
    public int EtablissementComptableId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'organisation de l'établissement
    /// </summary>
    public int? OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la société de l'établissement
    /// </summary>
    public int? SocieteId { get; set; }

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
    /// Obtient ou définit une valeur indiquant si ModuleCommandeEnabled
    /// </summary>
    public bool ModuleCommandeEnabled { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si ModuleProductionEnabled
    /// </summary>
    public bool ModuleProductionEnabled { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de l'auteur de création
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de l'auteur de modification
    /// </summary>
    public int? AuteurModifcationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de l'auteur de suppression
    /// </summary>
    public int? AuteurSuppressionId { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si IsDeleted
    /// </summary>
    public bool IsDeleted { get; set; }
  }
}