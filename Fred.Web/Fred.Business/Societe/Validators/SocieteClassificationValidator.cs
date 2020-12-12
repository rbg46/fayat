using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Societe.Interfaces;
using Fred.Entities.Societe.Classification;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Societe.Validators
{
    /// <summary>
    /// Classe Valideur des Classifications Sociétés
    /// </summary>
    public class SocieteClassificationValidator : AbstractValidator<SocieteClassificationEnt>, ISocieteClassificationValidator
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="SocieteClassificationValidator" />.
        /// </summary>
        public SocieteClassificationValidator()
        {
            //RG_7252_007
            RuleFor(x => x.SocieteClassificationId).NotNull().WithMessage(FeatureSociete.SocieteClassification_Id_Required);
            RuleFor(x => x.Code).NotEmpty().WithMessage(FeatureSociete.SocieteClassification_Code_Required);
            RuleFor(x => x.Libelle).NotEmpty().WithMessage(FeatureSociete.SocieteClassification_Libelle_Required);
        }


        /// <summary>
        /// Contrôle avant suppression
        /// </summary>
        /// <param name="classification">Classification société</param>
        /// <remarks>Exception si RG non respectés</remarks>
        public void DeletingValidation(SocieteClassificationEnt classification)
        {
            //RG_7252_009
            if (classification.Societes != null && classification.Societes.Count > 0)
            {
                throw new ValidationException(new List<ValidationFailure> { new ValidationFailure(nameof(classification.Societes), FeatureSociete.SocieteClassification_Societes_Delete_NotEmpty) });
            }
        }

        /// <summary>
        /// Contrôle avant désactivation
        /// </summary>
        /// <param name="classification">Classification société</param>
        /// <remarks>Exception si RG non respectés</remarks>
        public void DisablingValidation(SocieteClassificationEnt classification)
        {
            //RG_7252_009
            if (classification.Societes != null && classification.Societes.Count > 0)
            {
                throw new ValidationException(new List<ValidationFailure> { new ValidationFailure(nameof(classification.Societes), FeatureSociete.SocieteClassification_Societes_Disable_NotEmpty) });
            }
        }

        /// <summary>
        /// Contrôle avant ajout
        /// </summary>
        /// <param name="societesClassifications">liste des classifications sociétés</param>
        /// <param name="classification">Classification société</param>
        /// <remarks>Exception si RG non respectés</remarks>
        public void AddingValidation(IEnumerable<SocieteClassificationEnt> societesClassifications, SocieteClassificationEnt classification)
        {
            //Test Existence du Code
            if (societesClassifications.Any(c => c.Code.Equals(classification.Code) && !c.SocieteClassificationId.Equals(0)))
            {
                throw new ValidationException(new List<ValidationFailure> { new ValidationFailure(nameof(classification.Societes), FeatureSociete.SocieteClassification_Code_AlreadyUsed) });
            }
        }
    }
}
