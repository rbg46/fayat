using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI;
using Fred.Business.Organisation.Tree;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.CI.AnaelSystem.Anael;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Ci.Input;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des cis a partir d'une liste de ciId
    /// </summary>
    public class ImportCiByCiListAnaelSystemContextProvider : IImportCiByCiListAnaelSystemContextProvider
    {
        private readonly ISocieteManager societeManager;
        private readonly ICIManager ciManager;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IEtablissementComptableManager etablissementComptableManager;
        private readonly ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider;

        public ImportCiByCiListAnaelSystemContextProvider(
            ISocieteManager societeManager,
            ICIManager ciManager,
            IOrganisationTreeService organisationTreeService,
            IEtablissementComptableManager etablissementComptableManager,
            ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider)
        {
            this.societeManager = societeManager;
            this.ciManager = ciManager;
            this.organisationTreeService = organisationTreeService;
            this.etablissementComptableManager = etablissementComptableManager;
            this.commonAnaelSystemContextProvider = commonAnaelSystemContextProvider;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="input">Données d'entrées</param>
        /// <param name="logger">LOGGER</param>       
        /// <returns>les données necessaires a l'import des cis</returns>
        public ImportCiContext<ImportCisByCiListInputs> GetContext(ImportCisByCiListInputs input, CiImportExportLogger logger)
        {
            var result = new ImportCiContext<ImportCisByCiListInputs>();

            result.Input = input;

            result.OrganisationTree = organisationTreeService.GetOrganisationTree();

            result.SocietesNeeded = GetSocietesNeeded(result.OrganisationTree, input.CiIds);

            var allFredCis = ciManager.GetCisByIds(input.CiIds);

            foreach (var societe in result.SocietesNeeded)
            {
                var societeContext = new ImportCiSocieteContext();

                var ciIdsOfSociete = result.OrganisationTree.GetAllCisOfSociete(societe.SocieteId).Select(x => x.Id).ToList();

                var cisOfSociete = allFredCis.Where(x => ciIdsOfSociete.Contains(x.CiId)).ToList();

                societeContext.Societe = societe;

                societeContext.TypeSocietes = commonAnaelSystemContextProvider.GetTypeSocietes();

                if (societeContext.Societe != null)
                {
                    societeContext.EtablissementComptables = etablissementComptableManager.GetListBySocieteId(societeContext.Societe.SocieteId).ToList();

                    //log etablissement comptable de la societe
                    logger.LogEtablissementComptablesOfSociete(societeContext.Societe, societeContext.EtablissementComptables);

                    societeContext.AnaelCis = GetAnaelCis(societeContext.Societe.CodeSocieteComptable, cisOfSociete.Select(x => x.Code).ToList());

                    //log cis anael
                    logger.LogAnaelModels(societeContext.AnaelCis);

                }
                result.SocietesContexts.Add(societeContext);
            }

            return result;

        }

        /// <summary>
        /// Je recuper les societes necessaire a l'import etant donné que les flux sont par societes
        /// </summary>
        /// <param name="organisationTree">L'arbre</param>
        /// <param name="ciIds">La liste de ci</param>
        /// <returns>Les societes des cis</returns>
        private List<SocieteEnt> GetSocietesNeeded(OrganisationTree organisationTree, List<int> ciIds)
        {
            var societeOrganisationBases = new List<OrganisationBase>();

            foreach (var ciId in ciIds)
            {
                societeOrganisationBases.Add(organisationTree.GetSocieteParentOfCi(ciId));
            }

            var societeIds = societeOrganisationBases.Select(x => x.Id).Distinct().ToList();

            return societeManager.GetAllSocietesByIds(societeIds);
        }

        private List<CiAnaelModel> GetAnaelCis(string codeSocieteComptable, List<string> codeCisOfSociete)
        {
            var anaelCiProvider = new AnaelCiProvider();

            // je recupere les cis par 'liste de cis', donc je ne filtre les cis recus d'anael, 
            // en effet le ci a pu etre importé par fichier excel il faut donc qu'il soit possible de le mettre a jour
            return anaelCiProvider.GetCisFromAnael(codeSocieteComptable, codeCisOfSociete, applyFilterOnResult: false);
        }

    }
}
