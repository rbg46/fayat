using Fred.Entities.ReferentielFixe;

namespace Fred.Entities.CI
{
    /// <summary>
    ///   Représente un CIRessource (association entre un CI et une Ressource)
    /// </summary>
    public class CIRessourceEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'association.
        /// </summary>
        public int CiRessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un CI.
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une Ressource.
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit le CI associé
        /// </summary>
        public virtual CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit la Ressource associé
        /// </summary>
        public virtual RessourceEnt Ressource { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant la consommation au niveau CI
        /// </summary>
        public decimal? Consommation { get; set; }
    }
}