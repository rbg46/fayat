using System;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Budget.Avancement
{
    /// <summary>
    ///   Représente un workflow pour l'avancement
    /// </summary>
    public class AvancementWorkflowEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un workflow avancement
        /// </summary>
        public int AvancementWorkflowId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du avancement
        /// </summary>
        public int AvancementId { get; set; }

        /// <summary>
        ///   Obtient ou définit le avancement
        /// </summary>
        public AvancementEnt Avancement { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identfiant de l'état initial
        /// </summary>
        public int? EtatInitialId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'état initial
        /// </summary>
        public AvancementEtatEnt EtatInitial { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identfiant de l'état cible
        /// </summary>
        public int EtatCibleId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'état cible
        /// </summary>
        public AvancementEtatEnt EtatCible { get; set; }

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