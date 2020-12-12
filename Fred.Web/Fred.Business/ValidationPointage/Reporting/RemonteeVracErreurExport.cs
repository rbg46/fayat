using Fred.Entities.Referential;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.Reporting;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Fred.Business.ValidationPointage
{
    /// <summary>
    ///   Classe de gestion de l'export des erreurs de remontée vrac
    /// </summary>
    public static class RemonteeVracErreurExport
    {
        private static string excelTemplate = "Templates/ValidationPointage/TemplateErreurRemonteeVrac.xlsx";

        /// <summary>
        ///   Construction du fichier excel
        /// </summary>
        /// <param name="erreurs">Liste des erreurs</param>        
        /// <returns>Pdf</returns>
        public static byte[] ToPdf(IEnumerable<PersonnelErreur<RemonteeVracErreurEnt>> erreurs)
        {
            var stream = new MemoryStream();

            try
            {
                // Si EtablissementPaie n'est pas renseigné pour un personnel, on crée un etab fictif "Inconnu"
                erreurs.ForEach(x =>
                {
                    if (x.Personnel.EtablissementPaie == null)
                    {
                        x.Personnel.EtablissementPaie = new EtablissementPaieEnt { Code = "Inconnu" };
                    }
                });

                //// Regroupement par établissement
                var etabs = erreurs.GroupBy(x => x.Personnel?.EtablissementPaie?.Code).Select(x => new { CodeEtab = x.Key, Personnels = x.ToList() });

                var excelFormat = new ExcelFormat();
                IWorkbook wb = excelFormat.OpenTemplateWorksheet(AppDomain.CurrentDomain.BaseDirectory + excelTemplate);
                IWorksheet ws = wb.Worksheets[0];

                int rowIndex1 = 5;
                const int dateDebIndex = 5, dateFinIndex = 6, codeAbsAnael = 7, codeAbsFred = 8;

                // Boucle sur les établissements de paie
                foreach (var etab in etabs.ToList())
                {
                    ws.Range[rowIndex1, 1].Text = etab.CodeEtab;
                    ws.Range[rowIndex1, 1].CellStyle.Color = Color.FromArgb(236, 236, 236);
                    ws.Range[rowIndex1, 1].CellStyle.Font.Size = 14;
                    ws.Range[rowIndex1, 1].CellStyle.Font.Bold = true;
                    ws.Range[rowIndex1, 1, rowIndex1, codeAbsFred].Merge();
                    ws.Range[rowIndex1, 1, rowIndex1, codeAbsFred].BorderAround();

                    int rowIndex2 = rowIndex1 + 1;

                    // Boucle sur chaque personnel de l'établissement de paie
                    foreach (var perso in etab.Personnels)
                    {
                        ws.Range[rowIndex2, 2, rowIndex2, codeAbsFred].BorderAround();
                        ws.Range[rowIndex2, 2].Text = perso.Personnel?.Matricule;
                        ws.Range[rowIndex2, 2].CellStyle.Font.Bold = true;
                        ws.Range[rowIndex2, 2].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        ws.Range[rowIndex2, 2].BorderAround();

                        ws.Range[rowIndex2, 3].Text = perso.Personnel?.Prenom;

                        ws.Range[rowIndex2, 4].Text = perso.Personnel?.Nom;

                        int rowIndex3 = rowIndex2 + 1;

                        // Boucle sur chaque erreur d'un personnel
                        foreach (var err in perso.Erreurs)
                        {
                            FillErreurs(ws, err, rowIndex3, dateDebIndex, dateFinIndex, codeAbsAnael, codeAbsFred);
                            rowIndex3++;
                        }

                        rowIndex2 = rowIndex3;
                    }

                    rowIndex1 = rowIndex2;
                }

                //// Convert excel to PDF
                var pdf = excelFormat.PrintExcelToPdf(wb);

                //// Save the document stream
                pdf.Save(stream);

                wb.Close();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }

            return stream.ToArray();
        }

        private static void FillErreurs(IWorksheet ws, RemonteeVracErreurEnt err, int rowIndex3, int dateDebIndex, int dateFinIndex, int codeAbsAnael, int codeAbsFred)
        {
            ws.Range[rowIndex3, dateDebIndex].VerticalAlignment = ExcelVAlign.VAlignTop;
            ws.Range[rowIndex3, dateDebIndex].Text = err.DateDebut.ToShortDateString();

            ws.Range[rowIndex3, dateFinIndex].VerticalAlignment = ExcelVAlign.VAlignTop;
            ws.Range[rowIndex3, dateFinIndex].Text = err.DateFin?.ToShortDateString();

            ws.Range[rowIndex3, codeAbsAnael].VerticalAlignment = ExcelVAlign.VAlignTop;
            ws.Range[rowIndex3, codeAbsAnael].Text = err.CodeAbsenceAnael;

            ws.Range[rowIndex3, codeAbsFred].VerticalAlignment = ExcelVAlign.VAlignTop;
            ws.Range[rowIndex3, codeAbsFred].Text = err.CodeAbsenceFred;
        }
    }
}
