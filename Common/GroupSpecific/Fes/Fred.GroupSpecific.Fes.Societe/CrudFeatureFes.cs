using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Affectation;
using Fred.Business.CI;
using Fred.Business.FeatureFlipping;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Duplication;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Rapport.RapportHebdo;
using Fred.Business.RapportStatut;
using Fred.Business.Referential.CodeDeplacement;
using Fred.Business.Referential.Tache;
using Fred.Business.Utilisateur;
using Fred.Business.ValidationPointage;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Rapport;
using Fred.Entities.Rapport.Search;
using Fred.Entities.Referential;

namespace Fred.Business.Referential.Materiel
{
    public class CrudFeatureFes : CrudFeature
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IEtablissementPaieManager etablissementPaieManager;

        public CrudFeatureFes(
            IUnitOfWork uow,
            IRapportRepository repository,
            IUtilitiesFeature utilities,
            IRapportTacheRepository rapportTacheRepository,
            IRapportValidator rapportValidator,
            IUtilisateurManager utilisateurManager,
            IPersonnelManager personnelManager,
            ICIManager ciManager,
            IPointageManager pointageManager,
            ITacheManager tacheManager,
            ILotPointageManager lotPointageManager,
            IValorisationManager valorisationManager,
            IAffectationManager affectationManager,
            IContratInterimaireManager contratInterimaireManager,
            IRapportHebdoManager rapportHebdoManager,
            IRapportDuplicationService rapportDuplicationService,
            IRapportDuplicationNewCiService rapportDuplicationNewCiService,
            IRapportStatutManager rapportStatutManager,
            IFeatureFlippingManager featureFlippingManager,
            ICodeMajorationManager codeMajorationManager,
            ICodeZoneDeplacementManager codeZoneDeplacementManager,
            IPrimeManager primeManager,
            ICodeDeplacementManager codeDeplacementManager,
            IRapportHebdoService rapportHebdoService,
            IPointageRepository pointageRepository,
            IEtablissementPaieManager etablissementPaieManager)
            : base(uow,
                   repository,
                   utilities,
                   rapportTacheRepository,
                   rapportValidator,
                   utilisateurManager,
                   personnelManager,
                   ciManager,
                   pointageManager,
                   tacheManager,
                   lotPointageManager,
                   valorisationManager,
                   affectationManager,
                   contratInterimaireManager,
                   rapportHebdoManager,
                   rapportDuplicationService,
                   rapportDuplicationNewCiService,
                   rapportStatutManager,
                   featureFlippingManager,
                   codeMajorationManager,
                   codeZoneDeplacementManager,
                   primeManager,
                   codeDeplacementManager,
                   rapportHebdoService,
                   pointageRepository,
                   etablissementPaieManager)
        {
            this.utilisateurManager = utilisateurManager;
            this.etablissementPaieManager = etablissementPaieManager;
        }

        public override LockRapportResponse VerrouillerListeRapport(IEnumerable<int> rapportIds, int valideurId, IEnumerable<int> reportNotToLock, SearchRapportEnt filter, string groupe)
        {
            LockRapportResponse response = base.VerrouillerListeRapport(rapportIds, valideurId, reportNotToLock, filter, groupe);

            // Pour FES, vérouillage des rapports lignes seulement pour le cas de filtre par etablissment paie du personnel
            if (groupe == Constantes.CodeGroupeFES)
            {
                List<RapportEnt> allReportsToLock = Repository.GetRapportToLock(rapportIds);
                List<RapportEnt> reportNotToLockList = allReportsToLock.Where(r => !response.LockedRapports.Select(x => x.RapportId).Contains(r.RapportId)).ToList();
                response.PartialLockedReport = reportNotToLockList.Select(x => x.RapportId).ToList();
                if (filter.OrganisationId.HasValue)
                {
                    IEnumerable<EtablissementPaieEnt> etablissementPaieList = etablissementPaieManager.GetEtablissementPaieByOrganisationId(filter.OrganisationId.Value);
                    if (etablissementPaieList != null && etablissementPaieList.Any())
                    {
                        if (filter.EtablissementPaieIdList == null)
                        {
                            filter.EtablissementPaieIdList = new List<int?>();
                        }
                        filter.EtablissementPaieIdList.AddRange(etablissementPaieList.Select(x => (int?)x.EtablissementPaieId).Distinct());
                    }
                }

                List<RapportLigneEnt> pointages = reportNotToLockList.SelectMany(r => r.ListLignes).Where(rl => filter.EtablissementPaieIdList.Contains(rl.Personnel.EtablissementPaieId)).ToList();
                pointages.ForEach(rpl => rpl.RapportLigneStatutId = RapportStatutEnt.RapportStatutVerrouille.Key);

                //Si tous les rapports lignes du rapport sont vérrouillés, on verrouille le rapport aussi.
                foreach (RapportEnt rapport in reportNotToLockList)
                {
                    if (rapport.ListLignes.All(rl => rl.RapportLigneStatutId == RapportStatutEnt.RapportStatutVerrouille.Key))
                    {
                        rapport.AuteurVerrouId = valideurId;
                        rapport.DateVerrou = DateTime.UtcNow;
                        rapport.RapportStatutId = RapportStatutEnt.RapportStatutVerrouille.Key;
                    }
                }

                response.PartialLockedReport = reportNotToLockList.Where(r => r.RapportStatutId != RapportStatutEnt.RapportStatutVerrouille.Key).Select(r => r.RapportId).ToList();

                Save();
            }

            return response;
        }
    }
}
