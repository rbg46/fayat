using System;
using System.Linq;
using Fred.Common.Tests.Helper;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestCodeMajoration : FredBaseTest
    {
        private UtilisateurEnt GetUser()
        {
            UtilisateurEnt utilisateur = UtilisateurRepository.Query().Filter(u => u.UtilisateurId == 1).Get().Single();
            if (utilisateur.Personnel == null)
            {
                utilisateur.Personnel = PersonnelRepository.Query().Include(p => p.Societe).Get().First();
            }

            return utilisateur;
        }

        /// <summary>
        ///   Teste que la liste des codes majoration retournée n'est jamais égale à null
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCodesMajoration()
        {

            var codesMaj = CodeMajorationMgr.GetCodeMajorationList();
            Assert.IsTrue(codesMaj != null);
        }

        /// <summary>
        ///   Retourne le rôle avec l'identifiant unique indiqué.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCodeMajorationById()
        {
            CodeMajorationEnt codeMajTest = new CodeMajorationEnt
            {
                Libelle = "Test Unitaire",
                Code = GuidEx.CreateBase64Guid(20),
                EtatPublic = false,
                IsActif = true,
                GroupeId = 1
            };

            UtilisateurEnt utilisateur = GetUser();
            int codeMajId = CodeMajorationMgr.AddCodeMajoration(codeMajTest, utilisateur);
            CodeMajorationEnt codeMaj2 = CodeMajorationMgr.GetCodeMajorationById(codeMajId);
            Assert.IsTrue(codeMaj2 != null);

            //Test cleanup
            CodeMajorationMgr.DeleteCodeMajorationById(codeMajTest);
        }

        /// <summary>
        ///   Teste que les codes majoration associés à la société sont bien retournés
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCodeMajorationListBySocieteId()
        {
            int groupeId = 1;

            CodeMajorationEnt code = new CodeMajorationEnt
            {
                Libelle = "Test Unitaire",
                Code = GuidEx.CreateBase64Guid(20),
                GroupeId = groupeId
            };

            var liste = CodeMajorationMgr.GetCodeMajorationListByGroupeId(groupeId);

            Assert.IsTrue(liste != null && liste.Any());
        }

        /// <summary>
        ///   Teste la création d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddNewCodeMajoration()
        {
            CodeMajorationEnt codeMajoration = new CodeMajorationEnt
            {
                Libelle = "Test Unitaire",
                Code = GuidEx.CreateBase64Guid(20),
                EtatPublic = false,
                IsActif = true,
                GroupeId = 1
            };

            var repo = CodeMajorationRepository;
            int countBefore = repo.Query().Get().Count();
            UtilisateurEnt utilisateur = GetUser();
            CodeMajorationMgr.AddCodeMajoration(codeMajoration, utilisateur);
            int countAfter = repo.Query().Get().Count();
            Assert.IsTrue((countBefore + 1) == countAfter);

            //Test cleanup
            CodeMajorationMgr.DeleteCodeMajorationById(codeMajoration);
        }

        /// <summary>
        ///   Teste la mise à jour d'un enregistrement existant en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void UpdateExistingCodeMajoration()
        {
            CodeMajorationEnt codeMajoration = new CodeMajorationEnt
            {
                Libelle = "Test Unitaire",
                Code = GuidEx.CreateBase64Guid(20),
                EtatPublic = false,
                IsActif = true,
                GroupeId = 1
            };

            UtilisateurEnt utilisateur = GetUser();
            int codeMajId = CodeMajorationMgr.AddCodeMajoration(codeMajoration, utilisateur);
            string libBefore = codeMajoration.Libelle;
            codeMajoration.Libelle = "Test";
            CodeMajorationMgr.UpdateCodeMajoration(codeMajoration);
            CodeMajorationEnt codeMajorationAfter = CodeMajorationMgr.GetCodeMajorationById(codeMajId);
            Assert.AreNotEqual(libBefore, codeMajorationAfter.Libelle);

            //Test cleanup
            CodeMajorationMgr.DeleteCodeMajorationById(codeMajoration);
        }

        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void DeleteExistingCodeMajoration()
        {
            CodeMajorationEnt codeMajoration = new CodeMajorationEnt
            {
                Libelle = "Test Unitaire",
                Code = GuidEx.CreateBase64Guid(20),
                EtatPublic = false,
                IsActif = true,
                GroupeId = 1
            };
            UtilisateurEnt utilisateur = GetUser();
            int codeMajId = CodeMajorationMgr.AddCodeMajoration(codeMajoration, utilisateur);
            var repo = CodeMajorationRepository;
            int countBefore = repo.Query().Get().Count();
            Assert.IsTrue(CodeMajorationMgr.DeleteCodeMajorationById(codeMajoration));
            int countAfter = repo.Query().Get().Count();
            Assert.IsTrue(countBefore == (countAfter + 1));
        }

        /// <summary>
        ///   Teste la récupération d'un code majoration via CiCodeMajoration par un CI ID
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCodeMajorationIdsByCiId()
        {
            CodeMajorationEnt codeMajoration = new CodeMajorationEnt
            {
                Libelle = "Test Unitaire Add",
                Code = GuidEx.CreateBase64Guid(20),
                EtatPublic = false,
                IsActif = true,
                GroupeId = 1
            };
            UtilisateurEnt utilisateur = GetUser();
            int codeId = CodeMajorationMgr.AddCodeMajoration(codeMajoration, utilisateur);

            CIEnt ci = new CIEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "CI de test de création",
                DateOuverture = new DateTime(2016, 1, 1),
                DateFermeture = new DateTime(2016, 1, 1),
                EtablissementComptableId = 1,
                Sep = false
            };
            int ciId = CIMgr.AddCI(ci).CiId;

            CICodeMajorationEnt cIcodeMaj = new CICodeMajorationEnt
            {
                CiId = ciId,
                CodeMajorationId = codeId
            };

            CodeMajorationMgr.AddCiCodesMajoration(cIcodeMaj.CodeMajorationId, cIcodeMaj.CiId);

            var idList = CodeMajorationMgr.GetCodeMajorationIdsByCiId(ciId);

            Assert.IsTrue(idList.Count > 0);

            //Test cleanup
            CodeMajorationMgr.DeleteCICodeMajorationById(codeId, ciId);
            CIMgr.DeleteCIById(ciId);
            CodeMajorationMgr.DeleteCodeMajorationById(codeMajoration);
        }

        /// <summary>
        ///   Ajout un nouvelle association CI/Code majoration
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddCiCodesMajoration()
        {
            CodeMajorationEnt codeMajoration = new CodeMajorationEnt
            {
                Libelle = "Test Unitaire Add",
                Code = GuidEx.CreateBase64Guid(20),
                EtatPublic = false,
                IsActif = true,
                GroupeId = 1
            };
            UtilisateurEnt utilisateur = GetUser();
            int codeId = CodeMajorationMgr.AddCodeMajoration(codeMajoration, utilisateur);

            CIEnt ci = new CIEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "CI de test de création",
                DateOuverture = new DateTime(2016, 1, 1),
                DateFermeture = new DateTime(2016, 1, 1),
                EtablissementComptableId = 1,
                Sep = false
            };

            int ciId = CIMgr.AddCI(ci).CiId;

            CICodeMajorationEnt cIcodeMaj = new CICodeMajorationEnt
            {
                CiId = ciId,
                CodeMajorationId = codeId
            };

            int countBefore = CodeMajorationMgr.GetCiCodeMajorationList().Count();
            CodeMajorationMgr.AddCiCodesMajoration(cIcodeMaj.CodeMajorationId, cIcodeMaj.CiId);
            int countAfter = CodeMajorationMgr.GetCiCodeMajorationList().Count();
            Assert.IsTrue((countBefore + 1) == countAfter);

            //Test cleanup
            CodeMajorationMgr.DeleteCICodeMajorationById(codeId, ciId);
            CIMgr.DeleteCIById(ciId);
            CodeMajorationMgr.DeleteCodeMajorationById(codeMajoration);
        }

        /// <summary>
        ///   Supprime une association entre un code majoration et un CI
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void DeleteCICodeMajorations()
        {
            CodeMajorationEnt codeMajoration = new CodeMajorationEnt
            {
                Libelle = "Test Unitaire Add",
                Code = GuidEx.CreateBase64Guid(20),
                EtatPublic = false,
                IsActif = true,
                GroupeId = 1
            };
            UtilisateurEnt utilisateur = GetUser();
            int codeId = CodeMajorationMgr.AddCodeMajoration(codeMajoration, utilisateur);
            CIEnt ci = new CIEnt
            {
                Code = GuidEx.CreateBase64Guid(20),
                Libelle = "CI de test de création",
                DateOuverture = new DateTime(2016, 1, 1),
                DateFermeture = new DateTime(2016, 1, 1),
                EtablissementComptableId = 1,
                Sep = false
            };
            int ciId = CIMgr.AddCI(ci).CiId;

            CICodeMajorationEnt cIcodeMaj = new CICodeMajorationEnt
            {
                CiId = ciId,
                CodeMajorationId = codeId
            };


            CodeMajorationMgr.AddCiCodesMajoration(cIcodeMaj.CodeMajorationId, cIcodeMaj.CiId);
            int countBefore = CodeMajorationMgr.GetCiCodeMajorationList().Count();

            CodeMajorationMgr.DeleteCICodeMajorationById(cIcodeMaj.CodeMajorationId, cIcodeMaj.CiId);
            int countAfter = CodeMajorationMgr.GetCiCodeMajorationList().Count();

            Assert.IsTrue(countAfter < countBefore);


            //Test cleanup
            CIMgr.DeleteCIById(ciId);
            CodeMajorationMgr.DeleteCodeMajorationById(codeMajoration);
        }
    }
}
