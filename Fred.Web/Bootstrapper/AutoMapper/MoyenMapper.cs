using AutoMapper;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Web.Models.Rapport;
using Fred.Web.Shared.Models.Moyen;

namespace Bootstrapper.AutoMapper
{
    public static class MoyenMapper
    {
        public static void Map(IMapperConfiguration config)
        {
            config.CreateMap<MaterielEnt, MoyenModel>()
                .ForMember(dest => dest.MaterielId, opt => opt.MapFrom(src => src.MaterielId))
                .ForMember(dest => dest.IsActif, opt => opt.MapFrom(src => src.Actif));

            config.CreateMap<MoyenModel, MaterielEnt>()
                .ForMember(dest => dest.MaterielId, opt => opt.MapFrom(src => src.MaterielId))
                .ForMember(dest => dest.Actif, opt => opt.MapFrom(src => src.IsActif));

            config.CreateMap<AffectationMoyenEnt, AffectationMoyenModel>()
                .ForMember(dest => dest.MoyenId, opt => opt.MapFrom(src => src.Materiel != null ? src.Materiel.MaterielId : src.MaterielLocation.MaterielId))
                .ForMember(dest => dest.Moyen, opt => opt.MapFrom(src => src.Materiel != null ? src.Materiel : src.MaterielLocation.Materiel));

            config.CreateMap<AffectationMoyenModel, AffectationMoyenEnt>()
                .ForMember(dest => dest.MaterielId, opt => opt.MapFrom(src => src.MoyenId))
                .ForMember(dest => dest.Materiel, opt => opt.MapFrom(src => src.Moyen));

            config.CreateMap<RapportLigneEnt, RapportLigneModel>()
                .ForMember(dest => dest.AffectationMoyenId, opt => opt.MapFrom(src => src.AffectationMoyenId))
                .ForMember(dest => dest.AffectationMoyen, opt => opt.MapFrom(src => src.AffectationMoyen))
                .ReverseMap();

            config.CreateMap<MaterielLocationEnt, MaterielLocationModel>().ReverseMap();
            config.CreateMap<AffectationMoyenTypeEnt, AffectationMoyenTypeModel>().ReverseMap();
            config.CreateMap<AffectationMoyenFamilleEnt, AffectationMoyenFamilleModel>().ReverseMap();
            config.CreateMap<SiteEnt, SiteModel>().ReverseMap();
            config.CreateMap<SearchAffectationMoyenEnt, SearchAffectationMoyenModel>().ReverseMap();
            config.CreateMap<SearchRapportLigneMoyenEnt, SearchRapportLigneMoyenModel>().ReverseMap();
            config.CreateMap<SearchMoyenEnt, SearchMoyenModel>().ReverseMap();
            config.CreateMap<SearchImmatriculationMoyenEnt, SearchImmatriculationMoyenModel>().ReverseMap();
            config.CreateMap<SearchSocieteMoyenEnt, SearchSocieteMoyenModel>().ReverseMap();
            config.CreateMap<SearchEtablissementMoyenEnt, SearchEtablissementMoyenModel>().ReverseMap();
        }
    }
}
