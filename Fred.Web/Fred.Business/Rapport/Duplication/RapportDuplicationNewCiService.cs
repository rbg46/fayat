using System.Collections.Generic;
using System.Linq;
using Fred.Business.IndemniteDeplacement;
using Fred.Business.Personnel;
using Fred.Business.Referential;
using Fred.Business.Referential.Tache;
using Fred.Entities.CI;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;

namespace Fred.Business.Rapport.Duplication
{
    public class RapportDuplicationNewCiService : IRapportDuplicationNewCiService
    {
        private readonly IPersonnelManager personnelManager;
        private readonly ITacheManager tacheManager;
        private readonly IPrimeManager primeManager;
        private readonly IIndemniteDeplacementManager indemniteDeplacementManager;

        public RapportDuplicationNewCiService(
            IPersonnelManager personnelManager,
            ITacheManager tacheManager,
            IPrimeManager primeManager,
            IIndemniteDeplacementManager indemniteDeplacementManager)
        {
            this.personnelManager = personnelManager;
            this.tacheManager = tacheManager;
            this.primeManager = primeManager;
            this.indemniteDeplacementManager = indemniteDeplacementManager;
        }

        /// <summary>
        ///   Duplique un rapport
        /// </summary>
        /// <param name="rapport">Le rapport à dupliquer</param>
        /// <returns>Un rapport</returns>
        public RapportEnt DuplicateRapport(RapportEnt rapport)
        {
            return new RapportEnt
            {
                RapportId = 0,
                DateChantier = rapport.DateChantier,
                RapportStatut = rapport.RapportStatut,
                RapportStatutId = rapport.RapportStatutId,
                IsStatutEnCours = rapport.IsStatutEnCours,
                Meteo = null,
                Evenements = null,
                AuteurCreationId = null,
                AuteurCreation = null,
                AuteurModificationId = null,
                AuteurModification = null,
                AuteurSuppressionId = null,
                AuteurSuppression = null,
                HoraireDebutM = rapport.CI.HoraireDebutM,
                HoraireDebutS = rapport.CI.HoraireDebutS,
                HoraireFinM = rapport.CI.HoraireFinM,
                HoraireFinS = rapport.CI.HoraireFinS,
                DateCreation = null,
                DateModification = null,
                DateSuppression = null,
                CiId = rapport.CiId,
                CI = rapport.CI,
                ListLignes = DuplicateListPointages(rapport.ListLignes.ToList(), rapport.CI),
                ListCommentaires = null,
                NbMaxPrimes = rapport.NbMaxPrimes,
                Cloture = false,
                NbMaxTaches = rapport.NbMaxTaches
            };
        }

        private List<RapportLigneEnt> DuplicateListPointages(List<RapportLigneEnt> pointages, CIEnt ci)
        {
            var newListPointage = new List<RapportLigneEnt>();

            foreach (var pointage in pointages)
            {
                if (!pointage.IsDeleted)
                {
                    RapportLigneEnt duplicatedPointage = DuplicatePointage(pointage, ci);
                    if (duplicatedPointage != null)
                    {
                        newListPointage.Add(duplicatedPointage);
                    }
                }
            }
            return newListPointage;
        }


        private RapportLigneEnt DuplicatePointage(RapportLigneEnt pointage, CIEnt ci)
        {
            RapportLigneEnt duplicatedPointage = new RapportLigneEnt()
            {
                Rapport = null,
                CiId = ci.CiId,
                Ci = ci,
                DatePointage = pointage.DatePointage,
                Personnel = pointage.Personnel,
                PersonnelId = pointage.PersonnelId,
                PrenomNomTemporaire = pointage.PrenomNomTemporaire,
                HeureNormale = 0,
                HeureMajoration = 0,
                CodeMajorationId = null,
                CodeMajoration = null,
                CodeAbsenceId = null,
                CodeAbsence = null,
                CodeDeplacementId = null,
                CodeDeplacement = null,
                CodeZoneDeplacement = null,
                CodeZoneDeplacementId = null,
                DeplacementIV = false,
                HeureAbsence = 0,
                NumSemaineIntemperieAbsence = null,
                Materiel = pointage.Materiel != null && pointage.Materiel.MaterielLocation ? null : pointage.Materiel,
                MaterielId = pointage.Materiel != null && pointage.Materiel.MaterielLocation ? null : pointage.MaterielId,
                MaterielNomTemporaire = pointage.Materiel != null && pointage.Materiel.MaterielLocation ? null : pointage.MaterielNomTemporaire,
                MaterielMarche = 0,
                MaterielArret = 0,
                MaterielPanne = 0,
                MaterielIntemperie = 0,
                ListRapportLigneTaches = new List<RapportLigneTacheEnt>(),
                IsDeleted = false
            };

            if (!duplicatedPointage.PersonnelId.HasValue && !duplicatedPointage.MaterielId.HasValue)
            {
                return null;
            }

            if (duplicatedPointage.PersonnelId.HasValue)
            {
                GetMaterielDefaultForPersonnel(ref duplicatedPointage);

                GetOrCreateIndemniteDeplacement(ref duplicatedPointage);
            }
            GetTacheParDefaut(ref duplicatedPointage);
            duplicatedPointage.ListRapportLignePrimes = DuplicateListPrimes(pointage.ListRapportLignePrimes.ToList(), ci.CiId);

            return duplicatedPointage;
        }

        private List<RapportLignePrimeEnt> DuplicateListPrimes(List<RapportLignePrimeEnt> pointagePrimes, int ciId)
        {
            var newListPrimes = new List<RapportLignePrimeEnt>();

            foreach (var pointagePrime in pointagePrimes)
            {
                if (!pointagePrime.IsDeleted)
                {
                    RapportLignePrimeEnt duplicatedPointagePrime = DuplicatePrime(pointagePrime, ciId);
                    if (duplicatedPointagePrime != null)
                    {
                        newListPrimes.Add(duplicatedPointagePrime);
                    }
                }
            }
            return newListPrimes;
        }

        private RapportLignePrimeEnt DuplicatePrime(RapportLignePrimeEnt pointagePrime, int ciId)
        {
            if (pointagePrime.Prime.Publique || primeManager.GetCIPrimeByPrimeIdAndCiId(pointagePrime.PrimeId, ciId) != null)
            {
                pointagePrime.HeurePrime = 0;
                pointagePrime.IsChecked = false;
                pointagePrime.IsDeleted = false;

                return pointagePrime;
            }

            return null;
        }

        private void GetMaterielDefaultForPersonnel(ref RapportLigneEnt duplicatedPointage)
        {
            MaterielEnt materielDefaut = personnelManager.GetMaterielDefault(duplicatedPointage.PersonnelId.Value);
            if (materielDefaut != null)
            {
                duplicatedPointage.Materiel = materielDefaut;
                duplicatedPointage.MaterielId = materielDefaut.MaterielId;
            }
        }

        private void GetOrCreateIndemniteDeplacement(ref RapportLigneEnt duplicatedPointage)
        {
            var indemniteDeplacement = indemniteDeplacementManager.GetOrCreateIndemniteDeplacementByPersonnelAndCi(duplicatedPointage.Personnel, duplicatedPointage.Ci, false);
            if (indemniteDeplacement != null)
            {
                duplicatedPointage.CodeDeplacement = indemniteDeplacement.CodeDeplacement;
                duplicatedPointage.CodeDeplacementId = indemniteDeplacement.CodeDeplacementId;
                duplicatedPointage.CodeZoneDeplacement = indemniteDeplacement.CodeZoneDeplacement;
                duplicatedPointage.CodeZoneDeplacementId = indemniteDeplacement.CodeZoneDeplacementId;
                duplicatedPointage.DeplacementIV = indemniteDeplacement.IVD;
            }
        }

        private void GetTacheParDefaut(ref RapportLigneEnt duplicatedPointage)
        {
            TacheEnt tacheDefaut = tacheManager.GetTacheParDefaut(duplicatedPointage.CiId);
            if (tacheDefaut != null)
            {
                duplicatedPointage.ListRapportLigneTaches.Add(new RapportLigneTacheEnt()
                {
                    RapportLigneTacheId = 0,
                    RapportLigne = null,
                    RapportLigneId = 0,
                    TacheId = tacheDefaut.TacheId,
                    Tache = tacheDefaut,
                    HeureTache = 0

                });
            }
        }
    }
}
