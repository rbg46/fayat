using System;
using System.Linq;
using Fred.Entities.Personnel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestPersonnel : FredBaseTest
    {
        /// <summary>
        ///   Teste que la liste des personnels n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetPersonnelListReturnNotNullList()
        {
            var personnels = PersonnelMgr.GetPersonnelList();
            Assert.IsTrue(personnels != null);
        }

        /// <summary>
        ///   Teste recherche des tâches
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GetPersonnelListReturnAtLeastOneRecord()
        {

            var personnels = PersonnelMgr.GetPersonnelList().ToList();
            Assert.IsTrue(personnels.Count > 0);
        }

        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetNonExistingPersonnelReturnNull()
        {

            PersonnelEnt personnel = PersonnelMgr.GetPersonnel(-1);
            Assert.IsNull(personnel);
        }

        [TestMethod]
        [TestCategory("DBDepend")]

        public void AddPersonnel()
        {
            Random oRandom = new Random();
            PersonnelEnt perso = new PersonnelEnt
            {
                Matricule = "TST" + oRandom.Next(0, 999),
                Nom = "TestCreation",
                Prenom = "Jean-Michel",
                SocieteId = 1,
                RessourceId = 1,
                DateCreation = DateTime.Today,
                DateEntree = DateTime.Today,
                DateSortie = DateTime.Today
            };
            var dbsetperso = Uow.Context.Personnels;
            int countBefore = dbsetperso.Count();
            var personnelAdded = PersonnelMgr.Add(perso);
            int countAfter = dbsetperso.Count();

            Assert.IsTrue((countBefore + 1) == countAfter);

            //Test cleanup
            PersonnelMgr.DeleteById(personnelAdded.PersonnelId);
        }

        /// <summary>
        ///   Teste que la liste des personnels n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void GetCountPersonnel()
        {
            SearchPersonnelEnt filter = new SearchPersonnelEnt
            {
                IsActif = true,
                IsInterimaire = true,
                IsInterne = true,
                IsUtilisateur = false,
                ValueText = string.Empty,
                Matricule = true,
                MatriculeAsc = false,
                Nom = true,
                Prenom = true,
                NomPrenomAsc = true,
                SocieteAsc = true,
                SocieteCodeLibelle = true
            };


            int nbPersonnels = PersonnelMgr.GetCountPersonnel(filter);
            Assert.IsTrue(nbPersonnels == 57);
        }
    }
}
