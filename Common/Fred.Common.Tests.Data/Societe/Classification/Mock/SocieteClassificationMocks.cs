using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Common.Tests.Data.Societe.Mock;
using Fred.Common.Tests.EntityFramework;
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

namespace Fred.Common.Tests.Data.Societe.Classification.Mock
{
    /// <summary>
    /// Elements fictifs de SocieteClassification
    /// </summary>
    public class SocieteClassificationMocks
    {
        /// <summary>
        /// éléments fictifes des sociétés
        /// </summary>
        private readonly SocieteMocks societeMock;

        /// <summary>
        /// ctor
        /// </summary>
        public SocieteClassificationMocks()
        {
            societeMock = new SocieteMocks();
        }

        /// <summary>
        /// Ontient la liste fictives des sociétés
        /// </summary>
        public List<SocieteEnt> SocieteStub => societeMock.GetFakeDbSet().ToList();

        private IMapper fakeMapper;
        /// <summary>
        /// Obtient ou définit l'automapper
        /// </summary>
        public IMapper FakeMapper
        {
            get { return fakeMapper ?? (fakeMapper = GetMapperConfig().CreateMapper()); }
            set { fakeMapper = value; }
        }

        private List<SocieteClassificationEnt> societesClassificationsEntStub;
        /// <summary>
        /// obtient ou définit la liste d'entites des classifications société
        /// </summary>
        public List<SocieteClassificationEnt> SocietesClassificationsEntStub
        {
            get { return societesClassificationsEntStub ?? (societesClassificationsEntStub = GetFakeDbSet().ToList()); }
            set { societesClassificationsEntStub = value; }
        }


        private List<SocieteClassificationModel> societesClassificationsModelStub;
        /// <summary>
        /// Obtient ou définit la liste des classifications Société en model
        /// </summary>
        public List<SocieteClassificationModel> SocietesClassificationsModelStub
        {
            get { return societesClassificationsModelStub ?? (societesClassificationsModelStub = FakeMapper.Map<List<SocieteClassificationModel>>(SocietesClassificationsEntStub)); }
            set { societesClassificationsModelStub = value; }
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<SocieteClassificationEnt> GetFakeDbSet()
        {
            return new FakeDbSet<SocieteClassificationEnt>
            {
                new SocieteClassificationEnt
                {
                    SocieteClassificationId = 1,
                    Code = "00",
                    Libelle = "Fayat",
                    Statut = true,
                    Societes = SocieteStub.FindAll(s => s.SocieteClassificationId.Equals(1)),
                    GroupeId = 1
                },
                new SocieteClassificationEnt
                {
                    SocieteClassificationId = 2,
                    Code = "01",
                    Libelle = "Bouygues",
                    Statut = true,
                    Societes = SocieteStub.FindAll(s => s.SocieteClassificationId.Equals(2)),
                    GroupeId = 1
                },
                new SocieteClassificationEnt
                {
                    SocieteClassificationId = 3,
                    Code = "02",
                    Libelle = "Eiffage",
                    Statut = true,
                    Societes = SocieteStub.FindAll(s => s.SocieteClassificationId.Equals(3)),
                    GroupeId = 1
                }
            };
        }

        /// <summary>
        /// Obtient une nouvelle classification fictive à ajouter
        /// </summary>
        public SocieteClassificationEnt ClassificationAdded => new SocieteClassificationEnt
        {
            SocieteClassificationId = 0,
            Code = "03",
            Libelle = "Eiffage Ajout",
            Statut = true
        };

        /// <summary>
        /// Obtient une nouveau model classification fictive à ajouter
        /// </summary>
        public SocieteClassificationModel ClassificationModelAdded => FakeMapper.Map<SocieteClassificationModel>(ClassificationAdded);

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
    }
}
