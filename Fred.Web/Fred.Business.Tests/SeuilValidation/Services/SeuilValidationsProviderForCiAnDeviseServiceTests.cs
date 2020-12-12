using System.Collections.Generic;
using System.Linq;
using Fred.Business.AffectationSeuilOrga;
using Fred.Business.AffectationSeuilUtilisateur;
using Fred.Business.Organisation.Tree;
using Fred.Business.SeuilValidation.Manager;
using Fred.Business.SeuilValidation.Services;
using Fred.Common.Tests.Data.Affectation.Mock;
using Fred.Common.Tests.Data.Devise.Mock;
using Fred.Common.Tests.Data.Organisation.Tree.Mock;
using Fred.Common.Tests.Data.Role.Mock;
using Fred.Common.Tests.Data.SeuilValidation.Mock;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.Entities.Organisation;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.SeuilValidation.Services
{
    [TestClass]
    public class SeuilValidationsProviderForCiAnDeviseServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IOrganisationTreeService> mockOrganisationTreeService;
        private Mock<IAffectationSeuilUtilisateurManager> mockAffectationSeuilUtilisateurManager;
        private Mock<IAffectationSeuilOrgaManager> mockAffectationSeuilOrgaManager;
        private Mock<ISeuilValidationManager> mockSeuilValidationManager;


        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockOrganisationTreeService = this.mockRepository.Create<IOrganisationTreeService>();
            this.mockAffectationSeuilUtilisateurManager = this.mockRepository.Create<IAffectationSeuilUtilisateurManager>();
            this.mockAffectationSeuilOrgaManager = this.mockRepository.Create<IAffectationSeuilOrgaManager>();
            this.mockSeuilValidationManager = this.mockRepository.Create<ISeuilValidationManager>();



        }


        private SeuilValidationsProviderForCiAnDeviseService CreateService()
        {
            return new SeuilValidationsProviderForCiAnDeviseService(
                this.mockOrganisationTreeService.Object,
                this.mockAffectationSeuilUtilisateurManager.Object,
                this.mockAffectationSeuilOrgaManager.Object,
                this.mockSeuilValidationManager.Object);
        }

        [TestMethod]
        public void Le_seuil_defini_sur_l_utilisateur_et_sur_la_societe_doit_etre_pris_en_compte_car_c_est_le_seul()
        {

            // Arrange
            var service = this.CreateService();
            int ciId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB;
            int deviseId = DeviseMocks.DEVISE_ID_EURO;

            //creation de l'abre
            var societeRzb = OrganisationTreeMocks.GetSocieteRzb();
            societeRzb.Affectations.Add(AffectationBaseMocks.CreateAffectation_THOMAS_On_SocieteRzb());


            var oranisations = new List<OrganisationBase>
                {
                     OrganisationTreeMocks.GetHoldingFayat(),
                     OrganisationTreeMocks.GetGroupeRzb(),
                     societeRzb,
                     OrganisationTreeMocks.GetCi_411100_SocieteRzb(),

                };

            var orgaTree = new OrganisationTree(oranisations);


            this.mockOrganisationTreeService.Setup(x => x.GetOrganisationTree(It.IsAny<bool>()))
                            .Returns(orgaTree);

            // setup des repos
            var affectationSeuilUtilisateurs = new List<AffectationSeuilUtilisateurEnt>()
            {
               AffectationSeuilUtilisateurMocks.CreateAffectation_THOMAS_On_SocieteRzb()
            };
            var affectationSeuilOrgas = new List<AffectationSeuilOrgaEnt>()
            {

            };
            var seuilValidations = new List<SeuilValidationEnt>()
            {

            };

            this.mockAffectationSeuilUtilisateurManager.Setup(x => x.Get(It.IsAny<List<int>>()))
                            .Returns(affectationSeuilUtilisateurs);

            this.mockAffectationSeuilOrgaManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>(), It.IsAny<List<int>>()))
                          .Returns(affectationSeuilOrgas);

            this.mockSeuilValidationManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>()))
                       .Returns(seuilValidations);

            // Act
            var result = service.GetUsersWithSeuilValidationsOnCi(ciId, deviseId);

            // Assert

            var count = result.Count();
            var userThomasResult = result.FirstOrDefault(x => x.UtilisateurId == UtilisateurMocks.Utilisateur_ID_THOMAS);

            Assert.AreEqual(1, count, "Un seul resultat attendu, car un seul utilisateur dans l'arbre");
            Assert.AreEqual(10000, userThomasResult.Seuil, "Le seuil defini sur l'utilisateur et sur la societe doit etre pris en compte car c'est le seul.");

        }



        [TestMethod]
        public void Le_seuil_defini_sur_l_utilisateur_et_sur_la_societe_doit_etre_pris_en_compte_car_c_est_le_seul_avec_une_valeur()
        {

            // Arrange
            var service = this.CreateService();
            int ciId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB;
            int deviseId = DeviseMocks.DEVISE_ID_EURO;

            //creation de l'abre
            var societeRzb = OrganisationTreeMocks.GetSocieteRzb();
            societeRzb.Affectations.Add(AffectationBaseMocks.CreateAffectation_THOMAS_On_SocieteRzb());

            var ci_411100_SocieteRzb = OrganisationTreeMocks.GetCi_411100_SocieteRzb();
            societeRzb.Affectations.Add(AffectationBaseMocks.CreateAffectation_THOMAS_On_Ci_411100_SocieteRzb());

            var oranisations = new List<OrganisationBase>
                {
                     OrganisationTreeMocks.GetHoldingFayat(),
                     OrganisationTreeMocks.GetGroupeRzb(),
                     societeRzb,
                     ci_411100_SocieteRzb,

                };

            var orgaTree = new OrganisationTree(oranisations);

            this.mockOrganisationTreeService.Setup(x => x.GetOrganisationTree(It.IsAny<bool>()))
                                     .Returns(orgaTree);

            // setup des repos
            var affectationSeuilUtilisateurs = new List<AffectationSeuilUtilisateurEnt>()
            {
               AffectationSeuilUtilisateurMocks.CreateAffectation_THOMAS_On_SocieteRzb(),
               AffectationSeuilUtilisateurMocks.CreateAffectation_THOMAS_On_Ci_411100_SocieteRzb()
            };
            var affectationSeuilOrgas = new List<AffectationSeuilOrgaEnt>()
            {

            };
            var seuilValidations = new List<SeuilValidationEnt>()
            {

            };

            this.mockAffectationSeuilUtilisateurManager.Setup(x => x.Get(It.IsAny<List<int>>()))
                            .Returns(affectationSeuilUtilisateurs);

            this.mockAffectationSeuilOrgaManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>(), It.IsAny<List<int>>()))
                          .Returns(affectationSeuilOrgas);

            this.mockSeuilValidationManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>()))
                       .Returns(seuilValidations);

            // Act
            var result = service.GetUsersWithSeuilValidationsOnCi(ciId, deviseId);

            // Assert

            var count = result.Count();
            var userThomasResult = result.FirstOrDefault(x => x.UtilisateurId == UtilisateurMocks.Utilisateur_ID_THOMAS);

            Assert.AreEqual(1, count, "Un seul resultat attendu, car un seul utilisateur dans l'arbre");
            Assert.AreEqual(0, userThomasResult.Seuil, "Le seuil defini sur l'utilisateur et sur la societe doit etre pris en compte car c'est le seul qui a une valeur.");

        }


        [TestMethod]
        public void Le_seuil_defini_sur_l_utilisateur_et_sur_le_ci_doit_etre_pris_en_compte_car_c_est_le_seul_avec_une_valeur()
        {

            // Arrange
            var service = this.CreateService();
            int ciId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB;
            int deviseId = DeviseMocks.DEVISE_ID_EURO;

            //creation de l'abre
            var societeRzb = OrganisationTreeMocks.GetSocieteRzb();
            societeRzb.Affectations.Add(AffectationBaseMocks.CreateAffectation_THOMAS_On_SocieteRzb());

            var ci_411100_SocieteRzb = OrganisationTreeMocks.GetCi_411100_SocieteRzb();
            societeRzb.Affectations.Add(AffectationBaseMocks.CreateAffectation_THOMAS_On_Ci_411100_SocieteRzb());

            var oranisations = new List<OrganisationBase>
                {
                     OrganisationTreeMocks.GetHoldingFayat(),
                     OrganisationTreeMocks.GetGroupeRzb(),
                     societeRzb,
                     ci_411100_SocieteRzb,
                };

            var orgaTree = new OrganisationTree(oranisations);

            this.mockOrganisationTreeService.Setup(x => x.GetOrganisationTree(It.IsAny<bool>()))
                            .Returns(orgaTree);


            // setup des repos
            var affectation_THOMAS_On_SocieteRzb = AffectationSeuilUtilisateurMocks.CreateAffectation_THOMAS_On_SocieteRzb();
            affectation_THOMAS_On_SocieteRzb.CommandeSeuil = 0;
            var affectation_THOMAS_On_Ci_411100_SocieteRzb = AffectationSeuilUtilisateurMocks.CreateAffectation_THOMAS_On_Ci_411100_SocieteRzb();
            affectation_THOMAS_On_Ci_411100_SocieteRzb.CommandeSeuil = 25000;

            var affectationSeuilUtilisateurs = new List<AffectationSeuilUtilisateurEnt>()
            {
              affectation_THOMAS_On_SocieteRzb ,
              affectation_THOMAS_On_Ci_411100_SocieteRzb
            };


            this.mockAffectationSeuilUtilisateurManager.Setup(x => x.Get(It.IsAny<List<int>>()))
                            .Returns(affectationSeuilUtilisateurs);

            this.mockAffectationSeuilOrgaManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>(), It.IsAny<List<int>>()))
                          .Returns(new List<AffectationSeuilOrgaEnt>());

            // pas logique car il y a forcement un role avec un seuil mais se n'est pas l'object du test
            this.mockSeuilValidationManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>()))
                       .Returns(new List<SeuilValidationEnt>());

            // Act
            var result = service.GetUsersWithSeuilValidationsOnCi(ciId, deviseId);

            // Assert

            var count = result.Count();
            var userThomasResult = result.FirstOrDefault(x => x.UtilisateurId == UtilisateurMocks.Utilisateur_ID_THOMAS);

            Assert.AreEqual(1, count, "Un seul resultat attendu, car un seul utilisateur dans l'arbre");
            Assert.AreEqual(25000, userThomasResult.Seuil, "Le seuil defini sur l'utilisateur et sur le ci doit etre pris en compte car c'est le seul qui a une valeur.");

        }


        [TestMethod]
        public void Le_seuil_defini_sur_l_organisation_et_sur_le_ci_doit_etre_pris_en_compte()
        {

            // Arrange
            var service = this.CreateService();
            int ciId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB;
            int deviseId = DeviseMocks.DEVISE_ID_EURO;

            //creation de l'abre
            var societeRzb = OrganisationTreeMocks.GetSocieteRzb();
            societeRzb.Affectations.Add(AffectationBaseMocks.CreateAffectation_THOMAS_On_SocieteRzb());

            var ci_411100_SocieteRzb = OrganisationTreeMocks.GetCi_411100_SocieteRzb();
            societeRzb.Affectations.Add(AffectationBaseMocks.CreateAffectation_THOMAS_On_Ci_411100_SocieteRzb());

            var oranisations = new List<OrganisationBase>
                {
                     OrganisationTreeMocks.GetHoldingFayat(),
                     OrganisationTreeMocks.GetGroupeRzb(),
                     societeRzb,
                     ci_411100_SocieteRzb,
                };

            var orgaTree = new OrganisationTree(oranisations);


            this.mockOrganisationTreeService.Setup(x => x.GetOrganisationTree(It.IsAny<bool>()))
                          .Returns(orgaTree);


            // setup des repos

            //CREATION SEUIL SUR PERSONNEL
            var affectation_THOMAS_On_SocieteRzb = AffectationSeuilUtilisateurMocks.CreateAffectation_THOMAS_On_SocieteRzb();
            affectation_THOMAS_On_SocieteRzb.CommandeSeuil = null;
            var affectation_THOMAS_On_Ci_411100_SocieteRzb = AffectationSeuilUtilisateurMocks.CreateAffectation_THOMAS_On_Ci_411100_SocieteRzb();
            affectation_THOMAS_On_Ci_411100_SocieteRzb.CommandeSeuil = null;

            var affectationSeuilUtilisateurs = new List<AffectationSeuilUtilisateurEnt>()
            {
              affectation_THOMAS_On_SocieteRzb ,
              affectation_THOMAS_On_Ci_411100_SocieteRzb
            };

            //CREATION SEUIL SUR ORGANISATION
            var affectationSeuilOrgaEnt_CHEF_DE_CHANTIER_ON_SOCIETE_RZB = AffectationSeuilOrgaMocks.Create_AffectationSeuilOrgaEnt_CHEF_DE_CHANTIER_ON_SOCIETE_RZB();
            affectationSeuilOrgaEnt_CHEF_DE_CHANTIER_ON_SOCIETE_RZB.Seuil = 26358;

            var affectationSeuilOrgas = new List<AffectationSeuilOrgaEnt>()
            {
                affectationSeuilOrgaEnt_CHEF_DE_CHANTIER_ON_SOCIETE_RZB
            };

            //CREATION SEUIL SUR ROLE
            var seuilValidationEnt_CHEF_CHANTIER_ON_SOCIETE_RZB = SeuilValidationMocks.Create_SeuilValidationEnt_CHEF_CHANTIER_ON_SOCIETE_RZB();
            seuilValidationEnt_CHEF_CHANTIER_ON_SOCIETE_RZB.Montant = 3000;

            var seuilValidations = new List<SeuilValidationEnt>()
            {
                seuilValidationEnt_CHEF_CHANTIER_ON_SOCIETE_RZB
            };

            this.mockAffectationSeuilUtilisateurManager.Setup(x => x.Get(It.IsAny<List<int>>()))
                            .Returns(affectationSeuilUtilisateurs);

            this.mockAffectationSeuilOrgaManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>(), It.IsAny<List<int>>()))
                          .Returns(affectationSeuilOrgas);

            this.mockSeuilValidationManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>()))
                       .Returns(seuilValidations);

            // Act
            var result = service.GetUsersWithSeuilValidationsOnCi(ciId, deviseId);

            // Assert

            var count = result.Count();
            var userThomasResult = result.FirstOrDefault(x => x.UtilisateurId == UtilisateurMocks.Utilisateur_ID_THOMAS);

            Assert.AreEqual(1, count, "Un seul resultat attendu, car un seul utilisateur dans l'arbre");
            Assert.AreEqual(26358, userThomasResult.Seuil, "Le seuil defini sur l'organisation et sur le ci doit etre pris en compte.");

        }

        [TestMethod]
        public void Le_seuil_defini_sur_le_role_et_sur_le_ci_doit_etre_pris_en_compte()
        {

            // Arrange
            var service = this.CreateService();
            int ciId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB;
            int deviseId = DeviseMocks.DEVISE_ID_EURO;

            //creation de l'abre
            var societeRzb = OrganisationTreeMocks.GetSocieteRzb();
            societeRzb.Affectations.Add(AffectationBaseMocks.CreateAffectation_THOMAS_On_SocieteRzb());

            var ci_411100_SocieteRzb = OrganisationTreeMocks.GetCi_411100_SocieteRzb();
            societeRzb.Affectations.Add(AffectationBaseMocks.CreateAffectation_THOMAS_On_Ci_411100_SocieteRzb());

            var oranisations = new List<OrganisationBase>
                {
                     OrganisationTreeMocks.GetHoldingFayat(),
                     OrganisationTreeMocks.GetGroupeRzb(),
                     societeRzb,
                     ci_411100_SocieteRzb,
                };

            var orgaTree = new OrganisationTree(oranisations);

            this.mockOrganisationTreeService.Setup(x => x.GetOrganisationTree(It.IsAny<bool>()))
                                     .Returns(orgaTree);

            // setup des repos
            //CREATION SEUIL SUR PERSONNEL
            var affectation_THOMAS_On_SocieteRzb = AffectationSeuilUtilisateurMocks.CreateAffectation_THOMAS_On_SocieteRzb();
            affectation_THOMAS_On_SocieteRzb.CommandeSeuil = null;
            var affectation_THOMAS_On_Ci_411100_SocieteRzb = AffectationSeuilUtilisateurMocks.CreateAffectation_THOMAS_On_Ci_411100_SocieteRzb();
            affectation_THOMAS_On_Ci_411100_SocieteRzb.CommandeSeuil = null;

            var affectationSeuilUtilisateurs = new List<AffectationSeuilUtilisateurEnt>()
            {
              affectation_THOMAS_On_SocieteRzb ,
              affectation_THOMAS_On_Ci_411100_SocieteRzb
            };

            //CREATION SEUIL SUR ROLE
            var seuilValidationEnt_CHEF_CHANTIER_ON_SOCIETE_RZB = SeuilValidationMocks.Create_SeuilValidationEnt_CHEF_CHANTIER_ON_SOCIETE_RZB();
            seuilValidationEnt_CHEF_CHANTIER_ON_SOCIETE_RZB.Montant = 3000;

            var seuilValidations = new List<SeuilValidationEnt>()
            {
                seuilValidationEnt_CHEF_CHANTIER_ON_SOCIETE_RZB
            };

            this.mockAffectationSeuilUtilisateurManager.Setup(x => x.Get(It.IsAny<List<int>>()))
                            .Returns(affectationSeuilUtilisateurs);

            this.mockAffectationSeuilOrgaManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>(), It.IsAny<List<int>>()))
                          .Returns(new List<AffectationSeuilOrgaEnt>());

            this.mockSeuilValidationManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>()))
                       .Returns(seuilValidations);

            // Act
            var result = service.GetUsersWithSeuilValidationsOnCi(ciId, deviseId);

            // Assert

            var count = result.Count();
            var userThomasResult = result.FirstOrDefault(x => x.UtilisateurId == UtilisateurMocks.Utilisateur_ID_THOMAS);

            Assert.AreEqual(1, count, "Un seul resultat attendu, car un seul utilisateur dans l'arbre");
            Assert.AreEqual(3000, userThomasResult.Seuil, "Le seuil defini sur le role et sur le ci doit etre pris en compte.");

        }


        [TestMethod]
        public void Aucun_utilisateur_ne_doit_etre_remonte_car_pas_d_affectation()
        {

            // Arrange
            var service = this.CreateService();
            int ciId = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB;
            int deviseId = DeviseMocks.DEVISE_ID_EURO;

            //creation de l'abre
            var societeRzb = OrganisationTreeMocks.GetSocieteRzb();
            var ci_411100_SocieteRzb = OrganisationTreeMocks.GetCi_411100_SocieteRzb();

            var oranisations = new List<OrganisationBase>
                {
                     OrganisationTreeMocks.GetHoldingFayat(),
                     OrganisationTreeMocks.GetGroupeRzb(),
                     societeRzb,
                     ci_411100_SocieteRzb,
                };

            var orgaTree = new OrganisationTree(oranisations);


            this.mockOrganisationTreeService.Setup(x => x.GetOrganisationTree(It.IsAny<bool>()))
                          .Returns(orgaTree);

            this.mockAffectationSeuilUtilisateurManager.Setup(x => x.Get(It.IsAny<List<int>>()))
                            .Returns(new List<AffectationSeuilUtilisateurEnt>());

            this.mockAffectationSeuilOrgaManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>(), It.IsAny<List<int>>()))
                          .Returns(new List<AffectationSeuilOrgaEnt>());

            this.mockSeuilValidationManager.Setup(x => x.Get(deviseId, It.IsAny<List<int>>()))
                       .Returns(new List<SeuilValidationEnt>());

            // Act
            var result = service.GetUsersWithSeuilValidationsOnCi(ciId, deviseId);

            // Assert

            var count = result.Count();

            var userThomasResult = result.FirstOrDefault(x => x.UtilisateurId == UtilisateurMocks.Utilisateur_ID_THOMAS);

            Assert.AreEqual(0, count, "Zero resultat attendu, car zero affectation dans l'arbre");

        }



        [TestMethod]
        public void rien_ne_doit_remonte_car_le_seuil_n_a_ni_devise_ni_seuil()
        {

            // Arrange
            var service = this.CreateService();
            int ciIdSelected = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB;
            int deviseIdSelected = DeviseMocks.DEVISE_ID_EURO;

            // creation de l'abre
            var societeRzb = OrganisationTreeMocks.GetSocieteRzb();
            var ci_411100_SocieteRzb = OrganisationTreeMocks.GetCi_411100_SocieteRzb();

            var oranisations = new List<OrganisationBase>
                {
                     OrganisationTreeMocks.GetHoldingFayat(),
                     OrganisationTreeMocks.GetGroupeRzb(),
                     societeRzb,
                     ci_411100_SocieteRzb,
                };

            var orgaTree = new OrganisationTree(oranisations);


            // creation d'une affection sur le ci sans seuil ni devise dans l'arbre
            societeRzb.Affectations.Add(new AffectationBase
            {
                AffectationId = AffectationSeuilUtilisateurMocks.AFFECTATION_SEUIL_UTILISATEUR_ID_2,
                UtilisateurId = UtilisateurMocks.Utilisateur_ID_THOMAS,
                OrganisationId = OrganisationTreeMocks.ORGANISATION_ID_CI_411100_SOCIETE_RZB,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CONDUCTEUR_DE_TRAVAUX,
            });

            // creation d'une affection sur le ci sans seuil ni devise dans la table           
            var affectationSeuilUtilisateurs = new List<AffectationSeuilUtilisateurEnt>()
            {
               new AffectationSeuilUtilisateurEnt
                {
                    AffectationRoleId = AffectationSeuilUtilisateurMocks.AFFECTATION_SEUIL_UTILISATEUR_ID_2,
                    UtilisateurId = UtilisateurMocks.Utilisateur_ID_THOMAS,
                    OrganisationId = OrganisationTreeMocks.ORGANISATION_ID_CI_411100_SOCIETE_RZB,
                    RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CONDUCTEUR_DE_TRAVAUX,
                    DeviseId = null,
                    CommandeSeuil = null
                },
            };



            this.mockOrganisationTreeService.Setup(x => x.GetOrganisationTree(It.IsAny<bool>()))
                            .Returns(orgaTree);

            this.mockAffectationSeuilUtilisateurManager.Setup(x => x.Get(It.IsAny<List<int>>()))
                            .Returns(affectationSeuilUtilisateurs);

            this.mockAffectationSeuilOrgaManager.Setup(x => x.Get(deviseIdSelected, It.IsAny<List<int>>(), It.IsAny<List<int>>()))
                          .Returns(new List<AffectationSeuilOrgaEnt>());

            this.mockSeuilValidationManager.Setup(x => x.Get(deviseIdSelected, It.IsAny<List<int>>()))
                       .Returns(new List<SeuilValidationEnt>());

            // Act
            var result = service.GetUsersWithSeuilValidationsOnCi(ciIdSelected, deviseIdSelected);

            // Assert

            var count = result.Count();

            var userThomasResult = result.FirstOrDefault(x => x.UtilisateurId == UtilisateurMocks.Utilisateur_ID_THOMAS);

            Assert.AreEqual(0, count, "Zero resultat attendu, car zero affectation dans l'arbre");

        }


        [TestMethod]
        public void le_montant_sur_le_role_doit_remonte_meme_si_l_affectaion_n_a_ni_devise_ni_seuil()
        {

            // Arrange
            var service = this.CreateService();
            int ciIdSelected = OrganisationTreeMocks.CI_ID_411100_SOCIETE_RZB;
            int deviseIdSelected = DeviseMocks.DEVISE_ID_EURO;

            // creation de l'abre
            var societeRzb = OrganisationTreeMocks.GetSocieteRzb();
            var ci_411100_SocieteRzb = OrganisationTreeMocks.GetCi_411100_SocieteRzb();

            var oranisations = new List<OrganisationBase>
                {
                     OrganisationTreeMocks.GetHoldingFayat(),
                     OrganisationTreeMocks.GetGroupeRzb(),
                     societeRzb,
                     ci_411100_SocieteRzb,
                };

            var orgaTree = new OrganisationTree(oranisations);


            // creation d'une affection sur le ci sans seuil ni devise dans l'arbre
            societeRzb.Affectations.Add(new AffectationBase
            {
                AffectationId = AffectationSeuilUtilisateurMocks.AFFECTATION_SEUIL_UTILISATEUR_ID_2,
                UtilisateurId = UtilisateurMocks.Utilisateur_ID_THOMAS,
                OrganisationId = OrganisationTreeMocks.ORGANISATION_ID_CI_411100_SOCIETE_RZB,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CONDUCTEUR_DE_TRAVAUX,
            });

            // creation d'une affection sur le ci sans seuil ni devise dans la table           
            var affectationSeuilUtilisateurs = new List<AffectationSeuilUtilisateurEnt>()
            {
               new AffectationSeuilUtilisateurEnt
                {
                    AffectationRoleId = AffectationSeuilUtilisateurMocks.AFFECTATION_SEUIL_UTILISATEUR_ID_2,
                    UtilisateurId = UtilisateurMocks.Utilisateur_ID_THOMAS,
                    OrganisationId = OrganisationTreeMocks.ORGANISATION_ID_CI_411100_SOCIETE_RZB,
                    RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CONDUCTEUR_DE_TRAVAUX,
                    DeviseId = null,
                    CommandeSeuil = null
                },
            };

            //creation d'un seuil sur le role
            var seuilValidations = new List<SeuilValidationEnt>()
            {
              new SeuilValidationEnt
                {
                    SeuilValidationId = SeuilValidationMocks.SEUIL_VALIDATION_ID_1,
                    DeviseId = DeviseMocks.DEVISE_ID_EURO,
                    RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CONDUCTEUR_DE_TRAVAUX,
                    Montant = 18000
                }
            };


            this.mockOrganisationTreeService.Setup(x => x.GetOrganisationTree(It.IsAny<bool>()))
                            .Returns(orgaTree);

            this.mockAffectationSeuilUtilisateurManager.Setup(x => x.Get(It.IsAny<List<int>>()))
                            .Returns(affectationSeuilUtilisateurs);

            this.mockAffectationSeuilOrgaManager.Setup(x => x.Get(deviseIdSelected, It.IsAny<List<int>>(), It.IsAny<List<int>>()))
                          .Returns(new List<AffectationSeuilOrgaEnt>());

            this.mockSeuilValidationManager.Setup(x => x.Get(deviseIdSelected, It.IsAny<List<int>>()))
                       .Returns(seuilValidations);

            // Act
            var result = service.GetUsersWithSeuilValidationsOnCi(ciIdSelected, deviseIdSelected);

            // Assert

            var count = result.Count();

            var userThomasResult = result.FirstOrDefault(x => x.UtilisateurId == UtilisateurMocks.Utilisateur_ID_THOMAS);

            Assert.AreEqual(1, count, "Zero resultat attendu, car zero affectation dans l'arbre");

        }


    }
}
