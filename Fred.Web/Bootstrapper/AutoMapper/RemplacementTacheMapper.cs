using AutoMapper;
using Fred.Entities.Depense;
using Fred.Entities.ObjectifFlash;
using Fred.Web.Models.ObjectifFlash;
using Fred.Web.Models.Depense;

namespace Bootstrapper.AutoMapper
{
  public static class RemplacementTacheMapper
  {
    public static void Map(IMapperConfiguration cfg)
    {
      cfg.CreateMap<RemplacementTacheEnt, RemplacementTacheModel>().ReverseMap();
    }
  }
}
