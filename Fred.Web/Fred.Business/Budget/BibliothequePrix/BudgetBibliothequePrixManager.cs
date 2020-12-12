using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Web.Shared.Models.Budget.BibliothequePrix;

namespace Fred.Business.Budget.BibliothequePrix
{
    /// <summary>
    /// Gestionnaire des bibliothèques de prix.
    /// </summary>
    public class BudgetBibliothequePrixManager : IBudgetBibliothequePrixManager
    {
        private readonly IUnitOfWork uow;
        private readonly IBudgetBibliothequePrixRepository budgetBibliothequePrixRepository;
        private readonly IBudgetBibliothequePrixItemRepository budgetBibliothequePrixItemRepository;
        private readonly IBudgetT4Repository budgetT4Repository;
        private readonly IBudgetSousDetailRepository budgetSousDetailRepository;
        private readonly IOrganisationRepository organisationRepository;
        private readonly ISocieteDeviseRepository societeDeviseRepository;
        private readonly ICIRepository ciRepository;
        private readonly IRessourceRepository ressourceRepository;
        private readonly IUniteRepository uniteRepository;
        private readonly IUniteSocieteRepository uniteSocieteRepository;

        public BudgetBibliothequePrixManager(
            IUnitOfWork uow,
            IBudgetBibliothequePrixRepository budgetBibliothequePrixRepository,
            IBudgetBibliothequePrixItemRepository budgetBibliothequePrixItemRepository,
            IBudgetT4Repository budgetT4Repository,
            IBudgetSousDetailRepository budgetSousDetailRepository,
            IOrganisationRepository organisationRepository,
            ISocieteDeviseRepository societeDeviseRepository,
            ICIRepository ciRepository,
            IRessourceRepository ressourceRepository,
            IUniteRepository uniteRepository,
            IUniteSocieteRepository uniteSocieteRepository)
        {
            this.uow = uow;
            this.budgetBibliothequePrixRepository = budgetBibliothequePrixRepository;
            this.budgetBibliothequePrixItemRepository = budgetBibliothequePrixItemRepository;
            this.budgetT4Repository = budgetT4Repository;
            this.budgetSousDetailRepository = budgetSousDetailRepository;
            this.organisationRepository = organisationRepository;
            this.societeDeviseRepository = societeDeviseRepository;
            this.ciRepository = ciRepository;
            this.ressourceRepository = ressourceRepository;
            this.uniteRepository = uniteRepository;
            this.uniteSocieteRepository = uniteSocieteRepository;
        }

        /// <summary>
        /// Charge la bibliothèque des prix pour l'organisation indiquée.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation concernée.</param>
        /// <param name="deviseId">L'identifiant de la devise. Si null, c'est la devise par défaut qui sera utilisée ou sinon la première de la liste.</param>
        /// <returns>Le résultat du chargement.</returns>
        public BibliothequePrixLoad.ResultModel Load(int organisationId, int? deviseId)
        {
            return new BudgetBibliothequePrixLoader(
                organisationId,
                deviseId,
                organisationRepository,
                societeDeviseRepository,
                ciRepository,
                ressourceRepository,
                uniteRepository,
                uniteSocieteRepository,
                budgetBibliothequePrixRepository).Load();
        }

        /// <summary>
        /// Charge la bibliothèque des prix pour l'organisation indiquée pour la copie.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation concernée.</param>
        /// <param name="deviseId">L'identifiant de la devise concernée.</param>
        /// <returns>Le résultat du chargement.</returns>
        public BibliothequePrixForCopyLoad.ResultModel LoadForCopy(int organisationId, int deviseId)
        {
            return new BudgetBibliothequePrixForCopyLoader(
                organisationId,
                deviseId,
                organisationRepository,
                uniteRepository,
                uniteSocieteRepository,
                budgetBibliothequePrixRepository).Load();
        }

        /// <summary>
        /// Enregistre une bibliothèque des prix.
        /// </summary>
        /// <param name="model">Données à enregistrer.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public BibliothequePrixSave.ResultModel Save(BibliothequePrixSave.Model model)
        {
            return new BudgetBibliothequePrixSaver(model, budgetBibliothequePrixRepository).Save();
        }

        /// <summary>
        /// Cette fonction récupère les dernières valeurs de la bibliotheque des prix saisie sur un CI et applique 
        /// les valeurs pour les budgets listées dans le model
        /// </summary>
        /// <param name="model">Model décrivant le périmètre de la propagation, voir la documentation du type ApplyBibliothequePrixBudgetsBrouillonsModel</param>
        public void ApplyNewBibliothequePrixToBudgetBrouillon(ApplyBibliothequePrixBudgetsBrouillonsModel model)
        {
            var bpSurCi = budgetBibliothequePrixItemRepository
                .Get()
                .Where(item => item.BudgetBibliothequePrix.OrganisationId == model.CiOrganisationId && item.BudgetBibliothequePrix.DeviseId == model.DeviseId)
                .Select(item => new
                {
                    item.Prix,
                    item.UniteId,
                    item.RessourceId,
                    ValueBefore = item.ItemValuesHisto.OrderByDescending(v => v.DateInsertion).FirstOrDefault()
                });

            var entitiesToUpdate = budgetSousDetailRepository.Get()
                .Where(sd => model.BudgetIdAEnregistrer.Contains(sd.BudgetT4.BudgetId)
                    && bpSurCi.Any(bp => bp.RessourceId == sd.RessourceId))
                .Select(sd => new
                {
                    SousDetail = sd,
                    T4 = sd.BudgetT4
                })
                .ToList();

            foreach (var sd in entitiesToUpdate.Select(e => e.SousDetail))
            {
                var bpValues = bpSurCi.Single(bp => bp.RessourceId == sd.RessourceId);


                //Une ligne en exception est une ligne de sous détail dont la valeur de prix unitaire est différente de la valeur spéciifée dans la bibliiotheque des prix
                //Donc avant de mettre à jour un sous détail, on vérifie que l'utilisateur demande la modification des lignes en exception ou alors que la ligne n'est pas une exception
                //C'est à dire que la valeur dans le sous détail est différente de la valeur anciennement saisie dans la bibliotheque des prix. 
                //(La valeur dans le sous détail ne peut pas correspondre à la valeur actuelle dans la bibliotheque des prix puisque cette fonction est designée pour appliquer
                //une nouvelle bibliotheque des prix sur des budgets brouillons
                //On ne vérifie pas l'unité car l'unité ne peut pas être modifiée dans le sous détail
                if (model.UpdateValeursSurLignesEnException || sd.PU == bpValues.ValueBefore?.Prix)
                {
                    sd.PU = bpValues.Prix;
                    sd.Montant = sd.PU * sd.Quantite;
                    sd.UniteId = bpValues.UniteId;
                }


                budgetSousDetailRepository.Update(sd);
            }

            foreach (var t4 in entitiesToUpdate.Select(e => e.T4))
            {
                t4.MontantT4 = t4.BudgetSousDetails.Sum(sd => sd.PU * sd.Quantite);
                t4.PU = (t4.QuantiteARealiser ?? 0) == 0 ? null : t4.MontantT4 / t4.QuantiteARealiser;

                budgetT4Repository.Update(t4);
            }

            uow.Save();
        }

        /// <summary>
        /// Charge l'historique d'un item d'une bibliothèque des prix.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation.</param>
        /// <param name="deviseId">Identifiant de la devise.</param>
        /// <param name="ressourceId">Identifiant de la ressource.</param>
        /// <returns>Le résultat du chargement.</returns>
        public BibliothequePrixItemHistoLoadModel.Model LoadItemHistorique(int organisationId, int deviseId, int ressourceId)
        {
            var histo = budgetBibliothequePrixRepository.Get()
                .Where(bp => bp.OrganisationId == organisationId && bp.DeviseId == deviseId)?
                .SelectMany(bp => bp.Items)
                .Where(item => item.RessourceId == ressourceId)
                .SelectMany(item => item.ItemValuesHisto)
                .Select(BibliothequePrixItemHistoLoadModel.ItemModel.Selector)
                .OrderByDescending(itemHisto => itemHisto.DateInsertion)
                .ToList();

            if (histo != null)
            {
                // NPI : code temporaire dû à l'impossibilité de débugger pour le moment sur ma machine...
                foreach (var item in histo)
                {
                    item.DateInsertion = DateTime.SpecifyKind(item.DateInsertion, DateTimeKind.Utc);
                }
                return new BibliothequePrixItemHistoLoadModel.Model() { Histo = histo };
            }
            return new BibliothequePrixItemHistoLoadModel.Model() { Histo = new List<BibliothequePrixItemHistoLoadModel.ItemModel>() };
        }

        /// <summary>
        /// Indique si une bibliothèque des prix existe.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation.</param>
        /// <param name="deviseId">Identifiant de la devise.</param>
        /// <returns>True si la bibliothèque des prix existe, sinon false.</returns>
        public bool Exists(int organisationId, int deviseId)
        {
            return budgetBibliothequePrixRepository.Get()
                .Where(b => b.OrganisationId == organisationId && b.DeviseId == deviseId)
                .Any();
        }
    }
}
