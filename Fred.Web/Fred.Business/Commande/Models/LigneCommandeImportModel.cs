using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;

namespace Fred.Business.Commande.Models
{
    /// <summary>
    /// Model Ligne Commande
    /// </summary>
    public class LigneCommandeImportModel
    {
        /// <summary>
        /// Numéro commande
        /// </summary>
        public int Lignecommande { get; set; }

        /// <summary>
        /// Designation
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Ressource
        /// </summary>
        public RessourceModel Ressource { get; set; }

        /// <summary>
        /// Tache
        /// </summary>
        public TacheModel Tache { get; set; }

        /// <summary>
        /// Unité
        /// </summary>
        public UniteModel Unite { get; set; }

        /// <summary>
        /// Prix unitaire
        /// </summary>
        public decimal PuHT { get; set; }

        /// <summary>
        /// Quantité
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        /// Diminution
        /// </summary>
        public bool IsDiminution { get; set; }
    }
}


