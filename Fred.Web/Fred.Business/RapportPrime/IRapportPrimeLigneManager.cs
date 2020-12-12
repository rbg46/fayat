using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Rapport;
using Fred.Entities.RapportPrime;
using Fred.Web.Shared.Models.RapportPrime;
using Fred.Web.Shared.Models.RapportPrime.Update;

namespace Fred.Business.RapportPrime
{
    public interface IRapportPrimeLigneManager : IManager<RapportPrimeLigneEnt>
    {
        IEnumerable<RapportPrimeLigneEnt> GetListeRapportPrimeLigneByMonth(int year, int month, TypeFiltreEtatPaie typeFiltre, int organisationId, bool tri, int? personnelId);
        IEnumerable<RapportPrimeLigneEnt> SearchRapportPrimeLigneReelWithFilter(DateTime dateRapportPrimeLigneMin, DateTime dateRapportPrimeLigneMax, int? personnelId);
        bool IsLineValidated(RapportPrimeLigneEnt rapportPrimeLigne);
        Task AddLinesAsync(int rapportPrimeId, List<RapportPrimeLigneUpdateModel> lines, int userId);
        Task UpdateLinesAsync(int rapportPrimeId, List<RapportPrimeLigneUpdateModel> lines, int userId);
        Task DeleteLinesAsync(List<int> lines, int userId);
    }
}
