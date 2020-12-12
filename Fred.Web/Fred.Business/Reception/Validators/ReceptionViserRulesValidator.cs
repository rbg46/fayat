using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.Validators
{
    /// <summary>
    /// Verifie que l'on peux pas mettre a jour ou supprimer une reception deja visée
    /// </summary>
    public class ReceptionViserRulesValidator : AbstractValidator<ReceptionListForValidate>, IReceptionViserRulesValidator
    {
        private readonly IDepenseRepository depenseRepository;

        public ReceptionViserRulesValidator(IDepenseRepository depenseRepository)
        {
            this.depenseRepository = depenseRepository;
            RuleFor(x => x.Receptions).Must(CanUpdateOrDeleteReceptions).WithMessage(BusinessResources.Validator_Update_Or_Delete_Reception_Already_Visee_Is_Not_Possible);
        }

        private bool CanUpdateOrDeleteReceptions(List<DepenseAchatEnt> receptions)
        {
            var receptionsIds = receptions.Select(x => x.DepenseId).ToList();

            var hasAnyReceptionAlreadyViseeResult = depenseRepository.HasAnyReceptionAlreadyVisee(receptionsIds);

            return !hasAnyReceptionAlreadyViseeResult.HasAnyReceptionsAlreadyVisee;
        }
    }
}
