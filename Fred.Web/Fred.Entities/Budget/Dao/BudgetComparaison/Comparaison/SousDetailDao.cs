namespace Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison
{
    /// <summary>
    /// Représente un sous-détail.
    /// </summary>
    public class SousDetailDao
    {
        /// <summary>
        /// L'identifiant du budget.
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// L'identifiant du sous-détail.
        /// </summary>
        public int SousDetailId { get; set; }

        /// <summary>
        /// L'identifiant de la tâche 1.
        /// </summary>
        public int Tache1Id { get; set; }

        /// <summary>
        /// L'identifiant de la tâche 2.
        /// </summary>
        public int Tache2Id { get; set; }

        /// <summary>
        /// L'identifiant de la tâche 3.
        /// </summary>
        public int Tache3Id { get; set; }

        /// <summary>
        /// L'identifiant de la tâche 4.
        /// </summary>
        public int Tache4Id { get; set; }

        /// <summary>
        /// L'identifiant du chapitre.
        /// </summary>
        public int ChapitreId { get; set; }

        /// <summary>
        /// L'identifiant du sous-chapitre.
        /// </summary>
        public int SousChapitreId { get; set; }

        /// <summary>
        /// L'identifiant de la ressource.
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// La quantité.
        /// </summary>
        public decimal? Quantite { get; set; }

        /// <summary>
        /// L'identifiant de l'unité.
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        /// Le prix unitaire.
        /// </summary>
        public decimal? PrixUnitaire { get; set; }

        /// <summary>
        /// Le montant.
        /// </summary>
        public decimal? Montant { get; set; }
    }
}
