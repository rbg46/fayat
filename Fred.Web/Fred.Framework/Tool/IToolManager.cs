using System;

namespace Fred.Framework.Tool
{
  /// <summary>
  ///   La classe IToolManager sert à lire une valeur dans le fichier de configuration
  /// </summary>
  public interface IToolManager
  {
    /// <summary>Obtient une valeur dans le fichier de configuration </summary>
    /// <param name="key">Clé de la valeur recherchée</param>
    /// <returns>Valeur de la clé</returns>
    string GetConfig(string key);

    /// <summary>
    ///   Calcule et retourne le numéro de la semaine d'une date
    /// </summary>
    /// <param name="date">date dont on veut connaître le numéro de semaine</param>
    /// <returns>Retourne le numéro de la semaine d'une date</returns>
    int? GetWeekOfYear(DateTime date);


  }
}