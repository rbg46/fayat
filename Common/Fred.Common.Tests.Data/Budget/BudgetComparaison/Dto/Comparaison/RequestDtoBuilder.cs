using System.Collections.Generic;
using Fred.Business.Budget.BudgetComparaison.Dto;
using Fred.Business.Budget.BudgetComparaison.Dto.Comparaison;
using Fred.Common.Tests.EntityFramework;

namespace Fred.Common.Tests.Data.Budget.BudgetComparaison.Dto.Comparaison
{
    public class RequestDtoBuilder : ModelDataTestBuilder<RequestDto>
    {
        public RequestDtoBuilder BudgetId1(int budgetId1)
        {
            Model.BudgetId1 = budgetId1;
            return this;
        }

        public RequestDtoBuilder BudgetId2(int budgetId2)
        {
            Model.BudgetId2 = budgetId2;
            return this;
        }

        public RequestDtoBuilder Axes(List<AxeType> list)
        {
            Model.Axes = list;
            return this;
        }
    }
}
