namespace Fred.Framework.Models.Reporting
{
    /// <summary>
    /// Modèle contenant les index de colonne pour l'entête des éditions Excel.
    /// Permet de calculer automatiquement ces index en fonction des largeurs minimums indiquées.
    /// </summary>
    public class BestIndexHeaderExcelModel : IndexHeaderExcelModel
    {
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="logoSocieteMinWidth">La largeur minimum du logo de la société.</param>
        /// <param name="titreMinWidth">La largeur minimum de la colonne du titre.</param>
        /// <param name="infosMinWidth">La largeur minimum de la colonne des informations.</param>
        /// <param name="logoFredMinWidth">La largeur minimum de la colonne du logo de FRED.</param>
        /// <param name="minLastIndex">L'index minimum de la fin de la bannière.</param>
        public BestIndexHeaderExcelModel(double logoSocieteMinWidth, double titreMinWidth, double infosMinWidth, double logoFredMinWidth, int minLastIndex)
            : base(0, 0, 0, 0)
        {
            LogoSocieteMinWidth = logoSocieteMinWidth;
            TitreMinWidth = titreMinWidth;
            InfosMinWidth = infosMinWidth;
            LogoFredMinWidth = logoFredMinWidth;
            MinLastIndex = minLastIndex;
        }

        /// <summary>
        /// La largeur minimum du logo de la société.
        /// </summary>
        public double LogoSocieteMinWidth { get; }

        /// <summary>
        /// La largeur minimum de la colonne du titre.
        /// </summary>
        public double TitreMinWidth { get; }

        /// <summary>
        /// La largeur minimum de la colonne des informations.
        /// </summary>
        public double InfosMinWidth { get; }

        /// <summary>
        /// La largeur minimum de la colonne du logo de FRED.
        /// </summary>
        public double LogoFredMinWidth { get; }

        /// <summary>
        /// L'index minimum de la fin de la bannière.
        /// </summary>
        public int MinLastIndex { get; }
    }
}
