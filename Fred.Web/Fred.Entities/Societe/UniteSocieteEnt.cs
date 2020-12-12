using Fred.Entities.Referential;

namespace Fred.Entities.Societe
{
    /// <summary>
    ///   Représente une société
    /// </summary>
    public class UniteSocieteEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une société.
        /// </summary>
        public int UniteSocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet socite attaché à une organisation
        /// </summary>
        public int UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la société de l'établissement
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet socite attaché à une organisation
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la société de l'établissement
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la société de l'établissement
        /// </summary>
        public int Type { get; set; }
    }
}