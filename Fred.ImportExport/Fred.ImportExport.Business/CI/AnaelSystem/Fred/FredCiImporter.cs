using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI;
using Fred.Business.Organisation;
using Fred.Business.Referential;
using Fred.Business.Referential.Tache;
using Fred.Business.Societe;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Fred
{
    public class FredCiImporter : IFredCiImporter
    {
        private readonly IEtablissementComptableManager etabComptaManager;
        private readonly IOrganisationManager organisationManager;
        private readonly ISocieteManager societeManager;
        private readonly ITacheManager tacheManager;
        private readonly ICIManager ciManager;

        public FredCiImporter(
            IEtablissementComptableManager etabComptaManager,
            IOrganisationManager organisationManager,
            ISocieteManager societeManager,
            ITacheManager tacheManager,
            ICIManager ciManager)
        {
            this.etabComptaManager = etabComptaManager;
            this.organisationManager = organisationManager;
            this.societeManager = societeManager;
            this.tacheManager = tacheManager;
            this.ciManager = ciManager;
        }

        /// <summary>
        /// Importe les cis anael vers Fred
        /// </summary>
        /// <param name="organisationTree">organisationTree</param>
        /// <param name="societe">societe</param>
        /// <param name="fredAnaelCIs">Les ci anaen en tant qu'entité Fred</param>
        /// <returns>Liste des nouveaux et des ci mis a jour</returns>
        public List<CIEnt> ImportCIsFromAnael(OrganisationTree organisationTree, SocieteEnt societe, List<CIEnt> fredAnaelCIs)
        {
            var cis = new List<CIEnt>();

            var ciCodes = fredAnaelCIs.Select(x => x.Code).ToList();

            List<CIEnt> existingCIs = GetFredCis(organisationTree, societe, ciCodes);

            List<EtablissementComptableEnt> etsComptables = etabComptaManager.GetEtablissementComptableList().ToList();

            var utcNow = DateTime.UtcNow;

            foreach (var anaelCI in fredAnaelCIs.ToList())
            {
                if (!string.IsNullOrEmpty(anaelCI.Code) && !string.IsNullOrEmpty(anaelCI.Libelle))
                {
                    // Le code CI n'est unique QUE par SOCIETE (Dans le context RZB, le code ci est unique sur une société.)
                    CIEnt existingCI = existingCIs.Find(x => x.Code == anaelCI.Code && x.SocieteId == anaelCI.SocieteId);

                    // Si le CI existe déjà
                    if (existingCI != null)
                    {
                        existingCI.DateUpdate = utcNow;

                        var updatedCi = UpdateCi(existingCI, anaelCI, societe.SocieteId);
                        UpdateOrganisationOnUpdatedCi(organisationTree, societe, updatedCi);
                        cis.Add(updatedCi);
                    }
                    else
                    {
                        anaelCI.DateImport = utcNow;

                        AddNewOrganisationOnNewCi(organisationTree, societe, anaelCI);

                        cis.Add(anaelCI);
                    }
                }
                else
                {
                    NLog.LogManager.GetCurrentClassLogger().Error("FredBusiness.CIManager.InternalManageImportedCIs : Le code ou le libellé du CI n'est pas renseigné.");
                }
            }
            if (cis.Count > 0)
            {
                var ciAdded = ciManager.AddOrUpdateCIList(cis, updateOrganisation: true);
                CreateTachesSystem(ciAdded);
                List<SocieteEnt> societes = societeManager.GetSocieteList().ToList();
                HandleDeviseCiImported(ciAdded.ToList(), etsComptables, societes);
            }
            return cis;
        }


        private List<CIEnt> GetFredCis(OrganisationTree organisationTree, SocieteEnt societe, List<string> ciCodes)
        {
            var allCisByCodes = ciManager.GetCisByCodes(ciCodes);

            var allCisIdsOfSociete = organisationTree.GetAllCisOfSociete(societe.SocieteId).Select(x => x.Id).ToList();

            return allCisByCodes.Where(x => allCisIdsOfSociete.Contains(x.CiId)).ToList();

        }

        /// <summary>
        ///   Gestion de la mise à jour des CI importés depuis ANAEL
        /// </summary>
        /// <param name="existingCI">CI existant dans FRED</param>
        /// <param name="fredAnaelCI">CI issu d'ANAEL</param>
        /// <param name="societeId">societeId</param>
        /// <returns>Vrai si la mise à jour a réussie, sinon faux</returns>
        private CIEnt UpdateCi(CIEnt existingCI, CIEnt fredAnaelCI, int societeId)
        {
            existingCI.Libelle = fredAnaelCI.Libelle;
            existingCI.EtablissementComptableId = fredAnaelCI.EtablissementComptableId;
            existingCI.Sep = fredAnaelCI.Sep;
            existingCI.DateFermeture = fredAnaelCI.DateFermeture;
            existingCI.DateOuverture = fredAnaelCI.DateOuverture;
            existingCI.ChantierFRED = fredAnaelCI.ChantierFRED;
            existingCI.SocieteId = societeId;
            return existingCI;
        }

        /// <summary>
        ///  Ajoute la tâche par défaut et ses 2 parents aux CI indiqués.
        /// </summary>
        /// <remarks>Méthode créée pour réduire la complexité cognitive</remarks>
        /// <param name="cis">Ensemble de CI concernés</param>
        private void CreateTachesSystem(IEnumerable<CIEnt> cis)
        {
            tacheManager.AddTachesSysteme(cis.Select(x => x.CiId));
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




        public void AddNewOrganisationOnNewCi(OrganisationTree organisationTree, SocieteEnt societe, CIEnt newCi)
        {
            if (!newCi.DateOuverture.HasValue)
            {
                newCi.DateOuverture = DateTime.UtcNow; //RG_440_005
            }

            newCi.SocieteId = societe.SocieteId;

            // Creation de l'organisation pour un Ci avec un etablissement comptable
            if (HasEtablissementComptable(newCi))
            {
                newCi.Organisation = CreateOrganisationWithEtablissementComptableAsParent(organisationTree, newCi.EtablissementComptableId.Value);
                return;
            }

            // Creation de l'organisation pour les autres type de ci 
            newCi.Organisation = CreateOrganisationWithSocieteAsParent(organisationTree, societe.SocieteId);
        }

        public void UpdateOrganisationOnUpdatedCi(OrganisationTree organisationTree, SocieteEnt societe, CIEnt updatedCi)
        {

            var ci = organisationTree.GetCi(updatedCi.CiId);

            OrganisationBase pere = null;

            // mise a jour de l'organisation pour un Ci avec un etablissement comptable
            if (HasEtablissementComptable(updatedCi))
            {
                pere = organisationTree.GetEtablissementComptable(updatedCi.EtablissementComptableId.Value);
            }
            else
            {
                //  mise a jour de l'organisation pour les autres type de ci
                pere = organisationTree.GetSociete(societe.SocieteId);
            }

            updatedCi.Organisation = new OrganisationEnt
            {
                OrganisationId = ci.OrganisationId,
                PereId = pere.OrganisationId,
                TypeOrganisationId = ci.TypeOrganisationId
            };
        }

        private bool HasEtablissementComptable(CIEnt ci)
        {
            return ci.EtablissementComptableId.HasValue;
        }

        private OrganisationEnt CreateOrganisationWithEtablissementComptableAsParent(OrganisationTree organisationTree, int etablissementComptableId)
        {
            var etablissementComptableOrganisationPere = organisationTree.GetEtablissementComptable(etablissementComptableId);
            return organisationManager.GenerateOrganisation(Constantes.OrganisationType.CodeCi, etablissementComptableOrganisationPere.OrganisationId);
        }

        private OrganisationEnt CreateOrganisationWithSocieteAsParent(OrganisationTree organisationTree, int societeId)
        {
            var societeOrganisationPere = organisationTree.GetSociete(societeId);
            return organisationManager.GenerateOrganisation(Constantes.OrganisationType.CodeCi, societeOrganisationPere.OrganisationId);
        }
    }
}
