using System;
using Syncfusion.XlsIO;

namespace Fred.Framework.Reporting.SyncFusion.Excel
{
    /// <summary>
    /// Représente un style basique Excel.
    /// Ici la fonction d'application du style ne prend pas de paramètre utilisateur.
    /// </summary>
    public sealed class ExcelBasicStyle : ExcelStyle
    {
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="apply">La fonction qui applique le style.</param>
        public ExcelBasicStyle(Action<IRange> apply)
            : base(apply)
        { }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="baseStyle">Le style de base.</param>
        /// <param name="apply">La fonction qui applique le style.</param>
        public ExcelBasicStyle(ExcelStyle baseStyle, Action<IRange> apply)
            : base(baseStyle, apply)
        { }

        /// <summary>
        /// Applique le style.
        /// </summary>
        /// <param name="columns">Les colonnes où appliquer le style.</param>
        /// <param name="row">L'index de la ligne où appliquer le style.</param>
        /// <param name="value">La valeur à mettre dans les colonnes.</param>
        public new void Apply(IExcelColumns columns, int row, string value)
        {
            base.Apply(columns, row, value);
        }
    }
}
