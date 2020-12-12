using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  /// <summary>
  /// Simule un appel pour générer un challenge de la brique Owin et assurer l'authentification
  /// </summary>
  public class AuthController : ApiController
  {
    // GET api/Auth/Login
    /// <summary>
    /// Challenge le middleware Owin
    /// </summary>
    /// <returns>Retourne "troue" :-) pour l'instant</returns>
    [HttpGet]
    [Route("api/Auth/Login")]
    public string Login()
    {
      return "Ready";
    }

    [Authorize]
    [HttpGet]
    [Route("api/Auth/Test")]
    public string Test()
    {
      return "Ready";
    }
  }
}