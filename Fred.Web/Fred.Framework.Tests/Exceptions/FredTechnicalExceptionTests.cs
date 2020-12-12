using System;
using FluentAssertions;
using Fred.Framework.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Framework.Tests.Exceptions
{
    [TestClass]
    public class FredTechnicalExceptionTests
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
        ///   Test FredTechnicalException : With Message
        /// </summary>
        [TestMethod]
        public void TestFredTechnicalExceptionWithMessage()
        {
            // Arrange
            ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

            // Act
            Action act = () => test.ThrowFredTechnicalExceptionWithMessage();

            // Assert
            act.Should().Throw<FredException>()
               .WithMessage("Message");
        }

        /// <summary>
        ///   Test FredTechnicalException : With Inner
        /// </summary>
        [TestMethod]
        public void TestFredTechnicalExceptionWithInner()
        {
            // Arrange
            ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

            // Act
            Action act = () => test.ThrowFredTechnicalExceptionWithInner();

            // Assert
            act.Should().Throw<FredException>()
               .WithMessage("Message")
               .WithInnerException<NullReferenceException>();
        }

        /// <summary>
        ///   Test FredTechnicalException : With Object
        /// </summary>
        [TestMethod]
        public void TestFredTechnicalExceptionWithObject()
        {
            // Arrange
            ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

            // Act
            Action act = () => test.ThrowFredTechnicalExceptionWithObject();

            // Assert
            act.Should().Throw<FredException>()
               .WithMessage("Message")
               .Which.ObjectToDump.Should()
               .NotBeNull();
        }

        /// <summary>
        ///   Test FredTechnicalException : With Object and Inner
        /// </summary>
        [TestMethod]
        public void TestFredTechnicalExceptionWithObjectAndInner()
        {
            // Arrange
            ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

            // Act
            Action act = () => test.ThrowFredTechnicalExceptionWithObjectAndInner();

            // Assert
            act.Should().Throw<FredException>();
            act.Should().Throw<FredException>().WithMessage("Message");
            act.Should().Throw<FredException>().Which.ObjectToDump.Should();
            act.Should().Throw<FredException>().NotBeNull();
        }

        /// <summary>
        ///   Test FredTechnicalException : With all possible parameters
        /// </summary>
        [TestMethod]
        public void TestFredTechnicalExceptionWithEverythink()
        {
            // Arrange
            ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

            // Act
            Action act = () => test.ThrowFredTechnicalExceptionWithEverythink();

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
            public void ThrowFredTechnicalExceptionWithMessage()
            {
                throw new FredTechnicalException("Message");
            }

            public void ThrowFredTechnicalExceptionWithInner()
            {
                throw new FredTechnicalException("Message", new NullReferenceException());
            }

            public void ThrowFredTechnicalExceptionWithObject()
            {
                throw new FredTechnicalException("Message", "anObject");
            }

            public void ThrowFredTechnicalExceptionWithObjectAndInner()
            {
                throw new FredTechnicalException("Message", "anObject", new NullReferenceException());
            }

            public void ThrowFredTechnicalExceptionWithEverythink()
            {
                throw new FredTechnicalException("Message", "anObject", new NullReferenceException())
                { Service = "Service", UserLogin = "UserLogin" };
            }
        }
    }
}
