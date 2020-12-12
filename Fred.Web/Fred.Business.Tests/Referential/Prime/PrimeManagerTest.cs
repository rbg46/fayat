using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.CI.Mock;
using Fred.Common.Tests.Data.Referential.Prime.Mock;
using Fred.Common.Tests.Data.Societe.Mock;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Referential.Prime;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Referential.Prime
{
    /// <summary>
    /// Classe de test de <see cref="PrimeManager"/>
    /// </summary>
    [TestClass]
    public class PrimeManagerTest : BaseTu<PrimeManager>
    {

        private List<PrimeEnt> ExpectedPrimes;
        private List<CIPrimeEnt> CIPrimesPrivee;
        private List<AssocieSepEnt> Associes;
        private readonly UtilisateurBuilder UserBuilder = new UtilisateurBuilder();

        [TestInitialize]
        public void Initialize()
        {
            var mocks = new PrimeMocks();
            var ciPrimeMocks = new CiPrimeMocks();
            ExpectedPrimes = mocks.GetFakeDbSet().ToList();
            CIPrimesPrivee = ciPrimeMocks.GetFakeDbSet().ToList();
            Associes = new AssocieSepMocks().GetFakeDbSet().ToList();

            var societeManager = GetMocked<ISocieteManager>();
            societeManager.Setup(m => m.GetSocieteById(It.IsAny<int>(), It.IsAny<List<Expression<Func<SocieteEnt, object>>>>(), It.IsAny<bool>()))
                .Returns(new SocieteEnt { SocieteId = 3, AssocieSeps = Associes });
            var context = GetMocked<FredDbContext>();
            context.Setup(c => c.Set<PrimeEnt>()).Returns(mocks.GetFakeDbSet());
            context.Setup(c => c.Set<CIPrimeEnt>()).Returns(ciPrimeMocks.GetFakeDbSet());
            context.Object.Primes = mocks.GetFakeDbSet();
            context.Object.CIPrimes = ciPrimeMocks.GetFakeDbSet();
            var securityManager = GetMocked<ISecurityManager>();
            var unitOfWork = new UnitOfWork(context.Object, securityManager.Object);
            var primeRepository = new PrimeRepository(context.Object, GetMocked<ILogManager>().Object);
            SubstituteConstructorArgument<IPrimeRepository>(primeRepository);
            SubstituteConstructorArgument<IUnitOfWork>(unitOfWork);
            var utilisateurManager = GetMocked<IUtilisateurManager>();
            var user = UserBuilder.Prototype();
            user.SuperAdmin = false;
            utilisateurManager.Setup(u => u.GetContextUtilisateur()).Returns(user);
        }

        [TestMethod]
        [TestCategory("PrimeManager")]
        public async Task SearchLight_ForSEP_ReturnsAllPrimesOfSocietiesOfSEP()
        {
            //Shim pour SEP
            GetMocked<ISepService>().Setup(m => m.IsSep(It.IsAny<SocieteEnt>())).Returns(true);

            var nombrePrimesPriveeCi = CIPrimesPrivee.Where(p => p.CiId.Equals(1)).Count();
            var IdSocieteDesAssocies = Associes.Select(a => a.SocieteAssocieeId).ToList();
            var nombrePrimesDesSocieteAssociees = ExpectedPrimes.Where(p => p.Publique && IdSocieteDesAssocies.Contains(p.SocieteId.Value) && p.GroupeId.Equals(1)).Count();

            var result = await Actual.SearchLightAsync("", 1, 25, 3, 1, false);
            result.Should().HaveCount(nombrePrimesPriveeCi + nombrePrimesDesSocieteAssociees);
        }

        [TestMethod]
        [TestCategory("PrimeManager")]
        public async Task SearchLight_ForInterne_ReturnsAllPrimesOfSociety()
        {
            //Shim pour non SEP (ni FES)
            GetMocked<ISepService>().Setup(m => m.IsSep(It.IsAny<SocieteEnt>())).Returns(false);

            var nombrePrimesDelaSociete = ExpectedPrimes.FindAll(p => p.SocieteId.Equals(1) && p.GroupeId.Equals(1)).Count();

            var result = await Actual.SearchLightAsync("", 1, 25, 1, 1, false);

            result.Should().HaveCount(nombrePrimesDelaSociete);
        }
    }
}
