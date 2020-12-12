using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Logger;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Sap
{
    /// <summary>
    /// Envoie a SAP les Cis
    /// </summary>
    public interface IPersonnelSapSender : IService
    {
        /// <summary>
        /// Mappe et Envoie a SAP les Cis
        /// </summary>
        /// <typeparam name="T">Type d'input</typeparam>
        /// <param name="logger">logger</param>
        /// <param name="context">context</param>
        /// <param name="societesContexts">societesContexts</param>
        Task MapAndSendToSapAsync<T>(PersonnelImportExportLogger logger, ImportPersonnelContext<T> context, List<ImportPersonnelSocieteContext> societesContexts) where T : class;
    }
}
