using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport
{
    /// <summary>
    ///   le model personnel à exporter vers TIBCO
    /// </summary>
    public class ExportPersonnelRapportLigneModel
    {
        /// <summary>
        ///  numéro du rapport ligne
        /// </summary>
        public int numero { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si le rapport est au statut Vérouillé
        /// </summary>
        public bool IsStatutVerrouille { get; set; }

        /// <summary>
        /// Obtient ou définit la date du pointage
        /// </summary>
        public DateTime DatePointage { get; set; }

        /// <summary>
        /// Code de la société du personnel pointé
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Code de l’établissement de paie du personnel pointé
        /// </summary>
        public string EtablissementPaieCode { get; set; }

        /// <summary>
        ///   Obtient ou définit le code du l'etablissement comptable
        /// </summary>
        public string EtablissementComptableCode { get; set; }


        /// <summary>
        /// Obtient ou définit le nom du personnel
        /// </summary>
        public string PersonnelNom { get; set; }

        /// <summary>
        /// Obtient ou définit le prenom du personnel
        /// </summary>
        public string PersonnelPrenom { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule du personnel
        /// </summary>
        public string PersonnelMatricule { get; set; }

        /// <summary>
        /// Obtient ou définit le code societé du CI
        /// </summary>
        public string SocieteCi { get; set; }

        /// <summary>
        /// Obtient ou définit le code etablissement comptable du CI
        /// </summary>
        public string EtablissementComptableCi { get; set; }

        /// <summary>
        /// Obtient ou définit le code de CI
        /// </summary>
        public string CiCode { get; set; }

        /// <summary>
        /// Code de l’absence
        /// </summary>
        public string AbsenceCode { get; set; }

        /// <summary>
        /// Quantité en heure saisie pour l’absence
        /// </summary>
        public double HeuresAbsences { get; set; }

        /// <summary>
        /// Données concernant la tâche
        /// </summary>
        public ICollection<ExportPersonnelSousRapportLignesModel> TacheLignes { get; set; }

        /// <summary>
        /// Données concernant les majorations 
        /// </summary>
        public ICollection<ExportPersonnelSousRapportLignesModel> MajorationLignes { get; set; }

        /// <summary>
        /// Données concernant les primes 
        /// </summary>
        public ICollection<ExportPersonnelSousRapportLignesModel> PrimeLignes { get; set; }

        /// <summary>
        /// Données concernant les sorties d’astreinte 
        /// </summary>
        public ICollection<ExportPersonnelSousRapportLignesModel> AstreinteLignes { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire 
        /// </summary>
        public string Commentaire { get; set; }
    }
}
