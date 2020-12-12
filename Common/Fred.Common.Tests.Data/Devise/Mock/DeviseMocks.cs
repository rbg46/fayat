using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Interfaces;
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

namespace Fred.Common.Tests.Data.Devise.Mock
{
    /// <summary>
    /// Elements fictifs de CommandeTypeEnt
    /// </summary>
    public class DeviseMocks
    {
        public const int DEVISE_ID_EURO = 48;

        /// <summary>
        /// ctor
        /// </summary>
        public DeviseMocks()
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

        private List<DeviseEnt> deviseEntStub;
        /// <summary>
        /// obtient ou définit la liste d'entites des classifications société
        /// </summary>
        public List<DeviseEnt> DeviseEntStub
        {
            get { return deviseEntStub ?? (deviseEntStub = GetFakeDbSet().ToList()); }
            set { deviseEntStub = value; }
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<DeviseEnt> GetFakeDbSet()
        {
            FakeDbSet<DeviseEnt> fakeDbSet = new FakeDbSet<DeviseEnt>();
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
        public List<DeviseEnt> GetFakeList()
        {
            return new List<DeviseEnt>
            {
                new DeviseEnt()
                {
                    DeviseId = 8,
                    IsoCode = "AUD",
                    IsoNombre = "36",
                    Symbole = "$",
                    Libelle = "Dollar australien",
                    CodePaysIso = "AU",
                    DateCreation = DateTime.UtcNow,
                    IsDeleted = false,
                    AuteurCreation = 1,
                    Active = true
                },
                new DeviseEnt()
                {
                    DeviseId = 48,
                    IsoCode = "EUR",
                    IsoNombre = "978",
                    Symbole = "€",
                    Libelle = "Euro",
                    CodePaysIso = "",
                    DateCreation = DateTime.UtcNow,
                    IsDeleted = false,
                    AuteurCreation = 1,
                    Active = true
                },
                new DeviseEnt()
                {
                    DeviseId = 151,
                    IsoCode = "USD",
                    IsoNombre = "840",
                    Symbole = "$",
                    Libelle = "Dollar américain",
                    CodePaysIso = "US",
                    DateCreation = DateTime.UtcNow,
                    IsDeleted = false,
                    AuteurCreation = 1,
                    Active = true
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
        public Mock<IDeviseRepository> GetFakeRepository()
        {
            var fakeRepository = new Mock<IDeviseRepository>();

            //setup
            fakeRepository.Setup(r => r.Get()).Returns(DeviseEntStub.AsQueryable());

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
            deviseEntStub = null;
        }
    }
}

