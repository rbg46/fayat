using System;
using System.Collections.Generic;
using Fred.Entities.Budget;
using Fred.Web.Models.Budget.Liste;
using Fred.Web.Shared.Models.Budget;
using Fred.Web.Shared.Models.Budget.Liste;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Cette interface définit les fonctions qui doivent être implémenté pour permettre 
    /// la manipulation des budgets
    /// </summary>
    public interface IBudgetManager : IManager<BudgetEnt>
    {
        /// <summary>
        /// Retourne l'id du ci associé à ce budget
        /// </summary>
        /// <param name="budgetId">id du budget dont on veut le CI</param>
        /// <returns>un entier représentant l'id du ci</returns>
        int GetCiIdAssociatedToBudgetId(int budgetId);

        /// <summary>
        /// Retourne tous les budgets appartenant à l'utilisateur donné et les budgets partagés à tous les utilisateurs.
        /// Ainsi que le budget actuellement en application
        /// </summary>
        /// <param name="utilisateurId">Id de l'utilisateur dont on récupère les budgets</param>
        /// <param name="ciId">le ci dont on doit récupérer les budgets</param>
        /// <returns>Une liste de BudgetEnt</returns>
        IEnumerable<BudgetEnt> GetBudgetVisiblePourUserSurCi(int utilisateurId, int ciId);

        /// <summary>
        /// Renvoi le budget ayant l'id donné
        /// </summary>
        /// <param name="budgetId">Id du budget à trouver</param>
        /// <param name="avecT4etRessource">charge les T4 et les ressources des sous-détail</param>
        /// <returns>Le budget ayant l'id passé en paramètre</returns>
        BudgetEnt GetBudget(int budgetId, bool avecT4etRessource = false);

        /// <summary>
        /// Met à jour un budget
        /// </summary>
        /// <param name="budget">Le budget</param>
        /// <returns>le budget à jour</returns>
        BudgetEnt Update(BudgetEnt budget);

        /// <summary>
        /// Insère dans la base de donnée le budget passé en paramètre
        /// </summary>
        /// <param name="budget">Budget à insérer dans la base</param>
        /// <returns>Le budget inséré</returns>
        BudgetEnt Create(BudgetEnt budget);

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
        /// Retourne la date de mise en application du budget en application sur ce CI
        /// </summary>
        /// <param name="ciId">Id du CI dont on va récupérer la date de mise en application du budget en application</param>
        /// <returns>Une date si un budget en application existe sur ce CI, null sinon</returns>
        DateTime? GetDateMiseEnApplicationBudgetSurCi(int ciId);

        /// <summary>
        /// Renvoi le numéro de version le plus elevé de tous les budgets existants sur le ci donné
        /// e.g s'il existe des budgets en version 0.1,0.2,1.0 et 1.1 alors la fonction renverra 1.1 
        /// Si aucun budget n'existe sur le ci la fonction renvoi 0.0
        /// </summary>
        /// <param name="ciId">id du ci que l'on doit examiner</param>
        /// <returns>une chaine de charactères contenant le numéro de version, jamais null</returns>
        string GetCiMaxVersion(int ciId);

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
        /// Retourne l'id du budget en application. Si aucun budget n'existe pour le CI donné alors la fonction renvoie null
        /// </summary>
        /// <param name="ciId">Identifiant du CI dont on récupère le budget en application</param>
        /// <returns>un entier contenant l'id du budget ou null si aucun budget n'est en application sur ce CI</returns>
        int GetIdBudgetEnApplication(int ciId);

        /// <summary>
        /// Retourne l'id du budget en application. Si aucun budget n'existe pour le CI donné alors la fonction renvoie null
        /// </summary>
        /// <param name="ciId">Identifiant du CI dont on récupère le budget en application</param>
        /// <returns>list entier contenant l'id du budget ou null si aucun budget n'est en application sur ce CI</returns>
        List<int> GetListIdBudgetEnApplication(int ciId);

        /// <summary>
        /// Retourne la version majeure de budget en application sur ce CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>la version majeure de budget en application sur ce CI</returns>
        int GetVersionMajeureEnApplication(int ciId);

        /// <summary>
        /// Partage le budget ayant l'id donné à tous les utilisateurs ayant les droits sur le CI auquel le budget est rattaché
        /// Si le budget est déjà partagé alors la fonction le privatise et il ne devient visible que pour son créateur
        /// </summary>
        /// <param name="budgetId">budget à partager</param>
        /// <returns>Nouvelle etat du partage : True : budget partagé, False : budget privé</returns>
        bool PartageOuPrivatiseBudget(int budgetId);

        /// <summary>
        /// Créé un nouveau budget vide ne contenant qu'un workflow avec une seule entrée avec pour auteur l'utilisateur connecté
        /// </summary>
        /// <param name="ciId">ci dans lequel on va créé le budget</param>
        /// <param name="utilisateurConnecteId">Le futur auteur du nouveau budget</param>
        /// <returns>retourne le budget nouvellement créé</returns>
        BudgetEnt CreateEmptyBudgetSurCi(int ciId, int utilisateurConnecteId);

        /// <summary>
        /// Supprime le budget ayant l'id donné
        /// </summary>
        /// <param name="budgetId">id du budget à supprimer</param>
        void DeleteById(int budgetId);


        /// <summary>
        /// Retourne la periode de début du budget
        /// </summary>
        /// <param name="budgetId">id du budget dont on veut la période</param>
        /// <returns>la période au format YYYYMM si elle existe, null sinon</returns>
        int? GetPeriodeDebutBudget(int budgetId);


        /// <summary>
        /// Supprime le budget ayant l'id passé en paramètre
        /// </summary>
        /// <param name="budgetId">id du budget a supprimer</param>
        /// <returns>Le model de suppression du budget</returns>
        BudgetSuppressionSuccessModel SupprimeBudget(int budgetId);

        /// <summary>
        /// Sauvegarde les changements effectuées à un budget dans la liste des budgets 
        /// Sont supportées :
        /// Recettes 
        /// Commentaire du workflow le plus récent
        /// </summary>
        /// <param name="budgetListeModel">budget à modifier</param>
        /// <returns>le nouveau budget</returns>
        BudgetEnt SaveBudgetChangeInListView(ListeBudgetModel budgetListeModel);

        /// <summary>
        /// Modifie l'état d'un budget et genère une entrée dans le workflow
        /// </summary>
        /// <param name="budget">Le budget</param>
        /// <param name="etatCibleId">identifiant de l'état cible</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        /// <param name="commentaire">le commentaire</param>
        void BudgetChangeEtat(BudgetEnt budget, int etatCibleId, int utilisateurId, string commentaire);

        /// <summary>
        /// Restaure le budget ayant l'id passé en paramètre
        /// </summary>
        /// <param name="budgetId">l'id du budget à restaurer</param>
        /// <returns>True si le budget a pu être restauré, false sinon</returns>
        bool RestaurerBudget(int budgetId);

        /// <summary>
        /// Retourne les révisions de budget d'un CI accessible à l'utilisateur connecté.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI concerné.</param>
        /// <returns>Les révisions de budget du CI.</returns>
        IEnumerable<BudgetRevisionLoadModel> GetBudgetRevisions(int ciId);

        /// <summary>
        /// Met à jour la date de suppression de la notification des nouvelles tâches
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        void UpdateDateDeleteNotificationNewTask(int budgetId);

        /// <summary>
        /// Retourne tous les budgets brouillons créés à partir du budget actuellement en application sur le CI.
        /// Cela signifie que la version majeures de ces budgets brouillon est identique a celle du budget actuellement en application
        /// </summary>
        /// <param name="ciId">Identifiant du CI dont on veut récupérer les budgets</param>
        /// <param name="deviseId">Identifiant de la devise des budgets à récupérer</param>
        /// <returns>Une liste potentiellement vide, jamais null</returns>
        IEnumerable<BudgetVersionAuteurModel> GetBudgetBrouillonDuBudgetEnApplication(int ciId, int deviseId);

        /// <summary>
        /// Retourne tous les budgets brouillons d'un CI pour une devise.
        /// </summary>
        /// <param name="ciId">Identifiant du CI.</param>
        /// <param name="deviseId">Identifiant de la devise.</param>
        /// <returns>Les budgets brouillons concernés.</returns>
        IEnumerable<BudgetVersionAuteurModel> GetBudgetsBrouillons(int ciId, int deviseId);
    }
}
