using AutoMapper;
using Fred.Business.Personnel.Interimaire;
using Fred.Entities.Personnel.Interimaire;
using Fred.Web.Shared.Models.Personnel.Interimaire;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  public class ZoneDeTravailController : ApiControllerBase
  {
    private readonly IZoneDeTravailManager zoneDeTravailManager;
    private readonly IMapper mapper;


    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="ContratInterimaireController" />.
    /// </summary>  
    /// <param name="zoneDeTravailManager">Manager des zones de travail</param>
    /// <param name="mapper">Mapper Model / ModelVue</param>
    public ZoneDeTravailController(IMapper mapper, IZoneDeTravailManager zoneDeTravailManager)
    {
      this.zoneDeTravailManager = zoneDeTravailManager;
      this.mapper = mapper;
    }

    /// <summary>
    /// Méthode GET de récupération d'un contrat d'intérimaire
    /// </summary>
    /// <param name="contratInterimaireId">Identifiant du contrat d'interimaire</param>
    /// <returns>Retourne le contrat d'interimaire</returns>
    [HttpGet]
    [Route("api/ZoneDeTravail/{contratInterimaireId}")]
    public HttpResponseMessage GetContratInterimaireById(int contratInterimaireId)
    {
      return Get(() => this.mapper.Map<IEnumerable<ZoneDeTravailModel>>(this.zoneDeTravailManager.GetZoneDeTravailByContratId(contratInterimaireId)));
    }

    /// <summary>
    /// Ajout d'une zone de travail
    /// </summary>
    /// <param name="zoneDeTravailModel">Objet Model envoyé</param>
    /// <returns>Retourne une réponse HTTP</returns>
    [HttpPost]
    [Route("api/ZoneDeTravail/Add")]
    public HttpResponseMessage AddContratInterimaire(ZoneDeTravailModel zoneDeTravailModel)
    {
      return Post(() => this.zoneDeTravailManager.AddZoneDeTravail(this.mapper.Map<ZoneDeTravailEnt>(zoneDeTravailModel)));
    }

    /// <summary>
    /// Suppression d'une contrat d'intérimaire en fonction de son identifiant
    /// </summary>
    /// <param name="zoneDeTravailModel">Objet Model envoyé</param>
    /// <returns>Retourne une réponse HTTP</returns>
    [HttpPost]
    [Route("api/ZoneDeTravail/Delete")]
    public HttpResponseMessage DeleteZoneDeTravail(ZoneDeTravailModel zoneDeTravailModel)
    {
      return Delete(() => this.zoneDeTravailManager.DeleteZoneDeTravail(this.mapper.Map<ZoneDeTravailEnt>(zoneDeTravailModel)));
    }
  }
}