using Fred.Entities;
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
    ///   Classe de gestion de l'export des erreurs de validation de pointage
    /// </summary>
    public static class ControlePointageErreurExport
    {
        private static Dictionary<int, string> excelTemplates = new Dictionary<int, string>
    {
      { 1, "Templates/ValidationPointage/TemplateErreurControleChantier.xlsx" },
      { 2, "Templates/ValidationPointage/TemplateErreurControleVrac.xlsx" }
    };

        /// <summary>
        ///   Construction du fichier excel
        /// </summary>
        /// <param name="erreurs">Liste des erreurs</param>
        /// <param name="typeControle">Type de controle pointage (Chantier ou Vrac)</param>
        /// <param name="periode">Période du lot</param>
        /// <returns>Pdf</returns>
        public static byte[] ToPdf(IEnumerable<PersonnelErreur<ControlePointageErreurEnt>> erreurs, int typeControle, DateTime periode)
        {
            var stream = new MemoryStream();

            try
            {
                var affairesNonCreees = new List<ControlePointageErreurEnt>();

                InitData(erreurs, periode, affairesNonCreees);

                //// Regroupement par établissement
                var etabs = erreurs.GroupBy(x => x.Personnel.EtablissementPaie?.Code).Select(x => new { CodeEtab = x.Key, Personnels = x.ToList() });

                var excelFormat = new ExcelFormat();
                IWorkbook wb = excelFormat.OpenTemplateWorksheet(AppDomain.CurrentDomain.BaseDirectory + excelTemplates[typeControle]);
                IWorksheet ws = wb.Worksheets[0];

                int rowIndex1 = 5;
                int dateColumnIndex = 6, messageColumnIndex = 7;
                if (typeControle == TypeControlePointage.ControleChantier.ToIntValue())
                {
                    dateColumnIndex = 5;
                    messageColumnIndex = 6;
                }

                // Erreur affaires non créées
                foreach (var aff in affairesNonCreees)
                {
                    if (typeControle == TypeControlePointage.ControleVrac.ToIntValue())
                    {
                        ws.Range[rowIndex1, 5].Text = aff.CodeCi.Trim();
                        ws.Range[rowIndex1, 5].VerticalAlignment = ExcelVAlign.VAlignTop;
                    }

                    ws.Range[rowIndex1, dateColumnIndex].VerticalAlignment = ExcelVAlign.VAlignTop;
                    ws.Range[rowIndex1, dateColumnIndex].Text = string.Format("{0:MM/yyyy}", aff.DateRapport.Value);
                    ws.Range[rowIndex1, messageColumnIndex].VerticalAlignment = ExcelVAlign.VAlignTop;
                    ws.Range[rowIndex1, messageColumnIndex].Text = aff.Message.Trim();
                    ws.Range[rowIndex1, messageColumnIndex].CellStyle.Font.Size = 10;
                    ws.Range[rowIndex1, messageColumnIndex].CellStyle.WrapText = true;
                    ws.Range[rowIndex1, messageColumnIndex].AutofitRows();

                    rowIndex1++;
                }

                // Boucle sur les établissements de paie
                foreach (var etab in etabs.ToList())
                {
                    ws.Range[rowIndex1, 1].Text = etab.CodeEtab;
                    ws.Range[rowIndex1, 1].CellStyle.Color = Color.FromArgb(236, 236, 236);
                    ws.Range[rowIndex1, 1].CellStyle.Font.Size = 14;
                    ws.Range[rowIndex1, 1].CellStyle.Font.Bold = true;
                    ws.Range[rowIndex1, 1, rowIndex1, messageColumnIndex].Merge();
                    ws.Range[rowIndex1, 1, rowIndex1, messageColumnIndex].BorderAround();

                    int rowIndex2 = rowIndex1 + 1;

                    // Boucle sur chaque personnel de l'établissement de paie
                    foreach (var perso in etab.Personnels)
                    {
                        ws.Range[rowIndex2, 2, rowIndex2, messageColumnIndex].BorderAround();
                        ws.Range[rowIndex2, 2].Text = perso.Personnel.Matricule;
                        ws.Range[rowIndex2, 2].CellStyle.Font.Bold = true;
                        ws.Range[rowIndex2, 2].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        ws.Range[rowIndex2, 2].BorderAround();

                        ws.Range[rowIndex2, 3].Text = perso.Personnel.Prenom;

                        ws.Range[rowIndex2, 4].Text = perso.Personnel.Nom;

                        int rowIndex3 = rowIndex2 + 1;

                        // Boucle sur chaque erreur d'un personnel
                        foreach (var err in perso.Erreurs)
                        {
                            FillErreurs(ws, err, rowIndex3, dateColumnIndex, messageColumnIndex, typeControle, periode);
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


        private static void FillErreurs(IWorksheet ws, ControlePointageErreurEnt err, int rowIndex3, int dateColumnIndex, int messageColumnIndex, int typeControle, DateTime periode)
        {
            if (typeControle == TypeControlePointage.ControleVrac.ToIntValue())
            {
                ws.Range[rowIndex3, 5].Text = err.CodeCi.Trim();
            }

            ws.Range[rowIndex3, dateColumnIndex].VerticalAlignment = ExcelVAlign.VAlignTop;
            ws.Range[rowIndex3, dateColumnIndex].Text = err.DateRapport.HasValue ? err.DateRapport.Value.ToShortDateString() : string.Format("{0:MM/yyyy}", periode);

            ws.Range[rowIndex3, messageColumnIndex].VerticalAlignment = ExcelVAlign.VAlignTop;
            ws.Range[rowIndex3, messageColumnIndex].Text = err.Message;
            ws.Range[rowIndex3, messageColumnIndex].CellStyle.WrapText = true;
            ws.Range[rowIndex3, messageColumnIndex].AutofitRows();
        }

        private static void InitData(IEnumerable<PersonnelErreur<ControlePointageErreurEnt>> erreurs, DateTime periode, List<ControlePointageErreurEnt> affairesNonCreees)
        {
            // Si EtablissementPaie n'est pas renseigné pour un personnel, on crée un etab fictif "Inconnu"
            erreurs.ForEach(x =>
            {
                if (x.Personnel.EtablissementPaie == null)
                {
                    x.Personnel.EtablissementPaie = new EtablissementPaieEnt { Code = "Inconnu" };
                }

                foreach (var a in x.Erreurs.Where(z => !z.DateRapport.HasValue && !string.IsNullOrEmpty(z.CodeCi.Trim())).ToList())
                {
                    var err = new ControlePointageErreurEnt
                    {
                        CodeCi = a.CodeCi.Trim(),
                        ControlePointageErreurId = a.ControlePointageErreurId,
                        ControlePointageId = a.ControlePointageId,
                        ControlePointage = a.ControlePointage,
                        DateRapport = periode,
                        Message = a.Message
                    };

                    if (!affairesNonCreees.Any(c => c.CodeCi.Trim() == err.CodeCi.Trim() && c.Message == err.Message))
                    {
                        affairesNonCreees.Add(err);
                    }
                }
            });
        }
    }
}
