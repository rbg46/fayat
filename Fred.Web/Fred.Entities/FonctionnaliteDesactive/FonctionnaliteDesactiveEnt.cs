using Fred.Entities.Fonctionnalite;
using Fred.Entities.Societe;

namespace Fred.Entities.FonctionnaliteDesactive
{
    /// <summary>
    /// Représente un fonctionnalité desactivée pour une societe.
    /// </summary>
    public class FonctionnaliteDesactiveEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un rôle.
        /// </summary>
        public int FonctionnaliteDesactiveId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une societe.
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la societe.
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une Fonctionnalite.
        /// </summary>
        public int FonctionnaliteId { get; set; }

        /// <summary>
        /// Obtient ou définit la Fonctionnalite.
        /// </summary>
        public virtual FonctionnaliteEnt Fonctionnalite { get; set; }
    }
}
