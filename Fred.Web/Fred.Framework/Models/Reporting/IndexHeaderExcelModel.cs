namespace Fred.Framework.Models.Reporting
{
    /// <summary>
    /// Modèle contenant les index de colonne pour l'entête des éditions Excel
    /// </summary>
    public class IndexHeaderExcelModel
    {
        /// <summary>
        /// Constructeur du modèle contenant les index de colonne pour l'entête de l'édition
        /// </summary>
        /// <param name="indexColonneTitre">Index de la colonne pour le titre</param>
        /// <param name="indexColonneInfos">Index de la colonne pour les informations</param>
        /// <param name="indexColonneLogoFred">Index de la colonne pour le logo FRED</param>
        /// <param name="indexDerniereColonne">Index de la dernière colonne de la page</param>
        public IndexHeaderExcelModel(int indexColonneTitre, int indexColonneInfos, int indexColonneLogoFred, int indexDerniereColonne)
        {
            IndexColonneTitre = indexColonneTitre;
            IndexColonneInfos = indexColonneInfos;
            IndexColonneLogoFred = indexColonneLogoFred;
            IndexDerniereColonne = indexDerniereColonne;
        }

        /// <summary>
        /// Index de la colonne du titre
        /// </summary>
        public int IndexColonneTitre { get; internal set; }

        /// <summary>
        /// Index de la colonne des informations
        /// </summary>
        public int IndexColonneInfos { get; internal set; }

        /// <summary>
        /// Index de la colonne pour le logo Fred
        /// </summary>
        public int IndexColonneLogoFred { get; internal set; }

        /// <summary>
        /// Index de la colonne pour le logo Fred
        /// </summary>
        public int IndexDerniereColonne { get; internal set; }
    }
}
