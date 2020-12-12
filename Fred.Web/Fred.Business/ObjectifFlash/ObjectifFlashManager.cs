using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Depense;
using Fred.Business.Depense.Services;
using Fred.Business.ObjectifFlash.Models;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ObjectifFlash;
using Fred.Entities.ObjectifFlash.Search;
using Fred.Framework.DateTimeExtend;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Depense;
using MoreLinq;

namespace Fred.Business.ObjectifFlash
{
    public class ObjectifFlashManager : Manager<ObjectifFlashEnt, IObjectifFlashRepository>, IObjectifFlashManager
    {
        private readonly IUtilisateurManager userManager;
        private readonly IDateTimeExtendManager dateTimeExtendManager;
        private readonly IDepenseServiceMediator depenseServiceMediator;

        public ObjectifFlashManager(
          IUnitOfWork uow,
          IObjectifFlashRepository objectifFlashRepository,
          IObjectifFlashValidator validator,
          IUtilisateurManager userManager,
          IDateTimeExtendManager dateTimeExtendManager,
          IDepenseServiceMediator depenseServiceMediator)
          : base(uow, objectifFlashRepository, validator)
        {
            this.userManager = userManager;
            this.dateTimeExtendManager = dateTimeExtendManager;
            this.depenseServiceMediator = depenseServiceMediator;
        }

        #region Search ObjectifFlash

        /// <summary>
        /// Récupère la liste des objectifs flashs filtrés en fonction des critères
        /// </summary>
        /// <param name="filter">filtres</param>
        /// <param name="page">page number</param>
        /// <param name="pageSize">page size</param>
        /// <exception cref="ValidationException">Erreur levée si la date de début est postérieure à la date de fin</exception>
        /// <returns>Liste des Objectif Flashs</returns>
        public async Task<SearchObjectifFlashListWithFilterResult> SearchObjectifFlashListWithFilterAsync(SearchObjectifFlashEnt filter, int page = 1, int pageSize = 20)
        {
            if (filter.DateDebut.HasValue && filter.DateFin.HasValue && filter.DateDebut.Value.Date > filter.DateFin.Value.Date)
            {
                throw new ValidationException(
                  new List<ValidationFailure>
                  {
                      new ValidationFailure("datesFilter", FeatureObjectifFlash.ObjectifFlash_Error_DateDebutPosterieureDateFin)
                  });
            }
            var currentUser = userManager.GetContextUtilisateur();
            filter.UserCiIds = userManager.GetAllCIbyUser(currentUser.UtilisateurId).ToList();

            var objectifFlashList = Repository.SearchObjectifFlashListWithFilter(filter, page, pageSize);

            var depenses = await GetObjectifFlashListDepensesAsync(objectifFlashList.Items).ConfigureAwait(false);

            foreach (var objectifFlash in objectifFlashList.Items)
            {
                objectifFlash.TotalMontantRealise = 0;
                foreach (var tache in objectifFlash.Taches ?? Enumerable.Empty<ObjectifFlashTacheEnt>())
                {

                    objectifFlash.TotalMontantRealise += depenses
                                                            .Where(x => x.TacheId == tache.TacheId
                                                                && x.CiId == objectifFlash.CiId
                                                                && x.Date.Date >= objectifFlash.DateDebut
                                                                && x.Date.Date <= objectifFlash.DateFin)
                                                         .Sum(x => x.MontantHT);
                }
                objectifFlash.EcartRealiseObjectif = (objectifFlash.TotalMontantRealise ?? 0) - (objectifFlash.TotalMontantObjectif ?? 0);
            }

            return objectifFlashList;
        }

        /// <summary>
        ///   Retourne une nouvelle recherche d'Objectif flash initialisée.
        /// </summary>
        /// <returns>Une nouvelle recherche d'Objectif flash</returns>
        public SearchObjectifFlashEnt GetNewFilter()
        {
            return new SearchObjectifFlashEnt
            {
                CI = null,
                DateDebut = null,
                DateFin = null,
                DisplayClosed = false
            };
        }

        #endregion Search ObjectifFlash

        #region ObjectifFlash
        /// <summary>
        /// Récupère un objectif flash et ses taches/ressources par son id
        /// </summary>
        /// <param name="objectifFlashId">Identifiant d'objectif flash</param>
        /// <returns>ObjectifFlashEnt</returns>
        public ObjectifFlashEnt GetObjectifFlashWithTachesById(int objectifFlashId)
        {
            ObjectifFlashEnt objectifFlash = this.Repository.GetObjectifFlashWithTachesById(objectifFlashId);
            CalculateObjectifFlashProperties(objectifFlash);
            ((IObjectifFlashValidator)this.Validator).CheckObjectifFlashErrors(objectifFlash);
            return objectifFlash;
        }

        /// <summary>
        /// Retourne l'objectif flash portant l'identifiant unique indiqué avec taches et realisations.
        /// </summary>
        /// <param name="objectifFlashId">Identifiant de l'objectif flash à retrouver.</param>
        /// <param name="dateDebut">date de début de réalisations</param>
        /// <param name="dateFin">date de fin de réalisations</param>
        /// <returns>L'objectif flash retrouvée, sinon null.</returns>
        public ObjectifFlashEnt GetObjectifFlashWithRealisationsById(int objectifFlashId, DateTime dateDebut, DateTime dateFin)
        {
            ObjectifFlashEnt objectifFlash = this.Repository.GetObjectifFlashWithRealisationsById(objectifFlashId);
            CalculateObjectifFlashProperties(objectifFlash);
            foreach (var objectifFlashTache in objectifFlash.Taches)
            {
                objectifFlashTache.TacheJournalisations = objectifFlashTache.TacheJournalisations
                                                            .Where(x => x.DateJournalisation >= dateDebut
                                                                     && x.DateJournalisation <= dateFin)
                                                            .ToList();

            }

            return objectifFlash;
        }


        /// <summary>
        /// Calcul des propriétés d'objectif flash, totaux, flags, tri des collections
        /// </summary>
        /// <param name="objectifFlash">objectif flash</param>
        private void CalculateObjectifFlashProperties(ObjectifFlashEnt objectifFlash)
        {
            if (objectifFlash == null)
            {
                return;
            }
            foreach (var tache in objectifFlash.Taches)
            {
                // order des journalisations de tache
                tache.TacheJournalisations = tache.TacheJournalisations.OrderBy(x => x.DateJournalisation).ToList();
                foreach (var ressource in tache.Ressources)
                {
                    // définition du code chapitre au niveau de l'objectif flash ressource
                    ressource.ChapitreCode = ressource.Ressource?.SousChapitre?.Chapitre?.Code;

                    // order des journalisations de ressources
                    ressource.TacheRessourceJournalisations = ressource.TacheRessourceJournalisations.OrderBy(x => x.DateJournalisation).ToList();
                }

                // order des ressources par chapitre
                tache.Ressources = tache.Ressources.OrderBy(x => x.ChapitreCode).ToList();
            }

            // liste des journalisations
            if (objectifFlash.Taches.Any(x => x.TacheJournalisations.Any()))
            {
                objectifFlash.Journalisations = GetJournalisationsForDates(objectifFlash.DateDebut, objectifFlash.DateFin);
            }

            CalculateTotalValues(objectifFlash);

            // définit le flag de cloture
            objectifFlash.IsClosed = objectifFlash.DateCloture != null;
        }

        /// <summary>
        /// Calcul des totaux
        /// </summary>
        /// <param name="objectifFlash">objectif flash</param>
        private void CalculateTotalValues(ObjectifFlashEnt objectifFlash)
        {
            // totaux hors journalisation
            foreach (var tache in objectifFlash.Taches)
            {
                tache.TotalMontantRessource = tache.Ressources.Sum(x => x.QuantiteObjectif * x.PuHT);
            }
            objectifFlash.TotalMontantObjectif = objectifFlash.Taches.Sum(x => x.TotalMontantRessource);
            objectifFlash.TotalMontantJournalise = objectifFlash.TotalMontantObjectif;


            if (objectifFlash.Journalisations == null || !objectifFlash.Journalisations.Any())
            {
                return;
            }

            // Totaux journalisation par jours
            foreach (var journalisation in objectifFlash.Journalisations)
            {
                journalisation.TotalMontant = 0;
                foreach (var tache in objectifFlash.Taches)
                {
                    var tacheJournalisation = tache.TacheJournalisations.SingleOrDefault(x => x.DateJournalisation == journalisation.DateJournalisation);
                    if (tacheJournalisation == null)
                    {
                        continue;
                    }
                    tacheJournalisation.TotalMontantRessource = tache.Ressources.SelectMany(ressource => ressource.TacheRessourceJournalisations.Where(x => x.DateJournalisation == tacheJournalisation.DateJournalisation),
                                                                                (ressource, ressourceJournalisation) => new { PuHT = ressource.PuHT, QuantiteObjectif = ressourceJournalisation.QuantiteObjectif })
                                                                                .Sum(x => x.PuHT * x.QuantiteObjectif);
                    journalisation.TotalMontant += tacheJournalisation.TotalMontantRessource;
                    tache.TotalQuantiteJournalise = tache.TacheJournalisations.Sum(x => x.QuantiteObjectif);
                }
            }

            // Totaux journalisation par tache/ressources
            foreach (var tache in objectifFlash.Taches)
            {
                foreach (var ressource in tache.Ressources)
                {
                    ressource.TotalQuantiteJournalise = ressource.TacheRessourceJournalisations.Sum(x => x.QuantiteObjectif);
                }
                tache.TotalMontantJournalise = tache.Ressources.SelectMany(ressource => ressource.TacheRessourceJournalisations,
                                                                            (ressource, ressourceJournalisation) => new { PuHT = ressource.PuHT, QuantiteObjectif = ressourceJournalisation.QuantiteObjectif })
                                                                            .Sum(x => x.PuHT * x.QuantiteObjectif);
            }

        }

        /// <summary>
        /// Calcul du total montant objectif uniquement
        /// </summary>
        /// <param name="objectifFlash">objectif flash</param>
        private void CalculateTotalMontantObjectif(ObjectifFlashEnt objectifFlash)
        {
            objectifFlash.TotalMontantObjectif = objectifFlash.Taches.SelectMany(x => x.Ressources).Sum(x => x.QuantiteObjectif ?? 0 * x.PuHT ?? 0);
        }

        /// <summary>
        /// Retourne un nouvel objectif flash
        /// </summary>
        /// <returns>ObjectifFlashEnt</returns>
        public ObjectifFlashEnt GetNewObjectifFlash()
        {
            return new ObjectifFlashEnt
            {
                DateDebut = DateTime.Now.Date,
                DateFin = DateTime.Now.Date.AddDays(1)
            };
        }

        /// <summary>
        /// Ajout d'un nouvel objectif flash
        /// </summary>
        /// <param name="objectifFlash">objectif flash</param>
        /// <returns>ObjectifFlashEnt</returns>
        public ObjectifFlashEnt AddObjectifFlash(ObjectifFlashEnt objectifFlash)
        {
            if (objectifFlash == null)
            {
                throw new ArgumentNullException(nameof(objectifFlash));
            }

            objectifFlash.DateCreation = DateTime.UtcNow;
            objectifFlash.AuteurCreationId = this.userManager.GetContextUtilisateurId();

            BusinessValidation(objectifFlash);
            objectifFlash.DateDebut = objectifFlash.DateDebut.Date;
            objectifFlash.DateFin = objectifFlash.DateFin.Date;
            this.JournalisationsInitialize(objectifFlash);
            this.JournalisationQuantiteAjustement(objectifFlash);
            this.CalculateTotalMontantObjectif(objectifFlash);
            this.Repository.AddObjectifFlash(objectifFlash);
            Save();

            return GetObjectifFlashWithTachesById(objectifFlash.ObjectifFlashId);

        }


        /// <summary>
        /// Update d'un objectif flash
        /// </summary>
        /// <param name="objectifFlash">objectif flash à updater</param>
        /// <returns>objectif flash updaté</returns>
        public ObjectifFlashEnt UpdateObjectifFlash(ObjectifFlashEnt objectifFlash)
        {
            if (objectifFlash == null)
            {
                throw new ArgumentNullException(nameof(objectifFlash));
            }

            var objectifFlashInDb = this.Repository.GetObjectifFlashWithTachesById(objectifFlash.ObjectifFlashId);


            objectifFlash.AuteurModificationId = this.userManager.GetContextUtilisateurId();
            objectifFlash.DateModification = DateTime.UtcNow;
            this.JournalisationsInitialize(objectifFlash);
            this.JournalisationQuantiteAjustement(objectifFlash);
            this.CalculateTotalMontantObjectif(objectifFlash);

            // changement de l'état cloturé, gestion de la date de cloture
            if (!objectifFlashInDb.IsClosed && objectifFlash.IsClosed)
            {
                objectifFlash.DateCloture = DateTime.UtcNow;
            }
            // desactivation des objectif flash cloturés
            if (objectifFlash.IsClosed)
            {
                objectifFlash.IsActif = false;
            }

            // Validation avant enregistrement
            BusinessValidation(objectifFlash);
            this.Repository.UpdateObjectifFlash(objectifFlash, objectifFlashInDb);
            return GetObjectifFlashWithTachesById(objectifFlash.ObjectifFlashId);
        }

        /// <summary>
        /// Activation d'un objectif flash
        /// </summary>
        /// <param name="objectifFlashId">identifiant d'objectif flash</param>
        /// <returns>true si activé</returns>
        public bool ActivateObjectifFlash(int objectifFlashId)
        {
            var objectifFlashInDb = this.Repository.GetObjectifFlashWithTachesById(objectifFlashId);
            objectifFlashInDb.IsActif = true;
            BusinessValidation(objectifFlashInDb);
            this.Save();
            return true;
        }

        /// <summary>
        /// Duplication d'un objectif flash
        /// </summary>
        /// <param name="objectifFlashId">identifiant d'objectif flash</param>
        /// <param name="dateDebut">date de début</param>
        /// <returns>identifiant de l'objectif flash dupliqué</returns>
        public int DuplicateObjectifFlash(int objectifFlashId, DateTime dateDebut)
        {
            var objectifFlashToDuplicate = this.Repository.GetObjectifFlashWithTachesByIdNoTracking(objectifFlashId);
            CalculateObjectifFlashProperties(objectifFlashToDuplicate);
            // suppression des flags
            objectifFlashToDuplicate.IsActif = false;
            objectifFlashToDuplicate.DateCloture = null;
            objectifFlashToDuplicate.DateSuppression = null;
            // suppression des ids
            objectifFlashToDuplicate.ObjectifFlashId = 0;
            foreach (var tache in objectifFlashToDuplicate.Taches)
            {
                tache.ObjectifFlashTacheId = 0;
                tache.TacheJournalisations.ForEach(x => x.ObjectifFlashTacheJournalisationId = 0);
                foreach (var ressource in tache.Ressources)
                {
                    ressource.ObjectifFlashTacheRessourceId = 0;
                    ressource.TacheRessourceJournalisations.ForEach(x => x.ObjectifFlashTacheRessourceId = 0);
                }
            }
            objectifFlashToDuplicate.CleanProperties();
            objectifFlashToDuplicate = this.GetReportJournalisation(objectifFlashToDuplicate, dateDebut);
            Repository.AddObjectifFlash(objectifFlashToDuplicate);
            Save();
            return objectifFlashToDuplicate.ObjectifFlashId;
        }

        /// <summary>
        ///   Supprime logiquement un Objectif flash.
        ///   conserver les ressources / taches associées pour réactivation éventuelle
        /// </summary>
        /// <param name="objectifFlashId">L'identifiant du Objectif flash à supprimer.</param>
        public void DeleteObjectifFlashById(int objectifFlashId)
        {
            ObjectifFlashEnt objectifFlash = Repository.GetObjectifFlashById(objectifFlashId);
            if (objectifFlash == null)
            {
                throw new ArgumentException("ObjectifFlash is null");
            }

            objectifFlash.DateSuppression = DateTime.UtcNow;
            objectifFlash.AuteurSuppressionId = this.userManager.GetContextUtilisateurId();

            BusinessValidation(objectifFlash);
            this.Save();
        }


        #endregion ObjectifFlash

        #region Journalisation

        /// <summary>
        /// Retourne un objectif flash journalisé
        /// </summary>
        /// <param name="objectifFlashEnt">objectif flash</param>
        /// <returns>objectif flash journalisé</returns>
        public ObjectifFlashEnt GetNewJournalisation(ObjectifFlashEnt objectifFlashEnt)
        {
            // check values
            if (objectifFlashEnt.Taches.Any(t => !t.QuantiteObjectif.HasValue))
            {
                ThrowBusinessValidationException("taches_quantites", "Toutes les quandités de taches doivent etre définies pour effectuer la journalisation");
            }

            if (objectifFlashEnt.Taches.Any(t => t.Ressources.Any(r => !r.QuantiteObjectif.HasValue)))
            {
                ThrowBusinessValidationException("ressources_quantites", "Toutes les quandités de ressources doivent etre définies pour effectuer la journalisation");
            }

            objectifFlashEnt.Journalisations = GetJournalisationsForDates(objectifFlashEnt.DateDebut, objectifFlashEnt.DateFin);
            if (!objectifFlashEnt.Journalisations.Any())
            {
                return objectifFlashEnt;
            }

            var totalWorkingDays = objectifFlashEnt.Journalisations.Count(x => !x.IsWeekEndOrHoliday);
            var lastWorkingDay = objectifFlashEnt.Journalisations.LastOrDefault(x => !x.IsWeekEndOrHoliday);

            foreach (var tache in objectifFlashEnt.Taches)
            {
                var quantiteTacheJournalise = totalWorkingDays == 0 ? 0 : Math.Round(((tache.QuantiteObjectif ?? 0) / totalWorkingDays), 3);
                tache.TacheJournalisations = this.CreateTacheJournalisations(tache, objectifFlashEnt.Journalisations, quantiteTacheJournalise);
                // relicat sur le dernier jour ouvré
                tache.TacheJournalisations.LastOrDefault(x => lastWorkingDay == null || x.DateJournalisation == lastWorkingDay?.DateJournalisation).QuantiteObjectif += tache.QuantiteObjectif - tache.TacheJournalisations.Sum(x => x.QuantiteObjectif);
                foreach (var ressource in tache.Ressources)
                {
                    var quantiteRessourceJournalise = totalWorkingDays == 0 ? 0 : Math.Round(((ressource.QuantiteObjectif ?? 0) / totalWorkingDays), 3);
                    ressource.TacheRessourceJournalisations = this.CreateRessourceJournalisations(ressource, objectifFlashEnt.Journalisations, quantiteRessourceJournalise);
                    // relicat sur le dernier jour ouvré
                    ressource.TacheRessourceJournalisations.LastOrDefault(x => lastWorkingDay == null || x.DateJournalisation == lastWorkingDay?.DateJournalisation).QuantiteObjectif += ressource.QuantiteObjectif - ressource.TacheRessourceJournalisations.Sum(x => x.QuantiteObjectif);
                }
            }
            this.CalculateTotalValues(objectifFlashEnt);
            return objectifFlashEnt;
        }

        /// <summary>
        /// Retourne un objectif flash journalisé reporté à partir de la nouvelle date fournie.
        /// </summary>
        /// <param name="objectifFlash">objectif flash</param>
        /// <param name="dateDebut">nouvelle date de début</param>
        /// <returns>objectif flash journalisé</returns>
        public ObjectifFlashEnt GetReportJournalisation(ObjectifFlashEnt objectifFlash, DateTime dateDebut)
        {
            var oldJournalisation = GetJournalisationsForDates(objectifFlash.DateDebut, objectifFlash.DateFin);
            this.JournalisationsInitialize(objectifFlash);
            var isJournalise = objectifFlash.Taches.Any(t => t.TacheJournalisations.Any());
            var dateFin = AddWorkingDays(dateDebut, GetWorkingDays(objectifFlash.DateDebut, objectifFlash.DateFin));
            objectifFlash.DateDebut = dateDebut;
            objectifFlash.DateFin = dateFin;
            // si pas de journalisation sur les taches, modification des dates de l'objectif flash uniquement.
            if (!isJournalise)
            {
                return objectifFlash;
            }


            // nouvelles journalisations calculées à partir des nouvelles dates
            objectifFlash.Journalisations = GetJournalisationsForDates(objectifFlash.DateDebut, objectifFlash.DateFin);

            foreach (var tache in objectifFlash.Taches)
            {
                var newTacheJournalisations = this.CreateTacheJournalisations(tache, objectifFlash.Journalisations, 0);
                var oldTacheJournalisationsWorkingDays = tache.TacheJournalisations.Where(x => oldJournalisation.Any(y => y.DateJournalisation == x.DateJournalisation && !y.IsWeekEndOrHoliday)).ToList();
                var newTacheJournalisationsWorkingDays = newTacheJournalisations.Where(x => objectifFlash.Journalisations.Any(y => y.DateJournalisation == x.DateJournalisation && !y.IsWeekEndOrHoliday)).ToList();

                for (int workingDayIndex = 0; workingDayIndex < newTacheJournalisationsWorkingDays.Count; workingDayIndex++)
                {
                    newTacheJournalisationsWorkingDays[workingDayIndex].QuantiteObjectif = oldTacheJournalisationsWorkingDays[workingDayIndex].QuantiteObjectif;
                }
                tache.TacheJournalisations = newTacheJournalisations;
                foreach (var ressource in tache.Ressources)
                {
                    var newRessourceJournalisations = this.CreateRessourceJournalisations(ressource, objectifFlash.Journalisations, 0);
                    var oldRessourceJournalisationsWorkingDays = ressource.TacheRessourceJournalisations.Where(x => oldJournalisation.Any(y => y.DateJournalisation == x.DateJournalisation && !y.IsWeekEndOrHoliday)).ToList();
                    var newRessourceJournalisationsWorkingDays = newRessourceJournalisations.Where(x => objectifFlash.Journalisations.Any(y => y.DateJournalisation == x.DateJournalisation && !y.IsWeekEndOrHoliday)).ToList();

                    for (int workingDayIndex = 0; workingDayIndex < newTacheJournalisationsWorkingDays.Count; workingDayIndex++)
                    {
                        newRessourceJournalisationsWorkingDays[workingDayIndex].QuantiteObjectif = oldRessourceJournalisationsWorkingDays[workingDayIndex].QuantiteObjectif;
                    }
                    ressource.TacheRessourceJournalisations = newRessourceJournalisations;
                }
            }

            objectifFlash.DateDebut = dateDebut;
            objectifFlash.DateFin = objectifFlash.Journalisations.Last().DateJournalisation;
            CalculateObjectifFlashProperties(objectifFlash);
            return objectifFlash;
        }

        private int GetWorkingDays(DateTime dateDebut, DateTime dateFin)
        {
            var workingDaysCount = 0;
            var totalDays = (int)(dateFin.Date - dateDebut.Date).TotalDays;
            for (int dayIndex = 0; dayIndex <= totalDays; dayIndex++)
            {
                var date = dateDebut.Date.AddDays(dayIndex);
                if (!IsWeekEndOrHoliday(date))
                {
                    workingDaysCount++;
                }
            }

            return workingDaysCount;
        }

        /// <summary>
        /// indique si la date est un jour ferié ou weekend
        /// </summary>
        /// <param name="date">date</param>
        /// <returns>true si ferié ou weekend</returns>
        private bool IsWeekEndOrHoliday(DateTime date)
        {
            return dateTimeExtendManager.IsWeekend(date) || dateTimeExtendManager.IsHoliday(date);
        }

        private DateTime AddWorkingDays(DateTime dateDebut, int workingDays)
        {
            if (workingDays <= 0)
            {
                return dateDebut;
            }
            int daysToAdd = 0;
            int workingDayIndex = 0;
            while (workingDayIndex < workingDays)
            {
                var date = dateDebut.Date.AddDays(daysToAdd);
                if (!IsWeekEndOrHoliday(date))
                {
                    workingDayIndex++;
                }
                daysToAdd++;
            }
            return dateDebut.AddDays(daysToAdd - 1);
        }


        /// <summary>
        /// Initialise la collection de journalisations de tache à partir des journalisations d'objectif flash
        /// </summary>
        /// <param name="ressource">ressources</param>
        /// <param name="journalisations">journalisations d'objectif flash</param>
        /// <param name="quantiteObjectif">quantité à appliquer sur les jours ouvrés</param>
        /// <returns>liste de journalisations de ressources</returns>
        private List<ObjectifFlashTacheRessourceJournalisationEnt> CreateRessourceJournalisations(ObjectifFlashTacheRessourceEnt ressource, List<ObjectifFlashJournalisation> journalisations, decimal quantiteObjectif)
        {
            return new List<ObjectifFlashTacheRessourceJournalisationEnt>(
                 journalisations.Select(journalisation =>
                    new ObjectifFlashTacheRessourceJournalisationEnt
                    {
                        ObjectifFlashTacheRessourceId = ressource.ObjectifFlashTacheRessourceId,
                        DateJournalisation = journalisation.DateJournalisation,
                        QuantiteObjectif = journalisation.IsWeekEndOrHoliday ? 0 : quantiteObjectif
                    }));
        }


        /// <summary>
        /// Initialise la collection de journalisations de tache à partir des journalisations d'objectif flash
        /// </summary>
        /// <param name="tache">tache</param>
        /// <param name="journalisations">journalisations d'objectif flash</param>
        /// <param name="quantiteObjectif">quantité à appliquer sur les jours ouvrés</param>
        /// <returns>liste de journalisations de tache</returns>
        private List<ObjectifFlashTacheJournalisationEnt> CreateTacheJournalisations(ObjectifFlashTacheEnt tache, List<ObjectifFlashJournalisation> journalisations, decimal quantiteObjectif)
        {
            return new List<ObjectifFlashTacheJournalisationEnt>(
                journalisations.Select(journalisation =>
                    new ObjectifFlashTacheJournalisationEnt
                    {
                        ObjectifFlashTacheId = tache.ObjectifFlashTacheId,
                        DateJournalisation = journalisation.DateJournalisation,
                        QuantiteObjectif = journalisation.IsWeekEndOrHoliday ? 0 : quantiteObjectif
                    }));

        }

        /// <summary>
        /// Génère une liste de journalisations pour une plage de dates
        /// </summary>
        /// <param name="dateDebut">date de début</param>
        /// <param name="dateFin">date de fin</param>
        /// <returns>Liste d'ObjectifFlashJournalisations</returns>
        private List<ObjectifFlashJournalisation> GetJournalisationsForDates(DateTime dateDebut, DateTime dateFin)
        {
            var totalDays = (decimal)(dateFin.Date - dateDebut.Date).TotalDays;
            var journalisations = new List<ObjectifFlashJournalisation>();

            for (int dayIndex = 0; dayIndex <= totalDays; dayIndex++)
            {
                var dateJournalisation = dateDebut.Date.AddDays(dayIndex);
                journalisations.Add(new ObjectifFlashJournalisation
                {
                    DateJournalisation = dateJournalisation,
                    IsWeekEndOrHoliday = IsWeekEndOrHoliday(dateJournalisation)
                });
            }
            return journalisations;
        }

        private void JournalisationQuantiteAjustement(ObjectifFlashEnt objectifFlash)
        {
            if (!objectifFlash.Taches.Any(x => x.TacheJournalisations.Any()))
            {
                return;
            }
            var journalisations = GetJournalisationsForDates(objectifFlash.DateDebut, objectifFlash.DateFin);
            foreach (var tache in objectifFlash.Taches ?? new List<ObjectifFlashTacheEnt>())
            {
                // ajustement sur le dernier jour
                var totalQuantiteJournaliseTache = tache.TacheJournalisations.Sum(x => x.QuantiteObjectif);
                var lastTacheJournalisation = tache.TacheJournalisations.Where(tj => journalisations.Any(j => j.DateJournalisation == tj.DateJournalisation && !j.IsWeekEndOrHoliday)).OrderBy(x => x.DateJournalisation).Last();
                var newQuantiteJournaliseTache = lastTacheJournalisation.QuantiteObjectif + (tache.QuantiteObjectif - totalQuantiteJournaliseTache);
                lastTacheJournalisation.QuantiteObjectif = newQuantiteJournaliseTache > 0 ? newQuantiteJournaliseTache : lastTacheJournalisation.QuantiteObjectif;

                // ajustement des autres ressources
                foreach (var ressource in tache.Ressources)
                {

                    // ajustement sur le dernier jour
                    var totalQuantiteJournaliseRessource = ressource.TacheRessourceJournalisations.Sum(x => x.QuantiteObjectif);
                    var lastRessourceJournalisation = ressource.TacheRessourceJournalisations.Where(rj => journalisations.Any(j => j.DateJournalisation == rj.DateJournalisation && !j.IsWeekEndOrHoliday)).OrderBy(x => x.DateJournalisation).Last();
                    var newQuantiteJournaliseRessource = lastRessourceJournalisation.QuantiteObjectif + (ressource.QuantiteObjectif - totalQuantiteJournaliseRessource);
                    lastRessourceJournalisation.QuantiteObjectif = newQuantiteJournaliseRessource > 0 ? newQuantiteJournaliseRessource : lastRessourceJournalisation.QuantiteObjectif;
                }
            }
        }

        /// <summary>
        /// Initialise les collections de jounalisations à partir des données de l'objectif flash dates et id de taches/ressources
        /// </summary>
        /// <param name="objectifFlash">objectif flash</param>
        private void JournalisationsInitialize(ObjectifFlashEnt objectifFlash)
        {
            var dateDebut = objectifFlash.DateDebut.Date;
            foreach (var tache in objectifFlash.Taches ?? new List<ObjectifFlashTacheEnt>())
            {
                var tacheDayIndex = 0;
                foreach (var tacheJournalisation in tache.TacheJournalisations ?? Enumerable.Empty<ObjectifFlashTacheJournalisationEnt>())
                {
                    tacheJournalisation.ObjectifFlashTacheId = tache.ObjectifFlashTacheId;
                    tacheJournalisation.DateJournalisation = dateDebut.AddDays(tacheDayIndex);
                    tacheDayIndex++;
                }

                foreach (var tacheRessource in tache.Ressources)
                {
                    var ressourceDayIndex = 0;

                    foreach (var ressourceJournalisation in tacheRessource.TacheRessourceJournalisations ?? Enumerable.Empty<ObjectifFlashTacheRessourceJournalisationEnt>())
                    {
                        ressourceJournalisation.ObjectifFlashTacheRessourceId = tacheRessource.ObjectifFlashTacheRessourceId;
                        ressourceJournalisation.DateJournalisation = dateDebut.AddDays(ressourceDayIndex);
                        ressourceDayIndex++;
                    }
                }
            }
        }
        #endregion Journalisation

        #region export

        /// <summary>
        /// Retourne la liste des dépenses de l'objectifFlash comprises entre la date de début et la date de fin sous forme de BilanFlashDepenseModel
        /// </summary>
        /// <param name="objectifFlash">objectifs flash</param>
        /// <param name="dateDebut">date de debut</param>
        /// <param name="dateFin">date de fin</param>
        /// <returns>Liste de dépenses</returns>
        public async Task<List<ObjectifFlashDepenseModel>> GetObjectifFlashDepensesAsync(ObjectifFlashEnt objectifFlash, DateTime? dateDebut, DateTime? dateFin)
        {
            List<int> tacheIdList = objectifFlash.Taches.Select(x => x.TacheId).Distinct().ToList();

            var filtre = new SearchDepense
            {
                CiId = objectifFlash.CiId.Value,
                PeriodeFin = dateFin,
                TakeReception = true,
                TakeValorisation = true
            };
            IEnumerable<DepenseExhibition> depenses = await depenseServiceMediator.GetAllDepenseExternetWithTacheAndRessourceAsync(filtre).ConfigureAwait(false);
            return depenses.Where(x => tacheIdList.Contains(x.TacheId) && (!dateDebut.HasValue || x.DateDepense.Date >= dateDebut.Value.Date) && (!dateFin.HasValue || x.DateDepense.Date <= dateFin.Value.Date)).Select(x => MapObjectifFlashDepenseModel(x)).ToList();
        }

        /// <summary>
        /// retourne la liste des dépenses liées à une liste d'objectifs flash
        /// </summary>
        /// <param name="objectifFlashList">Liste d'objectifs flash</param>
        /// <returns>Liste de dépenses</returns>
        public async Task<List<ObjectifFlashDepenseModel>> GetObjectifFlashListDepensesAsync(IEnumerable<ObjectifFlashEnt> objectifFlashList)
        {
            if (!objectifFlashList.Any())
            {
                return new List<ObjectifFlashDepenseModel>();
            }

            var ciIdList = objectifFlashList.Where(x => x.CiId.HasValue).Select(x => x.CiId.Value).Distinct().ToList();
            var tacheIdList = objectifFlashList.SelectMany(x => x.Taches ?? Enumerable.Empty<ObjectifFlashTacheEnt>()).Select(x => x.TacheId).Distinct().ToList();
            var maxDate = objectifFlashList.Max(x => x.DateFin);

            var objectifFLashDepenses = new List<ObjectifFlashDepenseModel>();
            foreach (var ciId in ciIdList)
            {
                var filtre = new SearchDepense
                {
                    CiId = ciId,
                    PeriodeFin = maxDate,
                    TakeReception = true,
                    TakeValorisation = true
                };
                IEnumerable<DepenseExhibition> depenses = await depenseServiceMediator.GetAllDepenseExternetWithTacheAndRessourceAsync(filtre).ConfigureAwait(false);
                objectifFLashDepenses.AddRange(depenses.Where(x => tacheIdList.Contains(x.TacheId))
                    .Select(x => MapObjectifFlashDepenseModel(x)));
            }
            return objectifFLashDepenses;
        }

        /// <summary>
        /// Mapping depenseExhibition vers ObjectifFlashDepenseModel
        /// </summary>
        /// <param name="depenseExhibition">depense Exhibition à mapper</param>
        /// <returns>ObjectifFlashDepenseModel</returns>
        private ObjectifFlashDepenseModel MapObjectifFlashDepenseModel(DepenseExhibition depenseExhibition)
        {
            return new ObjectifFlashDepenseModel
            {
                CiId = depenseExhibition.Ci.CiId,
                RessourceId = depenseExhibition.RessourceId,
                Ressource = depenseExhibition.Ressource,
                Tache = depenseExhibition.Tache,
                TacheId = depenseExhibition.TacheId,
                MontantHT = depenseExhibition.MontantHT,
                Quantite = depenseExhibition.Quantite,
                Date = depenseExhibition.DateDepense,
                Unite = depenseExhibition.Unite.Code
            };
        }

        #endregion export

    }
}
