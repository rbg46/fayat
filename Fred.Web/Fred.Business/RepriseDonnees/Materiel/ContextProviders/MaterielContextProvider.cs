using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.ReferentielFixe;
using Fred.Business.RepriseDonnees.Materiel.Models;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Materiel.ContextProviders
{
    public class MaterielContextProvider : IMaterielContextProvider
    {
        private readonly IRepriseDonneesRepository repriseDonneesRepository;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IReferentielFixeManager referentielFixeManager;


        public MaterielContextProvider(
            IRepriseDonneesRepository repriseDonneesRepository,
            IOrganisationTreeService organisationTreeService,
            IUtilisateurManager utilisateurManager,
            IReferentielFixeManager referentielFixeManager)
        {
            this.repriseDonneesRepository = repriseDonneesRepository;
            this.organisationTreeService = organisationTreeService;
            this.utilisateurManager = utilisateurManager;
            this.referentielFixeManager = referentielFixeManager;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des Materiels
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelMateriel">repriseExcelMateriel</param>
        /// <returns>les données necessaires a l'import des Materiels</returns>
        public ContextForImportMateriel GetContextForImportMateriel(int groupeId, List<RepriseExcelMateriel> repriseExcelMateriel)
        {
            ContextForImportMateriel result = new ContextForImportMateriel();

            result.GroupeId = groupeId;

            result.OrganisationTree = organisationTreeService.GetOrganisationTree();

            result.FredIeUser = utilisateurManager.GetByLogin("fred_ie");

            List<string> ressourceCodes = repriseExcelMateriel.Select(x => x.CodeRessource).Distinct().ToList();
            result.RessourcesUsedInExcel = referentielFixeManager.GetRessourceListByGroupeId(result.GroupeId).Where(x => ressourceCodes.Contains(x.Code)).ToList();

            List<string> societeCodes = repriseExcelMateriel.Select(x => x.CodeSociete).Distinct().ToList();
            result.SocietesUsedInExcel = repriseDonneesRepository.GetListSocieteByGroupAndCodes(groupeId, societeCodes);

            List<string> materielCodes = repriseExcelMateriel.Select(x => x.CodeMateriel).Distinct().ToList();
            result.MaterielsUsedInExcel = repriseDonneesRepository.GetListMaterielBySocieteAndCodes(societeCodes, materielCodes);

            return result;
        }
    }
}
