using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;
using System;

namespace Fred.Web.Models.AffectationInterimaire
{
  /// <summary>
  ///   Représente une Affectation intérimaire.
  /// </summary>  
  public class AffectationInterimaireModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'un budget.
    /// </summary>
    public int AffectationInterimaireId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du Personnel.
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    ///   Obtient ou définit le Personnel.
    /// </summary>
    public PersonnelModel Personnel { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du Fournisseur.
    /// </summary>
    public int FournisseurId { get; set; }

    /// <summary>
    ///   Obtient ou définit le Fournisseur.
    /// </summary>
    public FournisseurModel Fournisseur { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la Société.
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    ///   Obtient ou définit la Société.
    /// </summary>
    public SocieteModel Societe { get; set; }

    /// <summary>
    ///   Obtient ou définit la date d'entrée.
    /// </summary>
    public DateTime? DateEntree { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de modification.
    /// </summary>
    public DateTime? DateSortie { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si une affectation est active.
    /// </summary>    
    public bool? IsActif { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si une affectation est hors énergie.
    /// </summary>    
    public bool? Energie { get; set; }
  }
}