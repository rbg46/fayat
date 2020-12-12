using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Utilisateur;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestUtilisateur : FredBaseTest
    {

        /// <summary>
        ///   Teste l'affactation d'un liste de role et d'organisation pour un utilisateur.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void UpdateAffectationRole()
        {
            UtilisateurEnt utilisateur = UserMgr.GetById(SuperAdminUserId);
            var listAffectations = new List<AffectationSeuilUtilisateurEnt>();

            AffectationSeuilUtilisateurEnt affectation1 = new AffectationSeuilUtilisateurEnt
            {
                UtilisateurId = SuperAdminUserId,
                OrganisationId = 1,
                RoleId = 1
            };

            listAffectations.Add(affectation1);

            AffectationSeuilUtilisateurEnt affectation2 = new AffectationSeuilUtilisateurEnt
            {
                UtilisateurId = SuperAdminUserId,
                OrganisationId = 2,
                RoleId = 1
            };
            listAffectations.Add(affectation2);

            UserMgr.UpdateRole(utilisateur.UtilisateurId, listAffectations);

            int count = utilisateur.AffectationsRole.Count();

            Assert.IsTrue(count == listAffectations.Count);
        }

        /// <summary>
        ///   Teste l'affactation d'un liste de role et d'organisation pour un utilisateur.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void UpdateAffectationRoleWithDevise()
        {
            UtilisateurEnt utilisateur = UserMgr.GetById(2);

            var listAffectations = new List<AffectationSeuilUtilisateurEnt>();

            AffectationSeuilUtilisateurEnt affectation1 = new AffectationSeuilUtilisateurEnt
            {
                UtilisateurId = 2,
                OrganisationId = 1,
                RoleId = 1,
                DeviseId = 48,
                CommandeSeuil = 15000
            };

            listAffectations.Add(affectation1);

            AffectationSeuilUtilisateurEnt affectation2 = new AffectationSeuilUtilisateurEnt
            {
                UtilisateurId = 2,
                OrganisationId = 1,
                RoleId = 1,
                DeviseId = 49,
                CommandeSeuil = 10000
            };
            listAffectations.Add(affectation2);

            UserMgr.UpdateRole(utilisateur.UtilisateurId, listAffectations);

            int count = utilisateur.AffectationsRole.Count();

            Assert.IsTrue(count == listAffectations.Count);
        }
    }
}
