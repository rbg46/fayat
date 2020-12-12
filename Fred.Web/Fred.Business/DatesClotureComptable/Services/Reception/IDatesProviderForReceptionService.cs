using Fred.Business.DatesClotureComptable.Reception.Models;
using System.Collections.Generic;

namespace Fred.Business.DatesClotureComptable.Services
{
    /// <summary>
    /// Fournie les dates  necessaires pour l'affichages des receptions
    /// </summary>
    public interface IDatesProviderForReceptionService : IService
    {

        /// <summary>
        /// Permet de savoir si des receptions sont 'bloqué en reception'.        
        /// </summary>
        /// <param name="receptionBlockedResquests">Liste de demande d'information (ciid, year et month) </param>
        /// <returns>Liste de responses pour chaque demande</returns>
        List<ReceptionBlockedResponse> IsBlockedInReception(List<ReceptionBlockedResquest> receptionBlockedResquests);

        /// <summary>
        /// Permet de savoir la prochaine datecomptable disponible pour une reception.        
        /// </summary>
        /// <param name="resquests">Liste de demande d'information (ciid, starDate) </param>
        /// <returns>Liste de responses pour chaque demande</returns>
        List<NextDateUnblockedInReceptionResponse> GetNextDatesUnblockedInReception(List<NextDateUnblockedInReceptionResquest> resquests);

        /// <summary>
        /// Permet de savoir la prochaine DateTransfertFAR pour un ci, une année et un mois.        
        /// </summary>
        /// <param name="resquests">Liste de demande d'information (ciid,  une année et un mois) </param>
        /// <returns>Liste de responses pour chaque demande</returns>
        List<GetDateTransfertFarResponse> GetDateTransfertFars(List<GetDateTransfertFarResquest> resquests);
    }
}
