using Fred.Entities.Module;
using Fred.Entities.Societe;

namespace Fred.Entities.ModuleDesactive
{
    /// <summary>
    ///   Représente un module desactive pour une societe.
    /// </summary>
    public class ModuleDesactiveEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un rôle.
        /// </summary>
        public int ModuleDesactiveId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une societe.
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la societe.
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un module.
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// Obtient ou définit le module.
        /// </summary>
        public virtual ModuleEnt Module { get; set; } // FK_FONCTIONNALITE_MODULE
    }
}
