using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI;
using Fred.Business.Societe;
using Fred.Entities.Depense;
using Fred.Framework.Linq;

namespace Fred.Business.Depense
{
    /// <summary>
    /// Service Depense Achat
    /// </summary>
    public class DepenseAchatService : IDepenseAchatService
    {

        private readonly ICIManager ciManager;
        private readonly ISepService sepService;

        /// <summary>
        /// Constructeur DepenseAchatService
        /// </summary>
        /// <param name="ciManager">Gestionnaire CI</param>
        /// <param name="sepService">Service pour société SEP</param>
        public DepenseAchatService(ICIManager ciManager, ISepService sepService)
        {
            this.ciManager = ciManager;
            this.sepService = sepService;
        }

        /// <summary>
        /// Récupération et affectation de la nature de chaque dépense de la liste depenseAchats
        /// </summary>
        /// <param name="depenseAchats">Liste de dépenses achat</param>
        /// <returns>Liste de dépenses achat avec Nature Analytique</returns>
        public List<DepenseAchatEnt> ComputeNature(IEnumerable<DepenseAchatEnt> depenseAchats)
        {
            Dictionary<int, Tuple<int?, int?>> ciIdSocieteIdSocieteGeranteId = new Dictionary<int, Tuple<int?, int?>>();
            int? societeGeranteId = null;

            foreach (int ciId in depenseAchats.Where(x => x.CiId.HasValue).Select(x => x.CiId.Value).Distinct().ToList())
            {
                var societe = ciManager.GetSocieteByCIId(ciId, true);

                if (sepService.IsSep(societe))
                {
                    societeGeranteId = sepService.GetSocieteGerante(societe.SocieteId)?.SocieteId;
                }

                ciIdSocieteIdSocieteGeranteId.Add(ciId, Tuple.Create<int?, int?>(societe.SocieteId, societeGeranteId));
            }

            depenseAchats?.ForEach(depense =>
            {
                int? selectedSocieteId = null;

                if (depense.CiId.HasValue)
                {
                    selectedSocieteId = ciIdSocieteIdSocieteGeranteId[depense.CiId.Value].Item2 ?? ciIdSocieteIdSocieteGeranteId[depense.CiId.Value].Item1;
                }

                if (selectedSocieteId.HasValue)
                {
                    depense.Nature = depense.Ressource?.ReferentielEtendus?.FirstOrDefault(x => x.SocieteId == selectedSocieteId.Value)?.Nature;
                }
            });

            return depenseAchats.ToList();
        }
    }
}
