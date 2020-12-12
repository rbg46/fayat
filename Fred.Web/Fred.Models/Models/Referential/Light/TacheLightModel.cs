using Fred.Web.Models.Budget;
using System.Collections.Generic;

namespace Fred.Web.Models.Referential.Light
{
  /// <summary>
  /// Modèle d'une tâche.
  /// </summary>
  public class TacheLightModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une tache.
    /// </summary>
    public int TacheId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une tache.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une tache.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur de l'identifiant CI
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur .
    /// </summary>
    public int? ParentId { get; set; }


    /// <summary>
    /// Obtient ou définit une valeur .
    /// </summary>   
    public TacheLightModel Parent { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des taches enfants
    /// </summary>
    public virtual ICollection<TacheLightModel> TachesEnfants { get; set; }

    /// <summary>
    /// Obtient ou définit le niveau de la tache.
    /// </summary>
    public int? Niveau { get; set; }

    /// <summary>
    ///   Obtient ou définit la revision du budget auquelle cette tâche appartient
    /// </summary>
    public int? BudgetRevisionId { get; set; }

    /// <summary>
    ///   Obtient ou définit la revision du budget
    /// </summary>
    public BudgetRevisionModel BudgetRevision { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des ressources assiciées à une tâche T4 dans un budget
    ///   Ne devrait contenir des éléments que dans le cas d'une tâche T4
    /// </summary>
    public ICollection<RessourceTacheModel> RessourceTaches { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des recette par devise
    /// </summary>
    public IEnumerable<TacheRecetteModel> TacheRecettes { get; set; }

    /// <summary>
    /// QuantiteBase 
    /// </summary>
    public double? QuantiteBase { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur .
    /// </summary>
    public int? UniteId { get; set; }


    /// <summary>
    /// Obtient ou définit une valeur .
    /// </summary>   
    public UniteLightModel Unite { get; set; }


    ///// <summary>
    ///// PrixTotalQB
    ///// </summary>
    //public double? PrixTotalQB { get; set; }

    ///// <summary>
    ///// Correspond prix unitaire QB
    ///// </summary>
    //public double? PrixUnitaireQB { get; set; }

    ///// <summary>
    ///// TotalHeureMO
    ///// </summary>
    //public double? TotalHeureMO { get; set; }

    ///// <summary>
    ///// HeureMOUnite
    ///// </summary>
    //public double? HeureMOUnite { get; set; }

    /// <summary>
    /// QuantiteARealise
    /// </summary>
    public double? QuantiteARealise { get; set; }

    ///// <summary>
    ///// TotalT4
    ///// </summary>
    //public double? TotalT4 { get; set; }

    ///// <summary>
    ///// PrixUnitaireT4
    ///// </summary>
    //public double? PrixUnitaireT4 { get; set; }//calculé

    ///// <summary>
    ///// TotalHeureMOT4
    ///// </summary>
    //public double? TotalHeureMOT4 { get; set; }//calculé

    /// <summary>
    /// Avancement
    /// </summary>
    public bool? Avancement { get; set; }

    /// <summary>
    /// TypeAvancement
    /// </summary>
    public int? TypeAvancement { get; set; }

    ///// <summary>
    ///// NbrRessourcesToParam
    ///// </summary>
    //public int? NbrRessourcesToParam { get; set; }

  }
}