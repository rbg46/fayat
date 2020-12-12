using Fred.Web.Models.Budget;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Modèle d'une tâche.
  /// </summary>
  public class TacheModel : IReferentialModel
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
    /// Obtient ou définit le champs indiquant si une tâche est la tâche par défaut.
    /// </summary>
    public bool TacheParDefaut { get; set; }

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
    public TacheModel Parent { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des taches enfants
    /// </summary>
    public virtual ICollection<TacheModel> TachesEnfants { get; set; }

    /// <summary>
    /// Obtient ou définit le niveau de la tache.
    /// </summary>
    public int? Niveau { get; set; }

    /// <summary>
    /// Obtient ou définit si la tache est active.
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    /// Obtient ou définit l'identifiant de l'auteur de la création
    /// </summary>  
    public int? AuteurCreationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'auteur de la modification
    /// </summary>  
    public int? AuteurModificationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'auteur de la suppression
    /// </summary>  
    public int? AuteurSuppressionId { get; set; }

    /// <summary>
    /// Obtient ou définit la date de creation 
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de modification 
    /// </summary>   
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit la date de Suppression 
    /// </summary>
    public DateTime? DateSuppression { get; set; }


    public bool Selected { get; set; }

    /// <summary>
    ///   Obtient ou définit la revision du budget auquelle cette tâche appartient
    /// </summary>
    public int? BudgetRevisionId { get; set; }
    

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
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle => this.Code + " - " + this.Libelle;

    /// <summary>
    /// Obtient ou définit l'identifiant du référentiel prime
    /// </summary>
    public string IdRef => this.TacheId.ToString();

    /// <summary>
    /// Obtient ou définit le libelle du référentiel prime
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient ou définit le code du référentiel prime
    /// </summary>
    public string CodeRef => this.Code;

    /// <summary>
    /// QuantiteBase 
    /// </summary>
    public double? QuantiteBase { get; set; }

    /// <summary>
    /// PrixTotalQB
    /// </summary>
    public double? PrixTotalQB { get; set; }

    /// <summary>
    /// Correspond prix unitaire QB
    /// </summary>
    public double? PrixUnitaireQB { get; set; }

    /// <summary>
    /// TotalHeureMO
    /// </summary>
    public double? TotalHeureMO { get; set; }

    /// <summary>
    /// HeureMOUnite
    /// </summary>
    public double? HeureMOUnite { get; set; }

    /// <summary>
    /// QuantiteARealise
    /// </summary>
    public double? QuantiteARealise { get; set; }

    /// <summary>
    /// TotalT4
    /// </summary>
    public double? TotalT4 { get; set; }

    /// <summary>
    /// PrixUnitaireT4
    /// </summary>
    public double? PrixUnitaireT4 { get; set; }//calculé

    /// <summary>
    /// TotalHeureMOT4
    /// </summary>
    public double? TotalHeureMOT4 { get; set; }//calculé

    /// <summary>
    /// Avancement
    /// </summary>
    public bool? Avancement { get; set; }

    /// <summary>
    /// TypeAvancement
    /// </summary>
    public int? TypeAvancement { get; set; }

    /// <summary>
    /// NbrRessourcesToParam
    /// </summary>
    public int? NbrRessourcesToParam { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une unité.
    /// </summary>
    public int? UniteId { get; set; }
  }
}