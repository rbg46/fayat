using AutoMapper;
using Fred.Business.Referential.Tache;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.Web.Models.Referential;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
    public class ReferentielTachesController : ApiControllerBase
    {
        /// <summary>
        /// Manager business des types de dépenses
        /// </summary>
        protected readonly ITacheManager TacheMgr;

        /// <summary>
        /// Mapper Model / ModelVue
        /// </summary>
        protected readonly IMapper Mapper;

        public ReferentielTachesController(ITacheManager tacheMgr, IMapper mapper)
        {
            TacheMgr = tacheMgr;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("api/ReferentielTaches/{id}")]
        public HttpResponseMessage Get(int id)
        {
            return Get(() =>
            {
                return Mapper.Map<IEnumerable<TacheModel>>(TacheMgr.GetTacheLevel1ByCiId(id));
            });
        }

        [HttpGet]
        [Route("api/ReferentielTaches/GetTache/{id}")]
        public HttpResponseMessage GetTache(int id)
        {
            return Get(() =>
            {
                return Mapper.Map<TacheModel>(TacheMgr.GetTacheById(id));
            });
        }

        [HttpPost]
        [Route("api/ReferentielTaches")]
        public HttpResponseMessage PostTache(TacheModel model)
        {
            try
            {
                return Post(() =>
                {
                    var tacheEnt = Mapper.Map<TacheEnt>(model);
                    TacheMgr.AddTache(tacheEnt);
                    return Mapper.Map<TacheModel>(tacheEnt);
                });
            }
            catch (FredBusinessException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, ex);
            }
        }

        [HttpPut]
        [Route("api/ReferentielTaches")]
        public HttpResponseMessage UpdateTache(TacheModel model)
        {
            return Put(() =>
            {
                var tacheEnt = Mapper.Map<TacheEnt>(model);

                tacheEnt.TachesEnfants.Clear();

                tacheEnt.Parent = null;

                TacheMgr.UpdateTache(tacheEnt);

                TacheMgr.UpdateChildrenActiveStatus(TacheMgr.GetTacheById(tacheEnt.TacheId));

                return Mapper.Map<TacheModel>(tacheEnt);
            });
        }

        [HttpDelete]
        [Route("api/ReferentielTaches/{id}")]
        public HttpResponseMessage DeleteChapitre(int id)
        {
            return Delete(() =>
            {
                TacheMgr.DeleteTacheById(id);
            });
        }

        [HttpGet]
        [Route("api/ReferentielTaches/GetNextTaskCode/{id}")]
        public HttpResponseMessage GetNextTaskCode(int id)
        {
            return Get(() =>
            {
                TacheEnt parentTask = TacheMgr.GetTacheById(id);
                return TacheMgr.GetNextTaskCode(parentTask);
            });
        }

        [HttpPost]
        [Route("api/ReferentielTaches/CopyCI/{source}/{destination}")]
        public HttpResponseMessage CopyCI(int source, int destination)
        {
                return Post(() =>
                {
                    TacheMgr.CopyPlanTache(source, destination);
                });
        }
        [HttpPost]
        [Route("api/ReferentielTaches/CopyTachePartial/{source}/{destination}")]
        public HttpResponseMessage CopyTachePartial(int source, int destination, List<int> listIdTache)
        {
            return Post(() =>
            {
                TacheMgr.CopyTachePartially(destination, source, listIdTache);
            });
        }

        [HttpGet]
        [Route("api/ReferentielTaches/GetActiveTacheByCiAndLevel/{ciId}/{level}")]
        public HttpResponseMessage GetActiveTacheByCiAndLevel(int ciID, int level)
        {
            return Get(() => TacheMgr.GetActiveTacheListByCiIdAndNiveau(ciID, level));
        }

    }
}
