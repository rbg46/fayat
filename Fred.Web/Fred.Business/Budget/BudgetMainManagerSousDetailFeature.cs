using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.ReferentielEtendu;
using Fred.Business.ReferentielFixe;
using Fred.Business.Societe;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Budget;
using Fred.Web.Shared.Models.Budget.BibliothequePrix.SousDetail;
using Fred.Web.Shared.Models.Budget.Details.SousDetail;
using Fred.Web.Shared.Models.RessourceRecommandee;
using MoreLinq;
using static Fred.Entities.Constantes;

namespace Fred.Business.Budget
{
    public class BudgetMainManagerSousDetailFeature : ManagersAccess
    {
        private readonly ICIRepository ciRepository;
        private readonly IRessourceRepository ressourceRepository;
        private readonly IBudgetBibliothequePrixItemRepository bibliothequePrixItemRepository;
        private readonly IBudgetT4Repository budgetT4Repository;
        private readonly IBudgetSousDetailManager budgetSousDetailManager;
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly IBudgetManager budgetManager;
        private readonly IReferentielEtenduManager referentielEtenduManager;
        private readonly IRessourceValidator ressourceValidator;
        private readonly IUnitOfWork uow;
        private readonly ISocieteManager societeManager;

        public BudgetMainManagerSousDetailFeature(
            IUnitOfWork uow,
            IRessourceRepository ressourceRepository,
            IBudgetBibliothequePrixItemRepository bibliothequePrixItemRepository,
            ICIRepository ciRepository,
            IBudgetT4Repository budgetT4Repository,
            IBudgetSousDetailManager budgetSousDetailManager,
            IBudgetT4Manager budgetT4Manager,
            IBudgetManager budgetManager,
            IReferentielEtenduManager referentielEtenduManager,
            IRessourceValidator ressourceValidator,
            ISocieteManager societeManager)
        {
            this.uow = uow;
            this.ressourceRepository = ressourceRepository;
            this.bibliothequePrixItemRepository = bibliothequePrixItemRepository;
            this.ciRepository = ciRepository;
            this.budgetT4Repository = budgetT4Repository;
            this.budgetSousDetailManager = budgetSousDetailManager;
            this.budgetT4Manager = budgetT4Manager;
            this.budgetManager = budgetManager;
            this.referentielEtenduManager = referentielEtenduManager;
            this.ressourceValidator = ressourceValidator;
            this.societeManager = societeManager;
        }

        /// <summary>
        /// Retourne le sous-détail d'une tâche de niveau 4 pour un budget.
        /// Retourne le sous-détail d'une tâche de niveau 4 pour un budget.
        /// </summary>
        /// <param name="ciId">Identifiant du CI.</param>
        /// <param name="budgetT4Id">Identifiant de la tâche de niveau 4.</param>
        /// <returns>Le sous-détail de la tâche de niveau 4 pour le budget.</returns>
        public BudgetSousDetailLoad.Model Get(int ciId, int budgetT4Id)
        {
            var ret = new BudgetSousDetailLoad.Model(budgetT4Id);
            var budgetT4Exists = budgetT4Repository.Get().Any(t4 => t4.BudgetT4Id == budgetT4Id);
            if (!budgetT4Exists)
            {
                ret.Erreur = FeatureBudgetDetail.Budget_Detail_Erreur_Chargement_Inexistant;
            }
            else
            {
                var deviseId = budgetT4Repository
                        .Get()
                        .Where(t4 => t4.BudgetT4Id == budgetT4Id)
                        .Select(t4 => t4.Budget.DeviseId)
                        .First();

                var budgetSousDetailEnts = budgetSousDetailManager.GetByBudgetT4Id(budgetT4Id).ToList();

                var bibliothequePrixItems = bibliothequePrixItemRepository.GetAllBibliothequePrixItemForCi(ciId, deviseId, BibliothequePrixItemModel.Selector);

                foreach (var budgetSousDetailEnt in budgetSousDetailEnts)
                {
                    var ressourceEnt = ressourceRepository.GetById(budgetSousDetailEnt.RessourceId);

                    var item = bibliothequePrixItems.SingleOrDefault(i => i.RessourceId == budgetSousDetailEnt.RessourceId);

                    var budgetSousDetailItemModel = new BudgetSousDetailLoad.ItemModel(budgetSousDetailEnt, ressourceEnt, item);
                    ret.Items.Add(budgetSousDetailItemModel);
                }
            }
            return ret;
        }

        /// <summary>
        /// Permet de récupérer la liste des chapitres, sous-chapitres, ressources, référentiel étendus et paramétrages associés
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="deviseId"> Id de la devise voulue</param>
        /// <param name="ciEtablissementComptable">Etablissement comptable du CI</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Nombre d'éléments par page</param>
        /// <returns>Liste de chapitres</returns>
        public IEnumerable<BudgetSousDetailChapitreBibliothequePrixModel> GetChapitres(int ciId, int deviseId, EtablissementComptableEnt ciEtablissementComptable, int page, int pageSize)
        {
            try
            {
                var organisationIdOfCi = ciRepository.GetOrganisationIdByCiId(ciId).Value;
                var societe = societeManager.GetSocieteParentByOrgaId(organisationIdOfCi);

                if (societe.AssocieSeps.Any())
                {
                    societe = societe.AssocieSeps.SingleOrDefault(x => x.TypeParticipationSep.Code == TypeParticipationSep.Gerant && x.AssocieSepParentId == null).SocieteAssociee;
                }

                List<RessourceEnt> ressources = Managers.ReferentielFixe.GetListRessourceBySocieteIdWithSousChapitreEtChapitre(societe.SocieteId, ciId);

                if (ciEtablissementComptable != null && ciEtablissementComptable.RessourcesRecommandeesEnabled && ciEtablissementComptable.Organisation?.OrganisationId != null)
                {
                    List<RessourceRecommandeeFromEtablissementCIOrganisationModel> ressourcesRecommandees = Managers.ReferentielFixe.GetRessourceRecommandeeList(ciEtablissementComptable.Organisation.OrganisationId);
                    foreach (RessourceEnt ressourceToCheck in ressources)
                    {
                        ressourceToCheck.IsRecommandee = ressourceToCheck.ReferentielEtendus.Any(re => ressourcesRecommandees.Any(rr => rr.ReferentielEtenduId == re.ReferentielEtenduId));
                    }
                }

                var chapitresModel = ressources
                                        .Select(r => r.SousChapitre.Chapitre)
                                        .Distinct()
                                        .Select(BudgetSousDetailChapitreBibliothequePrixModel.Selector)
                                        .ToList();

                // les chapitres ont été récupérés pour toute la société, la collection doit etre filtrée
                if (chapitresModel != null)
                {
                    GetBibliothequePrixValues(chapitresModel, ciId, deviseId);
                    chapitresModel = chapitresModel.OrderBy(c => c.ChapitreId).Skip(page * pageSize).Take(pageSize).ToList();
                }

                return chapitresModel;
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Enregistre le sous-détail d'une tâche T4 d'un budget.
        /// </summary>
        /// <param name="model">Données à enregistrer.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public BudgetSousDetailSave.ResultModel Save(BudgetSousDetailSave.Model model)
        {
            var ret = new BudgetSousDetailSave.ResultModel();

            try
            {
                // NPI : transaction
                BudgetT4Ent budgetT4Ent = null;
                if (model.BudgetT4 != null)
                {
                    bool created;
                    budgetT4Ent = budgetT4Manager.Save(model.BudgetId, model.BudgetT4, out created);
                    if (created)
                    {
                        ret.BudgetT4CreatedId = budgetT4Ent.BudgetT4Id;
                    }
                }
                else
                {
                    budgetT4Ent = budgetT4Manager.FindById(model.BudgetT4Id);
                    if (budgetT4Ent == null)
                    {
                        ret.Erreur = FeatureBudgetSousDetail.Budget_SousDetail_Erreur_Enregistrement_DetailInexistant;
                        return ret;
                    }
                }

                ret.ItemsCreated = budgetSousDetailManager.Save(budgetT4Ent.BudgetT4Id, model.ItemsChanged, model.ItemsDeletedId);
            }
            catch (Exception ex)
            {
                ret.Erreur = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// Permet de créer un détail de ressource.
        /// </summary>
        /// <param name="ressource">La ressource à créer.</param>
        /// <returns>La ressource créée.</returns>
        public RessourceEnt CreateRessource(RessourceEnt ressource)
        {
            if (Managers.ReferentielFixe.IsRessourceExistByCode(ressource.Code))
            {
                throw new FredBusinessConflictException(string.Format(BusinessResources.CodeDejaExistant, BusinessResources.Ressource));
            }

            try
            {
                //Add ressource and ref etendu
                // à mettre un jour dans le referentielFixeMgr (mais pour ça il faut le faire hériter de Manager<> ...)
                budgetManager.BusinessValidation<RessourceEnt>(ressource, ressourceValidator);
                var userId = Managers.Utilisateur.GetContextUtilisateurId();
                var referentielEtendu = ressource.ReferentielEtendus.ToList()[0];
                referentielEtendu.ParametrageReferentielEtendus.ForEach(p => p.Devise = null);
                referentielEtendu.ParametrageReferentielEtendus.ForEach(p => p.Unite = null);
                referentielEtendu.ParametrageReferentielEtendus.ForEach(p => p.DateCreation = DateTime.Now);
                referentielEtendu.ParametrageReferentielEtendus.ForEach(p => p.AuteurCreationId = userId);
                referentielEtendu.ParametrageReferentielEtendus.ForEach(p => p.Organisation = null);

                ressource.DateCreation = DateTime.Now;
                ressource.AuteurCreationId = userId;
                ressource.SousChapitre = null;
                ressourceRepository.Insert(ressource);
                uow.Save();

                var result = referentielEtenduManager.GetRessourceWithRefEtenduAndParams(ressource.RessourceId, referentielEtendu.SocieteId);

                return result;
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de mettre à jour un détail de ressource.
        /// </summary>
        /// <param name="ressource">La ressource concernée.</param>
        /// <returns>La ressource mise-à-jour.</returns>
        public RessourceEnt UpdateRessource(RessourceEnt ressource)
        {
            try
            {
                //Clean
                ressource.SousChapitre = null;

                ressource.RessourcesEnfants.Clear();
                var parametrageRefEts = ressource.ReferentielEtendus.ToList()[0].ParametrageReferentielEtendus.ToList();
                //Clear params
                var referentielEtendu = ressource.ReferentielEtendus.ToList()[0];
                referentielEtendu.ParametrageReferentielEtendus.Clear();

                //Update ressource
                budgetManager.BusinessValidation<RessourceEnt>(ressource, ressourceValidator);// à mettre un jour dans le referentielFixeMgr (mais pour ça il faut le faire hériter de Manager<> ...)
                Managers.ReferentielFixe.UpdateRessource(ressource);

                //Update Params
                parametrageRefEts.ForEach(p => p.Devise = null);
                parametrageRefEts.ForEach(p => p.Unite = null);
                parametrageRefEts.ForEach(p => p.Organisation = null);
                parametrageRefEts.ForEach(p => referentielEtenduManager.AddOrUpdateParametrageReferentielEtendu(p));
                var result = referentielEtenduManager.GetRessourceWithRefEtenduAndParams(ressource.RessourceId, referentielEtendu.SocieteId);

                return result;
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de créer un détail de ressource.
        /// </summary>
        /// <param name="code">Code de la ressource.</param>
        /// <param name="groupeCode">Le code du groupe.</param>
        /// <returns>La ressource créée.</returns>
        public List<RessourceEnt> GetRessourceByCodeAndGroupeCode(string code, string groupeCode)
        {
            return referentielEtenduManager.GetRessourceByCodeAndGroupeCode(code, groupeCode);
        }

        /// <summary>
        /// Renseigne les PU et unité des ressources à partir des données présentes dans la bibliotheque des prix 
        /// </summary>
        /// <param name="chapitres">chapitres à filtrer</param>
        /// <param name="ciId">CiId dont on récupère les valeurs de la bibliotheque des prix</param>
        /// <param name="deviseId">Devise que l'on souhaite utiliser pour la bibliotheque</param>
        private void GetBibliothequePrixValues(IEnumerable<BudgetSousDetailChapitreBibliothequePrixModel> chapitres, int ciId, int deviseId)
        {
            var bibliothequePrixItems = bibliothequePrixItemRepository.GetAllBibliothequePrixItemForCi(
                ciId,
                deviseId,
                item => new
                {
                    item.Prix,
                    item.Unite,
                    item.RessourceId
                })
                .ToList();

            var ressources = chapitres.SelectMany(c => c.SousChapitres.SelectMany(sc => sc.Ressources));
            foreach (var ressource in ressources)
            {
                var item = bibliothequePrixItems.SingleOrDefault(i => i.RessourceId == ressource.RessourceId);
                if (item != null)
                {
                    ressource.BibliothequePrixMontant = item.Prix;
                    ressource.Unite = item.Unite;
                }
            }
        }
    }
}
