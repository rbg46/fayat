using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.RapportPrime;

namespace Fred.DataAccess.Interfaces
{
    public interface IRapportPrimeLigneRepository : IRepository<RapportPrimeLigneEnt>
    {
        IEnumerable<RapportPrimeLigneEnt> GetRapportPrimeLigneVerrouillesByUserId(int userid, int annee, int mois);
        IEnumerable<RapportPrimeLigneEnt> SearchRapportPrimeLigneWithFilter(Func<RapportPrimeLigneEnt, bool> predicateWhere);
        Task<int> GetRapportLignePrimeIdAsync(int primeId);
        Task AddAsync(RapportPrimeLigneEnt rapportPrimeLigne);
        Task<List<RapportPrimeLigneEnt>> GetListAsync(List<int> ids);
        Task<List<RapportPrimeLigneEnt>> GetListWithLinkedPropertiesAsync(IEnumerable<int> ids);
        void UpdateRapportPrimeLigne(RapportPrimeLigneEnt lineToUpdate);
    }
}
