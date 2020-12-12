using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport
{
    /// <summary>
    /// Synthese Validation affaire model
    /// </summary>
    public class SyntheseValidationAffairesModel
    {
        /// <summary>
        /// Ci Identifiant
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// List des personnels Ids affected
        /// </summary>
        public List<int> AffectedPersonnelsIds { get; set; }

        /// <summary>
        /// Code societe
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Code Etablissement comptable
        /// </summary>
        public string CodeEtabComptable { get; set; }

        /// <summary>
        /// Code Ci et libelle
        /// </summary>
        public string CodeCi_Libelle { get; set; }

        /// <summary>
        /// Statut du Ci
        /// </summary>
        public string Statut { get; set; }

        /// <summary>
        /// Total heure normal + Majoratin nuit
        /// </summary>
        public double TotalHeure { get; set; }

        /// <summary>
        /// Total Heure sup
        /// </summary>
        public double TotalHeureSup { get; set; }

        /// <summary>
        /// Nbre des primes
        /// </summary>
        public double TotalNbrePrime { get; set; }

        /// <summary>
        /// Total des heures astreintes
        /// </summary>
        public double TotalHeureAstreinte { get; set; }

        /// <summary>
        /// Total heures absence
        /// </summary>
        public double TotalHeureAbsence { get; set; }

        /// <summary>
        /// Check if Ci is selected 
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
