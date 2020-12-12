using System;
using System.Threading.Tasks;
using AutoMapper;
using Fred.ImportExport.Business.CI.ImportMoyenEtl.Process;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Common;
using Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Transform;
using Fred.ImportExport.DataAccess.ExternalService;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Moyen;
using Fred.ImportExport.Models.Moyen;

namespace Fred.ImportExport.Business.Moyen
{
    public class MoyenFluxManager : AbstractFluxManager
    {
        private readonly IMapper mapper;

        public MoyenFluxManager(IFluxManager fluxManager, IMapper mapper)
            : base(fluxManager)
        {
            this.mapper = mapper;
        }

        public async Task ImportMoyenAsync(MoyenModel moyen)
        {
            var importMaterielProcess = new ImportMoyenProcess();

            importMaterielProcess.Init(moyen);
            importMaterielProcess.Build();
            await importMaterielProcess.ExecuteAsync();
        }

        public async Task<EnvoiPointageMoyenResultModel> ExportPointageMoyenAsync(ExportPointageMoyenModel model)
        {
            try
            {
                var exportMoyenProcess = new ExportPointageMoyenProcess();
                exportMoyenProcess.Init(model);
                exportMoyenProcess.Build();
                await exportMoyenProcess.ExecuteAsync();
                var result = mapper.Map<EnvoiPointageMoyenResultModel>(exportMoyenProcess.PointageMoyenResult);

                return result;
            }
            catch (Exception ex)
            {
                return mapper.Map<EnvoiPointageMoyenResultModel>(new EnvoiPointageMoyenResult(Constantes.TibcoRetourErrorCode, ex.Message));
            }
        }
    }
}
