using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.OperationDiverse.Excel;
using Fred.Entities.Organisation.Tree;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.ContextProvider
{
    public class ODContextProvider : IODContextProvider
    {
        private readonly IRepriseDonneesRepository repriseDonneesRepository;
        private readonly IRepriseODRepository repriseODRepository;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IUtilisateurManager utilisateurManager;

        public ODContextProvider(
            IRepriseDonneesRepository repriseDonneesRepository,
            IRepriseODRepository repriseODRepository,
            IOrganisationTreeService organisationTreeService,
            IUtilisateurManager utilisateurManager)
        {
            this.repriseDonneesRepository = repriseDonneesRepository;
            this.repriseODRepository = repriseODRepository;
            this.organisationTreeService = organisationTreeService;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des rapports
        /// </summary>
        /// <param name="dateComptable">date comptable recupéré de l'écran Rapprochement Comta/Gestion</param>
        /// <param name="excelODs">ExcelOdModel</param>
        /// <returns>les données necessaires a l'import des operations diverses</returns>
        public ContextForImportOD GetContextForImportOD(DateTime dateComptable, List<ExcelOdModel> excelODs)
        {
            ContextForImportOD result = new ContextForImportOD();
            result.User = utilisateurManager.GetContextUtilisateur();

            int groupeId = result.User.Personnel.Societe.GroupeId;

            result.DateComtableFromUI = new DateTime(dateComptable.Year, dateComptable.Month, 1);
            result.GroupeId = groupeId;
            result.OrganisationTree = organisationTreeService.GetOrganisationTree();
            result.SocietesOfGroupe = result.OrganisationTree.GetAllSocietesForGroupe(groupeId);

            List<int> societesIds = result.SocietesOfGroupe.Select(x => x.Id).ToList();

            List<string> ciCodes = excelODs.Select(x => x.CodeCi).ToList();
            result.CisUsedInExcel = repriseDonneesRepository.GetCisByCodesWithSocieteAndOrganisation(ciCodes);

            List<GetT3ByCodesOrDefaultRequest> tachesRequests = BuildTachesRequest(result.GroupeId, result.OrganisationTree, excelODs);
            result.TachesUsedInExcel = repriseODRepository.GetT3ByCodesOrDefault(tachesRequests);

            List<string> codeUniteList = excelODs.Select(x => x.CodeUnite).ToList();
            result.UnitsUsedInExcel = repriseODRepository.GetUniteListByCodes(codeUniteList);

            string defaultUniteCode = "FRT";
            result.DefaultUnite = repriseODRepository.GetDefaultUniteCode(defaultUniteCode);

            string defaultDeviseCode = "EUR";
            result.DefaultDevise = repriseODRepository.GetDefaultDeviseCode(defaultDeviseCode);

            List<string> codeDeviseList = excelODs.Select(x => x.CodeDevise).ToList();
            result.DevisesUsedInExcel = repriseODRepository.GetDeviseListByCodes(codeDeviseList);

            List<string> codeFamilleList = excelODs.Select(x => x.CodeFamille).ToList();
            result.FamillesOdUsedInExcel = repriseODRepository.GetFamilleODListByCodes(societesIds, codeFamilleList);

            List<string> codeRessourceList = excelODs.Select(x => x.CodeRessource).ToList();
            result.RessourcesUsedInExcel = repriseODRepository.GetRessourceListByCodes(groupeId, codeRessourceList);

            return result;
        }

        private List<GetT3ByCodesOrDefaultRequest> BuildTachesRequest(int groupeId, OrganisationTree organisationTree, List<ExcelOdModel> excelODs)
        {
            var result = new List<GetT3ByCodesOrDefaultRequest>();

            foreach (var excelOD in excelODs)
            {
                OrganisationBase ci = organisationTree.GetCi(groupeId, excelOD.CodeSociete, excelOD.CodeCi);

                if (ci != null)
                {
                    result.Add(new GetT3ByCodesOrDefaultRequest()
                    {
                        CiId = ci.Id,
                        Code = excelOD.CodeTache,
                    });
                }
            }
            return result;
        }
    }
}
