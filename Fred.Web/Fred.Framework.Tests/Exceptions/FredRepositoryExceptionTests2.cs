using System;
using FluentAssertions;
using Fred.Framework.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Framework.Tests.Exceptions
{
  [TestClass]
  public class FredBusinessExceptionTests
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
    ///   Test FredBusinessException : With Message
    /// </summary>
    [TestMethod]
    public void TestFredBusinessExceptionWithMessage()
    {
      // Arrange
      ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

      // Act
      Action act = () => test.ThrowFredBusinessExceptionWithMessage();

      // Assert
      act.ShouldThrow<FredException>()
         .WithMessage("Message");
    }

    /// <summary>
    ///   Test FredBusinessException : With Inner
    /// </summary>
    [TestMethod]
    public void TestFredBusinessExceptionWithInner()
    {
      // Arrange
      ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

      // Act
      Action act = () => test.ThrowFredBusinessExceptionWithInner();

      // Assert
      act.ShouldThrow<FredException>()
         .WithMessage("Message")
         .WithInnerException<NullReferenceException>();
    }

    /// <summary>
    ///   Test FredBusinessException : With Object
    /// </summary>
    [TestMethod]
    public void TestFredBusinessExceptionWithObject()
    {
      // Arrange
      ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

      // Act
      Action act = () => test.ThrowFredBusinessExceptionWithObject();

      // Assert
      act.ShouldThrow<FredException>()
         .WithMessage("Message")
         .Which.ObjectToDump.Should()
         .NotBeNull();
    }

    /// <summary>
    ///   Test FredBusinessException : With Object and Inner
    /// </summary>
    [TestMethod]
    public void TestFredBusinessExceptionWithObjectAndInner()
    {
      // Arrange
      ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

      // Act
      Action act = () => test.ThrowFredBusinessExceptionWithObjectAndInner();

      // Assert
      act.ShouldThrow<FredException>()
         .WithMessage("Message")
         .WithInnerException<NullReferenceException>()
         .Which.ObjectToDump.Should()
         .NotBeNull();
    }

    /// <summary>
    ///   Test FredBusinessException : With all possible parameters
    /// </summary>
    [TestMethod]
    public void TestFredBusinessExceptionWithEverythink()
    {
      // Arrange
      ObjectThatLaunchFredException test = new ObjectThatLaunchFredException();

      // Act
      Action act = () => test.ThrowFredBusinessExceptionWithEverythink();

      // Assert
      act.ShouldThrow<FredException>().WithMessage("Message").WithInnerException<NullReferenceException>();
      act.ShouldThrow<FredException>().Which.ObjectToDump.Should().NotBeNull();
      act.ShouldThrow<FredException>().Which.Service.Should().Be("Service");
      act.ShouldThrow<FredException>().Which.UserLogin.Should().Be("UserLogin");
    }


    /// <summary>
    ///   Object qui lance des Exceptions de type FredException
    /// </summary>
    private class ObjectThatLaunchFredException
    {
      public void ThrowFredBusinessExceptionWithMessage()
      {
        throw new FredBusinessException("Message");
      }

      public void ThrowFredBusinessExceptionWithInner()
      {
        throw new FredBusinessException("Message", new NullReferenceException());
      }

      public void ThrowFredBusinessExceptionWithObject()
      {
        throw new FredBusinessException("Message", "anObject");
      }

      public void ThrowFredBusinessExceptionWithObjectAndInner()
      {
        throw new FredBusinessException("Message", "anObject", new NullReferenceException());
      }

      public void ThrowFredBusinessExceptionWithEverythink()
      {
        throw new FredBusinessException("Message", "anObject", new NullReferenceException())
          { Service = "Service", UserLogin = "UserLogin" };
      }
    }
  }
}