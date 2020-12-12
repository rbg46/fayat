using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fred.Entities.Budget.Recette;
using Fred.Entities.CI;
using Fred.Entities.Referential;

namespace Fred.Entities.Budget
{
    /// <summary>
    ///   Représente un budget
    /// </summary>
    public class BudgetEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un budget.
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du CI auquel se budget appartient
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit le CI auquel ce budget appartient
        /// </summary>
        public CIEnt Ci { get; set; }

        /// <summary>
        /// Définit la période de début du budget à laquel il prend effet. 
        /// Format YYYYMM
        /// </summary>
        public int? PeriodeDebut { get; set; }

        /// <summary>
        /// Définit la période de fin du budget à laquel il cesse de faire effet. 
        /// Format YYYYMM
        /// </summary>
        public int? PeriodeFin { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la Devise attachée à cette recette
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise attachée à cette recette
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit la version du budget
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///   Obtient ou définit l'état du budget
        /// </summary>
        public int BudgetEtatId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'état du budget
        /// </summary>
        public BudgetEtatEnt BudgetEtat { get; set; }

        /// <summary>
        /// Représente toutes les étates de modification de ce budget
        /// </summary>
        public ICollection<BudgetWorkflowEnt> Workflows { get; set; }

        /// <summary>
        /// Représente tous les T4 associés à ce budget
        /// </summary>
        public ICollection<BudgetT4Ent> BudgetT4s { get; set; }

        /// <summary>
        /// Recette du budget
        /// </summary>
        public virtual BudgetRecetteEnt Recette { get; set; }

        /// <summary>
        ///   Obtient ou définit le partage du budget
        /// </summary>
        public bool Partage { get; set; } = false;

        /// <summary>
        ///  Null si le budget n'a jamais été supprimé, sinon contient la date de suppression
        ///  la composante horaire n'est pas utilisée
        /// </summary>
        public DateTime? DateSuppressionBudget { get; set; } = null;

        /// <summary>
        /// Obtient ou définit le Libelle du budget
        /// </summary>
        [Display(Name = "Libelle")]
        public string Libelle { get; set; }

        /// <summary>
        /// Date de suppression de la notification "Nouvelles tâches"
        /// </summary>
        public DateTime? DateDeleteNotificationNewTask { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// L'historique de la copie du budget.
        /// </summary>
        public IList<BudgetCopyHistoEnt> CopyHistos { get; set; }
    }
}
