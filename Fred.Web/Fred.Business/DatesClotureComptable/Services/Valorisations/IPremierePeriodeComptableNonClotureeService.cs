using System;
using System.Collections.Generic;
using Fred.Entities.DatesClotureComptable;
namespace Fred.Business.DatesClotureComptable.Services.Valorisations
{
    public interface IPremierePeriodeComptableNonClotureeService
    {
        List<CiDernierePeriodeComptableNonCloturee> GetPremierePeriodeComptableNonCloturees(List<int> ciIds, DateTime periode);
        DateTime GetPremierePeriodeComptableNonClotureeByCiId(List<CiDernierePeriodeComptableNonCloturee> premierePeriodeComptableNonCloturees, int ciId);
    }
}