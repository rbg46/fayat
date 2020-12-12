using System.Collections.Generic;
using FluentAssertions;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Entities.Common.CommonSearchExpression;
using Fred.Entities.Depense;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Common.CommonSearchExpression
{


    [TestClass]
    public class SearchExpressionTests
    {

        [TestInitialize]
        public void TestInitialize()
        {

        }

        private SearchExpression CreateSearchExpression()
        {
            return new SearchExpression();
        }


        private DepenseTypeEnt CreteReceptionType()
        {
            return new DepenseTypeEnt()
            {
                Code = Constantes.DepenseTypeCode.Reception
            };
        }

        // cl1 => somme des receptions = (Abs (0))
        // 10-0 > 0 
        // resultat =>  receptionnable
        [TestMethod]
        public void Une_commande_est_receptionnable_si_au_moins_une_ligne_de_commande_est_receptionnable_1()
        {
            // Arrange
            var isReceivableExpression = this.CreateSearchExpression();


            var commande = new CommandeEnt();
            commande.Lignes = new List<CommandeLigneEnt>() {
                new CommandeLigneEnt()
                {
                    PUHT = 1,
                    Quantite = 10,
                    AllDepenses = new List<DepenseAchatEnt>()// obligatoire, variable utilisée donc a initialisée
                },

            };

            // Act
            var expression = isReceivableExpression.GetCommandeReceivableExpression();

            var lambda = expression.Compile();

            var isReceivableExpressionResult = lambda.Invoke(commande);

            // Assert
            isReceivableExpressionResult.Should().BeTrue();

        }

        // cl1 => somme des receptions = Abs (10) = 10
        // 10 - 10 = 0 resultat =>  non receptionnable
        [TestMethod]
        public void Une_commande_est_receptionnable_si_au_moins_une_ligne_de_commande_est_receptionnable_2()
        {
            // Arrange
            var isReceivableExpression = this.CreateSearchExpression();
            var commande = new CommandeEnt();
            commande.Lignes = new List<CommandeLigneEnt>() {
                new CommandeLigneEnt()
                {
                    PUHT = 1,
                    Quantite = 10,
                    AllDepenses = new List<DepenseAchatEnt>()
                    {
                        new DepenseAchatEnt()
                        {
                            Quantite = 10,
                            PUHT=1,
                            DepenseType = CreteReceptionType()
                        }
                    }
                },

            };

            // Act
            var expression = isReceivableExpression.GetCommandeReceivableExpression();

            var lambda = expression.Compile();

            var isReceivableExpressionResult = lambda.Invoke(commande);

            // Assert
            isReceivableExpressionResult.Should().BeFalse();

        }

        // cl1 =>  somme des receptions = 5 + 4  =Abs(9)
        // cl2 =>  somme des receptions = 10+10  = Abs(20)
        // 10 + 10 - (9+20) = - 9 => non receptionnable
        [TestMethod]
        public void Une_commande_est_receptionnable_si_au_moins_une_ligne_de_commande_est_receptionnable_4()
        {
            // Arrange
            var isReceivableExpression = this.CreateSearchExpression();
            var commande = new CommandeEnt();
            commande.Lignes = new List<CommandeLigneEnt>() {
                new CommandeLigneEnt()
                {
                    PUHT = 1,
                    Quantite = 10,
                    AllDepenses = new List<DepenseAchatEnt>()
                    {
                        new DepenseAchatEnt()
                        {
                            Quantite = 5,
                            PUHT=1,
                            DepenseType = CreteReceptionType()
                        },
                        new DepenseAchatEnt()
                        {
                            Quantite = 4,
                            PUHT=1,
                            DepenseType = CreteReceptionType()
                        }
                    }
                },
                new CommandeLigneEnt()
                {
                    PUHT = 1,
                    Quantite = 10,
                    AllDepenses = new List<DepenseAchatEnt>()
                    {
                        new DepenseAchatEnt()
                        {
                            Quantite = 10,
                            PUHT= 1,
                            DepenseType =CreteReceptionType()
                        },
                        new DepenseAchatEnt()
                        {
                            Quantite = 10,
                            PUHT=1,
                            DepenseType =CreteReceptionType()
                        }
                    }
                },

            };

            // Act
            var expression = isReceivableExpression.GetCommandeReceivableExpression();

            var lambda = expression.Compile();

            var isReceivableExpressionResult = lambda.Invoke(commande);

            // Assert
            isReceivableExpressionResult.Should().BeFalse();

        }


        // cl1 =>  somme des receptions = Abs(-20) = 20          
        // 10 - 20 = -10  => non receptionnable
        [TestMethod]
        public void Une_commande_est_receptionnable_si_au_moins_une_ligne_de_commande_est_receptionnable_5()
        {
            // Arrange
            var isReceivableExpression = this.CreateSearchExpression();
            var commande = new CommandeEnt();
            commande.Lignes = new List<CommandeLigneEnt>() {
                new CommandeLigneEnt()
                {
                    PUHT = 1,
                    Quantite = 10,
                    AllDepenses = new List<DepenseAchatEnt>()
                    {
                        new DepenseAchatEnt()
                        {
                            Quantite = -20,
                            PUHT=1,
                            DepenseType = CreteReceptionType()
                        },
                    }
                }

            };


            // Act
            var expression = isReceivableExpression.GetCommandeReceivableExpression();

            var lambda = expression.Compile();

            var isReceivableExpressionResult = lambda.Invoke(commande);

            // Assert
            isReceivableExpressionResult.Should().BeFalse();

        }


        // cl1 =>  somme des receptions =20  =Abs(20)
        // cl2 =>  somme des receptions = 5-20   = Abs(-15)
        // 10 + 10 - (20+15) = - 15 => non receptionnable
        [TestMethod]
        public void Une_commande_est_receptionnable_si_au_moins_une_ligne_de_commande_est_receptionnable_6()
        {
            // Arrange
            var isReceivableExpression = this.CreateSearchExpression();
            var commande = new CommandeEnt();
            commande.Lignes = new List<CommandeLigneEnt>() {
                new CommandeLigneEnt()
                {
                    PUHT = 1,
                    Quantite = 10,
                    AllDepenses = new List<DepenseAchatEnt>()
                    {
                        new DepenseAchatEnt()
                        {
                            Quantite = 20,
                            PUHT=1,
                            DepenseType = CreteReceptionType()
                        },
                    }
                },
                 new CommandeLigneEnt()
                {
                    PUHT = 1,
                    Quantite = 10,
                    AllDepenses = new List<DepenseAchatEnt>()
                    {
                        new DepenseAchatEnt()
                        {
                            Quantite = 5,
                            PUHT=1,
                            DepenseType = CreteReceptionType()
                        },
                        new DepenseAchatEnt()
                        {
                            Quantite = -20,
                            PUHT=1,
                            DepenseType = new DepenseTypeEnt()
                            {
                                Code =  Constantes.DepenseTypeCode.Reception
                            }
                        },
                    }
                }

            };
            //var test = commande.Lignes.Sum(l => l.Quantite * l.PUHT);

            //var receptions =

            //var test2 = Math.Abs(commande.Lignes.SelectMany(l => l.AllDepenses).AsQueryable().Where(r => !r.DateSuppression.HasValue && r.DepenseType.Code == Constantes.DepenseTypeCode.Reception).Sum(r => r.Quantite * r.PUHT));

            // Act
            var expression = isReceivableExpression.GetCommandeReceivableExpression();

            var lambda = expression.Compile();

            var isReceivableExpressionResult = lambda.Invoke(commande);

            // Assert
            isReceivableExpressionResult.Should().BeFalse();
        }

        // cl1 =>  somme des receptions = (Abs (5))  =5
        // cl2 =>  somme des receptions = (Abs (-90+90))  = 0
        // 10 + 100 -(5-0) =>  receptionnable
        [TestMethod]
        public void Une_commande_est_receptionnable_si_au_moins_une_ligne_de_commande_est_receptionnable_7()
        {
            // Arrange
            var isReceivableExpression = this.CreateSearchExpression();
            var commande = new CommandeEnt();
            commande.Lignes = new List<CommandeLigneEnt>() {
                new CommandeLigneEnt()
                {
                    PUHT = 1,
                    Quantite = 10,
                    AllDepenses = new List<DepenseAchatEnt>()
                    {
                        new DepenseAchatEnt()
                        {
                            Quantite = 5,
                            PUHT=1,
                            DepenseType = CreteReceptionType()
                        },
                    }
                },
                 new CommandeLigneEnt()
                {
                    PUHT = 1,
                    Quantite = 100,
                    AllDepenses = new List<DepenseAchatEnt>()
                    {
                        new DepenseAchatEnt()
                        {
                            Quantite = -90,
                            PUHT=1,
                            DepenseType = CreteReceptionType()
                        },
                        new DepenseAchatEnt()
                        {
                            Quantite = +90,
                            PUHT=1,
                            DepenseType = CreteReceptionType()
                        },
                    }
                }

            };

            // Act
            var expression = isReceivableExpression.GetCommandeReceivableExpression();

            var lambda = expression.Compile();

            var isReceivableExpressionResult = lambda.Invoke(commande);

            // Assert
            isReceivableExpressionResult.Should().BeTrue();
        }
    }
}
