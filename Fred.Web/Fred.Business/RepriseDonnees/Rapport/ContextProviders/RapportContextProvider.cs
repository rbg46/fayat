using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Rapport.ContextProviders
{
    public class RapportContextProvider : IRapportContextProvider
    {
        private readonly IRepriseDonneesRepository repriseDonneesRepository;
        private readonly IRepriseRapportRepository repriseRapportRepository;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IUtilisateurManager utilisateurManager;

        public RapportContextProvider(
            IRepriseDonneesRepository repriseDonneesRepository,
            IRepriseRapportRepository repriseRapportRepository,
            IOrganisationTreeService organisationTreeService,
            IUtilisateurManager utilisateurManager)
        {
            this.repriseDonneesRepository = repriseDonneesRepository;
            this.repriseRapportRepository = repriseRapportRepository;
            this.organisationTreeService = organisationTreeService;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des rapports
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelRapports">repriseExcelRapports</param>
        /// <returns>les données necessaires a l'import des rapports</returns>
        public ContextForImportRapport GetContextForImportRapports(int groupeId, List<RepriseExcelRapport> repriseExcelRapports)
        {
            var result = new ContextForImportRapport();

            result.GroupeId = groupeId;

            result.FredIeUser = utilisateurManager.GetByLogin("fred_ie");

            result.OrganisationTree = organisationTreeService.GetOrganisationTree();
            result.SocietesOfGroupe = result.OrganisationTree.GetAllSocietesForGroupe(groupeId);

            var societesIds = result.SocietesOfGroupe.Select(x => x.Id).ToList();

            var ciCodes = repriseExcelRapports.Select(x => x.CodeCi).ToList();
            result.CisUsedInExcel = repriseDonneesRepository.GetCisByCodes(ciCodes);

            var tachesRequests = BuildTachesRequest(result.GroupeId, result.OrganisationTree, repriseExcelRapports);
            result.TachesUsedInExcel = repriseRapportRepository.GetT3ByCodesOrDefault(tachesRequests);

            var codeDeplacementList = repriseExcelRapports.Select(x => x.CodeDeplacement).ToList();
            result.CodeDeplacementsUsedInExcel = repriseRapportRepository.GetCodeDeplacementListByCodes(societesIds, codeDeplacementList);

            var codeZoneDeplacementList = repriseExcelRapports.Select(x => x.CodeZoneDeplacement).ToList();
            result.CodeZoneDeplacementsUsedInExcel = repriseRapportRepository.GetCodeZoneDeplacementListByCodes(societesIds, codeZoneDeplacementList);

            var matriculePersonnels = repriseExcelRapports.Select(x => x.MatriculePersonnel).ToList();
            result.PersonnelsUsedInExcel = repriseRapportRepository.GetPersonnelListBySocieteIdsAndMatricules(societesIds, matriculePersonnels);

            return result;
        }

        private List<GetT3ByCodesOrDefaultRequest> BuildTachesRequest(int groupeId, OrganisationTree organisationTree, List<RepriseExcelRapport> repriseExcelRapports)
        {

            var result = new List<GetT3ByCodesOrDefaultRequest>();

            foreach (var repriseExcelRapport in repriseExcelRapports)
            {
                OrganisationBase ci = organisationTree.GetCi(groupeId, repriseExcelRapport.CodeSocieteCi, repriseExcelRapport.CodeCi);

                if (ci != null)
                {
                    result.Add(new GetT3ByCodesOrDefaultRequest()
                    {
                        CiId = ci.Id,
                        Code = repriseExcelRapport.CodeTache,
                    });
                }
            }
            return result;

        }
    }
}
