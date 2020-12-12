using FluentAssertions;
using Fred.Business.Moyen;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Moyen.Builder;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Moyen;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Fred.Framework.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Moyen
{
    [TestClass]
    public class MaterielLocationManagerTest : BaseTu<MaterielLocationManager>
    {
        private readonly UtilisateurBuilder UserBuilder = new UtilisateurBuilder();
        private MaterielLocationBuilder MaterielLocationBuilder = new MaterielLocationBuilder();
        private Mock<FredDbContext> Context;

        [TestInitialize]
        public void TestInitialize()
        {          
            Context = GetMocked<FredDbContext>();
            Context.Object.MaterielLocation = MaterielLocationBuilder.BuildFakeDbSet(MaterielLocationBuilder.New());
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(Context.Object, securityManager.Object);

            SubstituteConstructorArgument<IUnitOfWork>(uow);
            var utilisateurManager = GetMocked<IUtilisateurManager>();
            utilisateurManager.Setup(u => u.GetContextUtilisateurId()).Returns(UserBuilder.Prototype().UtilisateurId);
        }

        [TestMethod]
        [TestCategory("MaterielLocationManager")]
        public void AddMaterielLocation_NormalCase()
        {
            var result = Actual.AddMaterielLocation(MaterielLocationBuilder.MaterielLocationId(2).Build());
            result.Should().Equals(2);
        }

        [TestMethod]
        [TestCategory("MaterielLocationManager")]
        public void AddMaterielLocation_ObjetNull_ThrowException()
        {
            Invoking(() => Actual.AddMaterielLocation(It.IsAny<MaterielLocationEnt>())).Should().Throw<FredBusinessException>().WithMessage(BusinessResources.ErrorMaterielLocationNull);
        }

        [TestMethod]
        [TestCategory("MaterielLocationManager")]
        public void UpdateMaterielLocation_ObjetNull_ThrowException()
        {
            Invoking(() => Actual.UpdateMaterielLocation(It.IsAny<MaterielLocationEnt>())).Should().Throw<FredBusinessException>().WithMessage(BusinessResources.ErrorMaterielLocationNull);
        }
    }
}
