using Fred.Entities.Rapport;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
  /// <summary>
  /// Model contenant pour chaque jour les heures travaillés 
  /// </summary>
  public class PersonnelTotalHourByDayModel
  {
       /// <summary>
    /// le jour dont on veut récupérer les heures normales
    /// </summary>
    public int DayNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<PersonnelTotalHourByDayAndByCiModel> ListTotalHourByCi { get; set; }
  }
}
