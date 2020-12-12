using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.Referential;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.CI.AnaelSystem.Anael;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Societe.Input;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context
{
    /// <summary>
    ///  Service qui fournit les données necessaires a l'import des cis a partir d'une societe
    /// </summary>
    public class ImportCiBySocieteAnaelSystemContextProvider : IImportCiBySocieteAnaelSystemContextProvider
    {
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IEtablissementComptableManager etablissementComptableManager;
        private readonly ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider;

        public ImportCiBySocieteAnaelSystemContextProvider(
            IOrganisationTreeService organisationTreeService,
            IEtablissementComptableManager etablissementComptableManager,
            ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider)
        {
            this.organisationTreeService = organisationTreeService;
            this.etablissementComptableManager = etablissementComptableManager;
            this.commonAnaelSystemContextProvider = commonAnaelSystemContextProvider;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="input">Données d'entrées</param>
        /// <param name="logger">logger</param>       
        /// <returns>les données necessaires a l'import des cis</returns>
        public ImportCiContext<ImportCisBySocieteInputs> GetContext(ImportCisBySocieteInputs input, CiImportExportLogger logger)
        {
            var result = new ImportCiContext<ImportCisBySocieteInputs>();

            result.Input = input;

            result.TypeSocietes = commonAnaelSystemContextProvider.GetTypeSocietes();

            result.OrganisationTree = organisationTreeService.GetOrganisationTree();
            // Ici c'est une Societe mais pour garder la meme structure que les autres imports CI, je laisse une liste
            result.SocietesNeeded = new List<SocieteEnt>() { input.Societe };

            foreach (var societe in result.SocietesNeeded)
            {
                var societeContext = new ImportCiSocieteContext();

                societeContext.Societe = societe;

                societeContext.TypeSocietes = commonAnaelSystemContextProvider.GetTypeSocietes();

                if (societeContext.Societe != null)
                {
                    societeContext.EtablissementComptables = etablissementComptableManager.GetListBySocieteId(societeContext.Societe.SocieteId).ToList();

                    //log etablissement comptable de la societe
                    logger.LogEtablissementComptablesOfSociete(societeContext.Societe, societeContext.EtablissementComptables);

                    societeContext.AnaelCis = GetAnaelCis(societeContext.Societe.CodeSocieteComptable, result.Input.IsFullImport, result.Input.DateDerniereExecution);

                    //log cis anael
                    logger.LogAnaelModels(societeContext.AnaelCis);
                }
                result.SocietesContexts.Add(societeContext);
            }

            return result;

        }


        private List<CiAnaelModel> GetAnaelCis(string codeSocieteComptable, bool isFullImport, DateTime? dateDerniereExecution)
        {
            var anaelCiProvider = new AnaelCiProvider();
            // je recupere les si par societe, donc je filtre les ciS recuS d'anael
            return anaelCiProvider.GetCisFromAnael(codeSocieteComptable, isFullImport, dateDerniereExecution, applyFilterOnResult: true);
        }


    }
}
