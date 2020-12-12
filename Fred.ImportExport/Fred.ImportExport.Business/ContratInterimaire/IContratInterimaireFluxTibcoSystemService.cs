using System;
using Fred.Business;

namespace Fred.ImportExport.Business.ContratInterimaire
{
    /// <summary>
    /// Gere l'import des contrats interimaires depuis Tibco vers l'interface Fred IE web
    /// </summary>
    public interface IContratInterimaireFluxTibcoSystemService : IService
    {
        /// <summary>
        /// Importe depuis la derniere date d'execution
        /// </summary>
        /// <param name="lastExecutionDate">DateTime dernière date d'execution du flux</param>
        void ImportContratInterimairePixid(DateTime? lastExecutionDate);
    }
}
