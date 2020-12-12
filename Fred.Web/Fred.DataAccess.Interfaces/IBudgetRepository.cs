using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Dao.BudgetComparaison.ExcelExport;
using Fred.Web.Shared.Models.Budget;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Référentiel de données pour les Budgets.
    /// </summary>
    public interface IBudgetRepository : IFredRepository<BudgetEnt>
    {

        /// <summary>
        /// Retourne le budget associé à cet id. Si l'id n'existe pas la fonction retourne null.
        /// </summary>
        /// <param name="budgetId">Id du budget à trouver</param>
        /// <param name="avecT4etRessource">Charge les T4 et les ressources</param>
        /// <returns>Le budget s'il existe null sinon</returns>
        BudgetEnt GetBudget(int budgetId, bool avecT4etRessource = false);

        /// <summary>
        /// Récupère tous les budgets associé à ce CI. 
        /// </summary>
        /// <param name="ciId">Id du CI dont on doit récupérer les budgets</param>
        /// <returns>la liste des budgets associé à ce CI</returns>
        IEnumerable<BudgetEnt> GetBudgetForCi(int ciId);

        /// <summary>
        /// Retourne tous les budgets appartenant à l'utilisateur donné et les budgets partagés à tous les utilisateurs.
        /// Ainsi que le budget actuellement en application
        /// </summary>
        /// <param name="utilisateurId">Id de l'utilisateur dont on récupère les budgets</param>
        /// <param name="ciId">le ci dont on doit récupérer les budgets</param>
        /// <returns>Une liste de BudgetEnt</returns>
        IEnumerable<BudgetEnt> GetBudgetVisiblePourUserSurCi(int utilisateurId, int ciId);

        /// <summary>
        /// Renvoi le numéro de version le plus elevé de tous les budgets existants sur le ci donné
        /// e.g s'il existe des budgets en version 0.1,0.2,1.0 et 1.1 alors la fonction renverra 1.1 
        /// Si aucun budget n'existe sur le ci la fonction renvoi 0.0
        /// </summary>
        /// <param name="ciId">id du ci que l'on doit examiner</param>
        /// <returns>une chaine de charactères contenant le numéro de version, jamais null</returns>
        string GetCiMaxVersion(int ciId);

        /// <summary>
        /// Renvoi le numéro de version max existant  pour la version majeure de ce budget. 
        /// Par exemple si ce budget est présent en version 0.1, 0.2 et 0.3 alors cette fonction
        /// renverra 0.3 même si l'utilisateur qui demande la nouvelle version n'a pas de visibilité sur les versions 0.2 et 0.3
        /// et même si une version de ce budget existe en version 1.0 ou 2.0 ou une autre version majeure
        /// </summary>
        /// <param name="budgetId">Id du budget dont on doit récupérer le prochain numéro de version</param>
        /// <returns>une chaine de charactères contenant le numéro de version Majeur.Mineur</returns>
        string GetBudgetMaxVersion(int budgetId);

        /// <summary>
        /// Retourne le budget en application sur ce CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>le budget en application sur ce CI</returns>
        BudgetEnt GetBudgetEnApplication(int ciId);

        /// <summary>
        /// Retourne le budget en application sur ce CI avec la devise inclut
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>le budget en application sur ce CI</returns>
        BudgetEnt GetBudgetEnApplicationIncludeDevise(int ciId);

        /// <summary>
        /// Retourne la version majeure de budget en application sur ce CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>la version majeure de budget en application sur ce CI</returns>
        int GetVersionMajeureEnApplication(int ciId);

        /// <summary>
        /// Récupère un budget.
        /// </summary>
        /// <typeparam name="TBudget">Type de budget souhaité.</typeparam>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="selector">Selector permettant de construire un TBudget en fonction d'un BudgetEnt.</param>
        /// <returns>Le budget s'il existe, sinon null.</returns>
        TBudget GetBudget<TBudget>(int budgetId, Expression<Func<BudgetEnt, TBudget>> selector);

        /// <summary>
        /// Récupère un BudgetEnt.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <returns>Le budget s'il existe, sinon null.</returns>
        Task<BudgetEnt> GetTargetBudgetForCopyAsync(int budgetId);

        /// <summary>
        /// Retourne un budget visible pour un utilisateur.
        /// </summary>
        /// <typeparam name="TBudget">Type de budget souhaité.</typeparam>
        /// <param name="utilisateurId">Identifiant de l'utilisateur.</param>
        /// <param name="ciId">Identifiant du CI du budget.</param>
        /// <param name="deviseId">Identifiant de la devise du budget.</param>
        /// <param name="revision">Révision du budget concerné.</param>
        /// <param name="selector">Selector permettant de construire un TBudget en fonction d'un BudgetEnt.</param>
        /// <returns>Le budget s'il existe et est visible, sinon null.</returns>
        TBudget GetBudgetVisiblePourUserSurCi<TBudget>(int utilisateurId, int ciId, int deviseId, string revision, Expression<Func<BudgetEnt, TBudget>> selector);

        /// <summary>
        /// Retourne les révisions de budget d'un CI accessible à un utilisateur donné.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI concerné.</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur concerné.</param>
        /// <returns>Les révisions de budget du CI.</returns>
        List<BudgetRevisionLoadModel> GetBudgetRevisions(int ciId, int utilisateurId);

        /// <summary>
        /// Retourne les révisions de budget d'un CI accessible à un utilisateur donné pour la comparaison de budget.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI concerné.</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur concerné.</param>
        /// <returns>Les révisions de budget du CI.</returns>
        List<Entities.Budget.Dao.BudgetComparaison.Comparaison.BudgetRevisionDao> GetBudgetRevisionsPourBudgetComparaison(int ciId, int utilisateurId);

        /// <summary>
        /// Retourne les révisions des budgets comparés pour l'export Excel de la comparaison de budget.
        /// </summary>
        /// <param name="budgetId1">Identifiant du budget 1.</param>
        /// <param name="budgetId2">Identifiant du budget 2.</param>
        /// <returns>Les révisions des budgets comparés.</returns>
        BudgetsRevisionsDao GetBudgetRevisionsPourBudgetComparaisonExportExcel(int budgetId1, int budgetId2);
    }
}
