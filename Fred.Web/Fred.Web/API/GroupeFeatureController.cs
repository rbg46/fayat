using Fred.Business.GroupeFeature.Services;
using System.Web.Http;

namespace Fred.Web.API
{
    public class GroupeFeatureController : ApiControllerBase
    {
        private readonly IGroupeFeatureService groupeFeatureService;

        public GroupeFeatureController(IGroupeFeatureService groupeFeatureService)
        {
            this.groupeFeatureService = groupeFeatureService;
        }

        [HttpGet]
        [Route("api/GroupeFeature")]
        public IHttpActionResult Get()
        {
            var context = groupeFeatureService.GetFeaturesForGroupe();
            return Ok(context);
        }
    }
}
