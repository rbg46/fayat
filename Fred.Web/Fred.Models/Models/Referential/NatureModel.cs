using Fred.Web.Models.Societe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Represente une code absence
  /// </summary>
  public class NatureModel : IReferentialModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une nature.
    /// </summary>
    public int NatureId { get; set; }

    /// <summary>
    /// Obtient ou définit le code de la nature
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la société 
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la société 
    /// </summary>
    public SocieteModel Societe { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la nature est active ou inactive
    /// </summary>
    public bool IsActif { get; set; }

    /// <summary>
    /// Obtient ou définit la date de creation du rapport
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'auteur de la création
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    /// Obtient ou définit la date de modification du rapport
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'auteur de la modification
    /// </summary>
    public int? AuteurModificationId { get; set; }

    ///// <summary>
    ///// Obtient ou définit la date de suppression du rapport
    ///// </summary>
    //public DateTime? DateSuppression { get; set; }

    ///// <summary>
    ///// Obtient ou définit l'identifiant de l'auteur de la suppression
    ///// </summary>
    //public int? AuteurSuppressionId { get; set; }

    /// <summary>
    /// Obtient l'identifiant du référentiel Nature
    /// </summary>
    public string IdRef => this.NatureId.ToString();

    /// <summary>
    /// Obtient le libelle du référentiel Nature
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient le code du référentiel Nature
    /// </summary>
    public string CodeRef => this.Code;

    /// <summary>
    /// Obtient ou définit la concaténation du code et du libelle
    /// </summary>
    public string CodeLibelle => this.Code + " - " + this.Libelle;
  }
}