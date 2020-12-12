using AutoMapper;
using Fred.Entities.OperationDiverse;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Common.Tests.Data.OperationDiverse.Fake
{
    public class OperationDiverseFake
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
                cfg.CreateMap<OperationDiverseEnt, OperationDiverseAbonnementModel>().ReverseMap();
            });
        }
    }
}
