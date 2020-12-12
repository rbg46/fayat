using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;

namespace Fred.Business.Role
{
    /// <summary>
    ///   Validateur des seuils de validatio
    /// </summary>
    public class SeuilValidationValidator : AbstractValidator<SeuilValidationEnt>, ISeuilValidationValidator
    {
        private readonly IRoleRepository roleRepository;
        private const int MontantSeuilMax = 9999999;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="SeuilValidationValidator" />.
        /// </summary>
        /// <param name="uow">Gestionnaire des Unit of work.</param>
        /// <param name="roleRepository1"></param>
        public SeuilValidationValidator(IUnitOfWork uow, IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;

            RuleFor(sv => sv.DeviseId).NotEmpty().WithMessage(BusinessResources.DeviseObligatoire);

            RuleFor(sv => sv.Montant)
              .NotEmpty()
              .WithMessage(RoleResources.SeuilValidation_MontantObligatoire)
              .Must(montant => montant > 0 && montant <= MontantSeuilMax)
              .WithMessage(RoleResources.SeuilValidation_LimiteMontant);

            RuleFor(sv => sv).Must(IsSeuilWithDeviseUnique).WithMessage(RoleResources.SeuilValidation_DeviseDejaAssocieeAuRole);
        }

        /// <summary>
        ///   Vérifie si le rôle ne possède pas déjà une devise
        /// </summary>
        /// <param name="seuil">SeuilValidationEnt</param>
        /// <returns>True si la devise n'est pas déjà associé au rôle, sinon False</returns>
        private bool IsSeuilWithDeviseUnique(SeuilValidationEnt seuil)
        {
            if (seuil.RoleId == null)
            {
                return true;
            }

            var seuilValidationList = this.roleRepository.GetSeuilValidationListByRoleId(seuil.RoleId.Value).ToList();

            if (seuilValidationList.Any())
            {
                var sameDevise = seuilValidationList.Where(s => s.DeviseId == seuil.DeviseId).ToList();

                if (!sameDevise.Any())
                {
                    return true;
                }

                // Si c'est une mise à jour du montant, le seuilValidationEnt existe déjà donc on renvoit TRUE
                if (sameDevise.Count == 1 && seuil.SeuilValidationId != 0)
                {
                    SeuilValidationEnt seuilDeviseMatching = sameDevise.FirstOrDefault();
                    return seuilDeviseMatching != null && seuilDeviseMatching.SeuilValidationId == seuil.SeuilValidationId;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        ///   Valider le Rôle.
        /// </summary>
        /// <param name="instance">Rôle à valider.</param>
        /// <returns>Résultat de validation</returns>
        public new ValidationResult Validate(SeuilValidationEnt instance)
        {
            return base.Validate(instance);
        }
    }
}