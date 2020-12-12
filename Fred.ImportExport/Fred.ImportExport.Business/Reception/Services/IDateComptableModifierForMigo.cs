using System;
using Fred.Business;

namespace Fred.ImportExport.Business.Reception.Services
{
    public interface IDateComptableModifierForMigo : IService
    {
        DateTime? AddOneMinuteForTheFirstDayOfMonth(DateTime? date);
    }
}
