using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Depense;
using Fred.Entities.Models;
using Fred.Entities.Models.Flux.Depense;

namespace Fred.Business.ExternalService
{
    /// <summary>
    /// Définit un gestionnaire externe des réceptions
    /// </summary>
    public interface IReceptionManagerExterne : IManagerExterne
    {
        /// <summary>
        ///   Envoi des réceptions à SAP
        /// </summary>
        /// <param name="receptionIds">Liste des identifiants des réceptions à envoyer à SAP</param>
        /// <returns>Liste de résultats</returns>
        Task<List<ResultModel<DepenseFluxResponseModel>>> ExportReceptionListToSapAsync(IEnumerable<int> receptionIds);

        /// <summary>
        ///   Envoi des réceptions à SAP
        /// </summary>
        /// <param name="receptions">Liste des réceptions à envoyer à SAP</param>
        Task ExportReceptionListToSapAsync(List<DepenseAchatEnt> receptions);
    }
}
