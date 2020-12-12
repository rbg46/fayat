using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.Personnel;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Entities.Personnel;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Personnel.Input;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Logger;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context
{
    public class ImportByPersonnelListContextProvider : IImportByPersonnelListContextProvider
    {
        private readonly ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider;
        private readonly ISocieteManager societeManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IEtablissementPaieManager etablissementPaieManager;
        private readonly IOrganisationTreeService organisationTreeService;

        public ImportByPersonnelListContextProvider(
            ICommonAnaelSystemContextProvider commonAnaelSystemContextProvider,
            ISocieteManager societeManager,
            IPersonnelManager personnelManager,
            IEtablissementPaieManager etablissementPaieManager,
            IOrganisationTreeService organisationTreeService)
        {
            this.commonAnaelSystemContextProvider = commonAnaelSystemContextProvider;
            this.societeManager = societeManager;
            this.personnelManager = personnelManager;
            this.etablissementPaieManager = etablissementPaieManager;
            this.organisationTreeService = organisationTreeService;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des personnels
        /// </summary>
        /// <param name="input">Données d'entrées</param>
        /// <param name="logger">logger</param>       
        /// <returns>les données necessaires a l'import des personnels</returns>
        public ImportPersonnelContext<ImportByPersonnelListInputs> GetContext(ImportByPersonnelListInputs input, PersonnelImportExportLogger logger)
        {
            var allFredPersonnels = personnelManager.GetPersonnelsByIds(input.PersonnelIds);

            var result = new ImportPersonnelContext<ImportByPersonnelListInputs>
            {
                Input = input,

                SocietesNeeded = GetSocietesNeeded(allFredPersonnels),

                TypeSocietes = commonAnaelSystemContextProvider.GetTypeSocietes()

            };

            var organisationTree = this.organisationTreeService.GetOrganisationTree();

            foreach (var societe in result.SocietesNeeded)
            {
                var societeContext = new ImportPersonnelSocieteContext
                {
                    FredPersonnels = allFredPersonnels.Where(p => p.SocieteId == societe.SocieteId).ToList(),

                    Societe = societe,

                    EtablissementsPaies = etablissementPaieManager.GetEtablissementPaieBySocieteId(societe.SocieteId).ToList(),

                    SocieteGroupeParent = organisationTree.GetGroupeParentOfSociete(societe.SocieteId)
                };

                result.SocietesContexts.Add(societeContext);
            }

            return result;
        }

        private List<SocieteEnt> GetSocietesNeeded(List<PersonnelEnt> personnels)
        {
            List<int> societesNeededIds = new List<int>();

            personnels.ForEach(x => societesNeededIds.Add(x.SocieteId ?? 0));

            return societeManager.GetAllSocietesByIds(societesNeededIds).Distinct().ToList();
        }
    }
}
