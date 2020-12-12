using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Journal;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Web.Models.Journal;
using Fred.Web.Models.Societe;
using Fred.Web.Models.Utilisateur;
using Fred.Web.Shared.Models.Journal;

namespace Fred.Common.Tests.Data.Journal.Mock
{
    /// <summary>
    /// Elements fictifs de Journaux comptable
    /// </summary>
    public class JournalMocks
    {
        /// <summary>
        /// ctor
        /// </summary>
        public JournalMocks()
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

        private List<JournalEnt> journauxEntStub;

        public List<JournalEnt> JournauxEntStub
        {
            get { return journauxEntStub ?? (journauxEntStub = GetFakeDbSet().ToList()); }
            set { journauxEntStub = value; }
        }

        private List<JournalModel> journauxModelStub;

        /// <summary>
        /// Obtient ou définit la liste de models des classifications société
        /// </summary>
        public List<JournalModel> JournauxModelStub
        {
            get { return journauxModelStub ?? (journauxModelStub = FakeMapper.Map<List<JournalModel>>(JournauxEntStub)); }
            set { journauxModelStub = value; }
        }

        public static List<JournalFamilleODModel> GetListJournalFamilleODModel()
        {
            return new List<JournalFamilleODModel>
            {
                new JournalFamilleODModel
                {
                    JournalId = 1,
                    Code = "ACD",
                    Libelle = "Achats Correct° *DEMAT*",
                    TypeJournal = "ACHAT",
                    ParentFamilyODWithOrder = 1,
                    ParentFamilyODWithoutOrder = 2,
                    Checked = false,
                    Selected = false,
                    DateCreation = DateTime.Now
                },
                new JournalFamilleODModel
                {
                    JournalId = 2,
                    Code = "NDG",
                    Libelle = "Note débit GAM",
                    TypeJournal = "L",
                    ParentFamilyODWithOrder = 1,
                    ParentFamilyODWithoutOrder = 2,
                    Checked = false,
                    Selected = false,
                    DateCreation = DateTime.Now
                },
                new JournalFamilleODModel
                {
                    JournalId = 3,
                    Code = "OYP",
                    Libelle = "OD Analyt. Paie France",
                    TypeJournal = "ACHAT",
                    ParentFamilyODWithOrder = 0,
                    ParentFamilyODWithoutOrder = 0,
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
        public FakeDbSet<JournalEnt> GetFakeDbSet()
        {
            return new FakeDbSet<JournalEnt>
            {
                new JournalEnt
                {
                    JournalId = 1,
                    SocieteId = 1,
                    Code = "ACD",
                    Libelle = "Achats Correct° *DEMAT*",
                    ImportFacture = false,
                    DateCloture = null,
                    AuteurClotureId = null,
                    TypeJournal = "ACHAT",
                    ParentFamilyODWithOrder = 1,
                    ParentFamilyODWithoutOrder = 2
                },
                new JournalEnt
                {
                    JournalId = 2,
                    SocieteId = 1,
                    Code = "NDG",
                    Libelle = "Note débit GAM",
                    ImportFacture = false,
                    DateCloture = DateTime.UtcNow,
                    AuteurClotureId = 1,
                    TypeJournal = "L",
                    ParentFamilyODWithOrder = 0,
                    ParentFamilyODWithoutOrder = 0
                },
                new JournalEnt
                {
                    JournalId = 3,
                    SocieteId = 1,
                    Code = "OYP",
                    Libelle = "OD Analyt. Paie France",
                    ImportFacture = true,
                    DateCloture = null,
                    AuteurClotureId = null,
                    TypeJournal = "ACHAT",
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
                    cfg.CreateMap<JournalModel, JournalEnt>().ReverseMap();
                    cfg.CreateMap<SocieteModel, SocieteEnt>().ReverseMap();
                    cfg.CreateMap<UtilisateurModel, UtilisateurEnt>().ReverseMap();
                });
        }
    }
}
