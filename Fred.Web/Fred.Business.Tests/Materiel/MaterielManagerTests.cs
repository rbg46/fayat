using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Fred.Business.Referential.Materiel;
using Fred.DataAccess.Referential.Materiel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Fred.Web.Shared.Models.Materiel.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Materiel
{
    [TestClass]
    public class MaterielManagerTests
    {
        private static FredDbContext context;

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext testContext)
        {
            DbContextOptions<FredDbContext> options = new DbContextOptionsBuilder<FredDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var initializationContext = new FredDbContext(options))
            {
                var societe1 = new SocieteEnt { Code = "S1 - Code", Libelle = "S1 - Libelle" };
                var societe2 = new SocieteEnt { Code = "S2 - Code", Libelle = "S2 - Libelle" };
                await initializationContext.Societes.AddAsync(societe1).ConfigureAwait(false);
                await initializationContext.Societes.AddAsync(societe2).ConfigureAwait(false);

                var etablissementComptable = new EtablissementComptableEnt { Code = "EC1 - Code", Libelle = "EC1 - Libelle" };
                await initializationContext.EtablissementsComptables.AddAsync(etablissementComptable).ConfigureAwait(false);

                await initializationContext.Materiels.AddAsync(new MaterielEnt { MaterielId = 1, Code = "M1 - Code", Libelle = "M1 - Libelle", Societe = societe1, EtablissementComptable = etablissementComptable, Actif = true }).ConfigureAwait(false);
                await initializationContext.Materiels.AddAsync(new MaterielEnt { MaterielId = 2, Code = "M2 - Code", Libelle = "M2 - Libelle", Societe = societe1, IsStorm = true }).ConfigureAwait(false);
                await initializationContext.Materiels.AddAsync(new MaterielEnt { MaterielId = 3, Code = "M3 - Code", Libelle = "M3 - Libelle", Societe = societe1, MaterielLocation = true }).ConfigureAwait(false);
                await initializationContext.Materiels.AddAsync(new MaterielEnt { MaterielId = 4, Code = "M4 - Code", Libelle = "M4 - Libelle", Societe = societe2 }).ConfigureAwait(false);
                await initializationContext.Materiels.AddAsync(new MaterielEnt { MaterielId = 5, Code = "M5 - Code", Libelle = "M5 - Libelle", Societe = societe2 }).ConfigureAwait(false);

                await initializationContext.SaveChangesAsync().ConfigureAwait(false);
            }

            context = new FredDbContext(options);
        }

        [ClassCleanup]
        public static void TestCleanup()
        {
            context?.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Search_NullParameter_ThrowsArgumentNullException()
        {
            await SearchMaterielsAsync(null).ConfigureAwait(false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Search_SocieteIdNotGreaterThanZero_ThrowsArgumentException()
        {
            var parameter = new MaterielSearchParameter { SocieteId = 0, PageSize = 1 };

            await SearchMaterielsAsync(parameter).ConfigureAwait(false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Search_NegativePageIndex_ThrowsArgumentException()
        {
            var parameter = new MaterielSearchParameter { SocieteId = 1, PageIndex = -1, PageSize = 2 };

            await SearchMaterielsAsync(parameter).ConfigureAwait(false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Search_PageSizeNotGreatherThanZero_ThrowsArgumentException()
        {
            var parameter = new MaterielSearchParameter { SocieteId = 2, PageSize = -3 };

            await SearchMaterielsAsync(parameter).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task Search_SocieteIdIsOne_ReturnsSocietePurchasedMateriels()
        {
            var expected = new List<MaterielSearchModel>
            {
                new MaterielSearchModel { Id = 1, Code = "M1 - Code", Libelle = "M1 - Libelle", EtablissementComptable = "EC1 - Code - EC1 - Libelle", IsActif = true },
                new MaterielSearchModel { Id = 2, Code = "M2 - Code", Libelle = "M2 - Libelle", IsStorm = true }
            };

            var parameter = new MaterielSearchParameter { SocieteId = 1 };
            IEnumerable<MaterielSearchModel> actual = await SearchMaterielsAsync(parameter).ConfigureAwait(false);

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task Search_SocieteIdIsOneAndPageSizeIsOne_ReturnsSocietePurchasedMateriels()
        {
            var expected = new List<MaterielSearchModel>
            {
                new MaterielSearchModel { Id = 1, Code = "M1 - Code", Libelle = "M1 - Libelle", EtablissementComptable = "EC1 - Code - EC1 - Libelle", IsActif = true }
            };

            var parameter = new MaterielSearchParameter { SocieteId = 1, PageSize = 1 };
            IEnumerable<MaterielSearchModel> actual = await SearchMaterielsAsync(parameter).ConfigureAwait(false);

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task Search_SocieteIdIsOneAndPageSizeIsOneAndPageIndexIsOne_ReturnsSocietePurchasedMateriels()
        {
            var expected = new List<MaterielSearchModel>
            {
                new MaterielSearchModel { Id = 2, Code = "M2 - Code", Libelle = "M2 - Libelle", IsStorm = true }
            };

            var parameter = new MaterielSearchParameter { SocieteId = 1, PageSize = 1, PageIndex = 1 };
            IEnumerable<MaterielSearchModel> actual = await SearchMaterielsAsync(parameter).ConfigureAwait(false);

            actual.Should().BeEquivalentTo(expected);
        }

        private async Task<IEnumerable<MaterielSearchModel>> SearchMaterielsAsync(MaterielSearchParameter parameter)
        {
            var instance = new DefaultMaterielManager(null, new MaterielRepository(context, null), null, null, null, null);

            return await instance.SearchMaterielsAsync(parameter).ConfigureAwait(false);

        }
    }
}