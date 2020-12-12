using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Framework.Exceptions;
using Fred.Framework.Linq;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Web.Shared.App_LocalResources;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fred.Business.Rapport
{
    /// <summary>
    ///   Classe de gestion de l'export des pointages d'un personnel pour un mois donné
    /// </summary>
    public static class PointagePersonnelExport
    {
        private const string NumberFormat = "0.0";
        private const int InitRowIndex = 10;
        private const int PrimeJournaliereQte = 1;

        /// <summary>
        ///   Construction du fichier excel
        /// </summary>
        /// <param name="pointages">Liste des pointages personnel</param>
        /// <param name="editePar">Matricule - Nom Prénom de l'éditeur</param>
        /// <param name="periode">Période de filtrage pour l'édition</param>
        /// <param name="pathLogo">Chemin du logo de la société</param>
        /// <param name="isFes">Indique si l'édition est à destination de FES</param>
        /// <returns>Tableau de bytes de l'excel</returns>
        public static byte[] ToExcel(IEnumerable<RapportLigneEnt> pointages, string editePar, DateTime periode, string pathLogo, bool isFes, string templateFolderPath)
        {
            var stream = new MemoryStream();

            IWorkbook wb = BuildWorkbook(pointages, editePar, periode, pathLogo, isFes, templateFolderPath);

            wb.Worksheets[0].Select();
            wb.SaveAs(stream);

            wb.Close();

            return stream.ToArray();
        }

        /// <summary>
        ///   Construction du fichier PDF
        /// </summary>
        /// <param name="pointages">Liste des pointages personnel</param>
        /// <param name="editePar">Matricule - Nom Prénom de l'éditeur</param>
        /// <param name="periode">Période de filtrage pour l'édition</param>
        /// <param name="pathLogo">Chemin du logo de la société</param>
        /// <param name="isFes">Indique si l'édition est à destination de FES</param>
        /// <returns>Tableau de bytes du PDF</returns>
        public static byte[] ToPdf(IEnumerable<RapportLigneEnt> pointages, string editePar, DateTime periode, string pathLogo, bool isFes, string templateFolderPath)
        {
            var excelFormat = new ExcelFormat();
            var stream = new MemoryStream();
            IWorkbook wb = BuildWorkbook(pointages, editePar, periode, pathLogo, isFes, templateFolderPath);

            //// Convert workbook to PDF
            var pdf = excelFormat.PrintExcelToPdf(wb);

            //// Save the document stream
            pdf.Save(stream);

            wb.Close();
            return stream.ToArray();
        }

        #region Private functions : Gestion du document
        /// <summary>
        ///   Rempli l'entête du document avec les informations du personnel
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="personnel">Personnel choisi</param>
        private static void FillSubHeader(IWorksheet ws, PersonnelEnt personnel)
        {
            ws.Range[5, 2].Text = personnel.NomPrenom;
            ws.Range[6, 2].Text = personnel.Societe?.Code;
            ws.Range[5, 9].Text = personnel.Ressource?.Libelle;
            ws.Range[6, 9].Text = personnel.Matricule;
        }

        /// <summary>
        ///   Rempli la partie Prime (un pointage peut avoir plusieurs primes)
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="pointage">Pointage courant</param>
        /// <param name="rowIndex1">Index de la ligne</param>
        /// <returns>Index de la ligne suivante après avoir rempli les Primes</returns>
        private static int FillPrime(IWorksheet ws, RapportLigneEnt pointage, int rowIndex1)
        {
            int rowIndex = rowIndex1;
            if (pointage.ListePrimes.Count == 0)
            {
                rowIndex++;
            }

            foreach (var prime in pointage.ListePrimes.ToList())
            {
                // Si prime Horaire
                if (prime.Prime.PrimeType == Entities.Referential.ListePrimeType.PrimeTypeHoraire &&
                    prime.HeurePrime.HasValue && 
                    prime.HeurePrime.Value > 0)
                {
                    ws.Range[rowIndex, 14].Text = prime.Prime.Code;
                    SetNumber(ws.Range[rowIndex, 15], prime.HeurePrime.Value);
                    AddBorders(ws, rowIndex);
                    rowIndex++;
                }
                //Si prime Journalière
                else if (prime.Prime.PrimeType == Entities.Referential.ListePrimeType.PrimeTypeJournaliere
                        && prime.IsChecked)
                {
                    ws.Range[rowIndex, 14].Text = prime.Prime.Code;
                    SetNumber(ws.Range[rowIndex, 15], PrimeJournaliereQte);
                    AddBorders(ws, rowIndex);
                    rowIndex++;
                }
            }

            return rowIndex == InitRowIndex ? rowIndex + 1 : rowIndex;
        }

        /// <summary>
        ///   Rempli la partie Majoration 
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="pointage">Pointage courant</param>
        /// <param name="rowIndex1">Index de la ligne</param>
        /// <returns>Index de la ligne suivante après avoir rempli les Majorations</returns>
        public static int FillMajorationFES(IWorksheet ws, RapportLigneEnt pointage, int rowIndex1)
        {
            int rowIndex = rowIndex1;
            if (pointage.ListRapportLigneMajorations.Count == 0)
            {
                rowIndex++;
                return rowIndex;
            }
            var majorations = pointage.ListRapportLigneMajorations.GroupBy(i => i.CodeMajoration.Code).Select(l => new { key = l.Key, values = l.ToArray() });
            majorations.ForEach(x =>
            {
                double sum = x.values.Select(value => value.HeureMajoration).Sum();
                ws.Range[rowIndex, 12].Text = x.key;
                ws.Range[rowIndex, 13].NumberFormat = NumberFormat;
                SetNumber(ws.Range[rowIndex, 13], sum);
                AddBorders(ws, rowIndex);
                rowIndex++;
            });

            return rowIndex == InitRowIndex ? rowIndex + 1 : rowIndex;
        }

        /// <summary>
        /// Vérifie l'ordonnencement de l'écriture des Majorations,Primes,Astreintes 
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="pointage">Pointage courant</param>
        /// <param name="rowIndex1">Index de la ligne</param>
        /// <param name="isFes">Indique si l'édition est à destination de FES</param>
        /// <returns>Index de la ligne suivante après avoir rempli les Majorations,les Astreintes et les Primes</returns>
        public static int CheckBeforeWriteInExcelOrPdf(IWorksheet ws, RapportLigneEnt pointage, int rowIndex1, bool isFes)
        {
            int rowIndex = rowIndex1;
            int indexEnd = 0;
            if (isFes)
            {
                if (pointage.ListRapportLigneMajorations.Count == 0 && pointage.ListRapportLigneAstreintes.Count == 0 && pointage.ListePrimes.Count == 0)
                {
                    rowIndex++;
                    return rowIndex;
                }

                if (pointage.ListRapportLigneMajorations.Count > 0)
                {
                    indexEnd = CheckMajorations(ws, pointage, rowIndex1);
                }
                if (pointage.ListePrimes.Count > 0)
                {
                    indexEnd = CheckPrimes(ws, pointage, rowIndex1);
                }
                if (pointage.ListRapportLigneAstreintes.Count > 0)
                {
                    indexEnd = Checkastreinte(ws, pointage, rowIndex1);
                }
            }
            else
            {
                indexEnd = FillMajorationAndPrime(ws, pointage, rowIndex1, rowIndex);
            }

            if (indexEnd == rowIndex)
            {
                indexEnd++;
            }
            return indexEnd;
        }

        private static int FillMajorationAndPrime(IWorksheet ws, RapportLigneEnt pointage, int rowIndex1, int rowIndex)
        {
            if (pointage.HeureMajoration > 0)
            {
                ws.Range[rowIndex, 12].Text = pointage.CodeMajoration?.Code;
                SetNumber(ws.Range[rowIndex, 13], pointage.HeureMajoration);
            }
            return FillPrime(ws, pointage, rowIndex1);
        }

        /// <summary>
        ///  Vérifie l'ordonnencement des majorations
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="pointage">Pointage courant</param>
        /// <param name="rowIndex1">Index de la ligne</param>
        /// <returns>Index de la ligne suivante après avoir rempli les Majorations</returns>
        public static int CheckMajorations(IWorksheet ws, RapportLigneEnt pointage, int rowIndex1)
        {
            int indexEnd = 0;
            var indexprime = FillPrime(ws, pointage, rowIndex1);
            var indexmajo = FillMajorationFES(ws, pointage, rowIndex1);
            var indexastreinte = FillAstreinte(ws, pointage, rowIndex1);
            if (indexmajo >= indexastreinte && indexmajo >= indexprime)
            {
                indexEnd = indexmajo;
            }
            else if (indexastreinte >= indexmajo && indexastreinte >= indexprime)
            {
                indexEnd = indexastreinte;
            }
            else
            {
                indexEnd = indexprime;
            }
            return indexEnd;
        }

        /// <summary>
        /// Vérifie l'ordonnencement des primes
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="pointage">Pointage courant</param>
        /// <param name="rowIndex1">Index de la ligne</param>
        /// <returns>Index de la ligne suivante après avoir rempli les primes</returns>
        public static int CheckPrimes(IWorksheet ws, RapportLigneEnt pointage, int rowIndex1)
        {
            int indexEnd = 0;
            var indexprime = FillPrime(ws, pointage, rowIndex1);
            var indexastreinte = FillAstreinte(ws, pointage, rowIndex1);
            var indexmajo = FillMajorationFES(ws, pointage, rowIndex1);

            if (indexmajo >= indexastreinte && indexmajo >= indexprime)
            {
                indexEnd = indexmajo;
            }
            else if (indexprime >= indexmajo && indexprime >= indexastreinte)
            {
                indexEnd = indexprime;
            }
            else
            {
                indexEnd = indexastreinte;
            }
            return indexEnd;
        }

        /// <summary>
        /// Vérifie l'ordonnencement des astreintes
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="pointage">Pointage courant</param>
        /// <param name="rowIndex1">Index de la ligne</param>
        /// <returns>Index de la ligne suivante après avoir rempli les astreintes</returns>
        public static int Checkastreinte(IWorksheet ws, RapportLigneEnt pointage, int rowIndex1)
        {
            int indexEnd = 0;
            var indexprime = FillPrime(ws, pointage, rowIndex1);
            var indexastreinte = FillAstreinte(ws, pointage, rowIndex1);
            var indexmajo = FillMajorationFES(ws, pointage, rowIndex1);

            if (indexastreinte >= indexmajo && indexastreinte >= indexprime)
            {
                indexEnd = indexastreinte;
            }
            else if (indexprime >= indexmajo)
            {
                indexEnd = indexprime;
            }
            else
            {
                indexEnd = indexmajo;
            }
            return indexEnd;
        }
        /// <summary>
        ///   Rempli la partie Astreinte
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="pointage">Pointage courant</param>
        /// <param name="rowIndex1">Index de la ligne</param>
        /// <returns>Index de la ligne suivante après avoir rempli les astreintes</returns>
        public static int FillAstreinte(IWorksheet ws, RapportLigneEnt pointage, int rowIndex1)
        {
            int rowIndex = rowIndex1;
            if (pointage.ListRapportLigneAstreintes.Count == 0)
            {
                rowIndex++;
                return rowIndex;
            }
            string code = pointage.ListRapportLigneAstreintes.FirstOrDefault(x => x.ListCodePrimeSortiesAstreintes.Count > 0).ListCodePrimeSortiesAstreintes?.FirstOrDefault().CodeAstreinte?.Code;
            double somme = 0;
            foreach (var item in pointage.ListRapportLigneAstreintes)
            {
                somme += (item.DateFinAstreinte - item.DateDebutAstreinte).TotalHours;
            }
            ws.Range[rowIndex, 16].Text = code;
            SetNumber(ws.Range[rowIndex, 17], somme);
            AddBorders(ws, rowIndex);
            rowIndex++;
            return rowIndex == InitRowIndex ? rowIndex + 1 : rowIndex;
        }

        /// <summary>
        ///   Rempli la dernière ligne contenant les totaux des heures
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="rowIndex1">Index de la dernière ligne</param>
        private static void FillTotal(IWorksheet ws, int rowIndex1)
        {
            int lastRow = rowIndex1 - 1;

            ws.Range[rowIndex1, 1].Text = BusinessResources.Global_Total;
            ws.Range[rowIndex1, 1, rowIndex1, 19].CellStyle.Font.Bold = true;
            ws.Range[rowIndex1, 1].CellStyle.Font.Size = 13;

            ws.Range[rowIndex1, 5].FormulaArray = "=SUM(E10:E" + lastRow.ToString() + ")";//Hrs a pied
            ws.Range[rowIndex1, 7].FormulaArray = "=SUM(G10:G" + lastRow.ToString() + ")";//Absence
            ws.Range[rowIndex1, 13].FormulaArray = "=SUM(M10:M" + lastRow.ToString() + ")";//Majorations
            ws.Range[rowIndex1, 15].FormulaArray = "=SUM(O10:O" + lastRow.ToString() + ")";//primes
            ws.Range[rowIndex1, 17].FormulaArray = "=SUM(Q10:Q" + lastRow.ToString() + ")";//Astreintes
            ws.Range[rowIndex1, 18].FormulaArray = "=SUM(E"+ (lastRow +1).ToString() +":M"+ (lastRow + 1).ToString() + ")";
        }

        /// <summary>
        ///   Ajoute des border pour chaque ligne
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="index">Index de la ligne</param>
        private static void AddBorders(IWorksheet ws, int index)
        {
            for (var i = 1; i <= 19; i++)
            {
                ws.Range[index, i].BorderAround();
            }
        }

        private static void SetNumber(IRange range, double hour)
        {
            if (hour > 0)
            {
                range.Number = hour;
            }
        }

        /// <summary>
        ///   Création du document excel
        /// </summary>
        /// <param name="pointages">Liste des pointages personnel</param>
        /// <param name="editeur">Matricule - Nom Prénom de l'éditeur</param>
        /// <param name="periode">Période de filtrage pour l'édition</param>
        /// <param name="pathLogo">Chemin du logo de la société</param>
        /// <param name="isFes">Indique si l'édition est à destination de FES</param>
        /// <returns>Workbook excel</returns>
        private static IWorkbook BuildWorkbook(IEnumerable<RapportLigneEnt> pointages, string editeur, DateTime periode, string pathLogo, bool isFes, string templateFolderPath)
        {
            IWorkbook wb = null;

            try
            {
                PersonnelEnt personnel = pointages.FirstOrDefault()?.Personnel;
                // Groupement par Date de pointage
                var pointagesByDate = pointages.OrderBy(d => d.DatePointage).GroupBy(x => x.DatePointage.Date).Select(x => new { DatePointage = x.Key, Pointages = x.ToList() });
                string pathName = Path.Combine(templateFolderPath, "PointagePersonnel/TemplatePointagePersonnel.xlsx");

                var excelFormat = new ExcelFormat();
                wb = excelFormat.OpenTemplateWorksheet(pathName);
                IWorksheet ws = wb.Worksheets[0];
                ws.EnableSheetCalculations();

                int rowIndex1 = InitRowIndex;

                FillSubHeader(ws, personnel);

                // Boucle sur les dates
                foreach (var date in pointagesByDate.ToList())
                {
                    //// Date
                    ws.Range[rowIndex1, 1].Text = date.DatePointage.ToLocalTime().ToShortDateString();

                    int tmpIndex = rowIndex1;
                    int lastRowOfDay = 0;

                    foreach (var p in date.Pointages)
                    {
                        rowIndex1 = GenerateLine(p, ws, rowIndex1, isFes);
                    }

                    lastRowOfDay = rowIndex1 > InitRowIndex ? rowIndex1 - 1 : rowIndex1;

                    ws.Range[lastRowOfDay, 1, lastRowOfDay, 19].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

                    // Merge column DateRapport for each day
                    ws.Range[tmpIndex, 1, tmpIndex, 1].Merge();
                }

                //// Remplissage de la ligne des totaux
                FillTotal(ws, rowIndex1);
                AddBorders(ws, rowIndex1);
                ws.Range[rowIndex1, 1, rowIndex1, 19].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                ws.Range[rowIndex1, 1, rowIndex1, 19].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;

                // Construction de l'entête
                string dateEdition =  BusinessResources.Pointage_Interimaire_Export_DateEdition + DateTime.Now.ToShortDateString() + BusinessResources.Pointage_Personnel_Hebdomadaire_Export_A + DateTime.Now.ToShortTimeString();
                string editePar =  BusinessResources.Pointage_Interimaire_Export_EditePar + editeur;
                string sousTitre =  periode.ToString("MM/yyyy");
                var buildHeaderModel = new BuildHeaderExcelModel("Pointage Mensuel", sousTitre, dateEdition, editePar, null, pathLogo, new IndexHeaderExcelModel(4, 15, 19, 19));
                excelFormat.BuildHeader(ws, buildHeaderModel);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }

            return wb;
        }

        private static int GenerateLine(RapportLigneEnt p, IWorksheet ws, int rowIndex1, bool isFes)
        {

                double totalHeuresTravaillees = p.HeureNormale + p.HeureMajoration;
                double heureAPied = totalHeuresTravaillees - p.MaterielMarche;
                double totalHrs = isFes ? p.HeureNormale + p.ListRapportLigneMajorations.Sum(x => x.HeureMajoration) : totalHeuresTravaillees;
                //// Matériel
                ws.Range[rowIndex1, 2].Text = p.Materiel?.Societe?.Code;
                ws.Range[rowIndex1, 3].Text = p.Materiel?.Code;
                ws.Range[rowIndex1, 3].CellStyle.WrapText = true;

                ws.Range[rowIndex1, 4].NumberFormat = NumberFormat;
                SetNumber(ws.Range[rowIndex1, 4], p.MaterielMarche);

                //// Heures à pied | Heures hors SEP                        
                SetNumber(ws.Range[rowIndex1, 5], p.Materiel != null ? heureAPied : totalHeuresTravaillees);
                ws.Range[rowIndex1, 5].NumberFormat = NumberFormat;
                ws.Range[rowIndex1, 6].Text = string.Empty; //// heures hosr SEP (laisser vide pour l'instant);

                //// Absence
                SetNumber(ws.Range[rowIndex1, 7], p.HeureAbsence);
                ws.Range[rowIndex1, 7].NumberFormat = NumberFormat;
                ws.Range[rowIndex1, 8].Text = p.CodeAbsence?.Libelle;
                ws.Range[rowIndex1, 8].CellStyle.WrapText = true;
                ws.Range[rowIndex1, 8].AutofitRows();

                //// Déplacement
                ws.Range[rowIndex1, 9].Text = p.CodeDeplacement?.Code;
                ws.Range[rowIndex1, 10].Text = p.CodeZoneDeplacement?.Code;
                ws.Range[rowIndex1, 11].Text = p.DeplacementIV ? BusinessResources.Global_Oui : BusinessResources.Global_Non;

                //// Gestion des Majorations, Prime et Astreintes
                int rowIndex2 = CheckBeforeWriteInExcelOrPdf(ws, p, rowIndex1, isFes);

                ////  Total heures                        
                SetNumber(ws.Range[rowIndex1, 18], totalHrs);

                //// Affaire (CI)
                ws.Range[rowIndex1, 19].Text = p.Ci.CodeLibelle;
                ws.Range[rowIndex1, 19].CellStyle.WrapText = true;
                ws.Range[rowIndex1, 19].AutofitRows();

                AddBorders(ws, rowIndex1);

                return rowIndex2 != 0 ? rowIndex2 : rowIndex1 + 1;
        }
        #endregion
    }
}
