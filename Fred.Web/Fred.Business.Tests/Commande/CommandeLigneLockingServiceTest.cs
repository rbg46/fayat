using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Fred.Business.Commande;
using Fred.Business.Commande.Validators;
using Fred.Business.OrganisationFeature;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Commande.Builder;
using Fred.Common.Tests.Data.Depense.Builder;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Commande
{
    [TestClass]
    public class CommandeLigneLockingServiceTest : BaseTu<CommandeLigneLockingService>
    {
        private const int ContextUtilisateurId = 1;
        private CommandeLigneBuilder CommandeLigneBuilder = null;
        private DepenseAchatBuilder DepenseAchatBuilder = null;
        Mock<ICommandeLignesRepository> commandeLigneRepositoryFake;

        [TestInitialize]
        public void TestInitialize()
        {
            CommandeLigneBuilder = new CommandeLigneBuilder();
            DepenseAchatBuilder = new DepenseAchatBuilder();

            var organisationRelatedFeatureServiceFake = GetMocked<IOrganisationRelatedFeatureService>();
            organisationRelatedFeatureServiceFake.Setup(s => s.IsEnabledForCurrentUser(It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

            commandeLigneRepositoryFake = GetMocked<ICommandeLignesRepository>();
            commandeLigneRepositoryFake.Setup(m => m.LockCommandeLigne(It.IsAny<int>(), It.IsAny<int>())).Verifiable();
            commandeLigneRepositoryFake.Setup(m => m.UnlockCommandeLigne(It.IsAny<int>(), It.IsAny<int>())).Verifiable();

            var utilisateurMgrFake = GetMocked<IUtilisateurManager>();
            utilisateurMgrFake.Setup(m => m.GetContextUtilisateurId())
                .Returns(ContextUtilisateurId);

            SubstituteConstructorArgument<ICommandeLigneLockValidator>(new CommandeLigneLockValidator());
            SubstituteConstructorArgument<ICommandeLigneUnlockValidator>(new CommandeLigneUnlockValidator());
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void LockAsync_WithAlreadyLocked_ThrowException()
        {
            commandeLigneRepositoryFake.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(CommandeLigneBuilder.IsVerrou(true).Build());
            Invoking(() => Actual.LockAsync(0)).Should().Throw<FluentValidation.ValidationException>();
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void LockAsync_WithUnlocked_CallLockRepository()
        {
            commandeLigneRepositoryFake.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(CommandeLigneBuilder.IsVerrou(false).Build());
            Invoking(() => Actual.LockAsync(0)).Invoke();

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void UnlockAsync_WithAlreadyUnLocked_ThrowException()
        {
            commandeLigneRepositoryFake.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(CommandeLigneBuilder.IsVerrou(false).Build());
            Invoking(() => Actual.UnlockAsync(0)).Should().Throw<FluentValidation.ValidationException>();
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void UnlockAsync_WithLocked_CallUnlockRepository()
        {
            commandeLigneRepositoryFake.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(CommandeLigneBuilder.IsVerrou(true).Build());
            Invoking(() => Actual.UnlockAsync(0)).Invoke();

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.UnlockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnAdd_WithEnoughQuantite_CallLockRepository()
        {
            decimal quantiteAReceptionner = 100;
            decimal quantiteReceptionnee = 90;

            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLigneWithReceptionQuantiteById(It.IsAny<int>()))
                .Returns(new CommandeLigneWithReceptionQuantiteModel
                {
                    Quantite = quantiteAReceptionner,
                    Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                    {
                        new Entities.Commande.ReceptionQuantiteModel
                        {
                            Quantite = quantiteReceptionnee
                        }
                    }
                });


            var result = DepenseAchatBuilder.CommandeLigneId(0).Quantite(quantiteAReceptionner).Build();
            Invoking(
                () => Actual.AutomaticLockIfNeededOnAdd(result))
            .Invoke();

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnUpdate_WithEnoughQuantite_CallLockRepository()
        {
            decimal quantiteAReceptionner = 100;
            decimal quantiteReceptionnee = 90;
            int modidiedReceptionId = 99;
            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLignesWithReceptionQuantiteByIds(It.IsAny<List<int>>()))
                .Returns(
                new List<CommandeLigneWithReceptionQuantiteModel>
                {
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = quantiteAReceptionner,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {
                            new Entities.Commande.ReceptionQuantiteModel
                            {
                                ReceptionId = modidiedReceptionId,
                                Quantite = quantiteReceptionnee
                            }
                        }
                    }
                });

            var result = DepenseAchatBuilder.DepenseId(modidiedReceptionId).CommandeLigneId(0).Quantite(quantiteAReceptionner).Build();
            Invoking(
                () => Actual.AutomaticLockIfNeededOnUpdate(new List<DepenseAchatEnt> { result }))
            .Invoke();

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void CanAddOrUpdateReceptionsOnCommandeLignes_WithOneLigneLocked_ReturnsFalse()
        {
            commandeLigneRepositoryFake
                .Setup(
                    r =>
                    r.Get(
                        It.IsAny<List<Expression<Func<CommandeLigneEnt, bool>>>>(),
                        It.IsAny<Func<IQueryable<CommandeLigneEnt>, IOrderedQueryable<CommandeLigneEnt>>>(),
                        It.IsAny<List<Expression<Func<CommandeLigneEnt, object>>>>(),
                        It.IsAny<int?>(),
                        It.IsAny<int?>(),
                        It.IsAny<bool>()))
                .Returns(new List<CommandeLigneEnt>
                {
                    CommandeLigneBuilder.IsVerrou(true).Build(),
                    CommandeLigneBuilder.IsVerrou(false).Build()
                });

            Invoking(() => Actual.CanAddOrUpdateReceptionsOnCommandeLignes(new List<int>()))
            .Should().Equals(false);
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void CanAddOrUpdateReceptionsOnCommandeLignes_WithAllLignesUnlocked_ReturnsTrue()
        {
            commandeLigneRepositoryFake
                .Setup(
                    r =>
                    r.Get(
                        It.IsAny<List<Expression<Func<CommandeLigneEnt, bool>>>>(),
                        It.IsAny<Func<IQueryable<CommandeLigneEnt>, IOrderedQueryable<CommandeLigneEnt>>>(),
                        It.IsAny<List<Expression<Func<CommandeLigneEnt, object>>>>(),
                        It.IsAny<int?>(),
                        It.IsAny<int?>(),
                        It.IsAny<bool>()))
                .Returns(new List<CommandeLigneEnt>
                {
                    CommandeLigneBuilder.IsVerrou(false).Build(),
                    CommandeLigneBuilder.IsVerrou(false).Build()
                });

            Invoking(() => Actual.CanAddOrUpdateReceptionsOnCommandeLignes(new List<int>()))
            .Should().Equals(true);
        }




        private ReceptionQuantiteModel CreateReceptionQuantiteModel(int receptionId, decimal quantite)
        {
            return new Entities.Commande.ReceptionQuantiteModel
            {
                ReceptionId = receptionId,
                Quantite = quantite
            };
        }

        private DepenseAchatEnt CreateReception(int commandLigneId, int receptionId, decimal quantite)
        {
            return new DepenseAchatEnt()
            {
                DepenseId = receptionId,
                CommandeLigneId = commandLigneId,
                Quantite = quantite
            };
        }


        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnUpdate_test_case_0()
        {
            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLignesWithReceptionQuantiteByIds(It.IsAny<List<int>>()))
                .Returns(
                new List<CommandeLigneWithReceptionQuantiteModel>
                {
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = 10,
                        CommandeLigneId = 1,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {
                            CreateReceptionQuantiteModel(receptionId:1,quantite:10),
                            CreateReceptionQuantiteModel(receptionId:2,quantite:10),
                        }
                    }
                });


            var receptionUpdateds = new List<DepenseAchatEnt>()
            {
                 CreateReception(commandLigneId: 1, receptionId: 2, quantite: -10)

            };
            //ACT 
            Actual.AutomaticLockIfNeededOnUpdate(receptionUpdateds);

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Never);
        }


        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnUpdate_test_case_1()
        {
            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLignesWithReceptionQuantiteByIds(It.IsAny<List<int>>()))
                .Returns(
                new List<CommandeLigneWithReceptionQuantiteModel>
                {
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = 10,
                        CommandeLigneId = 1,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {
                            CreateReceptionQuantiteModel(receptionId:1,quantite:-10),
                            CreateReceptionQuantiteModel(receptionId:2,quantite:-10),

                        }
                    }
                });


            var receptionUpdateds = new List<DepenseAchatEnt>()
            {
                  CreateReception(commandLigneId: 1, receptionId: 2, quantite: -20)

            };
            //ACT 
            Actual.AutomaticLockIfNeededOnUpdate(receptionUpdateds);

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
        }


        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnUpdate_test_case_2()
        {

            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLignesWithReceptionQuantiteByIds(It.IsAny<List<int>>()))
                .Returns(
                new List<CommandeLigneWithReceptionQuantiteModel>
                {
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = 10,
                        CommandeLigneId = 1,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {
                            CreateReceptionQuantiteModel(receptionId:1,quantite:-10),
                            CreateReceptionQuantiteModel(receptionId:2,quantite:-10),

                        }
                    }
                });


            var receptionUpdateds = new List<DepenseAchatEnt>()
            {
                CreateReception(commandLigneId: 1, receptionId: 2, quantite: 5)

            };
            //ACT 
            Actual.AutomaticLockIfNeededOnUpdate(receptionUpdateds);

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Never);
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnAdd_test_case_0()
        {

            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLigneWithReceptionQuantiteById(It.IsAny<int>()))
                .Returns(
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = 10,
                        CommandeLigneId = 1,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {

                        }
                    }
                );
            var newReception = CreateReception(commandLigneId: 1, receptionId: 0, quantite: 30);

            //ACT 
            Actual.AutomaticLockIfNeededOnAdd(newReception);

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
        }


        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnAdd_test_case_1()
        {

            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLigneWithReceptionQuantiteById(It.IsAny<int>()))
                .Returns(
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = 10,
                        CommandeLigneId = 1,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {

                        }
                    }
                );


            var newReception = CreateReception(commandLigneId: 1, receptionId: 0, quantite: 9);

            //ACT 
            Actual.AutomaticLockIfNeededOnAdd(newReception);

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Never);
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnAdd_test_case_2()
        {

            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLigneWithReceptionQuantiteById(It.IsAny<int>()))
                .Returns(
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = 10,
                        CommandeLigneId = 1,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {

                        }
                    }
                );

            var newReception = CreateReception(commandLigneId: 1, receptionId: 0, quantite: -10);

            //ACT 
            Actual.AutomaticLockIfNeededOnAdd(newReception);

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnAdd_test_case_3()
        {

            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLigneWithReceptionQuantiteById(It.IsAny<int>()))
                .Returns(
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = 10,
                        CommandeLigneId = 1,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {
                             CreateReceptionQuantiteModel(receptionId:1,quantite:-10),

                        }
                    }
                );

            var newReception = CreateReception(commandLigneId: 1, receptionId: 0, quantite: 10);


            //ACT 
            Actual.AutomaticLockIfNeededOnAdd(newReception);

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Never);
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnAdd_test_case_4()
        {

            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLigneWithReceptionQuantiteById(It.IsAny<int>()))
                .Returns(
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = 10,
                        CommandeLigneId = 1,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {
                             CreateReceptionQuantiteModel(receptionId:1,quantite:-10),
                             CreateReceptionQuantiteModel(receptionId:2,quantite:19)
                        }
                    }
                );

            var newReception = CreateReception(commandLigneId: 1, receptionId: 0, quantite: 2);

            //ACT 
            Actual.AutomaticLockIfNeededOnAdd(newReception);

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
        }


        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnAdd_test_case_5()
        {

            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLigneWithReceptionQuantiteById(It.IsAny<int>()))
                .Returns(
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = 10,
                        CommandeLigneId = 1,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {
                             CreateReceptionQuantiteModel(receptionId:1,quantite:-10),
                             CreateReceptionQuantiteModel(receptionId:2,quantite:-10)
                        }
                    }
                );

            var newReception = CreateReception(commandLigneId: 1, receptionId: 0, quantite: 30);


            //ACT 
            Actual.AutomaticLockIfNeededOnAdd(newReception);

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
        }


        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnDelete_test_case_1()
        {

            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLigneWithReceptionQuantiteById(It.IsAny<int>()))
                .Returns(
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = 10,
                        CommandeLigneId = 1,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {
                             CreateReceptionQuantiteModel(receptionId:1,quantite:-10),
                             CreateReceptionQuantiteModel(receptionId:2,quantite:19)
                        }
                    }
                );

            var deletedReception = CreateReception(commandLigneId: 1, receptionId: 2, quantite: 19);

            //ACT 
            Actual.AutomaticLockIfNeededOnDelete(deletedReception);

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
        }

        [TestMethod]
        [TestCategory("CommandeLigneLocking")]
        public void AutomaticLockIfNeededOnDelete_test_case_2()
        {

            commandeLigneRepositoryFake
                .Setup(r => r.GetCommandeLigneWithReceptionQuantiteById(It.IsAny<int>()))
                .Returns(
                    new CommandeLigneWithReceptionQuantiteModel
                    {
                        Quantite = 10,
                        CommandeLigneId = 1,
                        Receptions = new List<Entities.Commande.ReceptionQuantiteModel>
                        {
                             CreateReceptionQuantiteModel(receptionId:1,quantite:-10),
                             CreateReceptionQuantiteModel(receptionId:2,quantite:10),
                             CreateReceptionQuantiteModel(receptionId:3,quantite:-10)

                        }
                    }
                );


            var deletedReception = CreateReception(commandLigneId: 1, receptionId: 2, quantite: 10);

            Actual.AutomaticLockIfNeededOnDelete(deletedReception);

            commandeLigneRepositoryFake.Verify(
                commandeLigneRepositoryFake => commandeLigneRepositoryFake.LockCommandeLigne(
                    It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
        }

    }
}
