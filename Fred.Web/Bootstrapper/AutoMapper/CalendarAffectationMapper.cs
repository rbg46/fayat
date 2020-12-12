using AutoMapper;
using Fred.Entities.Affectation;
using Fred.Web.Shared.Models.Affectation;

namespace Bootstrapper.AutoMapper
{
    /// <summary>
    /// Calendar affectation mapper
    /// </summary>
    public static class CalendarAffectationMapper
    {
        public static void Map(IMapperConfiguration config)
        {
            config.CreateMap<CalendarAffectationViewEnt, CalendarAffectationViewModel>().ReverseMap();
            config.CreateMap<AffectationViewEnt, AffectationViewModel>().ReverseMap();
            config.CreateMap<AstreinteViewEnt, AstreinteViewModel>().ReverseMap();
        }
    }
}
