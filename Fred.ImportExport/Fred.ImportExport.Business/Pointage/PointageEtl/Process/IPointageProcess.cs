using System.Threading.Tasks;
using Fred.ImportExport.Business.Pointage.PointageEtl.Parameter;

namespace Fred.ImportExport.Business.Pointage.PointageEtl.Process
{
    /// <summary>
    /// Interface Pour le pointage process
    /// Cela permet a la factory de retourner toujours le meme type
    /// </summary>
    public interface IPointageProcess
    {
        void Init(EtlPointageParameter parameter);

        void Build();

        Task ExecuteAsync();

    }
}
