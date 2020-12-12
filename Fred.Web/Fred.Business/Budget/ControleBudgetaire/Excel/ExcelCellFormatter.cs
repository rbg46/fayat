using System.Drawing;

namespace Fred.Business.Budget.ControleBudgetaire.Excel
{
    /// <summary>
    /// Contient des fonctions static renvoyant les couleurs pour les différents types de cellule affichées par l'excel.
    /// e.g Couleur des cellules pour un T1 ou T2...
    /// </summary>
    internal static class ExcelCellFormatter
    {
        /// <summary>
        /// Renvoi la couleur par défault pour la colonne budget de l'excel 
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorColonneBudget => Color.FromArgb(217, 225, 242);

        /// <summary>
        /// Retourne la couleur  de la colonne Dad et la colonne Avancement
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorColonneDadAvancement => Color.FromArgb(221, 235, 247);

        /// <summary>
        /// Renvoi la couleur de la colonne ecart
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorEcartRafAjustement => Color.FromArgb(255, 242, 204);

        /// <summary>
        /// Renvoi la couleur de la colonne dépenese
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorColonneDepense => Color.FromArgb(255, 243, 243);

        /// <summary>
        /// Renvoi la couleur de la colonne Ajustement
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorEcartAjustement => Color.FromArgb(255, 242, 204);

        /// <summary>
        /// Renvoi la couleur de la ligne T1
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorT1 => Color.FromArgb(64, 109, 161);

        /// <summary>
        /// Renvoi la couleur de la ligne T2
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorT2 => Color.FromArgb(102, 138, 180);

        /// <summary>
        /// Renvoi la couleur de la ligne T3
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorT3 => Color.FromArgb(160, 182, 208);

        /// <summary>
        /// Renvoi la couleur de la ligne R1
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorR1 => Color.FromArgb(255, 216, 0);

        /// <summary>
        /// Renvoi la couleur de la ligne R2
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorR2 => Color.FromArgb(255, 234, 113);

        /// <summary>
        /// Renvoi la couleur de la ligne R3
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorR3 => Color.FromArgb(255, 244, 187);

        /// <summary>
        /// Renvoi la couleur par défaut
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color ColorCIHeader => Color.FromArgb(191, 191, 191);

        /// <summary>
        /// Renvoi la couleur par défaut
        /// </summary>
        /// <returns>une Couleur jamais null</returns>
        internal static Color DefautColor => Color.FromArgb(255, 255, 255);
    }
}
