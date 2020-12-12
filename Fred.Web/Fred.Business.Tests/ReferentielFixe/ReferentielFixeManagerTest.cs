using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Fred.Business.ReferentielFixe;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.ReferentielEtendu.Mock;
using Fred.Common.Tests.Data.ReferentielFixe.Builder;
using Fred.Common.Tests.Data.ReferentielFixe.Mock;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.ReferentielEtendu;
using Fred.DataAccess.ReferentielFixe;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.ReferentielFixe
{
    [TestClass]
    public class ReferentielFixeManagerTest : BaseTu<ReferentielFixeManager>
    {
        private readonly RessourceMocks Ressources = new RessourceMocks();
        private readonly ReferentielEtenduMocks Referentiels = new ReferentielEtenduMocks();
        private readonly SousChapitreBuilder SsChapitreBuilder = new SousChapitreBuilder();
        private readonly ChapitreBuilder ChapitreBuilder = new ChapitreBuilder();
        private const int TypePersonnel = (int)RessourceMocks.TypeRessource.personnel;
        private const int TypeMateriel = (int)RessourceMocks.TypeRessource.materiel;

        [TestInitialize]
        public void TestInitialize()
        {
            var fakeUserManager = GetMocked<IUtilisateurManager>();
            fakeUserManager.Setup(u => u.GetContextUtilisateur()).Returns(new UtilisateurBuilder().Prototype());
            var fakeSepService = GetMocked<ISepService>();
            fakeSepService.Setup(s => s.GetSocieteGerante(It.IsAny<int>())).Returns(new SocieteBuilder().Prototype());
            var fakeContext = GetMocked<FredDbContext>();
            fakeContext.Setup(c => c.Set<RessourceEnt>()).Returns(Ressources.GetFakeDbSet());
            fakeContext.Setup(c => c.Set<ReferentielEtenduEnt>()).Returns(Referentiels.GetFakeDbSet());
            fakeContext.Setup(c => c.Set<ChapitreEnt>()).Returns(ChapitreBuilder.BuildFakeDbSet(1));
            fakeContext.Setup(c => c.Set<SousChapitreEnt>()).Returns(SsChapitreBuilder.BuildFakeDbSet(1));
            fakeContext.Object.Ressources = Ressources.GetFakeDbSet();
            fakeContext.Object.ReferentielEtendus = Referentiels.GetFakeDbSet();
            fakeContext.Object.Chapitres = ChapitreBuilder.BuildFakeDbSet(1);
            fakeContext.Object.SousChapitres = SsChapitreBuilder.BuildFakeDbSet(1);
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(fakeContext.Object, securityManager.Object);
            SubstituteConstructorArgument<IUnitOfWork>(uow);
            SubstituteConstructorArgument<IReferentielEtenduRepository>(new ReferentielEtenduRepository(fakeContext.Object, null, null, null));
            SubstituteConstructorArgument<IRessourceRepository>(new RessourceRepository(fakeContext.Object));
        }


        [TestMethod]
        [TestCategory("ReferentielFixeManager")]
        public void SearchLightReferentielFixe_ForSocietePartenaire_InGestionPersonnel_Returns_OnlyPersonnels()
        {
            //RG_5406_003
            SocieteEnt societe = new SocieteBuilder().Type.Partenaire()
                .Do(r => GetMocked<ISocieteManager>()
                    .Setup(m => m.GetSocieteById(It.IsAny<int>(), It.IsAny<List<Expression<Func<SocieteEnt, object>>>>(), false))
                    .Returns(r))
                .Build();
            Actual.SearchLight("", 1, 1, 20, TypePersonnel, null)
            .Should().OnlyContain(r => TypePersonnel.Equals(r.TypeRessource.TypeRessourceId));
        }

        [TestMethod]
        [TestCategory("ReferentielFixeManager")]
        public void SearchLightReferentielFixe_ForSocietePartenaire_InGestionMateriel_Returns_OnlyMateriels()
        {
            //RG_5406_004
            SocieteEnt societe = new SocieteBuilder().Type.Partenaire()
                .Do(r => GetMocked<ISocieteManager>()
                    .Setup(m => m.GetSocieteById(It.IsAny<int>(), It.IsAny<List<Expression<Func<SocieteEnt, object>>>>(), false))
                    .Returns(r))
                .Build();
            Actual.SearchLight("", 1, 1, 20, TypeMateriel, null)
            .Should().OnlyContain(r => TypeMateriel.Equals(r.TypeRessource.TypeRessourceId));
        }

        [TestMethod]
        [TestCategory("ReferentielFixeManager")]
        public void SearchLightReferentielFixe_ForSocieteSep_InGestionMateriel_Returns_OnlyMateriels()
        {
            //RG_5406_004
            SocieteEnt societe = new SocieteBuilder().Type.Sep()
                .Do(r => GetMocked<ISocieteManager>()
                    .Setup(m => m.GetSocieteById(It.IsAny<int>(), It.IsAny<List<Expression<Func<SocieteEnt, object>>>>(), false))
                    .Returns(r))
                .Build();
            Actual.SearchLight("", 1, 1, 20, TypeMateriel, null)
            .Should().OnlyContain(r => TypeMateriel.Equals(r.TypeRessource.TypeRessourceId));
        }

        [TestMethod]
        [TestCategory("ReferentielFixeManager")]
        [TestCategory("OperationDiverseExcelService")]
        public void GetListRessourceBySocieteIdWithSousChapitreEtChapitre_WithSocietyAndCi_Returns_OnlyContainExpeted()
        {
            Actual.GetListRessourceBySocieteIdWithSousChapitreEtChapitre(1, 1)
                .Should().OnlyContain(r => r.SpecifiqueCiId.Equals(1) && r.ReferentielEtendus.Any(re => re.SocieteId.Equals(1)));
        }

    }
}
