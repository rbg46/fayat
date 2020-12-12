using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentValidation.Results;
using Fred.Business.Reception.Validators;
using Fred.Business.Reception.Validators.GFTP;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CommandeLigne.QuantiteNegative;
using Fred.Entities.Depense;
using Fred.Entities.Reception.QuantiteNegative;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Reception.Validators
{
    [TestClass]
    public class ReceptionNegativeRulesValidatorTests
    {
        private MockRepository mockRepository;

        private Mock<ICommandeLignesRepository> mockCommandeLignesRepository;
        public string ErrorQuantitySuppOrEgual = BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Supp_Or_Egual;// "La quantité doit être supérieure ou égale à {0} ";
        public string ErrorQuantityInfOrEgual = BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Inf_Or_Egual;//"La quantité doit être inférieure ou égale à {0} ";
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockCommandeLignesRepository = this.mockRepository.Create<ICommandeLignesRepository>();
        }

        private ReceptionQuantityRulesValidatorFtp CreateReceptionNegativeRulesValidator()
        {
            return new ReceptionQuantityRulesValidatorFtp(this.mockCommandeLignesRepository.Object);
        }

        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // + 					    | 2							    	  | 2								  | 3								  | ok	     |  -
        [TestMethod]
        public void Reception_Positive_Update_1_reception_where_the_sum_is_positive_on_command_line_positive_should_be_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
               .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = false,
                        QuantiteReceptionnee = 2,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = 2
                            }
                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 1,
                    CommandeLigneId = 1,
                    Quantite = 3
                }
            };
            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeTrue();

        }


        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // + 					    | 5							    	  | 5								  | 6								  | ok	     |  -
        [TestMethod]
        public void Reception_Positive_Update_1_reception_where_the_sum_is_positive_on_command_line_positive_should_be_ok_2()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
              .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = false,
                        QuantiteReceptionnee = 5,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = 5
                            }
                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 1,
                    CommandeLigneId = 1,
                    Quantite = 6
                }
            };

            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeTrue();

        }


        // commande ligne + ou -  | Cumul des receptions receptionnes     | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        //	+					  | 0							    	  | 0								  | -10								  | ko	     |  La quantité doit être supérieure ou égale à 0
        [TestMethod]
        public void Reception_Positive_Update_1_reception_where_the_sum_is_negative_on_command_line_positive_should_be_not_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
               .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = false,
                        QuantiteReceptionnee = 15,
                        ReceptionQuantites =  new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = 5
                            },
                             new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 2,
                                 Quantity = 10
                            }
                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 1,
                    CommandeLigneId = 1,
                    Quantite = -100
                }
            };

            // Act
            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeFalse();

            var firstError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_1").FirstOrDefault();
            firstError.Should().NotBeNull();

            var errorMessage = string.Format(BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Supp_Or_Egual, 10);
            firstError.ErrorMessage.Should().Be(errorMessage);
        }


        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // + 					    | 5							    	  | 5								  | 5,-5							  | ok	     |  -
        [TestMethod]
        public void Reception_Positive_Add_1_reception_where_the_sum_is_ZERO_on_command_line_positive_should_be_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
              .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = false,
                        QuantiteReceptionnee = 5,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = 5
                            }
                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 0,
                    CommandeLigneId = 1,
                    Quantite = -5
                }
            };

            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeTrue();

        }


        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // + 					    | 0							    	  | 5,-5							  | 4,-5							  | ko	     |  -
        [TestMethod]
        public void Reception_Positive_remove_1_reception_should_be_not_ok_if_the_new_sum_is_negative()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
              .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = false,
                        QuantiteReceptionnee = 0,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = 5
                            },
                             new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 2,
                                 Quantity = -5
                            }
                        }
                   }
               });

            DepenseAchatEnt receptionsToDelete = new DepenseAchatEnt()
            {
                DepenseId = 1,
                CommandeLigneId = 1,
                Quantite = 4

            };

            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForDeletion(receptionsToDelete));

            //ASSERT
            validationResult.IsValid.Should().BeFalse();

            var firstError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_1").FirstOrDefault();
            firstError.Should().NotBeNull();

            firstError.ErrorMessage.Should().Be(BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Positive);
        }


        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // 5 					    | 0							    	  | -								  | -1								  | ko	     | La quantité doit être supérieure ou égale à 0
        [TestMethod]
        public void Reception_Positive_Add_1_reception_where_the_sum_is_negative_on_command_line_positive_should_be_not_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
              .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = false,
                        QuantiteReceptionnee = 3,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = 3
                            }
                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 0,
                    CommandeLigneId = 1,
                    Quantite = -10
                }
            };

            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeFalse();

            var firstError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_0").FirstOrDefault();
            firstError.Should().NotBeNull();
            var errorMessage = string.Format(BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Supp_Or_Egual, 3);

            firstError.ErrorMessage.Should().Be(errorMessage);

        }

        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // + 					    | 3							    	  | 3								  | -4								  | ko	     | La quantité doit être supérieure ou égale à -3
        [TestMethod]
        public void Reception_Positive_Update_1_reception_where_the_sum_is_negative_on_command_line_positive_should_be_not_ok_2()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
              .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = false,
                        QuantiteReceptionnee = 3,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = 3
                            }
                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 1,
                    CommandeLigneId = 1,
                    Quantite = -4
                }
            };

            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeFalse();

            var firstError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_1").FirstOrDefault();

            firstError.Should().NotBeNull();

            var errorMessage = string.Format(BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Supp_Or_Egual, 0);

            firstError.ErrorMessage.Should().Be(errorMessage);

        }

        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // + 					    | 3							    	  | 3,0,0							  | 3,-2,-2							  | ko	     | La quantité doit être supérieure ou égale à -3
        [TestMethod]
        public void Reception_Positive_Update_3_receptions_where_the_sum_is_negative_on_command_line_positive_should_be_not_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
              .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = false,
                        QuantiteReceptionnee = 3,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = 3
                            },
                              new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 2,
                                 Quantity = 0
                            },
                                new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 3,
                                 Quantity = 0
                            }
                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 1,
                    CommandeLigneId = 1,
                    Quantite = 3
                },
                new DepenseAchatEnt()
                {
                    DepenseId = 2,
                    CommandeLigneId = 1,
                    Quantite = -2
                },
                new DepenseAchatEnt()
                {
                    DepenseId = 3,
                    CommandeLigneId = 1,
                    Quantite = -2
                }
            };

            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeFalse();

            var firstError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_1").FirstOrDefault();
            firstError.Should().NotBeNull();

            var secondError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_2").FirstOrDefault();
            secondError.Should().NotBeNull();

            var lastError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_3").FirstOrDefault();
            lastError.Should().NotBeNull();

        }

        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // + 					    | 3							    	  | 3,0,0							  | =old,-2,-2			        	  | ko	     | La quantité doit être supérieure ou égale à -3
        [TestMethod]
        public void Reception_Positive_Update_2_receptions_where_the_sum_is_negative_on_command_line_positive_should_be_not_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
              .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = false,
                        QuantiteReceptionnee = 3,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = 3
                            },
                              new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 2,
                                 Quantity = 0
                            },
                                new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 3,
                                 Quantity = 0
                            }
                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 2,
                    CommandeLigneId = 1,
                    Quantite = -2
                },
                new DepenseAchatEnt()
                {
                    DepenseId = 3,
                    CommandeLigneId = 1,
                    Quantite = -3
                }
            };

            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeFalse();

            var firstError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_2").FirstOrDefault();

            firstError.Should().NotBeNull();

            var errorMessage = string.Format(ErrorQuantitySuppOrEgual, 3);

            firstError.ErrorMessage.Should().Be(errorMessage);

            var secondError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_3").FirstOrDefault();

            secondError.Should().NotBeNull();

            var secondErrorMessage = string.Format(ErrorQuantitySuppOrEgual, 3);

            secondError.ErrorMessage.Should().Be(secondErrorMessage);


        }

        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // - 					    | 0							    	  | -								  | -3								  | ok	     |  -
        [TestMethod]
        public void Reception_Negative_Add_1_new_receptions_where_the_sum_is_negative_on_command_line_negative_should_be_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
               .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = true,
                        QuantiteReceptionnee = 0,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {

                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 0,
                    CommandeLigneId = 1,
                    Quantite = -3
                }
            };
            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeTrue();

        }

        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // - 					    | -2						    	  | -2								  | 3								  | ko	     |  -
        [TestMethod]
        public void Reception_Negative_Add_1_new_receptions_where_the_sum_is_positive_on_command_line_negative_should_be_not_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
               .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = true,
                        QuantiteReceptionnee = -2,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                             new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = -2
                            },
                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 0,
                    CommandeLigneId = 1,
                    Quantite = 3
                }
            };
            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT

            validationResult.IsValid.Should().BeFalse();

            var firstError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_0").FirstOrDefault();

            firstError.Should().NotBeNull();

            var errorMessage = string.Format(BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Inf_Or_Egual, 2);

            firstError.ErrorMessage.Should().Be(errorMessage);

        }


        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // - 					    | 0							    	  | -								  | -3,-1							  | ok	     |  -
        [TestMethod]
        public void Reception_Negative_Add_2_news_receptions_where_the_sum_is_negative_on_command_line_negative_should_be_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
               .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = true,
                        QuantiteReceptionnee = 0,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {

                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 0,
                    CommandeLigneId = 1,
                    Quantite = -3
                },
                 new DepenseAchatEnt()
                {
                    DepenseId = 0,
                    CommandeLigneId = 1,
                    Quantite = -1
                }
            };
            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeTrue();

        }


        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // - 					    | -3							      | -3,-,-							  | =old-3,-1,+6			          | ko	     | La quantité doit être supérieure ou égale à 3
        [TestMethod]
        public void Reception_Negative_Add_2_news_receptions_where_the_sum_is_positive_on_command_line_negative_should_be_not_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
              .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = true,
                        QuantiteReceptionnee = -3,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = -3
                            },

                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 0,
                    CommandeLigneId = 1,
                    Quantite = -1
                },
                new DepenseAchatEnt()
                {
                    DepenseId = 0,
                    CommandeLigneId = 1,
                    Quantite = +6
                }
            };

            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeFalse();

            var errors = validationResult.Errors.Where(p => p.PropertyName == "Quantite_0").ToList();

            var errorMessage1 = string.Format(ErrorQuantityInfOrEgual, 3);

            errors.Where(x => x.ErrorMessage == errorMessage1).Should().HaveCount(2);


        }

        // Quantite commande ligne  | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // - 					    | -9							      | -3,-3,-3					      | =old-3,+3,+1			          | ko	     | La quantité doit être inférieure ou égale à 3
        [TestMethod]
        public void Reception_Negative_update_2_receptions_where_the_sum_is_positive_on_command_line_negative_should_be_not_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
              .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = true,
                        QuantiteReceptionnee = -9,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = -3
                            },
                             new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 2,
                                 Quantity = -3
                            },
                              new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 3,
                                 Quantity = -3
                            },
                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 2,
                    CommandeLigneId = 1,
                    Quantite = +3
                },
                new DepenseAchatEnt()
                {
                    DepenseId = 3,
                    CommandeLigneId = 1,
                    Quantite = +1
                }
            };

            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeFalse();
            var firstError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_2").FirstOrDefault();
            firstError.Should().NotBeNull();

            var secondError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_3").FirstOrDefault();
            secondError.Should().NotBeNull();

        }

        // Quantite commande ligne  | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // - 					    | -9							      | -3,-3,-3					      | =old-3,+100,-3			          | ko	     | 
        [TestMethod]
        public void Reception_Negative_update_1_reception_where_the_sum_is_positive_on_command_line_negative_should_be_not_ok()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
              .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = true,
                        QuantiteReceptionnee = -9,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = -3
                            },
                             new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 2,
                                 Quantity = -3
                            },
                              new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 3,
                                 Quantity = -3
                            },
                        }
                   }
               });

            List<DepenseAchatEnt> receptionsToAddOrUpdates = new List<DepenseAchatEnt>()
            {
                new DepenseAchatEnt()
                {
                    DepenseId = 2,
                    CommandeLigneId = 1,
                    Quantite = +100
                }
            };

            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForAddOrUpdates(receptionsToAddOrUpdates));

            //ASSERT
            validationResult.IsValid.Should().BeFalse();

            var firstError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_2").FirstOrDefault();

            firstError.Should().NotBeNull();

            var errorMessage = string.Format(BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Inf_Or_Egual, 6);

            firstError.ErrorMessage.Should().Be(errorMessage);

        }


        // commande ligne + ou -    | Cumul des receptions receptionnes   | ancienne quantite de la reception | nouvelle quantite de la reception | resultat | Message 
        // - 					    | 0							    	  | 5,-5							  | 4,5 							  | ko	     |  -
        [TestMethod]
        public void Reception_Negative_remove_1_reception_should_be_not_ok_if_the_new_sum_is_positive()
        {
            // Arrange
            var validator = this.CreateReceptionNegativeRulesValidator();

            this.mockCommandeLignesRepository
               .Setup(cdr => cdr.GetCommandeLignesWithReceptionsQuantities(It.IsAny<List<int>>()))
              .Returns(new List<CommandeLigneQuantiteNegativeModel>()
               {
                   new CommandeLigneQuantiteNegativeModel()
                   {
                        CommandeLigneId = 1,
                        LigneDeCommandeNegative = true,
                        QuantiteReceptionnee = 0,
                        ReceptionQuantites = new List<ReceptionQuantiteModel>()
                        {
                            new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 1,
                                 Quantity = 5
                            },
                             new ReceptionQuantiteModel()
                            {
                                 ReceptionId = 2,
                                 Quantity = -5
                            }
                        }
                   }
               });
            DepenseAchatEnt receptionsToDelete = new DepenseAchatEnt()
            {
                DepenseId = 2,
                CommandeLigneId = 1,
                Quantite = 5

            };


            // Act

            ValidationResult validationResult = validator.Validate(ReceptionsValidationModel.CreateForDeletion(receptionsToDelete));

            //ASSERT
            validationResult.IsValid.Should().BeFalse();

            var firstError = validationResult.Errors.Where(p => p.PropertyName == "Quantite_2").FirstOrDefault();

            firstError.Should().NotBeNull();

            firstError.ErrorMessage.Should().Be(BusinessResources.Reception_Quantity_Rules_Validator_GFTP_Error_Quantity_Negative);

        }
    }
}
