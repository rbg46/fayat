using Fred.Web.Shared.Models.Budget.BibliothequePrix;

namespace Fred.Business.Budget.BibliothequePrix
{
    /// <summary>
    /// Interface du gestionnaire des bibliothèques de prix.
    /// </summary>
    public interface IBudgetBibliothequePrixManager : IManager
    {
        /// <summary>
        /// Charge la bibliothèque des prix pour l'organisation indiquée.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation concernée.</param>
        /// <param name="deviseId">L'identifiant de la devise. Si null, c'est la devise par défaut qui sera utilisée ou sinon la première de la liste.</param>
        /// <returns>Le résultat du chargement.</returns>
        BibliothequePrixLoad.ResultModel Load(int organisationId, int? deviseId);

        /// <summary>
        /// Charge la bibliothèque des prix pour l'organisation indiquée pour la copie.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation concernée.</param>
        /// <param name="deviseId">L'identifiant de la devise concernée.</param>
        /// <returns>Le résultat du chargement.</returns>
        BibliothequePrixForCopyLoad.ResultModel LoadForCopy(int organisationId, int deviseId);


        /// <summary>
        /// Enregistre une bibliothèque des prix.
        /// </summary>
        /// <param name="model">Données à enregistrer.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        BibliothequePrixSave.ResultModel Save(BibliothequePrixSave.Model model);

        /// <summary>
        /// Cette fonction récupère les dernières valeurs de la bibliotheque des prix saisie sur un CI et applique 
        /// les valeurs pour les budgets listées dans le model
        /// </summary>
        /// <param name="model">Model décrivant le périmètre de la propagation, voir la documentation du type ApplyBibliothequePrixBudgetsBrouillonsModel</param>
        void ApplyNewBibliothequePrixToBudgetBrouillon(ApplyBibliothequePrixBudgetsBrouillonsModel model);

        /// <summary>
        /// Charge l'historique d'un item d'une bibliothèque des prix.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation.</param>
        /// <param name="deviseId">Identifiant de la devise.</param>
        /// <param name="ressourceId">Identifiant de la ressource.</param>
        /// <returns>Le résultat du chargement.</returns>
        BibliothequePrixItemHistoLoadModel.Model LoadItemHistorique(int organisationId, int deviseId, int ressourceId);

        /// <summary>
        /// Indique si une bibliothèque des prix existe.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation.</param>
        /// <param name="deviseId">Identifiant de la devise.</param>
        /// <returns>True si la bibliothèque des prix existe, sinon false.</returns>
        bool Exists(int organisationId, int deviseId);
    }
}
