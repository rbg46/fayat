using AutoMapper;
using Fred.Entities;
using Fred.Entities.Societe;
using Fred.Entities.Societe.Classification;
using Fred.Web.Models;
using Fred.Web.Models.Societe;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Societe.Classification;

namespace Bootstrapper.AutoMapper
{
    public static class SocieteMapper
    {
        public static void Map(IMapperConfiguration config)
        {
            config.CreateMap<SocieteEnt, SocieteModel>().ReverseMap();
            config.CreateMap<SocieteEnt, SocieteLightModel>().ReverseMap();
            config.CreateMap<SocieteLightEnt, SocieteLightModel>().ReverseMap();
            config.CreateMap<SocieteLightEnt, SocieteModel>().ReverseMap();
            config.CreateMap<SearchSocieteEnt, SearchSocieteModel>().ReverseMap();
            config.CreateMap<SocieteDeviseEnt, SocieteDeviseModel>().ReverseMap();
            config.CreateMap<UniteSocieteEnt, UniteSocieteModel>().ReverseMap();
            config.CreateMap<AssocieSepEnt, AssocieSepModel>().ReverseMap();
            config.CreateMap<TypeSocieteEnt, TypeSocieteModel>().ReverseMap();
            config.CreateMap<TypeParticipationSepEnt, TypeParticipationSepModel>().ReverseMap();
            config.CreateMap<SocieteClassificationEnt, SocieteClassificationModel>().ReverseMap();
        }
    }
}
