using System.Diagnostics;

namespace Fred.Entities.RepriseDonnees.Excel
{
    /// <summary>
    /// Repersente des Indemnités de déplacement sur un fichier excel d'import pour la reprise de données
    /// </summary>
    [DebuggerDisplay("NumeroDeLigne = {NumeroDeLigne} CodeCI = {CodeCI} MatriculePersonnel = {MatriculePersonnel} CodeDeplacement = {CodeDeplacement} ")]
    public class RepriseExcelIndemniteDeplacement
    {
        /// <summary>
        /// Le numero de ligne du fichier excel
        /// </summary>
        public string NumeroDeLigne { get; set; }

        /// <summary>
        /// SocieteCI
        /// </summary>
        public string SocieteCI { get; set; }

        /// <summary>
        /// CodeCI
        /// </summary>
        public string CodeCI { get; set; }

        /// <summary>
        /// SocietePersonnel
        /// </summary>
        public string SocietePersonnel { get; set; }

        /// <summary>
        /// MatriculePersonnel
        /// </summary>
        public string MatriculePersonnel { get; set; }

        /// <summary>
        /// NbKlm
        /// </summary>
        public string NbKlm { get; set; }

        /// <summary>
        /// NbKlmVODomicileRattachement
        /// </summary>
        public string NbKlmVODomicileRattachement { get; set; }

        /// <summary>
        /// NbKlmVODomicileChantier
        /// </summary>
        public string NbKlmVODomicileChantier { get; set; }

        /// <summary>
        /// NbKlmVOChantierRattachement
        /// </summary>
        public string NbKlmVOChantierRattachement { get; set; }

        /// <summary>
        /// Code Societe du Code Déplacement
        /// </summary>
        public string SocieteCodeDeplacement { get; set; }

        /// <summary>
        /// Code Déplacement
        /// </summary>
        public string CodeDeplacement { get; set; }

        /// <summary>
        /// CodeZoneDeplacement
        /// </summary>
        public string CodeZoneDeplacement { get; set; }

        /// <summary>
        /// IVD (O/N)
        /// </summary>
        public string IVD { get; set; }

        /// <summary>
        /// Date du dernier Calcul
        /// </summary>
        public string DateDernierCalcul { get; set; }

        /// <summary>
        /// Saisie Manuelle (O/N)
        /// </summary>
        public string SaisieManuelle { get; set; }
    }
}
