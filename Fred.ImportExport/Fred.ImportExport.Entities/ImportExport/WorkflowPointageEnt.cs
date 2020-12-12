using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities
{
    [Table("FRED_WORKFLOW_POINTAGE")]
    public class WorkflowPointageEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un workflow.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkflowId { get; set; }

        /// <summary>
        /// Identifiant du rapport
        /// </summary>
        public int RapportId { get; set; }

        /// <summary>
        /// Identifiant de la ligne de pointage
        /// </summary>
        public int RapportLigneId { get; set; }

        /// <summary>
        /// Identifiant du logiciel tiers de destination
        /// </summary>
        [ForeignKey(nameof(LogicielTiers))]
        public int LogicielTiersId { get; set; }

        /// <summary>
        /// Logiciel tiers de destination
        /// </summary>
        public LogicielTiersEnt LogicielTiers { get; set; }

        /// <summary>
        /// Identifiant de l'auteur
        /// </summary>
        public int AuteurId { get; set; }

        /// <summary>
        /// Date d'envoi
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Nom du flux
        /// </summary>
        public string FluxName { get; set; }

        /// <summary>
        /// Identifiant du CI
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Date du pointage
        /// </summary>
        public DateTime DatePointage { get; set; }

        /// <summary>
        /// Identifiant du matériel
        /// </summary>
        public int? MaterielId { get; set; }

        /// <summary>
        /// Identifiant du personnel
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        /// Nombre d'heure de marche du matériel
        /// </summary>
        public double MaterielMarche { get; set; }

        /// <summary>
        /// Nombre d'heure d'arrêt du matériel
        /// </summary>
        public double MaterielArret { get; set; }

        /// <summary>
        /// Nombre d'heure de panne du matériel
        /// </summary>
        public double MaterielPanne { get; set; }

        /// <summary>
        /// Nombre d'heure d'intempérie du matériel
        /// </summary>
        public double MaterielIntemperie { get; set; }

        /// <summary>
        /// Nombre d'heure normale du personnel
        /// </summary>
        public double HeureNormale { get; set; }

        /// <summary>
        /// Nombre d'heure majorée du personnel
        /// </summary>
        public double HeureMajoration { get; set; }

        /// <summary>
        /// Nombre d'heure de marche du matériel
        /// </summary>
        public double HeureAbsence { get; set; }

        /// <summary>
        /// Identifiant du code absence
        /// </summary>
        public int? CodeAbsenceId { get; set; }

        /// <summary>
        /// Identifiant du code majoration
        /// </summary>
        public int? CodeMajorationId { get; set; }

        /// <summary>
        /// Identifiant du code déplacement
        /// </summary>
        public int? CodeDeplacementId { get; set; }

        /// <summary>
        /// Identifiant du code zone déplacmeent
        /// </summary>
        public int? CodeZoneDeplacementId { get; set; }

        /// <summary>
        /// IVD
        /// </summary>
        public bool? DeplacementIV { get; set; }

        /// <summary>
        /// Identifiant de Job Hangfire
        /// </summary>
        public string HangfireJobId { get; set; }

        /// <summary>
        /// Indique si le flux concerne une suppression ou un ajout/modification de pointage
        /// </summary>
        public bool Suppression { get; set; } = false;

        /// <summary>
        /// Date de suppression dans le cas d'une suppression de pointage
        /// </summary>
        public DateTime? DateEnvoiSuppression { get; set; }
    }
}
