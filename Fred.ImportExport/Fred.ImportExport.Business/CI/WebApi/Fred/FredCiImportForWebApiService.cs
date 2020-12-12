using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI;
using Fred.Business.Referential.Tache;
using Fred.Business.Societe;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.CI.WebApi.Context.Models;

namespace Fred.ImportExport.Business.CI.WebApi.Fred
{
    /// <summary>
    /// Service qui effectue l'import par web api
    /// </summary>
    public class FredCiImportForWebApiService : IFredCiImportForWebApiService
    {
        private readonly ISocieteManager societeManager;
        private readonly ITacheManager tacheManager;
        private readonly IOrganisationManagerOnImportCiService organisationManagerOnImportCiService;
        private readonly ICIManager ciManager;
        private readonly ICiFinderInWebApiSystemService ciFinderInWebApiSystem;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="societeManager">societeManager</param>
        /// <param name="tacheManager">tacheManager</param>
        /// <param name="organisationManagerOnImportCiService">organisationManagerOnImportCiService</param>
        /// <param name="ciManager">ciManager</param>
        /// <param name="ciFinderInWebApiSystem">ciFinderInWebApiSystem</param>
        public FredCiImportForWebApiService(ISocieteManager societeManager,
            ITacheManager tacheManager,
            IOrganisationManagerOnImportCiService organisationManagerOnImportCiService,
            ICIManager ciManager,
            ICiFinderInWebApiSystemService ciFinderInWebApiSystem)
        {
            this.societeManager = societeManager;
            this.tacheManager = tacheManager;
            this.organisationManagerOnImportCiService = organisationManagerOnImportCiService;
            this.ciManager = ciManager;
            this.ciFinderInWebApiSystem = ciFinderInWebApiSystem;
        }

        /// <summary>
        /// Execute l'import par web api par societe
        /// </summary>
        /// <param name="societesContexts">le context par societe</param>
        /// <param name="organisationTree">l'arbre des orgas</param>
        public void ManageImportedCIsFromApi(List<ImportCiByWebApiSocieteContext> societesContexts, OrganisationTree organisationTree)
        {
            foreach (var societeContext in societesContexts)
            {
                ImportBySociete(societeContext, organisationTree);
            }
        }

        private void ImportBySociete(ImportCiByWebApiSocieteContext societeContext, OrganisationTree organisationTree)
        {
            List<CIEnt> cisToAddOrUpdate = new List<CIEnt>();

            var webApiCIs = societeContext.WebApiCisMappedToCiEnt;

            List<CIEnt> existingCIs = societeContext.CisFoundInFredWithCode;

            var webApiCisToUpdate = webApiCIs.Where(x => ciFinderInWebApiSystem.CiExistIn(existingCIs, x)).ToList();

            foreach (var webApiCiToUpdate in webApiCisToUpdate)
            {
                CIEnt existingCI = ciFinderInWebApiSystem.GetCiIn(existingCIs, webApiCiToUpdate);

                webApiCiToUpdate.DateUpdate = DateTime.UtcNow;

                UpdateDesiredFieldsOnExistingCi(existingCI, webApiCiToUpdate);

                cisToAddOrUpdate.Add(existingCI);
            }

            var webApiCisToCreate = webApiCIs.Where(x => !ciFinderInWebApiSystem.CiExistIn(existingCIs, x)).ToList();

            foreach (var webApiCiToCreate in webApiCisToCreate)
            {
                webApiCiToCreate.DateImport = DateTime.UtcNow;

                if (!webApiCiToCreate.DateOuverture.HasValue)
                {
                    webApiCiToCreate.DateOuverture = DateTime.UtcNow; //RG_440_005
                }

                webApiCiToCreate.Organisation = organisationManagerOnImportCiService.CreateOrganisationOnNewCi(societeContext.TypeOrganisations, organisationTree, webApiCiToCreate, existingCIs, cisToAddOrUpdate);

                cisToAddOrUpdate.Add(webApiCiToCreate);

            }

            if (cisToAddOrUpdate.Count > 0)
            {
                var ciAdded = ciManager.AddOrUpdateCIList(cisToAddOrUpdate, updateOrganisation: true);
                CreateTachesSystem(ciAdded);
                List<SocieteEnt> societes = societeManager.GetSocieteList().ToList();
                HandleDeviseCiImported(ciAdded.ToList(), societeContext.EtablissementComptables, societes);
            }
        }


        private void UpdateDesiredFieldsOnExistingCi(CIEnt existingCI, CIEnt apiCI)
        {
            existingCI.Libelle = apiCI.Libelle;
            existingCI.DateFermeture = apiCI.DateFermeture;
            existingCI.DateOuverture = apiCI.DateOuverture;
            existingCI.EtablissementComptableId = apiCI.EtablissementComptableId;
            existingCI.Description = apiCI.Description;
            existingCI.SocieteId = apiCI.SocieteId;
            existingCI.CodeExterne = apiCI.CodeExterne;
            existingCI.ResponsableAdministratifId = apiCI.ResponsableAdministratifId;
            existingCI.CITypeId = apiCI.CITypeId;
        }


        private void HandleDeviseCiImported(List<CIEnt> cis, List<EtablissementComptableEnt> etsComptables, List<SocieteEnt> societes)
        {
            DeviseEnt deviseRef;
            SocieteEnt societe;
            var ciDeviseList = new List<CIDeviseEnt>();
            foreach (var ci in cis)
            {
                int societeId = 0;
                var etabl = etsComptables.Find(x => x.EtablissementComptableId == ci.EtablissementComptableId);
                if (etabl != null)
                {
                    societeId = etabl.SocieteId ?? 0;
                }
                else
                {
                    societeId = ci.SocieteId ?? 0;
                }
                societe = societes.Find(x => x.SocieteId == societeId);

                if (societe != null)
                {
                    deviseRef = societeManager.GetListSocieteDeviseRef(societe);

                    if (deviseRef != null)
                    {
                        ciDeviseList.Add(new CIDeviseEnt { CiId = ci.CiId, DeviseId = deviseRef.DeviseId, Reference = true });
                    }
                }
            }

            ciManager.BulkAddCIDevise(ciDeviseList);
        }

        private void CreateTachesSystem(IEnumerable<CIEnt> cis)
        {
            tacheManager.AddTachesSysteme(cis.Select(x => x.CiId));
        }
    }
}
