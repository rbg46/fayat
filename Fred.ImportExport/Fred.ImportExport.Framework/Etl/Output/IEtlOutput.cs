using System.Threading.Tasks;
using Fred.ImportExport.Framework.Etl.Result;

namespace Fred.ImportExport.Framework.Etl.Output
{

    /// <summary>
    /// Représente un processus pour éxécuter le code de sortie de l'Etl
    /// </summary>
    /// <typeparam name="TR">Result</typeparam>
    public interface IEtlOutput<TR>
    {
        /// <summary>
        /// Appelé par l'ETL
        /// Exécution du code de sortie
        /// </summary>
        /// <param name="result">Result</param>
        Task ExecuteAsync(IEtlResult<TR> result);
    }
}