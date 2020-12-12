using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Referential;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Fred.Business.Tests.Integration
{
    [TestClass]

    public class TestReception : FredBaseTest
    {
        /// <summary>
        ///   Teste la création d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void AddNewReception()
        {
            DeviseEnt d = DeviseMgr.GetById(48); // €

            CommandeEnt commande = new CommandeEnt
            {
                CiId = 1,
                TypeId = 1,
                Date = new DateTime(2016, 1, 1),
                Libelle = "Commande de test",
                FournisseurId = 1,
                Numero = "TEST00000001",
                AuteurCreationId = UserMgr.GetContextUtilisateurId(),
                FraisAmortissement = false,
                FraisAssurance = false,
                Carburant = false,
                Lubrifiant = false,
                EntretienJournalier = false,
                EntretienMecanique = false,
                MOConduite = false,
                CommandeManuelle = false,
                StatutCommandeId = 1,
                Lignes = new List<CommandeLigneEnt>(),
                DeviseId = d.DeviseId
            };

            CommandeLigneEnt commandeLigneEnt = new CommandeLigneEnt
            {
                Libelle = "Test création de réception",
                Quantite = 10,
                PUHT = 5,
                TacheId = 1,
                RessourceId = 1,
                Unite = this.UniteMgr.FindById(4),
                DepensesReception = new DepenseAchatEnt[] { }
            };

            commande.Lignes.Add(commandeLigneEnt);

            //Probleme avec ce test ici: etant DBDepend, il essaie de sauvegarder dans la base la commande ET le type ET le statut, pourquoi donc ???? 
            //Ligne 288, 289, 290 du CommandeValidator ou la methode Validate() fait une affectation de ces entites (???)
            //Solution : Passer par des mocks et des builders et virer le DBDepend
            CommandeMgr.AddCommande(commande);

            DepenseAchatEnt reception = new DepenseAchatEnt
            {
                CiId = 1,
                CommandeLigneId = commande.Lignes.First().CommandeLigneId,
                Libelle = "Test création de réception",
                Quantite = 10,
                PUHT = 5,
                TacheId = 1,
                RessourceId = 1,
                Unite = this.UniteMgr.FindById(4),
                AuteurCreationId = 1,
                DateCreation = DateTime.Now
            };

            int countRepoBefore = DepenseMgr.GetDepenseList().Count();
            DepenseMgr.AddDepense(reception);
            int countRepoAfter = DepenseMgr.GetDepenseList().Count();

            commande = CommandeMgr.GetCommandeById(commande.CommandeId);

            Assert.IsTrue((countRepoBefore + 1) == countRepoAfter && commande.Lignes.First().DepensesReception.Count() == 1);
        }

        /// <summary>
        ///   Teste la mise à jour d'un enregistrement existant en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void UpdateExistingReception()
        {
            DeviseEnt d = DeviseMgr.GetById(48); // €
            CommandeEnt commande = new CommandeEnt
            {
                CiId = 1,
                TypeId = 1,
                Date = new DateTime(2016, 1, 1),
                Libelle = "Commande de test",
                FournisseurId = 1,
                Numero = "TEST00000001",
                AuteurCreationId = UserMgr.GetContextUtilisateurId(),
                FraisAmortissement = false,
                FraisAssurance = false,
                Carburant = false,
                Lubrifiant = false,
                EntretienJournalier = false,
                EntretienMecanique = false,
                MOConduite = false,
                CommandeManuelle = false,
                StatutCommandeId = 1,
                Lignes = new[]
              {
          new CommandeLigneEnt
          {
            Libelle = "Test création de réception",
            Quantite = 10,
            PUHT = 5,
            TacheId = 1,
            RessourceId = 1,
            Unite = this.UniteMgr.FindById(4),
            DepensesReception = new DepenseAchatEnt[] { }
          }
        },
                DeviseId = d.DeviseId
            };

            CommandeMgr.AddCommande(commande);

            DepenseAchatEnt reception = new DepenseAchatEnt
            {
                CiId = 1,
                CommandeLigneId = commande.Lignes.First().CommandeLigneId,
                Libelle = "Test modification de réception",
                Quantite = 10,
                PUHT = 5,
                TacheId = 1,
                RessourceId = 1,
                Unite = this.UniteMgr.FindById(4)
            };

            int receptionId = DepenseMgr.AddDepense(reception).DepenseId;
            string libBefore = reception.Libelle;
            reception.Libelle = "Test réception après modification";
            DepenseMgr.UpdateDepense(reception);
            DepenseAchatEnt receptionAfter = DepenseMgr.GetDepenseById(receptionId);

            Assert.AreNotEqual(libBefore, receptionAfter.Libelle);
        }

        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        [Ignore]
        public void DeleteExistingReception()
        {
            DeviseEnt d = DeviseMgr.GetById(48); // €

            CommandeEnt commande = new CommandeEnt
            {
                CiId = 1,
                TypeId = 1,
                Date = new DateTime(2016, 1, 1),
                Libelle = "Commande de test",
                FournisseurId = 1,
                Numero = "TEST00000001",
                AuteurCreationId = UserMgr.GetContextUtilisateurId(),
                FraisAmortissement = false,
                FraisAssurance = false,
                Carburant = false,
                Lubrifiant = false,
                EntretienJournalier = false,
                EntretienMecanique = false,
                MOConduite = false,
                CommandeManuelle = false,
                StatutCommandeId = 1,
                Lignes = new[]
              {
          new CommandeLigneEnt
          {
            Libelle = "Test création de réception",
            Quantite = 10,
            PUHT = 5,
            TacheId = 1,
            RessourceId = 1,
            Unite = this.UniteMgr.FindById(4),
            DepensesReception = new DepenseAchatEnt[] { }
          }
        },
                DeviseId = d.DeviseId
            };

            CommandeMgr.AddCommande(commande);

            DepenseAchatEnt reception = new DepenseAchatEnt
            {
                CiId = 1,
                CommandeLigneId = commande.Lignes.First().CommandeLigneId,
                Libelle = "Test création de réception",
                Quantite = 10,
                PUHT = 5,
                TacheId = 1,
                RessourceId = 1,
                Unite = this.UniteMgr.FindById(4)
            };

            int receptionId = DepenseMgr.AddDepense(reception).DepenseId;
            int countRepoBefore = DepenseMgr.GetDepenseList().Count();
            DepenseMgr.DeleteDepenseById(receptionId);
            int countRepoAfter = DepenseMgr.GetDepenseList().Count();

            Assert.IsTrue((countRepoBefore - 1) == countRepoAfter);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetNewReception()
        {
            DeviseEnt d = DeviseMgr.GetById(48); // €

            CommandeEnt commande = new CommandeEnt
            {
                CiId = 1,
                TypeId = 1,
                Date = new DateTime(2016, 1, 1),
                DateCreation = DateTime.Now,
                Libelle = "Commande de test",
                FournisseurId = 1,
                Numero = "TEST00000001",
                AuteurCreationId = 1,
                FraisAmortissement = false,
                FraisAssurance = false,
                Carburant = false,
                Lubrifiant = false,
                EntretienJournalier = false,
                EntretienMecanique = false,
                MOConduite = false,
                CommandeManuelle = false,
                StatutCommandeId = 1,
                Lignes = new List<CommandeLigneEnt>(),
                DeviseId = d.DeviseId //€
            };

            CommandeLigneEnt commandeLigne = new CommandeLigneEnt
            {
                Libelle = "Test création de réception",
                Quantite = 10,
                PUHT = 5,
                TacheId = 1,
                RessourceId = 1,
                Unite = this.UniteMgr.FindById(4),
                DepensesReception = new DepenseAchatEnt[] { }
            };

            commande.Lignes.Add(commandeLigne);

            CommandeMgr.AddCommande(commande);

            DepenseAchatEnt reception = DepenseMgr.GetNewDepense(commandeLigne.CommandeLigneId);

            Assert.IsTrue(reception.CommandeLigneId == commandeLigne.CommandeLigneId);
        }
    }
}
