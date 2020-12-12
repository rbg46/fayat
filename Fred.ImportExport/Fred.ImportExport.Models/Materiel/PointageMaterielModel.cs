using System;
using System.Collections.Generic;

namespace Fred.ImportExport.Models.Materiel
{
    public class PointageMaterielModel
    {
        /// <summary>
        /// Obtient ou définit le codeSocieteComptable de la societe.
        /// </summary>
        public string SocieteComptableCode { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une ligne de rapport.
        /// </summary>
        public int RapportLigneId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une affaire.
        /// Ci.Code
        /// </summary>
        public string CiCode { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant d'une affaire.
        /// Ci.Code
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou définit le code condensé de la société.
        /// AuteurCreation.Personnel.Societe.Code
        /// </summary>
        public string AuteurSocieteCode { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule de l'auteur de la ligne.
        /// AuteurCreation.Personnel.Matricule
        /// </summary>
        public string AuteurMatricule { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création.
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de modification 
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit la date de suppression
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit la date du pointage.
        /// </summary>
        public DateTime DatePointage { get; set; }

        /// <summary>
        /// Obtient ou définit le code de la société du matériel.
        /// Materiel.Societe.Code
        /// </summary>
        public string MaterielSocieteCode { get; set; }

        /// <summary>
        /// Obtient ou définit le code de la société du matériel.
        /// Materiel.Code
        /// </summary>
        public string MaterielCode { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la société du matériel.
        /// Materiel.Code
        /// </summary>
        public int? MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule du valideur du rapport.
        /// [ValideurCDC ou ValideurCDT ou ValideurDRC].Personnel.Matricule
        /// </summary>
        public string ValideurMatricule { get; set; }

        /// <summary>
        /// Obtient ou définit le code de la société du valideur du rapport.
        /// [ValideurCDC ou ValideurCDT ou ValideurDRC].Personnel.Societe.Code
        /// </summary>
        public string ValideurSocieteCode { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule du personnel.
        /// Personnel.Matricule
        /// </summary>
        public string PersonnelMatricule { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre total des heures des tâches.
        /// </summary>
        public double TotalHeuresTaches { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre d'heure majorée du personnel.
        /// </summary>
        public double HeureMajoration { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre d'heure majorée du personnel.
        /// </summary>
        public double HeureAbsence { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un code déplacement.
        /// CodeDeplacement.Code
        /// </summary>
        public string CodeDeplacementCode { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant d'un code déplacement.
        /// CodeDeplacement.Code
        /// </summary>
        public int? CodeDeplacementId { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre d'heure de marche du matériel.
        /// </summary>
        public double MaterielMarche { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre d'heure d'attente du matériel.
        /// </summary>
        public double MaterielArret { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre d'heure de panne du matériel.
        /// </summary>
        public double MaterielPanne { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre d'heure d'intempérie du matériel.
        /// </summary>
        public double MaterielIntemperie { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique du rapport.
        /// </summary>
        public int RapportId { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des tâches.
        /// </summary>
        public IEnumerable<PointageTacheStormModel> Taches { get; set; }
    }
}
