using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Personnel;
using NLog;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   Le PointageJournalierEnt
    /// </summary>
    public class PointageMensuelPersonEnt
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Obtient ou définit le Année
        /// </summary>
        public int Annee { get; set; }

        /// <summary>
        /// Obtient ou définit le Mois
        /// </summary>
        public int Mois { get; set; }

        /// <summary>
        /// Obtient ou définit le personnel
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit les heures normales
        /// </summary>
        public HeuresNormales ListHeuresNormales { get; set; }

        /// <summary>
        /// Obtient ou définit les heures majoration
        /// </summary>
        public HeuresMajo ListHeuresMajo { get; set; }

        /// <summary>
        /// Obtient ou définit les heures à pied
        /// </summary>
        public HeuresAPied ListHeuresAPied { get; set; }

        /// <summary>
        /// Obtient ou définit les heures absence
        /// </summary>
        public HeuresAbsence ListHeuresAbsence { get; set; }

        /// <summary>
        /// Obtient ou définit les heures pointées
        /// </summary>
        public HeuresPointees ListHeuresPointees { get; set; }

        /// <summary>
        /// Obtient ou définit les heures travaillées
        /// </summary>
        public HeuresTravaillees ListHeuresTravaillees { get; set; }

        /// <summary>
        /// Obtient ou définit les heures travaillées
        /// </summary>
        public List<RapportLigneAstreinteEnt> ListRapportLigneAstreintes { get; set; } = new List<RapportLigneAstreinteEnt>();

        /// <summary>
        /// Obtient ou définit les IVD
        /// </summary>
        public Ivd ListIVD { get; set; }

        /// <summary>
        /// Obtient ou définit les primes
        /// </summary>
        public List<Primes> ListPrimes { get; set; } = new List<Primes>();

        /// <summary>
        /// Obtient ou définit les Astreintes
        /// </summary>
        public List<Astreintes> ListAstreintes { get; set; } = new List<Astreintes>();

        /// <summary>
        /// Obtient ou définit les absences
        /// </summary>
        public List<Absences> ListAbsences { get; set; } = new List<Absences>();

        /// <summary>
        /// Obtient ou définit les déplacements
        /// </summary>
        public List<Deplacements> ListDeplacements { get; set; } = new List<Deplacements>();

        /// <summary>
        /// Obtient ou définit les déplacements
        /// </summary>
        public CodeZoneDeplacements ListCodeZoneDeplacements { get; set; }

        /// <summary>
        /// totalprime
        /// </summary>
        public Dictionary<string, double> Totalprime { get; set; }

        /// <summary>
        /// LoadHeuresNormales
        /// </summary>
        /// <param name="listeHoraireJournalier">listeHoraireJournalier</param>
        public void LoadHeuresNormales(Dictionary<int, string> listeHoraireJournalier)
        {
            HeuresNormales heuresNormales = new HeuresNormales();

            for (int i = 1; i <= 31; i++)
            {
                heuresNormales.GetType().GetProperty("Jour" + i).SetValue(heuresNormales, listeHoraireJournalier[i]);
            }

            ListHeuresNormales = heuresNormales;
        }


        /// <summary>
        /// LoadHeuresMajo
        /// </summary>
        /// <param name="listeHoraireJournalier">listeHoraireJournalier</param>
        public void LoadHeuresMajo(Dictionary<int, string> listeHoraireJournalier)
        {
            HeuresMajo heuresMajo = new HeuresMajo();

            for (int i = 1; i <= 31; i++)
            {
                heuresMajo.GetType().GetProperty("Jour" + i).SetValue(heuresMajo, listeHoraireJournalier[i]);
            }

            ListHeuresMajo = heuresMajo;
        }



        /// <summary>
        /// LoadHeuresAPied
        /// </summary>
        /// <param name="listeHoraireJournalier">listeHoraireJournalier</param>
        public void LoadHeuresAPied(Dictionary<int, string> listeHoraireJournalier)
        {
            HeuresAPied heuresAPied = new HeuresAPied();

            for (int i = 1; i <= 31; i++)
            {
                heuresAPied.GetType().GetProperty("Jour" + i).SetValue(heuresAPied, listeHoraireJournalier[i]);
            }

            ListHeuresAPied = heuresAPied;
        }


        /// <summary>
        /// LoadHeuresTravaillees
        /// </summary>
        /// <param name="listeHoraireJournalier">listeHoraireJournalier</param>
        public void LoadHeuresTravaillees(Dictionary<int, string> listeHoraireJournalier)
        {
            HeuresTravaillees heuresTravaillees = new HeuresTravaillees();

            for (int i = 1; i <= 31; i++)
            {
                heuresTravaillees.GetType().GetProperty("Jour" + i).SetValue(heuresTravaillees, listeHoraireJournalier[i]);
            }

            ListHeuresTravaillees = heuresTravaillees;
        }



        /// <summary>
        /// LoadHeuresAbsence
        /// </summary>
        /// <param name="listeHoraireJournalier">listeHoraireJournalier</param>
        public void LoadHeuresAbsence(Dictionary<int, string> listeHoraireJournalier)
        {
            HeuresAbsence heuresAbsence = new HeuresAbsence();

            for (int i = 1; i <= 31; i++)
            {
                heuresAbsence.GetType().GetProperty("Jour" + i).SetValue(heuresAbsence, listeHoraireJournalier[i]);
            }

            ListHeuresAbsence = heuresAbsence;
        }


        /// <summary>
        /// LoadHeuresPointees
        /// </summary>
        /// <param name="listeHoraireJournalier">listeHoraireJournalier</param>
        public void LoadHeuresPointees(Dictionary<int, string> listeHoraireJournalier)
        {
            HeuresPointees heuresPointees = new HeuresPointees();

            for (int i = 1; i <= 31; i++)
            {
                heuresPointees.GetType().GetProperty("Jour" + i).SetValue(heuresPointees, listeHoraireJournalier[i]);
            }

            ListHeuresPointees = heuresPointees;
        }



        /// <summary>
        /// LoadPrimes
        /// </summary>
        /// <param name="listeHoraireJournalier">listeHoraireJournalier</param>
        /// <param name="codePrime">CodePrime</param>
        public void LoadPrimes(Dictionary<int, string> listeHoraireJournalier, string codePrime)
        {
            Primes primes = new Primes();

            primes.Libelle = "Prime " + codePrime;
            for (int i = 1; i <= 31; i++)
            {
                if (!listeHoraireJournalier.ContainsKey(i))
                {
                    logger.Error($"PrimeJours : Numéro du jour {i} non trouvé dans {listeHoraireJournalier.Keys.ToString()}");
                }
                primes.GetType().GetProperty("Jour" + i).SetValue(primes, listeHoraireJournalier[i]);
            }

            ListPrimes.Add(primes);
        }

        /// <summary>
        /// LoadDeplacement
        /// </summary>
        /// <param name="listeHoraireJournalier">listeHoraireJournalier</param>
        /// <param name="codeDeplacement">CodeDeplacement</param>
        public void LoadDeplacement(Dictionary<int, string> listeHoraireJournalier, string codeDeplacement)
        {
            Deplacements deplacements = new Deplacements();

            deplacements.Libelle = "Déplacement " + codeDeplacement;
            for (int i = 1; i <= 31; i++)
            {
                if (!listeHoraireJournalier.ContainsKey(i))
                {
                    logger.Error($"Deplacement : Numéro du jour {i} non trouvé dans {listeHoraireJournalier.Keys.ToString()}");
                }
                deplacements.GetType().GetProperty("Jour" + i).SetValue(deplacements, listeHoraireJournalier[i]);
            }

            ListDeplacements.Add(deplacements);
        }

        /// <summary>
        /// LoadDeplacement
        /// </summary>
        /// <param name="listeHoraireJournalier">listeHoraireJournalier</param>
        public void LoadCodeZoneDeplacement(Dictionary<int, string> listeHoraireJournalier)
        {
            if (listeHoraireJournalier != null)
            {
                CodeZoneDeplacements codeZoneDeplacements = new CodeZoneDeplacements();

                codeZoneDeplacements.Libelle = "Zone déplacement";
                for (int i = 1; i <= 31; i++)
                {
                    codeZoneDeplacements.GetType().GetProperty("Jour" + i).SetValue(codeZoneDeplacements, listeHoraireJournalier[i]);
                }
                codeZoneDeplacements.Total = listeHoraireJournalier.Count(x => x.Value != string.Empty).ToString();
                if (listeHoraireJournalier.FirstOrDefault(x => x.Value.Contains("*")).Value != null)
                {
                    codeZoneDeplacements.Total = codeZoneDeplacements.Total + "*";
                }

                ListCodeZoneDeplacements = codeZoneDeplacements;
            }
            else
            {
                ListCodeZoneDeplacements = null;
            }
        }

        /// <summary>
        /// LoadDeplacement
        /// </summary>
        /// <param name="listeHoraireJournalier">listeHoraireJournalier</param>
        public void LoadIVD(Dictionary<int, string> listeHoraireJournalier)
        {
            Ivd ivds = new Ivd();
            var empty = true;
            for (int i = 1; i <= 31; i++)
            {
                if (!string.IsNullOrEmpty(listeHoraireJournalier[i]))
                {
                    empty = false;
                    ivds.GetType().GetProperty("Jour" + i).SetValue(ivds, listeHoraireJournalier[i]);
                }
            }
            if (!empty)
            {
                ListIVD = ivds;
            }

        }

        /// <summary>
        /// LoadAbsence
        /// </summary>
        /// <param name="listeHoraireJournalier">listeHoraireJournalier</param>
        /// <param name="codeAbsence">CodeAbsence</param>
        public void LoadAbsence(Dictionary<int, string> listeHoraireJournalier, string codeAbsence)
        {
            Absences absences = new Absences();

            absences.Libelle = "Absence " + codeAbsence;
            for (int i = 1; i <= 31; i++)
            {
                if (!listeHoraireJournalier.ContainsKey(i))
                {
                    logger.Error($"Absences : Numéro du jour {i} non trouvé dans {listeHoraireJournalier.Keys.ToString()}");
                }
                absences.GetType().GetProperty("Jour" + i).SetValue(absences, listeHoraireJournalier[i]);
            }

            ListAbsences.Add(absences);
        }


        /// <summary>
        /// LoadAstreinte
        /// </summary>
        /// <param name="listeAstreinte">listeAstreinte</param>
        /// <param name="codeAstreinte">codeAstreinte</param>
        public void LoadAstreinte(Dictionary<int, string> listeAstreinte, string codeAstreinte)
        {
            Astreintes astreinte = new Astreintes();
            astreinte.Libelle = codeAstreinte;
            astreinte.TotalAstreintes = listeAstreinte.Count(i => i.Value != string.Empty);
            for (int i = 1; i <= 31; i++)
            {
                astreinte.GetType().GetProperty("Jour" + i).SetValue(astreinte, listeAstreinte[i]);
            }
            ListAstreintes.Add(astreinte);
        }

        /// <summary>
        /// Défini la classe des heures normales
        /// </summary>
        public class HeuresNormales
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle
            {
                get { return "Heures normales"; }
            }

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour1 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour2 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour3 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour4 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour5 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour6 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour7 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour8 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour9 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour10 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour11 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour12 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour13 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour14 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour15 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour16 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour17 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour18 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour19 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour20 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour21 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour22 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour23 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour24 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour25 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour26 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour27 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour28 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour29 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour30 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour31 { get; set; } = default(int).ToString();
        }


        /// <summary>
        /// Défini la classe des heures de majorations
        /// </summary>
        public class HeuresMajo
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle
            {
                get { return "Heures majorées"; }
            }

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour1 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour2 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour3 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour4 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour5 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour6 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour7 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour8 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour9 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour10 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour11 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour12 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour13 { get; set; } = default(int).ToString();


            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour14 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour15 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour16 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour17 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour18 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour19 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour20 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour21 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour22 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour23 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour24 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour25 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour26 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour27 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour28 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour29 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour30 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure majorées
            /// </summary>
            public string Jour31 { get; set; } = default(int).ToString();
        }
        /// <summary>
        /// Défini la classe des heures à pied
        /// </summary>
        public class HeuresAPied
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle
            {
                get { return "Heures à pied"; }
            }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour1 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour2 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour3 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour4 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour5 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour6 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour7 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour8 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour9 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour10 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour11 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour12 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour13 { get; set; }


            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour14 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour15 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour16 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour17 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour18 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour19 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour20 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour21 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour22 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour23 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour24 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour25 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour26 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour27 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour28 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour29 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure à pied
            /// </summary>
            public string Jour30 { get; set; }

            /// <summary>
            /// Obtient ou définit le nombre d'heure normales
            /// </summary>
            public string Jour31 { get; set; }
        }

        /// <summary>
        /// Défini la classe des heures travaillées
        /// </summary>
        public class HeuresTravaillees
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle
            {
                get { return "Heures travaillées"; }
            }

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour1 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour2 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour3 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour4 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour5 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour6 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour7 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour8 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour9 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour10 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour11 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour12 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour13 { get; set; } = default(int).ToString();


            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour14 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour15 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour16 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour17 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour18 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour19 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour20 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour21 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour22 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour23 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour24 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour25 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour26 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour27 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour28 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour29 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour30 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure travaillées
            /// </summary>
            public string Jour31 { get; set; } = default(int).ToString();
        }


        /// <summary>
        /// Défini la classe des heures absence
        /// </summary>
        public class HeuresAbsence
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle
            {
                get { return "Heures d'absence"; }
            }

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour1 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour2 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour3 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour4 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour5 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour6 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour7 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour8 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour9 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour10 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour11 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour12 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour13 { get; set; } = default(int).ToString();


            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour14 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour15 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour16 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour17 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour18 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour19 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour20 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour21 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour22 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour23 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour24 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour25 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour26 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour27 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour28 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour29 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour30 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure d'absence
            /// </summary>
            public string Jour31 { get; set; } = default(int).ToString();
        }

        /// <summary>
        /// Défini la classe des heures pointées
        /// </summary>
        public class HeuresPointees
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle
            {
                get { return "Heures pointées"; }
            }

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour1 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour2 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour3 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour4 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour5 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour6 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour7 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour8 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour9 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour10 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour11 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour12 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour13 { get; set; } = default(int).ToString();


            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour14 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour15 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour16 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour17 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour18 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour19 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour20 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour21 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour22 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour23 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour24 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour25 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour26 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour27 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour28 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour29 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour30 { get; set; } = default(int).ToString();

            /// <summary>
            /// Obtient ou définit le nombre d'heure pointées
            /// </summary>
            public string Jour31 { get; set; } = default(int).ToString();
        }

        /// <summary>
        /// Défini la classe des heures de majorations
        /// </summary>
        public class Primes
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle { get; set; }

            /// <summary>
            ///  Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour1 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour2 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour3 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour4 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour5 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour6 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour7 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour8 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour9 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour10 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour11 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour12 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour13 { get; set; } = string.Empty;


            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour14 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour15 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour16 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour17 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour18 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour19 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour20 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour21 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour22 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour23 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour24 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour25 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour26 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour27 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour28 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour29 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour30 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour31 { get; set; } = string.Empty;
        }
        /// <summary>
        /// Défini la classe des Astreintes
        /// </summary>
        public class Astreintes
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle { get; set; }
            /// <summary>
            /// Total prime Astreintes
            /// </summary>
            public int TotalAstreintes { get; set; }

            /// <summary>
            ///  Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour1 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour2 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour3 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour4 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour5 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour6 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour7 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour8 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour9 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour10 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour11 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour12 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour13 { get; set; } = string.Empty;


            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour14 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour15 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour16 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour17 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour18 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour19 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour20 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour21 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour22 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour23 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour24 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour25 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour26 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour27 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour28 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour29 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour30 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'une prime pour ce jour
            /// </summary>
            public string Jour31 { get; set; } = string.Empty;
        }

        /// <summary>
        /// Défini la classe des heures de majorations
        /// </summary>
        public class Deplacements
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle { get; set; }

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour1 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour2 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour3 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour4 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour5 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour6 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour7 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour8 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour9 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour10 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour11 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour12 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour13 { get; set; } = string.Empty;


            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour14 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour15 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour16 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour17 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour18 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour19 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour20 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour21 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour22 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour23 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour24 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour25 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour26 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour27 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour28 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour29 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour30 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour31 { get; set; } = string.Empty;
        }

        /// <summary>
        /// Défini la classe des heures de majorations
        /// </summary>
        public class CodeZoneDeplacements
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle { get; set; }

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour1 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour2 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour3 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour4 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour5 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour6 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour7 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour8 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour9 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour10 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour11 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour12 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour13 { get; set; } = string.Empty;


            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour14 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour15 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour16 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour17 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour18 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour19 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour20 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour21 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour22 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour23 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour24 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour25 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour26 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour27 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour28 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour29 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour30 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour31 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit le total de code zone déplacement affiché
            /// </summary>
            public string Total { get; set; } = string.Empty;
        }

        /// <summary>
        /// Défini la classe des heures de majorations
        /// </summary>
        public class Ivd
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle { get; set; } = "IVD";

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour1 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour2 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour3 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour4 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour5 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour6 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour7 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour8 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour9 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour10 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour11 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour12 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour13 { get; set; } = string.Empty;


            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour14 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour15 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour16 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour17 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour18 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour19 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour20 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour21 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour22 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour23 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour24 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour25 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour26 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour27 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour28 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour29 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour30 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'un déplacment pour ce jour
            /// </summary>
            public string Jour31 { get; set; } = string.Empty;
        }

        /// <summary>
        /// Défini la classe des heures de majorations
        /// </summary>
        public class Absences
        {
            /// <summary>
            /// Obtient ou définit le libelle
            /// </summary>
            public string Libelle { get; set; }

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour1 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour2 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour3 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour4 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour5 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour6 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour7 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour8 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour9 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour10 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour11 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour12 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour13 { get; set; } = string.Empty;


            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour14 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour15 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour16 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour17 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour18 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour19 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour20 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour21 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour22 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour23 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour24 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour25 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour26 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour27 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour28 { get; set; } = string.Empty;

            /// <summary>
            ///  Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour29 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour30 { get; set; } = string.Empty;

            /// <summary>
            /// Obtient ou définit la présence d'un absence pour ce jour
            /// </summary>
            public string Jour31 { get; set; } = string.Empty;


            public IEnumerable<Tuple<int, string>> GetJours()
            {
                yield return new Tuple<int, string>(1, Jour1);
                yield return new Tuple<int, string>(2, Jour2);
                yield return new Tuple<int, string>(3, Jour3);
                yield return new Tuple<int, string>(4, Jour4);
                yield return new Tuple<int, string>(5, Jour5);
                yield return new Tuple<int, string>(6, Jour6);
                yield return new Tuple<int, string>(7, Jour7);
                yield return new Tuple<int, string>(8, Jour8);
                yield return new Tuple<int, string>(9, Jour9);
                yield return new Tuple<int, string>(10, Jour10);
                yield return new Tuple<int, string>(11, Jour11);
                yield return new Tuple<int, string>(12, Jour12);
                yield return new Tuple<int, string>(13, Jour13);
                yield return new Tuple<int, string>(14, Jour14);
                yield return new Tuple<int, string>(15, Jour15);
                yield return new Tuple<int, string>(16, Jour16);
                yield return new Tuple<int, string>(17, Jour17);
                yield return new Tuple<int, string>(18, Jour18);
                yield return new Tuple<int, string>(19, Jour19);
                yield return new Tuple<int, string>(20, Jour20);
                yield return new Tuple<int, string>(21, Jour21);
                yield return new Tuple<int, string>(22, Jour22);
                yield return new Tuple<int, string>(23, Jour23);
                yield return new Tuple<int, string>(24, Jour24);
                yield return new Tuple<int, string>(25, Jour25);
                yield return new Tuple<int, string>(26, Jour26);
                yield return new Tuple<int, string>(27, Jour27);
                yield return new Tuple<int, string>(28, Jour28);
                yield return new Tuple<int, string>(29, Jour29);
                yield return new Tuple<int, string>(30, Jour30);
                yield return new Tuple<int, string>(31, Jour31);
            }
        }


    }
}
