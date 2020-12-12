using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Affectation;
using Fred.Business.CI;
using Fred.Business.FeatureFlipping;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport.Common.RapportParStatut;
using Fred.Business.Rapport.Duplication;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Rapport.RapportHebdo;
using Fred.Business.RapportStatut;
using Fred.Business.Referential;
using Fred.Business.Referential.CodeDeplacement;
using Fred.Business.Referential.Tache;
using Fred.Business.Utilisateur;
using Fred.Business.ValidationPointage;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Groupe;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Entities.Rapport.Search;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.FeatureFlipping;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.PointagePersonnel;
using static Fred.Entities.Constantes;
using Constantes = Fred.Entities.Constantes;

namespace Fred.Business.Rapport
{
    public class CrudFeature : ManagerFeature<IRapportRepository>, ICrudFeature
    {
        private const int NbrMaxPrimes = 4;
        private const int NbrMaxTaches = 10;

        private IUtilitiesFeature Utilities { get; }

        private readonly IUnitOfWork uow;
        private readonly IRapportRepository repository;
        private readonly IRapportTacheRepository rapportTacheRepository;
        private readonly IRapportValidator rapportValidator;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IPersonnelManager personnelManager;
        private readonly ICIManager ciManager;
        private readonly IPointageManager pointageManager;
        private readonly ITacheManager tacheManager;
        private readonly ILotPointageManager lotPointageManager;
        private readonly IValorisationManager valorisationManager;
        private readonly IAffectationManager affectationManager;
        private readonly IContratInterimaireManager contratInterimaireManager;
        private readonly IRapportHebdoManager rapportHebdoManager;
        private readonly IRapportDuplicationService rapportDuplicationService;
        private readonly IRapportDuplicationNewCiService rapportDuplicationNewCiService;
        private readonly IRapportStatutManager rapportStatutManager;
        private readonly IFeatureFlippingManager featureFlippingManager;
        private readonly ICodeMajorationManager codeMajorationManager;
        private readonly ICodeZoneDeplacementManager codeZoneDeplacementManager;
        private readonly IPrimeManager primeManager;
        private readonly ICodeDeplacementManager codeDeplacementManager;
        private readonly IRapportHebdoService rapportHebdoService;
        private readonly IPointageRepository pointageRepository;
        private readonly IEtablissementPaieManager etablissementPaieManager;

        public CrudFeature(
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
            : base(uow, repository)
        {
            this.uow = uow;
            this.repository = repository;
            this.rapportTacheRepository = rapportTacheRepository;
            this.rapportValidator = rapportValidator;
            this.utilisateurManager = utilisateurManager;
            this.personnelManager = personnelManager;
            this.ciManager = ciManager;
            this.pointageManager = pointageManager;
            this.tacheManager = tacheManager;
            this.lotPointageManager = lotPointageManager;
            this.valorisationManager = valorisationManager;
            this.affectationManager = affectationManager;
            this.contratInterimaireManager = contratInterimaireManager;
            this.rapportHebdoManager = rapportHebdoManager;
            this.rapportDuplicationService = rapportDuplicationService;
            this.rapportDuplicationNewCiService = rapportDuplicationNewCiService;
            this.rapportStatutManager = rapportStatutManager;
            this.featureFlippingManager = featureFlippingManager;
            this.codeMajorationManager = codeMajorationManager;
            this.codeZoneDeplacementManager = codeZoneDeplacementManager;
            this.primeManager = primeManager;
            this.codeDeplacementManager = codeDeplacementManager;
            this.rapportHebdoService = rapportHebdoService;
            this.pointageRepository = pointageRepository;
            this.etablissementPaieManager = etablissementPaieManager;

            Utilities = utilities;
        }

        /// <summary>
        /// Permet de mettre à jour ou d'ajouter un rapport
        /// </summary>
        /// <param name="rapport">Un rapport</param>
        public void AddOrUpdateRapport(RapportEnt rapport)
        {
            bool isExist = Repository.FindById(rapport.RapportId) != null;

            if (isExist)
            {
                var rapportBeforeUpdate = this.GetRapportByIdWithoutValidation(rapport.RapportId);
                this.CheckRapportStatutChangedInDb(rapport, rapportBeforeUpdate);
                this.UpdateRapport(rapport);
            }
            else
            {
                this.AddRapport(rapport);
            }
        }

        /// <summary>
        ///   Méthode d'ajout d'un rapport
        /// </summary>
        /// <param name="rapport">Rapport à ajouter</param>
        /// <returns>Retourne le rapport nouvellement créée</returns>
        public RapportEnt AddRapport(RapportEnt rapport)
        {
            int userId = this.utilisateurManager.GetContextUtilisateurId();
            var CurrentUser = personnelManager.GetPersonnelById(userId);
            if (rapport != null && CurrentUser.Societe?.Groupe?.Code == Constantes.CodeGroupeFES)
            {
                return FiltreRapportPersonnelStatut(rapport, userId);
            }
            rapport.Cloture = Utilities.IsTodayInPeriodeCloture(rapport);
            rapport.ListLignes.ForEach(l => l.Cloture = rapport.Cloture);

            // La re-récupération du CI permet de vérifier si l'utilisateur actuel y a accès (et accessoirement de résoudre un pb d'EF)
            if (rapport.CI != null)
            {
                rapport.CI = this.ciManager.GetCIById(rapport.CiId);
            }

            //Récupération du statut en cours pour les nouveaux rapports
            if (rapport.RapportStatutId == 0)
            {
                rapport.RapportStatutId = RapportStatutEnt.RapportStatutEnCours.Key;
            }
            var now = DateTime.UtcNow;

            rapport.AuteurCreationId = userId;
            rapport.DateCreation = now;
            rapport.DateChantier = rapport.DateChantier.Date;
            rapport.TypeStatutRapport = (int)TypeStatutRapport.Default;
            rapport.ListLignes.ToList().ForEach(l => l.DatePointage = rapport.DateChantier);
            rapport.ListLignes.ToList().ForEach(l => l.DateCreation = now);
            rapport.ListLignes.ToList().ForEach(l => l.AuteurCreationId = userId);
            rapport.ListLignes.ToList().ForEach(l => l.CiId = rapport.CiId);
            rapport.ListLignes.ToList().ForEach(l => this.pointageManager.TraiteDonneesPointage(l));
            rapport.ListLignes.ForEach(l => l.ListRapportLigneMajorations?.ForEach(m => m.CodeMajoration = null));
            // RG_6472_008 
            rapport.ListLignes.ForEach(l => l.ContratId = GetContratInterimaireId(l));
            rapport.CleanLinkedProperties(true);

            //Ajout du rapport
            List<RapportLigneEnt> pointagesToAdd = rapport.ListLignes?.ToList();
            rapport.ListLignes = null;
            Repository.Insert(rapport);
            Save();

            // CB 13538 : Traitement des pointages hors rapport pour garder le bon ordre d'insertion.
            // L'insertion en masse désordonne le tri des lignes d'insertion.
            foreach (RapportLigneEnt pointage in pointagesToAdd)
            {
                pointage.RapportId = rapport.RapportId;
                pointageRepository.Insert(pointage);
                Save();
            }

            rapport = GetRapportById(rapport.RapportId, true);

            valorisationManager.CreateValorisation(rapport.ListLignes, "Rapport");

            //Sauvegarde des traitements en base de données
            Save();

            // 7108 : 
            //ValorisationManager.NewNotificationTotalValorisation(userId, rapport.CiId, rapport.DateChantier.GetPeriode())

            RapportEnt rapportComplet = GetRapportById(rapport.RapportId, true);

            return rapportComplet;
        }

        /// <inheritdoc />        
        public RapportEnt AddRapportMaterialType(RapportEnt rapport)
        {
            if (rapport == null)
            {
                throw new ArgumentException(nameof(rapport));
            }

            rapport.DateCreation = DateTime.UtcNow;
            rapport.AuteurCreationId = utilisateurManager.GetContextUtilisateurId();
            RapportEnt returnedRapport = Repository.Insert(rapport);
            Save();
            return returnedRapport;
        }

        /// <summary>
        /// Permet d'inserer un nouveau rapport creer a partir de figgo
        /// </summary>
        /// <param name="rapport">rapport a inserer</param>
        /// <returns>rapport inserer</returns>
        public RapportEnt AddRapportFiggo(RapportEnt rapport)
        {
            //Récupération du statut en cours pour les nouveaux rapports
            if (rapport.RapportStatutId == 0)
            {
                rapport.RapportStatutId = RapportStatutEnt.RapportStatutValide2.Key;
            }
            Repository.Insert(rapport);
            Save();
            return rapport;
        }

        private int? GetContratInterimaireId(RapportLigneEnt rapportLigne)
        {
            PersonnelEnt personnel = rapportLigne.Personnel;
            if (personnel == null)
            {
                personnel = personnelManager.GetPersonnelById(rapportLigne.PersonnelId);
            }

            if (personnel?.IsInterimaire == true)
            {
                ContratInterimaireEnt contratInterimaireEnt = contratInterimaireManager.GetContratInterimaireByDatePointage(rapportLigne.PersonnelId, rapportLigne.DatePointage) ?? contratInterimaireManager.GetContratInterimaireByDatePointageAndSouplesse(rapportLigne.PersonnelId, rapportLigne.DatePointage);
                if (contratInterimaireEnt != null)
                {
                    return contratInterimaireEnt.ContratInterimaireId;
                }
            }

            return null;
        }

        /// <summary>
        ///   Ajoute une ligne dans le rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <returns>Un rapport</returns>
        public RapportEnt AddNewPointageReelToRapport(RapportEnt rapport)
        {
            if (rapport.CI?.Societe?.Groupe != null && rapport.CI.Societe.Groupe.Code.Trim() == FeatureRapport.Code_Groupe_FES)
            {
                FulfillAllPersonnelsAffectedToCi(rapport, rapport.CiId);
            }
            else
            {
                RapportLigneEnt pointage = this.pointageManager.GetNewPointageReelLight();
                pointage.RapportId = rapport.RapportId;
                pointage.Rapport = rapport;
                pointage.CiId = rapport.CiId;
                pointage.Ci = rapport.CI;

                pointageManager.GetOrCreateIndemniteDeplacementForRapportLigne(pointage);

                foreach (PrimeEnt prime in rapport.ListPrimes)
                {
                    pointage.ListRapportLignePrimes.Add(this.pointageManager.GetNewPointagePrime(pointage, prime) as RapportLignePrimeEnt);
                }

                //On ajoute les taches à paramétrer pour cette ligne
                foreach (TacheEnt tache in rapport.ListTaches)
                {
                    pointage.ListRapportLigneTaches.Add(this.pointageManager.GetNewPointageReelTache(pointage, tache));
                }

                if (rapport?.CI?.Societe?.Groupe != null && rapport.CI.Societe.Groupe.Code.Trim().Equals(FeatureRapport.Code_Groupe_FES))
                {
                    foreach (CodeMajorationEnt majoration in rapport.ListMajorations)
                    {
                        pointage.ListRapportLigneMajorations.Add(this.pointageManager.GetNewPointageReelMajoration(pointage, majoration));
                    }
                }

                rapport.ListLignes.Add(pointage);
            }
            return rapport;
        }

        /// <summary>
        ///   Ajoute une prime au paramétrage du rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <param name="prime">La prime</param>
        /// <returns>Un rapport</returns>
        public RapportEnt AddPrimeToRapport(RapportEnt rapport, PrimeEnt prime)
        {
            if (prime == null)
            {
                return rapport;
            }
            ////Ajout de la prime à chaque ligne de rapport
            foreach (RapportLigneEnt pointage in rapport.ListLignes)
            {
                if (!pointage.ListRapportLignePrimes.Any(t => t.PrimeId == prime.PrimeId))
                {
                    pointage.ListRapportLignePrimes.Add(this.pointageManager.GetNewPointagePrime(pointage, prime) as RapportLignePrimeEnt);
                }
                else
                {
                    pointage.ListRapportLignePrimes.FirstOrDefault(m => m.PrimeId == prime.PrimeId).IsDeleted = false;
                }
            }

            return rapport;
        }

        /// <summary>
        ///   Ajoute une tache au paramétrage du rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <param name="tache">La tache</param>
        /// <returns>Un rapport</returns>
        public RapportEnt AddTacheToRapport(RapportEnt rapport, TacheEnt tache)
        {
            if (tache == null)
            {
                return rapport;
            }

            //Ajout de la tache à chaque ligne de rapport
            foreach (RapportLigneEnt pointage in rapport.ListLignes)
            {
                if (!pointage.ListRapportLigneTaches.Any(t => t.TacheId == tache.TacheId))
                {
                    pointage.ListRapportLigneTaches.Add(this.pointageManager.GetNewPointageReelTache(pointage, tache));
                }
                else
                {
                    pointage.ListRapportLigneTaches.FirstOrDefault(m => m.TacheId == tache.TacheId).IsDeleted = false;
                }
            }

            return rapport;
        }

        /// <summary>
        ///   Applique les règles de gestion au pointage
        /// </summary>
        /// <param name="rapport">Un rapport</param>
        /// <param name="domaine">Le domaine à vérifier</param>
        /// <returns>Un pointage</returns>
        public RapportEnt ApplyValuesRgRapport(RapportEnt rapport, string domaine)
        {
            if (domaine.Equals(Constantes.EntityType.CI) && rapport.CI != null)
            {
                var currentUser = this.utilisateurManager.GetContextUtilisateur();
                if (rapport?.CI?.Societe?.Groupe != null && rapport.CI.Societe.Groupe.Code.Trim() != FeatureRapport.Code_Groupe_FES)
                {
                    FulfillDefaultMaterial(rapport.ListLignes, currentUser.Personnel.PersonnelId);
                    FulfillDefaultRapportLigne(rapport.ListLignes, currentUser.Personnel);
                }

                for (int i = 0; i < rapport.ListLignes.Count; i++)
                {
                    rapport.ListLignes.ElementAt(i).Ci = rapport.CI;
                    rapport.ListLignes.ElementAt(i).CiId = rapport.CiId;
                    this.pointageManager.ApplyValuesRGPointageReel(rapport.ListLignes.ElementAt(i), Constantes.EntityType.CI);
                }

                // On rajoute la tache par défaut
                TacheEnt tacheDefaut = this.tacheManager.GetTacheParDefaut(rapport.CiId);
                if (tacheDefaut != null)
                {
                    AddTacheToRapport(rapport, tacheDefaut);
                }
                InitHorairesRapport(rapport, rapport.CI);
            }

            return rapport;
        }

        /// <summary>
        ///   Duplique un rapport sur une periode
        /// </summary>
        /// <param name="rapportId">Le rapport à dupliquer</param>
        /// <param name="startDate">date de depart de la duplication</param>
        /// <param name="endDate">date de fin de la duplication</param>
        /// <returns>DuplicateRapportResult</returns>
        public DuplicateRapportResult DuplicateRapport(int rapportId, DateTime startDate, DateTime endDate)
        {
            var rapportToDuplicate = rapportDuplicationService.GetRapportForDuplication(rapportId);

            var duplicateRapportResult = rapportDuplicationService.DuplicateRapport(rapportToDuplicate, startDate, endDate);

            if (!duplicateRapportResult.HasDatesInClosedMonth && !duplicateRapportResult.HasInterimaireWithoutContrat && !duplicateRapportResult.DuplicationOnlyOnWeekend && !duplicateRapportResult.HasAllDuplicationInDifferentZoneDeTravail)
            {
                foreach (var rapport in duplicateRapportResult.Rapports)
                {
                    this.AddRapport(rapport);
                }
            }
            return duplicateRapportResult;
        }

        /// <summary>
        ///   Duplique un rapport
        /// </summary>
        /// <param name="rapport">Le rapport à dupliquer</param>
        /// <returns>Un rapport</returns>
        public RapportEnt DuplicateRapport(RapportEnt rapport)
        {
            return rapportDuplicationService.DuplicateRapport(rapport);
        }

        /// <summary>
        ///   Duplique un rapport pour un autre ci
        /// </summary>
        /// <param name="rapport">Le rapport à dupliquer</param>
        /// <returns>Un rapport</returns>
        public RapportEnt DuplicateRapportForNewCi(RapportEnt rapport)
        {
            return rapportDuplicationNewCiService.DuplicateRapport(rapport);
        }

        /// <summary>
        /// Verrouille le rapport
        /// Crée le lot de pointage de l'utilisateur 'valideurId' s'il n'existe pas (affecte un lotPointageId a chaque ligne du rapport)
        /// </summary>
        /// <param name="rapport">rapport à verrouiller</param>
        /// <param name="valideurId">Identifiant de l'utilisateur</param>
        /// <returns>Le rapport s'il a été verrouillé, sinon Null</returns>
        public RapportEnt VerrouillerRapport(RapportEnt rapport, int valideurId)
        {
            // Validation Gestionnaire de paie
            if (this.utilisateurManager.IsRolePaie(valideurId, rapport.CiId) && !Utilities.IsStatutVerrouille(rapport))
            {
                rapport.AuteurVerrouId = valideurId;
                rapport.DateVerrou = DateTime.UtcNow;
                UpdateLotPointage(rapport, valideurId, true, save: false);
                ChangeRapportStatut(rapport, RapportStatutEnt.RapportStatutVerrouille.Value);
                return rapport;
            }
            return null;
        }

        /// <summary>
        /// Déverrouille le rapport
        /// </summary>
        /// <param name="rapport">rapport à déverrouiller</param>
        /// <param name="valideurId">Identifiant de l'utilisateur</param>
        public virtual void DeverrouillerRapport(RapportEnt rapport, int valideurId)
        {
            // Validation Gestionnaire de paie
            if (this.utilisateurManager.IsRolePaie(valideurId, rapport.CiId))
            {
                rapport.CleanLinkedProperties(true);
                rapport.AuteurVerrouId = null;
                rapport.DateVerrou = null;
                UpdateLotPointage(rapport, valideurId, false, save: false);
                SetOldRapportStatut(rapport);
                UpdateRapport(rapport);
            }
        }

        /// <summary>
        ///   Verrouille une liste de rapport
        ///   Crée le lot de pointage de l'utilisateur 'valideurId' s'il n'existe pas (affecte un lotPointageId a chaque ligne du rapport)
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapports</param>
        /// <param name="valideurId">Identifiant utilisateur du valideur</param>
        /// <returns>La liste des rapports effectivement verrouillés</returns>
        public virtual LockRapportResponse VerrouillerListeRapport(IEnumerable<int> rapportIds, int valideurId, IEnumerable<int> reportNotToLock, SearchRapportEnt filter, string groupe)
        {
            if (rapportIds == null || valideurId <= 0)
            {
                throw new ArgumentException(nameof(rapportIds));
            }
            List<RapportEnt> allReportsToLock = Repository.GetRapportToLock(rapportIds);

            Dictionary<int, bool> ciIdIsRolePaie = this.utilisateurManager.IsRolePaie(valideurId, allReportsToLock.Select(x => x.CiId).ToList());
            List<RapportEnt> rapportsToLock = allReportsToLock.Where(r => ciIdIsRolePaie[r.CiId] && (reportNotToLock == null || !reportNotToLock.Contains(r.RapportId))).ToList();

            // groupe les rapports par paquets de 100
            var rapportsGrouped = rapportsToLock.Select((rapport, index) => new { Index = index, Value = rapport })
                                    .GroupBy(x => x.Index / 100)
                                    .Select(x => x.Select(y => y.Value).ToList())
                                    .ToList();

            // Traitement de chaque groupe de rapports
            foreach (var rapportsGroup in rapportsGrouped)
            {
                UpdateLotPointage(rapportsToLock, valideurId, verrouillage: true);

                foreach (RapportEnt r in rapportsToLock)
                {
                    if (ciIdIsRolePaie[r.CiId])
                    {
                        r.AuteurVerrouId = valideurId;
                        r.DateVerrou = DateTime.UtcNow;
                        r.RapportStatutId = RapportStatutEnt.RapportStatutVerrouille.Key;
                        if (r.ListLignes.Any())
                        {
                            r.ListLignes.ForEach(rpl => rpl.RapportLigneStatutId = RapportStatutEnt.RapportStatutVerrouille.Key);
                        }
                    }
                }

                // Mise à jour du groupe de rapports
                Save();
            }

            return new LockRapportResponse { LockedRapports = rapportsToLock, PartialLockedReport = new List<int>() };
        }

        /// <summary>
        ///   Déverouille la liste des rapports
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapports</param>
        /// <param name="valideurId">Identifiant utilisateur du valideur</param>
        public virtual void DeverrouillerListeRapport(IEnumerable<int> rapportIds, int valideurId)
        {
            if (rapportIds == null || valideurId <= 0)
            {
                throw new ArgumentException(nameof(rapportIds));
            }

            List<RapportEnt> rapportsToUnlock = Repository.Query()
                                                .Filter(r => rapportIds.Contains(r.RapportId) && r.RapportStatutId == RapportStatutEnt.RapportStatutVerrouille.Key)
                                                .Include(o => o.RapportStatut)
                                                .Include(o => o.ListLignes)
                                                .Get()
                                                .ToList();

            Dictionary<int, bool> ciIdIsRolePaie = this.utilisateurManager.IsRolePaie(valideurId, rapportsToUnlock.Select(x => x.CiId).ToList());
            rapportsToUnlock = rapportsToUnlock.Where(r => ciIdIsRolePaie[r.CiId]).ToList();

            // groupe les rapports par paquets de 100
            var rapportsGrouped = rapportsToUnlock.Select((rapport, index) => new { Index = index, Value = rapport })
                                    .GroupBy(x => x.Index / 100)
                                    .Select(x => x.Select(y => y.Value).ToList())
                                    .ToList();

            // Traitement des groupes de rapports
            foreach (var rapportsGroup in rapportsGrouped)
            {
                UpdateLotPointage(rapportsGroup, valideurId, verrouillage: false);

                foreach (RapportEnt r in rapportsGroup)
                {
                    r.AuteurVerrou = null;
                    r.AuteurVerrouId = null;
                    r.DateVerrou = null;
                    int? rapportverrouille = r.ListLignes.FirstOrDefault(x => x.RapportId == r.RapportId)?.ValideurId;
                    if (rapportverrouille.HasValue)
                    {
                        r.ListLignes.FirstOrDefault(x => x.RapportId == r.RapportId).RapportLigneStatutId = RapportStatutEnt.RapportStatutEnCours.Key;
                    }
                    else if (r.ListLignes.Any())
                    {
                        r.ListLignes.FirstOrDefault(x => x.RapportId == r.RapportId).RapportLigneStatutId = RapportStatutEnt.RapportStatutValide2.Key;
                    }
                    SetOldRapportStatut(r);
                }

                // Mise à jour du groupe de rapports
                Save();
            }
        }

        /// <summary>
        ///   Supprimer un rapport
        /// </summary>
        /// <param name="rapport">Le rapport à supprimer</param>
        /// <param name="suppresseurId">Identifiant de l'utilisateur ayant supprimer le rapport</param>
        /// <param name="fromListeRapport">Indique si on supprime depuis la liste des rapports</param>
        public void DeleteRapport(RapportEnt rapport, int suppresseurId, bool fromListeRapport = false)
        {
            if (fromListeRapport)
            {
                rapport = GetRapportById(rapport.RapportId, true);
            }

            string codeGroupe = rapport.CI?.Societe?.Groupe != null ? rapport.CI.Societe.Groupe.Code : string.Empty;
            rapport.CleanLinkedProperties(true);
            rapport.AuteurSuppressionId = suppresseurId;
            rapport.DateSuppression = DateTime.UtcNow;
            for (int i = rapport.ListLignes.Count - 1; i >= 0; --i)
            {
                RapportLigneEnt pointage = rapport.ListLignes.ElementAt(i);
                if (pointage.IsCreated)
                {
                    rapport.ListLignes.Remove(pointage);
                    continue;
                }

                pointage.IsUpdated = false;
                pointage.IsDeleted = true;
                pointageManager.GenerationPointageSamediCP(pointage);
                this.pointageManager.TraiteEtatPointage(pointage, suppresseurId, codeGroupe);
            }

            Repository.Update(rapport);

            //Sauvegarde des traitements en base de données
            Save();
        }

        /// <summary>
        ///   Retourne un rapport avec des données vides
        /// </summary>
        /// <param name="ciId">Identifiant d'un CI</param>
        /// <returns>Un rapport avec des données vides, ou avec le CI chargé</returns>
        public RapportEnt GetNewRapport(int? ciId = null)
        {
            RapportStatutEnt rs = rapportStatutManager.GetRapportStatutByCode(RapportStatutEnt.RapportStatutEnCours.Value);

            RapportEnt rapport = new RapportEnt
            {
                RapportStatutId = rs.RapportStatutId,
                RapportStatut = rs,
                DateChantier = DateTime.UtcNow.Date,
                ListLignes = new List<RapportLigneEnt>(),
                NbMaxPrimes = NbrMaxPrimes,
                NbMaxTaches = NbrMaxTaches
            };
            //// On set les statuts
            rapport.CanBeDeleted = true;

            Utilities.SetStatut(rapport);

            if (ciId.HasValue)
            {
                rapport.CI = this.ciManager.GetCiById(ciId.Value, true);
                rapport.CiId = ciId.Value;
            }

            if (rapport.CI?.Societe?.Groupe != null && rapport.CI.Societe.Groupe.Code.Trim() == FeatureRapport.Code_Groupe_FES)
            {
                FulfillAllPersonnelsAffectedToCi(rapport, rapport.CiId);
            }
            else
            {
                //// Ajout d'une ligne par défaut et dans cette ligne, figure le chef de chantier qui saisit le rapport
                AddNewPointageReelToRapport(rapport);
                UtilisateurEnt utilisateur = this.utilisateurManager.GetContextUtilisateur();
                FulfillDefaultRapportLigne(rapport.ListLignes, utilisateur.Personnel);
                FulfillDefaultMaterial(rapport.ListLignes, utilisateur.Personnel.PersonnelId);
            }

            //// Ajout de la tache par défaut 000000
            TacheEnt tacheDefaut = this.tacheManager.GetTacheParDefaut(rapport.CiId);
            if (tacheDefaut != null)
            {
                AddTacheToRapport(rapport, tacheDefaut);
            }

            return rapport;
        }

        /// <summary>
        /// Remplir des lignes de rapport par tous les personnels affecté au CI
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <param name="ciId">L'identifiant du CI</param>
        private void FulfillAllPersonnelsAffectedToCi(RapportEnt rapport, int ciId)
        {
            List<PersonnelEnt> personnelList = this.affectationManager.GetAffectationsByCiId(ciId).Select(a => a.Personnel).Where(p => p.Statut.Equals(Constantes.TypePersonnel.Ouvrier)).ToList();
            foreach (PersonnelEnt personnel in personnelList)
            {
                RapportLigneEnt rapportLigne = this.pointageManager.GetNewPointageReelLight();
                rapportLigne.RapportId = rapport.RapportId;
                rapportLigne.Rapport = rapport;
                rapportLigne.DatePointage = rapport.DateChantier;
                rapportLigne.CiId = rapport.CiId;
                rapportLigne.Ci = rapport.CI;
                rapportLigne.PersonnelId = personnel.PersonnelId;
                rapportLigne.Personnel = personnel;
                rapportLigne.PrenomNomTemporaire = personnel.PrenomNom;
                FulfillAstreintesInformations(rapportLigne, rapportLigne.DatePointage);

                rapport.ListLignes.Add(rapportLigne);
            }
        }

        /// <summary>
        /// Permet de remplir la premiere ligne par le personnel fourni en paramétre
        /// </summary>
        /// <param name="rapportLignes">La liste des lignes de rapport</param>
        /// <param name="personnel">Le personnel</param>
        private void FulfillDefaultRapportLigne(ICollection<RapportLigneEnt> rapportLignes, PersonnelEnt personnel)
        {
            if (personnel != null && rapportLignes?.Any() == true)
            {
                rapportLignes.ElementAt(0).PersonnelId = personnel.PersonnelId;
                rapportLignes.ElementAt(0).Personnel = personnel;
                rapportLignes.ElementAt(0).PrenomNomTemporaire = personnel.PrenomNom;
            }
        }

        /// <summary>
        /// Permet de remplir la premiere ligne par le materiel par défault
        /// </summary>
        /// <param name="rapportLignes">La liste des lignes de rapport</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        private void FulfillDefaultMaterial(ICollection<RapportLigneEnt> rapportLignes, int personnelId)
        {
            var matos = personnelManager.GetMaterielDefault(personnelId);
            if (matos != null && rapportLignes?.Any() == true)
            {
                rapportLignes.ElementAt(0).Materiel = matos;
                rapportLignes.ElementAt(0).MaterielId = matos.MaterielId;
                rapportLignes.ElementAt(0).MaterielNomTemporaire = matos.LibelleLong;
            }
        }

        /// <summary>
        ///   Retourne le rapport portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport à retrouver.</param>
        /// <param name="forWebUse">Provenance du Web</param>    
        /// <returns>Le rapport retrouvée, sinon nulle.</returns>
        public RapportEnt GetRapportById(int rapportId, bool forWebUse)
        {
            var rapport = Repository.GetRapportById(rapportId);
            if (rapport != null)
            {
                if (rapport.ListLignes.Count > 0)
                {
                    rapport.ListLignes = rapport.ListLignes.Where(rl => !rl.DateSuppression.HasValue).OrderBy(o => o.RapportLigneId).ToList();
                }

                rapport.ListCommentaires.ToList().ForEach(c =>
                {
                    c.Rapport = null;
                    c.Tache.RapportLigneTaches = null;
                });

                // Reconstitution des prime pour l'affichage du rapport, il doit y avoir autant de primes paramétrées dans le rapport que de primes dans les lignes
                // Pour garder une cohérence d'afficage dans le tableau
                // On trie les primes du paramétrage du rapport par code
                // LCO - 24/01/2017 : suppression suite à refacto EF

                // On fini par trier les taches et nouvelles taches associées par code
                foreach (RapportLigneEnt rl in rapport.ListLignes)
                {
                    rl.ListRapportLignePrimes = rl.ListRapportLignePrimes.OrderBy(o => o.Prime.Code).ToList();
                    rl.ListRapportLigneTaches = rl.ListRapportLigneTaches.OrderBy(o => o.Tache.Code).ToList();
                }

                if (rapport.CI?.Societe?.Groupe != null && rapport.CI.Societe.Groupe.Code.Trim().Equals(FeatureRapport.Code_Groupe_FES))
                {
                    foreach (RapportLigneEnt rl in rapport.ListLignes)
                    {
                        rl.ListRapportLigneMajorations = rl.ListRapportLigneMajorations.OrderBy(o => o.CodeMajoration?.Code).ToList();
                    }
                }

                InitNomTemporaire(rapport);

                // Reconstitution des taches pour l'affichage du rapport, il doit y avoir autant de taches paramétrées dans le rapport que de taches dans les lignes
                // Pour garder une cohérence d'afficage dans le tableau
                // On trie les taches du paramétrage du rapport par code
                // LCO - 24/01/2017 : suppression suite à refacto EF

                // On fini par trier les taches et nouvelles taches associées par code
                // LCO - 24/01/2017 : suppression suite à refacto EF
                UtilisateurEnt userConnected = this.utilisateurManager.GetContextUtilisateur();
                rapport.CanBeDeleted = Utilities.GetCanBeDeleted(rapport, userConnected);
                Utilities.SetStatut(rapport);
                rapportValidator.CheckRapport(rapport);
                rapport.CanBeValidated = Utilities.GetCanBeValidated(rapport);
                rapport.CanBeLocked = Utilities.GetCanBeLocked(rapport);
                rapport.ValidationSuperieur = Utilities.GetValidationSuperieur(rapport);
                rapport.Cloture = Utilities.IsTodayInPeriodeCloture(rapport);
                rapport.ListLignes.ForEach(l => l.Cloture = rapport.Cloture);
                rapport.ListLignes.ForEach(l => FulfillAstreintesInformations(l, l.DatePointage));
            }

            return rapport;
        }

        /// <summary>
        ///   Retourne le rapport portant l'identifiant unique indiqué sans validation.
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport à retrouver.</param> 
        /// <returns>Le rapport retrouvée, sinon nulle.</returns>
        public RapportEnt GetRapportByIdWithoutValidation(int rapportId)
        {
            return Repository.GetRapportById(rapportId);
        }

        /// <inheritdoc />
        public IEnumerable<RapportEnt> GetRapportLightList(int utilisateurId, DateTime? periode)
        {
            List<int> ciList = this.utilisateurManager.GetAllCIbyUser(utilisateurId).ToList();

            if (periode.HasValue)
            {
                DateTime firstDayOfMonth = new DateTime(periode.Value.Year, periode.Value.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                return Repository.GetRapportListBetweenDatesByCiList(ciList, firstDayOfMonth, lastDayOfMonth);
            }

            return Repository.GetRapportsListbyCiList(ciList);
        }

        /// <summary>
        ///   Retourne la liste des Rapports.
        /// </summary>
        /// <returns>Liste des Rapports.</returns>
        public IEnumerable<RapportEnt> GetRapportList()
        {
            IEnumerable<RapportEnt> rapports = Repository.GetRapportList().ToList();
            if (rapports != null)
            {
                UtilisateurEnt userConnected = this.utilisateurManager.GetById(this.utilisateurManager.GetContextUtilisateurId());
                foreach (RapportEnt rapport in rapports)
                {
                    Utilities.SetStatut(rapport);
                    rapport.CanBeDeleted = Utilities.GetCanBeDeleted(rapport, userConnected);
                }

                return rapports;
            }

            return new RapportEnt[] { };
        }

        /// <summary>
        /// Récupération de liste des rapports en fonction d'une liste d'identifiants de rapport
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapport</param>
        /// <returns>Liste de rapport</returns>
        public IEnumerable<RapportEnt> GetRapportList(IEnumerable<int> rapportIds)
        {
            return Repository.Query().Filter(x => rapportIds.Contains(x.RapportId)).Get();
        }

        /// <summary>
        /// Retourne la liste des rapports avec leurs lignes 
        /// </summary>
        /// <param name="rapportIds">Id des rapports à récupérer</param>
        /// <returns>Une liste de rapport potentiellement vide, jamais null</returns>
        public IEnumerable<RapportEnt> GetRapportListWithRapportLignesNoTracking(IEnumerable<int> rapportIds)
        {
            return Repository.GetRapportListWithRapportLignesNoTracking(rapportIds);
        }

        /// <summary>
        ///   liste rapport pour mobile
        /// </summary>
        /// <param name="sinceDate">The since date.</param>
        /// <param name="userId">Id utilisateur connecté.</param>
        /// <returns>Liste des rapport pour le mobile.</returns>
        public IEnumerable<RapportEnt> GetRapportsMobile(DateTime? sinceDate = null, int? userId = null)
        {
            return Repository.GetRapportsMobile(sinceDate);
        }

        /// <summary>
        ///   Méthode de mise à jour d'un rapport
        /// </summary>
        /// <param name="rapport">Rapport à mettre à jour</param>
        /// <returns>Retourne le rapport nouvellement créée</returns>
        public RapportEnt UpdateRapport(RapportEnt rapport)
        {
            CheckRapportLigneConccurency(rapport.ListLignes);
            int userId = utilisateurManager.GetContextUtilisateurId();
            rapport.Cloture = Utilities.IsTodayInPeriodeCloture(rapport);
            rapport.ListLignes.ForEach(l => l.Cloture = rapport.Cloture);

            if (rapport.IsStatutVerrouille)
            {
                // contrôle de la répartion des heures pour un rapport verrouillé
                this.CheckRapportLignesPersonnelHeuresErrors(rapport, userId);

                // Gestion de la récupération du numéro de lot de pointage dans le cas des rapports verrouillés
                var lotPointageId = rapport.ListLignes.FirstOrDefault(p => !p.IsCreated && !p.IsDeleted)?.LotPointageId;
                if (lotPointageId.HasValue)
                {
                    rapport.ListLignes.Where(l => l.IsCreated).ForEach(p => p.LotPointageId = lotPointageId);
                }
            }

            // Si le Ci de rapport est modifié 
            // on doit passer toutes les ancienes lignes a Deleted pour ne pas garder des pointages pour les personnels d'autres CI
            IEnumerable<RapportLigneEnt> otherRapportLigneToDelete = this.pointageManager.GetOtherRapportLignes(rapport.RapportId, rapport.ListLignes.Select(l => l.RapportLigneId).ToList());
            if (otherRapportLigneToDelete != null && otherRapportLigneToDelete.Any())
            {
                pointageManager.DeletePointageList(otherRapportLigneToDelete);
            }

            //On traite les données de chaque pointage
            foreach (RapportLigneEnt ligne in rapport.ListLignes)
            {
                // On renseigne le rapportId
                ligne.RapportId = rapport.RapportId;

                // On copie la date du rapport du rapport dans la date de pointage de chaque ligne
                if (ligne.DatePointage != rapport.DateChantier)
                {
                    ligne.DatePointage = rapport.DateChantier;
                    ligne.IsUpdated = true;
                }

                // De même pour le CI
                ligne.CiId = rapport.CiId;
                ligne.Ci = rapport.CI;
                this.pointageManager.TraiteDonneesPointage(ligne);

                UpdateSortiesAstreintes(ligne);
            }

            rapport.ListLignes.Where(l => l.IsCreated || l.IsUpdated).ForEach(p => p.ContratId = GetContratInterimaireId(p));

            //changement du dernier modificateur
            rapport.AuteurModificationId = userId;
            rapport.DateModification = DateTime.UtcNow;

            string codeGroupe = rapport.CI?.Societe?.Groupe != null ? rapport.CI.Societe.Groupe.Code.Trim() : string.Empty;
            //Suppression des propriétés liée
            rapport.CleanLinkedProperties(true);

            rapport.ListLignes.ToList().ForEach(p => this.pointageManager.TraiteEtatPointage(p, userId, codeGroupe));

            //// Traitement des commentaires sur taches
            List<RapportTacheEnt> listCommentaireDeleted = new List<RapportTacheEnt>();
            foreach (RapportTacheEnt commentaire in rapport.ListCommentaires)
            {
                if (commentaire.IsCreated)
                {
                    this.rapportTacheRepository.Insert(commentaire);
                }
                else if (commentaire.IsDeleted)
                {
                    listCommentaireDeleted.Add(commentaire);
                }
                else
                {
                    this.rapportTacheRepository.Update(commentaire);
                }
            }
            foreach (RapportTacheEnt commentaire in listCommentaireDeleted)
            {
                rapport.ListCommentaires.Remove(commentaire);
                rapportTacheRepository.Delete(commentaire);
            }

            Repository.RemoveAttachedEntityFromContext(rapport.RapportId);
            Repository.Update(rapport);

            ////Sauvegarde des traitements en base de données
            Save();

            // 7108 :
            //ValorisationManager.NewNotificationTotalValorisation(userId, rapport.CiId, rapport.DateChantier.GetPeriode())
            return rapport;
        }

        /// <summary>
        /// Permet de crééer ou mettre à jours des sorties astreintes associées à une ligne de rapport
        /// </summary>
        /// <param name="rapportLigneEnt">La ligne du rapport</param>
        private void UpdateSortiesAstreintes(RapportLigneEnt rapportLigneEnt)
        {
            IEnumerable<int> rapportLigneAstreinteIdsToKeep = rapportLigneEnt.ListRapportLigneAstreintes.Where(a => a.RapportLigneAstreinteId != 0).Select(l => l.RapportLigneAstreinteId);
            DeleteOtherAstreintes(rapportLigneEnt.RapportLigneId, rapportLigneAstreinteIdsToKeep);

            foreach (RapportLigneAstreinteEnt rapportLigneAstreinte in rapportLigneEnt.ListRapportLigneAstreintes)
            {
                if (rapportLigneAstreinte.RapportLigneAstreinteId != 0)
                {
                    this.Repository.UpdateRapportLigneAstreinte(rapportLigneAstreinte);
                }
                else if (rapportLigneAstreinte.RapportLigneId != 0)
                {
                    this.Repository.AddRapportLigneAstreinte(rapportLigneAstreinte);
                }
            }
            this.Save();
        }

        /// <summary>
        /// Permet de supprimer la liste des sorties astreintes qui n'appartient pas à la liste des astreintes à garder
        /// </summary>
        /// <param name="rapportLigneId">La ligne du rapport</param>
        /// <param name="rapportLigneAstreinteIdsToKeep">La liste des astreintes à garder</param>
        private void DeleteOtherAstreintes(int rapportLigneId, IEnumerable<int> rapportLigneAstreinteIdsToKeep)
        {
            IEnumerable<RapportLigneAstreinteEnt> allRapportLigneAstreintes = this.Repository.GetRapportLigneAstreintes(rapportLigneId);

            IEnumerable<RapportLigneAstreinteEnt> rapportLigneAstreintesToDelete = allRapportLigneAstreintes.Where(i => !rapportLigneAstreinteIdsToKeep.Contains(i.RapportLigneAstreinteId));

            this.Repository.DeleteRapportLigneAstreintes(rapportLigneAstreintesToDelete);
        }

        /// <summary>
        /// Permet de remplir les informations d'astreinte dans une ligne de rapport
        /// </summary>
        /// <param name="rapportLigne">La ligne de rapport</param>
        /// <param name="astreinteDate">Date d'astreinte</param>
        private void FulfillAstreintesInformations(RapportLigneEnt rapportLigne, DateTime astreinteDate)
        {
            if (rapportLigne?.PersonnelId != null)
            {
                var astreinte = affectationManager.GetAstreinte(rapportLigne.CiId, rapportLigne.PersonnelId.Value, astreinteDate);
                if (astreinte != null)
                {
                    rapportLigne.HasAstreinte = true;
                    rapportLigne.AstreinteId = astreinte.AstreintId;
                }
                else
                {
                    rapportLigne.HasAstreinte = false;
                    rapportLigne.AstreinteId = 0;
                }
            }
        }

        /// <summary>
        ///   Permet de valider un rapport
        /// </summary>
        /// <param name="rapport">Un rapport</param>
        /// <param name="valideurId">Entité UtilisateurEnt représentant le valideur</param>
        /// <returns>true en cas de succés, sinon false</returns>
        public bool ValidationRapport(RapportEnt rapport, int valideurId)
        {
            string level;
            // RG_6472_007b 
            if (rapport.CI.Societe.TypeSociete?.Code == TypeSociete.Sep && rapport.CI.CompteInterneSepId == null)
            {
                return false;
            }

            // Validation Directeur de chantier
            if (this.utilisateurManager.IsNiveauPaie3(valideurId, rapport.CiId))
            {
                rapport.DateValidationDRC = DateTime.UtcNow;
                rapport.ValideurDRCId = valideurId;
                level = RapportStatutEnt.RapportStatutValide3.Value;
            }
            // Validation Conducteur de travaux
            else if (this.utilisateurManager.IsNiveauPaie2(valideurId, rapport.CiId))
            {
                rapport.DateValidationCDT = DateTime.UtcNow;
                rapport.ValideurCDTId = valideurId;
                level = RapportStatutEnt.RapportStatutValide2.Value;
            }
            // Validation chef de chantier
            else if (this.utilisateurManager.IsNiveauPaie1(valideurId, rapport.CiId))
            {
                rapport.DateValidationCDC = DateTime.UtcNow;
                rapport.ValideurCDCId = valideurId;
                level = RapportStatutEnt.RapportStatutValide1.Value;
            }
            // pas de niveau de validation
            else
            {
                return false;
            }

            ChangeRapportStatut(rapport, level);
            return true;
        }

        /// <summary>
        /// Ajoute, met à jour ou supprime les pointages de la liste lors d'une duplication
        /// </summary>
        /// <param name="listPointages">Liste des pointages à traîter</param>
        /// <param name="duplicatedPointageId">Id du pointage dupliqué</param>
        /// <param name="rapportsAdded">Liste des rapports ajoutés</param>
        /// <param name="rapportsUpdated">Liste des rapports à mettre à jour</param>
        /// <returns>Le résultat de l'enregistrement</returns>
        public PointagePersonnelSaveResultModel SaveListDuplicatedPointagesPersonnel(IEnumerable<RapportLigneEnt> listPointages, int duplicatedPointageId, out List<RapportEnt> rapportsAdded, out List<RapportEnt> rapportsUpdated)
        {
            var duplicatedPointage = this.pointageManager.FindById(duplicatedPointageId);
            RapportEnt duplicatedRapport = duplicatedPointage != null && duplicatedPointage.Rapport == null ? this.Repository.FindById(duplicatedPointage.RapportId) : null;
            return SaveListPointagePersonnel(ref listPointages, out rapportsAdded, out rapportsUpdated, duplicatedRapport);
        }

        /// <summary>
        /// Ajoute, met à jour ou supprime les pointages de la liste
        /// </summary>
        /// <param name="listPointages">Liste des pointages à traîter</param>
        /// <param name="rapportsAdded">Liste des rapports ajoutés</param>
        /// <param name="rapportsUpdated">Liste des rapports à mettre à jour</param>
        /// <returns>Le résultat de l'enregistrement</returns>
        public PointagePersonnelSaveResultModel SaveListPointagesPersonnel(IEnumerable<RapportLigneEnt> listPointages, out List<RapportEnt> rapportsAdded, out List<RapportEnt> rapportsUpdated)
        {
            return SaveListPointagePersonnel(ref listPointages, out rapportsAdded, out rapportsUpdated);
        }

        /// <summary>
        /// Ajoute, met à jour ou supprime les pointages de la liste
        /// </summary>
        /// <param name="listPointages">Liste des pointages à traîter</param>
        /// <param name="rapportsAdded">Liste des rapports ajoutés</param>
        /// <param name="rapportsUpdated">Liste des rapports à mettre à jour</param>
        /// <param name="duplicatedRapport">Rapport du pointage dupliqué</param>
        /// <returns>Le résultat de l'enregistrement</returns>
        private PointagePersonnelSaveResultModel SaveListPointagePersonnel(ref IEnumerable<RapportLigneEnt> listPointages, out List<RapportEnt> rapportsAdded, out List<RapportEnt> rapportsUpdated, RapportEnt duplicatedRapport = null)
        {
            PointagePersonnelSaveResultModel saveResult = new PointagePersonnelSaveResultModel();

            listPointages = this.pointageManager.CheckPointageForSave(listPointages);
            rapportsAdded = new List<RapportEnt>();
            rapportsUpdated = new List<RapportEnt>();

            RapportHebdoDeplacement deplacement = new RapportHebdoDeplacement(codeMajorationManager, codeZoneDeplacementManager, primeManager, codeDeplacementManager);
            if (!BusinessValidationForSave(listPointages, deplacement, saveResult))
            {
                return saveResult;
            }

            // Suppression
            IEnumerable<RapportLigneEnt> listPointagesToDelete = listPointages.Where(p => p.PointageId != 0 && p.IsDeleted);
            var personnel = listPointages.FirstOrDefault(x => x.PersonnelId != null);
            GroupeEnt groupe = null;
            if (personnel != null)
            {
                groupe = personnelManager.GetPersonnelGroupebyId(personnel.PersonnelId.Value);
            }
            else
            {
                saveResult.Errors.Add(FeatureRapport.Rapport_Detail_Erreur_Doublon);
            }
            string codeGroupe = groupe != null ? groupe.Code.Trim() : string.Empty;

            foreach (RapportLigneEnt pointageToDelete in listPointagesToDelete)
            {
                pointageManager.TraiteDonneesPointage(pointageToDelete);
                pointageToDelete.CleanLinkedProperties();

                if (!pointageToDelete.IsGenerated || !pointageManager.IsSamediCP(pointageToDelete))
                {
                    // Déjà traité dans TraiteDonneesPointage
                    pointageManager.DeletePointage(pointageToDelete);
                    AddPointageToRapportUpdatedList(rapportsUpdated, pointageToDelete);
                }
            }

            foreach (var pointage in listPointages.Where(p => !p.IsDeleted && (p.IsCreated || p.IsUpdated)))
            {
                deplacement.UpdateCodeFromZone(pointage);

                pointageManager.TraiteDonneesPointage(pointage);
                pointage.Cloture = ciManager.IsCIClosed(pointage.CiId, pointage.DatePointage);
                pointage.CleanLinkedProperties();

                // Ajout
                if (pointage.IsCreated)
                {
                    HandleNewPointagePersonnel(pointage, rapportsAdded, rapportsUpdated, duplicatedRapport, codeGroupe);
                }
                // Mise à jour
                else if (pointage.IsUpdated)
                {
                    HandleUpdatePointagePersonnel(pointage, rapportsUpdated, codeGroupe);
                }
            }

            Save();
            foreach (var pointage in listPointages.Where(p => !p.IsDeleted))
            {
                rapportHebdoManager.CheckinAstreinteIntervalDay(pointage);
            }
            return saveResult;
        }

        private void HandleNewPointagePersonnel(RapportLigneEnt pointage, List<RapportEnt> rapportsAdded, List<RapportEnt> rapportsUpdated, RapportEnt duplicatedRapport, string codeGroupe)
        {
            var rapport = GenerateRapportFromNewPointagePersonnel(pointage, duplicatedRapport);
            if (rapport.RapportId != 0)
            {
                pointage.RapportId = rapport.RapportId;
                HandleUpdatePointagePersonnel(pointage, rapportsUpdated, codeGroupe);
            }
            else
            {
                Repository.Insert(rapport);
                Save();
                rapportHebdoService.CreateOrUpdatePrimeAstreinte(rapport);
                rapportsAdded.Add(rapport);
                valorisationManager.InsertValorisationFromPointage(pointage, "Rapport");
                // Flux SAP
            }
        }

        private void HandleUpdatePointagePersonnel(RapportLigneEnt pointage, List<RapportEnt> rapportsUpdated, string codeGroupe)
        {
            this.pointageManager.TraiteEtatPointage(pointage, utilisateurManager.GetContextUtilisateurId(), codeGroupe);
            HandleCommentaire(pointage);
            AddPointageToRapportUpdatedList(rapportsUpdated, pointage);
        }

        private void AddPointageToRapportUpdatedList(List<RapportEnt> rapportsUpdated, RapportLigneEnt pointage)
        {
            pointageManager.PerformEagerLoading(pointage, p => p.Rapport);
            if (!rapportsUpdated.Any(r => r.RapportId == pointage.RapportId))
            {
                //On ajoute le rapport dans la liste des rapports mis à jour que s'il n'existe pas déjà dans la liste
                rapportsUpdated.Add(pointage.Rapport);
            }
        }

        /// <summary>
        /// Valide des pointages.
        /// </summary>
        /// <param name="pointages">Les pointages concernés.</param>
        /// <param name="deplacement">Le gestionnaire des déplacements.</param>
        /// <param name="result">Le résultat de la validation.</param>
        /// <returns>True si les pointages sont valides, sinon false.</returns>
        private bool BusinessValidationForSave(IEnumerable<RapportLigneEnt> pointages, RapportHebdoDeplacement deplacement, PointagePersonnelSaveResultModel result)
        {
            if (deplacement.UtilisateurConnecteIsGFES)
            {
                // Business validation
                var allValide = true;
                var rapportHebdoValidator = new RapportHebdoValidator(featureFlippingManager, codeMajorationManager, primeManager);
                foreach (var pointage in pointages.Where(p => !p.IsDeleted))
                {
                    // Tous les pointages doivent être traités pour avoir l'ensemble des erreurs
                    allValide &= rapportHebdoValidator.Validate(pointage);
                }
                if (!allValide)
                {
                    foreach (var error in rapportHebdoValidator.GetErrorMessages())
                    {
                        result.Errors.Add(error);
                    }
                    return false;
                }
            }

            CheckRapportLigneConccurency(pointages);

            return true;
        }

        // Vérifie la conccurence d'accès pour les rapports lignes
        private void CheckRapportLigneConccurency(IEnumerable<RapportLigneEnt> pointages)
        {
            var updatedPointages = pointages.Where(p => p.IsUpdated);
            if (updatedPointages != null && updatedPointages.Any())
            {
                var pointageFromDatabase = pointageManager.GetRapportLignesByIds(updatedPointages.Where(p => p.IsUpdated).Select(x => x.RapportLigneId).ToList());

                foreach (var pointage in updatedPointages)
                {
                    var pointageBD = pointageFromDatabase.FirstOrDefault(rl => rl.RapportLigneId == pointage.RapportLigneId);
                    if (pointageBD != null)
                    {
                        if ((pointageBD.AuteurModificationId != pointage.AuteurModificationId) &&
                            ((pointageBD.DateModification.HasValue && !pointage.DateModification.HasValue) ||
                            (pointageBD.DateModification.HasValue && pointage.DateModification.HasValue && pointageBD.DateModification > pointage.DateModification)))
                        {
                            throw new ValidationException(new List<ValidationFailure>()
                            {
                                new ValidationFailure("Pointage", FeatureRapport.Pointage_Concurrence_acces)
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remplir les informations des sorties astreintes
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <returns>Le rapport remplit</returns>
        public RapportEnt FulfillAstreintesInformations(RapportEnt rapport)
        {
            if (rapport?.ListLignes.Any() == true)
            {
                foreach (RapportLigneEnt rapportLigne in rapport.ListLignes)
                {
                    FulfillAstreintesInformations(rapportLigne, rapport.DateChantier);
                }
            }

            return rapport;
        }

        private void HandleCommentaire(RapportLigneEnt rapportLigne)
        {
            var rapportId = rapportLigne.RapportId;
            foreach (RapportLigneTacheEnt ligneTache in rapportLigne.ListRapportLigneTaches)
            {
                var rapportTache = this.rapportTacheRepository.GetByRapportIdAndTacheId(rapportId, ligneTache.TacheId);
                if (rapportTache != null)
                {
                    rapportTache.Commentaire = ligneTache.Commentaire;
                    if (rapportTache.Tache != null)
                    {
                        this.rapportTacheRepository.Update(rapportTache);
                    }
                }
                else
                {
                    rapportTache = new RapportTacheEnt { RapportId = rapportId, TacheId = ligneTache.TacheId, Commentaire = ligneTache.Commentaire };
                    this.rapportTacheRepository.Insert(rapportTache);
                }
            }
        }
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high

        /// <summary>
        ///   Change le statut d'un rapport
        /// </summary>
        /// <param name="rapport">Un rapport</param>
        /// <param name="rapportStatutCode">nouveau statut du rapport</param>
        private void ChangeRapportStatut(RapportEnt rapport, string rapportStatutCode)
        {
            // On passe le statut du rapport à valider
            RapportStatutEnt rapportStatut = rapportStatutManager.GetRapportStatutByCode(rapportStatutCode);
            if (rapportStatut != null)
            {
                rapport.RapportStatutId = rapportStatut.RapportStatutId;
                if (rapport.RapportId == 0)
                {
                    AddRapport(rapport);
                }
                else
                {
                    UpdateRapport(rapport);
                }
            }
        }

        private void InitNomTemporaire(RapportEnt rapport)
        {
            // On passe toutes les données en IsUpdated = true pour qu'il y ai modification lors des changements sur le rapport
            foreach (RapportLigneEnt ligne in rapport.ListLignes)
            {
                // Permet de récuperer PrenomNomTemporaire à partir de Personnel.PrenomNom
                if (ligne.Personnel != null && ligne.PersonnelId != null)
                {
                    ligne.PrenomNomTemporaire = ligne.Personnel.PrenomNom;
                }

                // Permet de récuperer MaterielNomTemporaire à partir de Materiel.LibelleRef
                if (ligne.Materiel != null && ligne.MaterielId != null)
                {
                    ligne.MaterielNomTemporaire = ligne.Materiel.LibelleLong;
                }
            }
        }

        private void InitHorairesRapport(RapportEnt rapport, CIEnt ci, RapportEnt duplicatedRapport = null)
        {
            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.RapportsHorairesObligatoires))
            {
                if (duplicatedRapport != null)
                {
                    // On récupère les horaires du rapport de la ligne d'origine
                    rapport.HoraireDebutM = duplicatedRapport.HoraireDebutM;
                    rapport.HoraireFinM = duplicatedRapport.HoraireFinM;
                    rapport.HoraireDebutS = duplicatedRapport.HoraireDebutS;
                    rapport.HoraireFinS = duplicatedRapport.HoraireFinS;
                }
                else
                {
                    // On récupère les horaires par défaut du CI
                    rapport.HoraireDebutM = ci.HoraireDebutM;
                    rapport.HoraireFinM = ci.HoraireFinM;
                    rapport.HoraireDebutS = ci.HoraireDebutS;
                    rapport.HoraireFinS = ci.HoraireFinS;
                }
            }
            else
            {
                // On récupère les horaires par défaut du CI sinon on applique les horaires par défaut
                rapport.HoraireDebutM = ci.HoraireDebutM ?? DateTime.UtcNow.Date.AddHours(8);
                rapport.HoraireFinM = ci.HoraireFinM ?? DateTime.UtcNow.Date.AddHours(12);
                rapport.HoraireDebutS = ci.HoraireDebutS ?? DateTime.UtcNow.Date.AddHours(14);
                rapport.HoraireFinS = ci.HoraireFinS ?? DateTime.UtcNow.Date.AddHours(18);
            }
        }

        private void SetOldRapportStatut(RapportEnt rapport)
        {
            // Remise du statut précédent du rapport
            RapportStatutEnt rapportStatut = rapportStatutManager.GetRapportStatutByCode(RapportStatutEnt.RapportStatutEnCours.Value);
            if (rapport.DateValidationCDC.HasValue)
            {
                rapportStatut = rapportStatutManager.GetRapportStatutByCode(RapportStatutEnt.RapportStatutValide1.Value);
            }

            if (rapport.DateValidationCDT.HasValue)
            {
                rapportStatut = rapportStatutManager.GetRapportStatutByCode(RapportStatutEnt.RapportStatutValide2.Value);
            }

            if (rapport.DateValidationDRC.HasValue)
            {
                rapportStatut = rapportStatutManager.GetRapportStatutByCode(RapportStatutEnt.RapportStatutValide3.Value);
            }

            rapport.RapportStatut = null;
            rapport.RapportStatutId = rapportStatut.RapportStatutId;
        }

        /// <summary>
        ///   Mise à jour des lignes de rapports
        /// </summary>
        /// <param name="rapport">Rapport dont les lignes sont à mettre à jour</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur courant</param>
        /// <param name="verrouillage">indique si on veut verrouille ou déverrouille le lot de pointage</param>
        /// <param name="save">Enregistre ou non en BD</param>
        private void UpdateLotPointage(RapportEnt rapport, int utilisateurId, bool verrouillage, bool save)
        {
            if (verrouillage)
            {
                DateTime periode = new DateTime(rapport.DateChantier.Year, rapport.DateChantier.Month, 1);
                int lotPointageId = lotPointageManager.GetorCreate(utilisateurId, periode).LotPointageId;

                foreach (var rl in rapport.ListLignes.ToList())
                {
                    rl.LotPointageId = lotPointageId;
                    rl.IsLotPointageIdUpdated = true;
                    if (save)
                    {
                        this.pointageManager.UpdatePointage(rl);
                    }
                }
            }
            else
            {
                foreach (var rl in rapport.ListLignes.ToList())
                {
                    rl.LotPointageId = null;
                    rl.IsLotPointageIdUpdated = true;
                    rl.PrenomNomTemporaire = rl.PersonnelId > 0 ? null : rl.PrenomNomTemporaire;
                    rl.MaterielNomTemporaire = rl.MaterielId > 0 ? null : rl.MaterielNomTemporaire;
                    if (save)
                    {
                        this.pointageManager.UpdatePointage(rl);
                    }
                }
            }
        }

        /// <summary>
        ///   Mise à jour des lignes de d'une liste de rapports
        /// </summary>
        /// <param name="rapports">Liste de Rapports dont les lignes sont à mettre à jour</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur courant</param>
        /// <param name="verrouillage">indique si on veut verrouille ou déverrouille le lot de pointage</param>        
        protected void UpdateLotPointage(List<RapportEnt> rapports, int utilisateurId, bool verrouillage)
        {
            List<string> periodes = rapports.GroupBy(x => x.DateChantier.ToString("yyyyMM")).Select(x => x.Key).Distinct().ToList();
            Dictionary<string, int?> periodLotPointageId = lotPointageManager.GetLotPointageId(utilisateurId, periodes);
            int? lotPointageId = null;

            foreach (var r in rapports)
            {
                if (verrouillage)
                {
                    lotPointageId = periodLotPointageId[r.DateChantier.ToString("yyyyMM")];
                    if (lotPointageId == null)
                    {
                        var lotPointage = new LotPointageEnt { Periode = new DateTime(r.DateChantier.Year, r.DateChantier.Month, 1), AuteurCreationId = utilisateurId };
                        lotPointage = lotPointageManager.AddLotPointage(lotPointage, utilisateurId);
                        lotPointageId = lotPointage.LotPointageId;
                        periodLotPointageId[r.DateChantier.ToString("yyyyMM")] = lotPointageId;
                    }
                }

                foreach (var rl in r.ListLignes.ToList())
                {
                    rl.LotPointageId = verrouillage ? lotPointageId : null;
                    rl.AuteurModification = null;
                    rl.AuteurModificationId = utilisateurId;
                    rl.DateModification = DateTime.UtcNow;
                }
            }
        }

        private RapportEnt GenerateRapportFromNewPointagePersonnel(RapportLigneEnt pointage, RapportEnt duplicatedRapport)
        {
            var userId = utilisateurManager.GetContextUtilisateurId();
            var personnel = personnelManager.GetPersonnelById(userId);
            var ci = ciManager.GetCIById(pointage.CiId, byPassCheckAccess: true);
            if (personnel.Societe?.Groupe?.Code == Constantes.CodeGroupeFES)
            {
                return ProcessSaveRapportFesParPersonnelStatut(pointage, ci, userId, duplicatedRapport);
            }
            else
            {
                var rapport = Repository.GetRapportByCiIdAndDate(ci.CiId, pointage.DatePointage);
                //RG_6472_008
                pointage.ContratId = GetContratInterimaireId(pointage);
                if (ci.Societe?.Groupe?.Code == Constantes.CodeGroupeFES && rapport != null)
                {
                    var saveDate = DateTime.UtcNow;
                    pointage.AuteurCreationId = userId;
                    pointage.DateCreation = saveDate;
                    rapport.AuteurModificationId = userId;
                    rapport.DateModification = saveDate;
                    if (rapport.ListLignes != null)
                    {
                        rapport.ListLignes.Add(pointage);
                    }
                    else
                    {
                        rapport.ListLignes = new List<RapportLigneEnt>() { pointage };
                    }
                    UpdateLotPointage(rapport, userId, true, save: false);
                    return rapport;
                }
                else
                {
                    rapport = new RapportEnt();
                    rapport.CiId = pointage.CiId;
                    rapport.AuteurCreationId = userId;
                    rapport.DateCreation = DateTime.UtcNow;
                    pointage.AuteurCreationId = rapport.AuteurCreationId;
                    pointage.DateCreation = rapport.DateCreation;
                    rapport.AuteurVerrouId = rapport.AuteurCreationId;
                    rapport.DateVerrou = DateTime.UtcNow;
                    rapport.DateChantier = pointage.DatePointage;
                    rapport.IsGenerated = ci.Societe?.Groupe?.Code != Constantes.CodeGroupeFES;
                    rapport.RapportStatutId = RapportStatutEnt.RapportStatutVerrouille.Key;
                    InitHorairesRapport(rapport, ci, duplicatedRapport);
                    rapport.TypeStatutRapport = (int)TypeStatutRapport.Default;
                    rapport.ListLignes = new List<RapportLigneEnt>() { pointage };
                    foreach (RapportLigneTacheEnt ligneTache in pointage.ListRapportLigneTaches)
                    {
                        var rapportTache = new RapportTacheEnt();
                        rapportTache.RapportId = rapport.RapportId;
                        rapportTache.TacheId = ligneTache.TacheId;
                        rapportTache.Commentaire = ligneTache.Commentaire;
                    }
                    foreach (RapportLigneMajorationEnt ligneMajoration in pointage.ListRapportLigneMajorations)
                    {
                        ligneMajoration.CodeMajoration = null;
                    }
                    UpdateLotPointage(rapport, userId, true, save: false);
                    return rapport;
                }
            }
        }

        /// <summary>
        /// Ajout ou mise à jour en masse
        /// </summary>
        /// <param name="rapports">Liste de rapports</param>
        public void AddOrUpdateRapportList(IEnumerable<RapportEnt> rapports)
        {
            int count = 0;
            const int commitCount = 100;

            foreach (var item in rapports.ToList())
            {
                ++count;
                Repository.Update(item);

                // A chaque 100 opérations, on sauvegarde le contexte.
                if (count % commitCount == 0)
                {
                    Save();
                }
            }
            Save();
        }

        /// <summary>
        /// Vérifie les écarts éventuel de status entre l'état du rapport avant et apres update
        /// </summary>
        /// <param name="rapport">Le rapport mis à jour</param>
        /// <param name="rapportBeforeUpdate">Le rapport avant la mise à jour (en base de données)</param>
        /// <exception>ValidationException si les statuts sont différents</exception>
        public void CheckRapportStatutChangedInDb(RapportEnt rapport, RapportEnt rapportBeforeUpdate)
        {
            if (rapport.RapportStatutId != rapportBeforeUpdate.RapportStatutId)
            {
                throw new FluentValidation.ValidationException(new List<ValidationFailure>()
                {
                    new ValidationFailure("StatusChanged",string.Format(FeatureRapport.Rapport_Detail_Statut_Modifier_Erreur, rapportBeforeUpdate.RapportStatut.Libelle))
                });
            }
        }

        /// <summary>
        /// Vérifie les mises à jour de Materiel en comparant les lignes de rapport avant et apres update
        /// </summary>
        /// <param name="rapport">Le rapport mis à jour</param>
        /// <param name="rapportBeforeUpdate">Le rapport avant la mise à jour (en base de données)</param>
        /// <returns>un booleen indiquant si des modifications existent</returns>
        public bool CheckRapportLignesMaterielChanged(RapportEnt rapport, RapportEnt rapportBeforeUpdate)
        {
            // recherche des modifications de materiel
            var lignesWithMateriel = rapport.ListLignes.Where(l => l.MaterielId != null && l.DateSuppression == null)
                                .Select(l => new { l.RapportLigneId, l.MaterielId, l.MaterielMarche, l.MaterielArret, l.MaterielIntemperie, l.MaterielPanne });
            var lignesWithMaterielBeforeUpdate = rapportBeforeUpdate.ListLignes.Where(l => l.MaterielId != null && l.DateSuppression == null)
                                             .Select(l => new { l.RapportLigneId, l.MaterielId, l.MaterielMarche, l.MaterielArret, l.MaterielIntemperie, l.MaterielPanne });
            return lignesWithMateriel.Except(lignesWithMaterielBeforeUpdate).Any()
                || lignesWithMaterielBeforeUpdate.Except(lignesWithMateriel).Any();
        }

        /// <summary>
        /// Vérifie les mises à jour de Personnel en comparant les lignes de rapport avant et apres update
        /// </summary>
        /// <param name="rapport">Le rapport mis à jour</param>
        /// <param name="rapportBeforeUpdate">Le rapport avant la mise à jour (en base de données)</param>
        /// <returns>un booleen indiquant si des modifications existent</returns>
        public bool CheckRapportLignesPersonnelChanged(RapportEnt rapport, RapportEnt rapportBeforeUpdate)
        {
            // recherche des modifications sur le personnel
            var lignesWithPersonnel = rapport.ListLignes.Where(l => l.PersonnelId != null && l.DateSuppression == null)
                                            .SelectMany(l => l.ListRapportLigneTaches.Select(t => new { l.RapportLigneId, l.PersonnelId, t.TacheId, t.HeureTache }));
            var lignesWithPersonnelBeforeUpdate = rapportBeforeUpdate.ListLignes.Where(l => l.PersonnelId != null && l.DateSuppression == null)
                                            .SelectMany(l => l.ListRapportLigneTaches.Select(t => new { l.RapportLigneId, l.PersonnelId, t.TacheId, t.HeureTache }));
            return lignesWithPersonnel.Except(lignesWithPersonnelBeforeUpdate).Any()
                || lignesWithPersonnelBeforeUpdate.Except(lignesWithPersonnel).Any();
        }

        /// <summary>
        /// Controle du nombre d'heures travaillées pour chaque ligne de rapport
        /// </summary>
        /// <param name="rapport">rapport</param>
        /// <param name="userId">id du user</param>
        /// <exception>ValidationException si les heures travaillées sont différentes des heures normales</exception>
        private void CheckRapportLignesPersonnelHeuresErrors(RapportEnt rapport, int userId)
        {
            // Pas de contrôle pour les roles paie
            if (this.utilisateurManager.IsRolePaie(userId, rapport.CiId))
            {
                return;
            }

            // vérifie la répartition des heures personnel pour chaque ligne
            foreach (var rapportLigne in rapport.ListLignes.Where(x => x.PersonnelId.HasValue))
            {
                var totalHeuresTaches = rapportLigne.ListRapportLigneTaches.Where(x => !x.IsDeleted).Sum(x => x.HeureTache);
                if (totalHeuresTaches.IsNotEqual(rapportLigne.HeureNormale + rapportLigne.HeureMajoration))
                {
                    throw new FluentValidation.ValidationException(new List<ValidationFailure>()
                    {
                        new ValidationFailure("HeuresInvalides", FeatureRapport.Rapport_Detail_Somme_Heure_Erreur)
                    });
                }
            }
        }

        /// <summary>
        /// Ajout en masse des rapport
        /// </summary>
        /// <param name="rapportList">Liste des rapports à ajouter en masse</param>
        public void AddRangeRapportList(IEnumerable<RapportEnt> rapportList)
        {
            try
            {
                if (!rapportList.IsNullOrEmpty())
                {
                    int count = 0;
                    const int commitCount = 100;

                    foreach (var item in rapportList.ToList())
                    {
                        ++count;
                        Repository.Insert(item);

                        // A chaque 100 opérations, on sauvegarde le contexte.
                        if (count % commitCount == 0)
                        {
                            Save();
                        }
                    }
                    Save();
                }
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Renvoie la liste des rapport entre 2 dates . Les rapports retournés conçernent les ci envoyés
        /// </summary>
        /// <param name="ciList">Liste des Cis</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>List de rapports en les 2 dates</returns>
        public IEnumerable<RapportEnt> GetRapportListBetweenDatesByCiList(IEnumerable<int> ciList, DateTime startDate, DateTime endDate)
        {
            if (ciList.IsNullOrEmpty())
            {
                return new List<RapportEnt>();
            }

            return Repository.GetRapportListBetweenDatesByCiList(ciList, startDate, endDate);
        }

        /// <summary>
        ///   Ajoute une tache au paramétrage du rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <param name="tache">La tache</param>
        /// <param name="heureTache">heure tache</param>
        /// <returns>Un rapport</returns>
        public RapportEnt AddTacheToRapportFiggo(RapportEnt rapport, TacheEnt tache, double heureTache)
        {
            //Ajout de la tache à chaque ligne de rapport
            foreach (RapportLigneEnt pointage in rapport.ListLignes)
            {
                if (!pointage.ListRapportLigneTaches.Any(t => t.TacheId == tache.TacheId))
                {
                    pointage.ListRapportLigneTaches.Add(this.pointageManager.GetNewPointageTacheFiggo(pointage, tache, heureTache));
                }
                else
                {
                    pointage.ListRapportLigneTaches.FirstOrDefault(m => m.TacheId == tache.TacheId).IsDeleted = false;
                }
            }
            return rapport;
        }

        private void SaveRapportJournalierParStatutPersonnel(RapportEnt rapport, RapportEnt rapportParStatut, string statut, int userId)
        {
            rapportParStatut.Cloture = Utilities.IsTodayInPeriodeCloture(rapport);
            rapportParStatut.ListLignes.ForEach(l => l.Cloture = rapport.Cloture);
            // La re-récupération du CI permet de vérifier si l'utilisateur actuel y a accès (et accessoirement de résoudre un pb d'EF)
            if (rapportParStatut.CI == null)
            {
                rapportParStatut.CI = this.ciManager.GetCIById(rapport.CiId);
                rapportParStatut.CiId = rapport.CiId;
            }
            //Récupération du statut en cours pour les nouveaux rapports
            if (rapportParStatut.RapportStatutId == 0)
            {
                rapportParStatut.RapportStatutId = RapportStatutEnt.RapportStatutEnCours.Key;
            }
            var Datenow = DateTime.UtcNow;
            rapportParStatut.AuteurCreationId = userId;
            rapportParStatut.DateCreation = Datenow;
            rapportParStatut.DateChantier = rapport.DateChantier.Date;
            rapportParStatut = RapportStatutHelper.CheckPersonnelStatut(rapportParStatut, statut);
            rapportParStatut.ListLignes.ToList().ForEach(l => l.DatePointage = rapport.DateChantier);
            rapportParStatut.ListLignes.ToList().ForEach(l => l.DateCreation = Datenow);
            rapportParStatut.ListLignes.ToList().ForEach(l => l.AuteurCreationId = userId);
            rapportParStatut.ListLignes.ToList().ForEach(l => l.CiId = rapport.CiId);
            rapportParStatut.ListLignes.ToList().ForEach(l => this.pointageManager.TraiteDonneesPointage(l));
            rapportParStatut.ListLignes.ForEach(l => l.ListRapportLigneMajorations?.ForEach(m => m.CodeMajoration = null));
            // RG_6472_008 
            rapportParStatut.ListLignes.ForEach(l => l.ContratId = GetContratInterimaireId(l));
            rapportParStatut.CleanLinkedProperties(true);

            //Ajout du rapport
            Repository.Insert(rapportParStatut);
            valorisationManager.CreateValorisation(rapportParStatut.ListLignes, "Rapport");

            //Sauvegarde des traitements en base de données
            Save();
        }
        private RapportEnt FiltreRapportPersonnelStatut(RapportEnt rapport, int userId)
        {
            RapportEnt rapportIac = new RapportEnt();
            RapportEnt rapportEtam = new RapportEnt();
            RapportEnt rapportOuvrier = new RapportEnt();
            List<int?> listPersonnelId = rapport.ListLignes.Select(x => x.PersonnelId).ToList();
            List<PersonnelEnt> listPersonnel = personnelManager.GetPersonnelByListPersonnelId(listPersonnelId);
            foreach (var item in listPersonnel)
            {
                if (item.Statut == TypePersonnel.Ouvrier)
                {
                    rapportOuvrier.ListLignes.Add(rapport.ListLignes.FirstOrDefault(p => p.PersonnelId == item.PersonnelId));
                }
                if (item.Statut == TypePersonnel.ETAM)
                {
                    rapportEtam.ListLignes.Add(rapport.ListLignes.FirstOrDefault(p => p.PersonnelId == item.PersonnelId));
                }
                if (item.Statut == TypePersonnel.Cadre)
                {
                    rapportIac.ListLignes.Add(rapport.ListLignes.FirstOrDefault(p => p.PersonnelId == item.PersonnelId));
                }
            }

            if (rapportIac.ListLignes.Any())
            {
                SaveRapportJournalierParStatutPersonnel(rapport, rapportIac, TypePersonnel.Cadre, userId);
            }
            if (rapportEtam.ListLignes.Any())
            {
                SaveRapportJournalierParStatutPersonnel(rapport, rapportEtam, TypePersonnel.ETAM, userId);
            }
            if (rapportOuvrier.ListLignes.Any())
            {
                SaveRapportJournalierParStatutPersonnel(rapport, rapportOuvrier, TypePersonnel.Ouvrier, userId);
            }

            if (rapportIac.ListLignes.Any())
            {
                return GetRapportById(rapportIac.RapportId, true);
            }
            else if (rapportEtam.ListLignes.Any())
            {
                return GetRapportById(rapportEtam.RapportId, true);
            }
            if (rapportOuvrier.ListLignes.Any())
            {
                return GetRapportById(rapportOuvrier.RapportId, true);
            }
            return rapport;
        }

        private RapportEnt CheckRapportStatut(string personnelStatut, int ciId, DateTime datePointage)
        {
            if (personnelStatut == Constantes.TypePersonnel.Ouvrier)
            {
                return Repository.GetRapportByCiIdAndDate(ciId, datePointage, TypeRapportStatut.Ouvrier);
            }
            if (personnelStatut == Constantes.TypePersonnel.ETAM)
            {
                return Repository.GetRapportByCiIdAndDate(ciId, datePointage, TypeRapportStatut.ETAM);
            }
            if (personnelStatut == Constantes.TypePersonnel.Cadre)
            {
                return Repository.GetRapportByCiIdAndDate(ciId, datePointage, TypeRapportStatut.Cadre);
            }
            return new RapportEnt();
        }

        private RapportEnt ProcessSaveRapportFesParPersonnelStatut(RapportLigneEnt pointage, CIEnt ci, int userId, RapportEnt duplicatedRapport)
        {
            var personnelStatut = personnelManager.GetPersonnelById(pointage.PersonnelId).Statut;
            RapportEnt rapport = CheckRapportStatut(personnelStatut, ci.CiId, pointage.DatePointage);
            if (rapport != null)
            {
                var saveDate = DateTime.UtcNow;
                pointage.AuteurCreationId = userId;
                pointage.DateCreation = saveDate;
                rapport.AuteurModificationId = userId;
                rapport.DateModification = saveDate;
                if (rapport.ListLignes != null)
                {
                    rapport.ListLignes.Add(pointage);
                }
                else
                {
                    rapport.ListLignes = new List<RapportLigneEnt>() { pointage };
                }
                UpdateLotPointage(rapport, userId, true, save: false);
                return rapport;
            }
            else
            {
                rapport = new RapportEnt();
                var personnel = personnelManager.GetPersonnelById(pointage.PersonnelId.Value);
                rapport.CiId = pointage.CiId;
                rapport.AuteurCreationId = userId;
                rapport.DateCreation = DateTime.UtcNow;
                pointage.AuteurCreationId = rapport.AuteurCreationId;
                pointage.DateCreation = rapport.DateCreation;
                rapport.AuteurVerrouId = rapport.AuteurCreationId;
                rapport.DateVerrou = DateTime.UtcNow;
                rapport.DateChantier = pointage.DatePointage;
                rapport.IsGenerated = ci.Societe?.Groupe?.Code != Constantes.CodeGroupeFES;
                rapport.RapportStatutId = RapportStatutEnt.RapportStatutVerrouille.Key;
                rapport = RapportStatutHelper.CheckPersonnelStatut(rapport, personnel.Statut);
                InitHorairesRapport(rapport, ci, duplicatedRapport);
                rapport.ListLignes = new List<RapportLigneEnt>() { pointage };
                rapport.ListLignes.ForEach(x => x.RapportLigneStatutId = RapportStatutEnt.RapportStatutVerrouille.Key);
                foreach (RapportLigneTacheEnt ligneTache in pointage.ListRapportLigneTaches)
                {
                    var rapportTache = new RapportTacheEnt();
                    rapportTache.RapportId = rapport.RapportId;
                    rapportTache.TacheId = ligneTache.TacheId;
                    rapportTache.Commentaire = ligneTache.Commentaire;
                }
                foreach (RapportLigneMajorationEnt ligneMajoration in pointage.ListRapportLigneMajorations)
                {
                    ligneMajoration.CodeMajoration = null;
                }
                UpdateLotPointage(rapport, userId, true, save: false);
                return rapport;
            }
        }
    }
}
