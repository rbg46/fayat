using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Societe;
using Fred.Web.Shared.Models;

namespace Fred.Web.API
{
    public class TypeParticipationSepController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITypeParticipationSepManager typeParticipationSepManager;

        public TypeParticipationSepController(
            IMapper mapper,
            ITypeParticipationSepManager typeParticipationSepManager)
        {
            this.mapper = mapper;
            this.typeParticipationSepManager = typeParticipationSepManager;
        }

        /// <summary>
        /// Récupération de la liste des Types de participation SEP
        /// </summary>
        /// <returns>Liste de types de participation SEP</returns>
        [HttpGet]
        [Route("api/TypeParticipationSep")]
        public HttpResponseMessage GetAll()
        {
            return Get(() => mapper.Map<List<TypeParticipationSepModel>>(typeParticipationSepManager.GetAll()));
        }
    }
}
