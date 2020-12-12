using Fred.Business.Budget;
using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.Details;
using Fred.Business.Budget.Recette;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Depense.Services;
using Fred.Business.Params;
using Fred.Business.Referential;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielEtendu;
using Fred.Business.ReferentielFixe;
using Fred.Business.Societe;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Referential.Common;

namespace Fred.Business.Tests.Budget
{
    public class BudgetMainManagerTest : BudgetMainManager
    {
        public BudgetMainManagerTest(
            IAvancementManager avancementManager,
            IAvancementRecetteManager avancementRecetteManager,
            IAvancementEtatManager avancementEtatManager,
            IAvancementWorkflowManager avancementWorkflowManager,
            IBudgetManager budgetManager,
            IBudgetEtatManager budgetEtatManager,
            IBudgetSousDetailManager budgetSousDetailManager,
            IBudgetT4Manager budgetT4Manager,
            IBudgetTacheManager budgetTacheManager,
            IDatesClotureComptableManager datesClotureComptableManager,
            ITacheManager tacheManager,
            ICIManager ciMgr,
            IReferentielEtenduManager referentielEtenduManager,
            IReferentielFixeManager referentielFixeMgr,
            IUtilisateurManager utilisateurMgr,
            ISocieteManager societeManager,
            IUniteManager uniteManager,
            AvancementLoader avancementLoader,
            AvancementRecetteLoader avancementRecetteLoader,
            IControleBudgetaireManager controleBudgetaireManager,
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
            IBudgetWorkflowManager budgetWorkflowManager,
            IParamsManager paramsManager,
            IDeviseManager deviseManager,
            IRessourceValidator ressourceValidator)
            : base(
                avancementLoader,
                avancementRecetteLoader,
                uow,
                tacheRepository,
                taskSearchHelper,
                ressourceRepository,
                budgetBibliothequePrixItemRepository,
                ciRepository,
                budgetT4Repository,
                detailBudgetExportExcel,
                ciManager,
                depenseServiceMediator,
                budgetCopieManager,
                budgetEtatManager,
                budgetTacheManager,
                budgetSousDetailManager,
                avancementWorkflowManager,
                avancementRecetteManager,
                avancementEtatManager,
                avancementManager,
                budgetT4Manager,
                budgetWorkflowManager,
                budgetManager,
                controleBudgetaireManager,
                paramsManager,
                uniteManager,
                referentielEtenduManager,
                datesClotureComptableManager,
                tacheManager,
                deviseManager,
                ressourceValidator,
                societeManager)
        {
            Managers = new ManagersTest(
                ciMgr,
                referentielFixeMgr,
                utilisateurMgr);
        }
    }

    internal class ManagersTest : Managers
    {
        public ManagersTest(
            ICIManager ciMgr,
            IReferentielFixeManager referentielFixeMgr,
            IUtilisateurManager utilisateurMgr)
        {
            CI = ciMgr;
            ReferentielFixe = referentielFixeMgr;
            Utilisateur = utilisateurMgr;
        }
    }
}
