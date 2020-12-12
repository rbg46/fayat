using FluentValidation;
using Fred.GroupSpecific.Infrastructure;

namespace Fred.Business.Reception.Validators
{
    public interface IReceptionQuantityRulesValidator : IValidator<ReceptionsValidationModel>, IGroupAwareService
    {
    }
}
