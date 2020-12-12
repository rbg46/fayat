using Fred.Web.Models.Referential.Light;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;


namespace Fred.Web.Models.Budget
{
  public class BudgetRevisionModelOld
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une revision d'un budget.
    /// </summary>    
    public int BudgetRevisionId { get; set; }


    /// <summary>
    ///   Obtient ou définit une valeur indiquant le statut du budget,
    ///   relatif à l'enum eStatutBudget
    /// </summary>    
    public int Statut { get; set; }


    /// <summary>
    ///   Obtient ou définit la liste des taches du bugdet
    /// </summary>
    public virtual ICollection<TacheLightModel> Taches { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du budget auquelle cette tâche appartient
    /// </summary>
    public int BudgetId { get; set; }


    /// <summary>
    ///   Obtient ou définit le budget auquelle cette tâche appartient
    /// </summary>
    public BudgetModelOld Budget { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de changement du statut vers "Validé"
    /// </summary>
    public DateTime? DateValidation { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de changement de statut vers "A Valider"
    /// </summary>
    public DateTime? DateaValider { get; set; }


    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la création
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'auteur de la création
    /// </summary>
    public UtilisateurLightModel AuteurCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la modification
    /// </summary>
    public int? AuteurModificationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'auteur de la modification
    /// </summary>
    public UtilisateurLightModel AuteurModification { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la validation
    /// </summary>
    public int? AuteurValidationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la validation
    /// </summary>
    public UtilisateurLightModel AuteurValidation { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de creation
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de modification
    /// </summary>
    public DateTime? DateModification { get; set; }


    /// <summary>
    ///   Obtient ou définit la somme des recettes
    /// </summary>
    public double Recettes { get; set; }

    /// <summary>
    ///   Obtient ou définit la somme des dépenses
    /// </summary>
    public double Depenses { get; set; }

    /// <summary>
    ///   Obtient ou définit la marge brute
    /// </summary>
    public double MargeBrute { get; set; }

    /// <summary>
    ///   Obtient ou définit le pourcentage de marge brute
    /// </summary>
    public double MargeBrutePercent { get; set; }

    /// <summary>
    ///   Obtient ou définit la marge brute
    /// </summary>
    public double MargeNette { get; set; }

    /// <summary>
    ///   Obtient ou définit le pourcentage de marge brute
    /// </summary>
    public double MargeNettePercent { get; set; }
  }
}
