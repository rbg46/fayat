using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Sap
{
    /// <summary>
    /// Envoie a SAP les Fournisseurs
    /// </summary>
    public interface IFournisseurSapSender : IService
    {
        /// <summary>
        /// Mappe et Envoie a SAP les Cis
        /// </summary>
        /// <typeparam name="T">Type d'input</typeparam>
        /// <param name="logger">logger</param>
        /// <param name="context">context</param>
        /// <param name="societesContexts">societesContexts</param>
        Task MapAndSendToSapAsync<T>(FournisseurImportExportLogger logger, ImportFournisseurContext<T> context, List<ImportFournisseurSocieteContext> societesContexts) where T : class;
    }
}
