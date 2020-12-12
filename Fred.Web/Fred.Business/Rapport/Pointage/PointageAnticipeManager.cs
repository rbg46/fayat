using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Referential.CodeAbsence;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Framework;

namespace Fred.Business.Rapport.Pointage
{
    /// <summary>
    ///   Business Layer pour les pointages
    /// </summary>
    public class PointageAnticipeManager : Manager<PointageAnticipeEnt, IPointageAnticipeRepository>, IPointageAnticipeManager
    {
        private const string ConstCodeAbsCongePaye = "CP";
        private const string ConstCodeAbsJourFerie = "JF";
        private readonly ICodeAbsenceManager codeAbsenceMgr;
        private readonly ILogManager logMgr;
        private readonly int nbrMaxPrimes = 4;
        private readonly IPointageAnticipePrimeRepository pointageAnticipePrimeRepo;
        private readonly IUtilisateurManager userMgr;

        public PointageAnticipeManager(
            IUnitOfWork uow,
            IPointageAnticipeRepository pointageAnticipeRepository,
            ILogManager logMgr,
            IUtilisateurManager userMgr,
            ICodeAbsenceManager codeAbsenceMgr,
            IPointageAnticipePrimeRepository pointageAnticipePrimeRepo)
          : base(uow, pointageAnticipeRepository)
        {
            this.logMgr = logMgr;
            this.userMgr = userMgr;
            this.codeAbsenceMgr = codeAbsenceMgr;
            this.pointageAnticipePrimeRepo = pointageAnticipePrimeRepo;
        }

        /// <summary>
        ///   Créer une ligne de rapport vide
        /// </summary>
        /// <param name="pointage">La ligne du rapport</param>
        /// <param name="prime">La prime</param>
        /// <returns>Une ligne de rapport</returns>
        public PointageAnticipePrimeEnt GetNewPointageAnticipePrime(PointageAnticipeEnt pointage, PrimeEnt prime)
        {
            return new PointageAnticipePrimeEnt
            {
                PrimeId = prime.PrimeId,
                Prime = prime,
                PointageAnticipe = pointage
            };
        }

        /// <summary>
        ///   Récupère la liste des pointages correspondant aux critères de recherche
        /// </summary>
        /// <param name="searchPointage">Critère de recherche</param>
        /// <param name="applyReadOnly">Variable permettant d'appliquer les règles de gestion au pointage</param>
        /// <returns>IEnumerable contenant la liste des pointages correspondants aux critères</returns>
        public IEnumerable<PointageAnticipeEnt> SearchPointageAnticipeWithFilter(SearchRapportLigneEnt searchPointage, bool applyReadOnly)
        {
            //Bouchon dans l'attente de gestion d'une date max dans la récupération des pointages
            searchPointage.DatePointageMax = searchPointage.DatePointageMin.AddMonths(1);
            var pointages = this.Repository.SearchPointageAnticipeWithFilter(searchPointage.GetPredicateWherePointageAnticipe(), "DatePointage ascending");
            if (pointages != null)
            {
                foreach (var pointage in pointages)
                {
                    // Permet de récuperer PrenomNomTemporaire à partir de Personnel.PrenomNom
                    if (pointage.Personnel != null && pointage.PersonnelId != null)
                    {
                        pointage.PrenomNomTemporaire = pointage.Personnel.PrenomNom;
                    }
                }

                return pointages;
            }

            return new PointageAnticipeEnt[] { };
        }

        /// <summary>
        ///   Crée une nouvelle ligne de rapport vide (version light)
        /// </summary>
        /// <returns>Entité RapportLigneEnt correspondant à une nouvelle ligne vide</returns>
        public PointageAnticipeEnt GetNewPointageAnticipeLight()
        {
            var pointageAnticipe = new PointageAnticipeEnt
            {
                PointageId = 0,
                NbMaxPrimes = this.nbrMaxPrimes,
                ListPrimes = new List<PointageAnticipePrimeEnt>()
            };

            return pointageAnticipe;
        }

        /// <summary>
        ///   Créer une ligne de rapport vide
        /// </summary>
        /// <param name="pointage">La ligne du rapport</param>
        /// <param name="prime">La prime</param>
        /// <returns>Une ligne de rapport</returns>
        public PointagePrimeBase GetNewPointagePrime(PointageBase pointage, PrimeEnt prime)
        {
            if (pointage is PointageAnticipeEnt)
            {
                return new PointageAnticipePrimeEnt
                {
                    PrimeId = prime.PrimeId,
                    Prime = prime,
                    PointageAnticipe = pointage as PointageAnticipeEnt
                };
            }

            return null;
        }

        #region Traitement

        private PointageAnticipeEnt TraitmentPointageAnticipeForPersonnel(PointageAnticipeEnt pointageAnticipe)
        {
            // Si on a un personnel
            if (pointageAnticipe.Personnel != null && pointageAnticipe.PersonnelId != null)
            {
                if (pointageAnticipe.Personnel.LibelleRef == pointageAnticipe.PrenomNomTemporaire)
                {
                    pointageAnticipe.PrenomNomTemporaire = string.Empty;
                }
                else if (!string.IsNullOrEmpty(pointageAnticipe.PrenomNomTemporaire))
                {
                    pointageAnticipe.Personnel = null;
                    pointageAnticipe.PersonnelId = null;
                }
            }

            return pointageAnticipe;
        }
        #endregion

        #region Add

        /// <summary>
        ///   Ajoute une prime à un pointage Anticipe ou réel
        /// </summary>
        /// <param name="pointage">La ligne de rapport</param>
        /// <param name="prime">La prime</param>
        /// <returns>La ligne de rapport contenant la nouvelle prime</returns>
        public PointageBase AddPrimeToPointage(PointageBase pointage, PrimeEnt prime)
        {
            if (pointage == null || prime == null)
            {
                var exception = new ArgumentException("Le pointage et la prime ne peuvent être nul");
                this.logMgr.TraceException(exception.Message, exception);
                throw exception;
            }

            ((PointageAnticipeEnt)pointage).ListPrimes.Add(GetNewPointageAnticipePrime((PointageAnticipeEnt)pointage, prime));
            return pointage;
        }

        /// <summary>
        ///   Ajoute un pointage
        /// </summary>
        /// <param name="pointage">Objet de base représentant un pointage réel ou anticipé</param>
        public void AddPointage(PointageBase pointage)
        {
            int userId = this.userMgr.GetContextUtilisateurId();
            pointage.AuteurCreationId = userId;
            pointage.DateCreation = DateTime.UtcNow;

            this.Repository.Insert(pointage as PointageAnticipeEnt);
        }

        /// <summary>
        ///   Ajoute une ligne de prime à un pointage réel ou anticipe
        /// </summary>
        /// <param name="pointagePrime">Ligne de prime ajouter au pointage</param>
        public void AddPointagePrime(PointagePrimeBase pointagePrime)
        {
            if (pointagePrime is PointageAnticipePrimeEnt)
            {
                this.pointageAnticipePrimeRepo.Insert(pointagePrime as PointageAnticipePrimeEnt);
            }
        }
        #endregion

        #region Update

        /// <summary>
        ///   Met à jour une ligne de rapport
        /// </summary>
        /// <param name="pointage">pointage anticipé ou réel à mettre à jour</param>
        public void UpdatePointage(PointageBase pointage)
        {
            int userId = this.userMgr.GetContextUtilisateurId();
            pointage.AuteurModificationId = userId;
            pointage.DateModification = DateTime.UtcNow;

            if (pointage is PointageAnticipeEnt)
            {
                this.Repository.Update(pointage as PointageAnticipeEnt);
            }
            this.Save();
        }

        /// <summary>
        ///   Met à jour une ligne de prime
        /// </summary>
        /// <param name="pointagePrime">Ligne de prime à mettre à jour dans le pointage réel ou anticipé</param>
        public void UpdatePointagePrime(PointagePrimeBase pointagePrime)
        {
            if (pointagePrime is PointageAnticipePrimeEnt)
            {
                this.pointageAnticipePrimeRepo.Update(pointagePrime as PointageAnticipePrimeEnt);
            }
        }
        #endregion

        #region Delete

        /// <summary>
        ///   Supprime une ligne de rapport
        /// </summary>
        /// <param name="pointage">La ligne de rapport à supprimer</param>
        public void DeletePointage(PointageBase pointage)
        {
            if (pointage == null)
            {
                var exception = new ArgumentException("Le pointage à supprimer ne peut être null");
                this.logMgr.TraceException(exception.Message, exception);
                throw exception;
            }
            this.Repository.Update((PointageAnticipeEnt)pointage);
        }

        /// <summary>
        ///   Supprime une ligne de tache de prime
        /// </summary>
        /// <param name="pointagePrime">Ligne de prime à supprimer dans le pointage réel ou anticipé</param>
        public void DeletePointagePrime(PointagePrimeBase pointagePrime)
        {
            if (pointagePrime is PointageAnticipePrimeEnt)
            {
                this.pointageAnticipePrimeRepo.Delete(pointagePrime as PointageAnticipePrimeEnt);
            }
        }
        #endregion

        #region Duplicate

        /// <summary>
        ///   Duplique une liste de lignes de rapport
        /// </summary>
        /// <param name="listPointage">La liste de lignes de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une liste de ligne de rapport</returns>
        public IEnumerable<PointageBase> DuplicateListPointage(IEnumerable<PointageBase> listPointage, bool emptyValues = false)
        {
            var newListPointage = new List<PointageBase>();

            foreach (var pointage in listPointage)
            {
                if (!pointage.IsDeleted && pointage is PointageAnticipeEnt)
                {
                    newListPointage.Add(DuplicatePointageAnticipe(pointage as PointageAnticipeEnt, emptyValues));
                }
            }

            return newListPointage;
        }

        /// <summary>
        ///   Duplique une liste de lignes de rapport
        /// </summary>
        /// <param name="listPointage">La liste de lignes de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une liste de ligne de rapport</returns>
        public IEnumerable<PointageAnticipeEnt> DuplicateListPointageAnticipe(IEnumerable<PointageAnticipeEnt> listPointage, bool emptyValues = false)
        {
            var newListPointage = new List<PointageAnticipeEnt>();

            foreach (var pointage in listPointage)
            {
                if (!pointage.IsDeleted)
                {
                    newListPointage.Add(DuplicatePointageAnticipe(pointage, emptyValues));
                }
            }

            return newListPointage;
        }

        /// <summary>
        ///   Duplique une de lignes de rapport
        /// </summary>
        /// <param name="pointage">Une ligne de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de rapport</returns>
        public PointageBase DuplicatePointage(PointageBase pointage, bool emptyValues = false)
        {
            if (pointage is PointageAnticipeEnt)
            {
                return DuplicatePointageAnticipe(pointage as PointageAnticipeEnt, emptyValues);
            }

            return null;
        }

        /// <summary>
        ///   Duplique une ligne de rapport
        /// </summary>
        /// <param name="pointageAnticipe">La ligne de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de rapport</returns>
        public PointageAnticipeEnt DuplicatePointageAnticipe(PointageAnticipeEnt pointageAnticipe, bool emptyValues = false)
        {
            if (emptyValues)
            {
                return new PointageAnticipeEnt
                {
                    PointageId = 0,
                    PrenomNomTemporaire = pointageAnticipe.PrenomNomTemporaire,
                    PersonnelId = pointageAnticipe.PersonnelId,
                    Personnel = pointageAnticipe.Personnel,
                    HeureNormale = 0,
                    CodeMajorationId = null,
                    CodeMajoration = null,
                    CodeAbsenceId = null,
                    CodeAbsence = null,
                    HeureAbsence = 0,
                    NumSemaineIntemperieAbsence = null,
                    CodeDeplacementId = null,
                    CodeDeplacement = null,
                    CodeZoneDeplacement = null,
                    DeplacementIV = false,
                    ListPrimes = DuplicateListPointagePrimeAnticipe(pointageAnticipe.ListPrimes, emptyValues).ToList()
                };
            }

            return new PointageAnticipeEnt
            {
                PointageId = 0,
                PrenomNomTemporaire = pointageAnticipe.PrenomNomTemporaire,
                PersonnelId = pointageAnticipe.PersonnelId,
                Personnel = pointageAnticipe.Personnel,
                HeureNormale = 0,
                CodeMajorationId = pointageAnticipe.CodeMajorationId,
                CodeMajoration = pointageAnticipe.CodeMajoration,
                CodeAbsenceId = pointageAnticipe.CodeAbsenceId,
                CodeAbsence = pointageAnticipe.CodeAbsence,
                HeureAbsence = 0,
                NumSemaineIntemperieAbsence = null,
                CodeDeplacementId = pointageAnticipe.CodeDeplacementId,
                CodeDeplacement = pointageAnticipe.CodeDeplacement,
                CodeZoneDeplacement = pointageAnticipe.CodeZoneDeplacement,
                DeplacementIV = pointageAnticipe.DeplacementIV,
                ListPrimes = DuplicateListPointagePrimeAnticipe(pointageAnticipe.ListPrimes, emptyValues).ToList(),
                IsDeleted = pointageAnticipe.IsDeleted
            };
        }

        /// <summary>
        ///   Duplique une liste de ligne de prime de rapport
        /// </summary>
        /// <param name="listPointagePrime">La liste de ligne de prime de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une liste de ligne de prime de rapport</returns>
        public IEnumerable<PointageAnticipePrimeEnt> DuplicateListPointagePrimeAnticipe(IEnumerable<PointageAnticipePrimeEnt> listPointagePrime, bool emptyValues = false)
        {
            var newListPointagePrime = new List<PointageAnticipePrimeEnt>();

            foreach (var pointagePrime in listPointagePrime)
            {
                if (!pointagePrime.IsDeleted)
                {
                    newListPointagePrime.Add(DuplicatePointagePrimeAnticipe(pointagePrime, emptyValues));
                }
            }

            return newListPointagePrime;
        }

        /// <summary>
        ///   Duplique une ligne de prime de rapport
        /// </summary>
        /// <param name="pointagePrime">La ligne de prime de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de prime de rapport</returns>
        public PointagePrimeBase DuplicatePointagePrime(PointagePrimeBase pointagePrime, bool emptyValues = false)
        {
            if (emptyValues)
            {
                return new PointageAnticipePrimeEnt
                {
                    PointagePrimeId = 0,
                    PointageId = 0,
                    PrimeId = pointagePrime.PrimeId,
                    Prime = pointagePrime.Prime,
                    HeurePrime = 0
                };
            }

            return new PointageAnticipePrimeEnt
            {
                PointagePrimeId = 0,
                PointageId = 0,
                PrimeId = pointagePrime.PrimeId,
                Prime = pointagePrime.Prime,
                HeurePrime = pointagePrime.HeurePrime,
                IsChecked = pointagePrime.IsChecked,
                IsDeleted = pointagePrime.IsDeleted
            };
        }

        /// <summary>
        ///   Duplique une ligne de prime de rapport
        /// </summary>
        /// <param name="pointagePrime">La ligne de prime de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de prime de rapport</returns>
        public PointageAnticipePrimeEnt DuplicatePointagePrimeAnticipe(PointageAnticipePrimeEnt pointagePrime, bool emptyValues = false)
        {
            if (emptyValues)
            {
                return new PointageAnticipePrimeEnt
                {
                    PointagePrimeId = 0,
                    PointageId = 0,
                    PrimeId = pointagePrime.PrimeId,
                    Prime = pointagePrime.Prime,
                    HeurePrime = 0
                };
            }

            return new PointageAnticipePrimeEnt
            {
                PointagePrimeId = 0,
                PointageId = 0,
                PrimeId = pointagePrime.PrimeId,
                Prime = pointagePrime.Prime,
                HeurePrime = pointagePrime.HeurePrime,
                IsChecked = pointagePrime.IsChecked,
                IsDeleted = pointagePrime.IsDeleted
            };
        }
        #endregion

        #region SearchPointageWithFilter

        /// <summary>
        ///   Retourne une liste de pointages anticipés en fonction du personnel et d'une date
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="date">La date du pointage</param>
        /// <returns>Une liste de pointages anticipés</returns>
        public IEnumerable<PointageAnticipeEnt> GetListPointagesAnticipesByPersonnelIdAndDatePointage(int personnelId, DateTime date)
        {
            return this.Repository.GetListPointagesAnticipesByPersonnelIdAndDatePointage(personnelId, date);
        }
        #endregion
    }
}
