using System;
using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel
{
    public class RapportLigneSelectModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique du pointage
        /// </summary>
        public int RapportLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du statut du rapport ligne
        /// </summary>
        public int? RapportLigneStatutId { get; set; }

        /// <summary>
        /// Obtient ou défini les heures travaillées réels
        /// </summary>
        public double HeureTotalTravail { get; set; }

        /// <summary>
        /// Obtient ou défini les heures structurels
        /// </summary>
        public int MaxHeuresTravailleesJour { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de pointage
        /// </summary>
        public DateTime DatePointage { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Personnel
        /// </summary>
        public PersonnelSelectModel Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit le code societé du CI
        /// </summary>
        public string CiSocieteCode { get; set; }

        /// <summary>
        /// Obtient ou définit le code etablissement comptable du CI
        /// </summary>
        public string CiEtablissementComptableCode { get; set; }

        /// <summary>
        /// Obtient ou définit le code de CI
        /// </summary>
        public string CiCode { get; set; }

        /// <summary>
        /// Obtient ou définit le code absence
        /// </summary>
        public string CodeAbsenceCode { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit le l'heure de l'absence
        /// </summary>
        public double HeureAbsence { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de suppression de la ligne
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des primes
        /// </summary>
        public ICollection<LignePrimeSelectModel> ListRapportLignePrimes { get; set; } = new List<LignePrimeSelectModel>();

        /// <summary>
        ///   Obtient ou définit la liste des taches
        /// </summary>
        public ICollection<LigneTacheSelectModel> ListRapportLigneTaches { get; set; } = new List<LigneTacheSelectModel>();

        /// <summary>
        /// Obtient ou définit la liste des sorties astreintes
        /// </summary>
        public ICollection<LigneAstreinteSelectModel> ListRapportLigneAstreintes { get; set; } = new List<LigneAstreinteSelectModel>();

        /// <summary>
        /// Obtient ou définit la liste des majorations
        /// </summary>
        public ICollection<LigneMajorationSelectModel> ListRapportLigneMajorations { get; set; } = new List<LigneMajorationSelectModel>();
    }
}
