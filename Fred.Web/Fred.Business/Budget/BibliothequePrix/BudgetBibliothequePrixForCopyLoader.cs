using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Web.Shared.App_LocalResources;
using static Fred.Web.Shared.Models.Budget.BibliothequePrix.BibliothequePrixForCopyLoad;

namespace Fred.Business.Budget.BibliothequePrix
{
    /// <summary>
    /// Permet de charger une bibliothèque des prix pour la copie.
    /// </summary>
    public class BudgetBibliothequePrixForCopyLoader : BudgetBibliothequePrixLoaderBase
    {
        #region Membres

        private readonly IOrganisationRepository organisationRepository;

        private readonly int organisationId;
        private readonly int deviseId;
        private ResultModel ret;
        private int? societeId;

        #endregion
        #region Constructeur

        public BudgetBibliothequePrixForCopyLoader(
            int organisationId,
            int deviseId,
            IOrganisationRepository organisationRepository,
            IUniteRepository uniteRepository,
            IUniteSocieteRepository uniteSocieteRepository,
            IBudgetBibliothequePrixRepository budgetBibliothequePrixRepository) : base(uniteRepository, uniteSocieteRepository, budgetBibliothequePrixRepository)
        {
            this.organisationId = organisationId;
            this.deviseId = deviseId;
            this.organisationRepository = organisationRepository;
        }

        #endregion
        #region Chargement

        /// <summary>
        /// Charge la bibliothèque des prix.
        /// </summary>
        /// <returns>Le résultat du chargement.</returns>
        public ResultModel Load()
        {
            ret = new ResultModel();

            if (CheckOrganisationExists()
             && LoadSociete()
             && Exists()
             && LoadUnites())
            {
                LoadItems();
            }

            ret.OrganisationId = organisationId;
            ret.DeviseId = deviseId;
            return ret;
        }

        /// <summary>
        /// Vérifie que l'organisation existe.
        /// </summary>
        /// <returns>True si l'organisation existe, sinon false.</returns>
        private bool CheckOrganisationExists()
        {
            var exists = organisationRepository.Get()
                .Where(o => o.OrganisationId == organisationId)
                .Count() == 1;

            if (!exists)
            {
                ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_OrganisationNexistePas;
            }

            return exists;
        }

        /// <summary>
        /// Charge la société.
        /// </summary>
        /// <returns>True en cas de succès, sinon false.</returns>
        private bool LoadSociete()
        {
            societeId = organisationRepository.GetSocieteId(organisationId);

            if (!societeId.HasValue)
            {
                ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_SocieteNexistePas;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Indique si une bibliothèque des prix existe.
        /// </summary>
        /// <returns>True si la bibliothèque des prix existe, sinon false.</returns>
        private bool Exists()
        {
            if (!Exists(organisationId, deviseId))
            {
                ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_Inexistante;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Charge les unités.
        /// </summary>
        /// <returns>True en cas de succès, sinon false.</returns>
        private bool LoadUnites()
        {
            ret.Unites = GetUnites(societeId.Value, UniteModel.Selector);

            if (ret.Unites.Count == 0)
            {
                ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_SocieteAucuneUnite;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Charge les éléments de la bibliothèque des prix.
        /// </summary>
        private void LoadItems()
        {
            // Note : si ret.DeviseId est null c'est que cette fonction n'aurait pas dûe être appelée
            ret.Items = GetItems(organisationId, deviseId, ItemModel.Selector);

            foreach (var item in ret.Items)
            {
                if (item.UniteId.HasValue && !ret.Unites.Any(u => u.UniteId == item.UniteId))
                {
                    var unite = GetUnite(item.UniteId.Value, UniteModel.Selector);
                    ret.Unites.Add(unite);
                }
            }
        }

        #endregion
    }
}
