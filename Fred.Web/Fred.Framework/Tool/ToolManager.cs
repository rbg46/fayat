using System;
using System.Configuration;
using System.Globalization;

namespace Fred.Framework.Tool
{
  /// <summary>
  ///   La classe ToolManager sert à lire une valeur dans le fichier de configuration
  /// </summary>
  public class ToolManager : IToolManager
  {
    /// <summary>Obtient une valeur dans le fichier de configuration </summary>
    /// <param name="key">Clé de la valeur recherchée</param>
    /// <returns>Valeur de la clé</returns>
    public string GetConfig(string key)
    {
      string value = ConfigurationManager.AppSettings[key];
      return value;
    }

    /// <summary>
    ///   Calcule et retourne le numéro de la semaine d'une date
    /// </summary>
    /// <param name="date">date dont on veut connaître le numéro de semaine</param>
    /// <returns>Retourne le numéro de la semaine d'une date</returns>
    public int? GetWeekOfYear(DateTime date)
    {
      DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
      Calendar cal = dfi.Calendar;
      return cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, dfi.FirstDayOfWeek);
    }

    ////public static int GetUtilisateurId()
    ////{
    ////  SecurityManager scMgr = new SecurityManager();
    ////  return scMgr.GetUtilisateurId();
    ////}
  }
}