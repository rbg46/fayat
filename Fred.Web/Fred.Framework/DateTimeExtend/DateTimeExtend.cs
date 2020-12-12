using System;

namespace Fred.Framework.DateTimeExtend
{
  /// <summary>
  /// DateTime étendu à l'aide de propriétés supplémentaires
  /// </summary>
  public class DateTimeExtend
  {
    private DateTime date;    
    
    /// <summary>
    /// Obtient ou défini la date
    /// </summary>
    public DateTime Date
    {
      get { return DateTime.SpecifyKind(date, DateTimeKind.Utc); }
      set { date = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
    }

    /// <summary>
    /// Obtient ou défini si la date est un jour de vacances
    /// </summary>
    public bool IsHoliday { get; set; }

    /// <summary>
    /// Obtient ou défini si la date est un jour de weekend
    /// </summary>
    public bool IsWeekend { get; set; }
  }
}
