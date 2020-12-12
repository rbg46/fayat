using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.RapportPrime;
using Fred.Web.Shared.Models.RapportPrime.Get;

namespace Fred.DataAccess.Interfaces
{
    public interface IRapportPrimeRepository : IRepository<RapportPrimeEnt>
    {
        RapportPrimeEnt GetRapportPrimeByDate(DateTime dateRapport, List<int> listCiId);
        Task<RapportPrimeGetModel> GetRapportPrimeByDateAsync(DateTime dateRapport, List<int> listCiId);
        Task<bool> RapportPrimeExistsAsync(DateTime date);
        Task AddAsync(RapportPrimeEnt rapportPrime);
    }
}
