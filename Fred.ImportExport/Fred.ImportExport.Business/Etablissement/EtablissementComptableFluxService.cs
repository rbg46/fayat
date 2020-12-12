using System.Threading.Tasks;
using Fred.ImportExport.Business.Etablissement.Etl.Process;

namespace Fred.ImportExport.Business.Etablissement
{
    public class EtablissementComptableFluxService : IEtablissementComptableFluxService
    {
        private readonly IEtablissementComptableEtlProcess etl;

        public EtablissementComptableFluxService(IEtablissementComptableEtlProcess etl)
        {
            this.etl = etl;
        }

        public async Task ImportEtablissementComptableAsync()
        {
            etl.Build();
            await etl.ExecuteAsync();
        }
    }
}
