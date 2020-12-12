using FluentAssertions;
using Fred.Business.PermissionFonctionnalite;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.PermissionFonctionnalite.Builder;
using Fred.Common.Tests.Data.Permissions.Builder;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Permission;
using Fred.DataAccess.PermissionFonctionnalite;
using Fred.Entities.Permission;
using Fred.Entities.PermissionFonctionnalite;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Permission
{
    /// <summary>
    /// Classe de test de <see cref="PermissionManager"/>
    /// </summary>
    [TestClass]
    public class PermissionManagerTest : BaseTu<PermissionManager>
    {
        [TestMethod]
        [TestCategory("PermissionManager")]
        public void GetUnusedPermissions_Returns_OnlyUnusedPermissions()
        {
            //Builders
            var builderPermission = new PermissionBuilder();
            var builderPermissionDejaUtilisee = new PermissionFonctionnaliteBuilder();
            //Context Fake
            var context = GetMocked<FredDbContext>();
            //liste des permissions Fake
            context.Setup(c => c.Set<PermissionEnt>()).Returns(builderPermission.BuildFakeDbSet(
                builderPermission.PermissionId(1).Code("1").Libelle("Permission 1").PermissionKey("menu.show.1").Build(),
                builderPermission.PermissionId(2).Code("2").Libelle("Permission 2").PermissionKey("menu.show.2").Build(),
                builderPermission.PermissionId(3).Code("3").Libelle("Permission 3").PermissionKey("menu.show.3").Build()
                ));
            //liste des association permission / fonctionnalité Fake
            int PermissionUtilisee = 3;
            context.Setup(c => c.Set<PermissionFonctionnaliteEnt>()).Returns(builderPermissionDejaUtilisee.BuildFakeDbSet(
                builderPermissionDejaUtilisee.PermissionId(1).PermissionId(PermissionUtilisee).FonctionnaliteId(2).Build()
                ));
            //Real Uow
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(context.Object, securityManager.Object);
            //injection
            SubstituteConstructorArgument<IUnitOfWork>(uow);
            SubstituteConstructorArgument<IPermissionRepository>(new PermissionRepository(context.Object));
            SubstituteConstructorArgument<IPermissionFonctionnaliteRepository>(new PermissionFonctionnaliteRepository(context.Object));
            //Act et Assertion
            Actual.GetUnusedPermissions("", 1, int.MaxValue).Should().NotBeEmpty().And.NotContain(c => c.PermissionId.Equals(PermissionUtilisee));
        }
    }
}
