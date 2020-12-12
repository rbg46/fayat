using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.Societe;
using Fred.Entities.Depense;
using Fred.Entities.Organisation.Tree;

namespace Fred.Business.Reception.Services
{
    public class ReceptionNatureProviderService : IReceptionNatureProviderService
    {
        private readonly ISepService sepService;
        private readonly IOrganisationTreeService organisationTreeService;

        public ReceptionNatureProviderService(
            ISepService sepService,
            IOrganisationTreeService organisationTreeService)
        {
            this.sepService = sepService;
            this.organisationTreeService = organisationTreeService;
        }

        /// <summary>
        /// Initialize le champs Nature d'un ensemble de receptions
        /// </summary>
        /// <param name="depenseAchats">Les receptions</param>
        public void SetNatureOfReceptions(List<DepenseAchatEnt> depenseAchats)
        {
            var receptions = depenseAchats.Where(x => x.CiId.HasValue).ToList();

            List<CiSocieteSocieteGeranteMapping> ciSocieteSocieteGeranteMappings = SearchSociete(receptions);

            MapNatureWithSocieteData(receptions, ciSocieteSocieteGeranteMappings);
        }

        private static void MapNatureWithSocieteData(List<DepenseAchatEnt> receptions, List<CiSocieteSocieteGeranteMapping> ciSocieteSocieteGeranteMappings)
        {
            foreach (var reception in receptions)
            {
                var mapping = ciSocieteSocieteGeranteMappings.First(x => x.CiId == reception.CiId);

                var societeIdOfCi = mapping.SocieteGeranteId ?? mapping.SocieteId;

                reception.Nature = reception.Ressource?.ReferentielEtendus?.FirstOrDefault(x => x.SocieteId == societeIdOfCi)?.Nature;
            }
        }
        /// <summary>
        /// Recherche de la societe, elle est peu etre un Sep donc il  faut recupere la societe associée de type Gerante
        /// </summary>
        /// <param name="receptions">receptions</param>
        /// <returns>Le mapping Ci(de la reception) - societeId(du ci de la reception) - SocieteId(gerante)</returns>
        private List<CiSocieteSocieteGeranteMapping> SearchSociete(List<DepenseAchatEnt> receptions)
        {
            if (receptions.Count == 0)
            {
                return new List<CiSocieteSocieteGeranteMapping>();
            }

            var ciIdsOfReceptions = receptions.Select(x => x.CiId.Value).Distinct().ToList();

            var organisationTree = organisationTreeService.GetOrganisationTree();

            var ciSocieteSocieteGeranteMappings = new List<CiSocieteSocieteGeranteMapping>();

            foreach (int ciId in ciIdsOfReceptions)
            {
                OrganisationBase societe = organisationTree.GetSocieteParentOfCi(ciId);

                var ciSocieteSocieteGeranteMapping = new CiSocieteSocieteGeranteMapping();

                ciSocieteSocieteGeranteMapping.CiId = ciId;

                ciSocieteSocieteGeranteMapping.SocieteId = societe.Id;

                ciSocieteSocieteGeranteMapping.SocieteGeranteId = null;//determiner un peu plus tard

                ciSocieteSocieteGeranteMappings.Add(ciSocieteSocieteGeranteMapping);

            }

            var societesParentesIds = ciSocieteSocieteGeranteMappings.Select(x => x.SocieteId).ToList();

            var societesSep = sepService.GetSocietesThatAreSep(societesParentesIds);

            var idsOfSocietesSep = societesSep.Select(x => x.SocieteId).ToList();

            var societeGerantesDictionnary = sepService.GetSocieteGerantes(idsOfSocietesSep);

            foreach (var ciSocieteSocieteGeranteMapping in ciSocieteSocieteGeranteMappings)
            {
                var societeId = ciSocieteSocieteGeranteMapping.SocieteId;

                var isSep = idsOfSocietesSep.Contains(societeId);

                if (isSep)
                {
                    ciSocieteSocieteGeranteMapping.SocieteGeranteId = societeGerantesDictionnary[societeId].SocieteId;
                }
            }

            return ciSocieteSocieteGeranteMappings;
        }
    }
}
