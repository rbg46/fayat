using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.FeatureFlipping;
using Fred.Entities.Groupe;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Societe.Classification;
using Fred.Framework.FeatureFlipping;
using Fred.Web.Models;
using Fred.Web.Models.Groupe;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Societe;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Societe.Classification;
using Moq;

namespace Fred.Common.Tests.Data.FeatureFlipping.Mock
{
    /// <summary>
    /// Elements fictifs de CommandeTypeEnt
    /// </summary>
    public class FeatureFlippingMocks
    {
        /// <summary>
        /// ctor
        /// </summary>
        public FeatureFlippingMocks()
        {
        }

        private IMapper fakeMapper;
        /// <summary>
        /// Obtient ou définit l'automapper
        /// </summary>
        public IMapper FakeMapper
        {
            get { return fakeMapper ?? (fakeMapper = GetMapperConfig().CreateMapper()); }
            set { fakeMapper = value; }
        }

        private List<FeatureFlippingEnt> featureFlippingEntStub;
        /// <summary>
        /// obtient ou définit la liste d'entites des classifications société
        /// </summary>
        public List<FeatureFlippingEnt> FeatureFlippingEntStub
        {
            get { return featureFlippingEntStub ?? (featureFlippingEntStub = GetFakeDbSet().ToList()); }
            set { featureFlippingEntStub = value; }
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<FeatureFlippingEnt> GetFakeDbSet()
        {
            return new FakeDbSet<FeatureFlippingEnt>
            {
                new FeatureFlippingEnt()
                {
                    Code = (int)EnumFeatureFlipping.BlocageFournisseursSansSIRET,
                    Name = EnumFeatureFlipping.BlocageFournisseursSansSIRET.ToString(),
                    IsActived = true
                }
            };
        }

        /// <summary>
        /// Obtient la configuration de auto mapper fictif
        /// </summary>
        /// <returns></returns>
        public MapperConfiguration GetMapperConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SocieteClassificationModel, SocieteClassificationEnt>().ReverseMap();
                cfg.CreateMap<SocieteModel, SocieteEnt>().ReverseMap();
                cfg.CreateMap<OrganisationModel, OrganisationEnt>().ReverseMap();
                cfg.CreateMap<GroupeModel, GroupeEnt>().ReverseMap();
                cfg.CreateMap<TypeSocieteModel, TypeSocieteEnt>().ReverseMap();
                cfg.CreateMap<FournisseurLightModel, FournisseurEnt>().ReverseMap();
            });
        }

        /// <summary>
        /// Obtient un repository fictif
        /// </summary>
        /// <returns></returns>
        public Mock<IFeatureFlippingRepository> GetFakeRepository()
        {
            var fakeRepository = new Mock<IFeatureFlippingRepository>();

            //setup
            fakeRepository.Setup(r => r.Get()).Returns(FeatureFlippingEntStub.AsQueryable());

            return fakeRepository;
        }

        /// <summary>
        /// Obtient un unit of work fictif
        /// </summary>
        /// <returns></returns>
        public Mock<IUnitOfWork> GetFakeUow()
        {
            var unitOfWork = new Mock<IUnitOfWork>();

            return unitOfWork;
        }

        /// <summary>
        /// Permet d'inialiser les éléments fictifs
        /// </summary>
        public void InitialiseMocks()
        {
            featureFlippingEntStub = null;
        }
    }
}

