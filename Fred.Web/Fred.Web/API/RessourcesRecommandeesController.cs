using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.ReferentielEtendu;
using Fred.Business.RessourceRecommandee;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Shared.Models.RessourceRecommandee;

namespace Fred.Web.API
{
    public class RessourcesRecommandeesController : ApiControllerBase
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// The referentiel etendu MGR
        /// </summary>
        private readonly IReferentielEtenduManager referentielEtenduMgr;

        /// <summary>
        /// The ressource recommandee manager
        /// </summary>
        private readonly IRessourceRecommandeeManager ressourceRecommandeeManager;


        /// <summary>
        /// Initializes a new instance of the <see cref="RessourcesRecommandeesController"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="referentielEtenduMgr">The referentiel etendu MGR.</param>
        /// <param name="ressourceRecommandeeManager">The ressource recommandee manager.</param>
        public RessourcesRecommandeesController(IMapper mapper, IReferentielEtenduManager referentielEtenduMgr, IRessourceRecommandeeManager ressourceRecommandeeManager)
        {
            this.mapper = mapper;
            this.referentielEtenduMgr = referentielEtenduMgr;
            this.ressourceRecommandeeManager = ressourceRecommandeeManager;
        }


        /// <summary>
        /// Gets all referentiel recommandee as chapitre list.
        /// </summary>
        /// <param name="societeId">The societe identifier.</param>
        /// <param name="organisationId">The organisation identifier.</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/RessourcesRecommandees/{societeId?}/{organisationId?}")]
        public HttpResponseMessage GetAllReferentielRecommandeeAsChapitreList(int societeId, int organisationId)
        {
            var chapitres = mapper.Map<List<ChapitreModel>>(referentielEtenduMgr.GetAllReferentielEtenduRecommandeAsChapitreList(societeId));

            chapitres.ForEach(x =>
            {
                x.SousChapitres.ToList().ForEach(m =>
                {
                    m.Ressources.ToList().ForEach(r =>
                    {
                        bool isRecommandee = false;
                        r.ReferentielEtendus.ToList().ForEach(re =>
                        {
                            re.RessourcesRecommandees = re.RessourcesRecommandees.Where(rc => rc.OrganisationId == organisationId).ToList();
                            if (re.RessourcesRecommandees.Any())
                            {
                                isRecommandee = true;
                            }
                        });
                        r.IsRecommandee = isRecommandee;
                    });
                });
            });

            return Get(() => chapitres);

        }

        /// <summary>
        /// Saves the specified ressources recommandees list.
        /// </summary>
        /// <param name="ressourcesRecommandeesList">The ressources recommandees list.</param>
        /// <returns>Reponse HTTP</returns>
        [HttpPost]
        [Route("api/RessourcesRecommandees/Save")]
        public HttpResponseMessage Save(IEnumerable<RessourceRecommandeeModel> ressourcesRecommandeesList)
        {
            return Post(() => this.ressourceRecommandeeManager.AddOrUpdateRessourcesRecommandeeList(ressourcesRecommandeesList.ToList()));
        }
    }
}
