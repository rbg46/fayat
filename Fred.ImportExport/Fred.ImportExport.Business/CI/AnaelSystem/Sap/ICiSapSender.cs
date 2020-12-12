using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Sap
{
    /// <summary>
    /// Envoie a SAP les Cis
    /// </summary>
    public interface ICiSapSender : IService
    {
        /// <summary>
        /// Mappe et Envoie a SAP les Cis
        /// </summary>
        /// <typeparam name="T">Type d'input</typeparam>
        /// <param name="logger">logger</param>
        /// <param name="context">context</param>
        /// <param name="societesContexts">societesContexts</param>
        Task MapAndSendToSapAsync<T>(CiImportExportLogger logger, ImportCiContext<T> context, List<ImportCiSocieteContext> societesContexts) where T : class;
    }
}
