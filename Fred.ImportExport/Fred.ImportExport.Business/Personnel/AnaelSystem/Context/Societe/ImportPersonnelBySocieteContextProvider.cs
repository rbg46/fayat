using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.Referential;
using Fred.Business.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Anael;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Input;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Logger;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Models.Personnel;
using ICommonAnaelSystemContextProvider = Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common.ICommonAnaelSystemContextProvider;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context
{
    public class ImportPersonnelBySocieteContextProvider : IImportPersonnelBySocieteContextProvider
    {
        private readonly ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider;
        private readonly ITranspoCodeEmploiToRessourceRepository transpoCodeEmploiToRessourceRepository;
        private readonly IReferentielFixeManager referentielFixeManager;
        private readonly IEtablissementPaieManager etablissementPaieManager;
        private readonly IOrganisationTreeService organisationTreeService;

        public ImportPersonnelBySocieteContextProvider(
            ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider,
            ITranspoCodeEmploiToRessourceRepository transpoCodeEmploiToRessourceRepository,
            IReferentielFixeManager referentielFixeManager,
            IEtablissementPaieManager etablissementPaieManager,
            IOrganisationTreeService organisationTreeService)
        {
            this.commonAnaelSystemContextProvider = commonAnaelSystemContextProvider;
            this.transpoCodeEmploiToRessourceRepository = transpoCodeEmploiToRessourceRepository;
            this.referentielFixeManager = referentielFixeManager;
            this.etablissementPaieManager = etablissementPaieManager;
            this.organisationTreeService = organisationTreeService;
        }

        public ImportPersonnelContext<ImportPersonnelsBySocieteInput> GetContext(ImportPersonnelsBySocieteInput input, PersonnelImportExportLogger logger)
        {
            var result = new ImportPersonnelContext<ImportPersonnelsBySocieteInput>();

            result.Input = input;

            result.TypeSocietes = commonAnaelSystemContextProvider.GetTypeSocietes();

            result.SocietesNeeded = new List<SocieteEnt>() { input.Societe };

            var organisationTree = this.organisationTreeService.GetOrganisationTree();

            var groupeParent = organisationTree.GetGroupeParentOfSociete(input.Societe.SocieteId);

            //les ressources sont mis dans le context car elle sont vallable pour le groupe et non la societe
            result.Ressoures = this.referentielFixeManager.GetRessourceListByGroupeId(groupeParent.Id).ToList();

            var societeContext = new ImportPersonnelSocieteContext();

            societeContext.Societe = input.Societe;

            if (societeContext.Societe != null)
            {
                societeContext.TranspoCodeEmploiRessourceList = this.transpoCodeEmploiToRessourceRepository.GetList(societeContext.Societe.CodeSocietePaye).ToList();

                societeContext.EtablissementsPaies = this.etablissementPaieManager.GetEtablissementPaieBySocieteId(societeContext.Societe.SocieteId).ToList();

                societeContext.AnaelPersonnels = GetAnaelPersonnels(groupeParent.Code, societeContext.Societe, input.IsFullImport, result.Input.DateDerniereExecution);

                societeContext.SocieteGroupeParent = groupeParent;

                //log personnels anael
                logger.LogAnaelModels(societeContext.AnaelPersonnels);
                result.SocietesContexts.Add(societeContext);
            }


            return result;
        }

        private List<PersonnelAnaelModel> GetAnaelPersonnels(string groupeCode, SocieteEnt societe, bool isFullImport, DateTime? dateDerniereExecution)
        {
            var anaelCiProvider = new AnaelPersonnelProvider();
            // je recupere les si par societe, donc je filtre les ciS recuS d'anael
            return anaelCiProvider.GetPersonnelFromAnael(groupeCode, societe, isFullImport, dateDerniereExecution);
        }
    }
}
