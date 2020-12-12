using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Fred.Business.CI;
using Fred.Business.FeatureFlipping;
using Fred.Business.Personnel;
using Fred.Business.Rapport.Pointage.Validation;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Extensions;
using Fred.Framework.FeatureFlipping;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel;

namespace Fred.Business.Rapport.Pointage
{
    /// <summary>
    ///   Valideur des Pointages
    /// </summary>
    public class PointageValidator : AbstractValidatorWithManagersAccess<RapportLigneEnt>, IPointageValidator
    {
        private readonly IPersonnelManager personnelManager;
        private readonly ICIManager ciManager;
        private readonly IPointageRepository pointageRepository;
        private readonly IFeatureFlippingManager featureFlippingManager;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="PointageValidator" />.
        /// </summary>        
        /// <param name="personnelManager">Gestionnaire du personnel</param>
        /// <param name="ciManager">Gestionnaire des cis</param>
        /// <param name="pointageRepository">Repository des pointages</param>
        /// <param name="featureFlippingManager">Feature Flipping Manager</param>
        public PointageValidator(IPersonnelManager personnelManager, ICIManager ciManager, IPointageRepository pointageRepository, IFeatureFlippingManager featureFlippingManager)
        {
            this.personnelManager = personnelManager;
            this.ciManager = ciManager;
            this.pointageRepository = pointageRepository;
            this.featureFlippingManager = featureFlippingManager;
        }


        /// <summary>
        ///   Ajout des erreurs dans un pointage Personnel ou Materiel
        /// </summary>      
        /// <param name="rapportLigne">Pointage contrôlé</param>  
        /// <param name="validationGlobalData">Donnée globale pour faire une validation pointage</param>
        public void CheckPointage(RapportLigneEnt rapportLigne, GlobalDataForValidationPointage validationGlobalData)
        {
            if (!CanCheckPointageAndAddErrorMessageIfNecessary(rapportLigne))
            {
                return;
            }

            if (CanCheckPointagePersonnelAndAddErrorMessageIfNecessary(rapportLigne))
            {
                CheckPointagePersonnel(rapportLigne, validationGlobalData);
            }

            if (CanCheckPointageMaterielAndAddErrorMessageIfNecessary(rapportLigne))
            {
                CheckPointageMateriel(rapportLigne);
            }
        }

        /// <summary>
        /// Verifie si on peux controler le pointage
        /// </summary>
        /// <param name="pointage">pointage</param>
        /// <returns>vrai ou faux</returns>
        public bool CanCheckPointageAndAddErrorMessageIfNecessary(RapportLigneEnt pointage)
        {
            pointage.ListErreurs = new List<string>();
            if (string.IsNullOrWhiteSpace(pointage.PrenomNomTemporaire) && string.IsNullOrWhiteSpace(pointage.MaterielNomTemporaire))
            {
                pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Personnel_Materiel_Requis);
                return false;
            }
            return true;
        }

        /// <summary>
        ///  Verifie si on peux controler le pointage personnel
        /// </summary>
        /// <param name="pointage">pointage</param>
        /// <returns>vrai ou faux</returns>
        public bool CanCheckPointagePersonnelAndAddErrorMessageIfNecessary(RapportLigneEnt pointage)
        {
            if (!string.IsNullOrWhiteSpace(pointage.PrenomNomTemporaire) || pointage.PersonnelId != null && !pointage.IsDeleted)
            {
                // Teste si chaque ligne a bien un personnel de paramétrée
                if (string.IsNullOrEmpty(pointage.PrenomNomTemporaire) && (pointage.PersonnelId == null || pointage.PersonnelId == 0))
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Personnel_Requis);
                }
                else
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        ///   Verifie le pointage personnel
        /// </summary>
        /// <param name="pointage">pointage</param>
        /// <param name="validationGlobalData">validationGlobalData</param>
        private void CheckPointagePersonnel(RapportLigneEnt pointage, GlobalDataForValidationPointage validationGlobalData)
        {
            CheckDateEntreeSortie(pointage);
            CheckPointagePersonnelHours(pointage);
            HandleInterimaire(pointage);
            HandleRapportLigneTache(pointage);
            HandleRapportLignePrime(pointage, validationGlobalData.RapportsLignesWithPrimes);
            HandleCodeAbsence(pointage);
            HandleCodeMajoration(pointage);
            HandleUserRole(pointage, validationGlobalData.CiIdsOfPointagesWithRolePaieForCurrentUser);

            // pointage.Personnel != null  rajouté pour les personnel ajouté à la main via lookup
            if (pointage.Personnel != null && pointage.Personnel.IsInterne && !pointage.Personnel.IsInterimaire && pointage.Personnel.EtablissementPaie != null && pointage.Personnel.EtablissementPaie.GestionIndemnites)
            {
                HandleIndemniteDeplacement(pointage, validationGlobalData.CiIdsOfPointagesWithRolePaieForCurrentUser);
            }
        }

        private void CheckDateEntreeSortie(RapportLigneEnt pointage)
        {
            if (pointage.Personnel != null && (!pointage.Personnel.IsInterimaire && ((pointage.Personnel.DateEntree.HasValue && pointage.DatePointage.Date < pointage.Personnel.DateEntree.Value.Date) || (pointage.Personnel.DateSortie.HasValue && pointage.Personnel.DateSortie.Value.Date < pointage.DatePointage.Date))))
            {
                if (pointage.Personnel.DateSortie.HasValue)
                {
                    pointage.ListErreurs.Add(string.Format(FeatureRapport.PointageValidator_Date_Entree_Sortie, pointage.Personnel.DateEntree?.ToShortDateString(), pointage.Personnel.DateSortie.Value.ToShortDateString()));
                }
                else
                {
                    pointage.ListErreurs.Add(string.Format(FeatureRapport.PointageValidator_Date_Entree, pointage.Personnel.DateEntree?.ToShortDateString()));
                }
            }
        }

        private void HandleInterimaire(RapportLigneEnt pointage)
        {
            if (pointage.Ci == null)
            {
                pointage.Ci = ciManager.GetCIById(pointage.CiId);
            }

            // Test si le personnel est interimaire et possède bien un contrat actif pour la date et le CI en cours
            if (pointage.PersonnelId.HasValue && pointage.Personnel.IsInterimaire)
            {
                var contrat = personnelManager.GetAffectationInterimaireActive(pointage.PersonnelId.Value, pointage.DatePointage);
                var affectationInterimaireList = personnelManager.GetAffectationInterimaireList(pointage.PersonnelId.Value);
                bool hasPartialPointage = affectationInterimaireList.Any(c => contrat != null && c.ContratInterimaireId != contrat.ContratInterimaireId && c.ZonesDeTravail.Any(z => z.EtablissementComptableId == pointage.Ci.EtablissementComptableId) && c.DateDebut <= pointage.DatePointage && pointage.DatePointage <= c.DateFin.AddDays(c.Souplesse));
                if (contrat == null)
                {
                    pointage.ListErreurs.Add(string.Format(FeatureRapport.PointageValidator_Hors_Date_Contrat_Interimaire, pointage.Personnel.PrenomNom));
                }
                else if (!hasPartialPointage && contrat.CiId != pointage.CiId && !contrat.ZonesDeTravail.Any(z => z.EtablissementComptableId == pointage.Ci.EtablissementComptableId))
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Hors_Zone_Contrat_Interimaire);
                }
            }
        }

        private void HandleCodeAbsence(RapportLigneEnt pointage)
        {
            // Test si les codes absence utilisés ont une heure de paramétrée
            if (pointage.CodeAbsence != null && pointage.HeureAbsence <= 0 && !pointage.IsGenerated)
            {
                pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Heure_Absence_Nulle);
            }

            // Test si le code absence est vide et que les heures sont paramétrées
            if (pointage.CodeAbsence == null && pointage.HeureAbsence > 0)
            {
                pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Code_Absence_Nul);
            }
        }

        private void HandleCodeMajoration(RapportLigneEnt pointage)
        {
            if (pointage.Ci?.Societe?.Groupe != null && !pointage.Ci.Societe.Groupe.Code.Trim().Equals(FeatureRapport.Code_Groupe_FES))
            {
                // Test si les codes majoration utilisés ont une heure de paramétrée
                if (pointage.CodeMajoration != null && pointage.HeureMajoration <= 0)
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Heure_Majoration_Nulle);
                }

                // Test si les codes majoration utilisés ont une heure de paramétrée
                if (pointage.CodeMajoration == null && pointage.HeureMajoration > 0)
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Code_Majoration_Nul);
                }

                // Test si les heures paramétrées ne dépassent pas les bornes
                if (pointage.CodeMajoration != null && pointage.HeureMajoration > 12)
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Heure_Majoration_Seuil);
                }
            }
        }

        private void HandleRapportLignePrime(RapportLigneEnt pointage, IEnumerable<RapportLigneEnt> rapportsLignesWithPrimes)
        {
            // BUG 5298 : BUG_US_163 Saisie manuelle dans un rapport
            if (pointage.PersonnelId.HasValue)
            {
                // Test que les heures de chaque prime <= totale heure normale et heure majo
                bool errorPrime = false;
                foreach (var lignePrime in pointage.ListRapportLignePrimes.Where(o => !o.IsDeleted))
                {
                    if (lignePrime.Prime.PrimeType == ListePrimeType.PrimeTypeHoraire && lignePrime.HeurePrime > 0 && lignePrime.HeurePrime > (pointage.HeureNormale + pointage.HeureMajoration))
                    {
                        errorPrime = true;
                    }

                    var primeJournaliereAlreadyExists = pointageRepository.IsPrimeJournaliereAlreadyExists(rapportsLignesWithPrimes, lignePrime.PrimeId, pointage.PersonnelId.Value, pointage.DatePointage, pointage.RapportLigneId);
                    if (lignePrime.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere &&
                        lignePrime.IsChecked &&
                        primeJournaliereAlreadyExists && pointage.Ci.Societe.Groupe.Code.Trim() == FeatureRapport.Code_Groupe_FES)
                    {
                        pointage.ListErreurs.Add(string.Format(FeatureRapport.PointageValidator_Doublon_Prime, lignePrime.Prime.Code));
                    }
                }

                if (errorPrime)
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Exces_Heure_Prime);
                }
            }
        }

        private void HandleRapportLigneTache(RapportLigneEnt pointage)
        {
            // Test que le total des heures taches = totale heure normale et heure majo
            double totalHeuresTaches = 0.0;
            foreach (var ligneTache in pointage.ListRapportLigneTaches.Where(o => !o.IsDeleted))
            {
                totalHeuresTaches += ligneTache.HeureTache;
            }

            if (totalHeuresTaches.IsNotEqual(pointage.HeureNormale + pointage.HeureMajoration))
            {
                pointage.ListErreurs.Add(FeatureRapport.Pointage_Error_Heures_Taches_Invalides);
            }
        }

        private void HandleUserRole(RapportLigneEnt pointage, List<int> ciIdsOfPointagesWithRolePaieForCurrentUser)
        {
            if (ciIdsOfPointagesWithRolePaieForCurrentUser.Contains(pointage.CiId))
            {
                // RG_163_165
                if (pointage.PrenomNomTemporaire != null && pointage.CodeDeplacement != null && !pointage.CodeDeplacement.IGD && !pointage.CodeDeplacement.IndemniteForfaitaire && pointage.CodeZoneDeplacementId == null)
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Code_Deplacement_Requis);
                }

                // RG_163_166
                if (pointage.PrenomNomTemporaire != null && pointage.CodeZoneDeplacementId != null && pointage.CodeDeplacement != null && pointage.CodeDeplacement.IGD)
                {
                    pointage.ListErreurs.Add(FeatureIndemniteDeplacement.IndemniteDeplacement_Index_IGD_Zone_Error);
                }

                // RG_163_167
                if (pointage.PrenomNomTemporaire != null && pointage.DeplacementIV && pointage.CodeDeplacement != null && !pointage.CodeDeplacement.IGD)
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_IVD_Requis);
                }

                if (pointage.CodeAbsence != null && pointage.CodeAbsence.Intemperie && pointage.NumSemaineIntemperieAbsence == null)
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Numero_Semaine_Requis);
                }
            }
        }

        private void HandleIndemniteDeplacement(RapportLigneEnt pointage, List<int> ciIdsOfPointagesWithRolePaieForCurrentUser)
        {
            // RG_5174_001
            if (this.featureFlippingManager.IsActivated(EnumFeatureFlipping.VerificationCoordonneesGPS)
                && pointage.PersonnelId != 0
                && (!pointage.CodeDeplacementId.HasValue || pointage.CodeDeplacementId.Value == 0)
                && (!pointage.CodeZoneDeplacementId.HasValue || pointage.CodeZoneDeplacementId.Value == 0)
                && ciIdsOfPointagesWithRolePaieForCurrentUser.Contains(pointage.CiId))
            {
                List<string> errors;
                if (!Managers.Pointage.CanCalculateIndemniteDeplacement(pointage, out errors))
                {
                    foreach (var error in errors)
                    {
                        pointage.ListErreurs.Add(error);
                    }
                }
            }
        }

        /// <summary>
        ///  Verifie si on peux controler le pointage materiel
        /// </summary>
        /// <param name="pointage">pointage</param>
        /// <returns>vrai ou faux</returns>
        public bool CanCheckPointageMaterielAndAddErrorMessageIfNecessary(RapportLigneEnt pointage)
        {
            if ((!string.IsNullOrWhiteSpace(pointage.MaterielNomTemporaire) || pointage.MaterielId != null) && !pointage.IsDeleted)
            {
                //// On verifie la saisie de MaterielNomTemporaire
                if (pointage.MaterielNomTemporaire == null || pointage.MaterielNomTemporaire.Length == 0)
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Materiel_Requis);
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///  Verifie les erreurs materiel sur le pointage
        /// </summary>
        /// <param name="rapportLigne">pointage</param>
        public void CheckPointageMateriel(RapportLigneEnt rapportLigne)
        {
            // RG_74_109
            if (rapportLigne.MaterielMarche.IsZero() &&
                rapportLigne.MaterielArret.IsZero() &&
                rapportLigne.MaterielPanne.IsZero() &&
                rapportLigne.MaterielIntemperie.IsZero())
            {
                rapportLigne.ListErreurs.Add(FeatureRapport.PointageValidator_Heure_Materiel_Requise);
            }

            // RG_74_113
            if (rapportLigne.MaterielMarche + rapportLigne.MaterielArret > 12)
            {
                rapportLigne.ListErreurs.Add(FeatureRapport.PointageValidator_Exces_Heure_Materiel_Marche_Attente);
            }

            // RG_74_114
            if (rapportLigne.MaterielMarche + rapportLigne.MaterielArret + rapportLigne.MaterielPanne + rapportLigne.MaterielIntemperie > 24)
            {
                rapportLigne.ListErreurs.Add(FeatureRapport.PointageValidator_Exces_Heure_Materiel);
            }
        }

        private double HeuresTravaillees(PointageBase pointage)
        {
            return pointage.HeureNormale + pointage.HeureMajoration;
        }

        private double HeuresTravailleesForGFES(RapportLigneEnt pointage)
        {
            return pointage.HeureNormale +
                (pointage.ListRapportLigneMajorations != null ? pointage.ListRapportLigneMajorations.Any(m => m.CodeMajoration.IsHeureNuit) ? pointage.ListRapportLigneMajorations.Where(m => m.CodeMajoration.IsHeureNuit).Sum(m => m.HeureMajoration) : 0 : 0);
        }

        private double GetHeuresMajorationNuit(RapportLigneEnt pointage)
        {
            double heuresMajorationNuit = 0;
            // le cas de FES
            if (!pointage.CodeMajorationId.HasValue || !pointage.ListRapportLigneMajorations.IsNullOrEmpty())
            {
                foreach (var rapportLigneMajoration in pointage.ListRapportLigneMajorations)
                {
                    if (rapportLigneMajoration.CodeMajoration.IsHeureNuit)
                    {
                        heuresMajorationNuit += rapportLigneMajoration.HeureMajoration;
                    }
                }
            }
            else
            {
                if (pointage.CodeMajoration.IsHeureNuit)
                {
                    heuresMajorationNuit += pointage.HeureMajoration;
                }
            }

            return heuresMajorationNuit;
        }

        /// <summary>
        ///   Valideur
        /// </summary>
        /// <param name="instance">Pointage</param>
        /// <returns>Résultat de validation</returns>
        public new ValidationResult Validate(RapportLigneEnt instance)
        {
            return base.Validate(instance);
        }

        /// <summary>
        ///   Teste un pointage personnel
        /// </summary>
        /// <param name="pointage">Le pointage à vérifier</param>
        private void CheckPointagePersonnelHours(RapportLigneEnt pointage)
        {
            if (pointage.Ci?.Societe?.Groupe != null && pointage.Ci.Societe.Groupe.Code.Trim().Equals(FeatureRapport.Code_Groupe_FES))
            {
                if (HeuresTravailleesForGFES(pointage).IsZero() && pointage.HeureAbsence.IsZero())
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Verifier_Heures);
                }
            }
            else
            {
                // Test qu'on ai au moins des heures normales, ou absence
                if (HeuresTravaillees(pointage).IsZero() && pointage.HeureAbsence.IsZero())
                {
                    pointage.ListErreurs.Add(FeatureRapport.PointageValidator_Heure_Travaillee_Absence_Requise);
                }
            }

            // Test que le nombre d'heures travaillées maximum n'ont pas été dépassées
            var heuresMajorationNuit = GetHeuresMajorationNuit(pointage);
            var heuresTotalDeJour = (int)(pointage.HeureNormale + heuresMajorationNuit + pointage.HeureAbsence);
            if (heuresTotalDeJour > pointage.MaxHeuresTravailleesJour)
            {
                pointage.ListErreurs.Add(string.Format(FeatureRapport.PointageValidator_Exces_Heure_Travaillee, pointage.MaxHeuresTravailleesJour));
            }
        }

        /// <summary>
        /// Permet de controler les absences FIGGO
        /// </summary>
        /// <param name="rapportLigne">Pointage contrôlé</param>
        /// <param name="referentielPersonnel">referentiel Personnel</param>
        /// <param name="referentielSociete">referentiel Societe</param>
        /// <param name="referentielCodeAbs">referentiel Code Abs</param>
        /// <param name="referentielStatutAbs">referentiel Statut Abs</param>
        /// <returns>liste Tibco format</returns>
        public List<TibcoModel> CheckPointageFiggo(RapportLigneEnt rapportLigne,
            List<PersonnelEnt> referentielPersonnel,
            List<SocieteEnt> referentielSociete,
            List<CodeAbsenceEnt> referentielCodeAbs,
            List<StatutAbsenceEnt> referentielStatutAbs
            )
        {
            var controles = new TibcoListBuilder();

            controles.ControleMatriculePersonnel(rapportLigne.Personnel.Matricule, referentielPersonnel)
                     .ControleMatriculeValideur(rapportLigne.Valideur.Personnel.Matricule, referentielPersonnel)
                     .ControleSocietePersonnel(rapportLigne.Personnel.Societe.Code, referentielSociete)
                     .ControleSocieteValideur(rapportLigne.Valideur.Personnel.Societe.Code, referentielSociete)
                     .ControleCodeAbsence(rapportLigne.AffectationAbsence.CodeAbsence.Code, referentielCodeAbs)
                     .ControleAbsenceStatut(rapportLigne.AffectationAbsence.StatutAbsence.Code, referentielStatutAbs)
                     .ControleCi(rapportLigne.Personnel, rapportLigne.CiId);

            return controles;
        }

        /// <summary>
        /// Permet de controler les rapportslignes pour tibco
        /// </summary>
        /// <param name="rapportLigne">liste de ligne de rapport</param>
        /// <param name="datedebut">date debut choisi</param>
        /// <param name="datefin">date fin choisi</param>
        /// <returns>une liste de controle</returns>
        public IEnumerable<ControleSaisiesErreurTibcoModel> CheckPointagesForTibco(List<RapportLigneSelectModel> rapportLigne, DateTime datedebut, DateTime datefin)
        {
            var controles = new List<ControleSaisiesErreurTibcoModel>();
            if (rapportLigne.Any())
            {
                foreach (var ligne in rapportLigne)
                {
                    if (!(ligne.RapportLigneStatutId.HasValue && ligne.RapportLigneStatutId.Value.Equals(RapportStatutEnt.RapportStatutVerrouille.Key)
                    ) && !controles.Any(c => c.DateRapport.HasValue && c.DateRapport == ligne.DatePointage.Date))
                    {
                        controles.Add(new ControleSaisiesErreurTibcoModel
                        {
                            DateRapport = ligne.DatePointage,
                            Message = FeatureRapport.RapportLigneValidator_StatutNonVerrouille
                        });
                    }
                    double sumMajoration = 0;
                    if (ligne.ListRapportLigneMajorations != null && ligne.ListRapportLigneMajorations.Any())
                    {
                        sumMajoration = ligne.ListRapportLigneMajorations.Sum(x => x.HeureMajoration);
                    }

                    if ((ligne.HeureTotalTravail + ligne.HeureAbsence + sumMajoration < ligne.MaxHeuresTravailleesJour) && !controles.Any(c => c.DateRapport.HasValue && c.DateRapport == ligne.DatePointage.Date))
                    {
                        controles.Add(new ControleSaisiesErreurTibcoModel
                        {
                            DateRapport = ligne.DatePointage,
                            Message = FeatureRapport.RapportLigneValidator_DonneeIncomplete
                        });
                    }
                }

                var date = datedebut.Date;
                var datesPointes = rapportLigne.Select(r => r.DatePointage.Date).ToList();

                while (date.Date <= datefin.Date)
                {
                    if (!datesPointes.Contains(date.Date) && (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday))
                    {
                        controles.Add(new ControleSaisiesErreurTibcoModel
                        {
                            DateRapport = date,
                            Message = FeatureRapport.RapportLigneValidator_DonneeManquante
                        });
                    }
                    date = date.AddDays(1);
                }
            }
            return controles;
        }
    }
}
