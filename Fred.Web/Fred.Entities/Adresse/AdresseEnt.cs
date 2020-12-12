using System;
using System.Diagnostics;
using Fred.Entities.EntityBase;
using Fred.Entities.Referential;

namespace Fred.Entities.Adresse
{
    /// <summary>
    /// Represente une adresse
    /// </summary>
    [DebuggerDisplay("AdresseId = {AdresseId} CodePostal = {CodePostal} Ville = {Ville}")]
    public class AdresseEnt : AuditableEntity, IEquatable<AdresseEnt>
    {
        /// <summary>
        /// Obtient ou definit l'identifiant unique d'une adresse
        /// </summary>
        public int? AdresseId { get; set; }

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
        public PaysEnt Pays { get; set; }

        /// <summary>
        /// envoie l'égalité entre deux entités Adresse
        /// </summary>
        /// <param name="other">Adresse à comparer</param>
        /// <returns>retourne vrai si l'égalité est juste</returns>
        public bool Equals(AdresseEnt other)
        {
            return this.Ligne == other.Ligne
                && this.CodePostal == other.CodePostal
                && this.Ville == other.Ville
                && this.PaysId == other.PaysId;
        }
    }
}
