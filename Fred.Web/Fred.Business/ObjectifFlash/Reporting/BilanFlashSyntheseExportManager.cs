using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Fred.Business.ObjectifFlash.Models;
using Fred.Business.ObjectifFlash.Reporting.Models;
using Fred.Business.Utilisateur;
using Fred.Framework.Reporting;
using Syncfusion.XlsIO;

namespace Fred.Business.ObjectifFlash.Reporting
{
    public class BilanFlashSyntheseExportManager : Manager, IBilanFlashSyntheseExportManager
    {
        private const string ExcelTemplateBilanFlashSynthese = "Templates/BilanFlash/TemplateBilanFlashSynthese.xlsx";
        private const string TypeCacheExcel = "excelBytes_";
        private const string AxeSousChapitre = "SousChapitre";
        private const string AxeChapitre = "Chapitre";
        private const string AxeTache3 = "T3";

        private static Dictionary<string, Tuple<Color, Color>> AxeColors => new Dictionary<string, Tuple<Color, Color>>
        {
            {AxeChapitre, new Tuple<Color, Color>(Color.FromArgb(255, 216, 0), Color.Black)},
            {AxeSousChapitre, new Tuple<Color, Color>(Color.FromArgb(255, 234, 113), Color.Black) },
            {AxeTache3, new Tuple<Color, Color>(Color.FromArgb(160, 182, 208), Color.White)}
        };

        private readonly IObjectifFlashManager objectifFlashManager;
        private readonly IUtilisateurManager utilisateurManager;

        public BilanFlashSyntheseExportManager(
            IObjectifFlashManager objectifFlashManager,
            IUtilisateurManager utilisateurManager)
        {
            this.objectifFlashManager = objectifFlashManager;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Export de la synthese du bilan flash
        /// </summary>
        /// <param name="objectifFlashId">Id de l'objectif flash</param>
        /// <param name="dateDebut"> date de ébut</param>
        /// <param name="dateFin">date de fin</param>
        /// <param name="isPdfConverted">flag de conversion pdf</param>
        /// <returns>Identifiant d'export</returns>
        public async Task<object> ExportBilanFlashSyntheseAsync(int? objectifFlashId, DateTime? dateDebut, DateTime? dateFin, bool isPdfConverted)
        {
            ExcelFormat excelFormat = new ExcelFormat();
            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(AppDomain.CurrentDomain.BaseDirectory + ExcelTemplateBilanFlashSynthese);
            excelFormat.InitVariables(workbook);

            var objectifFlash = objectifFlashManager.GetObjectifFlashWithTachesById(objectifFlashId.Value);


            var enteteCI = $"{objectifFlash.Ci.Code} - {objectifFlash.Ci.Libelle}";
            var enteteDates = $"Du {dateDebut.Value.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)} " +
                              $"au {dateFin.Value.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";

            var utilisateurPrenomNom = utilisateurManager.GetContextUtilisateur()?.PrenomNom;

            var enteteEditionInfos = $"Edité par: {utilisateurPrenomNom} " +
                                     $"- Date d'édition: {DateTime.Now.ToString("dd/M/yyyy HH:mm", CultureInfo.InvariantCulture)}";

            excelFormat.AddVariable("EnteteCI", enteteCI);
            excelFormat.AddVariable("EnteteDates", enteteDates);
            excelFormat.AddVariable("EnteteEditionInfos", enteteEditionInfos);


            var depenses = await objectifFlashManager.GetObjectifFlashDepensesAsync(objectifFlash, dateDebut.Value.Date, dateFin.Value.Date).ConfigureAwait(false);
            var totalDepenses = depenses.Sum(x => x.MontantHT);


            var depensesChapitreSousChapitre = GroupDepenses(OrderDepenses(depenses, true), new List<string> { AxeChapitre, AxeSousChapitre });
            var depensesTache3Chapitre = GroupDepenses(OrderDepenses(depenses, true), new List<string> { AxeTache3, AxeChapitre });

            excelFormat.AddVariable("TotalDepenses", totalDepenses);
            excelFormat.AddVariable("DepensesChapitreSousChapitre", depensesChapitreSousChapitre);
            excelFormat.AddVariable("DepensesTache3Chapitre", depensesTache3Chapitre);
            excelFormat.ApplyVariables();
            CustomTransformation(workbook, depensesChapitreSousChapitre, depensesTache3Chapitre);

            byte[] bytes = null;

            if (isPdfConverted)
            {
                MemoryStream memoryStream = new MemoryStream();
                excelFormat.PrintExcelToPdfAutoFit(workbook).Save(memoryStream);
                bytes = memoryStream.ToArray();
            }
            else
            {
                bytes = excelFormat.ConvertToByte(workbook);
            }

            string cacheId = TypeCacheExcel + Guid.NewGuid().ToString();
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
            MemoryCache.Default.Add(cacheId, bytes, policy);
            return new { id = cacheId };
        }

        /// <summary>
        /// Tri des dépenses
        /// </summary>
        /// <param name="depenses">dépenses</param>
        /// <param name="orderByChapitre">tri par chapitre</param>
        /// <returns>liste des dépenses ordonnées</returns>
        private List<ObjectifFlashDepenseModel> OrderDepenses(IEnumerable<ObjectifFlashDepenseModel> depenses, bool orderByChapitre)
        {
            if (orderByChapitre)
            {
                return depenses.OrderBy(x => x.Ressource.SousChapitre.Chapitre.Code)
                               .ThenBy(x => x.Ressource.SousChapitre.Code)
                               .ToList();
            }
            else
            {
                return depenses.OrderBy(x => x.Tache.Code)
                               .ThenBy(x => x.Ressource.SousChapitre.Chapitre.Code)
                               .ToList();
            }
        }

        /// <summary>
        /// Regroupement des dépenses
        /// </summary>
        /// <param name="depenses">liste des dépenses flat</param>
        /// <param name="axes">liste des axes de regroupement</param>
        /// <param name="level">niveau calculé pour l'indentation</param>
        /// <returns>dépenses regroupées</returns>
        private List<BilanFlashSyntheseExportModel> GroupDepenses(List<ObjectifFlashDepenseModel> depenses, List<string> axes, int level = 0)
        {
            List<BilanFlashSyntheseExportModel> exportData = new List<BilanFlashSyntheseExportModel>();

            string libelleLeftSpace = "  ";
            for (int i = 0; i < level; i++)
            {
                libelleLeftSpace = $"       {libelleLeftSpace}";
            }

            string axe = axes.FirstOrDefault();
            //Axes restants
            axes = axes.Where(x => x != axe).ToList();
            // regroupement des données en fonction des axes
            foreach (var group in depenses.GroupBy(this.GetGroupingKey(axe)))
            {
                exportData.Add(
                    new BilanFlashSyntheseExportModel
                    {
                        AxeGroup = axe,
                        Libelle = libelleLeftSpace + GetCodeLibelle(group.First(), axe),
                        Montant = group.Sum(x => x.MontantHT),
                    });
                if (axes.Any())
                {
                    exportData.AddRange(GroupDepenses(group.ToList(), axes, level + 1));
                }
            }
            return exportData;
        }

        /// <summary>
        /// retourne la clef de regroupement en fonction de l'axe
        /// </summary>
        /// <param name="axe">axe</param>
        /// <returns>clef de regroupement</returns>
        private Func<ObjectifFlashDepenseModel, object> GetGroupingKey(string axe)
        {
            switch (axe)
            {
                case AxeSousChapitre:
                    return x => x.Ressource.SousChapitre.SousChapitreId;
                case AxeChapitre:
                    return x => x.Ressource.SousChapitre.Chapitre.ChapitreId;
                case AxeTache3:
                    return x => x.Tache.TacheId;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Retourne le code libellé en fonction de l'axe
        /// </summary>
        /// <param name="data">donnée</param>
        /// <param name="axe">axe</param>
        /// <returns>Code libellé concaténés</returns>
        private string GetCodeLibelle(ObjectifFlashDepenseModel data, string axe)
        {
            switch (axe)
            {
                case AxeSousChapitre:
                    return $"{ data.Ressource.SousChapitre.Code}-{data.Ressource.SousChapitre.Libelle}";
                case AxeChapitre:
                    return $"{ data.Ressource.SousChapitre.Chapitre.Code}-{data.Ressource.SousChapitre.Chapitre.Libelle}";
                case AxeTache3:
                    return $"{ data.Tache.Code}-{data.Tache.Libelle}";
                default:
                    return string.Empty;
            }
        }


        /// <summary>
        /// Mise en forme du workbook
        /// </summary>
        /// <param name="workbook">workbook</param>
        /// <param name="depensesChapitreSousChapitre">Dépenses groupées par Chapitre/Sous Chapitre</param>
        /// <param name="depensesTache3Chapitre">Dépenses groupées par Tache3/Sous Chapitre</param>
        private void CustomTransformation(IWorkbook workbook, List<BilanFlashSyntheseExportModel> depensesChapitreSousChapitre, List<BilanFlashSyntheseExportModel> depensesTache3Chapitre)
        {
            // mise en forme de la colonne Chapitre/Sous Chapitre
            for (int index = 0; index < depensesChapitreSousChapitre.Count; index++)
            {
                int rowIndex = 8 + index;
                var range = workbook.ActiveSheet.Range[rowIndex, 1, rowIndex, 3];
                range.Merge();
                range = workbook.ActiveSheet.Range[rowIndex, 1, rowIndex, 4];
                range.CellStyle.Color = AxeColors[depensesChapitreSousChapitre[index].AxeGroup].Item1;
                range.CellStyle.Font.RGBColor = AxeColors[depensesChapitreSousChapitre[index].AxeGroup].Item2;
            }

            // mise en forme de la colonne Tache3/Chapitre
            for (int index = 0; index < depensesTache3Chapitre.Count; index++)
            {
                int rowIndex = 8 + index;
                var range = workbook.ActiveSheet.Range[rowIndex, 6, rowIndex, 8];
                range.Merge();
                range = workbook.ActiveSheet.Range[rowIndex, 6, rowIndex, 9];
                range.CellStyle.Color = AxeColors[depensesTache3Chapitre[index].AxeGroup].Item1;
                range.CellStyle.Font.RGBColor = AxeColors[depensesTache3Chapitre[index].AxeGroup].Item2;
            }
        }
    }
}
