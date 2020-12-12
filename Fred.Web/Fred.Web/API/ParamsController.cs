using Fred.Business.Params;
using Fred.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
    /// <summary>
    ///  Controller Web API gestion des paramétres
    /// </summary>
    public class ParamsController : ApiControllerBase
    {
        private readonly ICacheManager cacheManager;
        private readonly string keySingleParamCache = "single_Parametre_Cache";
        private readonly string keyMultipleParamCache = "multiple_Parametre_Cache";
        private readonly IParamsManager paramsManager;

        public ParamsController(ICacheManager cacheManager, IParamsManager paramsManager)
        {
            this.cacheManager = cacheManager;
            this.paramsManager = paramsManager;
        }

        [HttpGet]
        [Route("api/Params/GetParamValue/{organisationId}/{key}")]
        public HttpResponseMessage GetParamValue(int organisationId, string key)
        {
            string param = null;
            param = cacheManager.GetOrCreate(keySingleParamCache + organisationId + key, () => param, new TimeSpan(1, 0, 0, 0, 0));
            if (string.IsNullOrEmpty(param))
            {
                param = cacheManager.GetOrCreate(
                keySingleParamCache + organisationId + key, () =>
                {
                    return paramsManager.GetParamValue(organisationId, key);
                },
                new TimeSpan(1, 0, 0, 0, 0));
            }
            return Get(() => param);
        }

        [HttpGet]
        [Route("api/Params/GetParamValues/{organisationId}/{key}")]
        public HttpResponseMessage GetParamValues(int organisationId, string key)
        {
            List<string> paramlist = null;
            paramlist = cacheManager.GetOrCreate(keyMultipleParamCache + organisationId + key, () => paramlist, new TimeSpan(1, 0, 0, 0, 0));
            if (paramlist == null)
            {
                paramlist = cacheManager.GetOrCreate(
                keyMultipleParamCache + organisationId + key, () =>
                {
                    return paramsManager.GetParamValues(organisationId, key);
                },
                new TimeSpan(1, 0, 0, 0, 0));
            }
            return Get(() => paramlist);
        }
    }
}