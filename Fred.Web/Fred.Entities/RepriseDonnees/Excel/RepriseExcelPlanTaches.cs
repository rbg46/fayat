using System.Diagnostics;

namespace Fred.Entities.RepriseDonnees.Excel
{
    /// <summary>
    /// Repersente un Plan de taches sur un fichier excel d'import pour la reprise de données
    /// </summary>
    [DebuggerDisplay("NumeroDeLigne = {NumeroDeLigne} CodeSociete = {CodeSociete} CodeCi = {CodeCi} CodeTache = {CodeTache} ")]
    public class RepriseExcelPlanTaches
    {
        /// <summary>
        /// Le numero de ligne du fichier excel
        /// </summary>
        public string NumeroDeLigne { get; set; }

        /// <summary>
        /// CodeSociete
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// CodeCi
        /// </summary>
        public string CodeCi { get; set; }

        /// <summary>
        /// NiveauTache
        /// </summary>
        public string NiveauTache { get; set; }

        /// <summary>
        /// CodeTache
        /// </summary>
        public string CodeTache { get; set; }

        /// <summary>
        /// LibelleTache
        /// </summary>
        public string LibelleTache { get; set; }

        /// <summary>
        /// CodeTacheParent
        /// </summary>
        public string CodeTacheParent { get; set; }

        /// <summary>
        /// T3ParDefaut
        /// </summary>
        public string T3ParDefaut { get; set; }
    }
}
