using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.ReferentielFixe;
using Fred.Business.RepriseDonnees.Personnel.Models;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Personnel.ContextProdivers
{
    public class PersonnelContextProvider : IPersonnelContextProvider
    {
        private readonly IRepriseDonneesRepository repriseDonneesRepository;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IReferentielFixeManager referentielFixeManager;


        public PersonnelContextProvider(
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
        /// Fournit les données necessaires a l'import des Personnel
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelPersonnel">RepriseExcelPersonnel</param>
        /// <returns>Les données necessaires a l'import des Personnels</returns>
        public ContextForImportPersonnel GetContextForImportPersonnel(int groupeId, List<RepriseExcelPersonnel> repriseExcelPersonnel)
        {
            ContextForImportPersonnel result = new ContextForImportPersonnel();

            result.GroupeId = groupeId;

            result.OrganisationTree = organisationTreeService.GetOrganisationTree();

            result.FredIeUser = utilisateurManager.GetByLogin("fred_ie");

            List<string> paysCodes = repriseExcelPersonnel.Select(x => x.CodePays).Distinct().ToList();
            result.PaysUsedInExcel = repriseDonneesRepository.GetPaysByCodes(paysCodes);

            List<string> personnelMatricules = repriseExcelPersonnel.Select(x => x.Matricule).Distinct().ToList();
            List<string> societeCodes = repriseExcelPersonnel.Select(x => x.CodeSociete).Distinct().ToList();
            result.PersonnelsUsedInExcel = repriseDonneesRepository.GetPersonnelsByCodesAndBySocietes(personnelMatricules, societeCodes);

            List<string> ressourceCodes = repriseExcelPersonnel.Select(x => x.CodeRessource).Distinct().ToList();
            result.RessourcesUsedInExcel = referentielFixeManager.GetRessourceListByGroupeId(result.GroupeId).Where(x => ressourceCodes.Contains(x.Code)).ToList();

            result.SocietesUsedInExcel = repriseDonneesRepository.GetListSocieteByGroupAndCodes(groupeId, societeCodes);

            List<string> adressesEmail = repriseExcelPersonnel.Select(x => x.Email).Distinct().ToList();
            result.MailUsedInExcel = repriseDonneesRepository.GetPersonnelsByEmails(adressesEmail).Select(p => p.Email).Distinct().ToList();

            return result;
        }
    }
}
