using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Business.BaremeExploitation;
using Fred.Business.Budget;
using Fred.Business.CI;
using Fred.Business.Organisation.Tree;
using Fred.Business.ReferentielEtendu;
using Fred.Business.ReferentielFixe;
using Fred.Entities.Bareme;
using Fred.Entities.Budget;
using Fred.Entities.CI;
using Fred.Entities.ObjectifFlash.Panel;

namespace Fred.Business.ObjectifFlash
{
    public class ObjectifFlashBudgetManager : Manager, IObjectifFlashBudgetManager
    {
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IMapper mapper;
        private readonly IBudgetManager budgetManager;
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly IBudgetSousDetailManager budgetSousDetailManager;
        private readonly IReferentielEtenduManager referentielEtenduManager;
        private readonly IBudgetMainManager budgetMainManager;
        private readonly IBaremeExploitationCIManager baremeExploitationCiManager;
        private readonly IBaremeExploitationOrganisationManager baremeExploitationOrganisationManager;
        private readonly ICIManager ciManager;
        private readonly IReferentielFixeManager referentielFixeManager;

        public ObjectifFlashBudgetManager(
            IOrganisationTreeService organisationTreeService,
            IMapper mapper,
            IBudgetManager budgetManager,
            IBudgetT4Manager budgetT4Manager,
            IBudgetSousDetailManager budgetSousDetailManager,
            IReferentielEtenduManager referentielEtenduManager,
            IBudgetMainManager budgetMainManager,
            IBaremeExploitationCIManager baremeExploitationCiManager,
            IBaremeExploitationOrganisationManager baremeExploitationOrganisationManager,
            ICIManager ciManager,
            IReferentielFixeManager referentielFixeManager)
        {
            this.organisationTreeService = organisationTreeService;
            this.mapper = mapper;
            this.budgetManager = budgetManager;
            this.budgetT4Manager = budgetT4Manager;
            this.budgetSousDetailManager = budgetSousDetailManager;
            this.referentielEtenduManager = referentielEtenduManager;
            this.budgetMainManager = budgetMainManager;
            this.baremeExploitationCiManager = baremeExploitationCiManager;
            this.baremeExploitationOrganisationManager = baremeExploitationOrganisationManager;
            this.ciManager = ciManager;
            this.referentielFixeManager = referentielFixeManager;
        }

        /// <summary>
        /// Récupère les ressources liée à une tache dans un budget en application pour l'identifiant de ci donné 
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="tacheId">Tache identifier</param>
        /// <returns>Http response</returns>
        public List<ChapitrePanelEnt> GetRessourcesInBudgetEnApplicationByCiId(int ciId, int tacheId)
        {
            // Récupération de la liste des sous details de budget
            var budgetSousDetails = new List<BudgetSousDetailEnt>();
            var budget = budgetManager.GetBudgetEnApplicationIncludeDevise(ciId);
            var etablissementComptableEnt = ciManager.GetEtablissementComptableByCIId(ciId);

            var referentielEtenduRecommandeeList = new List<int>();
            if (etablissementComptableEnt != null)
            {
                referentielEtenduRecommandeeList = referentielFixeManager.GetRessourceRecommandeeList(etablissementComptableEnt.Organisation.OrganisationId).Select(x => x.ReferentielEtenduId).ToList();
            }
            var chapitresPanel = new List<ChapitrePanelEnt>();

            if (budget == null)
            {
                return chapitresPanel;
            }
            else
            {
                var lisfOfT4Buget = budgetT4Manager.GetByBudgetIdAndTache3Id(budget.BudgetId, tacheId);

                foreach (BudgetT4Ent budgetT4 in lisfOfT4Buget)
                {
                    budgetSousDetails.AddRange(budgetSousDetailManager.GetByBudgetT4IdIncludeRessource(budgetT4.BudgetT4Id));
                }

                // recherche du ref etendu pour la liste de ressources du budget en application
                List<int> listRessourceIds = budgetSousDetails.Select(x => x.RessourceId).Distinct().ToList();
                chapitresPanel = mapper.Map<List<ChapitrePanelEnt>>(referentielEtenduManager.GetReferentielEtenduAsChapitreListLightByRessourceIdList(listRessourceIds));

                foreach (ChapitrePanelEnt chapitre in chapitresPanel)
                {
                    foreach (SousChapitrePanelEnt sousChapitre in chapitre.SousChapitres)
                    {
                        foreach (RessourcePanelEnt ressource in sousChapitre.Ressources)
                        {
                            ressource.IsRecommandee = ressource.ReferentielEtendus.Any(x => !referentielEtenduRecommandeeList.Any() || referentielEtenduRecommandeeList.Contains(x.ReferentielEtenduId));
                            ressource.ChapitreCode = chapitre.Code;
                            var ressourceBudget = budgetSousDetails.Find(x => x.RessourceId == ressource.RessourceId);
                            ressource.PuHT = ressourceBudget?.PU ?? 0;
                            ressource.Unite = ressourceBudget?.Unite;
                        }
                    }
                }
            }
            return chapitresPanel;
        }

        /// <summary>
        /// Récupère les ressources liée à une tache dans un budget en application pour l'identifiant de ci donné 
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="periode">periode basé sur la date de début de l'objectif flash</param>
        /// <returns>Http response</returns>
        public List<ChapitrePanelEnt> GetRessourcesInBaremeExploitation(int ciId, DateTime periode)
        {
            var ci = ciManager.GetCiById(ciId, true);

            var baremesCI = baremeExploitationCiManager.GetBaremeByCIIdAndPeriode(periode, ci.CiId);
            var baremesOrganisation = new List<BaremeExploitationOrganisationEnt>();
            var etablissementComptableEnt = ciManager.GetEtablissementComptableByCIId(ciId);
            var referentielEtenduRecommandeeList = new List<int>();
            if (etablissementComptableEnt != null)
            {
                referentielEtenduRecommandeeList = referentielFixeManager.GetRessourceRecommandeeList(etablissementComptableEnt.Organisation.OrganisationId).Select(x => x.ReferentielEtenduId).ToList();
            }

            if (baremesCI == null || baremesCI.Count == 0)
            {
                int organisationId = (int)ciManager.GetOrganisationIdByCiId(ci.CiId);
                baremesOrganisation = GetBaremeExploitationOrganisationByOrganisationId(organisationId, periode);
            }

            if ((baremesCI != null && baremesCI.Count > 0) || (baremesOrganisation.Count > 0))
            {
                return FillRessourcesPrixWithBaremeExploitation(ci, baremesCI, baremesOrganisation, referentielEtenduRecommandeeList);
            }
            return Enumerable.Empty<ChapitrePanelEnt>().ToList();
        }

        private List<BaremeExploitationOrganisationEnt> GetBaremeExploitationOrganisationByOrganisationId(int organisationId, DateTime periode)
        {
            var organisationTree = organisationTreeService.GetOrganisationTree();

            var organisationParent = organisationTree.GetFirstParent(organisationId);

            if (organisationParent == null)
            {
                return new List<BaremeExploitationOrganisationEnt>();
            }
            List<BaremeExploitationOrganisationEnt> baremes = baremeExploitationOrganisationManager.GetBaremesByOrganisationIdAndPeriode(periode, organisationParent.OrganisationId);
            if (baremes == null || baremes.Count == 0)
            {
                baremes = GetBaremeExploitationOrganisationByOrganisationId(organisationParent.OrganisationId, periode);
            }
            return baremes;
        }

        private List<ChapitrePanelEnt> FillRessourcesPrixWithBaremeExploitation(CIEnt ci, List<BaremeExploitationCIEnt> baremesCI, List<BaremeExploitationOrganisationEnt> baremesOrganisation, List<int> referentielEtenduRecommandeeList)
        {
            List<ChapitrePanelEnt> chapitres = mapper.Map<List<ChapitrePanelEnt>>(referentielEtenduManager.GetReferentielEtenduAsChapitreListLightBySocieteId((int)ci.SocieteId));

            foreach (ChapitrePanelEnt chapitre in chapitres)
            {
                foreach (SousChapitrePanelEnt sousChapitre in chapitre.SousChapitres)
                {
                    foreach (RessourcePanelEnt ressource in sousChapitre.Ressources)
                    {
                        ressource.ChapitreCode = chapitre.Code;
                        ressource.IsRecommandee = ressource.ReferentielEtendus.Any(x => !referentielEtenduRecommandeeList.Any() || referentielEtenduRecommandeeList.Contains(x.ReferentielEtenduId));

                        if (baremesCI != null && baremesCI.Where(b => b.ReferentielEtenduId == ressource.ReferentielEtendus.FirstOrDefault().ReferentielEtenduId) != null)
                        {
                            var referentielEtenduId = ressource.ReferentielEtendus.FirstOrDefault()?.ReferentielEtenduId;
                            var baremeCI = baremesCI.FirstOrDefault(b => b.ReferentielEtenduId == referentielEtenduId);
                            ressource.PuHT = baremeCI?.Prix ?? 0;
                            ressource.Unite = baremeCI?.Unite;
                        }
                        else if (baremesOrganisation != null && baremesOrganisation.Where(b => b.RessourceId == ressource.RessourceId) != null)
                        {
                            var baremeOrganisation = baremesOrganisation.FirstOrDefault(b => b.RessourceId == ressource.RessourceId);
                            ressource.PuHT = baremeOrganisation?.Prix ?? 0;
                            ressource.Unite = baremeOrganisation?.Unite;
                        }
                    }
                }
            }

            return chapitres;
        }

        /// <summary>
        /// Récupère les ressources de la bibliotheque des prix du ci  
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="deviseId">devise identifier</param>
        /// <param name="filter">filtre de recherche</param>
        /// <param name="page">numero de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Http response</returns>
        public List<ChapitrePanelEnt> GetRessourcesInBibliothequePrix(int ciId, int deviseId, string filter, int page, int pageSize)
        {
            List<ChapitrePanelEnt> chapitres = mapper.Map<List<ChapitrePanelEnt>>(this.budgetMainManager.GetChapitres(ciId, deviseId, filter, page, pageSize).ToList());

            foreach (ChapitrePanelEnt chapitre in chapitres)
            {
                foreach (SousChapitrePanelEnt sousChapitre in chapitre.SousChapitres)
                {
                    foreach (RessourcePanelEnt ressource in sousChapitre.Ressources)
                    {
                        ressource.ChapitreCode = chapitre.Code;
                    }
                }
            }

            return chapitres;
        }
    }
}
