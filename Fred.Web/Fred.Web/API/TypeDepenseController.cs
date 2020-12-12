using AutoMapper;
using Fred.Business.Referential;
using Fred.Web.Models.Referential;
using System.Collections.Generic;

using System.Web.Http;

namespace Fred.Web.API
{
  /// <summary>
  /// Controller WEB API des types de dépenses
  /// </summary>
  public class TypeDepenseController : ApiController
  {
    /// <summary>
    /// Manager business des types de dépenses
    /// </summary>
    protected readonly ITypeDepenseManager TypeDepenseMgr;

    /// <summary>
    /// Mapper Model / ModelVue
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="TypeDepenseController" />.
    /// </summary>
    /// <param name="typeDepenseMgr">Manager des types de dépenses</param>
    /// <param name="mapper">Mapper Model / ModelVue</param>
    public TypeDepenseController(ITypeDepenseManager typeDepenseMgr, IMapper mapper)
    {
      this.TypeDepenseMgr = typeDepenseMgr;
      this.Mapper = mapper;
    }

    /// <summary>
    /// Méthode GET de récupération des types de dépenses
    /// </summary>
    /// <returns>Retourne la liste des types de dépenses</returns>
    [HttpGet]
    public IEnumerable<TypeDepenseModel> Get()
    {
      var typeDepenses = this.TypeDepenseMgr.GetTypeDepenseList();
      return this.Mapper.Map<IEnumerable<TypeDepenseModel>>(typeDepenses);
    }
  }
}