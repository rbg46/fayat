using FluentValidation;
using Fred.Entities.Budget.Recette;

namespace Fred.Business.Budget
{
    /// <summary>
    ///   Implémentation de la validation d'une TacheRecette
    /// </summary>
    public class TacheRecetteValidatorOld : AbstractValidator<TacheRecetteEnt>, ITacheRecetteValidatorOld
    {
        /// <summary>
        ///   Constructeur
        /// </summary>
        public TacheRecetteValidatorOld()
        {
            // Règles de gestions métiers
            AddBusinessRules();
        }

        /// <summary>
        ///   Ajout des règles de validation métiers
        /// </summary>
        private void AddBusinessRules()
        {
            // RG_1132_004: Les champs de montant/ P.U.ne peuvent excéder 999 999 999 999,99 avec une précision maximale de 2 décimales
            // de plus, Recette doit avoir une valeur si non null
            RuleFor(e => e.Recette).Must(Validate_RG_1132_004).WithMessage("La recette doit avoir une valeur renseignée");
        }

        /// <summary>
        /// RG_1132_004: Les champs de montant/ P.U.ne peuvent excéder 999 999 999 999,99 avec une précision maximale de 2 décimales
        /// de plus, Recette doit avoir une valeur si non null
        /// de plus, Recette peut-être négatif
        /// </summary>
        /// <param name="tacheRecetteEnt">tâche en cours de test</param>
        /// <param name="recette">Valeur de la recette</param>
        /// <returns>vrai si la règle est vérifiée, sinon faux</returns>
        private bool Validate_RG_1132_004(TacheRecetteEnt tacheRecetteEnt, double? recette)
        {
            const double max = 999999999999.99;

            if (!recette.HasValue)
            {
                return true;
            }

            return recette.Value >= -max && recette <= max;
        }
    }
}
