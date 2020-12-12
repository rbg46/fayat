using Fred.Web.Models;

namespace Fred.Web.Shared.Models.Referential
{
    /// <summary>
    /// Represente une adresse
    /// </summary>
    public class AdresseModel
    {
        /// <summary>
        /// Obtient ou definit l'identifiant unique d'une adresse
        /// </summary>
        public int AdresseId { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse
        /// </summary>
        public string Ligne { get; set; }

        /// <summary>
        /// Obtient ou définit le code postal
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        /// Obtient ou définit la ville
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du pays
        /// </summary>
        public int? PaysId { get; set; }

        /// <summary>
        /// Obtient ou définit le pays
        /// </summary>
        public PaysModel Pays { get; set; }
    }
}
