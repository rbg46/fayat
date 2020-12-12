using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.Entities.Valorisation;

namespace Fred.Business.Valorisation.Services
{
    public class ValorisationVerrouPeriodesService : IValorisationVerrouPeriodesService
    {

        private readonly IValorisationRepository valorisationRepository;

        public ValorisationVerrouPeriodesService(IValorisationRepository valorisationRepository)
        {

            this.valorisationRepository = valorisationRepository;
        }
        public List<RapportRapportLigneVerrouPeriode> GetVerrouPeriodesList(List<RapportLigneEnt> rapportLignes)
        {
            return valorisationRepository.GetVerrouPeriodesList(rapportLignes);
        }

        public bool GetVerrouPeriodeTrueValorisation(List<RapportRapportLigneVerrouPeriode> rapportRapportLigneVerrouPeriodes, int rapportId, int rapportLigneId)
        {
            return rapportRapportLigneVerrouPeriodes.Where(x => x.RapportId == rapportId && x.RapportLigneId == rapportLigneId)
                                                    .Any(x => x.VerrouPeriode);
        }
    }

}
