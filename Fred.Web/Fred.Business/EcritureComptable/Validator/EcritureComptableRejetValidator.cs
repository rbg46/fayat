using FluentValidation;
using Fred.Entities.EcritureComptable;

namespace Fred.Business.EcritureComptable.Validator
{
    /// <summary>
    /// Validator des rejets des ecritures comptables 
    /// </summary>
    public class EcritureComptableRejetValidator : AbstractValidator<EcritureComptableRejetEnt>, IEcritureComptableRejetValidator
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="EcritureComptableRejetValidator" />.
        /// </summary>
        public EcritureComptableRejetValidator()
        {
            //Règles de gestion techniques
            AddChildTechnicalRules();

            //Règles de gestion métiers
            AddBusinessRules();

            //Règles de gestion en cascade.
            AddChildRules();
        }

        /// <summary>
        /// Règles de gestion techniques
        /// </summary>
        private void AddChildRules()
        {
            // N/A
        }

        /// <summary>
        /// Règles de gestion métiers
        /// </summary>
        private void AddBusinessRules()
        {
            // N/A
        }

        /// <summary>
        /// Règles de gestion en cascade.
        /// </summary>
        private void AddChildTechnicalRules()
        {
            // N/A
        }
    }
}
