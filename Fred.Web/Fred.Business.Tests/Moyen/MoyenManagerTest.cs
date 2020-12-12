using System;
using System.Linq;
using FluentAssertions;
using Fred.Business.Moyen;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Moyen.Builder;
using Fred.Common.Tests.Data.Moyen.Mock;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.DataAccess.AffectationMoyen;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Referential.Materiel;
using Fred.Entities.Moyen;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Fred.Framework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Moyen
{
    [TestClass]
    public class MoyenManagerTest : BaseTu<MoyenManager>
    {
        private readonly UtilisateurBuilder UserBuilder = new UtilisateurBuilder();
        private MaterielLocationBuilder MaterielLocationBuilder = new MaterielLocationBuilder();
        private readonly AffectationMoyenTypeMocks AffectationMoyenTypeMocks = new AffectationMoyenTypeMocks();
        private readonly MoyenMocks MoyenMocks = new MoyenMocks();
        private readonly MoyenBuilder MoyenBuilder = new MoyenBuilder();
        private Mock<FredDbContext> context;
        private IMaterielRepository materielRepository;
        private string codeMoyenInexistant = "XXXXXXX";

        [TestInitialize]
        public void TestInitialize()
        {
            context = GetMocked<FredDbContext>();
            context.Object.Materiels = MoyenBuilder.BuildFakeDbSet(MoyenMocks.GetFakeDbSet());
            context.Setup(c => c.Set<MaterielEnt>()).Returns(MoyenMocks.GetFakeDbSet());
            context.Setup(c => c.Set<AffectationMoyenTypeEnt>()).Returns(AffectationMoyenTypeMocks.GetFakeDbSet());
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(context.Object, securityManager.Object);
            SubstituteConstructorArgument<IUnitOfWork>(uow);
            materielRepository = new MaterielRepository(context.Object, GetMocked<ILogManager>().Object);
            SubstituteConstructorArgument<IMaterielRepository>(materielRepository);
            SubstituteConstructorArgument<IAffectationMoyenTypeRepository>(new AffectationMoyenTypeRepository(context.Object));

            var utilisateurManager = GetMocked<IUtilisateurManager>();
            utilisateurManager.Setup(u => u.GetContextUtilisateurId()).Returns(UserBuilder.Prototype().UtilisateurId);
        }


        [TestMethod]
        [TestCategory("MoyenManager")]
        public void GetMoyen_ByCode_NotNull()
        {
            var uniqueCodes = context.Object.Materiels.GroupBy(x => x.Code)
               .Where(g => g.Count() == 1)
               .Select(y => y.Key)
               .ToList();
            string codeMoyenUnique = uniqueCodes.FirstOrDefault();
            var result = Actual.GetMoyen(codeMoyenUnique);
            result.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("MoyenManager")]
        public void GetMoyen_ByCode_ReturnsNull()
        {
            var result = Actual.GetMoyen(codeMoyenInexistant);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("MoyenManager")]
        public void GetMoyen_ObjetNull_ThrowFredBusinessExceptionDoublon()
        {
            var duplicateKey = context.Object.Materiels.GroupBy(x => x.Code)
               .Where(g => g.Count() > 1)
               .Select(y => y.Key)
               .ToList();
            string duplicateCode = duplicateKey.FirstOrDefault();
            Invoking(() => Actual.GetMoyen(duplicateCode)).Should().Throw<FredBusinessException>().WithMessage(string.Format(BusinessResources.RechercheMoyen_Erreur_Doublon, duplicateCode));
        }

        [TestMethod]
        [TestCategory("MoyenManager")]
        public void GetMoyen_ByCodeAndSocieteId_NotNull()
        {
            string codeMoyen = context.Object.Materiels.FirstOrDefault().Code;
            int societeId = context.Object.Materiels.FirstOrDefault().SocieteId;
            int? etablissementComptableId = context.Object.Materiels.FirstOrDefault().EtablissementComptableId;
            var result = Actual.GetMoyen(codeMoyen, societeId, etablissementComptableId);
            result.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("MoyenManager")]
        public void GetMoyen_ByCodeAndSocieteId_ReturnsNull()
        {
            var result = Actual.GetMoyen(codeMoyenInexistant, 1, null);
            result.Should().BeNull();
        }

        [TestMethod]
        [TestCategory("MoyenManager")]
        public void AddOrUpdateMoyen_AddMateriel_OK()
        {
            var result = Actual.AddOrUpdateMoyen(MoyenBuilder.MaterielId(0).Build());
            result.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("MoyenManager")]
        public void AddOrUpdateMoyen_ObjetNull_ThrowException()
        {
            Invoking(() => Actual.AddOrUpdateMoyen(It.IsAny<MaterielEnt>())).Should().Throw<Exception>();
        }

        [TestMethod]
        [TestCategory("MoyenManager")]
        public void SearchLightForAffectationMoyenType_NotNull()
        {
            var result = Actual.SearchLightForAffectationMoyenType(1, 20);
            result.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("MoyenManager")]
        public void SearchLightForFicheGenerique_NotNull()
        {
            var result = Actual.SearchLightForFicheGenerique(1, 20);
            result.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("MoyenManager")]
        public void UpdateMaterielLocation_OK()
        {
            var result = Actual.UpdateMaterielLocation(MaterielLocationBuilder.New());
            result.Should().Equals(1);
        }
    }
}
