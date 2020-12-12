using System.Diagnostics;

namespace Fred.Entities.RepriseDonnees.Excel
{
    /// <summary>
    /// Repersente des Materiels sur un fichier excel d'import pour la reprise de données
    /// </summary>
    [DebuggerDisplay("NumeroDeLigne = {NumeroDeLigne} CodeSociete = {CodeSociete} CodeMateriel = {CodeMateriel} CodeRessource = {CodeRessource}")]
    public class RepriseExcelMateriel
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
        /// Code du Matériel au sein de sa societé
        /// </summary>
        public string CodeMateriel { get; set; }

        /// <summary>
        /// Libellé du Matériel au sein de sa societé
        /// </summary>
        public string LibelleMateriel { get; set; }

        /// <summary>
        /// Code de la Ressource au sein de sa societé
        /// </summary>
        public string CodeRessource { get; set; }
    }
}
