using System.Diagnostics;

namespace Fred.Entities.RepriseDonnees.Excel
{
    /// <summary>
    /// Repersente un rapport sur un fichier excel d'import pour la reprise de données
    /// </summary>
    [DebuggerDisplay("NumeroDeLigne = {NumeroDeLigne} CodeSociete = {CodeSocieteCi} CodeCi = {CodeCi} DateRapport = {DateRapport}")]
    public class RepriseExcelRapport
    {

        /// <summary>
        /// Le numero de ligne du fichier excel
        /// </summary>
        public string NumeroDeLigne { get; set; }

        /// <summary>
        /// Obtient ou définit le code de ma société.
        /// </summary>
        public string CodeSocieteCi { get; set; }


        /// <summary>
        /// Le code du ci
        /// </summary>
        public string CodeCi { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de l'affaire.
        /// </summary>     
        public string DateRapport { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse2 de l'affaire.
        /// </summary>      
        public string CodeSocietePersonnel { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de l'affaire.
        /// </summary>     
        public string MatriculePersonnel { get; set; }

        /// <summary>
        ///   Obtient ou définit la ville de l'affaire.
        /// </summary>     
        public string CodeDeplacement { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de l'affaire.
        /// </summary>      
        public string CodeZoneDeplacement { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays du CI
        /// </summary>       
        public string IVD { get; set; }

        /// <summary>
        ///   Obtient ou définit les heures Totales
        /// </summary>      
        public string HeuresTotal { get; set; }

        /// <summary>
        ///   Obtient ou définit le code de tache.
        /// </summary>       
        public string CodeTache { get; set; }
      
    }
}
