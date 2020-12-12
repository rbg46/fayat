using AutoMapper;
using Fred.Business;
using Fred.Web.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  public class AuthentificationLogController : ApiControllerBase
  {
    /// <summary>
    /// Manageur des AuthentificationLog.
    /// <seealso cref="authentificationLogManager"/>
    /// </summary>
    protected readonly IAuthentificationLogManager authentificationLogManager;

    /// <summary>
    /// Auto Mapper modèle / entité.
    /// <seealso cref="IMapper"/>
    /// </summary>
    protected readonly IMapper mapper;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="BudgetController"/>.
    /// </summary>
    /// <param name="authentificationLogManager">Manageur des budgets</param>
    /// <param name="mapper">Auto Mapper modèle / entité</param>
    public AuthentificationLogController(IAuthentificationLogManager authentificationLogManager, IMapper mapper)
    {
      this.authentificationLogManager = authentificationLogManager;
      this.mapper = mapper;
    }

    /// <summary>
    /// GET api/AuthentificationLogController/GetByLogin
    /// </summary>   
    /// <param name="login">login</param>
    /// <param name="skip">skip</param>
    /// <param name="take">take</param>
    /// <returns>Une liste d'AuthentificationLog </returns>
    [HttpGet]
    [Route("api/AuthentificationLog/GetByLogin")]
    public HttpResponseMessage GetByLogin(string login, int skip, int take)
    {
      return Get(() =>
      {
        var entities = this.authentificationLogManager.GetByLogin(login, skip, take);
        return mapper.Map<IEnumerable<AuthentificationLogListModel>>(entities);
      });
    }

    /// <summary>
    /// Obtient le detail du log
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>HttpResponseMessage</returns>
    [HttpGet]
    [Route("api/AuthentificationLog/GetDetail/{id}")]
    public HttpResponseMessage GetDetail(int id)
    {
      return Get(() =>
      {
        var result = this.authentificationLogManager.GetById(id);
        return mapper.Map<AuthentificationLogDetailModel>(result);
      });
    }

    /// <summary>
    /// Supprime une liste de logs
    /// </summary>
    /// <param name="authentificationLogIds">authentificationLogIds</param>
    /// <returns>HttpResponseMessage</returns>
    [HttpDelete]
    [Route("api/AuthentificationLog/DeleteAuthentificationLogs")]
    public HttpResponseMessage DeleteAuthentificationLogs(AuthentificationLogDeletedModel authentificationLogIds)
    {
      return Delete(() => this.authentificationLogManager.DeleteAuthentificationLogs(authentificationLogIds.Ids));
    }

  }
}
