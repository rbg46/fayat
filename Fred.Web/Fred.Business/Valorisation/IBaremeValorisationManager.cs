using System;
using Fred.Entities.Valorisation;

namespace Fred.Business.Valorisation
{
    public interface IBaremeValorisationManager : IManager<ValorisationEnt>
    {
        void NewValorisationJobBareme(int objectId, Action<int, DateTime, DateTime> procedureToExecute, DateTime startPeriode, DateTime baremePeriode);

        void UpdateValorisationFromBaremeCI(int ciId, DateTime startPeriode, DateTime baremePeriode);
    }
}