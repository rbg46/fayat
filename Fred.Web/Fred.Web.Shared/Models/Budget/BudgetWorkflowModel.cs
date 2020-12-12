using Fred.Web.Models.Utilisateur;
using System;

namespace Fred.Web.Shared.Models.Budget
{
  public class BudgetWorkflowModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une tâche T4 dans un budget.
    /// </summary>
    public int BudgetWorkflowId { get; set; }


    /// <summary>
    ///   Obtient ou définit le CI auquel se budget appartient
    /// </summary>
    public BudgetEtatModel EtatInitial { get; set; }

    /// <summary>
    ///   Obtient ou définit le CI auquel se budget appartient
    /// </summary>
    public BudgetEtatModel EtatCible { get; set; }

    /// <summary>
    ///   Obtient ou définit le commentaire
    /// </summary>
    public string Commentaire { get; set; }


    /// <summary>
    ///   Obtient ou définit le CI auquel se budget appartient
    /// </summary>
    public UtilisateurModel Auteur { get; set; }

    /// <summary>
    ///   Obtient ou définit le commentaire
    /// </summary>
    public DateTime Date { get; set; }
  }
}
