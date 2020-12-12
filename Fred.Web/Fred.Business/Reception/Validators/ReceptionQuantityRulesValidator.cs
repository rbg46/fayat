using FluentValidation;
using FluentValidation.Validators;

namespace Fred.Business.Reception.Validators
{
    public class ReceptionQuantityRulesValidator : AbstractValidator<ReceptionsValidationModel>, IReceptionQuantityRulesValidator
    {
        public ReceptionQuantityRulesValidator()
        {
            RuleFor(receptionsForValidate => receptionsForValidate).Custom((receptionsForValidate, context) =>
            {
                ExecuteQuantiteRuleForGroupe(receptionsForValidate, context);
            });
        }

        /// <summary>
        /// CONFIG POUR TOUS LES GROUPES QUI NE DERIVENT PAS DE CETTE CLASSE
        /// </summary>
        /// <param name="receptionsForValidate"></param>
        /// <param name="context"></param>
        public virtual void ExecuteQuantiteRuleForGroupe(ReceptionsValidationModel receptionsForValidate, CustomContext context)
        {
            foreach (var reception in receptionsForValidate.ReceptionToAddOrUpdates)
            {
                if (reception.Quantite < 0)
                {
                    context.AddFailure("Quantite_" + reception.DepenseId, Fred.Web.Shared.App_LocalResources.FredResource.Global_Control_DataRequired_Erreur);
                }
            }
        }
    }
}
