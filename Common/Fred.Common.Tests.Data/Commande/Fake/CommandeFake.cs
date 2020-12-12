using AutoMapper;
using Fred.Entities.Carburant;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielEtendu;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.Societe;

namespace Fred.Common.Tests.Data.Commande.Fake
{
    public class CommandeFake
    {
        private IMapper fakeMapper;

        /// <summary>
        /// Obtient ou définit l'automapper
        /// </summary>
        public IMapper FakeMapper
        {
            get { return fakeMapper ?? (fakeMapper = GetMapperConfig().CreateMapper()); }
            set { fakeMapper = value; }
        }

        private MapperConfiguration GetMapperConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RessourceModel, RessourceEnt>().ReverseMap();
                cfg.CreateMap<TacheModel, TacheEnt>().ReverseMap();
                cfg.CreateMap<UniteModel, UniteEnt>().ReverseMap();
                cfg.CreateMap<SousChapitreModel, SousChapitreEnt>().ReverseMap();
                cfg.CreateMap<ChapitreModel, ChapitreEnt>().ReverseMap();
                cfg.CreateMap<TypeRessourceModel, TypeRessourceEnt>().ReverseMap();
                cfg.CreateMap<ReferentielEtenduModel, ReferentielEtenduEnt>().ReverseMap();
                cfg.CreateMap<CarburantModel, CarburantEnt>().ReverseMap();
                cfg.CreateMap<MaterielModel, MaterielEnt>().ReverseMap();
                cfg.CreateMap<SocieteModel, SocieteEnt>().ReverseMap();
                cfg.CreateMap<NatureModel, NatureEnt>().ReverseMap();
            });
        }
    }
}
