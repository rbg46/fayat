using System;
using System.Collections.Generic;
using Fred.Web.Shared.Models.Budget;
using Fred.Web.Shared.Models.Budget.Recette;

namespace Fred.Web.Models.Budget.Liste
{
    /// <summary>
    /// 
    /// </summary>
    public class ListeBudgetModel
    {
        /// <summary>
        /// Représente l'Id du budget 
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// Définit la période de début du budget à laquel il commence à faire effet. 
        /// Seule la partie Mois et Année de la date est exploitée
        /// Format Mois année e.g Juin 2018
        /// </summary>
        public string PeriodeDebutFormatted { get; set; }

        /// <summary>
        /// Définit la période de fin du budget à laquel il cesse de faire effet. 
        /// Seule la partie Mois et Année de la date est exploitée
        /// Format Mois Année e.g Septembre 2018
        /// </summary>
        public string PeriodeFinFormatted { get; set; }

        /// <summary>
        /// Définit la période de début au format YYYYMM
        /// </summary>
        public int? PeriodeDebut { get; set; }

        /// <summary>
        /// Définit la période de fin au format YYYYMM
        /// </summary>
        public int? PeriodeFin { get; set; }

        /// <summary>
        ///   Définit l'identifiant de la Devise attachée à cette recette
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///   Définit la devise attachée à cette recette
        /// </summary>
        public BudgetEtatModel BudgetEtat { get; set; }

        /// <summary>
        ///   Definie si le budget est partagé ou non
        /// </summary>
        public bool Partage { get; set; }

        /// <summary>
        /// Prenom et nom du créateur du budget
        /// </summary>
        public string Createur { get; set; }

        /// <summary>
        /// Date de création du budget
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// DAte de la dernière modification
        /// </summary>
        public DateTime DateDerniereModification { get; set; }

        /// <summary>
        /// Date de la suppression si le budget a été supprimé
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        /// Prenom et nom de l'auteur de la dernière modification
        /// </summary>
        public string AuteurDeniereModification { get; set; }

        /// <summary>
        /// Prenom et Nom du valideur, peut être null si celui ci n'est pas validé
        /// </summary>
        public string Valideur { get; set; }

        /// <summary>
        /// Date de validation du budget. Peut être null si le budget n'est pas validé
        /// </summary>
        public DateTime? DateValidation { get; set; }

        /// <summary>
        /// Indique si oui ou non le budget est partageable
        /// </summary>
        public bool Partageable { get; set; }

        /// <summary>
        /// Commentaires sur le budget, cela correspond au commentaire donné au workflow le plus récent
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Montant total du budget qui est la somme des montants de tous les T4
        /// </summary>
        public decimal MontantTotal { get; set; }

        /// <summary>
        /// Somme des valeurs définies en recette
        /// </summary>
        public decimal SommeRecettes { get; set; }

        /// <summary>
        /// Représente le détail de chaque recette
        /// </summary>
        public BudgetRecetteModel Recettes { get; set; }

        /// <summary>
        /// Représente l'historique des actions réalisés sur le budget
        /// Par défaut cette liste doit être trié par date descendante (workflow le plus récent d'abord)
        /// </summary>
        public IEnumerable<BudgetWorkflowModel> Workflows { get; set; }

        /// <summary>
        /// L'historique de la copie du budget.
        /// </summary>
        public IEnumerable<BudgetCopyHistoModel> CopyHistos { get; set; }
    }
}
