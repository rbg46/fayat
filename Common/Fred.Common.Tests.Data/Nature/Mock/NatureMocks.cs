using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Journal;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Web.Models.Journal;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;
using Fred.Web.Models.Utilisateur;
using Fred.Web.Shared.Models.Journal;
using Fred.Web.Shared.Models.Nature;

namespace Fred.Common.Tests.Data.Nature.Mock
{
    /// <summary>
    /// Elements fictifs de Journaux comptable
    /// </summary>
    public class NatureMocks
    {
        /// <summary>
        /// ctor
        /// </summary>
        public NatureMocks()
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

        private List<NatureEnt> naturesEntStub;

        public List<NatureEnt> NaturesEntStub
        {
            get { return naturesEntStub ?? (naturesEntStub = GetFakeDbSet().ToList()); }
            set { naturesEntStub = value; }
        }

        private List<NatureModel> naturesModelStub;

        /// <summary>
        /// Obtient ou définit la liste de models des classifications société
        /// </summary>
        public List<NatureModel> NaturesModelStub
        {
            get { return naturesModelStub ?? (naturesModelStub = FakeMapper.Map<List<NatureModel>>(NaturesEntStub)); }
            set { naturesModelStub = value; }
        }
        List<NatureFamilleOdModel> AssociatedNatures = new List<NatureFamilleOdModel>();

        public static List<NatureFamilleOdModel> GetListNatureFamilleODModel()
        {
            return new List<NatureFamilleOdModel>
            {
                new NatureFamilleOdModel
                {
                    IdNature = 1,
                    NatureAnalytique = "ACH",
                    Libelle = "Achats Correct° *DEMAT*",
                    ParentFamilyODWithOrder = 2,
                    ParentFamilyODWithoutOrder = 3,
                    Checked = false,
                    Selected = false,
                    DateCreation = DateTime.Now
                },
                new NatureFamilleOdModel
                {
                    IdNature = 2,
                    NatureAnalytique = "NDG",
                    Libelle = "Achats Correct° *DEMAT*",
                    ParentFamilyODWithOrder = 1,
                    ParentFamilyODWithoutOrder = 2,
                    Checked = false,
                    Selected = false,
                    DateCreation = DateTime.Now
                },
                new NatureFamilleOdModel
                {
                    IdNature = 3,
                    NatureAnalytique = "OYP",
                    Libelle = "Achats Correct° *DEMAT*",
                    ParentFamilyODWithOrder = 1,
                    ParentFamilyODWithoutOrder = 2,
                    Checked = false,
                    Selected = false,
                    DateCreation = DateTime.Now
                }
            };

        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de JournalEnt</returns>
        public FakeDbSet<NatureEnt> GetFakeDbSet()
        {
            return new FakeDbSet<NatureEnt>
            {
                new NatureEnt
                {
                    NatureId = 1,
                    SocieteId = 1,
                    Code = "ACD",
                    Libelle = "Achats Correct° *DEMAT*",
                    ParentFamilyODWithOrder = 1,
                    ParentFamilyODWithoutOrder = 2
                },
                new NatureEnt
                {
                    NatureId = 2,
                    SocieteId = 1,
                    Code = "NDG",
                    Libelle = "Note débit GAM",
                    ParentFamilyODWithOrder = 0,
                    ParentFamilyODWithoutOrder = 0
                },
                new NatureEnt
                {
                    NatureId = 3,
                    SocieteId = 1,
                    Code = "OYP",
                    Libelle = "OD Analyt. Paie France",
                    ParentFamilyODWithOrder = 0,
                    ParentFamilyODWithoutOrder = 0
                }
            };
        }

        /// <summary>
        /// Obtient la configuration de auto mapper fictif
        /// </summary>
        public MapperConfiguration GetMapperConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NatureModel, NatureEnt>().ReverseMap();
                cfg.CreateMap<SocieteModel, SocieteEnt>().ReverseMap();
                cfg.CreateMap<UtilisateurModel, UtilisateurEnt>().ReverseMap();
            });
        }
    }
}

