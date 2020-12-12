using Fred.Entities.Referential;

namespace Fred.Entities.CI
{
    /// <summary>
    ///   Représente un CIDevise (association entre un CI et une devise)
    /// </summary>
    public class CIDeviseEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'association.
        /// </summary>
        public int CiDeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un CI.
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une devise.
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit le CI associé
        /// </summary>
        public virtual CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise associé
        /// </summary>
        public virtual DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la référence est associé
        /// </summary>
        public bool Reference { get; set; }
    }
}