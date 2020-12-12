using System.ComponentModel.DataAnnotations;
using System;

namespace Fred.Web.Models.Referential
{
  public class DeviseModel : IReferentialModel
  {
    /// <summary>
    /// Obtient ou définit Code Devise de Devise.
    /// </summary>

    public int DeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit Code ISO  2 Lettres devise de Devise.
    /// </summary>
    [Required]
    public string IsoCode { get; set; }

    /// <summary>
    /// Obtient ou définit Code ISO Nombre Devise de Devise.
    /// </summary>
    [Required]
    public string IsoNombre { get; set; }

    /// <summary>
    /// Obtient ou définit Symbole de la devise de Devise.
    /// </summary>
    public string Symbole { get; set; }

    /// <summary>
    /// Obtient ou définit Code Html de la devise de Devise.
    /// </summary>
    public string CodeHtml { get; set; }

    /// <summary>
    /// Obtient ou définit Libellé de la devise de Devise.
    /// </summary>
    [Required]
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit Code Iso Pays 2 lettres de Devise.
    /// </summary>

    public string CodePaysIso { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si une devise est active ou non.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Obtient ou définit Date de création de Devise.
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit Date de modification de Devise.
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit Statut Technique Suppression de Devise.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Obtient ou définit Date de suppression de Devise.
    /// </summary>
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit Auteur de modification de Devise.
    /// </summary>
    public int? AuteurModification { get; set; }

    /// <summary>
    /// Obtient ou définit Auteur de suppression de Devise.
    /// </summary>
    public int? AuteurSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit Auteur de création de Devise.
    /// </summary>
    public int? AuteurCreation { get; set; }

    /// <summary>
    /// Obtient ou définit référence
    /// </summary>
    public bool? Reference { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle => this.IsoCode + " - " + this.Libelle;

    public decimal Seuil { get; set; }

    /// <summary>
    /// Obtient le libellé référentiel d'un personnel.
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient le code référentiel d'un personnel.
    /// </summary>
    public string CodeRef => this.IsoCode;

    /// <summary>
    /// Obtient l'id du référentiel
    /// </summary>
    public string IdRef => this.DeviseId.ToString();
  }
}