using Fred.Entities.Referential;

namespace Fred.Entities.Budget.Avancement
{
    /// <summary>
    ///   Représente une tache d'avancement, lien entre une tache, un budget et une période
    /// </summary>
    public class AvancementTacheEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un avancement tâche .
        /// </summary>
        public int AvancementTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du budget
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        ///   Obtient ou définit le budget
        /// </summary>
        public BudgetEnt Budget { get; set; }

        /// <summary>
        /// Définit la période de début du avancement à laquel il prend effet. 
        /// Format YYYYMM
        /// </summary>
        public int Periode { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la tâche
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit la tâche
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit le commentaire d'avancement
        /// </summary>
        public string Commentaire { get; set; }
    }
}
