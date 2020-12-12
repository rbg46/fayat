using Fred.Entities.Depense;
using System.Collections.Generic;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Fournie les DateTransfertFar pour l'affichage des receptions 
    /// </summary>
    public interface IDateTransfertFarProviderService : IService
    {
        /// <summary>
        /// Met les date Transfert Far dans les receptions, elle est issue des datecloturecomptables
        /// </summary>
        /// <param name="receptions">receptions</param>
        /// <param name="year">year</param>
        /// <param name="month">month</param>
        void SetDateTransfertFarOfReceptions(List<DepenseAchatEnt> receptions, int year, int month);
    }
}
