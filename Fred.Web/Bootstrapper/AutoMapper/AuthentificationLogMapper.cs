using AutoMapper;
using Fred.Business.Authentification;
using Fred.Entities;
using Fred.Web.Shared.Models;

namespace Bootstrapper.AutoMapper
{
  public static class AuthentificationLogMapper
  {
    public static void Map(IMapperConfiguration config)
    {
      config.CreateMap<AuthentificationLogEnt, AuthentificationLogListModel>().AfterMap((src, dest) =>
      {
        dest.Error = AuthentificationManagerHelper.ConvertConnexionStatusIntToString(src.ErrorType); 
        dest.OriginText = AuthentificationManagerHelper.ConvertErrorOriginIntToText(src.ErrorOrigin);
      });
      config.CreateMap<AuthentificationLogEnt, AuthentificationLogDetailModel>().AfterMap((src, dest) =>
      {
        dest.Error = AuthentificationManagerHelper.ConvertConnexionStatusIntToString(src.ErrorType);
        dest.OriginText = AuthentificationManagerHelper.ConvertErrorOriginIntToText(src.ErrorOrigin);
      });
      config.CreateMap<AuthentificationLogEnt, AuthentificationLogListModel>().ReverseMap();
      config.CreateMap<AuthentificationLogEnt, AuthentificationLogDetailModel>().ReverseMap();
    }
  }
}
