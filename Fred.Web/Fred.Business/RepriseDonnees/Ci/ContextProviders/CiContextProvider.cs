using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Ci.ContextProviders
{
    public class CiContextProvider : ICiContextProvider
    {
        private readonly IRepriseDonneesRepository repriseDonneesRepository;
        private readonly IRepriseCiRepository repriseCiRepository;
        private readonly IOrganisationTreeService organisationTreeService;


        public CiContextProvider(
            IRepriseDonneesRepository repriseDonneesRepository,
            IRepriseCiRepository repriseCiRepository,
            IOrganisationTreeService organisationTreeService)
        {
            this.repriseDonneesRepository = repriseDonneesRepository;
            this.repriseCiRepository = repriseCiRepository;
            this.organisationTreeService = organisationTreeService;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelCis">repriseExcelCis</param>
        /// <returns>les données necessaires a l'import des cis</returns>
        public ContextForImportCi GetContextForImportCis(int groupeId, List<RepriseExcelCi> repriseExcelCis)
        {

            var result = new ContextForImportCi();
            // ici je raffraichis car on peux faire plusieurs imports a la suite.
            // CE N'est pas forcement la peine car il y a deja l'attribut CacheSynchronization sur l'action Asp.net
            // le filtre suppprime le cache avant l'exection sur fred ie 
            // puis resupprime le cache sur fred ie a la fin de l'exection et lance un appel http sur fred web pour supprime le cache de fred web
            result.OrganisationTree = organisationTreeService.GetOrganisationTree(forceCreation: true);

            result.SocietesOfGroupe = result.OrganisationTree.GetAllSocietesForGroupe(groupeId);

            var ciCodes = repriseExcelCis.Select(x => x.CodeCi).ToList();

            result.CisUsedInExcel = repriseDonneesRepository.GetCisByCodes(ciCodes);

            result.PaysUsedInExcel = GetPaysUsedInImport(repriseExcelCis);

            result.PersonnelsUsedInExcel = GetPersonnelsUsedInImport(repriseExcelCis, result.SocietesOfGroupe);

            return result;
        }

        private List<PaysEnt> GetPaysUsedInImport(List<RepriseExcelCi> repriseExcelCis)
        {
            var codePays = repriseExcelCis.Select(x => x.CodePays).Distinct().ToList();

            var codePaysLivraisons = repriseExcelCis.Select(x => x.CodePaysLivraison).Distinct().ToList();

            var codePaysFacturations = repriseExcelCis.Select(x => x.CodePaysFacturation).Distinct().ToList();

            var allCodesPays = new List<string>();

            allCodesPays.AddRange(codePays);

            allCodesPays.AddRange(codePaysLivraisons);

            allCodesPays.AddRange(codePaysFacturations);

            allCodesPays = allCodesPays.Distinct().ToList();

            return repriseCiRepository.GetPaysByCodes(allCodesPays);
        }

        private List<PersonnelEnt> GetPersonnelsUsedInImport(List<RepriseExcelCi> repriseExcelCis, List<OrganisationBase> societesOfGroupe)
        {
            var matriculePersonnels = new List<string>();

            var matriculeResponsableAdministratif = repriseExcelCis.Select(x => x.MatriculeResponsableAdministratif).ToList();

            var matriculeResponsableChantier = repriseExcelCis.Select(x => x.MatriculeResponsableChantier).ToList();

            matriculePersonnels.AddRange(matriculeResponsableAdministratif);

            matriculePersonnels.AddRange(matriculeResponsableChantier);

            matriculePersonnels = matriculePersonnels.Distinct().ToList();

            var societesIds = societesOfGroupe.Select(x => x.Id).ToList();

            return repriseCiRepository.GetPersonnelListBySocieteIdsAndMatricules(societesIds, matriculePersonnels);
        }
    }
}
