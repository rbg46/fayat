using System;
using System.Collections.Generic;
using FluentAssertions;
using Fred.Business.CI;
using Fred.Business.IndemniteDeplacement;
using Fred.Business.Personnel;
using Fred.Business.Referential;
using Fred.Business.Referential.CodeDeplacement;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.CI.Builder;
using Fred.Common.Tests.Data.Personnel.Builder;
using Fred.Common.Tests.Data.Referential.Etablissement.Builder;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Entities.Referential;
using Fred.Web.Shared.App_LocalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Referential.IndemniteDeplacement
{
    /// <summary>
    /// Classe de test de <see cref="CalculFeature" de <seealso cref="IndemniteDeplacementManager"/>/>
    /// </summary>
    [TestClass]
    public class IndemniteDeplacementCalculFeatureTest : BaseTu<CalculFeature>
    {
        private readonly CiBuilder BuilderCi = new CiBuilder();
        private readonly PersonnelBuilder BuilderPersonnel = new PersonnelBuilder();
        private readonly EtablissementPaieBuilder BuilderEtab = new EtablissementPaieBuilder();
        private readonly SocieteBuilder BuilderSociete = new SocieteBuilder();

        private Mock<IPersonnelManager> PersonnelManager;

        [TestInitialize]
        public void TestInitialize()
        {
            PersonnelManager = GetMocked<IPersonnelManager>();
            var ciManager = GetMocked<ICIManager>();
            ciManager.Setup(m => m.GetCIById(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(
                BuilderCi.CiId(1).Societe(BuilderSociete.SocieteFTP()).LocalisationArcachon().Build());

            var zoneDeplacementManager = GetMocked<ICodeZoneDeplacementManager>();
            zoneDeplacementManager.Setup(m => m.GetCodeZoneDeplacementByKm(It.IsAny<int>(), It.IsAny<double>()))
                .Returns(new CodeZoneDeplacementEnt());

            var codeDeplacementManager = GetMocked<ICodeDeplacementManager>();
            codeDeplacementManager.Setup(m => m.GetCodeDeplacementByCode(It.IsAny<string>())).Returns(new CodeDeplacementEnt());
        }


        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_PersonnelNull_ThrowException()
        {
            Invoking(() => Actual.CalculIndemniteDeplacement(null, BuilderCi.Prototype())).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_CiNull_ThrowException()
        {
            Invoking(() => Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), null)).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_WithImputsNotNull_NotThrowException()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.Prototype())
                .EtablissementRattachement(BuilderEtab.Prototype())
                .EtablissementPaie(BuilderEtab.Prototype())
                .Build());
            Invoking(() => Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype())).Should().NotThrow<Exception>();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_WithoutGestionIndemnites_Returns_Null()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.SocieteFTP())
                .EtablissementRattachement(BuilderEtab.Prototype())
                .EtablissementPaie(BuilderEtab.Prototype())
                .Build());
            Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype()).Should().BeNull();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_ShouldCalculate_KmOrthrodromie_WithDomicile()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementDomicile()
                .LocalisationPessac() // important pour ce cas test
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .EtablissementPaie(BuilderEtab.Prototype())
                .Build());
            Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype()).NombreKilometreVOChantierRattachement.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_ShouldCalculate_KmOrthrodromie_WithAgence()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementAgence()
                .LocalisationPessac()
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .EtablissementPaie(BuilderEtab
                    .GestionIndemnites()
                    .Societe(BuilderSociete.SocieteFTP())
                    .LocalisationBordeaux() // important pour ce cas test
                    .Build())
                .Build());
            Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype()).NombreKilometreVOChantierRattachement.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_ShouldCalculate_KmOrthrodromie_WithSecteur()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementSecteur()
                .LocalisationPessac()
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .LocalisationBordeaux() // important pour ce cas test
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .EtablissementPaie(BuilderEtab
                    .GestionIndemnites()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .Build());
            Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype()).NombreKilometreVOChantierRattachement.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_ShouldCalculate_KmRoutier_Domicile()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.SocieteFTP())
                .LocalisationPessac() // important pour ce cas test
                .TypeRattachementDomicile()
                .EtablissementRattachement(BuilderEtab.GestionIndemnites().Societe(BuilderSociete.SocieteFTP()).Build())
                .EtablissementPaie(BuilderEtab.Prototype())
                .Build());
            Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype()).NombreKilometres.Should().BeGreaterOrEqualTo(0.00);
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_ShouldCalculate_KmRoutier_Secteur()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementSecteur()
                .LocalisationPessac()
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .LocalisationBordeaux() // important pour ce cas test
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .EtablissementPaie(BuilderEtab.Prototype())
                .Build());
            Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype()).NombreKilometres.Should().BeGreaterOrEqualTo(0.00);
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_ShouldCalculate_KmRoutier_Agence()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementAgence()
                .LocalisationPessac()
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .EtablissementPaie(BuilderEtab
                    .GestionIndemnites()
                    .LocalisationBordeaux() // important pour ce cas test
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .Build());
            Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype()).NombreKilometres.Should().BeGreaterOrEqualTo(0.00);
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_ShouldCalculCodeZoneDeplacement_HorsRegion()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementSecteur()
                .LocalisationPessac()
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .LocalisationArcachon()
                    .Societe(BuilderSociete.SocieteFTP())
                    .HorsRegion() // important pour ce cas test
                    .Build())
                .EtablissementPaie(BuilderEtab
                    .GestionIndemnites()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .Build());
            Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype()).CodeZoneDeplacement.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_ShouldCalculCodeZoneDeplacement_EnRegion()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementSecteur()
                .LocalisationPessac()
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .LocalisationArcachon()
                    .Societe(BuilderSociete.SocieteFTP())
                    .EnRegion() // important pour ce cas test
                    .Build())
                .EtablissementPaie(BuilderEtab
                    .GestionIndemnites()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .Build());
            Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype()).CodeZoneDeplacement.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_ShouldCalculCodeHZDeplacement_HorsRegion()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementSecteur()
                .LocalisationMontpellier()
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .LocalisationBordeaux()
                    .Societe(BuilderSociete.SocieteFTP())
                    .HorsRegion() // important pour ce cas test
                    .Build())
                .EtablissementPaie(BuilderEtab
                    .GestionIndemnites()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .Build());
            Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype()).CodeZoneDeplacement.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CalculIndemniteDeplacement_ShouldCalculCodeHZDeplacement_EnRegion()
        {
            PersonnelManager.Setup(m => m.GetPersonnel(It.IsAny<int>())).Returns<int>(i =>
                BuilderPersonnel.PersonnelId(i)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementSecteur()
                .LocalisationMontpellier()
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .LocalisationBordeaux()
                    .Societe(BuilderSociete.SocieteFTP())
                    .EnRegion() // important pour ce cas test
                    .Build())
                .EtablissementPaie(BuilderEtab
                    .GestionIndemnites()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .Build());
            Actual.CalculIndemniteDeplacement(BuilderPersonnel.Prototype(), BuilderCi.Prototype()).CodeZoneDeplacement.Should().NotBeNull();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CanCalculateIndemniteDeplacement_WithInputNull_NotThrowException()
        {
            Invoking(() => Actual.CanCalculateIndemniteDeplacement(null, null)).Should().NotThrow<Exception>();
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CanCalculateIndemniteDeplacement_WithPersonnelNoGPSForTypeDomicile_ReturnsErrorsGPSPErsonnel()
        {
            var personnel = BuilderPersonnel.PersonnelId(1)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementDomicile()
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .LocalisationBordeaux()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .EtablissementPaie(BuilderEtab
                    .GestionIndemnites()
                    .LocalisationBordeaux()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .Build();
            var ci = BuilderCi.CiId(1).Societe(BuilderSociete.SocieteFTP()).LocalisationArcachon().Build();
            Actual.CanCalculateIndemniteDeplacement(personnel, ci)
                .Should()
                .Match<Tuple<bool, List<string>>>(c => !c.Item1 && c.Item2.Contains(FeatureIndemniteDeplacement.IndemniteDeplacement_Warning_No_GPS_Personnel));
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CanCalculateIndemniteDeplacement_WithSecteurNoGPSForTypeSecteur_ReturnsErrorsGPSSecteur()
        {
            var personnel = BuilderPersonnel.PersonnelId(1)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementSecteur()
                .LocalisationPessac()
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .EtablissementPaie(BuilderEtab
                    .GestionIndemnites()
                    .LocalisationBordeaux()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .Build();
            var ci = BuilderCi.CiId(1).Societe(BuilderSociete.SocieteFTP()).LocalisationArcachon().Build();
            Actual.CanCalculateIndemniteDeplacement(personnel, ci)
                .Should()
                .Match<Tuple<bool, List<string>>>(c => !c.Item1 && c.Item2.Contains(FeatureIndemniteDeplacement.IndemniteDeplacement_Warning_No_GPS_Secteur));
        }

        [TestMethod]
        [TestCategory("IndemniteDeplacementManager")]
        public void CanCalculateIndemniteDeplacement_WithAgenceNoGPSForTypeAgence_ReturnsErrorsGPSAgence()
        {
            var personnel = BuilderPersonnel.PersonnelId(1)
                .Societe(BuilderSociete.SocieteFTP())
                .TypeRattachementAgence()
                .LocalisationPessac()
                .EtablissementRattachement(BuilderEtab
                    .GestionIndemnites()
                    .LocalisationBordeaux()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .EtablissementPaie(BuilderEtab
                    .GestionIndemnites()
                    .Societe(BuilderSociete.SocieteFTP())
                    .Build())
                .Build();
            var ci = BuilderCi.CiId(1).Societe(BuilderSociete.SocieteFTP()).LocalisationArcachon().Build();
            Actual.CanCalculateIndemniteDeplacement(personnel, ci)
                .Should()
                .Match<Tuple<bool, List<string>>>(c => !c.Item1 && c.Item2.Contains(FeatureIndemniteDeplacement.IndemniteDeplacement_Warning_No_GPS_Agence));
        }
    }
}
