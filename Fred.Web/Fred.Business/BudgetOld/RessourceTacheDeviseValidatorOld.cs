using FluentValidation;
using Fred.Entities.Budget;

namespace Fred.Business.Budget
{
    /// <summary>
    ///   Implémentation de la validation d'une RessourceTacheDevise
    /// </summary>
    public class RessourceTacheDeviseValidatorOld : AbstractValidator<RessourceTacheDeviseEnt>, IRessourceTacheDeviseValidatorOld
    {
        /// <summary>
        ///   Constructeur
        /// </summary>
        public RessourceTacheDeviseValidatorOld()
        {
            // Règles de gestions métiers
            AddBusinessRules();
        }

        /// <summary>
        ///   Ajout des règles de validation métiers
        /// </summary>
        private void AddBusinessRules()
        {
            //RG_1132_004: Les champs de montant/ P.U.ne peuvent excéder 999 999 999 999,99 avec une précision maximale de 2 décimales
            const double max = 999999999999.99;
            RuleFor(e => e.PrixUnitaire).InclusiveBetween(0, max).WithMessage("Le prix unitaire doit être compris entre 0 et " + max + ".");
        }
    }
}
