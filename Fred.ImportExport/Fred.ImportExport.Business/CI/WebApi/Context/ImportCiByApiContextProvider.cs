using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.CI;
using Fred.Business.Organisation.Tree;
using Fred.Business.Personnel;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.TypeOrganisation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.CI.WebApi.Context.Inputs;
using Fred.ImportExport.Business.CI.WebApi.Context.Models;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.WebApi.Context
{
    public class ImportCiByApiContextProvider : IImportCiByApiContextProvider
    {
        private readonly ISocieteManager societeManager;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IEtablissementComptableManager etablissementComptableManager;
        private readonly ITypeOrganisationManager typeOrganisationManager;
        private readonly ICIManager ciManager;
        private readonly IPersonnelManager personnelManager;

        public ImportCiByApiContextProvider(ISocieteManager societeManager,
            IOrganisationTreeService organisationTreeService,
            IEtablissementComptableManager etablissementComptableManager,
            ITypeOrganisationManager typeOrganisationManager,
            ICIManager ciManager,
            IPersonnelManager personnelManager
            )
        {
            this.societeManager = societeManager;
            this.organisationTreeService = organisationTreeService;
            this.etablissementComptableManager = etablissementComptableManager;
            this.typeOrganisationManager = typeOrganisationManager;
            this.ciManager = ciManager;
            this.personnelManager = personnelManager;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="input">Données d'entrées</param>      
        /// <returns>les données necessaires a l'import des cis</returns>
        public ImportCiByWebApiContext GetContext(ImportCisByApiInputs input)
        {
            var result = new ImportCiByWebApiContext();

            result.Input = input;

            result.OrganisationTree = organisationTreeService.GetOrganisationTree();

            result.SocietesUsedInJson = GetSocietesUsedInJson(input.WebApiCis);

            result.SocietesOfResponsableAffairesUsedInJson = GetSocietesOfReponsableAffaireUsedInJson(input.WebApiCis);

            result.ResponsableAffairesUsedInJson = GetReponsablesAffaireUsedInJson(input.WebApiCis, result.SocietesOfResponsableAffairesUsedInJson);

            result.CiTypes = ciManager.GetCITypes().ToList();

            var organisationTypes = typeOrganisationManager.GetAll();

            var webApiCisGroupedBySocieteCode = input.WebApiCis.GroupBy(x => x.SocieteCode);

            foreach (var cis in webApiCisGroupedBySocieteCode)
            {
                var societeContext = new ImportCiByWebApiSocieteContext();

                societeContext.CodeSocieteComptable = cis.Key;

                societeContext.Societe = result.SocietesUsedInJson.FirstOrDefault(x => x.CodeSocieteComptable == cis.Key);

                societeContext.CisToImport = cis.ToList();

                societeContext.SocietesOfResponsableAffairesUsedInJson = result.SocietesOfResponsableAffairesUsedInJson;

                societeContext.ResponsableAffairesUsedInJson = result.ResponsableAffairesUsedInJson;

                societeContext.CiTypes = result.CiTypes;

                societeContext.TypeOrganisations = organisationTypes;

                if (societeContext.Societe != null)
                {
                    societeContext.EtablissementComptables = etablissementComptableManager.GetListBySocieteId(societeContext.Societe.SocieteId).ToList();

                    var ciCodes = cis.Select(x => x.Code).Distinct().ToList();

                    societeContext.CisFoundInFredWithCode = GetFredCis(ciCodes.Distinct().ToList());

                }

                result.SocietesContexts.Add(societeContext);
            }

            return result;
        }

        private List<CIEnt> GetFredCis(List<string> ciCodes)
        {
            var filters = new List<Expression<Func<CIEnt, bool>>>
            {
                x => ciCodes.Contains(x.Code)
            };
            List<Expression<Func<CIEnt, object>>> includePorperties = new List<Expression<Func<CIEnt, object>>>
            {
                x => x.Organisation
            };
            var allCisByCodes = ciManager.Search(filters: filters, includeProperties: includePorperties);

            return allCisByCodes.ToList();

        }

        private List<SocieteEnt> GetSocietesUsedInJson(List<WebApiCiModel> webApiCis)
        {
            var societesCodeComptables = webApiCis.Select(x => x.SocieteCode).Distinct().ToList();

            var filters = new List<Expression<Func<SocieteEnt, bool>>>
            {
                x => societesCodeComptables.Contains(x.CodeSocieteComptable),
                x=>x.Active
            };

            var result = societeManager.Search(filters, asNoTracking: true);

            return result;
        }

        private List<SocieteEnt> GetSocietesOfReponsableAffaireUsedInJson(List<WebApiCiModel> webApiCis)
        {
            var societesCodeComptables = webApiCis.Where(x => x.SocieteCodeResponsableAffaire != null).Select(x => x.SocieteCodeResponsableAffaire).Distinct().ToList();

            var filters = new List<Expression<Func<SocieteEnt, bool>>>
            {
                x => societesCodeComptables.Contains(x.CodeSocieteComptable),
                x=> x.Active
            };

            var result = societeManager.Search(filters, asNoTracking: true);

            return result;
        }

        public List<PersonnelEnt> GetReponsablesAffaireUsedInJson(List<WebApiCiModel> webApiCis, List<SocieteEnt> societeOfReponsablesAffaire)
        {
            var matriculeOfResponsablesAffaire = webApiCis.Where(x => x.MatriculeResponsableAffaire != null).Select(x => x.MatriculeResponsableAffaire).Distinct().ToList();
            var societesIdsOfResponsablesAffaire = societeOfReponsablesAffaire.Select(x => x.SocieteId).Distinct().ToList();

            var filters = new List<Expression<Func<PersonnelEnt, bool>>>
            {
                x => matriculeOfResponsablesAffaire.Contains(x.Matricule),
                x => societesIdsOfResponsablesAffaire.Contains(x.SocieteId.Value)
            };

            var result = personnelManager.Get(filters, asNoTracking: true);

            return result;
        }
    }
}
