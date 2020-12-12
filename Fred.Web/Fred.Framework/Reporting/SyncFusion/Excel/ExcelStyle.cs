using System;
using Syncfusion.XlsIO;

namespace Fred.Framework.Reporting.SyncFusion.Excel
{
    /// <summary>
    /// Représente un style Excel.
    /// </summary>
    public abstract class ExcelStyle
    {
        /// <summary>
        /// Le style de base s'il existe.
        /// </summary>
        protected readonly ExcelStyle baseStyle;

        /// <summary>
        /// La fonction qui applique le style.
        /// </summary>
        protected readonly Action<IRange> apply;

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="apply">La fonction qui applique le style.</param>
        protected ExcelStyle(Action<IRange> apply)
        {
            this.apply = apply;
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="baseStyle">Le style de base.</param>
        /// <param name="apply">La fonction qui applique le style.</param>
        protected ExcelStyle(ExcelStyle baseStyle, Action<IRange> apply)
            : this(apply)
        {
            this.baseStyle = baseStyle;
        }

        /// <summary>
        /// Applique le style.
        /// </summary>
        /// <param name="columns">Les colonnes où appliquer le style.</param>
        /// <param name="row">L'index de la ligne où appliquer le style.</param>
        /// <param name="value">La valeur à mettre dans les colonnes.</param>
        protected void Apply(IExcelColumns columns, int row, string value)
        {
            var range = columns.GetRange(row);
            if (baseStyle != null)
            {
                baseStyle.apply(range);
            }
            apply(range);
            if (value != null)
            {
                range.Value = value;
            }
        }
    }
}
