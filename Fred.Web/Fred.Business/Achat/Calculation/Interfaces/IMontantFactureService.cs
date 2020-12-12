using System;
using System.Collections.Generic;
using Fred.Entities.Depense;
namespace Fred.Business.Achat.Calculation
{
    public interface IMontantFactureService
    {
        void GetAndMapMontantFacture(List<DepenseAchatEnt> receptionsMustBeMappedWithMontantFacture, DateTime? dateDebut, DateTime? dateFin);
    }
}
