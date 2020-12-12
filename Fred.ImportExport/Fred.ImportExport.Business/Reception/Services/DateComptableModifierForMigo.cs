using System;
using Fred.Framework.DateTimeExtend;

namespace Fred.ImportExport.Business.Reception.Services
{
    public class DateComptableModifierForMigo : IDateComptableModifierForMigo
    {
        public DateTime? AddOneMinuteForTheFirstDayOfMonth(DateTime? date)
        {
            if (!date.HasValue)
            {
                return date;
            }
            var firstDay = date.Value.GetLimitsOfMonth().StartDate;

            if (DateTime.Equals(date, firstDay))
            {
                return firstDay.AddMinutes(1);
            }
            return date;
        }

    }
}
