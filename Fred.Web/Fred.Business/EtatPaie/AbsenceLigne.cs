using Fred.Entities.Referential;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Absence Ligne
    /// </summary>
    public class AbsenceLigne
    {
        /// <summary>
        /// Etablissement
        /// </summary>
        public EtablissementPaieEnt Etablissement { get; set; }
        /// <summary>
        /// Personnel
        /// </summary>
        public string Personnel { get; set; }
        /// <summary>
        /// Statut
        /// </summary>
        public string Statut { get; set; }
        /// <summary>
        /// CI
        /// </summary>
        public string CILibelle { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// total Heures 
        /// </summary>
        public string Heures { get; set; }
        /// <summary>
        /// debut de période d'absence
        /// </summary>
        public string Du { get; set; }
        /// <summary>
        /// fin de période d'absence
        /// </summary>
        public string Au { get; set; }
    }
}
