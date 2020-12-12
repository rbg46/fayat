using Fred.Business.CI;
using Fred.Business.VerificationPointage.Model;
using Fred.Entities.CI;
using Fred.Entities.VerificationPointage;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Fred.Framework.Reporting;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Fred.Business.VerificationPointage
{
    /// <summary>
    /// Exporter le resultat vers fichier Excel
    /// </summary>
    public class ChekingpointingReport
    {
        private const string ExcelTemplateChekingPointing = "Templates/PointagePersonnel/TemplateChekingPointing.xlsx";
        private const int StartligneExcel = 6;
        private readonly List<IStyle> listStyle = new List<IStyle>();
        private int maxdaysmonth;
        private readonly ICIManager ciManager;

        /// <summary>
        /// Le gestionnaire des CI.
        /// </summary>
        /// <param name="ciManager">Manager CI</param>
        public ChekingpointingReport(ICIManager ciManager)
        {
            this.ciManager = ciManager;
        }

        /// <summary>
        /// Exporter le resultat vers fichier Excel
        /// </summary>
        /// <param name="chekingPointingMonths">données verification pointage plat</param>
        /// <param name = "param" >paramètre filtre</param>
        /// <returns>Fichier Byte </returns>
        public byte[] ExportInExcelChekingPointing(List<ChekingPointingMonth> chekingPointingMonths, FilterChekingPointing param)
        {
            MemoryStream memoryStream = new MemoryStream();
            ExcelFormat excelFormat = new ExcelFormat();
            try
            {
                IEnumerable<CIEnt> listCi = ciManager.GetCIList();
                IWorkbook excelTemplate = excelFormat.OpenTemplateWorksheet(AppDomain.CurrentDomain.BaseDirectory + ExcelTemplateChekingPointing);
                var workSheetTemplate = excelTemplate.ActiveSheet;
                FormatStyleCells(excelTemplate);

                //grouper les données sous plusieurs groupes(Personel ou materiel /CI /Etat Materiel)
                List<ChekingPointingGroup> result = ExecuteGroupQuery(chekingPointingMonths, param.TypePointing);

                int i = StartligneExcel;
                maxdaysmonth = DateTime.DaysInMonth(param.Period.Value.Year, param.Period.Value.Month);
                workSheetTemplate.Range["x2"].Value2 = param.Period.Value;
                workSheetTemplate.PageSetup.CenterHeader = string.Format(workSheetTemplate.PageSetup.CenterHeader, ((DisplayTypePointing)param.TypePointing).ToString().ToUpper());
                InitExcelheader(workSheetTemplate, param);

                foreach (ChekingPointingGroup rows in result)
                {
                    i++;
                    //Lister groupe Personnel / Machine
                    Addligne(workSheetTemplate, rows, 0, ref i);
                }
                Freedays(workSheetTemplate, param, StartligneExcel, i);
                workSheetTemplate.UsedRange.AutofitColumns();
                workSheetTemplate.Range["B2"].Value2 = string.Join(" / ", listCi.Where(x => param.Cis.Contains(x.CiId)).Select(x => x.Societe.Code + "-" + x.Code + "-" + x.Libelle).ToList());
                SaveToCorrectFormat(ref excelFormat, excelTemplate, ref memoryStream, "Excel");
            }
            catch (Exception e)
            {
                throw new FredTechnicalException(e.Message, e);
            }

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Sauvegarde dans le stream les données générées
        /// </summary>
        /// <param name="excelFormat">L'objet excel d'origine</param>
        /// <param name="excelTemplate">le workbook</param>
        /// <param name="memoryStream">Le stream de convertion</param>
        /// <param name="format">PDF ou Excel</param>
        private void SaveToCorrectFormat(ref ExcelFormat excelFormat, IWorkbook excelTemplate, ref MemoryStream memoryStream, string format)
        {
            if (format.Equals("PDF"))
            {
                excelFormat.PrintExcelToPdf(excelTemplate).Save(memoryStream);
            }
            else
            {
                excelFormat.SaveExcelToMemoryStream(excelTemplate, memoryStream);
            }
        }

        private void Addligne(IWorksheet ws, ChekingPointingGroup row, int level, ref int ligne)
        {
            double totalperso = 0;
            int firstligne = ligne;
            const int firstcol = 1;

            ws.Range[ligne, firstcol].Text = row.Libelle;
            ws.Range[ligne, firstcol, ligne, maxdaysmonth + firstcol].CellStyle = listStyle[level];

            foreach (var dayworks in row.DayWorks)
            {
                if (!dayworks.Value.Equals(0))
                {
                    ws.Range[ligne, dayworks.Key + 1].Value2 = dayworks.Value;
                    totalperso += dayworks.Value;
                }
            }
            ws.Range[ligne, maxdaysmonth + firstcol + 1].Value2 = totalperso;
            ws.Range[ligne, maxdaysmonth + firstcol + 1].CellStyle = listStyle[4];
            if (row.Lignepointing != null)
            {
                foreach (ChekingPointingGroup rows in row.Lignepointing)
                {
                    ligne++;
                    Addligne(ws, rows, level + 1, ref ligne);
                }
                ws.Range["A" + (firstligne + 1) + ":A" + ligne].Group(ExcelGroupBy.ByRows, true);
            }
        }

        private List<ChekingPointingGroup> ExecuteGroupQuery(List<ChekingPointingMonth> chekingPointingMonths, int display)
        {
            switch (display)
            {
                case (int)DisplayTypePointing.Personnels:

                    return chekingPointingMonths.SelectMany(x => x.DayWorks, (employeeObj, workDays) => new { employeeObj, workDays })
                        .OrderBy(x => x.employeeObj.InfoLabelle)
                                .GroupBy(x => x.employeeObj.InfoLabelle)
                                .Select(x => new ChekingPointingGroup
                                {
                                    Libelle = x.Key,
                                    DayWorks = x.GroupBy(z => z.workDays.Key).Select(
                                        sg => new { key = sg.Key, valeur = sg.Sum(t => t.workDays.Value) }).ToDictionary(dc => dc.key, dc => dc.valeur),

                                    Lignepointing = x.GroupBy(gp => new { gp.employeeObj.InfoCI })
                                    .Select(z => new ChekingPointingGroup
                                    {
                                        Libelle = z.Key.InfoCI,
                                        DayWorks = z.GroupBy(grp => grp.workDays.Key).Select(
                                            sg => new { key = sg.Key, valeur = sg.Sum(t => t.workDays.Value) }).ToDictionary(dc => dc.key, dc => dc.valeur)
                                    })
                                    .OrderBy(o => o.Libelle)
                                    .ToList()
                                })
                               .ToList();

                default:
                    return chekingPointingMonths.SelectMany(x => x.DayWorks, (materielObj, workDays) => new { materielObj, workDays })
                        .OrderBy(x => x.materielObj.InfoLabelle)
                                 .GroupBy(x => x.materielObj.InfoLabelle)
                                .Select(x => new ChekingPointingGroup
                                {
                                    Libelle = x.Key,
                                    DayWorks = x.GroupBy(z => z.workDays.Key).Select(
                                        sg => new { key = sg.Key, valeur = sg.Sum(t => t.workDays.Value) }).ToDictionary(dc => dc.key, dc => dc.valeur),

                                    Lignepointing = x.GroupBy(gp => new { gp.materielObj.InfoCI })
                                    .Select(
                                        z => new ChekingPointingGroup
                                        {
                                            Libelle = z.Key.InfoCI,
                                            DayWorks = z.GroupBy(grp => grp.workDays.Key).Select(
                                            sg => new { key = sg.Key, valeur = sg.Sum(t => t.workDays.Value) }).ToDictionary(dc => dc.key, dc => dc.valeur),
                                            Lignepointing = z.GroupBy(gp => gp.materielObj.EtatMachine).Select(
                                                    sz => new ChekingPointingGroup
                                                    {
                                                        Libelle = sz.Key,
                                                        DayWorks = sz.GroupBy(grp => grp.workDays.Key).Select(
                                                        sg => new { key = sg.Key, valeur = sg.Sum(t => t.workDays.Value) }).ToDictionary(dc => dc.key, dc => dc.valeur)
                                                    }).ToList()
                                        })
                                        .OrderBy(o => o.Libelle)
                                        .ToList(),
                                }).ToList();
            }
        }

        private void FormatStyleCells(IWorkbook wb)
        {
            IStyle stylePersoMac = wb.Styles.Add("StylePersoMac");
            IStyle styleCI = wb.Styles.Add("StyleCi");
            IStyle stylestatMac = wb.Styles.Add("StylestatMzv");
            IStyle stylefreeday = wb.Styles.Add("Stylefreeday");
            IStyle styleTotal = wb.Styles.Add("stymletotal");

            stylePersoMac.BeginUpdate();
            stylePersoMac.Font.Bold = true;
            stylePersoMac.Font.FontName = "Calibri";
            stylePersoMac.Font.Size = 8;

            styleCI.BeginUpdate();
            styleCI.Font.Bold = false;
            styleCI.Font.FontName = "Calibri";
            styleCI.Font.Size = 8;
            styleCI.Color = Color.FromArgb(231, 230, 230);

            stylestatMac.BeginUpdate();
            stylestatMac.Font.Bold = false;
            stylestatMac.Font.FontName = "Calibri";
            stylestatMac.Font.Size = 8;
            stylestatMac.Color = Color.FromArgb(220, 220, 220);

            stylefreeday.BeginUpdate();
            stylefreeday.Font.Bold = false;
            stylefreeday.Font.FontName = "Calibri";
            stylefreeday.Font.Size = 8;
            stylefreeday.Color = Color.FromArgb(242, 242, 242);

            styleTotal.BeginUpdate();
            styleTotal.Font.Bold = true;
            styleTotal.Font.FontName = "Calibri";
            styleTotal.Font.Size = 9;
            styleTotal.HorizontalAlignment = ExcelHAlign.HAlignRight;

            stylePersoMac.EndUpdate();
            styleCI.EndUpdate();
            stylestatMac.EndUpdate();
            stylefreeday.EndUpdate();
            styleTotal.EndUpdate();

            listStyle.Add(stylePersoMac);
            listStyle.Add(styleCI);
            listStyle.Add(stylestatMac);
            listStyle.Add(stylefreeday);
            listStyle.Add(styleTotal);
        }

        private void Freedays(IWorksheet ws, FilterChekingPointing param, int startligne, int endligne)
        {
            DateTime testdate = param.Period.Value;
            DateTimeExtendManager dateTimeExtendManager = new DateTimeExtendManager();
            for (int i = 0; i <= maxdaysmonth - 1; i++)
            {
                if (dateTimeExtendManager.IsHoliday(testdate.AddDays(i)) || dateTimeExtendManager.IsWeekend(testdate.AddDays(i)))
                {
                    ws.Range[startligne, i + 2, endligne, i + 2].CellStyle = listStyle[3];
                }
            }
        }

        private void InitExcelheader(IWorksheet ws, FilterChekingPointing param)
        {
            DateTime testdate = param.Period.Value;
            int i;
            for (i = 0; i <= maxdaysmonth - 1; i++)
            {
                ws.Range[StartligneExcel - 2, i + 2].Value2 = testdate.AddDays(i).ToString("ddd").ToUpper().Substring(0, 1);
                ws.Range[StartligneExcel - 1, i + 2].Text = testdate.AddDays(i).Day.ToString();
            }
            ws.Range[StartligneExcel - 2, i + 2].Value2 = "Total";
            ws.Range[StartligneExcel - 2, i + 2].CellStyle.Font.Bold = true;
            ws.Range[StartligneExcel - 2, i + 2].CellStyle.Font.Underline = ExcelUnderline.Single;

            // Répétition des headers si impression multi-page
            ws.PageSetup.PrintTitleRows = "$" + ws.Range[StartligneExcel - 2, i + 2].Row + ":$" + ws.Range[StartligneExcel - 1, i + 2].Row;
        }
    }
}
