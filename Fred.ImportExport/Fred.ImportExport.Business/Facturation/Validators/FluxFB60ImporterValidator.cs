using Fred.Business.Facturation;

namespace Fred.ImportExport.Business.Facturation.Validators
{
    public class FluxFB60ImporterValidator : BaseFluxImporterValidator, IFluxFB60ImporterValidator
    {
        public FluxFB60ImporterValidator(IFacturationManager facturationManager)
            : base(facturationManager)
        {
        }
    }
}
