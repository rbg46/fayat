using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.Entities.Valorisation;
namespace Fred.Business.Valorisation.Services
{
    public interface IValorisationVerrouPeriodesService
    {
        List<RapportRapportLigneVerrouPeriode> GetVerrouPeriodesList(List<RapportLigneEnt> rapportLignes);
        bool GetVerrouPeriodeTrueValorisation(List<RapportRapportLigneVerrouPeriode> rapportRapportLigneVerrouPeriodes, int rapportId, int rapportLigneId);
    }
}