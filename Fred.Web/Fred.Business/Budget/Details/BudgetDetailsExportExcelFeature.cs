using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Fred.Business.CI;
using Fred.Business.Images;
using Fred.Business.Personnel;
using Fred.Business.Referential.Tache;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Web.Shared.Models.Budget.Details;
using MoreLinq;
using Syncfusion.XlsIO;

namespace Fred.Business.Budget.Details
{
    /// <summary>
    /// Gère l'export excel du détail d'un budget
    /// </summary>
    public class BudgetDetailsExportExcelFeature : IBudgetDetailsExportExcelFeature
    {
        private static string templateNameAnalyse = "Analyse";
        private static string templateNameEditable = "Editable";
        private static string excelTemplateAnalyse = "Templates/DetailsBudget/TemplateDetailsBudgetAnalyse.xlsx";
        private static string excelTemplateEditable = "Templates/DetailsBudget/TemplateDetailsBudget.xlsx";

        private string labelCIInfos = string.Empty;
        private IEnumerable<BudgetDetailsExportExcelEditableModel> valeursExportEditable;
        private IEnumerable<BudgetDetailsExportExcelModel> valeursExportAnalyse;

        /// <summary>
        /// Gestionnaire principal des budgets utilisé pour récupérer le détail
        /// </summary>
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly ICIManager ciManager;
        private readonly IBudgetManager budgetManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ITacheManager tacheManager;
        private readonly IBudgetWorkflowManager budgetWorkflowManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IImageManager ImageManager;
        private readonly IPersonnelRepository personnelRepository;

        /// <summary>
        /// Instancie un export excel à l'aide d'un budget main manager
        /// </summary>
        /// <param name="budgetT4Manager">Gestionnaire de T4 utilisé pour récupérer le budget</param>
        /// <param name="ciManager">Manager de CI</param>
        /// <param name="budgetManager">Manager de budget</param>
        /// <param name="utilisateurManager">Manager Utilisateur</param>
        /// <param name="budgetWorkflowManager">Manager de Workflow</param>
        /// <param name="tacheManager">Manager de Tache</param>
        /// <param name="personnelRepository">Manager du Personnel</param>
        public BudgetDetailsExportExcelFeature(IBudgetT4Manager budgetT4Manager,
          ICIManager ciManager,
          IBudgetManager budgetManager,
          IUtilisateurManager utilisateurManager,
          IBudgetWorkflowManager budgetWorkflowManager,
          ITacheManager tacheManager,
          IImageManager imageManager,
          IPersonnelManager personnelManager,
          IPersonnelRepository personnelRepository)
        {
            this.budgetT4Manager = budgetT4Manager;
            this.ciManager = ciManager;
            this.budgetManager = budgetManager;
            this.utilisateurManager = utilisateurManager;
            this.tacheManager = tacheManager;
            this.budgetWorkflowManager = budgetWorkflowManager;
            this.personnelManager = personnelManager;
            this.ImageManager = imageManager;
            this.personnelRepository = personnelRepository;
        }



        /// <summary>
        /// Renvoi un export excel en utilisant la classe BudgetDetailExportExcelFeature
        /// </summary>
        /// <param name="model">BudgetDetailsExportExcelLoadModel</param>
        /// <returns>les données de l'excel sous forme de tableau d'octets</returns>
        public byte[] GetBudgetDetailExportExcel(BudgetDetailsExportExcelLoadModel model)
        {

            int budgetId = model.BudgetId;
            string templateName = model.TemplateName;
            bool isPdfConverted = model.IsPdfConverted;
            bool disabledTasksDisplayed = model.DisabledTasksDisplayed;
            List<int> niveauxVisibles = model.NiveauxVisibles;

            using (var excelFormat = new ExcelFormat())
            {
                byte[] bytes = null;

                if (templateName == templateNameAnalyse)
                {
                    this.valeursExportAnalyse = GetValeursAnalyse(budgetId);
                    if (!isPdfConverted)
                    {
                        bytes = excelFormat.GenerateExcel(AppDomain.CurrentDomain.BaseDirectory + excelTemplateAnalyse, this.valeursExportAnalyse, CustomTransformationAnalyse);
                    }
                    else
                    {
                        bytes = excelFormat.GeneratePdfFromExcel(AppDomain.CurrentDomain.BaseDirectory + excelTemplateAnalyse, this.valeursExportAnalyse, CustomTransformationAnalyse);
                    }
                }
                else if (templateName == templateNameEditable)
                {
                    this.valeursExportEditable = GetValeursExportEditable(budgetId, disabledTasksDisplayed, niveauxVisibles);
                    if (!isPdfConverted)
                    {
                        bytes = excelFormat.GenerateExcel(AppDomain.CurrentDomain.BaseDirectory + excelTemplateEditable, this.valeursExportEditable, CustomTransformationEditable, GetHeaderModel());
                    }
                    else
                    {
                        bytes = excelFormat.GeneratePdfFromExcel(AppDomain.CurrentDomain.BaseDirectory + excelTemplateEditable, this.valeursExportEditable, CustomTransformationEditable, buildHeaderModel: GetHeaderModel());
                    }
                }
                else
                {
                    throw new FredBusinessMessageResponseException("Export Budget template invalide");
                }

                var memoryStream = new MemoryStream(bytes);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Recupération des valeurs pour l'export editable
        /// </summary>
        /// <param name="budgetId">Id du budget</param>
        /// <param name="disabledTasksDisplayed">flag affichage tâches inactives</param>
        /// <returns>liste de BudgetDetailsExportExcelEditableModel</returns>
        /// <param name="niveauxVisibles">Liste des niveaux à affficher</param>
        private IEnumerable<BudgetDetailsExportExcelEditableModel> GetValeursExportEditable(int budgetId, bool disabledTasksDisplayed, List<int> niveauxVisibles)
        {
            var result = new List<BudgetDetailsExportExcelEditableModel>();
            var ciId = this.budgetManager.GetCiIdAssociatedToBudgetId(budgetId);
            var ci = this.ciManager.GetCIById(ciId);
            var dateMiseEnApplicationBudget = budgetWorkflowManager.GetLastLockWorkflowDate(budgetId);
            var tacheEnts = tacheManager.GetAllT1ByCiId(ciId, dateMiseEnApplicationBudget, true).ToList();
            var budgetT4s = this.budgetT4Manager.GetByBudgetId(budgetId, true);

            labelCIInfos = ci.CodeLibelle;

            // taches du plan de tache
            result.AddT1Nodes(tacheEnts, budgetT4s, disabledTasksDisplayed, niveauxVisibles);

            return result;
        }

        /// <summary>
        /// Recupération des valeurs pour l'export analyse
        /// </summary>
        /// <param name="budgetId">Id du budget</param>
        /// <returns>liste de BudgetDetailsExportExcelModel</returns>
        private IEnumerable<BudgetDetailsExportExcelModel> GetValeursAnalyse(int budgetId)
        {
            var ciId = this.budgetManager.GetCiIdAssociatedToBudgetId(budgetId);
            var ci = this.ciManager.GetCIById(ciId);
            var budgetT4s = this.budgetT4Manager.GetByBudgetId(budgetId, true)
                .OrderBy(bt4 => bt4.T3.Parent.Parent.Code)
                .ThenBy(bt4 => bt4.T3.Parent.Code)
                .ThenBy(bt4 => bt4.T3.Code)
                .ThenBy(bt4 => bt4.T4.Code);

            var result = new List<BudgetDetailsExportExcelModel>();

            foreach (var budgetT4 in budgetT4s)
            {
                result.AddRange(budgetT4.BudgetSousDetails.Select(sd => GetBudgetDetailsExportExcelModel(sd, budgetT4, ci)));
            }

            return result;
        }

        private BudgetDetailsExportExcelModel GetBudgetDetailsExportExcelModel(BudgetSousDetailEnt sd, BudgetT4Ent budgetT4, CIEnt ci)
        {
            var hasVueSD = budgetT4.VueSD == 1;
            return new BudgetDetailsExportExcelModel
            {
                CiCodeLibelle = ci.CodeLibelle,
                CodeLibelleT1 = FormatTacheCodeLibelle(budgetT4.T3.Parent.Parent),
                CodeLibelleT2 = FormatTacheCodeLibelle(budgetT4.T3.Parent),
                CodeLibelleT3 = FormatTacheCodeLibelle(budgetT4.T3),
                CodeLibelleT4 = FormatTacheCodeLibelle(budgetT4.T4),
                Commentaire = sd.Commentaire,
                UniteT4 = budgetT4.Unite?.Libelle,
                QuantiteT4 = budgetT4.QuantiteARealiser,
                PuT4 = budgetT4.PU,
                MontantT4 = budgetT4.MontantT4,
                LibelleChapitre = sd.Ressource.SousChapitre.Chapitre.Code + " " +
                                  sd.Ressource.SousChapitre.Chapitre.Libelle,
                LibelleSousChapitre = sd.Ressource.SousChapitre.Code + " " + sd.Ressource.SousChapitre.Libelle,
                LibelleRessource = sd.Ressource.Code + " " + sd.Ressource.Libelle,
                QuantiteRessourceT4 = sd.Quantite,
                UniteRessourceT4 = sd.Unite?.Code,
                PuRessourceT4 = sd.PU,
                MontantRessourceT4 = sd.Quantite * sd.PU,
                QuantiteBaseRessourceSd = hasVueSD ? budgetT4.QuantiteDeBase : null,
                UniteBaseRessourceSd = budgetT4.Unite?.Code,
                QuantiteRessourceSd = hasVueSD ? sd.QuantiteSD ?? (decimal?)0 : null,
                UniteRessourceSd = hasVueSD ? sd.Unite?.Code : null,
                PuRessourceSd = hasVueSD ? sd.PU ?? (decimal?)0 : null,
                MontantRessourceSd = hasVueSD ? (decimal?)(sd.QuantiteSD ?? 0) * (sd.PU ?? 0) : null
            };
        }

        private string FormatTacheCodeLibelle(TacheEnt tache)
        {
            return tache.Code + "-" + tache.Libelle;
        }

        /// <summary>
        /// Custom Transformation pour l'export Editable
        /// </summary>
        /// <param name="workbook">workbook à transformer</param>
        private void CustomTransformationEditable(IWorkbook workbook)
        {
            // colonnes numériques
            // necessaire de le gérer ici car le color formatting ecrase le number format défini dans excel
            var numericColumnIndexes = new List<int> { 2, 4, 5, 10, 11, 12, 13, 14, 15 };

            // couleurs 
            Color colorT1 = Color.FromArgb(64, 109, 161);
            Color colorT2 = Color.FromArgb(102, 138, 180);
            Color colorT3 = Color.FromArgb(160, 182, 208);
            Color colorT4 = Color.FromArgb(207, 218, 231);
            Color colorMontantT1 = Color.FromArgb(255, 191, 74);
            Color colorMontantT2 = Color.FromArgb(255, 199, 97);
            Color colorMontantT3 = Color.FromArgb(255, 212, 134);
            Color colorMontantT4 = Color.FromArgb(255, 246, 230);
            Color colorUniteQtePuT1 = Color.FromArgb(255, 215, 140);
            Color colorUniteQtePuT2 = Color.FromArgb(255, 220, 155);
            Color colorUniteQtePuT3 = Color.FromArgb(255, 228, 178);
            Color colorUniteQtePuT4 = Color.FromArgb(255, 250, 239);

            var columnColorsStandard = new Dictionary<string, Color>
            {
                { "T1", colorT1},
                { "T2", colorT2},
                { "T3", colorT3},
                { "T4", colorT4},
            };

            var columnColorsMontant = new Dictionary<string, Color>
            {
                { "T1", colorMontantT1},
                { "T2", colorMontantT2},
                { "T3", colorMontantT3},
                { "T4", colorMontantT4},
            };

            var columnColorsTacheT4 = new Dictionary<string, Color>
            {
                { "T1", colorUniteQtePuT1},
                { "T2", colorUniteQtePuT2},
                { "T3", colorUniteQtePuT3},
                { "T4", colorUniteQtePuT4},
            };

            var sheet = workbook.Worksheets[0];

            for (var rowIndex = 3; rowIndex < this.valeursExportEditable.Count() + 3; rowIndex++)
            {
                var row = sheet.Rows[rowIndex];
                for (var colIndex = 0; colIndex < row.Cells.Count(); colIndex++)
                {
                    var cell = row.Cells[colIndex];
                    var level = row.Cells[0].Text?.Substring(0, 2) ?? string.Empty;

                    // cell background color
                    var cellBackgroundColor = GetExportEditableCellBackgroundColorForLevel(level, colIndex, columnColorsStandard, columnColorsMontant, columnColorsTacheT4);
                    if (cellBackgroundColor != null)
                    {
                        cell.CellStyle.Color = cellBackgroundColor.Value;
                    }

                    // text color
                    if ((colIndex < 2 || colIndex > 6) &&
                        new string[] { "T1", "T2", "T3" }.Contains(level))
                    {
                        cell.CellStyle.Font.RGBColor = Color.White;
                    }
                    // format numériques
                    if (numericColumnIndexes.Contains(colIndex))
                    {
                        cell.NumberFormat = "#,##0.00";
                    }
                }
            }
        }

        /// <summary>
        /// Retourne la couleur de cellule pour la l'export editable en fonction du level et de l'index de colonne
        /// </summary>
        /// <param name="level">level T1,T2,T3</param>
        /// <param name="colIndex">index de la colonne</param>
        /// <param name="columnColorsStandard">Dictionnaire des couleurs standards</param>
        /// <param name="columnColorsMontant">Dictionnaire des couleurs Montant</param>
        /// <param name="columnColorsTacheT4">Dictionnaire des couleurs T4</param>
        /// <returns>Color (null if not found)</returns>
        private Color? GetExportEditableCellBackgroundColorForLevel(
            string level,
            int colIndex,
            Dictionary<string, Color> columnColorsStandard,
            Dictionary<string, Color> columnColorsMontant,
            Dictionary<string, Color> columnColorsTacheT4)
        {

            if (colIndex == 2 && columnColorsMontant.ContainsKey(level))
            {
                return columnColorsMontant[level];
            }
            else if (colIndex > 2 && colIndex < 6 && columnColorsTacheT4.ContainsKey(level))
            {
                return columnColorsTacheT4[level];
            }
            else if (columnColorsStandard.ContainsKey(level))
            {
                return columnColorsStandard[level];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Custom Transformation pour l'export Analyse
        /// </summary>
        /// <param name="workbook">workbook à transformer</param>
        private void CustomTransformationAnalyse(IWorkbook workbook)
        {
            var sheet = workbook.Worksheets[0];

            // colonnes numériques
            // necessaire de le gérer ici car le color formatting ecrase le number format défini dans excel
            var numericColumnIndexes = new List<int> { 8, 9, 13, 15, 16, 17, 19, 21, 22 };

            // couleurs 
            Color colorT1 = Color.FromArgb(64, 109, 161);
            Color colorT2 = Color.FromArgb(102, 138, 180);
            Color colorT3 = Color.FromArgb(160, 182, 208);
            Color colorT4 = Color.FromArgb(207, 218, 231);
            Color colorT4Infos = Color.FromArgb(255, 228, 178);
            Color colorR1 = Color.FromArgb(255, 216, 0);
            Color colorR2 = Color.FromArgb(255, 234, 113);
            Color colorR3 = Color.FromArgb(255, 244, 187);
            Color colorDetailT4 = Color.FromArgb(132, 214, 210);
            Color colorDetailSD = Color.FromArgb(132, 199, 219);
            var columnColors = new Dictionary<int, Color>
            {
                { 1, colorT1},
                { 2, colorT2},
                { 3, colorT3},
                { 4, colorT4},
                { 5, colorT4Infos},
                { 6, colorT4Infos},
                { 7, colorT4Infos},
                { 8, colorT4Infos},
                { 9, colorT4Infos},
                { 10, colorR1},
                { 11, colorR2},
                { 12, colorR3},
                { 13, colorDetailT4},
                { 14, colorDetailT4},
                { 15, colorDetailT4},
                { 16, colorDetailT4},
                { 17, colorDetailSD},
                { 18, colorDetailSD},
                { 19, colorDetailSD},
                { 20, colorDetailSD},
                { 21, colorDetailSD},
                { 22, colorDetailSD}
            };

            // resize les colonnes au contenu
            for (int colIndex = 1; colIndex < 23; colIndex++)
            {
                sheet.AutofitColumn(colIndex);
            }

            for (var rowIndex = 1; rowIndex <= this.valeursExportAnalyse.Count() + 1; rowIndex++)
            {
                var row = sheet.Rows[rowIndex];
                for (var colIndex = 0; colIndex < 23; colIndex++)
                {
                    var cell = row.Cells[colIndex];
                    if (columnColors.ContainsKey(colIndex))
                    {
                        cell.CellStyle.Color = columnColors[colIndex];
                    }
                    // format numériques
                    if (numericColumnIndexes.Contains(colIndex))
                    {
                        cell.NumberFormat = "#,##0.00";
                    }
                }
            }
        }

        /// <summary>
        /// Créé le model du header
        /// </summary>
        /// <returns>Model du header</returns>
        public BuildHeaderExcelModel GetHeaderModel()
        {
            var now = DateTime.Now;

            var utilisateurId = utilisateurManager.GetContextUtilisateurId();
            var editeur = personnelRepository.GetPersonnelPourExportExcelHeader(utilisateurId);
            return new BuildHeaderExcelModel(
                labelCIInfos,
                null,
                BusinessResources.Export_Header_DateEdition + now.ToShortDateString() + " " + now.ToShortTimeString(),
                BusinessResources.Export_Header_EditePar + personnelManager.GetPersonnelMatriculeNomPrenom(editeur),
                null,
                editeur.SocieteId != null ? AppDomain.CurrentDomain.BaseDirectory + ImageManager.GetLogoImage(editeur.SocieteId.Value).Path : null,
                new IndexHeaderExcelModel(3, 10, 15, 16));
        }
    }
}
