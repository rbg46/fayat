using Fred.Web.Shared.Models.Budget;
using Syncfusion.XlsIO;
using System.Collections.Generic;
using System.Drawing;
using static Fred.Business.Budget.Avancement.Excel.AvancementExcelDescriber;

namespace Fred.Business.Budget.Avancement.Excel
{
    /// <summary>
    /// Cette classe offre l'accès a des propriétés non null contenant les styles et couleurs à utiliser dans l'export excel de l'avancement
    /// </summary>
    internal class AvancementExcelCellFormatter
    {
        private readonly IEnumerable<int> allRows;
        private readonly IWorksheet sheet;
        private readonly int totalRowIndex;

        private readonly Color colorT1Header = Color.FromArgb(48, 84, 150);
        private readonly Color colorT2Header = Color.FromArgb(47, 117, 181);
        private readonly Color colorT3Header = Color.FromArgb(142, 169, 219);
        private readonly Color colorT4Header = Color.White;

        private readonly Color colorT1Footer = Color.FromArgb(255, 192, 0);
        private readonly Color colorT2Footer = Color.FromArgb(255, 217, 102);
        private readonly Color colorT3Footer = Color.FromArgb(255, 230, 153);
        private readonly Color colorT4Footer = Color.FromArgb(255, 242, 204);

        private readonly Color colorTotal = Color.FromArgb(221, 235, 247);


        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="sheet">feuille excel contenant les données de l'avancement</param>
        /// <param name="allRows">Contient les indices (excel) de toutes les lignes affichées dans l'excel sauf la ligne des totaux</param>
        /// <param name="totalRowIndex">Indice (excel) de la ligne contenant les totaux</param>
        internal AvancementExcelCellFormatter(IWorksheet sheet, IEnumerable<int> allRows, int totalRowIndex)
        {
            this.sheet = sheet;
            this.allRows = allRows;
            this.totalRowIndex = totalRowIndex;
        }

        /// <summary>
        /// Applique le style des totaux pour la colonne donnée
        /// </summary>
        /// <param name="column">colonne dont on veut styliser le total</param>
        /// <param name="useBorder">true si la cellule doit avoir une bordure</param>
        internal void SetTotalStyleForColumn(char column, bool useBorder = true)
        {
            IRange cell;
            switch (column)
            {
                case AvancementMoisCourantUniteColumn:
                case AvancementMoisPrecedentUniteColumn:
                case EcartAvancementUniteColumn:
                    cell = sheet[$"{column}{totalRowIndex}"];
                    cell.CellStyle.Color = colorTotal;
                    cell.CellStyle.Font.Bold = true;
                    cell.HorizontalAlignment = ExcelHAlign.HAlignLeft;

                    if (useBorder)
                    {
                        cell.BorderAround();
                    }
                    break;
                default:
                    cell = sheet[$"{column}{totalRowIndex}"];
                    cell.CellStyle.Color = colorTotal;
                    cell.CellStyle.Font.Bold = true;
                    cell.HorizontalAlignment = ExcelHAlign.HAlignRight;

                    if (useBorder)
                    {
                        cell.BorderAround();
                    }
                    break;
            }
        }

        /// <summary>
        /// Dessine les bordures pour les cellules de l'avancement
        /// </summary>
        internal void SetBordersForAvancementCells()
        {
            foreach (var row in allRows)
            {
                var range = $"{AvancementMoisPrecedentColumn}{row}:{AvancementMoisPrecedentUniteColumn}{row}";
                sheet[range].BorderAround();

                range = $"{AvancementMoisCourantColumn}{row}:{AvancementMoisCourantUniteColumn}{row}";
                sheet[range].BorderAround();

                range = $"{EcartAvancementColumn}{row}:{EcartAvancementUniteColumn}{row}";
                sheet[range].BorderAround();
            }

            var totalRange = $"{AvancementMoisPrecedentColumn}{totalRowIndex}:{AvancementMoisPrecedentUniteColumn}{totalRowIndex}";
            sheet[totalRange].BorderAround();

            totalRange = $"{AvancementMoisCourantColumn}{totalRowIndex}:{AvancementMoisCourantUniteColumn}{totalRowIndex}";
            sheet[totalRange].BorderAround();

            totalRange = $"{EcartAvancementColumn}{totalRowIndex}:{EcartAvancementUniteColumn}{totalRowIndex}";
            sheet[totalRange].BorderAround();

        }

        /// <summary>
        /// Applique le style associé à la colonne donnée
        /// </summary>
        /// <param name="column">La colonne à styliser</param>
        internal void SetStyleForColumn(char column)
        {
            switch (column)
            {
                case CodeColumn:
                case LibelleColumn:
                case CommentaireColumn:
                case UniteColumn:
                    SetStyleForColumn(column, LineSection.Header, ExcelHAlign.HAlignCenter);
                    break;

                case QuantiteColumn:
                case PuColumn:
                case MontantColumn:
                    SetStyleForColumn(column, LineSection.Header, ExcelHAlign.HAlignRight);
                    break;

                case AvancementMoisPrecedentColumn:
                case AvancementMoisCourantColumn:
                case EcartAvancementColumn:
                    SetStyleForColumn(column, LineSection.Footer, ExcelHAlign.HAlignRight, false);
                    break;

                case AvancementMoisPrecedentUniteColumn:
                case AvancementMoisCourantUniteColumn:
                case EcartAvancementUniteColumn:
                    SetStyleForColumn(column, LineSection.Footer, ExcelHAlign.HAlignLeft, false);
                    break;

                case DadMoisPrecedentColumn:
                case DadMoisCourantColumn:
                case EcartDadColumn:
                    SetStyleForColumn(column, LineSection.Footer, ExcelHAlign.HAlignRight);
                    break;

            }
        }

        private void SetStyleForColumn(char column, LineSection lineSection, ExcelHAlign horizontalAlign, bool useBorder = true)
        {
            var columnIndex = GetColumnIndexFromColumnCharIndex(column);
            var columnCodeTache = GetColumnIndexFromColumnCharIndex(CodeColumn);

            foreach (var row in allRows)
            {
                var cell = sheet[row, columnIndex];
                var code = sheet[row, columnCodeTache].Text.Substring(0, 2);

                if (code == NiveauxTaches.T1.ToFriendlyName())
                {
                    SetColorForSpecificCell(cell, lineSection, colorT1Header, colorT1Footer, useBorder);
                }
                else if (code == NiveauxTaches.T2.ToFriendlyName())
                {
                    SetColorForSpecificCell(cell, lineSection, colorT2Header, colorT2Footer, useBorder);
                }
                else if (code == NiveauxTaches.T3.ToFriendlyName())
                {
                    SetColorForSpecificCell(cell, lineSection, colorT3Header, colorT3Footer, useBorder);
                }
                else if (code == NiveauxTaches.T4.ToFriendlyName())
                {
                    SetColorForSpecificCell(cell, lineSection, colorT4Header, colorT4Footer, useBorder);
                }

                sheet[row, columnIndex].HorizontalAlignment = horizontalAlign;
            }
        }

        private void SetColorForSpecificCell(IRange cell, LineSection lineSection, Color colorHeader, Color colorFooter, bool useBorder)
        {
            switch (lineSection)
            {
                case LineSection.Header:
                    cell.CellStyle.Color = colorHeader;
                    break;
                case LineSection.Footer:
                    cell.CellStyle.Color = colorFooter;
                    break;
            }

            if (useBorder)
            {
                cell.BorderAround();
            }
        }

        private int GetColumnIndexFromColumnCharIndex(char column)
        {
            //+1 car les colonnes démarrent à partir de 1
            return column - 'A' + 1;
        }

    }
}
