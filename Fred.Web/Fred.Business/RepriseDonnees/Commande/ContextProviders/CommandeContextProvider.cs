using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.ReferentielFixe;
using Fred.Business.RepriseDonnees.Commande.Models;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Commande.ContextProviders
{
    public class CommandeContextProvider : ICommandeContextProvider
    {
        private readonly IRepriseDonneesRepository repriseDonneesRepository;
        private readonly IRepriseCommandeRepository repriseCommandeRepository;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IReferentielFixeManager referentielFixeManager;

        public CommandeContextProvider(
            IRepriseDonneesRepository repriseDonneesRepository,
            IRepriseCommandeRepository repriseCommandeRepository,
            IOrganisationTreeService organisationTreeService,
            IUtilisateurManager utilisateurManager,
            IReferentielFixeManager referentielFixeManager)
        {
            this.repriseDonneesRepository = repriseDonneesRepository;
            this.repriseCommandeRepository = repriseCommandeRepository;
            this.organisationTreeService = organisationTreeService;
            this.utilisateurManager = utilisateurManager;
            this.referentielFixeManager = referentielFixeManager;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelCommandes">repriseExcelCis</param>
        /// <returns>les données necessaires a l'import des cis</returns>
        public ContextForImportCommande GetContextForImportCommandes(int groupeId, List<RepriseExcelCommande> repriseExcelCommandes)
        {
            var result = new ContextForImportCommande();

            result.GroupeId = groupeId;

            result.OrganisationTree = organisationTreeService.GetOrganisationTree();

            result.FredIeUser = utilisateurManager.GetByLogin("fred_ie");

            var numeroCommandeExternes = repriseExcelCommandes.Select(x => x.NumeroCommandeExterne).Distinct().ToList();

            result.CommandesUsedInExcel = repriseCommandeRepository.GetCommandes(numeroCommandeExternes);

            result.AllCommandesTypes = repriseCommandeRepository.GetCommandesTypes();

            result.StatutCommandeValidee = repriseDonneesRepository.GetStatusCommandeValidee();

            var fournisseurCodes = repriseExcelCommandes.Select(x => x.CodeFournisseur).Distinct().ToList();

            result.FournisseurUsedInExcel = repriseCommandeRepository.GetFournisseurByGroupeAndCodes(groupeId, fournisseurCodes);

            var codeDevises = repriseExcelCommandes.Select(x => x.CodeDevise).Distinct().ToList();

            result.DevisesUsedInExcel = repriseCommandeRepository.GetDeviseByCodes(codeDevises);

            var ciCodes = repriseExcelCommandes.Select(x => x.CodeCi).Distinct().ToList();

            result.CisUsedInExcel = repriseDonneesRepository.GetCisByCodes(ciCodes);

            var tachesRequests = BuildTachesRequest(result.GroupeId, result.OrganisationTree, repriseExcelCommandes);

            result.TachesUsedInExcel = repriseCommandeRepository.GetT3ByCodesOrDefault(tachesRequests);

            var ressourcesCodes = repriseExcelCommandes.Select(x => x.CodeRessource).Distinct().ToList();

            result.RessourcesUsedInExcel = referentielFixeManager.GetRessourceListByGroupeId(result.GroupeId).Where(x => ressourcesCodes.Contains(x.Code)).ToList();

            var unitesCodes = repriseExcelCommandes.Select(x => x.Unite).Distinct().ToList();

            result.UnitesUsedInExcel = repriseCommandeRepository.GetUnitesByCodes(unitesCodes).ToList();

            result.DepenseTypeReception = repriseCommandeRepository.GetDepenseTypeReception();

            return result;
        }

        private List<GetT3ByCodesOrDefaultRequest> BuildTachesRequest(int groupeId, OrganisationTree organisationTree, List<RepriseExcelCommande> repriseExcelCommandes)
        {

            var result = new List<GetT3ByCodesOrDefaultRequest>();

            foreach (var repriseExcelCommande in repriseExcelCommandes)
            {
                OrganisationBase ci = organisationTree.GetCi(groupeId, repriseExcelCommande.CodeSociete, repriseExcelCommande.CodeCi);

                if (ci != null)
                {
                    result.Add(new GetT3ByCodesOrDefaultRequest()
                    {
                        CiId = ci.Id,
                        Code = repriseExcelCommande.CodeTache,
                    });
                }
            }
            return result;

        }

    }
}
