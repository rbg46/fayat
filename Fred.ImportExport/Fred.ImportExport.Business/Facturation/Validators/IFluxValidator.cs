using Fred.GroupSpecific.Infrastructure;

namespace Fred.ImportExport.Business.Facturation
{
    public interface IFluxValidator<in T> : IGroupAwareService
        where T : class
    {
        void Validate(T modelToValidate);
    }
}
