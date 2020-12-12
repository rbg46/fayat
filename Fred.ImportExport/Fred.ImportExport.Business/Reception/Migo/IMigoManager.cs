using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Models;
using Fred.Entities.Models.Flux.Depense;

namespace Fred.ImportExport.Business.Reception.Migo
{
    /// <summary>
    /// Manager pour les Migos
    /// </summary>
    public interface IMigoManager
    {

        /// <summary>
        /// Execute l'envoie des Migo vers Sap, en splittant par societe
        /// </summary>
        /// <param name="receptionIds">Liste des receptions a envoyer</param>
        /// <returns>Un job par societe, donc un jobid avec les receptions de la societe</returns>
        List<ResultModel<DepenseFluxResponseModel>> ManageExportReceptionToSap(List<int> receptionIds);
    }
}
