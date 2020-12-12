using System;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Budget
{
    /// <summary>
    ///   Représente un budget
    /// </summary>
    public class BudgetWorkflowEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un workflow budget
        /// </summary>
        public int BudgetWorkflowId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du budget
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        ///   Obtient ou définit le budget
        /// </summary>
        public BudgetEnt Budget { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identfiant de l'état initial
        /// </summary>
        public int? EtatInitialId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'état initial
        /// </summary>
        public BudgetEtatEnt EtatInitial { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identfiant de l'état cible
        /// </summary>
        public int EtatCibleId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'état cible
        /// </summary>
        public BudgetEtatEnt EtatCible { get; set; }

        /// <summary>
        ///   Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur
        /// </summary>
        public int AuteurId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur
        /// </summary>
        public UtilisateurEnt Auteur { get; set; }

        /// <summary>
        ///   Obtient ou définit le date
        /// </summary>
        public DateTime Date { get; set; }
    }
}
