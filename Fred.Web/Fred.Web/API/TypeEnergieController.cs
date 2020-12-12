using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.CommandeEnergies;
using Fred.Web.Shared.Models;

namespace Fred.Web.API
{
    public class TypeEnergieController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITypeEnergieManager typeEnergieManager;

        public TypeEnergieController(
            IMapper mapper,
            ITypeEnergieManager typeEnergieManager)
        {
            this.mapper = mapper;
            this.typeEnergieManager = typeEnergieManager;
        }

        /// <summary>
        /// Récupération de la liste des Types d'énergies
        /// </summary>
        /// <returns>Liste de types de énergies</returns>
        [HttpGet]
        [Route("api/TypeEnergie")]
        public HttpResponseMessage GetAll()
        {
            return Get(() => mapper.Map<List<TypeEnergieModel>>(typeEnergieManager.GetAll()));
        }

        /// <summary>
        /// Récupération d'un Type d'énergie en fonction de son code
        /// </summary>
        /// <param name="code">Code type énergie</param>
        /// <returns>Type énergie</returns>
        [HttpGet]
        [Route("api/TypeEnergie/{code?}")]
        public HttpResponseMessage GetByCode(string code = null)
        {
            return Get(() => mapper.Map<TypeEnergieModel>(typeEnergieManager.GetByCode(code)));
        }
    }
}
