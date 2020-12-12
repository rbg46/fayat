using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Common.Tests.EntityFramework;
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
    /// Elements fictifs de CommandeTypeEnt
    /// </summary>
    public class CommandeTypeMocks
    {
        /// <summary>
        /// ctor
        /// </summary>
        public CommandeTypeMocks()
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

        private List<CommandeTypeEnt> commandeTypeEntStub;
        /// <summary>
        /// obtient ou définit la liste d'entites des classifications société
        /// </summary>
        public List<CommandeTypeEnt> CommandeTypeEntStub
        {
            get { return commandeTypeEntStub ?? (commandeTypeEntStub = GetFakeDbSet().ToList()); }
            set { commandeTypeEntStub = value; }
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<CommandeTypeEnt> GetFakeDbSet()
        {
            FakeDbSet<CommandeTypeEnt> fakeDbSet = new FakeDbSet<CommandeTypeEnt>();
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
        public List<CommandeTypeEnt> GetFakeList()
        {
            return new List<CommandeTypeEnt>
            {
                new CommandeTypeEnt()
                {
                    Code = CommandeTypeEnt.CommandeTypeF,
                    Libelle = "Fourniture",
                    CommandeTypeId = 1
                },
                new CommandeTypeEnt()
                {
                    Code = CommandeTypeEnt.CommandeTypeL,
                    Libelle = "Location",
                    CommandeTypeId = 2
                },
                new CommandeTypeEnt()
                {
                    Code = CommandeTypeEnt.CommandeTypeP,
                    Libelle = "Prestation",
                    CommandeTypeId = 3
                },
                new CommandeTypeEnt()
                {
                    Code = CommandeTypeEnt.CommandeTypeI,
                    Libelle = "Interimaire",
                    CommandeTypeId = 4
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
        public Mock<IRepository<CommandeTypeEnt>> GetFakeRepository()
        {
            var fakeRepository = new Mock<IRepository<CommandeTypeEnt>>();

            //setup
            fakeRepository.Setup(r => r.Get()).Returns(CommandeTypeEntStub.AsQueryable());

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
            commandeTypeEntStub = null;
        }
    }
}

