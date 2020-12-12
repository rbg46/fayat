using System;
using System.Collections.Generic;
using Fred.Entities.Depense;

namespace Fred.Business.Achat.Calculation
{
    public interface ISoldeFarService
    {
        List<int> SelectReceptionsWithSoldeFar(List<int> receptionsMustBeFiltered, DateTime? dateFrom, DateTime? dateTo);
        decimal CalculateSoldeFarTotal(List<int> receptionsMustBeCalculated, DateTime? dateFrom, DateTime? dateTo);
        void GetAndMapSoldeFar(List<DepenseAchatEnt> receptionsMustBeMappedWithSoldeFar, DateTime? dateDebut, DateTime? dateFin);
    }
}
