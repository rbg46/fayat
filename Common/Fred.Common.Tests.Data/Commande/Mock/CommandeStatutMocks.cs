using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Commande;
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

namespace Fred.Common.Tests.Data.Commande.Mock
{
    /// <summary>
    /// Elements fictifs de SocieteClassification
    /// </summary>
    public class CommandeStatutMocks
    {
        /// <summary>
        /// ctor
        /// </summary>
        public CommandeStatutMocks()
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

        private List<StatutCommandeEnt> statutCommandeEntStub;
        /// <summary>
        /// obtient ou définit la liste d'entites des classifications société
        /// </summary>
        public List<StatutCommandeEnt> StatutCommandeEntStub
        {
            get { return statutCommandeEntStub ?? (statutCommandeEntStub = GetFakeDbSet().ToList()); }
            set { statutCommandeEntStub = value; }
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<StatutCommandeEnt> GetFakeDbSet()
        {
            FakeDbSet<StatutCommandeEnt> fakeDbSet = new FakeDbSet<StatutCommandeEnt>();
            foreach (var item in GetFakeList())
            {
                fakeDbSet.Add(item);
            }
            return fakeDbSet;
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public List<StatutCommandeEnt> GetFakeList()
        {
            return new List<StatutCommandeEnt>
            {
                new StatutCommandeEnt()
                {
                    Code = StatutCommandeEnt.CommandeStatutBR,
                    Libelle = "Brouillon",
                    StatutCommandeId = 1
                },
                new StatutCommandeEnt()
                {
                    Code = StatutCommandeEnt.CommandeStatutAV,
                    Libelle = "A Valider",
                    StatutCommandeId = 2
                },
                new StatutCommandeEnt()
                {
                    Code = StatutCommandeEnt.CommandeStatutVA,
                    Libelle = "Validée",
                    StatutCommandeId = 3
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
        public Mock<IRepository<StatutCommandeEnt>> GetFakeRepository()
        {
            var fakeRepository = new Mock<IRepository<StatutCommandeEnt>>();

            //setup
            fakeRepository.Setup(r => r.Get()).Returns(StatutCommandeEntStub.AsQueryable());
            //fakeRepository.Setup(r => r.Query()).Returns(new RepositoryQuery<StatutCommandeEnt>(new DbRepository<StatutCommandeEnt>(null, GetFakeUow().Object)));

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
            statutCommandeEntStub = null;
        }
    }
}

