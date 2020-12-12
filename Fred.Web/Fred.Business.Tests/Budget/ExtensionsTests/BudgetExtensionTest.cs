using System;
using System.Collections.Generic;
using Fred.Business.Budget.Extensions;
using Fred.Entities.Budget;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Fred.Business.Tests.ModelBuilder.BudgetEtat;
using static Fred.Business.Tests.ModelBuilder.Utilisateur;

namespace Fred.Business.Tests.Budget.ExtensionsTests
{
    [TestClass]
    public class BudgetExtensionTest
    {
        private const string AuteurCreateur = "CREATEUR";
        private const string AuteurValideur = "VALIDEUR";
        private readonly BudgetEnt budget = new BudgetEnt();
        private DateTime dateCreationExpected;
        private DateTime dateDemandeValidationExpected;
        private DateTime dateValidationExpected;

        [TestInitialize]
        public void Init()
        {
            dateCreationExpected = new DateTime(2018, 6, 30);
            dateDemandeValidationExpected = new DateTime(2018, 7, 5);
            dateValidationExpected = new DateTime(2018, 7, 10);

            budget.Workflows = new List<BudgetWorkflowEnt>()
            {
                //Création du bduget
                new BudgetWorkflowEnt()
                {
                    Auteur = GetUtilisateurAvecNomEtPrenom(AuteurCreateur, AuteurCreateur),
                    Date = dateCreationExpected,
                    EtatCible = GetFakeBudgetEtatBrouillon(),
                    EtatInitialId = null
                },
                //Demande de validation
                new BudgetWorkflowEnt()
                {
                    Auteur = GetUtilisateurAvecNomEtPrenom(AuteurCreateur, AuteurCreateur),
                    Date = dateDemandeValidationExpected,
                    EtatCible  = GetFakeBudgetEtatAValider(),
                    EtatInitial = GetFakeBudgetEtatBrouillon()
                },
                //Validation
                new BudgetWorkflowEnt()
                {
                    Auteur = GetUtilisateurAvecNomEtPrenom(AuteurValideur, AuteurValideur),
                    Date = dateValidationExpected,
                    EtatCible  = GetFakeBudgetEtatEnApplication(),
                    EtatInitial = GetFakeBudgetEtatAValider()
                }
            };
        }

        [TestMethod]
        public void GetValideurBudget()
        {
            var valideur = budget.GetValideurBudget();
            Assert.AreEqual($"{AuteurValideur} {AuteurValideur}", valideur);
        }

        [TestMethod]
        public void GetCreateur()
        {
            var createur = budget.GetCreateur();
            Assert.AreEqual($"{AuteurCreateur} {AuteurCreateur}", createur);
        }


        [TestMethod]
        public void GetDateCreation()
        {
            var dateCreation = budget.GetDateCreation();
            Assert.AreEqual(dateCreationExpected, dateCreation);
        }

        [TestMethod]
        public void GetDateDerniereModification()
        {
            var dateModification = budget.GetDateDerniereModification();
            Assert.AreEqual(dateValidationExpected, dateModification);
        }

        [TestMethod]
        public void GetDateValidation()
        {
            var dateValidation = budget.GetDateValidation();
            Assert.AreEqual(dateValidationExpected, dateValidation);
        }

        [TestMethod]
        public void GetAuteurDerniereModification()
        {
            var auteurDerniereModification = budget.GetAuteurDerniereModification();
            Assert.AreEqual($"{AuteurValideur} {AuteurValideur}", auteurDerniereModification);
        }
    }
}
