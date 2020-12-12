using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Entities.Budget
{
    /// <summary>
    ///   Représente un sous-détail
    /// </summary>
    public class BudgetSousDetailEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un sous-détail.
        /// </summary>
        public int BudgetSousDetailId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la tache de niveau 4 auquel ce budget appartient
        /// </summary>
        public int BudgetT4Id { get; set; }

        /// <summary>
        ///   Obtient ou définit la tache de niveau 4 auquel ce budget appartient
        /// </summary>
        public BudgetT4Ent BudgetT4 { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la ressource auquel se budget appartient
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit la ressource auquel se budget appartient
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité de base calculée
        /// </summary>
        public decimal? Quantite { get; set; }

        /// <summary>
        ///   Obtient ou définit la formule de quantité de base calculée
        /// </summary>
        public string QuantiteFormule { get; set; }

        /// <summary>
        ///   Obtient ou définit le prix unitaire
        /// </summary>
        public decimal? PU { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant
        /// </summary>
        public decimal? Montant { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité SD
        /// </summary>
        public decimal? QuantiteSD { get; set; }

        /// <summary>
        ///   Obtient ou définit la formule de quantité SD
        /// </summary>
        public string QuantiteSDFormule { get; set; }

        /// <summary>
        ///   Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité
        /// </summary>
        public UniteEnt Unite { get; set; }
    }
}
