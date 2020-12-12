using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Business;

namespace Fred.ImportExport.Business.Personnel
{
    /// <summary>
    /// Gere l'export personnel Fes depuis l'interface Fred IE web vers Tibco
    /// </summary>
    public interface IPersonnelFluxTibcoSystemService : IService
    {
        /// <summary>
        /// Importe depuis le code flux
        /// </summary>
        /// <param name="byPassDate">indique s'il prend ou ignore la dernière date d'execution</param>
        /// <param name="lastExecutionDate">DateTime dernière date d'execution du flux</param>
        void ExportPersonnelToTibco(bool byPassDate, DateTime? lastExecutionDate);
    }
}
