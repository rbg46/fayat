using AutoMapper;
using Fred.Business.Personnel.Interimaire;
using Fred.Entities.Personnel.Interimaire;
using Fred.Web.Shared.Models.Personnel.Interimaire;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  public class MatriculeExterneController : ApiControllerBase
  {
    private readonly IMatriculeExterneManager matriculeExterneManager;
    private readonly IMapper mapper;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="MatriculeExterneController" />.
    /// </summary>  
    /// <param name="matriculeExterneManager">Manager des matricules externes</param>
    /// <param name="mapper">Mapper Model / ModelVue</param>
    public MatriculeExterneController(IMapper mapper, IMatriculeExterneManager matriculeExterneManager)
    {
      this.matriculeExterneManager = matriculeExterneManager;
      this.mapper = mapper;
    }

    /// <summary>
    /// Méthode GET de récupération d'un contrat d'intérimaire
    /// </summary>
    /// <param name="matriculeExterneId">Identifiant du matricule externe</param>
    /// <returns>Retourne le matricule externe</returns>
    [HttpGet]
    [Route("api/MatriculeExtenre/{matriculeExterneId}")]
    public HttpResponseMessage GetMatriculeExterneById(int matriculeExterneId)
    {
      return Get(() => this.mapper.Map<MatriculeExterneModel>(this.matriculeExterneManager.GetMatriculeExterneById(matriculeExterneId)));
    }

    /// <summary>
    /// Méthode GET de récupération des matricules externe lié au personnel id
    /// </summary>
    /// <param name="personnelId">Identifiant du personnel intérimaire</param>
    /// <returns>Retourne la liste des matricules externes lié au personnel id</returns>
    [HttpGet]
    [Route("api/MatriculeExterne/Personnel/{personnelId}")]
    public HttpResponseMessage GetMatriculeExterneByPersonnelId(int personnelId)
    {
      return Get(() => this.mapper.Map<IEnumerable<MatriculeExterneModel>>(this.matriculeExterneManager.GetMatriculeExterneByPersonnelId(personnelId)));
    }

    /// <summary>
    /// Méthode GET de récupération si un matricule externe est déjà existant
    /// </summary>
    /// <param name="matriculeExterneModel">matricule externe</param>
    /// <returns>Retourne true ou false si un matricule existe ou non</returns>
    [HttpPost]
    [Route("api/MatriculeExterne/Exist")]
    public HttpResponseMessage GetMatriculeExterneExist(MatriculeExterneModel matriculeExterneModel)
    {
      return Post(() => this.mapper.Map<MatriculeExterneModel>(this.matriculeExterneManager.GetMatriculeExterneExist(this.mapper.Map<MatriculeExterneEnt>(matriculeExterneModel))));
    }
  }
}