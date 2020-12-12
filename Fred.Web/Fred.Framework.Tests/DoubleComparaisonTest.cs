using System;
using FluentAssertions;
using Fred.Framework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Framework.Tests
{
  [TestClass]
  public class DoubleComparaisonTest
  {

    #region avec double
    /// <summary>
    /// Permet de tester la méthode IsZero de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleIsZero()
    {
      double double1 = 0.000000001d;
      double1.IsZero().Should().BeTrue();
    }


    /// <summary>
    /// Permet de tester la méthode IsZero de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleIsZeroInferiorToDefaultPrecision()
    {
      double double1 = 2 * DoubleExtensions.DefaultPrecision;
      double1.IsZero().Should().BeFalse();
    }




    /// <summary>
    /// Permet de tester la méthode IsEqual de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleIsEqual()
    {
      double double1 = 0.3333333d;
      double double2 = 1/3d;
      double1.IsEqual(double2).Should().BeTrue();
    }

    /// <summary>
    /// Permet de tester la méthode IsEqual de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleIsEqualInferiorToDefaultPrecision()
    {
      double double1 = 0.333d;
      double double2 = 1 / 3d;
      double1.IsEqual(double2).Should().BeFalse();
    }

    /// <summary>
    /// Permet de tester la méthode IsNotZero de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleIsNotZero()
    {
      double double1 = 0.000000001d;
      double1.IsNotZero().Should().BeFalse();
    }


    /// <summary>
    /// Permet de tester la méthode IsNotZero de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleIsNotZeroInferiorToDefaultPrecision()
    {
      double double1 = 2 * DoubleExtensions.DefaultPrecision;
      double1.IsNotZero().Should().BeTrue();
    }




    /// <summary>
    /// Permet de tester la méthode IsNotEqual de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleIsNotEqual()
    {
      double double1 = 0.3333333d;
      double double2 = 1 / 3d;

      double1.IsNotEqual(double2).Should().BeFalse();
    }

    /// <summary>
    /// Permet de tester la méthode IsNotEqual de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleIsNotEqualInferiorToDefaultPrecision()
    {
      double double1 = 0.333d;
      double double2 = 1 / 3d;

      double1.IsNotEqual(double2).Should().BeTrue();
    }


    #endregion



    #region avec double ?
    /// <summary>
    /// Permet de tester la méthode IsZero de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleNullableIsZero()
    {
      double? double1 = 0.000000001d;
      double1.IsZero().Should().BeTrue();

      double? double2 = default(double);
      double2.IsZero().Should().BeTrue();
    }


    /// <summary>
    /// Permet de tester la méthode IsZero de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleNullableIsZeroInferiorToDefaultPrecision()
    {
      double? double1 = 2 * DoubleExtensions.DefaultPrecision;
      double1.IsZero().Should().BeFalse();
    }




    /// <summary>
    /// Permet de tester la méthode IsEqual de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleNullableIsEqual()
    {
      double? double1 = 0.3333333d;
      double? double2 = 1/3d;
      double1.IsEqual(double2).Should().BeTrue();

      double1 = default(double);
      double2 = 0.3333333d;
      double1.IsEqual(double2).Should().BeFalse();

      double1 = 0.3333333d;
      double2 = default(double);
      double1.IsEqual(double2).Should().BeFalse();
    }

    /// <summary>
    /// Permet de tester la méthode IsEqual de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleNullableIsEqualInferiorToDefaultPrecision()
    {
      double? double1 = 0.333d;
      double? double2 = 1 / 3d;
      double1.IsEqual(double2).Should().BeFalse();

      double1 = default(double);
      double2 = 0.333d;
      double1.IsEqual(double2).Should().BeFalse();

      double1 = 0.333d;
      double2 = default(double);
      double1.IsEqual(double2).Should().BeFalse();
    }

    /// <summary>
    /// Permet de tester la méthode IsNotZero de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleNullableIsNotZero()
    {
      double? double1 = 0.000000001d;
      double1.IsNotZero().Should().BeFalse();

      double1 = 0.001d;
      double1.IsNotZero().Should().BeTrue();

      double1 = default(double);
      double1.IsNotZero().Should().BeFalse();
    }


    /// <summary>
    /// Permet de tester la méthode IsNotZero de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleNullableIsNotZeroInferiorToDefaultPrecision()
    {
      double? double1 = 2 * DoubleExtensions.DefaultPrecision;
      double1.IsNotZero().Should().BeTrue();
    }




    /// <summary>
    /// Permet de tester la méthode IsNotEqual de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleNullableIsNotEqual()
    {
      double? double1 = 0.3333333d;
      double? double2 = 1 / 3d;
      double1.IsNotEqual(double2).Should().BeFalse();

      double1 = default(double);
      double2 = default(double);
      double1.IsNotEqual(double2).Should().BeFalse();

      double1 = 0.3333333d;
      double2 = default(double);
      double1.IsNotEqual(double2).Should().BeTrue();

      double1 = default(double);
      double2 = 0.3333333d;
      double1.IsNotEqual(double2).Should().BeTrue();
    }

    /// <summary>
    /// Permet de tester la méthode IsNotEqual de la classe DoubleExtension
    /// </summary>
    [TestMethod]
    public void TestDoubleNullableIsNotEqualInferiorToDefaultPrecision()
    {
      double? double1 = 0.333d;
      double? double2 = 1 / 3d;
      double1.IsNotEqual(double2).Should().BeTrue();

      double1 = 0.333d;
      double2 = default(double);
      double1.IsNotEqual(double2).Should().BeTrue();

      double1 = default(double);
      double2 = 0.333d;
      double1.IsNotEqual(double2).Should().BeTrue();
    }
    #endregion





  }
}
