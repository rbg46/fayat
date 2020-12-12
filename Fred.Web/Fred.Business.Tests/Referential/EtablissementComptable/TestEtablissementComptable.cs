using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Organisation.Mock;
using Fred.Common.Tests.Data.Referential.Etablissement.Builder;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Organisation;
using Fred.Entities;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Referential.EtablissementComptable
{
    [TestClass]
    public class TestEtablissementComptable : BaseTu<EtablissementComptableManager>
    {
        private EtablissementComptableBuilder etablissementComptableBuilder;
        private UtilisateurBuilder userBuilder;
        private List<EtablissementComptableEnt> arrangedEtablissementComptableList;
        private List<SocieteEnt> societesList;
        private List<TypeOrganisationEnt> typeOrgaList;
        private SocieteBuilder societeBuilder;
        private Mock<FredDbContext> context;
        private IOrganisationRepository organisationRepositoryReal;
        private int organisationIdSocietePere = 11;
        private int organisationIdSociete = 1111;
        private int societeId = 1;

        [TestInitialize]
        public void Initialize()
        {
            arrangedEtablissementComptableList = new List<EtablissementComptableEnt>();
            etablissementComptableBuilder = new EtablissementComptableBuilder();
            userBuilder = new UtilisateurBuilder();
            societeBuilder = new SocieteBuilder();

            typeOrgaList = OrganisationBaseMocks.GetFakeDbSetTypeOrganisation().ToList();

            var societeOrganisation = new OrganisationEnt();

            societesList = new List<SocieteEnt>
            {
                //societe utilisee pour les tests de creation d'organisation
                societeBuilder.SocieteId(societeId).Organisation(organisationIdSociete, organisationIdSocietePere).Build(),
                societeBuilder.SocieteFTP(),
            };

            arrangedEtablissementComptableList.Add(etablissementComptableBuilder.Build());

            this.context = GetMocked<FredDbContext>();
            context.Object.OrganisationTypes = OrganisationBaseMocks.GetFakeDbSetTypeOrganisation();

            organisationRepositoryReal = new OrganisationRepository(context.Object, null, null);

            var etablissementComptableRepository = GetMocked<IEtablissementComptableRepository>();
            etablissementComptableRepository.Setup(m => m.GetEtablissementComptableList())
                .Returns(arrangedEtablissementComptableList);
            //etablissementComptableRepository.Setup(m => m.GetEtablissementPaieById(It.IsAny<int>()))
            //    .Returns<int>(id =>
            //    {
            //        return arrangedEtablissementComptableList.Find(x => x.EtablissementPaieId == id);
            //    });
            etablissementComptableRepository.Setup(m => m.AddEtablissementComptable(It.IsAny<EtablissementComptableEnt>()))
                .Callback<EtablissementComptableEnt>(x => arrangedEtablissementComptableList.Add(x));

            //var organisationRepository = GetMocked<IOrganisationRepository>();
            //organisationRepository.Setup(m => m.GenerateOrganisation(It.IsAny<string>(), It.IsAny<OrganisationEnt>()))
            //    .Returns<string, OrganisationEnt>((codeOrga, orgaParent) =>
            //    {
            //        return organisationRepositoryReal.GenerateOrganisation(codeOrga, orgaParent);
            //    });
            //organisationRepository.Setup(m => m.GenerateOrganisation(It.IsAny<string>(), It.IsAny<int>()))
            //    .Returns<string, int>((codeOrga, orgaParentId) =>
            //    {
            //        return organisationRepositoryReal.GenerateOrganisation(codeOrga, orgaParentId);
            //    });
            //organisationRepository.Setup(m => m.GetTypeOrganisationIdByCode(It.IsAny<string>()))
            //    .Returns<string>((codeTypeOrga) =>
            //    {
            //        return OrganisationBaseMocks.GetFakeDbSetTypeOrganisation().FirstOrDefault(x => x.Code == codeTypeOrga).TypeOrganisationId;
            //    });

            //etablissementComptableRepository.Setup(m => m.GetEtablissementPaieById(It.IsAny<int>()))
            //    .Returns<int>(id =>
            //    {
            //        return arrangedEtablissementComptableList.Find(x => x.EtablissementPaieId == id);
            //    });
            etablissementComptableRepository.Setup(m => m.AddEtablissementComptable(It.IsAny<EtablissementComptableEnt>()))
                .Callback<EtablissementComptableEnt>(x => arrangedEtablissementComptableList.Add(x));

            var securityManager = GetMocked<ISecurityManager>();
            var unitOfWork = new UnitOfWork(context.Object, securityManager.Object);
            SubstituteConstructorArgument<IUnitOfWork>(unitOfWork);

            var utilisateurManager = GetMocked<IUtilisateurManager>();
            var user = userBuilder.Prototype();
            user.SuperAdmin = false;
            utilisateurManager.Setup(u => u.GetContextUtilisateur()).Returns(user);

            SubstituteConstructorArgument<IUtilisateurManager>(utilisateurManager.Object);
            SubstituteConstructorArgument<IEtablissementComptableRepository>(etablissementComptableRepository.Object);
            //SubstituteConstructorArgument<IOrganisationRepository>(organisationRepository.Object);
            SubstituteConstructorArgument<IOrganisationRepository>(organisationRepositoryReal);
        }

        private void SetupSocieteManager(int societeIdSetup)
        {
            var societeManager = GetMocked<ISocieteManager>();
            societeManager.Setup(m => m.GetOrganisationIdBySocieteId(It.IsAny<int?>()))
                .Returns<int?>((societeId) =>
                {
                    return societesList.FirstOrDefault(x => x.SocieteId == societeIdSetup).OrganisationId;
                });

            SubstituteConstructorArgument<ISocieteManager>(societeManager.Object);
        }

        /// <summary>
        ///   Teste que la liste des établissements comptables n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("Test EtablissementComptable")]
        public void GetEtablissementListReturnNotNullList()
        {
            //var etablissementsComptables = EtabComptableMgr.GetEtablissementComptableList();
            //Assert.IsTrue(etablissementsComptables != null);

            //1. Arrange data
            var expectedNumberOfEtablissement = arrangedEtablissementComptableList.Count();

            //2. Act
            var etablissements = Actual.GetEtablissementComptableList();

            //3. Assert
            etablissements.Should().HaveCount(expectedNumberOfEtablissement);
        }

        /// <summary>
        ///   Teste la récupération de tous les établissements de paie éligibles à être une agence de rattachement pour un
        ///   établissement donné
        /// </summary>
        [TestMethod]
        [TestCategory("Test EtablissementComptable")]
        public void AddEtablissementComptable_WithCodeAndId_ShouldCreateEtablissementAndOrganisation()
        {
            //1. Arrange data
            var societeIdPereDeLEtablissement = 1;
            SetupSocieteManager(societeIdPereDeLEtablissement);

            var expectedIdEtablissementComptable = 77;
            EtablissementComptableEnt etablissement = etablissementComptableBuilder
                .EtablissementComptableId(expectedIdEtablissementComptable)
                .SocieteId(societeIdPereDeLEtablissement)
                .Build();

            //2. Act
            Actual.AddEtablissementComptable(etablissement);

            //3. Assert the id is added
            arrangedEtablissementComptableList.Should().Contain(etablissement)
                .And.Match(et => et.Any(x => x.Organisation != null
                    && x.Organisation.TypeOrganisationId == typeOrgaList.FirstOrDefault(x => x.Code == Constantes.OrganisationType.CodeEtablissement).TypeOrganisationId));
        }

        ///// <summary>
        /////   Teste la mise à jour d'un enregistrement existant en base de données.
        /////   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        ///// </summary>
        //[TestMethod]
        //[TestCategory("DBDependNotMaintained")]
        //[Ignore]
        //public void UpdateExistingEtablissementComptable()
        //{


        //    EtablissementComptableEnt etablissementBefore = new EtablissementComptableEnt
        //    {
        //        Code = "ZZ",
        //        Libelle = "Etablissement (test de création)",
        //        SocieteId = 1
        //    };

        //    int etablissementComptableId = EtabComptableMgr.AddEtablissementComptable(etablissementBefore);

        //    string libBefore = etablissementBefore.Libelle;
        //    etablissementBefore.Libelle = "Etablissement (test de modification)";

        //    EtabComptableMgr.UpdateEtablissementComptable(etablissementBefore);

        //    EtablissementComptableEnt etablissementAfter = EtabComptableMgr.GetEtablissementComptableById(etablissementComptableId);

        //    Assert.AreNotEqual(libBefore, etablissementAfter.Libelle);
        //}

        ///// <summary>
        /////   Teste la suppression d'un nouvel enregistrement en base de données.
        /////   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        ///// </summary>
        //[TestMethod]
        //[TestCategory("Test EtablissementComptable")]
        //public void DeleteExistingEtablissementComptable()
        //{
        //    EtablissementComptableEnt etablissement = new EtablissementComptableEnt
        //    {
        //        Code = GenerateString(6),
        //        Libelle = "Etablissement (test de création)",
        //        SocieteId = 1
        //    };

        //    int etablissementComptableId = EtabComptableMgr.AddEtablissementComptable(etablissement);

        //    int countBefore = EtabComptableMgr.GetEtablissementComptableList().Count();

        //    EtabComptableMgr.DeleteEtablissementComptableById(etablissement);

        //    int countAfter = EtabComptableMgr.GetEtablissementComptableList().Count();

        //    Assert.IsTrue(countBefore == (countAfter + 1));
        //}

        ///// <summary>
        /////   Teste de recupération des établissement comptables via societeId
        ///// </summary>
        //[TestMethod]
        //[TestCategory("Test EtablissementComptable")]
        //public void GetListBySocieteId()
        //{
        //    int societeId = 1;

        //    var etablissementsComptables = EtabComptableMgr.GetListBySocieteId(societeId);
        //    Assert.IsTrue(etablissementsComptables != null);
        //}

        ///// <summary>
        /////   Teste le refus d'import d'un établissement comptable existant
        ///// </summary>
        //[TestMethod]
        //[TestCategory("DBDependNotMaintained")]
        //[Ignore]
        //public void ImportExistingEtablissementComptable()
        //{


        //    // Déclaration d'un établissement comptable existant
        //    EtablissementComptableEnt etablissement = new EtablissementComptableEnt
        //    {
        //        Code = "01",
        //        Libelle = "DRN- Idf Est",
        //        SocieteId = 1
        //    };
        //    List<EtablissementComptableEnt> ets = new List<EtablissementComptableEnt>();
        //    ets.Add(etablissement);
        //    bool test = EtabComptableMgr.ManageImportedEtablissementComptables(ets);

        //    Assert.IsFalse(test);
        //}

        ///// <summary>
        /////   Teste dla réussite d'import d'un nouvel établissement comptable
        ///// </summary>
        //[TestMethod]
        //[TestCategory("Test EtablissementComptable")]
        //public void ImportNewEtablissementComptable()
        //{


        //    // Déclaration d'un nouvel établissement comptable
        //    EtablissementComptableEnt etablissement = new EtablissementComptableEnt
        //    {
        //        Code = GenerateString(6),
        //        Libelle = "Etablissement fictif de test",
        //        SocieteId = 1
        //    };

        //    List<EtablissementComptableEnt> ets = new List<EtablissementComptableEnt>();
        //    ets.Add(etablissement);

        //    bool test = EtabComptableMgr.ManageImportedEtablissementComptables(ets);

        //    Assert.IsTrue(test);

        //    EtabComptableMgr.DeleteEtablissementComptableById(etablissement);
        //}
    }
}
