using System.Linq;
using Fred.Common.Tests.Helper;
using Fred.Entities.Referential;
using Fred.Entities.Role;
using Fred.Framework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestRole : FredBaseTest
    {
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GetRoleById()
        {
            RoleEnt roleTest = new RoleEnt
            {
                Libelle = "RoleTest",
                Code = GuidEx.CreateBase64Guid(20),
                CodeNomFamilier = GuidEx.CreateBase64Guid(20),
                Actif = true,
                ModeLecture = false,
                SocieteId = 1
            };
            RoleEnt role = RoleMgr.AddRole(roleTest);
            Assert.IsTrue(role != null);
        }

        /// <summary>
        ///   Teste la création d'un rôle et l'incrémentation de son ID
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddRole()
        {

            RoleEnt roleTest = new RoleEnt
            {
                Libelle = "RoleTest",
                Code = GuidEx.CreateBase64Guid(20),
                CodeNomFamilier = GuidEx.CreateBase64Guid(20),
                Actif = true,
                ModeLecture = false,
                SocieteId = 1
            };
            var roleAdded = RoleMgr.AddRole(roleTest);
            Assert.IsTrue(roleTest != null && roleTest.RoleId > 0);

            //Test cleanup
            RoleMgr.DeleteRole(roleAdded.RoleId);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GetRoleSeuilsValidationByRoleId()
        {
            RoleEnt roleTest = new RoleEnt
            {
                Libelle = "RoleTest",
                Code = GuidEx.CreateBase64Guid(20),
                CodeNomFamilier = GuidEx.CreateBase64Guid(20),
                Actif = true,
                ModeLecture = false,
                SocieteId = 1
            };
            RoleEnt role = RoleMgr.AddRole(roleTest);

            SeuilValidationEnt seuil = new SeuilValidationEnt
            {
                RoleId = role.RoleId,
                DeviseId = 1,
                Montant = 10000
            };
            SeuilValidationEnt seuilId = RoleMgr.AddSeuilValidation(seuil);

            var seuils = RoleMgr.GetSeuilValidationListByRoleId(role.RoleId);
            Assert.IsTrue(seuils != null && seuils.Any());
        }

        /// <summary>
        ///   Test que la récupération d'un seuil de validation de commande au niveau d'un rôle depuis son identifiant
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void GetSeuilValidationById()
        {
            RoleEnt roleTest = new RoleEnt
            {
                Libelle = "RoleTest",
                Code = GuidEx.CreateBase64Guid(20),
                CodeNomFamilier = GuidEx.CreateBase64Guid(20),
                Actif = true,
                ModeLecture = false,
                SocieteId = 1
            };
            RoleEnt role = RoleMgr.AddRole(roleTest);

            SeuilValidationEnt seuil = new SeuilValidationEnt
            {
                RoleId = role.RoleId,
                DeviseId = 1,
                Montant = 10000
            };
            SeuilValidationEnt seuilId = RoleMgr.AddSeuilValidation(seuil);

            var seuils = RoleMgr.GetSeuilValidationListByRoleId(role.RoleId);
            Assert.IsTrue(seuils != null && seuils.Any());
        }

        /// <summary>
        ///   Test qu'un seuil associé à un rôle a bien été ajouté
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void AddRoleSeuilValidation()
        {

            RoleEnt roleTest = new RoleEnt
            {
                Libelle = "RoleTest",
                Code = GuidEx.CreateBase64Guid(20),
                CodeNomFamilier = GuidEx.CreateBase64Guid(20),
                Actif = true,
                ModeLecture = false,
                SocieteId = 1
            };
            RoleEnt role = RoleMgr.AddRole(roleTest);

            SeuilValidationEnt seuil = new SeuilValidationEnt
            {
                RoleId = role.RoleId,
                DeviseId = 1,
                Montant = 10000
            };
            SeuilValidationEnt seuilAdded = RoleMgr.AddSeuilValidation(seuil);

            var seuils = RoleMgr.GetSeuilValidationListByRoleId(role.RoleId);
            Assert.IsTrue(seuils != null && seuils.Any());

            //Test cleanup
            RoleMgr.DeleteSeuilValidationById(seuilAdded.SeuilValidationId);
            RoleMgr.DeleteRole(role.RoleId);
        }

        /// <summary>
        ///   Met à rout un seuil de validation
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void UpdateRoleSeuilValidation()
        {

            RoleEnt roleTest = new RoleEnt
            {
                Libelle = "RoleTest",
                Code = GuidEx.CreateBase64Guid(20),
                CodeNomFamilier = GuidEx.CreateBase64Guid(20),
                Actif = true,
                ModeLecture = false,
                SocieteId = 1
            };
            RoleMgr.AddRole(roleTest);

            SeuilValidationEnt seuil = new SeuilValidationEnt
            {
                RoleId = roleTest.RoleId,
                DeviseId = 1,
                Montant = 10000
            };
            RoleMgr.AddSeuilValidation(seuil);

            decimal montantBefore = RoleMgr.GetSeuilValidationById(seuil.SeuilValidationId).Montant;
            seuil.Montant = 15000;
            RoleMgr.UpdateSeuilValidation(seuil);
            decimal montantAfter = RoleMgr.GetSeuilValidationById(seuil.SeuilValidationId).Montant;

            Assert.IsTrue(montantAfter > montantBefore);
        }

        /// <summary>
        ///   Teste qu'un seuil a bien été supprimé
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void DeleteRoleSeuilValidationBySeuilId()
        {

            RoleEnt roleTest = new RoleEnt
            {
                Libelle = "RoleTest",
                Code = GuidEx.CreateBase64Guid(20),
                CodeNomFamilier = GuidEx.CreateBase64Guid(20),
                Actif = true,
                ModeLecture = false,
                SocieteId = 1
            };
            RoleEnt role = RoleMgr.AddRole(roleTest);

            SeuilValidationEnt seuil = new SeuilValidationEnt
            {
                RoleId = role.RoleId,
                DeviseId = 1,
                Montant = 10000
            };
            RoleMgr.AddSeuilValidation(seuil);

            RoleMgr.DeleteSeuilValidationById(seuil.SeuilValidationId);

            Assert.IsTrue(RoleMgr.GetSeuilValidationById(seuil.SeuilValidationId) == null);
        }

        /// <summary>
        ///   Teste que tous les seuils associés à un rôle ont été supprimés
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        public void DeleteAllRoleSeuilValidation()
        {

            RoleEnt roleTest = new RoleEnt
            {
                Libelle = "RoleTest",
                Code = GuidEx.CreateBase64Guid(20),
                CodeNomFamilier = GuidEx.CreateBase64Guid(20),
                Actif = true,
                ModeLecture = false,
                SocieteId = 1
            };
            RoleEnt role = RoleMgr.AddRole(roleTest);

            SeuilValidationEnt seuil = new SeuilValidationEnt
            {
                RoleId = role.RoleId,
                DeviseId = 1,
                Montant = 10000
            };
            RoleMgr.AddSeuilValidation(seuil);

            RoleMgr.DeleteSeuilValidationById(seuil.SeuilValidationId);

            Assert.IsTrue(RoleMgr.GetSeuilValidationById(seuil.SeuilValidationId) == null);
        }
    }
}
