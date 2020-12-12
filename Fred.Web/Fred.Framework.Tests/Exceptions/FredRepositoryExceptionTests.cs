using System;
using FluentAssertions;
using Fred.Framework.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Framework.Tests.Exceptions
{
    [TestClass]
    public class FredRepositoryExceptionTests
    {
        /// <summary>
        ///   Initialise l'ensemble des tests de la classe.
        /// </summary>
        /// <param name="context">Le contexte de tests.</param>
        [ClassInitialize]
        public static void InitAllTests(TestContext context)
        {
        }

        /// <summary>
        ///   Initialise un test, cette méthode s'exécute avant chaque test
        ///   ///
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
        }


        /// <summary>
        ///   Test FredRepositoryException : With Message
        /// </summary>
        [TestMethod]
        public void TestFredRepositoryExceptionWithMessage()
        {
            // Arrange
            ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

            // Act
            Action act = () => test.ThrowFredRepositoryExceptionWithMessage();

            // Assert
            act.Should().Throw<FredException>()
               .WithMessage("Message");
        }

        /// <summary>
        ///   Test FredRepositoryException : With Inner
        /// </summary>
        [TestMethod]
        public void TestFredRepositoryExceptionWithInner()
        {
            // Arrange
            ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

            // Act
            Action act = () => test.ThrowFredRepositoryExceptionWithInner();

            // Assert
            act.Should().Throw<FredException>()
               .WithMessage("Message")
               .WithInnerException<NullReferenceException>();
        }

        /// <summary>
        ///   Test FredRepositoryException : With Object
        /// </summary>
        [TestMethod]
        public void TestFredRepositoryExceptionWithObject()
        {
            // Arrange
            ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

            // Act
            Action act = () => test.ThrowFredRepositoryExceptionWithObject();

            // Assert
            act.Should().Throw<FredException>()
               .WithMessage("Message")
               .Which.ObjectToDump.Should()
               .NotBeNull();
        }

        /// <summary>
        ///   Test FredRepositoryException : With Object and Inner
        /// </summary>
        [TestMethod]
        public void TestFredRepositoryExceptionWithObjectAndInner()
        {
            // Arrange
            ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

            // Act
            Action act = () => test.ThrowFredRepositoryExceptionWithObjectAndInner();

            // Assert
            act.Should().Throw<FredException>();
            act.Should().Throw<FredException>().WithMessage("Message");
            act.Should().Throw<FredException>().Which.ObjectToDump.Should();
            act.Should().Throw<FredException>().NotBeNull();
        }

        /// <summary>
        ///   Test FredRepositoryException : With all possible parameters
        /// </summary>
        [TestMethod]
        public void TestFredRepositoryExceptionWithEverythink()
        {
            // Arrange
            ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

            // Act
            Action act = () => test.ThrowFredRepositoryExceptionWithEverythink();

            // Assert
            act.Should().Throw<FredException>().WithMessage("Message").WithInnerException<NullReferenceException>();
            act.Should().Throw<FredException>().Which.ObjectToDump.Should().NotBeNull();
            act.Should().Throw<FredException>().Which.Service.Should().Be("Service");
            act.Should().Throw<FredException>().Which.UserLogin.Should().Be("UserLogin");
        }


        /// <summary>
        ///   Object qui lance des Exceptions de type FredException
        /// </summary>
        private class ObjectThatLaunchFredException
        {
            public void ThrowFredRepositoryExceptionWithMessage()
            {
                throw new FredRepositoryException("Message");
            }

            public void ThrowFredRepositoryExceptionWithInner()
            {
                throw new FredRepositoryException("Message", new NullReferenceException());
            }

            public void ThrowFredRepositoryExceptionWithObject()
            {
                throw new FredRepositoryException("Message", "anObject");
            }

            public void ThrowFredRepositoryExceptionWithObjectAndInner()
            {
                throw new FredRepositoryException("Message", "anObject", new NullReferenceException());
            }

            public void ThrowFredRepositoryExceptionWithEverythink()
            {
                throw new FredRepositoryException("Message", "anObject", new NullReferenceException())
                { Service = "Service", UserLogin = "UserLogin" };
            }
        }
    }
}
