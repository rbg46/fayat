using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.CI;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielFixe;
using Fred.Business.Unite;
using Fred.Framework.Reporting;
using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Shared.Models.OperationDiverse;
using Syncfusion.XlsIO;

namespace Fred.Business.OperationDiverse.Service
{
    public class OperationDiverseExcelService : IOperationDiverseExcelService
    {
        private const string ExcelTemplate = "Templates/TemplateFichierExempleChargementOD.xlsx";
        private readonly IMapper mapper;
        private readonly IFamilleOperationDiverseManager familleOperationDiverseManager;
        private readonly IUniteManager uniteManager;
        private readonly ICIManager cIManager;
        private readonly ITacheManager tacheManager;
        private readonly IReferentielFixeManager referentielFixeManager;

        public OperationDiverseExcelService(IMapper mapper,
                                            IFamilleOperationDiverseManager familleOperationDiverseManager,
                                            IUniteManager uniteManager,
                                            ICIManager ciManager,
                                            ITacheManager tacheManager,
                                            IReferentielFixeManager referentielFixeManager)
        {
            this.mapper = mapper;
            this.familleOperationDiverseManager = familleOperationDiverseManager;
            this.uniteManager = uniteManager;
            this.cIManager = ciManager;
            this.tacheManager = tacheManager;
            this.referentielFixeManager = referentielFixeManager;
        }

        public async Task<byte[]> GetFichierExempleChargementODAsync(int ciId, DateTime dateComptable, string baseDirectory)
        {
            using (var excelFormat = new ExcelFormat())
            {
                try
                {
                    string pathName = baseDirectory + ExcelTemplate;
                    IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathName);
                    ITemplateMarkersProcessor marker = workbook.CreateTemplateMarkersProcessor();

                    await AlimenterDonneesFichierAsync(ciId, dateComptable, marker);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);

                        stream.Seek(0, SeekOrigin.Begin);
                        var bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, (int)stream.Length);
                        workbook.Close();
                        return bytes;
                    }
                }
                catch (Exception e)
                {
                    throw new FileNotFoundException(e.Message);
                }
            }
        }

        private async Task AlimenterDonneesFichierAsync(int ciId, DateTime dateComptable, ITemplateMarkersProcessor marker)
        {
            var ciEnt = cIManager.GetCiById(ciId, true);
            string societeCode = ciEnt?.Societe?.Code;

            AlimenterInformation(dateComptable, marker, ciEnt?.Code, societeCode);

            AlimenterUnite(marker);

            AlimenterFamilleOd(marker, ciEnt);

            AlimenterRessource(marker, ciEnt, societeCode);

            await AlimenterTacheAsync(ciId, marker, societeCode);

            AlimenterCi(marker, ciId);

            marker.ApplyMarkers();
        }

        private static void AlimenterInformation(DateTime dateComptable, ITemplateMarkersProcessor marker, string ciCode, string societeCode)
        {
            marker.AddVariable("SocieteCode", societeCode ?? string.Empty);
            marker.AddVariable("CiCode", ciCode ?? string.Empty);
            marker.AddVariable("DateComptable", dateComptable);
        }

        private void AlimenterCi(ITemplateMarkersProcessor marker, int ciId)
        {
            var ciModel = mapper.Map<IEnumerable<CIModel>>(cIManager.GetCIListBySocieteId(ciId));
            marker.AddVariable(typeof(CIModel).Name, ciModel);
        }

        private async Task AlimenterTacheAsync(int ciId, ITemplateMarkersProcessor marker, string societeCode)
        {
            var tacheModel = mapper.Map<IEnumerable<TacheModel>>(await tacheManager.GetActiveTacheListByCiIdAndNiveauAsync(ciId, 3));
            tacheModel.ToList().ForEach(m => m.SocieteCode = societeCode);
            marker.AddVariable(typeof(TacheModel).Name, tacheModel);
        }

        private void AlimenterRessource(ITemplateMarkersProcessor marker, Entities.CI.CIEnt ciEnt, string societeCode)
        {
            var ressourceModel = mapper.Map<IEnumerable<RessourceModel>>(referentielFixeManager.GetListRessourceBySocieteIdWithSousChapitreEtChapitre(ciEnt?.SocieteId ?? 0, ciEnt?.CiId ?? 0));
            ressourceModel.OrderBy(r => r.Libelle).ToList().ForEach(m => m.SocieteCode = societeCode);
            marker.AddVariable(typeof(RessourceModel).Name, ressourceModel);
        }

        private void AlimenterFamilleOd(ITemplateMarkersProcessor marker, Entities.CI.CIEnt ciEnt)
        {
            var familleOperationDiverseModel = mapper.Map<IEnumerable<FamilleOperationDiverseModel>>(familleOperationDiverseManager.GetFamiliesBySociety(ciEnt?.SocieteId ?? 0));
            marker.AddVariable(typeof(FamilleOperationDiverseModel).Name, familleOperationDiverseModel);
        }

        private void AlimenterUnite(ITemplateMarkersProcessor marker)
        {
            var uniteModel = mapper.Map<IEnumerable<UniteModel>>(uniteManager.SearchLight(string.Empty, 1, int.MaxValue));
            marker.AddVariable(typeof(UniteModel).Name, uniteModel);
        }
    }
}
