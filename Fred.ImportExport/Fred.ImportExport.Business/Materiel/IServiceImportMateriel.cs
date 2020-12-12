using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Framework.Services;

namespace Fred.ImportExport.Business.Materiel
{
    public interface IServiceImportMateriel
    {
        /// <summary>
        /// Permet d'importer les materiels depuis STORM/BRIDGE.
        /// </summary>
        /// <param name="date">La date de modification. Format : yyyy-MM-dd</param>
        /// <param name="restClient">Le Rest qui permet l'appel a SAP</param>
        /// <param name="webApiStormUrl">La base de l URL de SAP</param>
        /// <param name="importJobId">LE Job ID </param>
        /// <param name="codeSocieteComptables">Les Codes des societes comptables</param>
        Task ImportMaterielFromStormAsync(string date, RestClient restClient, string webApiStormUrl, string importJobId, List<string> codeSocieteComptables);
    }
}
