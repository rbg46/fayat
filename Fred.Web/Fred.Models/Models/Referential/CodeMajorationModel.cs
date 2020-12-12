using Fred.Web.Models.CI;
using Fred.Web.Models.Groupe;
using System.Collections.Generic;

namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Représente un code fonction
  /// </summary>
  public class CodeMajorationModel : IReferentialModel
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
    /// Obtient ou définit l'état public ou privé d'un code majoration.
    /// </summary>
    public bool EtatPublic { get; set; }

    /// <summary>
    /// Obtient ou définit si le code majoration est actif ou non
    /// </summary>
    public string Public
    {
      get
      {
        return this.EtatPublic ? "oui" : "non";
      }
    }

    /// <summary>
    /// Obtient ou définit si le code majoration est actif ou non
    /// </summary>
    public bool IsActif { get; set; }

    /// <summary>
    /// Obtient ou définit si le code majoration est actif ou non
    /// </summary>
    public string Actif
    {
      get
      {
        return this.IsActif ? "oui" : "non";
      }
    }

    /// <summary>
    /// Obtient ou définit l'identifiant unique du groupe associée.
    /// </summary>
    public int GroupeId { get; set; }

    /// <summary>
    /// Obtient ou définit du groupe associée.
    /// </summary>
    public GroupeModel Groupe { get; set; }

    /// <summary>
    /// Obtient ou définit le nom de la société associée
    /// </summary>
    public string GroupeNom => Groupe?.Libelle;

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la société associée.
    /// </summary>
    public ICollection<CICodeMajorationModel> CICodesMajoration { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la société associée.
    /// </summary>
    public bool IsLinkedToCI { get; set; }

    /// <summary>
    /// Obtient l'identifiant du référentiel CodeMajoration
    /// </summary>
    public string IdRef => this.CodeMajorationId.ToString();

    /// <summary>
    /// Obtient le libelle du référentiel CodeMajoration
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient le code du référentiel CodeMajoration
    /// </summary>
    public string CodeRef => this.Code;
  }
}
