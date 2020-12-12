using System.Linq;
using AutoMapper;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.CloturesPeriodes;
using Fred.Web.Models;
using Fred.Web.Models.CI;
using Fred.Web.Models.CloturesPeriodes;

namespace Bootstrapper.AutoMapper
{
    public static class CIMapper
    {
        public static void Map(IMapperConfiguration config)
        {
            config.CreateMap<CIEnt, CIModel>()
                .ForMember(dest => dest.TypeCI, opts => opts.MapFrom(src => src.CIType.RessourceKey))
                .AfterMap((src, dest) =>
                {
                    var uo = dest.Parents.FirstOrDefault(x => x.TypeOrganisation.Code == Constantes.OrganisationType.CodeUo);
                    var puo = dest.Parents.FirstOrDefault(x => x.TypeOrganisation.Code == Constantes.OrganisationType.CodePuo);
                    dest.PUO = puo?.OrganisationGenerique;
                    dest.UO = uo?.OrganisationGenerique;
                    dest.Parents = null;
                })
                .ReverseMap();

            config.CreateMap<CIEnt, CIFullLibelleModel>()
                .ForMember(dest => dest.TypeCI, opts => opts.MapFrom(src => src.CIType.RessourceKey))
                .AfterMap((src, dest) =>
                {
                    var uo = dest.Parents.FirstOrDefault(x => x.TypeOrganisation.Code == Constantes.OrganisationType.CodeUo);
                    var puo = dest.Parents.FirstOrDefault(x => x.TypeOrganisation.Code == Constantes.OrganisationType.CodePuo);
                    dest.PUO = puo?.OrganisationGenerique;
                    dest.UO = uo?.OrganisationGenerique;
                    dest.Parents = null;
                    dest.EtablissementComptable = src.EtablissementComptable != null ?
                        new Fred.Web.Models.Referential.EtablissementComptableModel { Code = src.EtablissementComptable.Code , Libelle =src.EtablissementComptable.Libelle }
                        : null;
                })
                .ReverseMap();

            config.CreateMap<CIDeviseEnt, CIDeviseModel>().ReverseMap();
            config.CreateMap<CIRessourceEnt, CIRessourceModel>().ReverseMap();
            config.CreateMap<SearchCIEnt, SearchCIModel>().ReverseMap();
            config.CreateMap<CICodeMajorationEnt, CICodeMajorationModel>().ReverseMap();
            config.CreateMap<CIPrimeEnt, CIPrimeModel>().ReverseMap();
            config.CreateMap<CITypeEnt, CITypeModel>().ReverseMap();
            config.CreateMap<CITypeSearchEnt, CITypeModel>().ReverseMap();
            config.CreateMap<CIEnt, CILightModel>().ReverseMap();
            config.CreateMap<SearchCloturesPeriodesForCiEnt, SearchCloturesPeriodesForCiModel>().ReverseMap();
        }
    }
}
