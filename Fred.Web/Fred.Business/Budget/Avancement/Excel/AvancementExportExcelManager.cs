using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fred.Business.Budget.Helpers;
using Fred.Business.Images;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Avancement;
using Fred.Framework.Linq;
using Fred.Framework.Reporting;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Budget;
using Fred.Web.Shared.Models.Budget.Avancement.Excel;
using Syncfusion.XlsIO;
using static Fred.Business.Budget.Avancement.Excel.AvancementExcelDescriber;

namespace Fred.Business.Budget.Avancement.Excel
{
    /// <summary>
    /// Implementation de l'interface IAvancementExportExcelManager
    /// </summary>
    public class AvancementExportExcelManager : Manager<AvancementEnt, IAvancementRepository>, IAvancementExportExcelManager
    {
        private const string ExcelTemplate = "Templates/Avancement/TemplateAvancement.xlsx";
        private string moisPrecedentFullMonthYear;
        private string moisCourantFullMonthYear;
        private string codeEtLibelleCi;

        private readonly IUtilisateurManager utilisateurManager;

        private IEnumerable<int> t1RowsIndex;
        private IEnumerable<int> t2RowsIndex;
        private IEnumerable<int> t3RowsIndex;
        private IEnumerable<int> t4RowsIndex;

        private int excelDataListeEndLine;
        private IWorksheet worksheet;

        private readonly IPersonnelRepository personnelRepository;
        private readonly ICIRepository ciRepository;
        private readonly IImageManager imageManager;

        public AvancementExportExcelManager(
            IUnitOfWork uow,
            IAvancementRepository avancementRepository,
            IUtilisateurManager utilisateurManager,
            IImageManager imageManager,
            IPersonnelRepository personnelRepository,
            ICIRepository ciRepository)
            : base(uow, avancementRepository)
        {
            this.utilisateurManager = utilisateurManager;
            this.imageManager = imageManager;
            this.personnelRepository = personnelRepository;
            this.ciRepository = ciRepository;
        }

        private enum Mois
        {
            MoisCourant,
            MoisPrecedent
        }

        /// <inheritdoc/>
        public byte[] GetExportExcel(AvancementExcelLoadModel model)
        {
            var periodePrecedente = PeriodeHelper.GetPreviousPeriod(model.Periode).Value;
            moisPrecedentFullMonthYear = PeriodeHelper.GetLiteralPeriode(periodePrecedente);
            moisCourantFullMonthYear = PeriodeHelper.GetLiteralPeriode(model.Periode);

            codeEtLibelleCi = ciRepository.SelectOneColumn(ci => ci.Code + " " + ci.Libelle, ci => ci.CiId == model.CiId).Single();
            //Filter suivant les axes
            model.Valeurs = model.Valeurs.Where(x => model.AnalyticalAxis.Select(t => t.Name).Contains(x.NiveauTache)).ToArray();
            t1RowsIndex = GetAllLineForTacheCode(model.Valeurs, NiveauxTaches.T1.ToFriendlyName());
            t2RowsIndex = GetAllLineForTacheCode(model.Valeurs, NiveauxTaches.T2.ToFriendlyName());
            t3RowsIndex = GetAllLineForTacheCode(model.Valeurs, NiveauxTaches.T3.ToFriendlyName());
            t4RowsIndex = GetAllLineForTacheCode(model.Valeurs, NiveauxTaches.T4.ToFriendlyName());

            excelDataListeEndLine = ExcelDataListeStartLine + model.Valeurs.Length - 1;

            using (var excelFormat = new ExcelFormat())
            {
                byte[] bytes = null;
                if (!model.IsPdfConverted)
                {
                    bytes = excelFormat.GenerateExcel(AppDomain.CurrentDomain.BaseDirectory + ExcelTemplate, model.Valeurs, CustomTransformation);
                }
                else
                {
                    bytes = excelFormat.GeneratePdfFromExcel(AppDomain.CurrentDomain.BaseDirectory + ExcelTemplate, model.Valeurs, CustomTransformationPdf);
                }

                var memoryStream = new MemoryStream(bytes);
                return memoryStream.ToArray();
            }
        }

        private IEnumerable<int> GetAllLineForTacheCode(IEnumerable<AvancementExcelModelValeurs> valeurs, string niveauTache)
        {
            return valeurs
                .Select((valeur, index) => new { valeur.NiveauTache, index })
                .Where(anon => anon.NiveauTache == niveauTache)
                .Select(anon => anon.index + ExcelDataListeStartLine);
        }

        /// <summary>
        /// Custom transfomation pour la génération pdf
        /// </summary>
        /// <param name="workbook">workbook a transformer</param>
        private void CustomTransformationPdf(IWorkbook workbook)
        {
            CustomTransformation(workbook);
            // Augmente la taille de la police et centre verticalement pour toutes les cells du détail
            var sheet = workbook.Worksheets[0];
            var rowIndex = 0;
            foreach (var row in sheet.Rows)
            {
                if (rowIndex > 0)
                {
                    foreach (var cell in row.Cells)
                    {
                        cell.CellStyle.VerticalAlignment = ExcelVAlign.VAlignBottom;
                        cell.CellStyle.Font.Size += 2;
                    }
                }
                row.RowHeight += 8;
                rowIndex++;
            }
        }

        private void CustomTransformation(IWorkbook workbook)
        {
            using (var excelFormat = new ExcelFormat())
            {

                worksheet = workbook.Worksheets[0];
                worksheet.DisableSheetCalculations();
                SetExcelHeader(worksheet);
                SetFormulaAfterPopulate(worksheet);
                SetTotal(worksheet);
                SetStyle(worksheet);

                var utilisateurId = utilisateurManager.GetContextUtilisateurId();
                var editeur = personnelRepository.GetPersonnelPourExportExcelHeader(utilisateurId);
                excelFormat.AddLogoSociete(editeur.SocieteId != null ? AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(editeur.SocieteId.Value).Path : null, worksheet);

            }
        }

        private void SetTotal(IWorksheet sheet)
        {
            var addAllT1 = string.Join("+", t1RowsIndex.Select(value => string.Concat("{0}", value)).ToArray());
            var addAllT4 = string.Join("+", t4RowsIndex.Select(value => string.Concat("{0}", value)).ToArray());

            //Formule pour les valeurs totales qui se calculent en additionnant les T1
            string montantFormula = InjectFormulaInTotalRowForColumn(sheet, MontantColumn, addAllT1);
            InjectFormulaInTotalRowForColumn(sheet, DadMoisPrecedentColumn, addAllT1);
            InjectFormulaInTotalRowForColumn(sheet, DadMoisCourantColumn, addAllT1);
            InjectFormulaInTotalRowForColumn(sheet, EcartDadColumn, addAllT1);
            InjectFormulaInTotalRowForColumnAvancement(sheet);
            InjectPercentUnitOnTotalRow(sheet);

            //Formule ajoutant tous les T4
            string quantiteFormula = InjectFormulaInTotalRowForColumn(sheet, QuantiteColumn, addAllT4);
            InjectFormulaInTotalRowForColumn(sheet, PuColumn, "(" + montantFormula + ")/(" + quantiteFormula + ")");
        }

        /// <summary>
        /// Applique la formule donnée en paramètre sur la cellule située sur la ligne des totaux à la colonne donnée
        /// </summary>
        /// <param name="sheet">Feuille excel à modifier</param>
        /// <param name="colonne">Colonne a laquelle la formule sera insérée</param>
        /// <param name="formulas">Formule à injecter</param>
        private string InjectFormulaInTotalRowForColumn(IWorksheet sheet, char colonne, string formulas)
        {
            //La formule pour les totaux mesure le total des T1 donc d'abord il faut savoir si la ligne correspondant à i 
            //est celle d'un T1
            var totauxStartLine = GetTotalRowIndex();

            var rangeTotalForColonne = sheet.Range[$"{colonne}{totauxStartLine}"];

            var formattedFormula = string.Format(formulas, colonne);

            //On initialise une formule qui sera : = [La valeur à la ligne courante]
            rangeTotalForColonne.Formula = formattedFormula;
            return formattedFormula;
        }

        /// <summary>
        /// Applique la formule donnée en paramètre sur la cellule située sur la ligne des totaux à la colonne donnée
        /// </summary>
        /// <param name="sheet">Feuille excel à modifier</param>
        private void InjectFormulaInTotalRowForColumnAvancement(IWorksheet sheet)
        {
            var totauxStartLine = GetTotalRowIndex();

            sheet.Range[$"{AvancementMoisPrecedentColumn}{totauxStartLine}"].Formula =
                string.Concat(DadMoisPrecedentColumn, totauxStartLine, "/", MontantColumn, totauxStartLine, "*100");
            sheet.Range[$"{AvancementMoisCourantColumn}{totauxStartLine}"].Formula =
                string.Concat(DadMoisCourantColumn, totauxStartLine, "/", MontantColumn, totauxStartLine, "*100");
            sheet.Range[$"{EcartAvancementColumn}{totauxStartLine}"].Formula =
                string.Concat(AvancementMoisCourantColumn, totauxStartLine, "-", AvancementMoisPrecedentColumn, totauxStartLine);
        }

        private void InjectPercentUnitOnTotalRow(IWorksheet sheet)
        {
            var totauxStartLine = GetTotalRowIndex();

            sheet.Range[$"{AvancementMoisPrecedentUniteColumn}{totauxStartLine}"].Value = "%";
            sheet.Range[$"{AvancementMoisCourantUniteColumn}{totauxStartLine}"].Value = "%";
            sheet.Range[$"{EcartAvancementUniteColumn}{totauxStartLine}"].Value = "%";
        }

        private void SetStyle(IWorksheet sheet)
        {
            var allRows = t1RowsIndex.Concat(t2RowsIndex.Concat(t3RowsIndex.Concat(t4RowsIndex)));
            var excelFormatter = new AvancementExcelCellFormatter(sheet, allRows, GetTotalRowIndex());

            excelFormatter.SetStyleForColumn(CodeColumn);
            excelFormatter.SetStyleForColumn(LibelleColumn);
            excelFormatter.SetStyleForColumn(CommentaireColumn);
            excelFormatter.SetStyleForColumn(UniteColumn);
            excelFormatter.SetStyleForColumn(QuantiteColumn);
            excelFormatter.SetStyleForColumn(PuColumn);
            excelFormatter.SetStyleForColumn(MontantColumn);

            excelFormatter.SetStyleForColumn(AvancementMoisPrecedentColumn);
            excelFormatter.SetStyleForColumn(AvancementMoisPrecedentUniteColumn);
            excelFormatter.SetStyleForColumn(AvancementMoisCourantColumn);
            excelFormatter.SetStyleForColumn(AvancementMoisCourantUniteColumn);
            excelFormatter.SetStyleForColumn(EcartAvancementColumn);
            excelFormatter.SetStyleForColumn(EcartAvancementUniteColumn);
            excelFormatter.SetStyleForColumn(DadMoisPrecedentColumn);
            excelFormatter.SetStyleForColumn(DadMoisCourantColumn);
            excelFormatter.SetStyleForColumn(EcartDadColumn);

            excelFormatter.SetTotalStyleForColumn(QuantiteColumn);
            excelFormatter.SetTotalStyleForColumn(PuColumn);
            excelFormatter.SetTotalStyleForColumn(MontantColumn);
            excelFormatter.SetTotalStyleForColumn(AvancementMoisPrecedentColumn, false);
            excelFormatter.SetTotalStyleForColumn(AvancementMoisPrecedentUniteColumn, false);
            excelFormatter.SetTotalStyleForColumn(AvancementMoisCourantColumn, false);
            excelFormatter.SetTotalStyleForColumn(AvancementMoisCourantUniteColumn, false);
            excelFormatter.SetTotalStyleForColumn(EcartAvancementColumn, false);
            excelFormatter.SetTotalStyleForColumn(EcartAvancementUniteColumn, false);
            excelFormatter.SetTotalStyleForColumn(DadMoisPrecedentColumn);
            excelFormatter.SetTotalStyleForColumn(DadMoisCourantColumn);
            excelFormatter.SetTotalStyleForColumn(EcartDadColumn);

            excelFormatter.SetBordersForAvancementCells();
        }

        private void SetExcelHeader(IWorksheet sheet)
        {
            string formattedDateEdition = string.Format(DateTime.Now.ToString(), DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());

            sheet.Range[CelluleDateEdition].Text = FeatureExportExcel.DateEdition + formattedDateEdition + Environment.NewLine + FeatureExportExcel.EditePar + utilisateurManager.GetContextUtilisateur().PrenomNom;

            sheet.Range[CelluleLibelleCi].Text = codeEtLibelleCi;

            sheet.Range[CelluleMoisCourantAvancement].Text = moisCourantFullMonthYear;
            sheet.Range[CelluleMoisPrecedentAvancement].Text = moisPrecedentFullMonthYear;
            sheet.Range[CelluleMoisCourantDad].Text = moisCourantFullMonthYear;
            sheet.Range[CelluleMoisPrecedentDad].Text = moisPrecedentFullMonthYear;
        }

        private void SetFormulaAfterPopulate(IWorksheet sheet)
        {
            sheet.DisableSheetCalculations();

            //Formule Ecart Mois Courant
            sheet.Range[ComputeRange(EcartAvancementColumn)].Cells.ForEach(cells => cells.Formula = $"={AvancementMoisCourantColumn}{cells.Row} - {AvancementMoisPrecedentColumn}{cells.Row}");
            sheet.Range[ComputeRange(EcartDadColumn)].Cells.ForEach(cells => cells.Formula = $"={DadMoisCourantColumn}{cells.Row} - {DadMoisPrecedentColumn}{cells.Row}");

            //Formules Ecart Dad pour les T1-T2-T3
            SetDadFormulaForT1T2T3(sheet, t1RowsIndex);
            SetDadFormulaForT1T2T3(sheet, t2RowsIndex);
            SetDadFormulaForT1T2T3(sheet, t3RowsIndex);

            //Formules Ecart Dad pour les T4
            foreach (var index in t4RowsIndex)
            {
                SetDadFormulaForT4(sheet, index, Mois.MoisCourant);
                SetDadFormulaForT4(sheet, index, Mois.MoisPrecedent);
            }
        }

        private void SetDadFormulaForT4(IWorksheet sheet, int index, Mois mois)
        {
            char columnDad = DadMoisCourantColumn;
            char columnValeurAvancement = AvancementMoisCourantColumn;
            char columnValeurAvancementUnite = AvancementMoisCourantUniteColumn;

            if (mois == Mois.MoisPrecedent)
            {
                columnDad = DadMoisPrecedentColumn;
                columnValeurAvancement = AvancementMoisPrecedentColumn;
                columnValeurAvancementUnite = AvancementMoisPrecedentUniteColumn;
            }

            var typeAvancement = sheet[$"{columnValeurAvancementUnite}{index}"].Text;
            if (typeAvancement == "%")
            {
                sheet[$"{columnDad}{index}"].Formula = $"{columnValeurAvancement}{index} / 100 * {MontantColumn}{index}";
            }
            else
            {
                sheet[$"{columnDad}{index}"].Formula = $"IF({QuantiteColumn}{index} <> 0; {columnValeurAvancement}{index}/{QuantiteColumn}{index} * {MontantColumn}{index}; 0)";
            }
        }

        private void SetDadFormulaForT1T2T3(IWorksheet sheet, IEnumerable<int> tacheIndex)
        {
            foreach (var index in tacheIndex)
            {
                //L'avancement sur les T1-T2-T3 est toujours exprimé en pourcentage, il faut donc diviser la valeur par 100 pour avoir un calcul correct
                sheet[$"{DadMoisPrecedentColumn}{index}"].Formula = $"{AvancementMoisPrecedentColumn}{index} / 100 * {MontantColumn}{index}";
                sheet[$"{DadMoisCourantColumn}{index}"].Formula = $"{AvancementMoisCourantColumn}{index} / 100 * {MontantColumn}{index}";
            }
        }

        private string ComputeRange(char column)
        {
            return $"{column}{ExcelDataListeStartLine}:{column}{excelDataListeEndLine}";
        }

        private int GetTotalRowIndex()
        {
            return excelDataListeEndLine + LigneTotauxStartAfterEndLine;
        }
    }
}
