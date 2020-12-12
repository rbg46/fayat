using System.Collections.Generic;
using Fred.Entities.VerificationPointage;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Interface de Cheking pointing 
    /// </summary>
    public interface  IChekingPointingReposity: IMultipleRepository
    {
        /// <summary>
        /// Récupération de liste des rapports Pointage
        /// </summary>
        /// <param name="filtre">Filtre de recherche</param>
        /// <returns>Envoie tous les Donées dont on a besoin</returns>
        IEnumerable<ChekingPointing> GetChekingPointing(FilterChekingPointing filtre);
    }
}
