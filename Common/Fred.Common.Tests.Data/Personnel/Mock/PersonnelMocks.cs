using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.Data.Personnel.Interimaire.Mock;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities;
using Fred.Entities.Groupe;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Fred.Common.Tests.Data.Personnel.Mock
{
    /// <summary>
    /// Elements fictifs du personnel
    /// </summary>
    public class PersonnelMocks
    {
        private ContratInterimaireMocks CttInterimMock;

        public PersonnelMocks()
        {
            CttInterimMock = new ContratInterimaireMocks();
        }

        public SocieteEnt GetDefaultSociete(bool modeFes)
        {
            var result = new SocieteEnt()
            {
                SocieteId = 1,
                CodeSocietePaye = "Code 0012A",
                Groupe = new GroupeEnt { GroupeId = 1, Code = modeFes ? Constantes.CodeGroupeFES : Constantes.CodeGroupeRZB },
                GroupeId = 1
            };
            return result;
        }

        public List<PersonnelEnt> GetAnaelPersonnel()
        {
            return GetFakeDbSet(false).ToList();
        }

        public GoogleApiParam GetDefaultGoogleAPIParam()
        {
            var result = new GoogleApiParam()
            {
                DateCourante = DateTime.Now.AddDays(-10),
            };
            return result;
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<PersonnelEnt> GetFakeDbSet(bool modeFes)
        {
            return new FakeDbSet<PersonnelEnt>
            {
                new PersonnelEnt
                {
                    PersonnelId = 1,
                    Matricule = "001",
                    Nom = "Jourdes",
                    Prenom = "Alexandre",
                    SocieteId = 1,
                    DateCreation = new DateTime(2017, 02, 01),
                    DateEntree = new DateTime(2017, 02, 03),
                    DateSortie = new DateTime(2019, 12, 27),
                    Societe = GetDefaultSociete(modeFes)
                },
                new PersonnelEnt
                {
                    PersonnelId = 2,
                    Matricule = "002",
                    Nom = "Sang",
                    Prenom = "Tava",
                    Societe = GetDefaultSociete(modeFes),
                    SocieteId = 1,
                    DateCreation = new DateTime(2017, 02, 01),
                    DateEntree = new DateTime(2017, 02, 03),
                    DateSortie = new DateTime(2019, 12, 27)
                }
            };
        }

        /// <summary>
        /// Obtient un dbset de interim fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<PersonnelEnt> GetFakeInterimDbSet(bool modeFes)
        {
            return new FakeDbSet<PersonnelEnt>
            {
                new PersonnelEnt
                {
                    PersonnelId = 1,
                    Matricule = "001",
                    Nom = "Interimaire",
                    Prenom = "Two",
                    SocieteId = 1,
                    DateCreation = new DateTime(2019, 01, 01),
                    DateEntree = new DateTime(2019, 01, 03),
                    DateSortie = new DateTime(2019, 12, 27),
                    Societe = GetDefaultSociete(modeFes),
                    IsInterimaire = true,
                    ContratInterimaires = CttInterimMock.GetFakeDbSet().ToList()
                }
            };
        }

        public FredDbContext GetFakeContext(bool onlyInterim, bool modeFes)
        {
            var options = new DbContextOptions<FredDbContext>();
            var context = new Mock<FredDbContext>(options);
            context.Object.Personnels = onlyInterim ? GetFakeInterimDbSet(modeFes) : GetFakeDbSet(modeFes);
            context.Object.ContratInterimaires = CttInterimMock.GetFakeDbSet();
            context.Setup(c => c.Set<PersonnelEnt>()).Returns(onlyInterim ? GetFakeInterimDbSet(modeFes) : GetFakeDbSet(modeFes));
            context.Setup(c => c.Set<ContratInterimaireEnt>()).Returns(CttInterimMock.GetFakeDbSet());
            return context.Object;
        }
        public FredDbContext GetFakeContext()
        {
            return GetFakeContext(false, false);
        }
    }
}
