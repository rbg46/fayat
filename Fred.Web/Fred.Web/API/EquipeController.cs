using AutoMapper;
using Fred.Business.Equipe;
using Fred.Web.Shared.Models.Affectation;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
  /// <summary>
  /// Controller WEB API d'equipe
  /// </summary>
  public class EquipeController : ApiControllerBase
  {
    #region private attribute

    private readonly IEquipeManager equipeManager;
    private readonly IMapper mapper;

    #endregion

    #region constructor

    public EquipeController(IEquipeManager equipeManager, IMapper mapper)
    {
      this.equipeManager = equipeManager;
      this.mapper = mapper;
    }

    #endregion

    #region public method

    /// <summary>
    /// Get une equipe par le proprietaire identifier
    /// </summary>
    /// <returns>Aquipe entity</returns>
    [HttpGet]
    [Route("api/Equipe/GetEquipeByProprietaireId")]
    public HttpResponseMessage GetEquipePersonnelsByProprietaireId()
    {
      return this.Get(() => this.mapper.Map<IEnumerable<ImportedEquipeModel>>(equipeManager.GetEquipePersonnelsByProprietaireId()));
    }

    /// <summary>
    /// Ajouter ou supprimer une lists des personnels à une equipe
    /// </summary>
    /// <param name="equipeViewModel">Object contient deux Listq des personnels identifiers</param>
    /// <returns>Retourne une réponse HTTP</returns>
    [HttpPost]
    [Route("api/Equipe/ManageEquipePersonnels")]
    public HttpResponseMessage ManageEquipePersonnels(EquipeViewModel equipeViewModel)
    {
      return this.Post(() => equipeManager.ManageEquipePersonnels(equipeViewModel?.OuvrierListIdToAdd, equipeViewModel?.OuvrierListIdToDelete));
    }

    #endregion
  }
}