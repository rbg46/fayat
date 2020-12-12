using System.Collections.Generic;
using Fred.Web.Models.CI;
using Fred.Web.Models.Societe;

namespace Fred.Web.Dtos.Mobile.Referential
{
  /// <summary>
  /// Représente un code fonction
  /// </summary>
  public class CodeMajorationDto : DtoBase
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un code majoration.
    /// </summary>
    public int CodeMajorationId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un code majoration.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un code majoration.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'état public ou privé d'un code majoration.
    /// </summary>
    public bool EtatPublic { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le code majoration est actif ou non
    /// </summary>
    public bool IsActif { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique du groupe associé.
    /// </summary>
    public int GroupeId { get; set; }

  }
}
