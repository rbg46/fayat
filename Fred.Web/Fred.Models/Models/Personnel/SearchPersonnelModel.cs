using Fred.Web.Models.Search;
using System;

namespace Fred.Web.Models.Personnel
{
  /// <summary>
  /// Représente un membre du personnel
  /// </summary>
  public class SearchPersonnelModel : AbstractSearchModel
  {
    /// <summary>
    /// Valeur textuelle recherchée
    /// </summary>
    public override string ValueText { get; set; }

    /// <summary>
    /// Obtient ou définit le nom du personnel.
    /// </summary>
    public bool Nom { get; set; }

    /// <summary>
    /// Obtient ou définit le prénom du personnel.
    /// </summary>
    public bool Prenom { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le code et libellé de la société.
    /// </summary>
    public bool SocieteCodeLibelle { get; set; }

    /// <summary>
    /// Obtient ou définit le matricule du personnel.
    /// </summary>
    public bool Matricule { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le personnel est interimaire ou non.
    /// </summary>
    public bool IsInterne { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le personnel est interne ou externe.
    /// </summary>
    public bool IsInterimaire { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le personnel est un utilisateur Fred.
    /// </summary>
    public bool IsUtilisateur { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le personnel est inactif.
    /// </summary>
    public bool IsActif { get; set; }

    /// <summary>
    ///  Obtient ou définit le tri : Nom et prénom
    /// </summary>
    public bool? NomPrenomAsc { get; set; }

    /// <summary>
    ///  Obtient ou définit le tri : Libelle Societe
    /// </summary>
    public bool? SocieteAsc { get; set; }

    /// <summary>
    ///  Obtient ou définit le tri : Matricule
    /// </summary>
    public bool? MatriculeAsc { get; set; }

  }
}