using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.Details;
using Fred.Business.Budget.Helpers;
using Fred.Business.Budget.Mapper;
using Fred.Business.Budget.Recette;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Depense;
using Fred.Business.Depense.Services;
using Fred.Business.Params;
using Fred.Business.Referential;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielEtendu;
using Fred.Business.ReferentielFixe;
using Fred.Business.Societe;
using Fred.Business.Unite;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Referential.Common;
using Fred.Entities;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Budget;
using Fred.Web.Shared.Models.Budget.Avancement;
using Fred.Web.Shared.Models.Budget.BibliothequePrix.SousDetail;
using Fred.Web.Shared.Models.Budget.Depense;
using Fred.Web.Shared.Models.Budget.Details;
using Fred.Web.Shared.Models.Budget.Liste;
using Fred.Web.Shared.Models.Budget.Recette;
using static Fred.Entities.Constantes;

namespace Fred.Business.Budget
{
    public class BudgetMainManager : ManagersAccess, IBudgetMainManager
    {
        private readonly AvancementLoader avancementLoader;
        private readonly AvancementRecetteLoader avancementRecetteLoader;
        private readonly ITacheRepository tacheRepository;
        private readonly IUnitOfWork uow;
        private readonly BudgetMainManagerDetailFeature detailFeature;
        private readonly BudgetMainManagerSousDetailFeature sousDetailFeature;
        private readonly ITacheSearchHelper taskSearchHelper;
        private readonly IBudgetDetailsExportExcelFeature detailBudgetExportExcel;
        private readonly ICIManager ciManager;
        private readonly IDepenseServiceMediator depenseServiceMediator;
        private readonly IBudgetCopieManager budgetCopieManager;
        private readonly IBudgetSousDetailManager budgetSousDetailManager;
        private readonly IAvancementWorkflowManager avancementWorkflowManager;
        private readonly IAvancementRecetteManager avancementRecetteManager;
        private readonly IAvancementEtatManager avancementEtatManager;
        private readonly IAvancementManager avancementManager;
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly IBudgetManager budgetManager;
        private readonly IControleBudgetaireManager controleBudgetaireManager;
        private readonly IUniteManager uniteManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly ITacheManager tacheManager;

        public BudgetMainManager(
            AvancementLoader avancementLoader,
            AvancementRecetteLoader avancementRecetteLoader,
            IUnitOfWork uow,
            ITacheRepository tacheRepository,
            ITacheSearchHelper taskSearchHelper,
            IRessourceRepository ressourceRepository,
            IBudgetBibliothequePrixItemRepository budgetBibliothequePrixItemRepository,
            ICIRepository ciRepository,
            IBudgetT4Repository budgetT4Repository,
            IBudgetDetailsExportExcelFeature detailBudgetExportExcel,
            ICIManager ciManager,
            IDepenseServiceMediator depenseServiceMediator,
            IBudgetCopieManager budgetCopieManager,
            IBudgetEtatManager budgetEtatManager,
            IBudgetTacheManager budgetTacheManager,
            IBudgetSousDetailManager budgetSousDetailManager,
            IAvancementWorkflowManager avancementWorkflowManager,
            IAvancementRecetteManager avancementRecetteManager,
            IAvancementEtatManager avancementEtatManager,
            IAvancementManager avancementManager,
            IBudgetT4Manager budgetT4Manager,
            IBudgetWorkflowManager budgetWorkflowManager,
            IBudgetManager budgetManager,
            IControleBudgetaireManager controleBudgetaireManager,
            IParamsManager paramsManager,
            IUniteManager uniteManager,
            IReferentielEtenduManager referentielEtenduManager,
            IDatesClotureComptableManager datesClotureComptableManager,
            ITacheManager tacheManager,
            IDeviseManager deviseManager,
            IRessourceValidator ressourceValidator,
            ISocieteManager societeManager)
        {
            this.avancementLoader = avancementLoader;
            this.avancementRecetteLoader = avancementRecetteLoader;
            this.uow = uow;
            this.tacheRepository = tacheRepository;
            this.taskSearchHelper = taskSearchHelper;
            this.detailBudgetExportExcel = detailBudgetExportExcel;
            this.ciManager = ciManager;
            this.depenseServiceMediator = depenseServiceMediator;
            this.budgetCopieManager = budgetCopieManager;
            this.budgetSousDetailManager = budgetSousDetailManager;
            this.avancementWorkflowManager = avancementWorkflowManager;
            this.avancementRecetteManager = avancementRecetteManager;
            this.avancementEtatManager = avancementEtatManager;
            this.avancementManager = avancementManager;
            this.budgetT4Manager = budgetT4Manager;
            this.budgetManager = budgetManager;
            this.controleBudgetaireManager = controleBudgetaireManager;
            this.uniteManager = uniteManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.tacheManager = tacheManager;

            detailFeature = new BudgetMainManagerDetailFeature(
                uow,
                tacheRepository,
                taskSearchHelper,
                ressourceRepository,
                budgetBibliothequePrixItemRepository,
                ciRepository,
                budgetT4Repository,
                budgetEtatManager,
                budgetTacheManager,
                budgetSousDetailManager,
                budgetT4Manager,
                budgetManager,
                budgetWorkflowManager,
                paramsManager,
                referentielEtenduManager,
                tacheManager,
                deviseManager,
                ressourceValidator,
                societeManager);

            sousDetailFeature = new BudgetMainManagerSousDetailFeature(
                uow,
                ressourceRepository,
                budgetBibliothequePrixItemRepository,
                ciRepository,
                budgetT4Repository,
                budgetSousDetailManager,
                budgetT4Manager,
                budgetManager,
                referentielEtenduManager,
                ressourceValidator,
                societeManager);
        }

        #region Détail

        /// <summary>
        /// Renvoi un export excel en utilisant la classe BudgetDetailExportExcelFeature
        /// </summary>
        /// <param name="model">BudgetDetailsExportExcelLoadModel</param>
        /// <returns>les données de l'excel sous forme de tableau d'octets</returns>
        public byte[] GetBudgetDetailExportExcel(BudgetDetailsExportExcelLoadModel model)
        {
            return detailFeature.GetExportExcel(detailBudgetExportExcel, model);
        }

        /// <summary>
        /// Retourne le détail d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <returns>Le détail du budget.</returns>
        public BudgetDetailLoad.Model GetDetail(int budgetId)
        {
            return detailFeature.Get(budgetId);
        }

        /// <summary>
        /// Enregistre le détail d'un budget.
        /// </summary>
        /// <param name="model">Données à enregistrer.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public BudgetDetailSave.ResultModel SaveDetail(BudgetDetailSave.Model model)
        {
            return detailFeature.Save(model);
        }

        /// <summary>
        /// Valide le détail d'un budget.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget.</param>
        /// <param name="commentaire">Commentaire</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public string ValidateDetailBudget(int budgetId, string commentaire = "")
        {
            return detailFeature.Validate(budgetId, commentaire);
        }

        /// <summary>
        /// Passe l'état d'un budget 
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <param name="commentaire">Commentaire</param>
        /// <returns>vide</returns>
        public string RetourBrouillon(int budgetId, string commentaire = "")
        {
            return detailFeature.RetourBrouillon(budgetId, commentaire);
        }

        /// <summary>
        /// Copie des sous-détails dans des budgets T4.
        /// </summary>
        /// <param name="model">Données à copier.</param>
        /// <returns>Le résultat de la copie.</returns>
        public BudgetSousDetailCopier.ResultModel CopySousDetails(BudgetSousDetailCopier.Model model)
        {
            return detailFeature.CopySousDetails(model);
        }

        /// <summary>
        /// Retourne les tâches de niveau 4 qui ne sont pas utilisées dans une révision de budget.
        /// </summary>
        /// <param name="model">Model de chargement des tâches 4 non utilisées dans une révision de budget.</param>
        /// <returns>Le résultat du chargement.</returns>
        public Tache4Inutilisees.ResultModel GetTache4Inutilisees(Tache4Inutilisees.Model model)
        {
            return detailFeature.GetTache4Inutilisees(model);
        }

        #endregion
        #region Sous détail

        /// <summary>
        /// Retourne le sous-détail d'une tâche de niveau 4 pour un budget.
        /// </summary>
        /// <param name="ciId">Identifiant du CI.</param>
        /// <param name="budgetT4Id">Identifiant de la tâche de niveau 4.</param>
        /// <returns>Le sous-détail de la tâche de niveau 4 pour le budget.</returns>
        public BudgetSousDetailLoad.Model GetSousDetail(int ciId, int budgetT4Id)
        {
            return sousDetailFeature.Get(ciId, budgetT4Id);
        }

        /// <summary>
        /// Permet de récupérer la liste des chapitres, sous-chapitres, ressources, référentiel étendus et paramétrages associés
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="deviseId">Devise que l'on souhaite avoir pour les PU des ressources retournées</param>
        /// <param name="filter">Filtre sur les libellés</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Nombre d'éléments par page</param>
        /// <returns>Liste de chapitres</returns>
        public IEnumerable<BudgetSousDetailChapitreBibliothequePrixModel> GetChapitres(int ciId, int deviseId, string filter, int page, int pageSize)
        {
            EtablissementComptableEnt etablissementCI = ciManager.GetEtablissementComptableByCIId(ciId);
            return sousDetailFeature.GetChapitres(ciId, deviseId, etablissementCI, page, pageSize);
        }

        /// <summary>
        /// Enregistre le sous-détail d'une tâche T4 d'un budget.
        /// </summary>
        /// <param name="model">Données à enregistrer.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public BudgetSousDetailSave.ResultModel SaveSousDetail(BudgetSousDetailSave.Model model)
        {
            return sousDetailFeature.Save(model);
        }

        /// <summary>
        /// Permet de créer un détail de ressource.
        /// </summary>
        /// <param name="ressource">La ressource à créer.</param>
        /// <returns>La ressource créée.</returns>
        public RessourceEnt CreateRessource(RessourceEnt ressource)
        {
            return sousDetailFeature.CreateRessource(ressource);
        }

        /// <summary>
        /// Permet de mettre à jour un détail de ressource.
        /// </summary>
        /// <param name="ressource">La ressource concernée.</param>
        /// <returns>La ressource mise-à-jour.</returns>
        public RessourceEnt UpdateRessource(RessourceEnt ressource)
        {
            return sousDetailFeature.UpdateRessource(ressource);
        }

        /// <summary>
        /// Permet de créer un détail de ressource.
        /// </summary>
        /// <param name="code">Code de la ressource.</param>
        /// <param name="groupeCode">Le code du groupe.</param>
        /// <returns>La ressource créée.</returns>
        public List<RessourceEnt> GetRessourceByCodeAndGroupeCode(string code, string groupeCode)
        {
            return sousDetailFeature.GetRessourceByCodeAndGroupeCode(code, groupeCode);
        }

        /// <summary>
        /// Retourne le message de confirmation pour la mise en applciation d'une version de budget
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="version">numéro de la revision</param>
        /// <returns>Le message de validation pour la mise en applciation du budget</returns>
        public string GetMessageMiseEnApllication(int ciId, string version)
        {
            var message = string.Empty;
            var messageVersion = string.Empty;
            var currentVersion = version;
            var newVersion = string.Empty;
            var budgetEnApplication = budgetManager.GetBudgetEnApplication(ciId);
            var date = DateTime.Today;
            var messageDate = string.Format(FeatureBudgetDetail.Budget_Validation_Avertissement_Date, date.ToString("d MMM yyyy"));
            if (budgetEnApplication != null)
            {
                var applyVersion = budgetEnApplication.Version;
                newVersion = VersionHelper.IncrementVersionMajeure(applyVersion);
                var messageVersionActuelle = string.Format(FeatureBudgetDetail.Budget_Validation_Avertissement_VersionActuelle, applyVersion);
                messageVersion = string.Format(FeatureBudgetDetail.Budget_Validation_Avertissement_VersionIncrementee, currentVersion, newVersion);
                message = string.Concat(messageDate, Environment.NewLine, messageVersionActuelle, Environment.NewLine, messageVersion);
            }
            else
            {
                newVersion = "1.0";
                messageVersion = string.Format(FeatureBudgetDetail.Budget_Validation_Avertissement_VersionIncrementee, currentVersion, newVersion);
                message = string.Concat(messageDate, Environment.NewLine, messageVersion);
            }

            return message;
        }

        #endregion
        #region Tâche niveau 4

        /// <summary>
        /// Propose un nouveau code pour une tâche enfant en fonction de sa tâche parente.
        /// </summary>
        /// <param name="tacheParenteId">Identifiant de la tâche parente.</param>
        /// <returns>Le code.</returns>
        public string GetNextTacheCode(int tacheParenteId)
        {
            var tacheEntParente = tacheManager.GetTacheById(tacheParenteId);
            return tacheManager.GetNextTaskCode(tacheEntParente);
        }

        /// <summary>
        /// Propose des nouveaux codes pour une tâche enfant en fonction de sa tâche parente.
        /// </summary>
        /// <param name="tacheParenteId">Identifiant de la tâche parente.</param>
        /// <param name="count">Le nombre de code à proposer</param>
        /// <returns>Les codes.</returns>
        public List<string> GetNextTacheCodes(int tacheParenteId, int count)
        {
            var tacheEntParente = tacheManager.GetTacheById(tacheParenteId);
            return tacheManager.GetNextTaskCodes(tacheEntParente, count);
        }

        /// <summary>
        /// Crée une tâche de niveau 4.
        /// </summary>
        /// <param name="model">Modèle de création d'une tâche de niveau 4.</param>
        /// <returns>Le modèle du résultat de la création.</returns>
        public ManageT4Create.ResultModel CreateTache4(ManageT4Create.Model model)
        {
            var ret = new ManageT4Create.ResultModel();
            string erreur = null;
            var tacheParent = tacheRepository.GetTacheById(model.Tache3Id);
            if (!CanCreateTache4(model.Code, model.Libelle, model.CiId, model.Tache3Id, out erreur))
            {
                ret.Erreur = erreur;
                return ret;
            }
            // Enregistrement
            var tache4Ent = new TacheEnt()
            {
                Active = tacheParent.Active,
                AuteurCreationId = Managers.Utilisateur.GetContextUtilisateurId(),
                CiId = model.CiId,
                Code = model.Code,
                DateCreation = DateTime.UtcNow,
                Libelle = model.Libelle,
                Niveau = 4,
                ParentId = model.Tache3Id,
                TacheParDefaut = false
            };
            try
            {
                var parent = tacheManager.FindById(model.Tache3Id);
                tache4Ent.Active = parent.Active;

                var tacheId = tacheManager.AddTache(tache4Ent);
                var tacheEnt = tacheManager.FindById(tacheId);
                ret.Tache = new TacheResultModel(tacheEnt);
            }
            catch (FredBusinessException ex)
            {
                ret.Tache = null;
                ret.Erreur = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// Créé plusieurs tâches 4.
        /// </summary>
        /// <param name="model">Modèle de création de plusieurs tâches 4..</param>
        /// <returns>Le modèle du résultat de la création.</returns>
        public CreateTaches4.ResultModel CreateTaches4(CreateTaches4.Model model)
        {
            var ret = new CreateTaches4.ResultModel();
            var erreurs = new List<string>(model.Taches4.Count);
            var valid = true;
            foreach (var tache4 in model.Taches4)
            {
                string erreur = null;
                valid &= CanCreateTache4(tache4.Code, tache4.Libelle, model.CiId, model.Tache3Id, out erreur);
                erreurs.Add(erreur);
            }
            if (!valid)
            {
                ret.Erreurs = erreurs;
                return ret;
            }
            // Enregistrement
            try
            {
                var auteurCreationId = Managers.Utilisateur.GetContextUtilisateurId();
                var utcNow = DateTime.UtcNow;
                var taches4CreatedEnts = new List<TacheEnt>();
                var tacheParent = tacheRepository.GetTacheById(model.Tache3Id);
                foreach (var tache4 in model.Taches4)
                {
                    var tache4Ent = new TacheEnt()
                    {
                        Active = tacheParent.Active,
                        AuteurCreationId = auteurCreationId,
                        CiId = model.CiId,
                        Code = tache4.Code,
                        DateCreation = utcNow,
                        Libelle = tache4.Libelle,
                        Niveau = 4,
                        ParentId = model.Tache3Id,
                        TacheParDefaut = false
                    };

                    taches4CreatedEnts.Add(tache4Ent);
                    tacheRepository.Insert(tache4Ent);
                }
                uow.Save();
                ret.Taches4CreatedIds = new List<int>(taches4CreatedEnts.Count);
                foreach (var tache4Created in taches4CreatedEnts)
                {
                    ret.Taches4CreatedIds.Add(tache4Created.TacheId);
                }
            }
            catch (FredRepositoryException ex)
            {
                ret.Erreur = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// Indique si une tâche 4 peut être créée.
        /// </summary>
        /// <param name="code">Le code de la tâche.</param>
        /// <param name="libelle">Le libellé de la tâche.</param>
        /// <param name="ciId">L'identifiant du CI de la tâche.</param>
        /// <param name="tache3Id">L'identifiant de la tâche 3 parente.</param>
        /// <param name="erreur">L'erreur ou null si pas d'erreur.</param>
        /// <returns>True si la tâche 4 peut-être ajoutée, sinon false.</returns>
        private bool CanCreateTache4(string code, string libelle, int ciId, int tache3Id, out string erreur)
        {
            erreur = null;
            if (string.IsNullOrEmpty(code?.Trim()))
            {
                erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_CodeManquant;
            }
            else if (string.IsNullOrEmpty(libelle?.Trim()))
            {
                erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_LibelleManquant;
            }
            else
            {
                var tacheParente = tacheManager.FindById(tache3Id);
                if (tacheParente == null)
                {
                    // La tâche parente n'existe pas
                    erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_TacheParenteInexistante;
                }
                else if (tacheParente.CiId != ciId)
                {
                    // La tâche parente n'est pas sur le bon CI
                    erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_TacheParenteMauvaisCI;
                }
                else if (tacheParente.Niveau != 3)
                {
                    // La tâche parente n'est pas de niveau 3
                    erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_TacheParentePasDeNiveau3;
                }
                else if (tacheManager.IsCodeTacheExist(code, ciId))
                {
                    // Le code de la tâche existe déjà
                    erreur = string.Format(FeatureBudgetDetail.Budget_GestionT4_Erreur_TacheExistante, code);
                }
            }
            return erreur == null;
        }

        /// <summary>
        /// Change une tâche de niveau 4.
        /// </summary>
        /// <param name="model">Modèle de changement d'une tâche de niveau 4.</param>
        /// <returns>Le modèle du résultat du changement.</returns>
        public ManageT4Change.ResultModel ChangeTache4(ManageT4Change.Model model)
        {
            var ret = new ManageT4Change.ResultModel();
            if (string.IsNullOrEmpty(model.Code?.Trim()))
            {
                ret.Erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_CodeManquant;
            }
            else if (string.IsNullOrEmpty(model.Libelle?.Trim()))
            {
                ret.Erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_LibelleManquant;
            }
            else
            {
                var tache4Ent = tacheManager.FindById(model.TacheId);
                if (tache4Ent == null)
                {
                    // La tâche n'existe pas
                    ret.Erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_TacheInexistante;
                }
                else if (tache4Ent.CiId != model.CiId)
                {
                    // La tâche n'est pas sur le bon CI
                    ret.Erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_TacheMauvaisCI;
                }
                else if (tache4Ent.Niveau != 4)
                {
                    // La tâche n'est pas de niveau 4
                    ret.Erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_TachePasDeNiveau4;
                }
                else if (tache4Ent.Code != model.Code && tacheManager.IsCodeTacheExist(model.Code, model.CiId))
                {
                    // Le code de la tâche existe déjà
                    ret.Erreur = string.Format(FeatureBudgetDetail.Budget_GestionT4_Erreur_TacheExistante, model.Code);
                }
                else
                {
                    try
                    {
                        tache4Ent.Code = model.Code;
                        tache4Ent.Libelle = model.Libelle;
                        tacheManager.UpdateTache(tache4Ent);
                        ret.Tache = new TacheResultModel(tache4Ent);
                    }
                    catch (Exception ex)
                    {
                        ret.Tache = null;
                        ret.Erreur = ex.Message;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Supprime une tâche de niveau 4.
        /// </summary>
        /// <param name="model">Modèle de suppression d'une tâche de niveau 4.</param>
        /// <returns>Le modèle du résultat de la suppression.</returns>
        public ManageT4Delete.ResultModel DeleteTache4(ManageT4Delete.Model model)
        {
            var ret = new ManageT4Delete.ResultModel();
            var tache4Ent = tacheManager.FindById(model.TacheId);
            if (tache4Ent == null)
            {
                // La tâche n'existe pas
                ret.Erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_TacheInexistante;
            }
            else if (tache4Ent.CiId != model.CiId)
            {
                // La tâche n'est pas sur le bon CI
                ret.Erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_TacheMauvaisCI;
            }
            else if (tache4Ent.Niveau != 4)
            {
                // La tâche n'est pas de niveau 4
                ret.Erreur = FeatureBudgetDetail.Budget_GestionT4_Erreur_TachePasDeNiveau4;
            }
            else
            {
                try
                {
                    // NPI : il faudrait une transaction pour pouvoir supprimer le BudgetT4 correspondant (s'il existe) avant de tenter de supprimer le T4.
                    // Comme BudgetT4 est lié au T4, on ne peut pas supprimer le T4 sans supprimer au moins le BudgetT4 correspondant (s'il existe)
                    tacheManager.DeleteTacheById(tache4Ent.TacheId);
                }
                catch (Exception ex)
                {
                    ret.Erreur = ex.Message;
                }
            }

            return ret;
        }

        #endregion
        #region Avancement
        /// <inheritdoc />
        public AvancementLoadModel GetAvancement(int ciId, int periode)
        {
            return avancementLoader.Load(ciId, periode);
        }

        /// <inheritdoc />
        public void SaveAvancement(AvancementSaveModel model)
        {
            var userId = Managers.Utilisateur.GetContextUtilisateurId();
            foreach (var tache4 in model.ListAvancementT4)
            {
                var budgetT4 = budgetT4Manager.GetByIdWithSousDetailAndAvancement(tache4.BudgetT4Id);
                if (budgetT4 != null)
                {
                    budgetT4.TypeAvancement = tache4.TypeAvancement;
                    foreach (var sd in budgetT4.BudgetSousDetails)
                    {
                        avancementManager.AddOrUpdateAvancement(sd, tache4, model.Periode, model.CiId, model.DeviseId, budgetT4.TypeAvancement.Value, userId);
                    }

                    budgetT4Manager.Update(budgetT4);
                }
            }
            // update des taches d'avancement indépendemment du sous détail de T4
            avancementManager.UpdateListeTacheAvancement(model.BudgetId, model.Periode, model.ListTaches);
            avancementManager.Save();
            budgetT4Manager.Save();
        }

        /// <inheritdoc />
        public void ValidateAvancementModel(AvancementSaveModel model, string etatAvancement)
        {
            var budgetEnt = budgetManager.GetBudget(model.BudgetId, true);

            List<int> listBudgetSousDetailId = new List<int>();
            foreach (var t4 in budgetEnt.BudgetT4s)
            {
                listBudgetSousDetailId = listBudgetSousDetailId.Concat(t4.BudgetSousDetails.Select(sd => sd.BudgetSousDetailId)).ToList();
            }

            var userId = Managers.Utilisateur.GetContextUtilisateurId();
            var listPeriode = avancementManager.GetListPeriodeAvancementNotValidBeforePeriode(model.CiId, model.Periode, etatAvancement, listBudgetSousDetailId);

            foreach (var periode in listPeriode)
            {
                foreach (var budgetT4 in budgetEnt.BudgetT4s)
                {
                    foreach (var sd in budgetT4.BudgetSousDetails)
                    {
                        var avancement = avancementManager.GetAvancement(sd.BudgetSousDetailId, periode);
                        if (avancement?.AvancementEtat.Code == EtatAvancement.Enregistre)
                        {
                            AvancementEtatToAValider(avancement, userId);
                        }
                        else if (avancement?.AvancementEtat.Code == EtatAvancement.AValider)
                        {
                            AvancementEtatToValide(avancement, userId);
                        }
                    }
                }
            }

            avancementManager.Save();
        }

        /// <inheritdoc />
        public void RetourBrouillonAvancement(AvancementSaveModel model)
        {
            var userId = Managers.Utilisateur.GetContextUtilisateurId();
            foreach (var tache4 in model.ListAvancementT4)
            {
                var budgetT4 = budgetT4Manager.GetByIdWithSousDetailAndAvancement(tache4.BudgetT4Id);
                if (budgetT4 != null)
                {
                    foreach (var sd in budgetT4.BudgetSousDetails)
                    {
                        var avancement = avancementManager.GetAvancement(sd.BudgetSousDetailId, model.Periode);
                        if (avancement.AvancementEtat.Code == EtatAvancement.AValider)
                        {
                            AvancementEtatToEnregistre(avancement, userId);
                        }
                    }
                }
            }
            avancementManager.Save();
        }

        /// <summary>
        /// Change l'état d'un budget brouillon vers à valider
        /// </summary>
        /// <param name="avancement">L'avancement</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        private void AvancementEtatToAValider(AvancementEnt avancement, int utilisateurId)
        {
            var etatCibleId = avancementEtatManager.GetByCode(EtatAvancement.AValider).AvancementEtatId;
            AvancementChangeEtat(avancement, etatCibleId, utilisateurId);
        }

        /// <summary>
        /// Change l'état d'un budget brouillon vers validé
        /// </summary>
        /// <param name="avancement">L'avancement</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        private void AvancementEtatToValide(AvancementEnt avancement, int utilisateurId)
        {
            var etatCibleId = avancementEtatManager.GetByCode(EtatAvancement.Valide).AvancementEtatId;
            AvancementChangeEtat(avancement, etatCibleId, utilisateurId);
        }

        /// <summary>
        /// Change l'état d'un budget brouillon vers enregistré
        /// </summary>
        /// <param name="avancement">L'avancement</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        private void AvancementEtatToEnregistre(AvancementEnt avancement, int utilisateurId)
        {
            var etatCibleId = avancementEtatManager.GetByCode(EtatAvancement.Enregistre).AvancementEtatId;
            AvancementChangeEtat(avancement, etatCibleId, utilisateurId);
        }

        /// <summary>
        /// Modifie l'état d'un budget et genère une entrée dans le workflow
        /// </summary>
        /// <param name="avancement">L'avancement</param>
        /// <param name="etatCibleId">identifiant de l'état cible</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur</param>
        private void AvancementChangeEtat(AvancementEnt avancement, int etatCibleId, int utilisateurId)
        {
            avancementWorkflowManager.Add(avancement, etatCibleId, utilisateurId);
            avancement.AvancementEtatId = etatCibleId;
            avancementManager.Update(avancement);
        }
        #endregion
        #region AvancementRecette
        /// <inheritdoc />
        public AvancementRecetteLoadModel GetAvancementRecette(int ciId, int periode)
        {
            var budgetId = budgetManager.GetIdBudgetEnApplication(ciId);
            return avancementRecetteLoader.Load(budgetId, periode);
        }

        /// <summary>
        /// Retourne une lists l'avancement d'une recette pour un CI et une période
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="fromperiode">Dabut Période YYYYMM</param>
        /// <param name="toperiode">Fin Période YYYYMM</param>
        /// <returns>une liste L'avancement d'une recette pour un CI et une période</returns>
        public List<AvancementRecetteLoadModel> GetAvancementRecetteList(int ciId, int fromperiode, int toperiode)
        {
            int budgetId = GetIdBudgetEnApplication(ciId);
            return avancementRecetteLoader.LoadRecetteToPeriode(budgetId, fromperiode, toperiode);
        }

        public List<PeriodAvancementRecetteLoadModel> GetAvancementRecettesForPeriode(int ciId, int fromPeriode, int toPeriode)
        {
            int budgetId = GetIdBudgetEnApplication(ciId);
            return avancementRecetteLoader.LoadRecetteToPeriodes(budgetId, fromPeriode, toPeriode);
        }

        private int GetIdBudgetEnApplication(int ciId)
        {
            return budgetManager.GetIdBudgetEnApplication(ciId);
        }

        /// <summary>
        /// Enregistre les données d'un avancement recette
        /// </summary>
        /// <param name="model">Modèle de sauvegarde pour l'avancement recette</param>
        /// <returns>L'identifiant de l'avancement recette</returns>
        public int SaveAvancementRecette(AvancementRecetteSaveModel model)
        {
            return avancementRecetteManager.SaveAvancementRecette(model);
        }
        #endregion
        #region Recalage 

        public async Task<int> RecalageBudgetaireAsync(int budgetSourceId, int userId, int periodeFin)
        {
            var budgetRevise = await budgetCopieManager.CopierBudgetDansMemeCiAsync(budgetSourceId, userId, false).ConfigureAwait(false);

            var filtre = new SearchDepense
            {
                CiId = budgetRevise.CiId,
                PeriodeDebut = null,
                PeriodeFin = PeriodeHelper.ToLastDayOfMonthDateTime(periodeFin)
            };

            var depenses = BudgetDepenseMapper.Map(await depenseServiceMediator.GetAllDepenseExternetWithTacheAndRessourceAsync(filtre).ConfigureAwait(false));
            UpdateT4s(budgetSourceId, budgetRevise, userId, periodeFin, depenses);
            return budgetRevise.BudgetId;
        }

        private void UpdateT4s(int budgetSourceId, BudgetEnt budgetRevise, int userId, int periodeFin, IEnumerable<BudgetDepenseModel> depenses)
        {
            List<TacheEnt> existingTaches = tacheManager.GetListByCi(budgetRevise.CiId);
            IEnumerable<TacheRecalageParameter> existingTachesT3 = GetTaches3(existingTaches);

            int uniteForfaitId = uniteManager.GetUnite(CodeUnite.Forfait).UniteId;
            AvancementEtatEnt saveAvancement = avancementEtatManager.GetByCode(EtatAvancement.Enregistre);

            foreach (var existingTacheT3 in existingTachesT3)
            {
                NewT4Rev(budgetRevise, existingTacheT3, userId, periodeFin, uniteForfaitId, depenses, existingTaches, saveAvancement.AvancementEtatId);
            }

            // Mise à jour des T4 existants
            List<BudgetT4Ent> listBudgetT4 = budgetT4Manager.GetT4ByBudgetId(budgetSourceId);
            RecalculQuantiteARealiserT4(listBudgetT4, budgetRevise.BudgetId, periodeFin);
            uow.Save();

            avancementManager.Save();
        }

        public IEnumerable<TacheRecalageParameter> GetTaches3(List<TacheEnt> taches)
        {
            return taches.Where(t => t.Niveau == 3).Select(t => new TacheRecalageParameter()
            {
                TacheId = t.TacheId,
                Code = t.Code,
                TacheType = t.TacheType
            });
        }

        private void NewT4Rev(BudgetEnt budgetRevise, TacheRecalageParameter tache3, int userId, int periodeFin, int uniteForfaitId, IEnumerable<BudgetDepenseModel> depenses, List<TacheEnt> existingTaches, int saveAvancementEtatId)
        {
            List<IGrouping<int, BudgetDepenseModel>> depensesGroupRessource;
            if (taskSearchHelper.IsTacheEcart(tache3.TacheType))
            {
                depensesGroupRessource = depenses.Where(x => x.Tache.Code == tache3.Code)
                                                 .GroupBy(x => x.RessourceId)
                                                 .ToList();
            }
            else
            {
                depensesGroupRessource = depenses.Where(x => x.TacheId == tache3.TacheId)
                                                 .GroupBy(x => x.RessourceId)
                                                 .ToList();
            }

            if (depensesGroupRessource.Count > 0)
            {
                var tache4 = new TacheEnt()
                {
                    CiId = budgetRevise.CiId,
                    AuteurCreationId = userId,
                    Code = "T4REV" + tache3.Code,
                    DateCreation = DateTime.UtcNow,
                    Libelle = "T4 Réalisée",
                    ParentId = tache3.TacheId,
                    Niveau = 4,
                    BudgetId = budgetRevise.BudgetId
                };
                var tache4Id = tacheManager.AddTache4(tache4, budgetRevise.BudgetId, existingTaches);
                BudgetT4Ent t4Rev = null;
                if (tache4Id == 0)
                {
                    t4Rev = new BudgetT4Ent()
                    {
                        BudgetId = budgetRevise.BudgetId,
                        T4 = tache4,
                        T3Id = tache3.TacheId,
                        TypeAvancement = (int)TypeAvancementBudget.Pourcentage,
                        QuantiteDeBase = 1,
                        QuantiteARealiser = 1,
                        UniteId = uniteForfaitId,
                        IsReadOnly = true
                    };
                }
                else
                {
                    t4Rev = new BudgetT4Ent()
                    {
                        BudgetId = budgetRevise.BudgetId,
                        T4Id = tache4Id,
                        T3Id = tache3.TacheId,
                        TypeAvancement = (int)TypeAvancementBudget.Pourcentage,
                        QuantiteDeBase = 1,
                        QuantiteARealiser = 1,
                        UniteId = uniteForfaitId,
                        IsReadOnly = true
                    };
                }

                var listSDRealise = GetListSousDetailFromDepense(t4Rev, depensesGroupRessource);

                foreach (var sousDetailRealise in listSDRealise)
                {
                    budgetSousDetailManager.InsereSousDetail(sousDetailRealise);
                    avancementManager.CreationAvancementT4Rev(sousDetailRealise, budgetRevise.CiId, budgetRevise.DeviseId, periodeFin, saveAvancementEtatId);
                }

                decimal montantT4REV = listSDRealise.Sum(x => x.Montant ?? 0);
                t4Rev.PU = montantT4REV;
                t4Rev.MontantT4 = montantT4REV;
                budgetT4Manager.Add(t4Rev);
            }
        }

        private List<BudgetSousDetailEnt> GetListSousDetailFromDepense(BudgetT4Ent t4Rev, List<IGrouping<int, BudgetDepenseModel>> depensesGroupRessource)
        {
            var listSD = new List<BudgetSousDetailEnt>();

            foreach (var depenseGroup in depensesGroupRessource)
            {
                listSD.Add(new BudgetSousDetailEnt
                {
                    BudgetT4 = t4Rev,
                    RessourceId = depenseGroup.Key,
                    Montant = depenseGroup.Sum(x => x.MontantHT),
                    Quantite = depenseGroup.Sum(x => Math.Abs(x.Quantite ?? 1)),
                    QuantiteSD = depenseGroup.Sum(x => Math.Abs(x.Quantite ?? 1)),
                    UniteId = depenseGroup.FirstOrDefault()?.UniteId
                });
            }

            // calcul du PU à partir des montants et des quantités
            listSD.ForEach(x => x.PU = (x.Quantite ?? 0) == 0 ? 0 : x.Montant / x.Quantite);

            return listSD;
        }

        private void RecalculQuantiteARealiserT4(IEnumerable<BudgetT4Ent> listBudgetT4, int budgetRecaleId, int periode)
        {
            IEnumerable<int> budgetSousDetailsIds = listBudgetT4.SelectMany(b => b.BudgetSousDetails.Select(bsd => bsd.BudgetSousDetailId));
            List<AvancementEnt> avancementsSDs = avancementManager.GetAvancements(budgetSousDetailsIds, periode);

            IEnumerable<int> t4Ids = listBudgetT4.Select(b => b.T4Id);
            List<BudgetT4Ent> budgets = budgetT4Manager.GetByTacheIdsAndBudgetId(budgetRecaleId, t4Ids);

            foreach (var t4 in listBudgetT4)
            {
                if (t4.BudgetSousDetails != null)
                    ProcessBudgetSousDetails(t4, avancementsSDs, budgets);
            }
        }

        private void ProcessBudgetSousDetails(BudgetT4Ent t4, List<AvancementEnt> avancementsSDs, List<BudgetT4Ent> budgets)
        {
            foreach (var sd in t4.BudgetSousDetails)
            {
                AvancementEnt avancementT4 = avancementsSDs.FirstOrDefault(a => a.BudgetSousDetailId == sd.BudgetSousDetailId);

                if (avancementT4 != null)
                {
                    BudgetT4Ent budget = budgets.FirstOrDefault(b => b.T4Id == t4.T4Id);

                    if (budget != null)
                    {
                        UpdateQuantiteT4(budget, avancementT4);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Recalcule la nouvelle quantité à réaliser et le montant d'un T4.
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <param name="tache4Id">Identifiant de la tâche de niveau 4</param>
        /// <param name="avancement">Entité avancement</param>
        private void UpdateQuantiteT4(BudgetT4Ent budgetT4, AvancementEnt avancement)
        {
            var quantiteARealiserInitiale = budgetT4.QuantiteARealiser;
            var montant = budgetT4.MontantT4;
            if (avancement.QuantiteSousDetailAvancee.HasValue)
            {
                budgetT4.QuantiteARealiser = budgetT4.QuantiteARealiser - avancement.QuantiteSousDetailAvancee.Value;
                budgetT4.MontantT4 = budgetT4.QuantiteARealiser * budgetT4.PU;
            }
            else if (avancement.PourcentageSousDetailAvance.HasValue)
            {
                budgetT4.QuantiteARealiser = budgetT4.QuantiteARealiser - (budgetT4.QuantiteARealiser * avancement.PourcentageSousDetailAvance.Value / 100);
                budgetT4.MontantT4 = budgetT4.QuantiteARealiser * budgetT4.PU;
            }
            foreach (var sd in budgetT4.BudgetSousDetails)
            {
                sd.Quantite = budgetT4.QuantiteARealiser / quantiteARealiserInitiale * sd.Quantite;
                sd.Montant = budgetT4.MontantT4 / montant * sd.Montant;
                budgetSousDetailManager.UpdateSousDetail(sd);
            }
            if (budgetT4.MontantT4 != montant)
            {
                budgetT4Manager.Update(budgetT4);
            }
        }

        /// <inheritdoc />
        public ListPeriodeRecalageModel LoadPeriodeRecalage(int ciId)
        {
            var budgetEnt = budgetManager.GetBudgetEnApplication(ciId);
            if (budgetEnt == null)
            {
                return new ListPeriodeRecalageModel { Erreur = FeatureBudget.Budget_Recalage_Error_AucunBudgetEnApplication };
            }

            var listPeriodeCBValides = controleBudgetaireManager.GetListPeriodeControleBudgetaireValide(budgetEnt.BudgetId);
            if (listPeriodeCBValides.Count == 0)
            {
                return new ListPeriodeRecalageModel { Erreur = FeatureBudget.Budget_Recalage_Error_AucunControleBudgetaireValide };
            }

            var minPeriode = listPeriodeCBValides.Min();
            var listDateCloture = datesClotureComptableManager.GetListDatesClotureComptableByCiGreaterThanPeriode(ciId, minPeriode)
                                                               .Where(x => x.DateCloture != null)
                                                               .ToList();
            var listPeriodesRecalage = new List<string>();
            foreach (var periode in listPeriodeCBValides)
            {
                var dateCloture = listDateCloture.FirstOrDefault(x => x.Annee == periode / 100 && x.Mois == periode % 100);
                if (dateCloture != null)
                {
                    listPeriodesRecalage.Add(dateCloture.Mois.ToString().PadLeft(2, '0') + "/" + dateCloture.Annee.ToString());
                }
            }
            if (!listPeriodesRecalage.Any())
            {
                return new ListPeriodeRecalageModel { Erreur = FeatureBudget.Budget_Recalage_Error_AucunControleBudgetaireValideSurPeriodeCloture };
            }

            return new ListPeriodeRecalageModel { BudgetId = budgetEnt.BudgetId, Periodes = listPeriodesRecalage };
        }
        #endregion //Recalage
    }
}
