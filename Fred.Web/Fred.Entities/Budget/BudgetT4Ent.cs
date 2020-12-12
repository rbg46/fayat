using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Fred.Entities.Referential;

namespace Fred.Entities.Budget
{
    /// <summary>
    ///   Représente un budget
    /// </summary>
    [DebuggerDisplay("{T4?.Code} - {T4?.Libelle}")]
    public class BudgetT4Ent
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une tâche T4 dans un budget.
        /// </summary>
        public int BudgetT4Id { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du budget
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        ///   Obtient ou définit le budget
        /// </summary>
        public BudgetEnt Budget { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la tache de niveau 4
        /// </summary>
        public int T4Id { get; set; }

        /// <summary>
        ///   Obtient ou définit la tache de niveau 4
        /// </summary>
        public TacheEnt T4 { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant l'unité
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité à réaliser
        /// </summary>
        public decimal? QuantiteARealiser { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant de la tache de niveau 4
        /// </summary>
        public decimal? MontantT4 { get; set; }

        /// <summary>
        ///   Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit le type d'avancement.
        /// Cette valeur n'est représentative que si la société sur laquelle est ce budget n'est pas configurée pour avoir un type d'avancement dynamique
        /// <see cref="TypeAvancementBudget" />
        /// </summary>
        public int? TypeAvancement { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité de base
        /// </summary>
        public decimal? QuantiteDeBase { get; set; }

        /// <summary>
        ///   Obtient ou définit le prix unitaire
        /// </summary>
        public decimal? PU { get; set; }

        /// <summary>
        /// Obtient ou défini la liste des ressources composant le sous-détail
        /// </summary>
        public ICollection<BudgetSousDetailEnt> BudgetSousDetails { get; set; }

        /// <summary>
        /// Obtient ou défini la vue sous-détail.
        /// </summary>
        public int VueSD { get; set; }

        /// <summary>
        /// Obtient ou défini si le budget T4 est en lecture seule
        /// </summary>
        public bool IsReadOnly { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit l'identifiant de la tache de niveau 3
        /// </summary>
        public int? T3Id { get; set; }

        /// <summary>
        ///   Obtient ou définit la tache de niveau 3
        /// </summary>
        public TacheEnt T3 { get; set; }
    }
}
