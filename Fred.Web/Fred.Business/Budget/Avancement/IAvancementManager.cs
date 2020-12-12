using System.Collections.Generic;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Fred.Web.Shared.Models.Budget.Avancement;

namespace Fred.Business.Budget.Avancement
{
    /// <summary>
    /// Interface des avancements.
    /// </summary>
    public interface IAvancementManager : IManager<AvancementEnt>
    {
        /// <summary>
        /// Retourne le modèle d'avancement
        /// </summary>
        /// <param name="sousDetailId">Identifiant du sous-détail</param>
        /// <param name="periode">Periode</param>
        /// <returns>Le modèle d'avancement</returns>
        AvancementEnt GetPreviousAvancement(int sousDetailId, int periode);

        /// <summary>
        /// Retourne le modèle d'avancement supérieure ou égale à la période
        /// </summary>
        /// <param name="sousDetailId">Identifiant du sous-détail</param>
        /// <param name="periode">Periode</param>
        /// <returns>Le modèle d'avancement</returns>
        AvancementEnt GetAvancement(int sousDetailId, int periode);

        /// <summary>
        /// Retourne les avancements d'un budget sur une période donnée.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget.</param>
        /// <param name="periode">La période.</param>
        /// <returns>Les avancements du budget sur la période.</returns>
        List<AvancementEnt> GetAvancements(int budgetId, int periode);

        List<AvancementEnt> GetAvancements(IEnumerable<int> budgetIds, int periode);

        /// <summary>
        /// Retourne l'avancement 
        /// </summary>
        /// <param name="budgetId">id du Budget dont on veut l'avancement</param>
        /// <param name="periode">periode pour laquelle on veut les avancements</param>
        /// <returns>Une IEnumerable, jamais null</returns>
        IEnumerable<AvancementEnt> GetAllAvancementForBudgetAndPeriode(int budgetId, int periode);

        /// <summary>
        /// Retourne les avancement cumulés du budgets
        /// </summary>
        /// <param name="budgetId">id du Budget dont on veut l'avancement</param>
        /// <param name="periode">periode pour laquelle on veut les avancements</param>
        /// <returns>Une IEnumerable, jamais null</returns>
        IEnumerable<AvancementEnt> GetAllAvancementCumuleForBudgetAndPeriode(int budgetId, int periode);

        /// <summary>
        /// Retourne tous les avancements saises sur la période la plus récente 
        /// </summary>
        /// <param name="ciId">Id du ci dont on va connaitre le budget en application et donc les avancements à récupérer</param>
        /// <returns>Une liste potentiellement vide, jamais null</returns>
        IEnumerable<AvancementEnt> GetAllAvancementsDePeriodeLaPlusRecente(int ciId);

        /// <summary>
        /// Insère un avancement dans le contexte
        /// </summary>
        /// <param name="avancement">Identifiant du sous-détail</param>
        void InsertAvancement(AvancementEnt avancement);

        /// <summary>
        /// Mets à jour un avancement dans le contexte
        /// </summary>
        /// <param name="avancement">Identifiant du sous-détail</param>
        void Update(AvancementEnt avancement);

        /// <summary>
        /// Evalue l'existence d'un avancement sur le sous-détail pour une période donné, et met à jour ce dernier ou insère un nouvel avancement 
        /// </summary>
        /// <param name="sd">Sous-détail</param>
        /// <param name="tache4SaveModel">Modèle de la tache 4</param>
        /// <param name="periode">Periode de référence</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <param name="typeAvancement">Type d'avancement</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        void AddOrUpdateAvancement(BudgetSousDetailEnt sd, AvancementTache4SaveModel tache4SaveModel, int periode, int ciId, int deviseId, int typeAvancement, int userId);


        /// <summary>
        /// Update la liste des taches d'avancements pour un budget et une période
        /// </summary>
        /// <param name="budgetId">Identifiant de budget</param>
        /// <param name="periode">Identifiant de période</param>
        /// <param name="avancementTaches">liste des taches</param>
        void UpdateListeTacheAvancement(int budgetId, int periode, IEnumerable<AvancementTacheSaveModel> avancementTaches);

        /// <summary>
        /// Créé des Avancements à 100% pour les T4 Rev
        /// </summary>
        void CreationAvancementT4Rev(BudgetSousDetailEnt sd, int ciId, int deviseId, int periode, int saveAvancementEtatId);

        /// <summary>
        /// Retourne Vrai si la période possède un avancement validé
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <param name="periode">Période</param>
        /// <returns>Vrai si la période possède un avancement validé</returns>
        bool IsAvancementValide(int ciId, int budgetId, int periode);

        /// <summary>
        /// Retourne le modèle d'avancement pour le dernier avancement validé
        /// </summary>
        /// <param name="sousDetailId">Identifiant du sous-détail</param>
        /// <returns>Le modèle d'avancement</returns>
        AvancementEnt GetLastAvancementValide(int sousDetailId);

        /// <summary>
        /// Retourne le modèle d'avancement pour le dernier avancement saisie jusqu'a la période donnée.
        /// Saisie : c'est à dire les avancements aussi bien enregistrés que validés
        /// </summary>
        /// <example>
        /// Imaginons un avancement enregistré pour Octobre 2018 et aucun avancement pour Novembre
        /// Appeler cette fonction avec 201811 comme paramètre retournera l'avancement d'octobre
        /// </example>
        /// <param name="sousDetailId">Id du sous détail dont on veut l'avancement</param>
        /// <param name="periode">période délimitant l'avancement à récupérer</param>
        /// <returns>Un avancement Ent</returns>
        AvancementEnt GetLastAvancementAvantPeriode(int sousDetailId, int periode);

        /// <summary>
        /// Retourne les derniers avancements d'un budget jusqu'à la période donnée.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget.</param>
        /// <param name="periode">La période délimitant les avancements à récupérer.</param>
        /// <returns>Les avancements.</returns>
        List<AvancementEnt> GetLastAvancementAvantPeriodes(int budgetId, int periode);

        /// <summary>
        /// Retourne une liste d'avancement supérieur à la période donnée et égale à l'état d'avancement donné
        /// </summary>
        /// <param name="ciid">Identifiant du ci</param>
        /// <param name="periode">la période</param>
        /// <param name="etatAvancement">état avancement recherché</param>
        /// <param name="budgetSousDetailId">identifiant du sous détail du budget</param>
        /// <returns>entité avancement</returns>
        AvancementEnt GetStatusAvancementAfterPeriode(int ciid, int periode, string etatAvancement, int budgetSousDetailId);


        /// <summary>
        /// Retourne une liste de periode des avancements antérieur à la période donnée et égale à l'état d'avancement donné
        /// </summary>
        /// <param name="ciid">Identifiant du ci</param>
        /// <param name="periode">la période</param>
        /// <param name="etatAvancement">état avancement recherché</param>
        /// <param name="listBudgetSousDetailId">Liste d'identifiant des sous détails du budget</param>
        /// <returns>entité avancement</returns>
        List<int> GetListPeriodeAvancementNotValidBeforePeriode(int ciid, int periode, string etatAvancement, List<int> listBudgetSousDetailId);
    }
}
