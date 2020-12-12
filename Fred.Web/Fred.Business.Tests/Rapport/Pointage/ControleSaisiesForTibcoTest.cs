using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Fred.Business.CI;
using Fred.Business.FeatureFlipping;
using Fred.Business.Personnel;
using Fred.Business.Rapport.Pointage;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Astreinte.Builder;
using Fred.Common.Tests.Data.CI.Builder;
using Fred.Common.Tests.Data.Personnel.Builder;
using Fred.Common.Tests.Data.Rapport.Builder;
using Fred.Common.Tests.Data.Referential.Etablissement.Builder;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Personnel;
using Fred.DataAccess.Rapport.Pointage;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Rapport.Pointage
{
    [TestClass]
    public class ControleSaisiesForTibcoTest : BaseTu<PointageManager>
    {
        private PersonnelEnt PersonnelExpected;

        [TestInitialize]
        public void TestInitialize()
        {
            var builder = new RapportLigneBuilder();
            var builderPersonnel = new PersonnelBuilder();
            var builderCi = new CiBuilder();
            var builderCodeAstreinte = new CodeAstreinteBuilder();
            var builderRapportLigneAstreinte = new RapportLigneAstreinteBuilder();
            var builderEtablissementsPaie = new EtablissementPaieBuilder();
            var context = GetMocked<FredDbContext>();
            PersonnelExpected = builderPersonnel.Prerequi().EtablissementPaieWithComptaDefault().Build();
            context.Object.RapportLignes = builder.BuildFakeDbSet(builder
                        .Rapport(new RapportBuilder().Prerequi().NePasVerrouiller().Build())
                        .DatePointage(new DateTime(2019, 01, 02))
                        .Personnel(PersonnelExpected)
                        .Ci(builderCi.Prototype())
                        .CodeAbsence(new CodeAbsenceEnt { Code = "ABSENCE", CodeAbsenceId = 1 })
                        .ListRapportLignePrimes(new List<RapportLignePrimeEnt> { new RapportLignePrimeBuilder().Prototype() })
                        .ListRapportLigneTaches(new List<RapportLigneTacheEnt>() { new RapportLigneTacheBuilder().Prototype() })
                        .ListRapportLigneAstreintes(new List<RapportLigneAstreinteEnt>())
                        .ListRapportLigneMajorations(new List<RapportLigneMajorationEnt>())
                        .Build(),
                        builder.RapportLigneStatutId(5)
                        .DatePointage(new DateTime(2019, 01, 15))
                        .Personnel(builderPersonnel.Prerequi().EtablissementPaieWithComptaDefault().Build())
                        .Ci(builderCi.Prototype())
                        .CodeAbsence(new CodeAbsenceEnt { Code = "RT", CodeAbsenceId = 2 })
                        .Build(),
                        builder.RapportLigneStatutId(5)
                        .Rapport(new RapportBuilder().Prerequi().Verrouiller().Build())
                        .DatePointage(new DateTime(2019, 01, 16))
                        .ListRapportLigneTaches(new List<RapportLigneTacheEnt>() { new RapportLigneTacheBuilder().Prototype() })
                        .Personnel(builderPersonnel.Prerequi().EtablissementPaieWithComptaDefault().Build())
                        .Ci(builderCi.Prototype())
                        .CodeAbsence(new CodeAbsenceEnt { Code = "RT", CodeAbsenceId = 2 })
                        .Build()
                        );
            context.Object.CodeAstreintes = builderCodeAstreinte.BuildFakeDbSet(2);
            context.Object.RapportLigneAstreintes = builderRapportLigneAstreinte.BuildFakeDbSet(2);
            context.Object.EtablissementsPaie = builderEtablissementsPaie.BuildFakeDbSet(PersonnelExpected.EtablissementPaie);
            context.Object.Personnels = builderPersonnel.BuildFakeDbSet(PersonnelExpected);
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(context.Object, securityManager.Object);
            var stubPointageRepository = new PointageRepository(context.Object);
            var personnelRepository = new PersonnelRepository(context.Object);
            var validator = new PointageValidator(GetMocked<IPersonnelManager>().Object, GetMocked<ICIManager>().Object, GetMocked<IPointageRepository>().Object, GetMocked<IFeatureFlippingManager>().Object);
            SubstituteConstructorArgument<IPointageValidator>(validator);
            SubstituteConstructorArgument<IPointageRepository>(stubPointageRepository);
            SubstituteConstructorArgument<IPersonnelRepository>(personnelRepository);
        }

        [TestMethod]
        public void ControleSaisiesForTibco_WithInvalideInput_ThrowsException()
        {
            Invoking(() => Actual.ControleSaisiesForTibco(null)).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ControleSaisiesForTibco_WithValidInPut_ContainsPersonnelInfos()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = true,
                Hebdo = false,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1 }
            };
            var result = Actual.ControleSaisiesForTibco(exportPointagePersonnelOptionsModel).ToList().FirstOrDefault();
            result.Should().Match<ControleSaisiesTibcoModel>(c => !string.IsNullOrEmpty(c.PersonnelNom)
            && !string.IsNullOrEmpty(c.PersonnelNom)
            && !string.IsNullOrEmpty(c.PersonnelMatricule)
            && !string.IsNullOrEmpty(c.EtablissementCode));
        }

        [TestMethod]
        public void ControleSaisiesForTibco_WithValidInPut_ErrorsListContainsNonVerroulle()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = true,
                Hebdo = false,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1 }
            };
            var result = Actual.ControleSaisiesForTibco(exportPointagePersonnelOptionsModel);
            result.Should().Contain(c => c.Erreurs.Any(e => e.Message == FeatureRapport.RapportLigneValidator_StatutNonVerrouille));
        }

        [TestMethod]
        public void ControleSaisiesForTibco_WithValidInPut_ErrorsListContainsIncomplet()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = true,
                Hebdo = false,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1 }
            };
            var result = Actual.ControleSaisiesForTibco(exportPointagePersonnelOptionsModel);
            result.Should().Contain(c => c.Erreurs.Any(e => e.Message == FeatureRapport.RapportLigneValidator_DonneeIncomplete));
        }

        [TestMethod]
        public void ControleSaisiesForTibco_WithValidInPut_ErrorsListManquant()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = true,
                Hebdo = false,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1 }
            };
            var result = Actual.ControleSaisiesForTibco(exportPointagePersonnelOptionsModel);
            result.Should().Contain(c => c.Erreurs.Any(e => e.Message == FeatureRapport.RapportLigneValidator_DonneeManquante));
        }

        [TestMethod]
        public void ControleSaisiesForTibco_WithValidInPut_ContainSingle()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = true,
                Hebdo = false,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1 }
            };
            var result = Actual.ControleSaisiesForTibco(exportPointagePersonnelOptionsModel);
            result.Should().ContainSingle(c => c.PersonnelNom == PersonnelExpected.Nom);
        }

        [TestMethod]
        public void ControleSaisiesForTibco_WithJanuaryInPut_ContainThirtyOneDays()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = true,
                Hebdo = false,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1 }
            };
            var ligne = Actual.ControleSaisiesForTibco(exportPointagePersonnelOptionsModel).FirstOrDefault();

            ligne.Erreurs.Should().HaveCount(23);
        }

        [TestMethod]
        public void ControleSaisiesForTibco_WithJanuaryInPut_OrderByDate()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = true,
                Hebdo = false,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1 }
            };
            var ligne = Actual.ControleSaisiesForTibco(exportPointagePersonnelOptionsModel).FirstOrDefault();

            ligne.Erreurs.FirstOrDefault().DateRapport.Should().Equals(new DateTime(2019, 01, 01));
            ligne.Erreurs.LastOrDefault().DateRapport.Should().Equals(new DateTime(2019, 01, 31));
        }

        [TestMethod]
        public void ControleSaisiesForTibco_WithHebdoInPut_HasSevenDays()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = true,
                Hebdo = true,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1 }
            };
            var ligne = Actual.ControleSaisiesForTibco(exportPointagePersonnelOptionsModel).FirstOrDefault();

            ligne.Erreurs.Should().HaveCount(5);
        }

        [TestMethod]
        public void ControleSaisiesForTibco_WithHebdoInPut_OrderByDate()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = true,
                Hebdo = true,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1 }
            };
            var ligne = Actual.ControleSaisiesForTibco(exportPointagePersonnelOptionsModel).FirstOrDefault();

            ligne.Erreurs.FirstOrDefault().DateRapport.Should().Equals(new DateTime(2019, 01, 01));
            ligne.Erreurs.LastOrDefault().DateRapport.Should().Equals(new DateTime(2019, 01, 07));
        }
    }
}
