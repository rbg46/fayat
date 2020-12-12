using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.ReferentielFixe;
using Fred.Web.Shared.Models.Budget;
using Fred.Web.Shared.Models.Budget.Avancement;
using Fred.Web.Shared.Models.Budget.BibliothequePrix.SousDetail;
using Fred.Web.Shared.Models.Budget.Details;
using Fred.Web.Shared.Models.Budget.Liste;
using Fred.Web.Shared.Models.Budget.Recette;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Interface du budget
    /// </summary>
    public interface IBudgetMainManager : IManager
    {
        /// <summary>
        /// Renvoi un export excel en utilisant la classe BudgetDetailExportExcelFeature
        /// </summary>
        /// <param name="model">BudgetDetailsExportExcelLoadModel</param>
        /// <returns>les données de l'excel sous forme de tableau d'octets</returns>
        byte[] GetBudgetDetailExportExcel(BudgetDetailsExportExcelLoadModel model);

        /// <summary>
        /// Retourne le détail d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <returns>Le détail du budget.</returns>
        BudgetDetailLoad.Model GetDetail(int budgetId);

        /// <summary>
        /// Enregistre le détail d'un budget.
        /// </summary>
        /// <param name="model">Données à enregistrer.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        BudgetDetailSave.ResultModel SaveDetail(BudgetDetailSave.Model model);

        /// <summary>
        /// Valide le détail d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="commentaire">Commentaire</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        string ValidateDetailBudget(int budgetId, string commentaire = "");

        /// <summary>
        /// Passe l'état d'un budget 
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <param name="commentaire">Commentaire</param>
        /// <returns>vie</returns>
        string RetourBrouillon(int budgetId, string commentaire = "");

        /// <summary>
        /// Propose un nouveau code pour une tâche enfant en fonction de sa tâche parente.
        /// </summary>
        /// <param name="tacheParenteId">Identifiant de la tâche parente.</param>
        /// <returns>Le code.</returns>
        string GetNextTacheCode(int tacheParenteId);

        /// <summary>
        /// Propose des nouveaux codes pour une tâche enfant en fonction de sa tâche parente.
        /// </summary>
        /// <param name="tacheParenteId">Identifiant de la tâche parente.</param>
        /// <param name="count">Le nombre de code à proposer</param>
        /// <returns>Les codes.</returns>
        List<string> GetNextTacheCodes(int tacheParenteId, int count);

        /// <summary>
        /// Crée une tâche de niveau 4.
        /// </summary>
        /// <param name="model">Modèle de création d'une tâche de niveau 4.</param>
        /// <returns>Le modèle du résultat de la création.</returns>
        ManageT4Create.ResultModel CreateTache4(ManageT4Create.Model model);

        /// <summary>
        /// Créé plusieurs tâches 4.
        /// </summary>
        /// <param name="model">Modèle de création de plusieurs tâches 4..</param>
        /// <returns>Le modèle du résultat de la création.</returns>
        CreateTaches4.ResultModel CreateTaches4(CreateTaches4.Model model);

        /// <summary>
        /// Change une tâche de niveau 4.
        /// </summary>
        /// <param name="model">Modèle de changement d'une tâche de niveau 4.</param>
        /// <returns>Le modèle du résultat du changement.</returns>
        ManageT4Change.ResultModel ChangeTache4(ManageT4Change.Model model);

        /// <summary>
        /// Supprime une tâche de niveau 4.
        /// </summary>
        /// <param name="model">Modèle de suppression d'une tâche de niveau 4.</param>
        /// <returns>Le modèle du résultat de la suppression.</returns>
        ManageT4Delete.ResultModel DeleteTache4(ManageT4Delete.Model model);

        /// <summary>
        /// Retourne le sous-détail d'une tâche de niveau 4 pour un budget.
        /// </summary>
        /// <param name="ciId">Identifiant du CI.</param>
        /// <param name="budgetT4Id">Identifiant de la tâche de niveau 4.</param>
        /// <returns>Le sous-détail de la tâche de niveau 4 pour le budget.</returns>
        BudgetSousDetailLoad.Model GetSousDetail(int ciId, int budgetT4Id);

        /// <summary>
        /// Permet de récupérer la liste des chapitres, sous-chapitres, ressources, référentiel étendus et paramétrages associés
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="deviseId">Devise que l'on souhaite avoir pour les PU des ressources retournées</param>
        /// <param name="filter">Filtre sur les libellés</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Nombre d'éléments par page</param>
        /// <returns>Liste de chapitres</returns>
        IEnumerable<BudgetSousDetailChapitreBibliothequePrixModel> GetChapitres(int ciId, int deviseId, string filter, int page, int pageSize);

        /// <summary>
        /// Enregistre le sous-détail d'une tâche T4 d'un budget.
        /// </summary>
        /// <param name="model">Données à enregistrer.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        BudgetSousDetailSave.ResultModel SaveSousDetail(BudgetSousDetailSave.Model model);

        /// <summary>
        /// Retourne le modèle permettant de gérer l'avancement
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="periode">Période YYYYMM</param>
        /// <returns>Le modèle permettant de gérer l'avancement</returns>
        AvancementLoadModel GetAvancement(int ciId, int periode);

        /// <summary>
        /// Retourne l'avancement d'une recette pour un CI et une période
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="periode">Période YYYYMM</param>
        /// <returns>L'avancement d'une recette pour un CI et une période</returns>
        AvancementRecetteLoadModel GetAvancementRecette(int ciId, int periode);


        /// <summary>
        /// Retourne une lists l'avancement d'une recette pour un CI et une période
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="fromperiode">Début Période YYYYMM</param>
        /// <param name="toperiode">Fin Période YYYYMM</param>
        /// <returns>une liste L'avancement d'une recette pour un CI et une période</returns>
        List<AvancementRecetteLoadModel> GetAvancementRecetteList(int ciId, int fromperiode, int toperiode);

        List<PeriodAvancementRecetteLoadModel> GetAvancementRecettesForPeriode(int ciId, int fromPeriode, int toPeriode);

        /// <summary>
        /// Enregistre les avancements pour la période définie
        /// </summary>
        /// <param name="model">Model de l'écran avancement</param>
        void SaveAvancement(AvancementSaveModel model);

        /// <summary>
        /// Génère une nouvelle version du budget (brouillon) tenant compte de l'avancement 
        /// </summary>
        /// <param name="budgetSourceId">Identifiant budget à recaler</param>
        /// <param name="userId">Identifiant de l'utilisateur demandeur de l'action de recalage</param>
        /// <param name="periodeFin">Periode de fin</param>
        /// <returns>l'id du budget révisé</returns>
        Task<int> RecalageBudgetaireAsync(int budgetSourceId, int userId, int periodeFin);

        /// <summary>
        /// Valide les avancements pour la période définie
        /// </summary>
        /// <param name="model">Model de l'écran avancement</param>
        /// <param name="etatAvancement">etat d'avancement du model</param>
        void ValidateAvancementModel(AvancementSaveModel model, string etatAvancement);

        /// <summary>
        /// Retour à l'état enregistré pour les avancements pour la période définie
        /// </summary>
        /// <param name="model">Model de l'écran avancement</param>
        void RetourBrouillonAvancement(AvancementSaveModel model);

        /// <summary>
        /// Permet de créer un détail de ressource.
        /// </summary>
        /// <param name="ressource">La ressource à créer.</param>
        /// <returns>La ressource créée.</returns>
        RessourceEnt CreateRessource(RessourceEnt ressource);

        /// <summary>
        /// Permet de mettre à jour un détail de ressource.
        /// </summary>
        /// <param name="ressource">La ressource concernée.</param>
        /// <returns>La ressource mise-à-jour.</returns>
        RessourceEnt UpdateRessource(RessourceEnt ressource);

        /// <summary>
        /// Permet de créer un détail de ressource.
        /// </summary>
        /// <param name="code">Code de la ressource.</param>
        /// <param name="groupeCode">Le code du groupe.</param>
        /// <returns>Liste de ressources.</returns>
        List<RessourceEnt> GetRessourceByCodeAndGroupeCode(string code, string groupeCode);

        /// <summary>
        /// Retourne un ListPeriodeRecalageModel
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <returns>un ListPeriodeClotureModel</returns>
        ListPeriodeRecalageModel LoadPeriodeRecalage(int ciId);

        /// <summary>
        /// Retourne le message de confirmation pour la mise en applciation d'une version de budget
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="version">numéro de la revision</param>
        /// <returns>Le message de validation pour la mise en applciation du budget</returns>
        string GetMessageMiseEnApllication(int ciId, string version);

        /// <summary>
        /// Enregistre les données d'un avancement recette
        /// </summary>
        /// <param name="model">Modèle de sauvegarde pour l'avancement recette</param>
        /// <returns>L'identifiant de l'avancement recette</returns>
        int SaveAvancementRecette(AvancementRecetteSaveModel model);

        /// <summary>
        /// Copie des sous-détails dans des budgets T4.
        /// </summary>
        /// <param name="model">Données à copier.</param>
        /// <returns>Le résultat de la copie.</returns>
        BudgetSousDetailCopier.ResultModel CopySousDetails(BudgetSousDetailCopier.Model model);

        /// <summary>
        /// Retourne les tâches de niveau 4 qui ne sont pas utilisées dans une révision de budget.
        /// </summary>
        /// <param name="model">Model de chargement des tâches 4 non utilisées dans une révision de budget.</param>
        /// <returns>Le résultat du chargement.</returns>
        Tache4Inutilisees.ResultModel GetTache4Inutilisees(Tache4Inutilisees.Model model);      
    }
}
