using System;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Dtos.Mobile
{
  /// <summary>
  /// Dto Devise
  /// </summary>
  public class DeviseDto
  {
    /// <summary>
    /// Obtient ou définit Code Devise de Devise.
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit Code ISO  2 Lettres devise de Devise.
    /// </summary>
    public string IsoCode { get; set; }

    /// <summary>
    /// Obtient ou définit Code ISO Nombre Devise de Devise.
    /// </summary>
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
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit Code Iso Pays 2 lettres de Devise.
    /// </summary>
    public string CodePaysIso { get; set; }

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
    /// Obtient ou définit une valeur indiquant si supprimé.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Obtient ou définit le seuil.
    /// </summary>
    /// <value>
    /// Le seuil.
    /// </value>
    public decimal Seuil { get; set; }

  }
}