using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Fred.Business.AffectationSeuilUtilisateur;
using Fred.Business.CI;
using Fred.Business.Parametre;
using Fred.Business.Personnel;
using Fred.Business.Referential.TypeRattachement;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Affectation.Mock;
using Fred.Common.Tests.Data.Personnel.Builder;
using Fred.Common.Tests.Data.Personnel.Interimaire.Mock;
using Fred.Common.Tests.Data.Personnel.Mock;
using Fred.Common.Tests.Data.Societe.Mock;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Personnel;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Security;
using Fred.Framework.Services.Google;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Personnel
{
    /// <summary>
    /// Attribut pour passer en mode test de query()
    /// </summary>
    internal class ModeQueryAttribute : Attribute
    {
    }

    /// <summary>
    /// Attribut pour passer en mode interim avec liste des intérims seulement
    /// </summary>
    internal class ModeInterimAttribute : Attribute
    {
    }

    /// <summary>
    /// Attribut pour passer en mode fes
    /// </summary>
    internal class ModeFesAttribute : Attribute
    {
    }

    [TestClass]
    public class PersonnelManagerTest : BaseTu<PersonnelManager>
    {
        private Mock<IPersonnelRepository> personnelRepository;
        private Mock<ISecurityManager> securityManager;
        private List<PersonnelEnt> anaelPersonnels;
        private List<PersonnelEnt> fredPersonnels;
        private readonly SearchPersonnelModelBuilder SearchPersonnelBuilder = new SearchPersonnelModelBuilder();
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            var mocks = new PersonnelMocks();
            var societeMocks = new SocieteMocks();

            // TRES IMPORTANT: MOQUER LE SERVICE DE GEOCODAGE POUR NE PAS UTILISE LE QUOTA
            GeocodeResult geoResponse = null;
            var geocodeService = GetMocked<IGeocodeService>();
            geocodeService.Setup(service => service.Geocode(It.IsAny<Address>())).Returns(geoResponse);
            personnelRepository = GetMocked<IPersonnelRepository>();
            securityManager = GetMocked<ISecurityManager>();

            var methode = GetType().GetMethod(TestContext.TestName);
            bool modeQuery = methode.GetCustomAttributes(typeof(ModeQueryAttribute), false).Any();
            bool modeInterim = methode.GetCustomAttributes(typeof(ModeInterimAttribute), false).Any();
            bool modeFes = methode.GetCustomAttributes(typeof(ModeFesAttribute), false).Any();

            anaelPersonnels = mocks.GetAnaelPersonnel();
            fredPersonnels = mocks.GetFakeDbSet(modeFes).ToList();

            if (modeQuery)
            {
                var fakeContext = mocks.GetFakeContext(modeInterim, modeFes);
                var uow = new UnitOfWork(fakeContext, securityManager.Object);
                var repository = new PersonnelRepository(fakeContext);

                SubstituteConstructorArgument<IPersonnelRepository>(repository);
                SubstituteConstructorArgument<IUnitOfWork>(uow);
                var cimanager = GetMocked<ICIManager>();
                cimanager.Setup(m => m.GetEtablissementComptableByCIId(It.IsAny<int>())).Returns(new EtablissementComptableEnt());
                cimanager.Setup(m => m.GetSocieteByCIId(It.IsAny<int>(), It.IsAny<bool>())).Returns(mocks.GetDefaultSociete(modeFes));
                var sepService = GetMocked<ISepService>();
                sepService.Setup(s => s.IsSep(It.IsAny<SocieteEnt>())).Returns(true);
                sepService.Setup(s => s.GetSocieteParticipantes(It.IsAny<int>())).Returns(new SocieteMocks().GetFakeDbSet().ToList());
            }

            var societeManager = GetMocked<ISocieteManager>();
            societeManager.Setup(mgr => mgr.GetSocieteById(It.IsAny<int>(), false)).Returns(mocks.GetDefaultSociete(modeFes));
            societeManager.Setup(mgr => mgr.GetSocieteInterim(It.IsAny<int>())).Returns(mocks.GetDefaultSociete(modeFes));
            var parametreManager = GetMocked<IParametreManager>();
            parametreManager.Setup(mgr => mgr.GetGoogleApiParams()).Returns(mocks.GetDefaultGoogleAPIParam());
            parametreManager.Setup(m => m.UpdateGoogleApiParams(It.IsAny<GoogleApiParam>()));
            var fakeUtilisateurManager = GetMocked<IUtilisateurManager>();
            fakeUtilisateurManager.Setup(u => u.GetContextUtilisateur()).Returns(new UtilisateurEnt { UtilisateurId = 1, SuperAdmin = true, Personnel = fredPersonnels[0] });
            fakeUtilisateurManager.Setup(u => u.IsFredUser(It.IsAny<int>())).Returns(true);
            fakeUtilisateurManager.Setup(u => u.GetById(It.IsAny<int>(), It.IsAny<bool>())).Returns(new UtilisateurEnt { UtilisateurId = 1, SuperAdmin = true, Personnel = fredPersonnels[0] });
            fakeUtilisateurManager.Setup(u => u.UpdateUtilisateur(It.IsAny<UtilisateurEnt>())).Returns<UtilisateurEnt>(u => u);
            fakeUtilisateurManager.Setup(u => u.GetContextUtilisateurId()).Returns(1);
            fakeUtilisateurManager.Setup(u => u.IsSuperAdmin(It.IsAny<int>())).Returns(true);
            var fakeAffectationManager = GetMocked<IAffectationSeuilUtilisateurManager>();
            fakeAffectationManager.Setup(x => x.GetAffectationByUserAndRolesAsync(It.IsAny<int>(), It.IsAny<bool>())).Returns(AffectationSeuilUtilisateurMocks.Create_Affectation_For_GetAffectationByUserAndRolesAsync());
            societeManager.Setup(x => x.GetSocieteByOrganisationIds(It.IsAny<List<int>>())).Returns(new SocieteMocks().GetFakeDbSet().ToList());
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("PersonnelManager")]
        public void Verifie_Que_Rien_N_Est_Fait_Si_Aucun_Changement()
        {

            this.personnelRepository.Setup(repo => repo.GetPersonnelListByCodeSocietePaye(It.IsAny<string>())).Returns(anaelPersonnels);

            Actual.ManageImportedPersonnels(anaelPersonnels, 1);

            this.personnelRepository.Verify(repo => repo.AddOrUpdatePersonnelList(It.IsAny<IEnumerable<PersonnelEnt>>()), Times.Never());

        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("PersonnelManager")]
        public void Verifie_Ajout_Dans_Ripo_Si_Un_Personnel_En_Plus()
        {

            fredPersonnels.RemoveAt(1);

            this.personnelRepository.Setup(repo => repo.GetPersonnelListByCodeSocietePaye(It.IsAny<string>())).Returns(fredPersonnels);

            Actual.ManageImportedPersonnels(anaelPersonnels, 1);

            this.personnelRepository.Verify(repo => repo.AddOrUpdatePersonnelList(It.IsAny<IEnumerable<PersonnelEnt>>()), Times.Once());

        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("PersonnelManager")]
        public void Verifie_Modification_si_Nom_Change()
        {
            anaelPersonnels.First().Nom = "JOURDES";
            fredPersonnels.First().Nom = "jourdes2";

            this.personnelRepository.Setup(repo => repo.GetPersonnelListByCodeSocietePaye(It.IsAny<string>())).Returns(fredPersonnels);

            Actual.ManageImportedPersonnels(anaelPersonnels, 1);

            //this.personnelRepository.Verify(repo => repo.AddPersonnel(It.IsAny<PersonnelEnt>()), Times.Never());
            //this.personnelRepository.Verify(repo => repo.UpdatePersonnel(It.Is<PersonnelEnt>(p => p.Nom == "JOURDES")), Times.Once());

            this.personnelRepository.Verify(repo => repo.AddOrUpdatePersonnelList(It.IsAny<IEnumerable<PersonnelEnt>>()), Times.Once());
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("PersonnelManager")]
        public void Verifie_Modification_si_DateEntree_Change()
        {
            anaelPersonnels.First().DateEntree = new DateTime(2017, 01, 03);
            fredPersonnels.First().DateEntree = new DateTime(2017, 01, 02);

            this.personnelRepository.Setup(repo => repo.GetPersonnelListByCodeSocietePaye(It.IsAny<string>())).Returns(fredPersonnels);

            Actual.ManageImportedPersonnels(anaelPersonnels, 1);

            //this.personnelRepository.Verify(repo => repo.AddPersonnel(It.IsAny<PersonnelEnt>()), Times.Never());
            //this.personnelRepository.Verify(repo => repo.UpdatePersonnel(It.Is<PersonnelEnt>(p => p.DateEntree == new DateTime(2017, 01, 03))), Times.Once());
            this.personnelRepository.Verify(repo => repo.AddOrUpdatePersonnelList(It.IsAny<IEnumerable<PersonnelEnt>>()), Times.Once());
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("PersonnelManager")]
        public void Verifie_Modification_si_DateSortie_Change()
        {
            anaelPersonnels.First().DateSortie = new DateTime(2017, 01, 03);
            fredPersonnels.First().DateSortie = new DateTime(2017, 01, 02);

            this.personnelRepository.Setup(repo => repo.GetPersonnelListByCodeSocietePaye(It.IsAny<string>())).Returns(fredPersonnels);

            Actual.ManageImportedPersonnels(anaelPersonnels, 1);

            //this.personnelRepository.Verify(repo => repo.AddPersonnel(It.IsAny<PersonnelEnt>()), Times.Never());
            //this.personnelRepository.Verify(repo => repo.UpdatePersonnel(It.Is<PersonnelEnt>(p => p.DateSortie == new DateTime(2017, 01, 03))), Times.Once());
            this.personnelRepository.Verify(repo => repo.AddOrUpdatePersonnelList(It.IsAny<IEnumerable<PersonnelEnt>>()), Times.Once());
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("PersonnelManager")]
        public void Verifie_Modification_si_Prenom_Change()
        {
            anaelPersonnels.First().Prenom = "Alex";
            fredPersonnels.First().Prenom = "Alexandre";

            this.personnelRepository.Setup(repo => repo.GetPersonnelListByCodeSocietePaye(It.IsAny<string>())).Returns(fredPersonnels);

            Actual.ManageImportedPersonnels(anaelPersonnels, 1);

            //this.personnelRepository.Verify(repo => repo.AddPersonnel(It.IsAny<PersonnelEnt>()), Times.Never());
            //this.personnelRepository.Verify(repo => repo.UpdatePersonnel(It.Is<PersonnelEnt>(p => p.Prenom == "Alex")), Times.Once());
            this.personnelRepository.Verify(repo => repo.AddOrUpdatePersonnelList(It.IsAny<IEnumerable<PersonnelEnt>>()), Times.Once());
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("PersonnelManager")]
        public void Verifie_Modification_si_Categorie_Change()
        {

            anaelPersonnels.First().CategoriePerso = "Categorie A";
            fredPersonnels.First().CategoriePerso = "Categorie B";

            this.personnelRepository.Setup(repo => repo.GetPersonnelListByCodeSocietePaye(It.IsAny<string>())).Returns(fredPersonnels);

            Actual.ManageImportedPersonnels(anaelPersonnels, 1);

            //this.personnelRepository.Verify(repo => repo.AddPersonnel(It.IsAny<PersonnelEnt>()), Times.Never());
            //this.personnelRepository.Verify(repo => repo.UpdatePersonnel(It.Is<PersonnelEnt>(p => p.CategoriePerso == "Categorie A")), Times.Once());
            this.personnelRepository.Verify(repo => repo.AddOrUpdatePersonnelList(It.IsAny<IEnumerable<PersonnelEnt>>()), Times.Once());
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("PersonnelManager")]
        public void Verifie_Modification_si_Status_Change()
        {

            anaelPersonnels.First().Statut = "Statut A";
            fredPersonnels.First().Statut = "Statut B";

            this.personnelRepository.Setup(repo => repo.GetPersonnelListByCodeSocietePaye(It.IsAny<string>())).Returns(fredPersonnels);

            Actual.ManageImportedPersonnels(anaelPersonnels, 1);

            //this.personnelRepository.Verify(repo => repo.AddPersonnel(It.IsAny<PersonnelEnt>()), Times.Never());
            //this.personnelRepository.Verify(repo => repo.UpdatePersonnel(It.Is<PersonnelEnt>(p => p.Statut == "Statut A")), Times.Once());
            this.personnelRepository.Verify(repo => repo.AddOrUpdatePersonnelList(It.IsAny<IEnumerable<PersonnelEnt>>()), Times.Once());
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("PersonnelManager")]
        public void Verifie_Modification_si_Ressource_Change()
        {

            anaelPersonnels.First().RessourceId = 1;
            fredPersonnels.First().RessourceId = 2;

            this.personnelRepository.Setup(repo => repo.GetPersonnelListByCodeSocietePaye(It.IsAny<string>())).Returns(fredPersonnels);

            Actual.ManageImportedPersonnels(anaelPersonnels, 1);

            //this.personnelRepository.Verify(repo => repo.AddPersonnel(It.IsAny<PersonnelEnt>()), Times.Never());
            //this.personnelRepository.Verify(repo => repo.UpdatePersonnel(It.Is<PersonnelEnt>(p => p.RessourceId == 1)), Times.Once());
            this.personnelRepository.Verify(repo => repo.AddOrUpdatePersonnelList(It.IsAny<IEnumerable<PersonnelEnt>>()), Times.Once());
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("PersonnelManager")]
        public void Verifie_Pas_Modification_si_TypeRattachement_Change()
        {

            anaelPersonnels.First().TypeRattachement = TypeRattachement.Agence;
            fredPersonnels.First().TypeRattachement = TypeRattachement.Domicile;

            this.personnelRepository.Setup(repo => repo.GetPersonnelListByCodeSocietePaye(It.IsAny<string>())).Returns(fredPersonnels);

            Actual.ManageImportedPersonnels(anaelPersonnels, 1);

            //this.personnelRepository.Verify(repo => repo.AddPersonnel(It.IsAny<PersonnelEnt>()), Times.Never());
            //this.personnelRepository.Verify(repo => repo.UpdatePersonnel(It.IsAny<PersonnelEnt>()), Times.Never());
            this.personnelRepository.Verify(repo => repo.AddOrUpdatePersonnelList(It.IsAny<IEnumerable<PersonnelEnt>>()), Times.Never());
        }

        [TestMethod]
        [TestCategory("Moq")]
        [TestCategory("PersonnelManager")]
        public void Verifie_Modification_Adresse_Change()
        {
            anaelPersonnels.First().Adresse1 = "Toulouse";
            fredPersonnels.First().Adresse1 = "Bordeaux";

            this.personnelRepository.Setup(repo => repo.GetPersonnelListByCodeSocietePaye(It.IsAny<string>())).Returns(fredPersonnels);

            Actual.ManageImportedPersonnels(anaelPersonnels, 1);

            //this.personnelRepository.Verify(repo => repo.AddPersonnel(It.IsAny<PersonnelEnt>()), Times.Never());
            //this.personnelRepository.Verify(repo => repo.UpdatePersonnel(It.Is<PersonnelEnt>(p => p.Adresse == "Toulouse2")), Times.Never());
            this.personnelRepository.Verify(repo => repo.AddOrUpdatePersonnelList(It.IsAny<IEnumerable<PersonnelEnt>>()), Times.Once());

        }

        [TestMethod]
        [TestCategory("PersonnelManager")]
        [ModeQuery]
        [ModeInterim]
        public async Task SearchLight_InterimWithContractOutside_ReturnsEmptyList()
        {
            var result = await Actual.SearchLightAsync(SearchPersonnelBuilder
                .DateChantier(2019, 05, 02)
                .Build()).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [TestCategory("PersonnelManager")]
        [ModeQuery]
        [ModeInterim]
        public async Task SearchLight_InterimWithContractInside_ReturnsInterim()
        {
            GetMocked<IContratInterimaireRepository>().Setup(r => r.GetListContratsInterimaires(It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .Returns(new ContratInterimaireMocks().ContratsInterimairesExpected.ToList());
            var result = await Actual.SearchLightAsync(SearchPersonnelBuilder
                .DateChantier(2019, 06, 02)
                .Build()).ConfigureAwait(false);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        [TestCategory("PersonnelManager")]
        [ModeQuery]
        public async Task SearchLight_ReturnsAll()
        {
            var result = await Actual.SearchLightAsync(SearchPersonnelBuilder
                 .DateChantier(2019, 06, 02)
                 .Build()).ConfigureAwait(false);

            Assert.AreEqual(fredPersonnels.Count, result.Count());
        }

        [TestMethod]
        [TestCategory("PersonnelManager")]
        [ModeQuery]
        [ModeInterim]
        [ModeFes]
        public async Task SearchLightFes_InterimOutside_ReturnsEmptyList()
        {
            var result = await Actual.SearchLightAsync(SearchPersonnelBuilder
                .DateChantier(2020, 05, 02)
                .Build()).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [TestCategory("PersonnelManager")]
        [ModeQuery]
        [ModeInterim]
        [ModeFes]
        public async Task SearchLightFes_InterimInside_ReturnsInterim()
        {
            var result = await Actual.SearchLightAsync(SearchPersonnelBuilder
                .DateChantier(2019, 06, 02)
                .Build()).ConfigureAwait(false);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        [TestCategory("PersonnelManager")]
        [ModeQuery]
        [ModeFes]
        public async Task SearchLightFes_ReturnsAll()
        {
            var result = await Actual.SearchLightAsync(SearchPersonnelBuilder
                 .DateChantier(2019, 06, 02)
                 .Build()).ConfigureAwait(false);
             Assert.AreEqual(fredPersonnels.Count, result.Count());
        }

        [TestMethod]
        [TestCategory("PersonnelManager")]
        public void GetPersonnelInterimaireByExternalMatriculeAndGroupeId_Returns_NotNull()
        {
            this.personnelRepository.Setup(repo => repo.GetPersonnelInterimaireByExternalMatriculeAndGroupeId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(fredPersonnels.FirstOrDefault());

            Actual.GetPersonnelInterimaireByExternalMatriculeAndGroupeId("1", "GRZB", "PIXID")
             .Should()
             .NotBeNull();
        }
    }
}
