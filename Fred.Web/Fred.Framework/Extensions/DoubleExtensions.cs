using System;
using System.Globalization;

namespace Fred.Framework.Extensions
{
  /// <summary>
  ///   Extension du type double
  /// </summary>
  public static class DoubleExtensions
  {

    /// <summary>
    /// Precision par défaut pour la compération de double
    /// Soit le nombre de chiffre après la virgule définissant le degrés d'erreur acceptable
    /// La valeur par défaut est 0.00001
    /// </summary>
    public static double DefaultPrecision { get; } = 0.00001;


    /// <summary>
    ///   Renvois un double au format Anglais
    /// </summary>
    /// <param name="value">Valeur à transformer</param>
    /// <returns>Retourne un double au format Anglais</returns>
    public static string ToEnglishString(this double value)
    {
      return value.ToString(CultureInfo.GetCultureInfo("en-GB"));
    }

    /// <summary>
    ///   Caulcul la valeur au carré
    /// </summary>
    /// <param name="value">Valeur à transformer</param>
    /// <returns>retourne le carré de la valeur courante</returns>
    public static double Square(this double value)
    {
      return value * value;
    }


    /// <summary>
    /// Permet de vérifier si un double est équal (à peu prés!) à zéro 
    /// En effet, 0.33 n'est pas équal à 1/3...
    /// La précision par défaut est DefaultPrecision
    /// Pour une précision personalisée utilisez la fonction : <see cref="IsZero(double,double)"/>
    /// voir: https://msdn.microsoft.com/fr-fr/library/ya2zha7s(v=vs.110).aspx
    /// </summary>
    /// <param name="value">Valeur double à vérifier</param>
    /// <returns>Vrais si le double est très proche de zéro, sinon faux.</returns>
    public static bool IsZero(this double value)
    {
      return IsZero(value, DefaultPrecision);
    }


    /// <summary>
    /// voir <see cref="IsZero(double)"/>
    /// </summary>
    /// <param name="value">Valeur double à vérifier</param>
    /// <returns>Vrais si le double est très proche de zéro ou s'il n'y a pas de valeur, sinon faux.</returns>
    public static bool IsZero(this double? value)
    {
      if (!value.HasValue)
      {
        return true;
      }

      return IsZero(value.Value, DefaultPrecision);
    }


    /// <summary>
    /// Permet de vérifier si un double est équal (à peu prés!) à zéro 
    /// En effet, 0.33 n'est pas équal à 1/3...
    /// voir: https://msdn.microsoft.com/fr-fr/library/ya2zha7s(v=vs.110).aspx
    /// </summary>
    /// <param name="value">Valeur double à vérifier</param>
    /// <param name="precision">nombre de chiffre après la virgule définissant le degrés d'erreur acceptable, exemple 0.00001</param>
    /// <returns>Vrais si le double est très proche de zéro, sinon faux.</returns>
    public static bool IsZero(this double value, double precision)
    {
      return Math.Abs(value) < precision;
    }


    /// <summary>
    /// Voir <see cref="IsZero(double,double)"/>
    /// </summary>
    /// <param name="value">Valeur double à vérifier</param>
    /// <param name="precision">nombre de chiffre après la virgule définissant le degrés d'erreur acceptable, exemple 0.00001</param>
    /// <returns>Vrais si le double est très proche de zéro ou s'il n'y a pas de valeur, sinon faux.</returns>
    public static bool IsZero(this double? value, double precision)
    {
      if (!value.HasValue)
      {
        return true;
      }

      return IsZero(value.Value, precision);
    }



    /// <summary>
    /// Voir <see cref="IsZero(double)"/>
    /// </summary>
    /// <param name="value">Valeur double à vérifier</param>
    /// <returns>Faux si le double est très proche de zéro, sinon vrais.</returns>
    public static bool IsNotZero(this double value)
    {
      return IsNotZero(value, DefaultPrecision);
    }


    /// <summary>
    /// Voir <see cref="IsZero(double)"/>
    /// </summary>
    /// <param name="value">Valeur double à vérifier</param>
    /// <returns>Faux si le double est très proche de zéro ou s'il n'y a pas de valeur, sinon vrais.</returns>
    public static bool IsNotZero(this double? value)
    {
      if (!value.HasValue)
      {
        return false;
      }

      return IsNotZero(value.Value, DefaultPrecision);
    }



    /// <summary>
    /// Voir <see cref="IsZero(double, double)"/>
    /// </summary>
    /// <param name="value">Valeur double à vérifier</param>
    /// <param name="precision">nombre de chiffre après la virgule définissant le degrés d'erreur acceptable, exemple 0.00001</param>
    /// <returns>Faux si le double est très proche de zéro, sinon vrais.</returns>
    public static bool IsNotZero(this double value, double precision)
    {
      return !IsZero(value, precision);
    }


    /// <summary>
    /// Voir <see cref="IsZero(double, double)"/>
    /// </summary>
    /// <param name="value">Valeur double à vérifier</param>
    /// <param name="precision">nombre de chiffre après la virgule définissant le degrés d'erreur acceptable, exemple 0.00001</param>
    /// <returns>Faux si le double est très proche de zéro ou s'il n'y a pas de valeur, sinon vrais.</returns>
    public static bool IsNotZero(this double? value, double precision)
    {
      if (!value.HasValue)
      {
        return false;
      }

      return !IsZero(value, precision);
    }




    /// <summary>
    /// Permet de vérifier si un double est équal (à peu prés!) à un autre double 
    /// La précision par défaut est DefaultPrecision
    /// Pour une précision personalisée utilisez la fonction : <see cref="IsEqual(double, double ,double)"/>
    /// voir: https://msdn.microsoft.com/fr-fr/library/ya2zha7s(v=vs.110).aspx
    /// </summary>
    /// <param name="left">Valeur double à gauche de l'opérande</param>
    /// <param name="right">Valeur double à droite de l'opérande</param>
    /// <returns>Vrais si les deux doubles sont égales à la précision par défaut, sinon faux.</returns>
    public static bool IsEqual(this double left, double right)
    {
      return IsEqual(left, right, DefaultPrecision);
    }


    /// <summary>
    /// Voir <see cref="IsEqual(double,double)"/>
    /// </summary>
    /// <param name="left">Valeur double à gauche de l'opérande</param>
    /// <param name="right">Valeur double à droite de l'opérande</param>
    /// <returns>Vrais si les deux doubles sont égales à la précision par défaut ou s'il n'y a pas de valeur, sinon faux.</returns>
    public static bool IsEqual(this double? left, double? right)
    {
      return IsEqual(left, right, DefaultPrecision);
    }



    /// <summary>
    /// Permet de vérifier si un double est équal (à peu prés!) à un autre double 
    /// voir: https://msdn.microsoft.com/fr-fr/library/ya2zha7s(v=vs.110).aspx
    /// </summary>
    /// <param name="left">Valeur double à gauche de l'opérande</param>
    /// <param name="right">Valeur double à droite de l'opérande</param>
    /// <param name="precision">nombre de chiffre après la virgule définissant le degrés d'erreur acceptable, exemple 0.00001</param>
    /// <returns>Vrais si les deux doubles sont égales à la précision par défaut, sinon faux.</returns>
    public static bool IsEqual(this double left, double right, double precision)
    {
      // absLeft contient la valeur absolue de degré de précision de la comparaison
      double absLeft = Math.Abs(left * precision);
      // Si la différence entre les deux doubles est inférieure au degrès de précision, 
      // alors on considère que les doubles sont égaux.
      return Math.Abs(left - right) <= absLeft;
    }


    /// <summary>
    /// Permet de vérifier si un double est équal (à peu prés!) à un autre double 
    /// voir: https://msdn.microsoft.com/fr-fr/library/ya2zha7s(v=vs.110).aspx
    /// </summary>
    /// <param name="left">Valeur double à gauche de l'opérande</param>
    /// <param name="right">Valeur double à droite de l'opérande</param>
    /// <param name="precision">nombre de chiffre après la virgule définissant le degrés d'erreur acceptable, exemple 0.00001</param>
    /// <returns>Vrais si les deux doubles sont égales à la précision par défaut, sinon faux.</returns>
    public static bool IsEqual(this double? left, double? right, double precision)
    {
      // Si aucun des doubles n'a de valeur, alors ils sont identiques
      if (!left.HasValue && !right.HasValue)
      {
        return true;
      }

      // Si un des deux double n'a pas de valeur, alors ils sont différents.
      if (!left.HasValue || !right.HasValue)
      {
        return false;
      }

      return IsEqual(left.Value, right.Value, precision);
    }


    /// <summary>
    /// Voir <see cref="IsEqual(double,double)"/>
    /// </summary>
    /// <param name="left">Valeur double à gauche de l'opérande</param>
    /// <param name="right">Valeur double à droite de l'opérande</param>
    /// <returns>Faux si les deux doubles sont égales à la précision par défaut, sinon Vrais.</returns>
    public static bool IsNotEqual(this double left, double right)
    {
      return !IsEqual(left, right);
    }


    /// <summary>
    /// Voir <see cref="IsEqual(double,double)"/>
    /// </summary>
    /// <param name="left">Valeur double à gauche de l'opérande</param>
    /// <param name="right">Valeur double à droite de l'opérande</param>
    /// <returns>Faux si les deux doubles sont égales à la précision par défaut ou s'il n'y a pas de valeur, sinon Vrais.</returns>
    public static bool IsNotEqual(this double? left, double? right)
    {
      return !IsEqual(left, right);
    }



    /// <summary>
    /// Voir <see cref="IsEqual(double,double, double)"/>
    /// </summary>
    /// <param name="left">Valeur double à gauche de l'opérande</param>
    /// <param name="right">Valeur double à droite de l'opérande</param>
    /// <param name="precision">nombre de chiffre après la virgule définissant le degrés d'erreur acceptable, exemple 0.00001</param>
    /// <returns>Faux si les deux doubles sont égales à la précision par défaut, sinon Vrais.</returns>
    public static bool IsNotEqual(this double left, double right, double precision)
    {
      return !IsEqual(left, right, precision);
    }

  }

  

}