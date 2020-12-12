using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.RoleFonctionnalite;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données pour les RoleFonctionnalite.
    /// </summary>
    public interface IRoleFonctionnaliteRepository : IRepository<RoleFonctionnaliteEnt>
    {
        /// <summary>
        /// Get role et fonctionnalite by utilisateur id and fonctionnalite libelle
        /// </summary>
        /// <param name="userId">Utilisateur Id</param>
        /// <param name="fonctionnaliteLibelle">Fonctionnalite libelle</param>
        /// <returns>List des roles fonctionnalites</returns>
        Task<List<RoleFonctionnaliteEnt>> GetRoleFonctionnaliteByUserIdAsync(int userId, string fonctionnaliteLibelle);

        Task<List<RoleFonctionnaliteEnt>> GetByUserIdAndListFonctionnaliteAsync(int userId, List<string> fonctionnaliteCodeList);
    }
}
