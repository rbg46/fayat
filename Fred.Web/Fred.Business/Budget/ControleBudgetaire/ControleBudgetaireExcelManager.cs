using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.BudgetManager.Services;
using Fred.Business.Budget.ControleBudgetaire.Excel;
using Fred.Business.Budget.ControleBudgetaire.Models;
using Fred.Business.Budget.Helpers;
using Fred.Business.Budget.Helpers.Extensions;
using Fred.Business.Depense.Services;
using Fred.Business.Images;
using Fred.Business.Personnel;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Referential.Common;
using Fred.Entities.Budget;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Budget.ControleBudgetaire;
using Syncfusion.XlsIO;

namespace Fred.Business.Budget.ControleBudgetaire
{
    public class ControleBudgetaireExcelManager : Manager<ControleBudgetaireEnt, IControleBudgetaireRepository>, IControleBudgetaireExcelManager
    {
        #region --- Fields ---

        private static readonly string ExcelTemplate = "Templates/ControleBudgetaire/TemplateControleBudgetaire.xlsx";

        private static readonly char LibelleColumn = 'B';
        private static readonly char CodeColumn = 'A';

        private static readonly char MontantBudgetColumn = 'D';

        private static readonly char DadMoisCourantColumn = 'G';
        private static readonly char AvancementMoisCourantColumn = 'F';
        private static readonly char DepensesMoisCourantColumn = 'H';
        private static readonly char EcartMoisCourantColumn = 'I';

        private static readonly char DadCumuleColumn = 'L';
        private static readonly char AvancementCumuleColumn = 'K';
        private static readonly char DepenseCumuleeColumn = 'M';
        private static readonly char EcartCumuleColumn = 'N';

        private static readonly char RadColumn = 'Q';
        private static readonly char PourcentageRadColumn = 'P';
        private static readonly char AjustementColumn = 'S';
        private static readonly char PfaColumn = 'U';

        private IWorksheet worksheet;

        private readonly ICollection<int> topLevelLines = new List<int>();

        /// <summary>
        /// Liste des modeles d'exports (partagés avec la méthode de mise en forme)
        /// </summary>
        private List<ControleBudgetaireExportModel> exportCIModels = new List<ControleBudgetaireExportModel>();

        /// <summary>
        /// La ligne (excel) à laquelle démarre les données provenant de la liste
        /// </summary>
        private readonly int excelDataListeStartLine = 10;

        /// <summary>
        /// La ligne (excel) à laquelle démarre le tableau 
        /// </summary>
        private readonly int excelDataListeStartLineForStyle = 8;

        /// <summary>
        /// La ligne (excel) à laquelle se terminent les données provenant de la liste.
        /// C'est à dire que c'est la dernière ligne contenant des données
        /// </summary>
        private int excelDataListeEndLine = 0;

        /// <summary>
        /// Cela veut dire qu'il y a aura X lignes d'écart entre la dernière lignes des données et la ligne des totaux
        /// </summary>
        private readonly int ligneTotauxStartAfterEndLine = 1;

        /// <summary>
        /// Variable tampon utilisée par la fonction de création personnalisée de l'excel
        /// </summary>
        private int periode;

        #endregion

        private readonly IPersonnelRepository personnelRepository;
        private readonly IImageManager imageManager;
        private readonly IPersonnelManager personnelManager;
        private readonly ITacheSearchHelper taskSearchHelper;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IBudgetMainManager budgetMainManager;
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly IAvancementManager avancementManager;
        private readonly IDepenseServiceMediator depenseServiceMediator;
        private readonly IControleBudgetaireManager controleBudgetaireManager;
        private readonly ICIRepository ciRepository;
        private readonly IGeneralExpenseService generalExpenseService;

        public ControleBudgetaireExcelManager(
            IUnitOfWork uow,
            IControleBudgetaireRepository controleBudgetaireRepository,
            IPersonnelManager personnelManager,
            IImageManager imageManager,
            IPersonnelRepository personnelRepository,
            ICIRepository ciRepository,
            ITacheSearchHelper taskSearchHelper,
            IUtilisateurManager utilisateurManager,
            IBudgetMainManager budgetMainManager,
            IBudgetT4Manager budgetT4Manager,
            IAvancementManager avancementManager,
            IDepenseServiceMediator depenseServiceMediator,
            IControleBudgetaireManager controleBudgetaireManager,
            IGeneralExpenseService generalExpenseService) : base(uow, controleBudgetaireRepository)
        {
            this.ciRepository = ciRepository;
            this.personnelRepository = personnelRepository;
            this.personnelManager = personnelManager;
            this.imageManager = imageManager;
            this.taskSearchHelper = taskSearchHelper;
            this.utilisateurManager = utilisateurManager;
            this.budgetMainManager = budgetMainManager;
            this.budgetT4Manager = budgetT4Manager;
            this.avancementManager = avancementManager;
            this.depenseServiceMediator = depenseServiceMediator;
            this.controleBudgetaireManager = controleBudgetaireManager;
            this.generalExpenseService = generalExpenseService;
        }

        /// <summary>
        /// Genère un modèle de données représenté par un tableau d'octets utilisable pour réaliser l'export excel 
        /// </summary>
        /// <param name="excelLoadModel">Données du controle budgétaire à exporter</param>
        /// <returns>Un tableau d'octets contenant les données</returns>
        public async Task<byte[]> GetExportExcelAsync(ControleBudgetaireExcelLoadModel excelLoadModel)
        {
            exportCIModels = new List<ControleBudgetaireExportModel>();
            periode = excelLoadModel.Periode;
            var datePeriode = PeriodeHelper.ToFirstDayOfMonthDateTime(excelLoadModel.Periode);
            var datePeriodeString = datePeriode.HasValue ? datePeriode.Value.ToString("dd/MM/yyyy") : string.Empty;

            // chargement des données pour chaque CI 
            foreach (var ciId in excelLoadModel.CiIdList)
            {
                // informations relatives au budget
                var budget = controleBudgetaireManager.GetBudgetForCiAndPeriode(ciId, periode);
                if (budget == null)
                {
                    continue;
                }
                var avancementRecette = budgetMainManager.GetAvancementRecette(ciId, periode);
                var codeLibelleCI = ciRepository.SelectOneColumn(ci => ci.Code + "-" + ci.Libelle, ci => ci.CiId == ciId).Single();

                var ciLoadModel = new ControleBudgetaireExcelLoadModel
                {
                    IsPdfConverted = excelLoadModel.IsPdfConverted,
                    BudgetId = budget.BudgetId,
                    Periode = excelLoadModel.Periode,
                    AxePrincipal = excelLoadModel.AxePrincipal,
                    AxeAffichees = excelLoadModel.AxeAffichees,
                    Tree = excelLoadModel.Tree
                };

                exportCIModels.Add(new ControleBudgetaireExportModel
                {
                    CiId = ciId,
                    CodeLibelleCI = codeLibelleCI,
                    TitreBudget = $"Indice {budget.Version} du {datePeriodeString} ",
                    TotalRecette = avancementRecette.BudgetRecette?.TotalRecette,
                    TotalAvancementFacture = avancementRecette.TotalAvancementFacture,
                    TotalAvancementFacturePeriode = avancementRecette.TotalAvancementFacturePeriode,
                    TotalPFA = avancementRecette.TotalPFA,
                    Valeurs = await GenerateValeursAsync(ciId, ciLoadModel).ConfigureAwait(false)
                });
            }

            List<GeneralExpense> generalExpenses = generalExpenseService.GetGeneralExpenses(excelLoadModel.CiIdList, periode);

            GeneralExpense totalGeneralExpense = generalExpenseService.GetTotalGeneralExpenses(generalExpenses);

            string pourcentage = (totalGeneralExpense.Pourcentage * 100).ToString("F", CultureInfo.InvariantCulture);

            // aggregation des données
            var aggregatedExportModel = new ControleBudgetaireExportModel
            {
                TitreGlobal = exportCIModels.Count > 1 ? "Contrôle budgétaire" : exportCIModels.First().CodeLibelleCI,
                TitreBudget = exportCIModels.Count > 1 ? string.Empty : exportCIModels.FirstOrDefault().TitreBudget,
                TitreTauxFraisGeneraux = $"Frais généraux ({pourcentage}%) ",
                TotalRecette = exportCIModels.Sum(x => x.TotalRecette),
                TotalAvancementFacture = exportCIModels.Sum(x => x.TotalAvancementFacture),
                TotalAvancementFacturePeriode = exportCIModels.Sum(x => x.TotalAvancementFacturePeriode),
                TotalPFA = exportCIModels.Sum(x => x.TotalPFA),
                FraisGenerauxBudget = totalGeneralExpense.Budget,
                FraisGenerauxRecette = totalGeneralExpense.Recette,
                FraisGenerauxRecetteCumul = totalGeneralExpense.RecetteCumul,
                FraisGenerauxPfa = totalGeneralExpense.Pfa,
                Valeurs = exportCIModels.SelectMany(x => x.Valeurs).ToList()
            };

            excelDataListeEndLine = excelDataListeStartLine + aggregatedExportModel.Valeurs.Count - 1;

            // Recherche de l'axe de plus haut niveau
            var topLevelTypeAxe = aggregatedExportModel.Valeurs.FirstOrDefault()?.TypeAxe;
            for (int i = 0; i < aggregatedExportModel.Valeurs.Count; i++)
            {
                if (aggregatedExportModel.Valeurs[i].TypeAxe == topLevelTypeAxe)
                {
                    // Stocke la liste des lignes de plus haut niveau pour calculer les totaux
                    topLevelLines.Add(excelDataListeStartLine + i);
                }
            }

            using (var excelFormat = new ExcelFormat())
            {
                byte[] bytes = null;

                if (!excelLoadModel.IsPdfConverted)
                {
                    bytes = excelFormat.GenerateExcel(AppDomain.CurrentDomain.BaseDirectory + ExcelTemplate, new List<ControleBudgetaireExportModel> { aggregatedExportModel }, CustomTransformation);
                }
                else
                {
                    bytes = excelFormat.GeneratePdfFromExcel(AppDomain.CurrentDomain.BaseDirectory + ExcelTemplate, new List<ControleBudgetaireExportModel> { aggregatedExportModel }, CustomTransformationPdf);
                }

                MemoryStream memoryStream = new MemoryStream(bytes);
                return memoryStream.ToArray();
            }
        }

        private async Task<List<ControleBudgetaireExportModelValeurs>> GenerateValeursAsync(int ciId, ControleBudgetaireExcelLoadModel excelLoadModel)
        {
            ExcelDataSourceBuilder excelDataSourceBuilder = new ExcelDataSourceBuilder(avancementManager, depenseServiceMediator, budgetT4Manager, controleBudgetaireManager);

            var sources = await excelDataSourceBuilder.BuildDataSourceAsync(ciId, excelLoadModel.BudgetId, periode).ConfigureAwait(false);

            var axe = new AxeTreeBuilder(sources, excelLoadModel.AxePrincipal, taskSearchHelper, excelLoadModel.AxeAffichees);
            var tacheRessourceTree = axe.GenerateTree();

            tacheRessourceTree = RenameCodeForExcel(tacheRessourceTree);

            List<AxeTreeLightModel> axeTreeLightModel = excelLoadModel.Tree.ToList();
            if (excelLoadModel.Tree.All(t => t.SousAxe == null))
            {
                axeTreeLightModel = new List<AxeTreeLightModel>();
                tacheRessourceTree.ToList().ForEach(t => axeTreeLightModel.Add(new AxeTreeLightModel { AxeType = t.AxeType, SousAxe = null }));
            }

            //Cette fonction ne peut fonctionner que si L'arbre passé depuis le front suit la même structure que l'arbre qui vient d'être généré
            //Cela sera le cas si l'abre passé depuis le front suit l'arborescence décrite par l'axe principal et les axes affichés passés dans le model
            CutBranches(tacheRessourceTree, axeTreeLightModel);

            var flattenedTree = FlattenAxe(tacheRessourceTree);
            return flattenedTree;
        }

        private IEnumerable<AxeTreeModel> RenameCodeForExcel(IEnumerable<AxeTreeModel> tree)
        {
            foreach (var t in tree)
            {
                switch (t.AxeType)
                {
                    case AxeTypes.T1:
                        t.Code = AxeTypes.T1.ToFriendlyName() + "-" + t.Code;
                        break;
                    case AxeTypes.T2:
                        t.Code = AxeTypes.T2.ToFriendlyName() + "-" + t.Code;
                        break;
                    case AxeTypes.T3:
                        t.Code = AxeTypes.T3.ToFriendlyName() + "-" + t.Code;
                        break;
                    case AxeTypes.Chapitre:
                        t.Code = AxeTypes.Chapitre.ToFriendlyName() + "-" + t.Code;
                        break;
                    case AxeTypes.SousChapitre:
                        t.Code = AxeTypes.SousChapitre.ToFriendlyName() + "-" + t.Code;
                        break;
                    case AxeTypes.Ressource:
                        t.Code = AxeTypes.Ressource.ToFriendlyName() + "-" + t.Code;
                        break;
                }

                if (t.SousAxe != null)
                {
                    t.SousAxe = RenameCodeForExcel(t.SousAxe).ToList();
                }
            }

            return tree;
        }


        private void CutBranches(IEnumerable<AxeTreeModel> treeToExport, IEnumerable<AxeTreeLightModel> treeFromClient)
        {
            for (int i = 0; i < treeFromClient.Count(); i++)
            {
                if (treeFromClient.ElementAt(i).SousAxe == null && treeToExport.ElementAtOrDefault(i)?.SousAxe != null)
                {
                    treeToExport.ElementAt(i).SousAxe = null;

                }
                else if (treeFromClient.ElementAt(i).SousAxe != null && treeToExport.ElementAtOrDefault(i)?.SousAxe != null)
                {
                    CutBranches(treeToExport.ElementAt(i).SousAxe, treeFromClient.ElementAt(i).SousAxe);
                }

                //Les deux valeurs peuvent être nulles, si par exemple on est sur une feuille de l'arbre
            }
        }

        private List<ControleBudgetaireExportModelValeurs> FlattenAxe(IEnumerable<AxeTreeModel> axe)
        {
            var result = new List<ControleBudgetaireExportModelValeurs>();


            axe.ToList().ForEach(item => result.AddRange(FlattenAxe(item)));
            return result.ToList();
        }

        private List<ControleBudgetaireExportModelValeurs> FlattenAxe(AxeTreeModel axe)
        {
            var result = new List<ControleBudgetaireExportModelValeurs>();
            result.Add(CreateModelExcelForAxe(axe));

            if (axe.SousAxe != null)
            {
                axe.SousAxe.ToList().ForEach(sousAxe => result.AddRange(FlattenAxe(sousAxe)));
            }
            return result.ToList();
        }

        private ControleBudgetaireExportModelValeurs CreateModelExcelForAxe(AxeTreeModel axe)
        {
            //L'excel s'attend a une valeur entre 0 et 1 alors que l'abre contiendra des valeurs entre 0 et 100
            var avancementMoisCourant = (decimal?)axe.Valeurs["PourcentageAvancementMoisCourant"] ?? 0m; // JSM
            avancementMoisCourant = avancementMoisCourant / 100;

            var avancementCumule = (decimal?)axe.Valeurs["PourcentageAvancement"] ?? 0m;
            avancementCumule = avancementCumule / 100;

            var exportModel = new ControleBudgetaireExportModelValeurs()
            {
                Code = axe.Code,
                LibelleTacheOuRessource = axe.Libelle,

                MontantBudget = (decimal)axe.Valeurs["MontantBudget"],

                AvancementMoisCourant = avancementMoisCourant,
                DadMoisCourant = (decimal)axe.Valeurs["MontantDadMoisCourant"],
                DepensesMoisCourant = (decimal)axe.Valeurs["MontantDepenseMoisCourant"],

                AvancementCumule = avancementCumule,
                DadCumule = (decimal)axe.Valeurs["MontantDad"],
                DepensesCumulees = (decimal)axe.Valeurs["MontantDepense"],

                PourcentageResteADepenserTheorique = 0m, // calcul RAD/Budget

                Ajustement = (decimal)axe.Valeurs["MontantAjustement"],

                PFA = (decimal)axe.Valeurs["PfaMoisCourant"],
                EcartM1 = (decimal)axe.Valeurs["EcartMoisPrecedent"],

                TypeAxe = axe.AxeType.ToString()
            };
            exportModel.ResteADepenserTheorique = exportModel.MontantBudget - exportModel.DadCumule;
            exportModel.PourcentageResteADepenserTheorique = exportModel.MontantBudget > 0 ? exportModel.ResteADepenserTheorique / exportModel.MontantBudget : 0;

            return exportModel;
        }

        /// <summary>
        /// Custom Transformation pour l'export Pdf
        /// </summary>
        /// <param name="workbook">workbook a transformer</param>
        private void CustomTransformationPdf(IWorkbook workbook)
        {
            CustomTransformation(workbook);
            // Augmente la taille de la police et centre verticalement pour toutes les cells du détail
            var sheet = workbook.Worksheets[0];
            sheet.EnableSheetCalculations();
            var rowIndex = 0;
            foreach (var row in sheet.Rows)
            {
                if (rowIndex > 0)
                {
                    foreach (var cell in row.Cells)
                    {
                        cell.CellStyle.Font.Size = cell.CellStyle.Font.Size + 2;
                        cell.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        // Les formules ne fonctionnent pas dans la conversion pdf, remplacement des valeurs en erreur.
                        if (cell.DisplayText == "#DIV/0!")
                        {
                            cell.Value = "0";
                        }
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

                SetStyle(workbook.Worksheets[0]);
                SetFormulaAfterPopulate(worksheet);
                SetTotal(worksheet);
                SetExcelFooter(worksheet);
                SetTotalCellsStyle(worksheet);
                SetCIHeaders(worksheet);
                SetHeader(excelFormat);
            }
        }

        /// <summary>
        /// Mise à jour du header
        /// </summary>
        /// <param name="excelFormat">excel format</param>
        public void SetHeader(ExcelFormat excelFormat)
        {
            var now = DateTime.Now;

            var utilisateurId = utilisateurManager.GetContextUtilisateurId();
            var editeur = personnelRepository.GetPersonnelPourExportExcelHeader(utilisateurId);
            var buildHeaderModel = new BuildHeaderExcelModel(
                null,
                null,
                BusinessResources.Export_Header_DateEdition + now.ToShortDateString() + " " + now.ToShortTimeString(),
                BusinessResources.Export_Header_EditePar + personnelManager.GetPersonnelMatriculeNomPrenom(editeur),
                null,
                editeur.SocieteId != null ? AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(editeur.SocieteId.Value).Path : null,
                new IndexHeaderExcelModel(3, 12, 18, 21));
            excelFormat.BuildHeader(worksheet, buildHeaderModel, false);
        }


        private string ComputeRange(char column)
        {
            return $"{column}{excelDataListeStartLine}:{column}{excelDataListeEndLine}";
        }

        private string ComputeRangeForStyle(char column)
        {
            return $"{column}{excelDataListeStartLineForStyle}:{column}{excelDataListeEndLine}";
        }

        private string GetTotalRangeForColumn(char column)
        {
            var totalRowStartAt = GetIndexLigneTotaux();
            return $"{column}{totalRowStartAt}";
        }

        private int GetIndexLigneTotaux()
        {
            return excelDataListeEndLine + ligneTotauxStartAfterEndLine;
        }

        /// <summary>
        /// Applique le style au classeur Excel
        /// </summary>
        /// <param name="sheet">Classeur Excel</param>
        private void SetStyle(IWorksheet sheet)
        {
            //Style colonne Code et Libellé
            ApplyCustomCellTemplateForTree(sheet, ComputeRangeForStyle(CodeColumn));

            //Style colonne Budget 
            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(MontantBudgetColumn), ExcelCellFormatter.ColorColonneBudget);

            //Style colonnes DAD
            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(DadMoisCourantColumn), ExcelCellFormatter.ColorColonneDadAvancement);

            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(DadCumuleColumn), ExcelCellFormatter.ColorColonneDadAvancement);

            //Style colonnes Dépense
            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(DepensesMoisCourantColumn), ExcelCellFormatter.ColorColonneDepense);

            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(DepenseCumuleeColumn), ExcelCellFormatter.ColorColonneDepense);

            //Style colonnes Ecart
            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(EcartMoisCourantColumn), ExcelCellFormatter.ColorEcartAjustement);

            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(EcartCumuleColumn), ExcelCellFormatter.ColorEcartAjustement);

            //Style colonnes Rad, Pourcentages Rad
            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(PourcentageRadColumn), ExcelCellFormatter.ColorColonneDadAvancement);

            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(RadColumn), ExcelCellFormatter.ColorColonneDadAvancement);

            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(AjustementColumn), ExcelCellFormatter.ColorEcartAjustement);

            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(PfaColumn), ExcelCellFormatter.DefautColor);

            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(AvancementMoisCourantColumn), ExcelCellFormatter.DefautColor);

            ApplyCustomCellTemplate(sheet, ComputeRangeForStyle(AvancementCumuleColumn), ExcelCellFormatter.DefautColor);
        }

        private void SetTotalCellsStyle(IWorksheet sheet)
        {
            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(MontantBudgetColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            //Style colonnes DAD
            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(DadMoisCourantColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(DadCumuleColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            //Style colonnes Dépense
            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(DepensesMoisCourantColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(DepenseCumuleeColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            //Style colonnes Ecart
            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(EcartMoisCourantColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(EcartCumuleColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            //Style colonnes Rad, Pourcentages Rad
            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(PourcentageRadColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(RadColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(AjustementColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(PfaColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(AvancementMoisCourantColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);

            ApplyCustomCellTemplate(sheet, GetTotalRangeForColumn(AvancementCumuleColumn), ExcelCellFormatter.DefautColor, applyBoldFont: true);
        }

        /// <summary>
        /// Définit le "faux" pied de page du classeur excel
        /// </summary>
        /// <param name="sheet">Classeur Excel</param>
        private void SetExcelFooter(IWorksheet sheet)
        {
            var indexLigneTotaux = GetIndexLigneTotaux();
            var rangeTotalBudget = sheet.Range[$"{LibelleColumn}{indexLigneTotaux}"];
            rangeTotalBudget.Text = FeatureControleBudgetaire.TotalDepenseExcel;

            rangeTotalBudget = sheet.Range[$"{CodeColumn}{indexLigneTotaux}:{LibelleColumn}{indexLigneTotaux}"];
            rangeTotalBudget.BorderAround();
            rangeTotalBudget.CellStyle.Font.Bold = true;
        }

        /// <summary>
        /// Applique les formules Excel après que le classeur soit populé
        /// </summary>
        /// <param name="sheet">Classeur Excel</param>
        private void SetFormulaAfterPopulate(IWorksheet sheet)
        {
            sheet.DisableSheetCalculations();
            //Formule Ecart Mois Courant
            sheet.Range[ComputeRange(EcartMoisCourantColumn)].Cells.ToList().ForEach(cells => cells.Formula = $"={DadMoisCourantColumn}{cells.Row} - {DepensesMoisCourantColumn}{cells.Row}");

            //Formule ecart cumule
            sheet.Range[ComputeRange(EcartCumuleColumn)].Cells.ToList().ForEach(cells => cells.Formula = $"={DadCumuleColumn}{cells.Row} - {DepenseCumuleeColumn}{cells.Row}");

            //Formule Rad
            sheet.Range[ComputeRange(RadColumn)].Cells.ToList().ForEach(cells => cells.Formula = $"={MontantBudgetColumn}{cells.Row} - {DadCumuleColumn}{cells.Row}");

            var totauxStartLine = excelDataListeEndLine + ligneTotauxStartAfterEndLine;

            //Formule pour les valeurs totales qui se calculent à partir d'autres valeurs totales
            //Formule Avancement mois courant : Dad/Budget
            var formulaTotal = $"={DadMoisCourantColumn}{totauxStartLine} / {MontantBudgetColumn}{totauxStartLine}";
            sheet.Range[$"{AvancementMoisCourantColumn}{totauxStartLine}"].Formula = formulaTotal;

            //Formule Ecart Mois Courant : Dad - Depenses
            formulaTotal = $"={DadMoisCourantColumn}{totauxStartLine} - {DepensesMoisCourantColumn}{totauxStartLine}";
            sheet.Range[$"{EcartMoisCourantColumn}{totauxStartLine}"].Formula = formulaTotal;

            //Formule Avancement Mois Cumule : Dad/Budget
            formulaTotal = $"={DadCumuleColumn}{totauxStartLine} / {MontantBudgetColumn}{totauxStartLine}";
            sheet.Range[$"{AvancementCumuleColumn}{totauxStartLine}"].Formula = formulaTotal;

            //Formule Ecart cumule : Dad - Depenses
            formulaTotal = $"={DadCumuleColumn}{totauxStartLine} - {DepenseCumuleeColumn}{totauxStartLine}";
            sheet.Range[$"{EcartCumuleColumn}{totauxStartLine}"].Formula = formulaTotal;

            //Formule pourcentage Rad : Rad / Budget
            formulaTotal = $"={RadColumn}{totauxStartLine}/{MontantBudgetColumn}{totauxStartLine}";
            sheet.Range[$"{PourcentageRadColumn}{totauxStartLine}"].Formula = formulaTotal;
        }

        /// <summary>
        /// Calcul le total du classeur
        /// </summary>
        /// <param name="sheet">Classeur Excel</param>
        private void SetTotal(IWorksheet sheet)
        {
            var formula = string.Join("+", topLevelLines.Distinct().Select(value => string.Concat("{0}", value)).ToArray());

            //Formule pour les valeurs totales qui se calculent en additionnant les T1
            AddTotalFormulaForColumn(sheet, MontantBudgetColumn, formula);
            AddTotalFormulaForColumn(sheet, DadMoisCourantColumn, formula);
            AddTotalFormulaForColumn(sheet, DepensesMoisCourantColumn, formula);
            AddTotalFormulaForColumn(sheet, DadCumuleColumn, formula);
            AddTotalFormulaForColumn(sheet, DepenseCumuleeColumn, formula);
            AddTotalFormulaForColumn(sheet, RadColumn, formula);
            AddTotalFormulaForColumn(sheet, AjustementColumn, formula);
            AddTotalFormulaForColumn(sheet, PfaColumn, formula);
        }

        /// <summary>
        /// Ajout des Headers de CI et des ruptures
        /// </summary>
        /// <param name="sheet">Feuille Excel</param>
        private void SetCIHeaders(IWorksheet sheet)
        {

            var rowIndex = excelDataListeStartLine;
            foreach (var ciModel in exportCIModels)
            {
                // Ajout des headers et des ruptures pour le mode multi CI
                if (exportCIModels.Count > 1)
                {
                    sheet.InsertRow(rowIndex);

                    // selection de la range de 21 colonnes
                    var headerRange = sheet.Range[rowIndex, 1, rowIndex, 21];
                    headerRange.Text = ciModel.CodeLibelleCI;
                    headerRange.Merge();
                    headerRange.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                    headerRange.CellStyle.Color = ExcelCellFormatter.ColorCIHeader;
                    headerRange.CellStyle.Font.Bold = true;
                    headerRange.BorderAround();
                    if (rowIndex != excelDataListeStartLine)
                    {
                        sheet.HPageBreaks.Add(sheet.Range[rowIndex, 1]);
                    }
                    rowIndex += 1;
                }
                rowIndex += ciModel.Valeurs.Count;
            }


            // Calcul de l'ajout ou non du dernier saut de page en fonction du nombre de pages affichées.
            var ciTotalRows = exportCIModels.Last().Valeurs.Count;
            if (exportCIModels.Count == 1)
            {
                // 10 lignes de header en plus si c'est le premier CI
                ciTotalRows += 10;
            }

            // il y a environ 40 lignes par page et le footer fait 10 lignes
            if (ciTotalRows % 45 > 30)
            {
                // Pagebreak sur la ligne de totaux uniquement si ça dépasse
                sheet.HPageBreaks.Add(sheet.Range[rowIndex, 1]);
            }
        }

        /// <summary>
        /// Cette fonction ajoute la ligne courante à la formule pour calculer le total de la colonne donnée si et seulement si
        /// la ligne courante correspond à un T1. La formule générée sera de la forme : a + b + c + ... 
        /// </summary>
        /// <param name="sheet">feuille de l'excel contenant les valeurs</param>
        /// <param name="colonne">colonne dont on veut la somme des T1</param>
        /// <param name="formulas">template de la formule pour calculer la somme</param>
        private void AddTotalFormulaForColumn(IWorksheet sheet, char colonne, string formulas)
        {
            var totauxStartLine = excelDataListeEndLine + ligneTotauxStartAfterEndLine;

            var rangeTotalForColonne = sheet.Range[$"{colonne}{totauxStartLine}"];

            var formattedFormula = string.Format(formulas, colonne);

            //On initialise une formule qui sera : = [La valeur à la ligne courante]
            rangeTotalForColonne.Formula = formattedFormula;
        }

        /// <summary>
        /// Applique une couleur de fond a une colonne excel d'un classeur, en indiquant si l'on souhaite avoir des bordures autours de cette dernière
        /// </summary>
        /// <param name="sheet">Classeure Excel</param>
        /// <param name="range">Range de la colonne Excel</param>
        /// <param name="color">Couleur de fond</param>
        /// <param name="applyBorder">Applique une bordure</param>
        /// <param name="applyBoldFont">Met le texte en gras si true</param>
        private void ApplyCustomCellTemplate(IWorksheet sheet, string range, Color color, bool applyBorder = true, bool applyBoldFont = false)
        {
            sheet.Range[range].CellStyle.Color = color;
            if (applyBorder)
            {
                sheet.Range[range].BorderAround();
            }

            if (applyBoldFont)
            {
                sheet.Range[range].CellStyle.Font.Bold = true;
            }
        }

        /// <summary>
        /// Applique une couleur de fond a une arbre budgetaire dans un classeur excel, en indiquant si l'on souhaite avoir des bordures autours de cette dernière
        /// </summary>
        /// <param name="sheet">Classeur Excel</param>
        /// <param name="range">Range de la colonne Excel</param>
        /// <param name="applyBorder">Applique une bordure</param>
        private void ApplyCustomCellTemplateForTree(IWorksheet sheet, string range, bool applyBorder = true)
        {
            sheet.Range[range].Cells.Where(q => q.Value.Contains((AxeTypes.T1.ToFriendlyName()))).ToList().ForEach(cell =>
            {
                cell.CellStyle.Color = ExcelCellFormatter.ColorT1;
                cell.CellStyle.Font.Color = ExcelKnownColors.White;
            });
            sheet.Range[range].Cells.Where(q => q.Value.Contains((AxeTypes.T2.ToFriendlyName()))).ToList().ForEach(cell =>
            {
                cell.CellStyle.Color = ExcelCellFormatter.ColorT2;
                cell.CellStyle.Font.Color = ExcelKnownColors.White;
            });
            sheet.Range[range].Cells.Where(q => q.Value.Contains((AxeTypes.T3.ToFriendlyName()))).ToList().ForEach(cell =>
            {
                cell.CellStyle.Color = ExcelCellFormatter.ColorT3;
                cell.CellStyle.Font.Color = ExcelKnownColors.White;
            });

            sheet.Range[range].Cells.Where(q => q.Value.Contains((AxeTypes.Chapitre.ToFriendlyName()))).ToList().ForEach(cell => cell.CellStyle.Color = ExcelCellFormatter.ColorR1);
            sheet.Range[range].Cells.Where(q => q.Value.Contains((AxeTypes.SousChapitre.ToFriendlyName()))).ToList().ForEach(cell => cell.CellStyle.Color = ExcelCellFormatter.ColorR2);
            sheet.Range[range].Cells.Where(q => q.Value.Contains((AxeTypes.Ressource.ToFriendlyName()))).ToList().ForEach(cell => cell.CellStyle.Color = ExcelCellFormatter.ColorR3);

            sheet.Range[range.Replace("A", "B")].Cells.ToList().ForEach(cell =>
            {
                cell.CellStyle.Color = sheet.Range[cell.Row, 1].CellStyle.Color;
                cell.CellStyle.Font.Color = sheet.Range[cell.Row, 1].CellStyle.Font.Color;
            });

            if (applyBorder)
            {
                sheet.Range[range].BorderAround();
                sheet.Range[range.Replace("A", "B")].BorderAround();
            }

        }

    }
}
