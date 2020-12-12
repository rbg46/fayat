using System.Collections.Generic;
using Fred.Business.Reception.Models;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Service qui permet de savoir quelles sont les receptions visables
    /// </summary>
    public interface IVisableReceptionProviderService : IService
    {
        /// <summary>
        /// Retourne les reception qui ne sont pas encore visées par rapport a une liste de reception passé en parametre
        /// Fait un appel a la base
        /// </summary>
        /// <param name="receptionsIds">reception (ids) dont on cherche a savoir si elles sont visables</param>
        /// <returns>Liste de reception non visées</returns>
        ReceptionVisablesResponse GetReceptionsVisables(List<int> receptionsIds);

        /// <summary>
        /// Indique si la réception est encore visable
        /// </summary>
        /// <param name="depense">Dépense</param>
        /// <returns>true si la reception est visable, false sinon</returns>
        bool IsVisable(DepenseAchatEnt depense);

        List<int> GetReceptionsVisablesIds(List<int> receptionsIds);
    }
}
