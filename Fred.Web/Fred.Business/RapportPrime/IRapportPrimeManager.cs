using Fred.Entities.RapportPrime;
using System;
using System.Threading.Tasks;
using Fred.Web.Shared.Models.RapportPrime.Get;
using Fred.Web.Shared.Models.RapportPrime.Update;

namespace Fred.Business.RapportPrime
{
    public interface IRapportPrimeManager : IManager<RapportPrimeEnt>
    {
        Task<RapportPrimeGetModel> GetAsync(DateTime date);

        Task<RapportPrimeEnt> AddAsync();

        RapportPrimeEnt GetRapportPrime(DateTime dateRapportPrime, int utilisateurId);

        Task UpdateAsync(int rapportPrimeId, RapportPrimeUpdateModel rapportPrimeModel);
    }
}
