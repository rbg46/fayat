using System.Collections.Generic;
using Fred.Web.Shared.Models.Budget.Recette;

namespace Fred.Business.Budget.Recette
{
    public interface IAvancementRecetteLoader
    {
        AvancementRecetteLoadModel Load(int budgetId, int periode);
        List<AvancementRecetteLoadModel> LoadRecetteToPeriode(int budgetId, int fromperiode, int toperiode);
        List<PeriodAvancementRecetteLoadModel> LoadRecetteToPeriodes(int budgetId, int fromperiode, int toperiode);
    }
}