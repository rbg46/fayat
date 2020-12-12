using System.Threading.Tasks;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Personnel.Etl.Process;

namespace Fred.ImportExport.Business.Personnel.EtlFactory
{
    public interface IPersonnelEtl
    {
        void Init(PersonnelEtlParameter parameter, IFluxManager flux);

        void Build();

        Task ExecuteAsync();
    }
}
