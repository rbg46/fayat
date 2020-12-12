using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Fred.Business.Referential;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Referential.Etablissement.Builder;
using Fred.Common.Tests.Data.Referential.Prime.Mock;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Referential.EtablissementPaie
{
    [TestClass]
    public class TestEtablissementPaie : BaseTu<EtablissementPaieManager>
    {
        private EtablissementPaieBuilder etablissementPaieBuilder;
        private UtilisateurBuilder userBuilder;
        private PrimeMocks mocks;
        private List<EtablissementPaieEnt> arrangedEtablissementPaieList;
        private Mock<FredDbContext> context;

        [TestInitialize]
        public void Initialize()
        {
            arrangedEtablissementPaieList = new List<EtablissementPaieEnt>();
            etablissementPaieBuilder = new EtablissementPaieBuilder();
            userBuilder = new UtilisateurBuilder();
            mocks = new PrimeMocks();

            arrangedEtablissementPaieList.Add(etablissementPaieBuilder.Societe(new SocieteBuilder().SocieteId(3).Build()).Build());

            var etablissementPaieRepository = GetMocked<IEtablissementPaieRepository>();
            etablissementPaieRepository.Setup(m => m.GetEtablissementPaieList())
                .Returns(arrangedEtablissementPaieList);
            etablissementPaieRepository.Setup(m => m.GetEtablissementPaieById(It.IsAny<int>()))
                .Returns<int>(id =>
                {
                    return arrangedEtablissementPaieList.Find(x => x.EtablissementPaieId == id);
                });
            etablissementPaieRepository.Setup(m => m.AddEtablissementPaie(It.IsAny<EtablissementPaieEnt>()))
                .Callback<EtablissementPaieEnt>(x => arrangedEtablissementPaieList.Add(x));

            this.context = GetMocked<FredDbContext>();
            var securityManager = GetMocked<ISecurityManager>();

            var unitOfWork = new UnitOfWork(context.Object, securityManager.Object);
            SubstituteConstructorArgument<IUnitOfWork>(unitOfWork);

            var utilisateurManager = GetMocked<IUtilisateurManager>();
            var user = userBuilder.Prototype();
            user.SuperAdmin = false;
            utilisateurManager.Setup(u => u.GetContextUtilisateur()).Returns(user);

            SubstituteConstructorArgument<IUtilisateurManager>(utilisateurManager.Object);
            SubstituteConstructorArgument<IEtablissementPaieRepository>(etablissementPaieRepository.Object);
        }

        /// <summary>
        ///   Teste que la liste des établissements de paie retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("Test EtablissementPaie")]
        public void GetEtablissementPaieList_ShouldHaveAllEtablissement()
        {
            //1. Arrange data
            var expectedNumberOfEtablissement = arrangedEtablissementPaieList.Count();

            //2. Act
            var etablissements = Actual.GetEtablissementPaieList();

            //3. Assert
            etablissements.Should().HaveCount(expectedNumberOfEtablissement);
        }

        /// <summary>
        ///   Teste la récupération de tous les établissements de paie éligibles à être une agence de rattachement pour un
        ///   établissement donné
        /// </summary>
        [TestMethod]
        [TestCategory("Test EtablissementPaie")]
        public void AgencesDeRattachement()
        {
            //1. Arrange data
            var expectedIdEtablissementPaie = 1;
            EtablissementPaieEnt etablissement = etablissementPaieBuilder
                .EtablissementPaieId(expectedIdEtablissementPaie)
                .Build();

            //2. Act
            Actual.AddEtablissementPaie(etablissement);
            Actual.AgencesDeRattachement(etablissement.EtablissementPaieId);

            //3. Assert the id is added
            arrangedEtablissementPaieList.Should().Contain(etablissement)
                .And.Match(x => x.Any(x => x.EtablissementPaieId == expectedIdEtablissementPaie));
        }

        /// <summary>
        ///   Teste La récupération d'un établissement de paie portant l'identifiant unique indiqué.
        /// </summary>
        [TestMethod]
        [TestCategory("Test EtablissementPaie")]
        public void GetEtablissementPaieById()
        {
            //1. Arrange data
            var expectedIdEtablissementPaie = 1;
            EtablissementPaieEnt etablissement = etablissementPaieBuilder
                .EtablissementPaieId(expectedIdEtablissementPaie)
                .Build();

            //2. Act
            Actual.AddEtablissementPaie(etablissement);
            EtablissementPaieEnt etablissementReturned = Actual.GetEtablissementPaieById(expectedIdEtablissementPaie);

            //3. Assert the id is added
            etablissement.Should().Match<EtablissementPaieEnt>(x => x.EtablissementPaieId == expectedIdEtablissementPaie);
        }
    }
}