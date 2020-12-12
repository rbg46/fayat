using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Societe;
using Fred.Web.Shared.Models;

namespace Fred.Web.API
{
    public class TypeSocieteController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITypeSocieteManager typeSocieteManager;

        public TypeSocieteController(
            IMapper mapper,
            ITypeSocieteManager typeSocieteManager)
        {
            this.mapper = mapper;
            this.typeSocieteManager = typeSocieteManager;
        }

        /// <summary>
        /// Récupération de la liste des Types de sociétés
        /// </summary>
        /// <returns>Liste de types de sociétés</returns>
        [HttpGet]
        [Route("api/TypeSociete")]
        public HttpResponseMessage GetAll()
        {
            return Get(() => mapper.Map<List<TypeSocieteModel>>(typeSocieteManager.GetAll()));
        }

        /// <summary>
        /// Récupération d'un Type de société en fonction de son code
        /// </summary>
        /// <param name="code">Code type société</param>
        /// <returns>Type société</returns>
        [HttpGet]
        [Route("api/TypeSociete/{code?}")]
        public HttpResponseMessage GetByCode(string code = null)
        {
            return Get(() => mapper.Map<TypeSocieteModel>(typeSocieteManager.GetByCode(code)));
        }
    }
}
