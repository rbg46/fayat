using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Referential.CodeAbsence;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Framework.DateTimeExtend;

namespace Fred.Business.Rapport.Pointage
{
    public class PointageSamediCongePayeService : IPointageSamediCongePayeService
    {
        private const string ConstCodeAbsCongePaye = "23";

        private readonly ICodeAbsenceManager codeAbsenceManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IPointageRepository pointageRepository;
        private readonly IPersonnelRepository personnelRepository;
        private readonly IDateTimeExtendManager dateTimeExtendManager;

        public PointageSamediCongePayeService(
            ICodeAbsenceManager codeAbsenceManager,
            IUtilisateurManager utilisateurManager,
            IPointageRepository pointageRepository,
            IPersonnelRepository personnelRepository,
            IDateTimeExtendManager dateTimeExtendManager)
        {
            this.codeAbsenceManager = codeAbsenceManager;
            this.utilisateurManager = utilisateurManager;
            this.pointageRepository = pointageRepository;
            this.personnelRepository = personnelRepository;
            this.dateTimeExtendManager = dateTimeExtendManager;
        }

        /// <summary>
        ///   Génération des samedi en CP
        /// </summary>
        /// <param name="pointage">La base du pointage</param>
        public void GenerationPointageSamediCP(RapportLigneEnt pointage)
        {
            var datePointage = pointage.DatePointage;
            //// Si la journée est un jeudi, le vendredi doit etre ferié et le samedi non férié (RG_1385_007)
            if (datePointage.DayOfWeek == DayOfWeek.Thursday && IsBankHoliday(datePointage.AddDays(1)) && !IsBankHoliday(datePointage.AddDays(2)))
            {
                HandlePointageSamediCP(pointage, datePointage.AddDays(2));
            }

            //// Si la journée est un vendredi le samedi doit etre non férié (RG_1385_007)
            if (datePointage.DayOfWeek == DayOfWeek.Friday && !IsBankHoliday(datePointage.AddDays(1)))
            {
                HandlePointageSamediCP(pointage, datePointage.AddDays(1));
            }
        }

        /// <summary>
        /// Indique si le pointage est un samedi en congés payé.
        /// </summary>
        /// <param name="pointage">Le pointage concerné.</param>
        /// <returns>True si le pointage est un samedi en congés payé, sinon false.</returns>
        public bool IsSamediCP(RapportLigneEnt pointage)
        {
            return pointage.DatePointage.DayOfWeek == DayOfWeek.Saturday && !IsBankHoliday(pointage.DatePointage);
        }

        private void HandlePointageSamediCP(RapportLigneEnt pointage, DateTime dateSamedi)
        {
            // Cas Ajout de pointage
            if (pointage.IsCreated)
            {
                if (pointage.PersonnelId.HasValue)
                {
                    if (pointage.Personnel == null)
                    {
                        pointage.Personnel = personnelRepository.FindById(pointage.PersonnelId);
                    }
                    AddSamediCP(pointage, dateSamedi);
                }
            }
            // Cas Mise à jour
            else if (pointage.IsUpdated)
            {
                HandleUpdateSamediCP(pointage, dateSamedi);
            }
            // Cas Suppression
            else if (pointage.IsDeleted && pointage.PersonnelId.HasValue)
            {
                DeleteSamediCP(pointage, dateSamedi);
            }
        }

        private void AddSamediCP(RapportLigneEnt pointage, DateTime dateSamedi)
        {
            if (pointage.CodeAbsenceId.HasValue && CodeAbsenceConcernedBySamediCP(this.codeAbsenceManager.GetCodeAbsenceById(pointage.CodeAbsenceId.Value)) && PersonnelConcernedBySamediCP(pointage.Personnel))
            {
                var codeAbsCP = this.codeAbsenceManager.GetCodeAbsenceByCode(ConstCodeAbsCongePaye);
                var newSamedi = PointageReelCreatorHelper.GetNewPointageReelLight();
                newSamedi.DateCreation = DateTime.UtcNow;
                newSamedi.AuteurCreationId = this.utilisateurManager.GetContextUtilisateurId();
                newSamedi.CiId = pointage.CiId;
                newSamedi.DatePointage = dateSamedi;
                newSamedi.PersonnelId = pointage.PersonnelId;
                newSamedi.IsGenerated = true;
                newSamedi.CodeAbsenceId = codeAbsCP.CodeAbsenceId;
                newSamedi.HeureAbsence = pointage.HeureAbsence;

                var rapport = new RapportEnt
                {
                    RapportStatutId = RapportStatutEnt.RapportStatutEnCours.Key,
                    AuteurCreationId = this.utilisateurManager.GetContextUtilisateurId(),
                    CiId = pointage.CiId,
                    DateChantier = dateSamedi,
                    DateCreation = DateTime.UtcNow,
                    HoraireDebutM = DateTime.UtcNow.Date.AddHours(8),
                    HoraireFinM = DateTime.UtcNow.Date.AddHours(12),
                    HoraireDebutS = DateTime.UtcNow.Date.AddHours(14),
                    HoraireFinS = DateTime.UtcNow.Date.AddHours(18),
                    ListLignes = new List<RapportLigneEnt>(),
                    IsGenerated = true
                };

                newSamedi.Rapport = rapport;
                //// Ici, EF devrait rajouter aussi l'entete de rapport via l'ajout du pointage référençant le rapport
                pointageRepository.Insert(newSamedi);
            }
        }

        private void HandleUpdateSamediCP(RapportLigneEnt pointage, DateTime dateSamedi)
        {
            var recordedPointage = this.pointageRepository.GetById(pointage.RapportLigneId);
            if (recordedPointage != null)
            {
                // Si on a supprimer ou modifier le personnel ou le code absence de la ligne et que le pointage est concerné par le samedi CP
                if (IsPersonnelOrCodeAbsenceDeleted(pointage, recordedPointage))
                {
                    if (!pointage.PersonnelId.HasValue)
                    {
                        DeleteSamediCP(pointage, dateSamedi);
                    }
                    else if (recordedPointage.PersonnelId.HasValue)
                    {
                        DeleteSamediCP(recordedPointage, dateSamedi);
                    }
                }
                // Si on a ajouter ou modifier le personnel ou le code absence de la ligne et que le pointage est concerné par le samedi CP
                if (IsPersonnelOrCodeAbsenceAdded(pointage, recordedPointage))
                {
                    AddSamediCP(pointage, dateSamedi);
                }
                // Si le pointage est mis à jour sans modification de personnel ni de code absence
                if (pointage.PersonnelId.HasValue && recordedPointage.PersonnelId.HasValue && pointage.PersonnelId.Value == recordedPointage.PersonnelId.Value
                    && pointage.CodeAbsenceId.HasValue && recordedPointage.CodeAbsenceId.HasValue && pointage.CodeAbsenceId.Value == recordedPointage.CodeAbsenceId.Value
                    && CodeAbsenceConcernedBySamediCP(pointage.CodeAbsence))
                {
                    UpdateSamediCP(pointage, dateSamedi);
                }
            }
        }

        private bool IsPersonnelOrCodeAbsenceDeleted(RapportLigneEnt pointage, RapportLigneEnt recordedPointage)
        {
            return (!pointage.PersonnelId.HasValue
                  || (pointage.PersonnelId.HasValue && recordedPointage.PersonnelId.HasValue && pointage.PersonnelId.Value != recordedPointage.PersonnelId.Value)
                  || !pointage.CodeAbsenceId.HasValue
                  || (pointage.CodeAbsenceId.HasValue && recordedPointage.CodeAbsenceId.HasValue && pointage.CodeAbsenceId.Value != recordedPointage.CodeAbsenceId.Value))
                  && recordedPointage.CodeAbsenceId.HasValue && CodeAbsenceConcernedBySamediCP(this.codeAbsenceManager.GetCodeAbsenceById(recordedPointage.CodeAbsenceId.Value));
        }

        private bool IsPersonnelOrCodeAbsenceAdded(RapportLigneEnt pointage, RapportLigneEnt recordedPointage)
        {
            return ((pointage.PersonnelId.HasValue && recordedPointage.PersonnelId.HasValue && pointage.PersonnelId.Value != recordedPointage.PersonnelId.Value)
                  || (pointage.CodeAbsenceId.HasValue && recordedPointage.CodeAbsenceId.HasValue && pointage.CodeAbsenceId.Value != recordedPointage.CodeAbsenceId.Value)
                  || (pointage.PersonnelId.HasValue && !recordedPointage.PersonnelId.HasValue)
                  || (pointage.CodeAbsenceId.HasValue && !recordedPointage.CodeAbsenceId.HasValue))
                  && CodeAbsenceConcernedBySamediCP(pointage.CodeAbsence);
        }

        /// <summary>
        /// Mise à jour du samedi CP
        /// recherche du pointage de samedi correspondant au personnelID et au ciid
        /// </summary>
        /// <param name="pointage">pointage d'origine</param>
        /// <param name="dateSamedi">date du samedi</param>
        private void UpdateSamediCP(RapportLigneEnt pointage, DateTime dateSamedi)
        {
            RapportLigneEnt ptgExistant = GetPointageByPersonnelIdAndDatePointage(pointage.PersonnelId.Value, pointage.CiId, dateSamedi);
            if (ptgExistant != null)
            {
                // passage de la Navigation property CodeAbsence à null car l'information est portée par le CodeAbsenceId
                // Cela évite l'erreur "another entity of the same type already has the same primary key value." lors d'un attach si plusieurs samedis updatés comportent le meme codeabsence            
                ptgExistant.CodeAbsenceId = pointage.CodeAbsenceId;
                ptgExistant.AuteurModificationId = this.utilisateurManager.GetContextUtilisateurId();
                ptgExistant.DateModification = DateTime.UtcNow;
                ptgExistant.HeureAbsence = pointage.HeureAbsence;
                pointageRepository.Update(ptgExistant);
            }
            else
            {
                AddSamediCP(pointage, dateSamedi);
            }
        }

        /// <summary>
        /// Suppression Samedi CP 
        /// Recherche le pointage du samedi correspondant au personnelid et au ciid du pointage d'origin
        /// </summary>
        /// <param name="pointage">pointage d'origine</param>
        /// <param name="dateSamedi">date du samedi</param>
        private void DeleteSamediCP(RapportLigneEnt pointage, DateTime dateSamedi)
        {
            // On supprime les possibles lignes de pointage CP ayant l'HeureAbsence == 0
            var lstPointages = GetListPointagesReelsByPersonnelIdAndDatePointage(pointage.PersonnelId.Value, pointage.CiId, dateSamedi);
            var ptg = lstPointages.FirstOrDefault(o => o.DatePointage.Day == dateSamedi.Day
                                                  && o.DatePointage.Month == dateSamedi.Month
                                                  && o.DatePointage.Year == dateSamedi.Year
                                                  && o.CiId == pointage.CiId
                                                  && (o.CodeAbsence == null || o.CodeAbsence.Code == ConstCodeAbsCongePaye)
                                                  && o.IsGenerated);
            if (ptg != null)
            {
                ptg.DateSuppression = DateTime.UtcNow;
                ptg.AuteurSuppressionId = this.utilisateurManager.GetContextUtilisateurId();
                pointageRepository.Update(ptg);
            }
        }

        /// <summary>
        /// Code absence lié à un samedi CP
        /// </summary>
        /// <param name="codeAbsence">code d'absence</param>
        /// <returns>true si le pointage est lié à un samedi CP</returns>
        private bool CodeAbsenceConcernedBySamediCP(CodeAbsenceEnt codeAbsence)
        {
            // Gestion des samedi CP uniquement pour le code absence 23
            return codeAbsence != null && codeAbsence.Code == ConstCodeAbsCongePaye;
        }

        /// <summary>
        /// Personnel avec génération des samedi CP, parametre de société
        /// </summary>
        /// <param name="personnel">personnel</param>
        /// <returns>true si la generation de samedi CP est active pour ce personnel</returns>
        private bool PersonnelConcernedBySamediCP(PersonnelEnt personnel)
        {
            //// RG_1385_005 : On doit pouvoir activer/désactiver la génération par société de PAYE
            //// RG_1385_002 : On ne génère que des samedi en CP QUE pour le personnel interne du GROUPE RZB et non intérimaire
            //// RG_1385_004 : On ne peut générer que 5 samedi maximum par personne dans la période de paye
            return personnel.IsInterne && !personnel.IsInterimaire && personnel.Societe.IsGenerationSamediCPActive;
        }

        /// <summary>
        ///   Retourne vrai si un jour est ferié
        /// </summary>
        /// <param name="date">La date</param>
        /// <returns>Vrai si c'est un jour férié sinon faux</returns>
        public bool IsBankHoliday(DateTime date)
        {
            return dateTimeExtendManager.IsHoliday(date);
        }

        /// <summary>
        ///   Retourne une liste de pointages réels en fonction du personnel et d'une date
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="date">La date du pointage</param>
        /// <returns>Une liste de pointages réels</returns>
        public IEnumerable<RapportLigneEnt> GetListPointagesReelsByPersonnelIdAndDatePointage(int personnelId, int ciId, DateTime date)
        {
            return pointageRepository.GetListPointagesReelsByPersonnelIdAndDatePointage(personnelId, ciId, date);
        }

        /// <summary>
        ///   Retourne une liste de pointages réels en fonction du personnel et d'une date
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="date">La date du pointage</param>
        /// <returns>Une liste de pointages réels</returns>
        public RapportLigneEnt GetPointageByPersonnelIdAndDatePointage(int personnelId, int ciId, DateTime date)
        {
            return pointageRepository.GetPointageByPersonnelIdAndDatePointage(personnelId, ciId, date);
        }
    }
}
