using AutoMapper;
using Fred.Entities.ObjectifFlash;
using Fred.Entities.ObjectifFlash.Panel;
using Fred.Entities.ObjectifFlash.Search;
using Fred.Entities.ReferentielFixe;
using Fred.Web.Models.ObjectifFlash;
using Fred.Web.Shared.Models.Budget.BibliothequePrix.SousDetail;
using Fred.Web.Shared.Models.ObjectifFlash.Panel;
using Fred.Web.Shared.Models.ObjectifFlash.Search;

namespace Bootstrapper.AutoMapper
{
    public static class ObjectifFlashMapper
    {
        public static void Map(IMapperConfiguration cfg)
        {
            cfg.CreateMap<SearchObjectifFlashListWithFilterResult, SearchObjectifFlashListWithFilterResultModel>().ReverseMap();
            cfg.CreateMap<SearchObjectifFlashEnt, SearchObjectifFlashModel>().ReverseMap();
            cfg.CreateMap<ObjectifFlashEnt, ObjectifFlashModel>().ReverseMap();
            cfg.CreateMap<ObjectifFlashEnt, ObjectifFlashLightModel>().AfterMap((src, dest) => dest.CiCodeLibelle = src?.Ci.CodeLibelle).ReverseMap();
            cfg.CreateMap<ObjectifFlashTacheEnt, ObjectifFlashTacheModel>().ReverseMap();
            cfg.CreateMap<ObjectifFlashTacheJournalisationEnt, ObjectifFlashTacheJournalisationModel>().ReverseMap();
            cfg.CreateMap<ObjectifFlashTacheRessourceEnt, ObjectifFlashTacheRessourceModel>().ReverseMap();
            cfg.CreateMap<ObjectifFlashTacheRessourceJournalisationEnt, ObjectifFlashTacheRessourceJournalisationModel>().ReverseMap();
            cfg.CreateMap<ObjectifFlashJournalisation, ObjectifFlashJournalisationModel>().ReverseMap();
            cfg.CreateMap<ObjectifFlashTacheRapportRealiseEnt, ObjectifFlashTacheRapportRealiseModel>().ReverseMap();

            cfg.CreateMap<BudgetSousDetailChapitreBibliothequePrixModel, ChapitrePanelEnt>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ChapitreCode))
                .ForMember(dest => dest.Libelle, opt => opt.MapFrom(src => src.ChapitreLibelle))
                .ReverseMap();

            cfg.CreateMap<BudgetSousDetailSousChapitreBibliothequePrixModel, SousChapitrePanelEnt>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.SousChapitreCode))
                .ForMember(dest => dest.Libelle, opt => opt.MapFrom(src => src.SousChapitreLibelle))
                .ReverseMap();

            cfg.CreateMap<BudgetSousDetailRessourceBibliothequePrixModel, RessourcePanelEnt>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.RessourceCode))
                .ForMember(dest => dest.Libelle, opt => opt.MapFrom(src => src.RessourceLibelle))
                .ForMember(dest => dest.PuHT, opt => opt.MapFrom(src => src.BibliothequePrixMontant))
                .ReverseMap();


            cfg.CreateMap<ChapitreEnt, ChapitrePanelEnt>().ReverseMap();
            cfg.CreateMap<SousChapitreEnt, SousChapitrePanelEnt>().ReverseMap();
            cfg.CreateMap<RessourceEnt, RessourcePanelEnt>().ReverseMap();

            cfg.CreateMap<ChapitrePanelEnt, ChapitrePanelModel>().ReverseMap();
            cfg.CreateMap<SousChapitrePanelEnt, SousChapitrePanelModel>().ReverseMap();
            cfg.CreateMap<RessourcePanelEnt, RessourcePanelModel>().ReverseMap();

        }
    }
}
