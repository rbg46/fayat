using Fred.Entities.Personnel;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.Rapport
{
    /// <summary>
    ///   Modèle pour l'édition : Vérification des Temps
    /// </summary>
    public class SummaryMensuelPersoModel
    {
        /// <summary>
        /// Obtient ou définit le personnel
        /// </summary>
        public string PersonnelLibelle
        {
            get
            {
                if (!string.IsNullOrEmpty(PersonnelCodeSociete))
                {
                    return string.Concat(PersonnelCodeSociete, " - ", Matricule, " - ", Libelle);
                }
                return string.Concat(Matricule, " - ", Libelle);
            }
        }
        /// <summary>
        /// Obtient ou définit le statut personnel
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit le libelle personnel
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le code personnel
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour1 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour2 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour3 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour4 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour5 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour6 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour7 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour8 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour9 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour10 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour11 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour12 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour13 { get; set; } = default(int).ToString();


        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour14 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour15 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour16 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour17 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour18 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour19 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour20 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour21 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour22 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour23 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour24 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour25 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour26 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour27 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour28 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour29 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour30 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le nombre d'heure
        /// </summary>
        public string Jour31 { get; set; } = default(int).ToString();

        /// <summary>
        /// Obtient ou définit le personnel
        /// </summary>
        public string TotalHeuresTravaillees { get; set; }

        // RG_5172_001-2
        /// <summary>
        /// Le code de la société d’appartenance du personnel.
        /// </summary>
        public string PersonnelCodeSociete { get; set; }

        /// <summary>
        /// Day date time dictionnary : Cette propriété a été ajoutée pour pouvoir distinguer les weekends
        /// des indexs utilisés pour determiner les jours ouvrés . (Bug 4991)
        /// </summary>
        public Dictionary<int, DateTime> DayDateTimeDictionnary { get; set; }
    }
}
