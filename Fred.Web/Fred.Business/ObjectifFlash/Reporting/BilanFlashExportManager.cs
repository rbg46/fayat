using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.ObjectifFlash.Reporting.Models;
using Fred.Entities.ObjectifFlash;
using Fred.Framework.Exceptions;
using Fred.Framework.Reporting;
using Fred.Web.Shared.App_LocalResources;
using Syncfusion.XlsIO;

namespace Fred.Business.ObjectifFlash.Reporting
{
    public class BilanFlashExportManager : Manager, IBilanFlashExportManager
    {
        private const string ExcelTemplateBilanFlash = "Templates/BilanFlash/TemplateBilanFlash.xlsx";
        private const string TypeCachePdf = "pdfBytes_";
        private const string TypeCacheExcel = "excelBytes_";

        private readonly IObjectifFlashManager objectifFlashManager;

        public BilanFlashExportManager(IObjectifFlashManager objectifFlashManager)
        {
            this.objectifFlashManager = objectifFlashManager;
        }

        /// <summary>
        /// Export d'un objectif flash en memory cache
        /// </summary>
        /// <param name="objectifFlashId">Identifiant d'objectif flash</param>
        /// <param name="dateDebut">date de début</param>
        /// <param name="dateFin">date de fin</param>
        /// <param name="isPdfConverted">flag de conversion pdf</param>
        /// <returns>Id de cachce dans un object anonyme</returns>
        public async Task<object> ExportBilanFlashAsync(int? objectifFlashId, DateTime? dateDebut, DateTime? dateFin, bool isPdfConverted)
        {
            if (!(objectifFlashId.HasValue && dateDebut.HasValue && dateFin.HasValue))
            {
                throw new FredBusinessException(FeatureObjectifFlash.ObjectifFlash_Error_InvalidExportParameters);
            }

            ExcelFormat excelFormat = new ExcelFormat();
            IWorkbook workbook = await GetExportWorkbookAsync(excelFormat, objectifFlashId.Value, dateDebut.Value, dateFin.Value).ConfigureAwait(false);
            try
            {
                byte[] bytes = null;

                if (isPdfConverted)
                {
                    this.CustomTransformationPdf(workbook);
                    MemoryStream memoryStream = new MemoryStream();
                    excelFormat.PrintExcelToPdfAutoFit(workbook).Save(memoryStream);
                    bytes = memoryStream.ToArray();
                }
                else
                {
                    bytes = excelFormat.ConvertToByte(workbook);
                }

                string cacheId = string.Empty;
                if (isPdfConverted)
                {
                    cacheId = TypeCachePdf + Guid.NewGuid().ToString();
                }
                else
                {
                    cacheId = TypeCacheExcel + Guid.NewGuid().ToString();
                }

                CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
                MemoryCache.Default.Add(cacheId, bytes, policy);

                return new { id = cacheId };
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }


        }

        /// <summary>
        /// Mise en forme du workbook pour l'export pdf
        /// </summary>
        /// <param name="workbook">classeur excel</param>
        private void CustomTransformationPdf(IWorkbook workbook)
        {
            foreach (var worksheet in workbook.Worksheets)
            {
                worksheet.PageSetup.Orientation = ExcelPageOrientation.Portrait;
                foreach (var row in worksheet.Rows)
                {
                    row.CellStyle.Font.Size = 9;
                }
            }
        }

        /// <summary>
        /// retourne l'export d'un objectif flash pdf ou excel sous forme d'un tableau d'octets
        /// </summary>
        /// <param name="excelFormat">Helper syncfusion</param>
        /// <param name="objectifFlashId">Identifiant d'objectif flash</param>
        /// <param name="dateDebut">date de début</param>
        /// <param name="dateFin">date de fin</param>
        /// <returns>workbook syncfusion</returns>
        public async Task<IWorkbook> GetExportWorkbookAsync(ExcelFormat excelFormat, int objectifFlashId, DateTime dateDebut, DateTime dateFin)
        {
            ObjectifFlashEnt objectifFlash = objectifFlashManager.GetObjectifFlashWithRealisationsById(objectifFlashId, dateDebut, dateFin);
            CheckBilanFlashCriteria(objectifFlashId, objectifFlash, dateDebut, dateFin);
            // récupération de toutes les ressources duu bilan flash
            List<BilanFlashTacheRessourceExportModel> bilanFlashRessources = await GetBilanFlashExportResourcesAsync(objectifFlash, dateDebut, dateFin).ConfigureAwait(false);
            // Initialisation du template
            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(AppDomain.CurrentDomain.BaseDirectory + ExcelTemplateBilanFlash);
            var workSheetTemplate = workbook.ActiveSheet;

            foreach (ObjectifFlashTacheEnt objectifFlashTache in objectifFlash.Taches.Where(t => t.Ressources.Count != 0))
            {
                IWorksheet worksheet = workbook.Worksheets.AddCopy(workSheetTemplate);
                worksheet.Name = this.GetWorkSheetName(objectifFlashTache.Tache.Libelle);

                excelFormat.InitVariables(worksheet);
                string entetePeriode = "Bilan Flash journalier du " + dateDebut.ToShortDateString();
                if (dateFin.Subtract(dateDebut).Days > 0)
                {
                    entetePeriode += " au " + dateFin.ToShortDateString();
                }
                excelFormat.AddVariable("EntetePeriode", entetePeriode);

                string enteteCITache = objectifFlash.Ci.Code + " - " + objectifFlash.Ci.Libelle + " / " + objectifFlashTache.Tache.Code + " - " + objectifFlashTache.Tache.Libelle;
                excelFormat.AddVariable("EnteteCITache", enteteCITache);
                BilanFlashTacheExportModel tacheExportModel = this.GetBilanFlashTacheExportModel(objectifFlashTache, bilanFlashRessources, dateDebut.Date, dateFin.Date);
                excelFormat.AddVariable("BilanFlashTacheExportModel", tacheExportModel);

                excelFormat.ApplyVariables();
                // suppression de la ligne des ressources hors périmètre si aucune données
                if (!tacheExportModel.BilanFlashRessourceHorsPerimetre.Any())
                {
                    int rowIndexHorsPerimetre = 14 + tacheExportModel.BilanFlashRessourcePerimetre.Count;
                    worksheet.DeleteRow(rowIndexHorsPerimetre);
                }
                worksheet.AutofitColumn(4);
            }

            // Suppression de la feuille modèle
            if (workbook.Worksheets.Count > 1)
            {
                workbook.Worksheets.Remove(0);
            }
            return workbook;
        }

        /// <summary>
        /// Retourne un model d'export de tache de bilan flash avec ses valeurs calculées
        /// </summary>
        /// <param name="objectifFlashTache">tache d'objectif flash</param>
        /// <param name="bilanFlashRessources">ressources d'objectif flash</param>
        /// <param name="dateDebut">date de debut du bilan flash</param>
        /// <param name="dateFin">date de fin du bilan flash</param>
        /// <returns>Bilan flash tache export Model</returns>
        private BilanFlashTacheExportModel GetBilanFlashTacheExportModel(ObjectifFlashTacheEnt objectifFlashTache, List<BilanFlashTacheRessourceExportModel> bilanFlashRessources, DateTime dateDebut, DateTime dateFin)
        {
            // liste des ressources de la tache
            List<BilanFlashTacheRessourceExportModel> tacheRessources = bilanFlashRessources.Where(x => x.TacheId == objectifFlashTache.TacheId).ToList();
            BilanFlashTacheRessourceExportModel ressourceRepartition = tacheRessources.SingleOrDefault(x => x.IsRepartitionKey);
            // regroupement des ressources par libellé, unité
            List<BilanFlashTacheRessourceExportModel> tacheRessourcePerimetre = GroupRessources(tacheRessources.Where(x => !x.HorsPerimetre));
            List<BilanFlashTacheRessourceExportModel> tacheRessourceHorsPerimetre = GroupRessources(tacheRessources.Where(x => x.HorsPerimetre));
            // calculs des quantités / totaux
            decimal? quantiteRealiseTache = objectifFlashTache.TacheRealisations.Sum(x => x.QuantiteRealise);
            decimal? quantiteObjectifTache = this.GetQuantiteObjectifTache(objectifFlashTache, dateDebut.Date, dateFin.Date);
            decimal? totalMontantObjectif = tacheRessources.Sum(x => x.QuantiteObjectif * x.PuHT);
            decimal? totalMontantRealise = tacheRessources.Sum(x => x.MontantRealise);
            decimal? totalMontantPerimetreObjectif = tacheRessources.Where(x => !x.HorsPerimetre).Sum(x => x.QuantiteObjectif * x.PuHT);
            decimal? totalMontantPerimetreRealise = tacheRessources.Where(x => !x.HorsPerimetre).Sum(x => x.MontantRealise);

            return new BilanFlashTacheExportModel
            {
                QuantiteObjectif = quantiteObjectifTache,
                QuantiteRealise = quantiteRealiseTache,
                QuantiteUnite = objectifFlashTache.Unite?.Code,
                RendementObjectif = (ressourceRepartition?.QuantiteObjectif ?? 0) == 0 ? 0 : objectifFlashTache.QuantiteObjectif / ressourceRepartition?.QuantiteObjectif,
                RendementUnite = $"{objectifFlashTache.Unite.Code}/{ressourceRepartition?.Unite}",
                RendementRealise = (ressourceRepartition?.QuantiteRealise ?? 0) == 0 ? 0 : quantiteRealiseTache / ressourceRepartition?.QuantiteRealise,
                BilanFlashRessourcePerimetre = tacheRessourcePerimetre,
                BilanFlashRessourceHorsPerimetre = tacheRessourceHorsPerimetre,
                TotalMontantObjectif = totalMontantObjectif,
                TotalMontantRealise = totalMontantRealise,
                TotalMontantHorsPerimetreObjectif = 0,
                TotalMontantHorsPerimetreRealise = tacheRessources.Where(x => x.HorsPerimetre).Sum(x => x.MontantRealise),
                TotalMontantPerimetreObjectif = totalMontantPerimetreObjectif,
                TotalMontantPerimetreRealise = totalMontantPerimetreRealise,
                CoutUnitaireObjectif = quantiteObjectifTache == 0 ? 0 : (totalMontantObjectif / quantiteObjectifTache),
                CoutUnitaireRealise = quantiteRealiseTache == 0 ? 0 : totalMontantRealise / quantiteRealiseTache,
                CoutUnitaireUnite = $"€/{objectifFlashTache.Unite?.Code}"
            };
        }

        /// <summary>
        /// retourne un nom de feuille excel valide, sans les caractères [ ] * / \ ? et de taille max 31 chars
        /// </summary>
        /// <param name="workSheetName">nom à transformer</param>
        /// <returns>nom de feuille valide</returns>
        private string GetWorkSheetName(string workSheetName)
        {
            workSheetName = workSheetName.Replace("[", string.Empty);
            workSheetName = workSheetName.Replace("]", string.Empty);
            workSheetName = workSheetName.Replace("*", string.Empty);
            workSheetName = workSheetName.Replace("/", string.Empty);
            workSheetName = workSheetName.Replace("\\", string.Empty);
            workSheetName = workSheetName.Replace("?", string.Empty);
            if (workSheetName.Length > 31)
            {
                workSheetName = workSheetName.Substring(0, 31);
            }
            return workSheetName;
        }

        /// <summary>
        /// Groupe la collection de ressource sur le Libellé et l'Unité
        /// </summary>
        /// <param name="ressources">ressource</param>
        /// <returns>Ressources de bilan flash groupées</returns>
        private List<BilanFlashTacheRessourceExportModel> GroupRessources(IEnumerable<BilanFlashTacheRessourceExportModel> ressources)
        {
            return ressources.GroupBy(x => new { x.RessourceLibelle, x.Unite })
                            .Select(x => new BilanFlashTacheRessourceExportModel
                            {
                                RessourceLibelle = x.First().RessourceLibelle,
                                Unite = x.First().Unite,
                                QuantiteObjectif = x.Sum(y => y.QuantiteObjectif),
                                QuantiteRealise = x.Sum(y => y.QuantiteRealise)
                            }).ToList();
        }

        /// <summary>
        /// retourne la quantité objectif de la tache d'objectif
        /// </summary>
        /// <param name="objectifFlashTache">tache d'objectif</param>
        /// <param name="dateDebut">date de début</param>
        /// <param name="dateFin">date de fin</param>
        /// <returns>quantité objectif</returns>
        private decimal? GetQuantiteObjectifTache(ObjectifFlashTacheEnt objectifFlashTache, DateTime dateDebut, DateTime dateFin)
        {
            // gestion des journalisations de tache
            var quantiteObjectif = objectifFlashTache.QuantiteObjectif;
            if (objectifFlashTache.TacheJournalisations.Any())
            {
                quantiteObjectif = objectifFlashTache.TacheJournalisations.Where(x => x.DateJournalisation >= dateDebut
                                                                                    && x.DateJournalisation <= dateFin)
                                                                                    .Sum(x => x.QuantiteObjectif);
            }
            return quantiteObjectif;
        }

        /// <summary>
        /// retourne la quantité objectif de la tache d'objectif
        /// </summary>
        /// <param name="objectifFlashTacheRessource">tache d'objectif</param>
        /// <param name="dateDebut">date de début</param>
        /// <param name="dateFin">date de fin</param>
        /// <returns>quantité objectif</returns>
        private decimal? GetQuantiteObjectifRessource(ObjectifFlashTacheRessourceEnt objectifFlashTacheRessource, DateTime dateDebut, DateTime dateFin)
        {
            // gestion des journalisations de tache
            var quantiteObjectif = objectifFlashTacheRessource.QuantiteObjectif;
            if (objectifFlashTacheRessource.TacheRessourceJournalisations.Any())
            {
                quantiteObjectif = objectifFlashTacheRessource.TacheRessourceJournalisations.Where(x => x.DateJournalisation >= dateDebut
                                                                                    && x.DateJournalisation <= dateFin)
                                                                                    .Sum(x => x.QuantiteObjectif);
            }
            return quantiteObjectif;
        }


        /// <summary>
        /// retourne la liste des ressources de bilan flash
        /// </summary>
        /// <param name="objectifFlash">objectif flash</param>
        /// <param name="dateDebut">date de début</param>
        /// <param name="dateFin">date de fin</param>
        /// <returns>Liste de ressources de bilan flash</returns>
        private async Task<List<BilanFlashTacheRessourceExportModel>> GetBilanFlashExportResourcesAsync(ObjectifFlashEnt objectifFlash, DateTime dateDebut, DateTime dateFin)
        {

            var depenses = await objectifFlashManager.GetObjectifFlashDepensesAsync(objectifFlash, dateDebut, dateFin).ConfigureAwait(false);
            var depenseRessourceGroupList = depenses
                                            .GroupBy(d => new { d.TacheId, d.RessourceId })
                                            .Select(d => new
                                            {
                                                d.Key.TacheId,
                                                d.Key.RessourceId,
                                                d.First().Ressource,
                                                d.First().Unite,
                                                QuantiteRealise = d.Sum(x => x.Quantite),
                                                MontantRealise = d.Sum(x => x.MontantHT)
                                            }).ToList();


            var bilanFlashRessources = new List<BilanFlashTacheRessourceExportModel>();

            foreach (var objectifFlashTache in objectifFlash.Taches)
            {
                foreach (var objectifFlashTacheRessource in objectifFlashTache.Ressources)
                {
                    var depenseRessourceGroup = depenseRessourceGroupList.FirstOrDefault(x => x.RessourceId == objectifFlashTacheRessource.RessourceId
                                                                                      && x.TacheId == objectifFlashTache.TacheId);
                    var exportRessource = new BilanFlashTacheRessourceExportModel
                    {
                        RessourceId = objectifFlashTacheRessource.RessourceId,
                        TacheId = objectifFlashTache.TacheId,
                        RessourceLibelle = objectifFlashTacheRessource.IsRepartitionKey ? $"{objectifFlashTacheRessource.Ressource.Libelle}*" :
                                                                                             objectifFlashTacheRessource.Ressource.Libelle,
                        Unite = objectifFlashTacheRessource.Unite.Code,
                        PuHT = objectifFlashTacheRessource.PuHT,
                        IsRepartitionKey = objectifFlashTacheRessource.IsRepartitionKey,
                        QuantiteObjectif = GetQuantiteObjectifRessource(objectifFlashTacheRessource, dateDebut, dateFin),
                        QuantiteRealise = depenseRessourceGroup?.QuantiteRealise,
                        MontantRealise = depenseRessourceGroup?.MontantRealise ?? 0,
                        HorsPerimetre = false,
                    };

                    // journalisation
                    if (objectifFlashTacheRessource.TacheRessourceJournalisations.Any())
                    {
                        exportRessource.QuantiteObjectif = objectifFlashTacheRessource.TacheRessourceJournalisations
                                                        .Where(x => x.DateJournalisation >= dateDebut.Date
                                                                 && x.DateJournalisation <= dateFin.Date).Sum(x => x.QuantiteObjectif);
                    }
                    bilanFlashRessources.Add(exportRessource);
                }
            }


            // ajout des ressources de dépenses/valo qui ne sont pas présentes dans l'objectif flash (hors périmetre)
            foreach (var depenseRessourceGroup in depenseRessourceGroupList.Where(x => !bilanFlashRessources.Any(y => y.RessourceId == x.RessourceId
                                                                                                        && y.TacheId == x.TacheId)))
            {
                var bilanFlashRessource = new BilanFlashTacheRessourceExportModel
                {
                    TacheId = depenseRessourceGroup.TacheId,
                    RessourceId = depenseRessourceGroup.RessourceId,
                    RessourceLibelle = depenseRessourceGroup.Ressource.Libelle,
                    QuantiteObjectif = 0,
                    Unite = depenseRessourceGroup.Unite,
                    IsRepartitionKey = false,
                    HorsPerimetre = true,
                    QuantiteRealise = depenseRessourceGroup.QuantiteRealise,
                    MontantRealise = depenseRessourceGroup.MontantRealise
                };
                bilanFlashRessources.Add(bilanFlashRessource);
            }

            return bilanFlashRessources;
        }

        private void CheckBilanFlashCriteria(int objectifFlashId, ObjectifFlashEnt objectifFlash, DateTime startDate, DateTime endDate)
        {
            // dates invalides
            if (startDate.Date > endDate.Date)
            {
                throw new ValidationException(
                  new List<ValidationFailure> { new ValidationFailure("datesFilter", FeatureObjectifFlash.ObjectifFlash_Error_DateDebutPosterieureDateFin) });
            }

            // objectif flash non trouvé
            if (objectifFlash == null || objectifFlash.DateSuppression.HasValue)
            {
                throw new ValidationException(
                  new List<ValidationFailure> { new ValidationFailure("objectifFlashId", string.Format(FeatureObjectifFlash.ObjectifFlash_Error_ObjectifIdNotFound, objectifFlashId)) });
            }

            // objectif flash 
            if (endDate.Subtract(startDate).Days > 1 && startDate < objectifFlash.DateDebut.Date)
            {
                throw new ValidationException(
                  new List<ValidationFailure> { new ValidationFailure("startDateTooOld", string.Format(FeatureObjectifFlash.ObjectifFlash_Error_StartDateTooOld, objectifFlashId)) });
            }

        }
    }
}
