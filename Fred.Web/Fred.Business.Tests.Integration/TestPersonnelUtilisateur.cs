using System;
using Fred.Entities.Directory;
using Fred.Entities.Personnel;
using Fred.Entities.Utilisateur;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestPersonnelUtilisateur : FredBaseTest
    {
        private static PersonnelEnt personnelInterne;

        private static PersonnelEnt personnelExterne;

        private static UtilisateurEnt utilisateur;

        /// <summary>
        ///   Initialise l'ensemble des tests de la classe.
        /// </summary>
        /// <param name="context">Le contexte de tests.</param>
        [ClassInitialize]
        public static void InitAllTests(TestContext context)
        {
            personnelInterne = new PersonnelEnt
            {
                Nom = "TestNom",
                Prenom = "TestPrenom",
                SocieteId = 1,
                EtablissementPaieId = 1,
                EtablissementRattachementId = 1,
                Adresse1 = "39 Quai du Président Roosevelt",
                CodePostal = "92130",
                Ville = "Issy-les-Moulineaux",
                PaysId = 1,
                IsInterne = true,
                IsInterimaire = false,
                Telephone1 = "0102030405",
                Telephone2 = "0203040506",
                TypeRattachement = "D",
                RessourceId = 1,
                Matricule = "1234Test",
                DateCreation = DateTime.Now,
                DateEntree = DateTime.Now,
                DateSortie = DateTime.Now.AddYears(3),
                Email = "personnelInterne1@fayat.com",
                LatitudeDomicile = 2.2653476,
                LongitudeDomicile = 48.8340179
            };

            personnelExterne = new PersonnelEnt
            {
                Nom = "TestNom",
                Prenom = "TestPrenom",
                SocieteId = 1,
                EtablissementPaieId = 1,
                EtablissementRattachementId = 1,
                Adresse1 = "39 Quai du Président Roosevelt",
                CodePostal = "92130",
                Ville = "Issy-les-Moulineaux",
                PaysId = 1,
                IsInterne = false,
                IsInterimaire = false,
                Telephone1 = "0102030405",
                Telephone2 = "0203040506",
                TypeRattachement = "A",
                RessourceId = 1,
                Matricule = "1234Test",
                DateCreation = DateTime.Now,
                DateEntree = DateTime.Now,
                DateSortie = DateTime.Now.AddYears(3),
                Email = "personnelInterne1@fayat.com",
                LatitudeDomicile = 2.2653476,
                LongitudeDomicile = 48.8340179
            };

            utilisateur = GetNewInstanceUtilisateur();
        }

        private static UtilisateurEnt GetNewInstanceUtilisateur()
        {
            return new UtilisateurEnt
            {
                //PersonnelId = 1,
                Login = "utilisateurTest",
                DateCreation = DateTime.Now,
                ExternalDirectory = new ExternalDirectoryEnt { DateExpiration = DateTime.Now.AddYears(2), IsActived = true, MotDePasse = "12345AZERTY" }
            };
        }

        /// <summary>
        ///   Teste que la liste des personnels retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetPersonnelList()
        {
            var resu = PersonnelMgr.GetPersonnelList();
            Assert.IsTrue(resu != null);
        }

        /// <summary>
        ///   Teste que la récupération d'un personnel par son id retourne bien un résultat
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetPersonnelById()
        {

            PersonnelEnt resu = PersonnelMgr.GetPersonnel(1);
            Assert.IsTrue(resu != null && resu.PersonnelId.Equals(1));
        }

        /// <summary>
        ///   Teste la création d'un personnel interne
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void CreatePersonnelInterne()
        {

            PersonnelEnt personnel = PersonnelMgr.Add(personnelInterne);
            Assert.IsNotNull(personnel);
            Assert.IsTrue(personnel.PersonnelId != 0);
        }

        /// <summary>
        ///   Teste la création d'un personnel externe
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void CreatePersonnelExterne()
        {
            //#############################################################################################################
            // Test mis a ignore car il ne fonctionne pas deux fois de suite
            //  => erreur d'ajout une seconde fois car un personnel a deja ete ajoute avec cet email pour cette societe.
            //  => cette erreur n'est pas un duplicate key mais une validation exception (prevoir un TU pou ce cas ci)
            //#############################################################################################################

            PersonnelEnt personnel = PersonnelMgr.Add(personnelExterne);
            Assert.IsNotNull(personnel);
            Assert.IsTrue(personnel.PersonnelId != 0);

            //Test cleanup
            PersonnelMgr.DeleteById(personnel.PersonnelId);
        }

        /// <summary>
        ///   Teste que la liste des favoris retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetUtilisateurList()
        {
            var utilisateur = UserMgr.GetUtilisateurList();
            Assert.IsTrue(utilisateur != null);
        }

        /// <summary>
        ///   Teste que la récupération d'un utilisateur par son id retourne bien un résultat
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetUtilisateurById()
        {
            UtilisateurEnt resu = UserMgr.GetUtilisateurById(1);
            Assert.IsTrue(resu != null && resu.UtilisateurId.Equals(1));
        }

        /// <summary>
        ///   Teste la création d'un utilisateur
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void CreateUtilisateur()
        {
            UtilisateurEnt user = GetNewInstanceUtilisateur();
            user.Login = GenerateString(10);
            int resu = UserMgr.AddUtilisateur(user).UtilisateurId;
            Assert.IsTrue(resu > 0);
        }
    }
}
