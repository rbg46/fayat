using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fred.Business.Commande;
using Fred.Common.Tests.Data.Commande.Builder;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public class TestCommande : FredBaseTest
    {
        private readonly CommandeBuilder commandeBuilder = new CommandeBuilder();

        /// <summary>
        ///   Teste que la liste des commandes n'est jamais nulle.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetCommandeListReturnNotNullList()
        {
            //2. Action get sur la base de donnees
            var commandes = CommandeMgr.GetCommandeList();

            //3. Check si la liste est null
            Assert.IsTrue(commandes != null);
        }

        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void AddNewCommande()
        {
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
                DeviseId = 48 //€                
            };

            CommandeLigneEnt commandeLigneTest = new CommandeLigneEnt
            {
                CommandeId = commande.CommandeId,
                Libelle = "Libelle Commande de test",
                RessourceId = 1,
                Quantite = 10,
                PUHT = 15.90M,
                Unite = this.UniteMgr.FindById(4)
            };

            commande.Lignes = new[] { commandeLigneTest };

            commande.Devise = DeviseMgr.GetById(commande.DeviseId.Value);

            int countBefore = CommandeRepository.Query().Get().Count();

            CommandeMgr.AddCommande(commande);

            int countAfter = CommandeRepository.Query().Get().Count();

            Assert.IsTrue((countBefore + 1) == countAfter);
        }

        /// <summary>
        ///   Teste la mise à jour d'un enregistrement existant en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void UpdateExistingCommande()
        {
            CommandeEnt commande = new CommandeEnt
            {
                TypeId = 1,
                CiId = 1,
                Date = new DateTime(2016, 1, 1),
                DateCreation = DateTime.Now,
                Libelle = "Commande de test",
                FournisseurId = 1,
                Numero = "TEST00000002",
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
                DeviseId = 48 //€
            };

            CommandeLigneEnt commandeLigneTest = new CommandeLigneEnt
            {
                CommandeId = commande.CommandeId,
                Libelle = "Libelle Commande de test",
                RessourceId = 1,
                Quantite = 10,
                PUHT = 15.90M,
                Unite = this.UniteMgr.FindById(4)
            };

            commande.Lignes = new[] { commandeLigneTest };

            CommandeMgr.AddCommande(commande);

            string libBefore = commande.Libelle;
            commande.Libelle = "Test Commande après modification";
            CommandeMgr.UpdateCommande(commande);

            Assert.AreNotEqual(libBefore, commande.Libelle);
        }

        /// <summary>
        ///   Teste la validation de la commande
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        [Ignore]
        public void ValidateCommande()
        {
            //1. Preparation des donnees
            CommandeEnt commande =
                commandeBuilder
                    .Libelle("MaCommandeAvecQuantiteInvalide").Numero("2200001")
                    .Statut.AValider()
                    .Do(c => c.ComputeMontantHT())
                    .Build();

            //2. Lancement de l'action
            CommandeMgr.AddCommande(commande);
            CommandeMgr.UpdateCommande(commande);

            //3. Condition attendue : on retrouve la commande avec le statut validee.
            CommandeEnt commandeValidee = CommandeMgr.GetCommandeById(commande.CommandeId);
            Assert.IsTrue(commandeValidee.IsStatutValidee);
        }

        /// <summary>
        ///   Teste la validation de la commande
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void ValidateInvalidCommande()
        {
            CommandeEnt commande = new CommandeEnt
            {
                CiId = 1,
                TypeId = 1,
                Date = new DateTime(2016, 1, 1),
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
                LivraisonAdresse = "blabla",
                LivraisonVille = "blabla",
                LivraisonCPostale = "34000",
                FacturationAdresse = "blabla",
                FacturationVille = "blabla",
                FacturationCPostale = "34000",
                StatutCommandeId = 1,
                DeviseId = 48, //€
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
        }
            };

            CommandeMgr.AddCommande(commande);
            CommandeMgr.UpdateCommande(commande);

            Assert.IsFalse(commande.IsStatutValidee);
        }

        /// <summary>
        ///   Teste la suppression d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        [Ignore]
        public void DeleteExistingCommande()
        {
            CommandeEnt commande = new CommandeEnt
            {
                TypeId = 1,
                CiId = 1,
                Date = new DateTime(2016, 1, 1),
                DateCreation = DateTime.Now,
                Libelle = "Commande de test",
                FournisseurId = 1,
                Numero = "TEST00000003",
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
                DeviseId = 48 //€
            };

            CommandeLigneEnt commandeLigneTest = new CommandeLigneEnt
            {
                CommandeId = commande.CommandeId,
                Libelle = "Libelle Commande de test",
                RessourceId = 1,
                Quantite = 10,
                PUHT = 15.90M,
                Unite = this.UniteMgr.FindById(4)
            };

            commande.Lignes = new[] { commandeLigneTest };

            CommandeMgr.AddCommande(commande);

            CommandeMgr.DeleteCommandeById(commande.CommandeId);
            CommandeEnt commandeDeleted = CommandeMgr.GetCommandeById(commande.CommandeId);

            Assert.IsTrue(commandeDeleted.DateSuppression != null);
        }

        /// <summary>
        ///   Teste la dupplication d'un nouvel enregistrement en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        [Ignore]
        public void DuplicateCommande()
        {
            bool duplicationSuccess = true;

            CommandeEnt commande = new CommandeEnt
            {
                TypeId = 1,
                CiId = 1,
                Date = new DateTime(2016, 1, 1),
                Libelle = "Commande de test",
                FournisseurId = 1,
                Numero = "TEST00000003",
                AuteurCreationId = 1,
                FraisAmortissement = false,
                FraisAssurance = false,
                Carburant = false,
                Lubrifiant = false,
                EntretienJournalier = false,
                EntretienMecanique = false,
                CommandeManuelle = false,
                MOConduite = false,
                StatutCommandeId = 1,
                Lignes = new[] { new CommandeLigneEnt { Libelle = "Test", Quantite = 1.5M, PUHT = 50, Unite = this.UniteMgr.FindById(4), TacheId = 1, RessourceId = 1 } },
                DeviseId = 48 //€
            };

            CommandeMgr.AddCommande(commande);

            CommandeEnt commandeDupliquee = CommandeMgr.DuplicateCommande(commande.CommandeId);

            duplicationSuccess = duplicationSuccess && commandeDupliquee.CommandeId == 0;
            duplicationSuccess = duplicationSuccess && string.IsNullOrEmpty(commandeDupliquee.Numero);
            duplicationSuccess = duplicationSuccess && commande.CiId == commandeDupliquee.CiId;
            duplicationSuccess = duplicationSuccess && commande.FournisseurId == commandeDupliquee.FournisseurId;
            duplicationSuccess = duplicationSuccess && commande.Libelle == commandeDupliquee.Libelle;
            duplicationSuccess = duplicationSuccess && !commandeDupliquee.AuteurModificationId.HasValue;
            duplicationSuccess = duplicationSuccess && !commandeDupliquee.AuteurSuppressionId.HasValue;
            duplicationSuccess = duplicationSuccess && commande.Carburant == commandeDupliquee.Carburant;
            duplicationSuccess = duplicationSuccess && commande.CommentaireFournisseur == commandeDupliquee.CommentaireFournisseur;
            duplicationSuccess = duplicationSuccess && commande.CommentaireInterne == commandeDupliquee.CommentaireInterne;
            duplicationSuccess = duplicationSuccess && commande.ConditionPrestation == commandeDupliquee.ConditionPrestation;
            duplicationSuccess = duplicationSuccess && commande.ConditionSociete == commandeDupliquee.ConditionSociete;
            duplicationSuccess = duplicationSuccess && commande.ContactId == commandeDupliquee.ContactId;
            duplicationSuccess = duplicationSuccess && commande.ContactTel == commandeDupliquee.ContactTel;
            duplicationSuccess = duplicationSuccess && commandeDupliquee.Date == DateTime.Today;
            duplicationSuccess = duplicationSuccess && commandeDupliquee.DateCloture == null;
            duplicationSuccess = duplicationSuccess && commande.DateMiseADispo == commandeDupliquee.DateMiseADispo;
            duplicationSuccess = duplicationSuccess && commandeDupliquee.DateModification == null;
            duplicationSuccess = duplicationSuccess && commandeDupliquee.DateSuppression == null;
            duplicationSuccess = duplicationSuccess && commandeDupliquee.DateValidation == null;
            duplicationSuccess = duplicationSuccess && commande.DelaiLivraison == commandeDupliquee.DelaiLivraison;
            duplicationSuccess = duplicationSuccess && commande.EntretienJournalier == commandeDupliquee.EntretienJournalier;
            duplicationSuccess = duplicationSuccess && commande.EntretienMecanique == commandeDupliquee.EntretienMecanique;
            duplicationSuccess = duplicationSuccess && commande.FacturationAdresse == commandeDupliquee.FacturationAdresse;
            duplicationSuccess = duplicationSuccess && commande.FacturationCPostale == commandeDupliquee.FacturationCPostale;
            duplicationSuccess = duplicationSuccess && commande.FacturationVille == commandeDupliquee.FacturationVille;
            duplicationSuccess = duplicationSuccess && commande.FraisAmortissement == commandeDupliquee.FraisAmortissement;
            duplicationSuccess = duplicationSuccess && commande.FraisAssurance == commandeDupliquee.FraisAssurance;
            duplicationSuccess = duplicationSuccess && commande.CommandeManuelle == commandeDupliquee.CommandeManuelle;

            duplicationSuccess = duplicationSuccess && commande.Justificatif == commandeDupliquee.Justificatif;
            duplicationSuccess = duplicationSuccess && commande.LivraisonAdresse == commandeDupliquee.LivraisonAdresse;
            duplicationSuccess = duplicationSuccess && commande.LivraisonCPostale == commandeDupliquee.LivraisonCPostale;
            duplicationSuccess = duplicationSuccess && commande.LivraisonVille == commandeDupliquee.LivraisonVille;
            duplicationSuccess = duplicationSuccess && commande.Lubrifiant == commandeDupliquee.Lubrifiant;
            duplicationSuccess = duplicationSuccess && commande.MOConduite == commandeDupliquee.MOConduite;
            duplicationSuccess = duplicationSuccess && commande.MontantHT == commandeDupliquee.MontantHT;
            duplicationSuccess = duplicationSuccess && commande.MontantHTReceptionne == commandeDupliquee.MontantHTReceptionne;
            duplicationSuccess = duplicationSuccess && commande.MontantHTSolde == commandeDupliquee.MontantHTSolde;
            duplicationSuccess = duplicationSuccess && commande.PourcentageReceptionne == commandeDupliquee.PourcentageReceptionne;
            duplicationSuccess = duplicationSuccess && commande.TypeId == commandeDupliquee.TypeId;
            duplicationSuccess = duplicationSuccess && commandeDupliquee.ValideurId == null;

            duplicationSuccess = duplicationSuccess && commande.Lignes.Count == commandeDupliquee.Lignes.Count;

            Assert.IsTrue(duplicationSuccess);
        }

        /// <summary>
        ///   Teste recherche des tâches
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]

        [TestCategory("DBDepend")]
        [Ignore]
        public void GetCommandeListReturnAtLeastOneRecord()
        {
            var commandes = CommandeMgr.GetCommandeList().ToList();
            Assert.IsTrue(commandes.Count > 0);
        }
        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        [Ignore]
        public void GetNonExistingCommandeReturnNull()
        {
            CommandeEnt commande = CommandeMgr.GetCommandeById(-1);
            Assert.IsNull(commande);
        }

        /// <summary>
        ///   Teste la récupération d'un enregistrement spécifique en base de données.
        ///   Cet exemple prend en compte le fait qu'une base de données de tests existe.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void GetNonExistingCommandeLigneReturnNull()
        {
            CommandeLigneEnt commandeLigne = CommandeMgr.GetCommandeLigneById(-1);
            Assert.IsNull(commandeLigne);
        }

        [TestMethod]
        [TestCategory("DBDepend")]

        [Ignore]
        public void AffectationCreationCodeTacheParDefaut()
        {
            // Création de la commande de test
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
                Lignes = new Collection<CommandeLigneEnt>()
            };

            // Création de la ligne de commande sans renseigner le code tâche
            CommandeLigneEnt commandeLigneTest = new CommandeLigneEnt
            {
                CommandeId = commande.CommandeId,
                Libelle = "Test ligne 1 affectation de code tâche par défaut (création)",
                RessourceId = 1,
                Quantite = 10,
                PUHT = 15.90M,
                Unite = this.UniteMgr.FindById(4)
            };

            commande.Lignes.Add(commandeLigneTest);

            // Création de la ligne de commande sans renseigner le code tâche
            CommandeLigneEnt commandeLigneTest2 = new CommandeLigneEnt
            {
                CommandeId = commande.CommandeId,
                Libelle = "Test ligne 2 affectation de code tâche par défaut (création)",
                RessourceId = 1,
                Quantite = 54,
                PUHT = 15,
                Unite = this.UniteMgr.FindById(4)
            };

            commande.Lignes.Add(commandeLigneTest2);

            // Création de la commande
            CommandeMgr.AddCommande(commande);

            // Toutes les lignes de la commande doivent posséder un code tâchepâr défaut si il n'est pas renseigné
            bool verifOK = !commande.Lignes.Any(l => l.TacheId == null);

            Assert.IsTrue(verifOK);
        }

        /// <summary>
        ///   Teste le remplissage automatique des codes tâches (si vides) lors de la création d'une ligne de commande.
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        [Ignore]
        public void AffectationModificationCodeTacheParDefaut()
        {
            // Création de la commande de test
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
                Lignes = new Collection<CommandeLigneEnt>()
            };

            // Création de la ligne de commande sans renseigner le code tâche
            CommandeLigneEnt commandeLigneTest = new CommandeLigneEnt
            {
                CommandeId = commande.CommandeId,
                Libelle = "Test ligne 1 affectation de code tâche par défaut (création)",
                RessourceId = 1,
                Quantite = 10,
                PUHT = 15.90M,
                Unite = this.UniteMgr.FindById(4)
            };

            commande.Lignes.Add(commandeLigneTest);

            // Création de la ligne de commande sans renseigner le code tâche
            CommandeLigneEnt commandeLigneTest2 = new CommandeLigneEnt
            {
                CommandeId = commande.CommandeId,
                Libelle = "Test ligne 2 affectation de code tâche par défaut (création)",
                RessourceId = 1,
                TacheId = 1,
                Quantite = 54,
                PUHT = 15,
                Unite = this.UniteMgr.FindById(4)
            };

            commande.Lignes.Add(commandeLigneTest2);

            // Création de la commande
            CommandeMgr.AddCommande(commande);

            // Vidage du code tâche de la deuxieme ligne de commande pour test
            commande.Lignes.AsEnumerable().ElementAt(1).TacheId = null;

            // Modification de la commande
            CommandeMgr.UpdateCommande(commande);

            // Toutes les lignes de la commande doivent posséder un code tâche pâr défaut si il n'est pas renseigné
            bool verifOK = !commande.Lignes.Any(l => l.TacheId == null);

            Assert.IsTrue(verifOK);
        }
    }
}
