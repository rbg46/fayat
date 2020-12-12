using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Referential.Fournisseur;
using Fred.Entities;
using Fred.Entities.Groupe;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Societe.Classification;
using Fred.Web.Models;
using Fred.Web.Models.Groupe;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Societe;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Societe.Classification;
using Moq;

namespace Fred.Common.Tests.Data.Fournisseur.Mock
{
    /// <summary>
    /// Elements fictifs de CommandeTypeEnt
    /// </summary>
    public class FournisseurMocks
    {
        /// <summary>
        /// ctor
        /// </summary>
        public FournisseurMocks()
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

        private List<FournisseurEnt> fournisseurEntStub;
        /// <summary>
        /// obtient ou définit la liste d'entites des classifications société
        /// </summary>
        public List<FournisseurEnt> FournisseurEntStub
        {
            get { return fournisseurEntStub ?? (fournisseurEntStub = GetFakeDbSet().ToList()); }
            set { fournisseurEntStub = value; }
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<FournisseurEnt> GetFakeDbSet()
        {
            return new FakeDbSet<FournisseurEnt>
            {
                new FournisseurEnt
                {
                    FournisseurId = 1,
                    GroupeId = 1,
                    Code = "&SPA0001"
                },
                new FournisseurEnt
                {
                    FournisseurId = 666,
                    GroupeId = 1,
                    Code = "AvecSIRENInvalide",
                    SIREN = ""
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
        public Mock<IFournisseurRepository> GetFakeRepository()
        {
            var fakeRepository = new Mock<IFournisseurRepository>();

            //setup
            fakeRepository.Setup(r => r.Get()).Returns(FournisseurEntStub.AsQueryable());
            fakeRepository.Setup(r => r.Query()).Returns(new RepositoryQuery<FournisseurEnt>(new DbRepository<FournisseurEnt>(null)));

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
            fournisseurEntStub = null;
        }
    }
}

