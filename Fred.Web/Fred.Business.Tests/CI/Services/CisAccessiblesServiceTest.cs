using System.Collections.Generic;
using FluentAssertions;
using Fred.Business.CI.Services;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.CI.Builder;
using Fred.Common.Tests.Data.Depense.Builder;
using Fred.Common.Tests.Data.Personnel.Builder;
using Fred.Common.Tests.Data.Rapport.Builder;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Rapport;
using Fred.Entities.Rapport;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.CI.Services
{
    /// <summary>
    /// Classe de Test de <see cref="CisAccessiblesService"/>
    /// </summary>
    [TestClass]
    public class CisAccessiblesServiceTest : BaseTu<CisAccessiblesService>
    {
        [TestMethod]
        public void GetCiIdsAvailablesForReceptionInterimaire_WhenNothingToSend_Returns_EmptyList()
        {
            //Builders
            var builderRapportLigne = new RapportLigneBuilder();
            //Context Fake
            var context = GetMocked<FredDbContext>();

            //liste des rapports lignes vérrouillé
            context.Object.RapportLignes = builderRapportLigne.BuildFakeDbSet();

            //Real Uow
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(context.Object, securityManager.Object);
            //real DepenseRepository
            IRapportRepository rapportRepository = new RapportRepository(context.Object, GetMocked<ILogManager>().Object);

            //injection
            SubstituteConstructorArgument<IRapportRepository>(rapportRepository);
            //Act et Assert 
            Actual.GetCiIdsAvailablesForReceptionInterimaire(new List<int>() { 1, 2, 3, 4 }).Should().BeEmpty();
        }

        [TestMethod]
        public void GetCiIdsAvailablesForReceptionInterimaire_WhenOneCiAvailable_Returns_OnlyOne()
        {
            //Builders
            var builderCi = new CiBuilder();
            var builderdepense = new DepenseAchatBuilder();
            var builderRapportLigne = new RapportLigneBuilder();
            //Context Fake
            var context = GetMocked<FredDbContext>();

            //3 rapports dont 1 vérrouillée et 1 déjà receptionné
            context.Object.RapportLignes = builderRapportLigne.BuildFakeDbSet(
                builderRapportLigne
                    .Rapport(new RapportBuilder().RapportStatutId(RapportStatutEnt.RapportStatutVerrouille.Key).Build())
                    .CiId(1)
                    .Personnel(new PersonnelBuilder().IsInterimaire().Build())
                    .Build(),
                builderRapportLigne
                    .Rapport(new RapportBuilder().RapportStatutId(RapportStatutEnt.RapportStatutVerrouille.Key).Build())
                    .CiId(1)
                    .Personnel(new PersonnelBuilder().IsInterimaire().Build())
                    .Build(),
                builderRapportLigne
                    .Rapport(new RapportBuilder().RapportStatutId(RapportStatutEnt.RapportStatutEnCours.Key).Build())
                    .CiId(2)
                    .Personnel(new PersonnelBuilder().IsInterimaire().Build())
                    .Build(),
                builderRapportLigne
                    .Rapport(new RapportBuilder().RapportStatutId(RapportStatutEnt.RapportStatutVerrouille.Key).Build())
                    .CiId(3)
                    .Personnel(new PersonnelBuilder().IsInterimaire().Build())
                    .ReceptionInterimaire()
                    .Build()
            );

            //Real Uow
            var securityManager = GetMocked<ISecurityManager>();
            var uow = new UnitOfWork(context.Object, securityManager.Object);
            //real DepenseRepository
            IRapportRepository rapportRepository = new RapportRepository(context.Object, GetMocked<ILogManager>().Object);

            //injection
            SubstituteConstructorArgument<IRapportRepository>(rapportRepository);

            //Act et Assert 
            Actual.GetCiIdsAvailablesForReceptionInterimaire(new List<int>() { 1, 2, 3, 4 }).Should().OnlyContain(c => c.Equals(1)).And.HaveCount(1);
        }
    }
}
