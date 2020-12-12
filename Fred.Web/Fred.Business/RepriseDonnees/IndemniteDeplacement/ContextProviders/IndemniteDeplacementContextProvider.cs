using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Models;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.ContextProviders
{
    public class IndemniteDeplacementContextProvider : IIndemniteDeplacementContextProvider
    {
        private readonly IRepriseDonneesRepository repriseDonneesRepository;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IUtilisateurManager utilisateurManager;

        public IndemniteDeplacementContextProvider(
            IRepriseDonneesRepository repriseDonneesRepository,
            IOrganisationTreeService organisationTreeService,
            IUtilisateurManager utilisateurManager)
        {
            this.repriseDonneesRepository = repriseDonneesRepository;
            this.organisationTreeService = organisationTreeService;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des Indémnités de Déplacement
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelIndemniteDeplacement">repriseExcelIndemniteDeplacement</param>
        /// <returns>Les données necessaires a l'import des Indémnités de Déplacement</returns>
        public ContextForImportIndemniteDeplacement GetContextForImportIndemniteDeplacement(int groupeId, List<RepriseExcelIndemniteDeplacement> repriseExcelIndemniteDeplacement)
        {
            ContextForImportIndemniteDeplacement result = new ContextForImportIndemniteDeplacement();

            result.GroupeId = groupeId;

            result.OrganisationTree = organisationTreeService.GetOrganisationTree();

            result.FredIeUser = utilisateurManager.GetByLogin("fred_ie");
            // Doit on isoler les societé en 3 sous listes pour les besoin du validatorService ??
            List<string> societeCodes = repriseExcelIndemniteDeplacement.Select(x => x.SocieteCodeDeplacement)
                                            .Concat(repriseExcelIndemniteDeplacement.Select(x => x.SocieteCI)
                                                .Concat(repriseExcelIndemniteDeplacement.Select(x => x.SocietePersonnel)))
                                        .Distinct().ToList();
            result.SocietesUsedInExcel = repriseDonneesRepository.GetListSocieteByGroupAndCodes(groupeId, societeCodes);

            List<string> codeDeplacements = repriseExcelIndemniteDeplacement.Select(x => x.CodeDeplacement).Distinct().ToList();
            List<string> societeCodesDep = repriseExcelIndemniteDeplacement.Select(x => x.SocieteCodeDeplacement).Distinct().ToList();
            result.CodesDeplacementUsedInExcel = repriseDonneesRepository.GetListCodeDeplacementBySocietesAndCodes(societeCodesDep, codeDeplacements);

            List<string> codeZoneDeplacements = repriseExcelIndemniteDeplacement.Select(x => x.CodeZoneDeplacement).Distinct().ToList();
            result.CodesZoneDeplacementUsedInExcel = repriseDonneesRepository.GetListCodeZoneDeplacementBySocietesAndCodes(societeCodesDep, codeZoneDeplacements);

            List<string> personnelMatricules = repriseExcelIndemniteDeplacement.Select(x => x.MatriculePersonnel).Distinct().ToList();
            List<string> societePersonnelCodes = repriseExcelIndemniteDeplacement.Select(x => x.SocietePersonnel).Distinct().ToList();
            result.PersonnelsUsedInExcel = repriseDonneesRepository.GetPersonnelsByCodesAndBySocietes(personnelMatricules, societePersonnelCodes);

            List<string> ciCodes = repriseExcelIndemniteDeplacement.Select(x => x.CodeCI).Distinct().ToList();
            result.CIsUsedInExcel = repriseDonneesRepository.GetCisByCodesWithSocieteAndOrganisation(ciCodes);

            result.IndemniteDeplacementUsedInExcel = repriseDonneesRepository.GetIndemniteDeplacementsByCIAndPersonnel(ciCodes, personnelMatricules);

            return result;
        }
    }
}
