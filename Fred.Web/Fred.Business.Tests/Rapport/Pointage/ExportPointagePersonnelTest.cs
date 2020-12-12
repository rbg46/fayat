using System;
using System.Collections.Generic;
using FluentAssertions;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Astreinte.Builder;
using Fred.Common.Tests.Data.CI.Builder;
using Fred.Common.Tests.Data.Personnel.Builder;
using Fred.Common.Tests.Data.Rapport.Builder;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Rapport.Pointage;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Fred.Web.Shared.Models.Rapport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Rapport.Pointage
{
    /// <summary>
    /// Classe de test pour l'export des pointage du personnel
    /// </summary>
    [TestClass]
    public class ExportPointagePersonnelTest : BaseTu<PointageManager>
    {

        [TestInitialize]
        public void TestInitialize()
        {
            var builder = new RapportLigneBuilder();
            var builderPersonnel = new PersonnelBuilder();
            var builderCi = new CiBuilder();
            var builderRapport = new RapportBuilder();
            var builderCodeAstreinte = new CodeAstreinteBuilder();
            var builderRapportLigneAstreinte = new RapportLigneAstreinteBuilder();
            var context = GetMocked<FredDbContext>();
            context.Object.RapportLignes = builder.BuildFakeDbSet(builder
                        .Rapport(builderRapport.Prototype())
                        .DatePointage(new DateTime(2019, 01, 02))
                        .Personnel(builderPersonnel.Prerequi().EtablissementPaieWithComptaDefault().Build())
                        .Ci(builderCi.Prototype())
                        .CodeAbsence(new CodeAbsenceEnt { Code = "ABSENCE", CodeAbsenceId = 1 })
                        .ListRapportLignePrimes(new List<RapportLignePrimeEnt> { new RapportLignePrimeBuilder().Prototype() })
                        .ListRapportLigneTaches(new List<RapportLigneTacheEnt>() { new RapportLigneTacheBuilder().Prototype() })
                        .ListRapportLigneAstreintes(new List<RapportLigneAstreinteEnt>())
                        .ListRapportLigneMajorations(new List<RapportLigneMajorationEnt>())
                        .Build(),
                        builder
                        .Rapport(builderRapport.Prerequi().NePasVerrouiller().Build())
                        .DatePointage(new DateTime(2019, 01, 15))
                        .Personnel(builderPersonnel.Prerequi().EtablissementPaieWithComptaDefault().Build())
                        .Ci(builderCi.Prototype())
                        .CodeAbsence(new CodeAbsenceEnt { Code = "RT", CodeAbsenceId = 2 })
                        .Build()
                        ,
                        builder.RapportLigneStatutId(5)
                        .Rapport(builderRapport.Prerequi().Verrouiller().Build())
                        .DatePointage(new DateTime(2019, 01, 16))
                        .Personnel(builderPersonnel.Prerequi().EtablissementPaieWithComptaDefault().Build())
                        .Ci(builderCi.Prototype())
                        .CodeAbsence(new CodeAbsenceEnt { Code = "RT", CodeAbsenceId = 2 })
                        .Build()
                        );
            context.Object.CodeAstreintes = builderCodeAstreinte.BuildFakeDbSet(2);
            context.Object.RapportLigneAstreintes = builderRapportLigneAstreinte.BuildFakeDbSet(2);
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(context.Object, securityManager.Object);
            var stubPointageRepository = new PointageRepository(context.Object);
            SubstituteConstructorArgument<IPointageRepository>(stubPointageRepository);
            GetMocked<IUtilisateurManager>().Setup(u => u.GetContextUtilisateur()).Returns(new UtilisateurBuilder().Prototype());
        }

        [TestMethod]
        [TestCategory("PointageManager")]
        public void GetPointagePersonnelForTibCo_WithNullInput_ThrowsException()
        {
            Invoking(() => Actual.GetPointagePersonnelForTibco(null)).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        [TestCategory("PointageManager")]
        public void GetPointagePersonnelForTibCo_WithEmptyListEtablissements_ThrowsException()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                EtablissementsComptablesCodes = new List<string>(),
                EtablissementsComptablesIds = new List<int>()
            };
            Invoking(() => Actual.GetPointagePersonnelForTibco(exportPointagePersonnelOptionsModel)).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        [TestCategory("PointageManager")]
        public void GetPointagePersonnelForTibCo_Semaine_Returns_LigneRapports_WithAllStatutsRapports()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = true,
                Hebdo = true,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1, 2 }
            };
            var result = Actual.GetPointagePersonnelForTibco(exportPointagePersonnelOptionsModel);
            result.RapportLignes.Should().ContainSingle();
        }

        [TestMethod]
        [TestCategory("PointageManager")]
        public void GetPointagePersonnelForTibCo_MoisWithSimulation_Returns_LigneRapports_WithAllStatutsRapports()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = true,
                Hebdo = false,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1, 2 }
            };
            var result = Actual.GetPointagePersonnelForTibco(exportPointagePersonnelOptionsModel);
            result.RapportLignes.Should().NotBeEmpty();
        }

        [TestMethod]
        [TestCategory("PointageManager")]
        public void GetPointagePersonnelForTibCo_MoisNoSimulation_Returns_LigneRapports_WithRapportsLocked()
        {
            ExportPointagePersonnelFilterModel exportPointagePersonnelOptionsModel = new ExportPointagePersonnelFilterModel
            {
                UserId = 1,
                DateDebut = new DateTime(2019, 01, 01),
                Simulation = false,
                Hebdo = false,
                EtablissementsComptablesCodes = new List<string> { "E001" },
                EtablissementsComptablesIds = new List<int> { 1, 2 }
            };

            var result = Actual.GetPointagePersonnelForTibco(exportPointagePersonnelOptionsModel);
            result.RapportLignes.Should().OnlyContain(r => r.IsStatutVerrouille);
        }
    }
}
