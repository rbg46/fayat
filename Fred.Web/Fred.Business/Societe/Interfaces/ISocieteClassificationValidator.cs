using System.Collections.Generic;
using FluentValidation;
using Fred.Entities.Societe.Classification;

namespace Fred.Business.Societe.Interfaces
{
    /// <summary>
    /// Interface Valideur des Classifications Sociétés
    /// </summary>
    public interface ISocieteClassificationValidator : IValidator<SocieteClassificationEnt>
    {
        /// <summary>
        /// Contrôle avant suppression
        /// </summary>
        /// <param name="classification">Classification société</param>
        /// <remarks>Exception si RG non respectés</remarks>
        void DeletingValidation(SocieteClassificationEnt classification);

        /// <summary>
        /// Contrôle avant désactivation
        /// </summary>
        /// <param name="classification">Classification société</param>
        /// <remarks>Exception si RG non respectés</remarks>
        void DisablingValidation(SocieteClassificationEnt classification);

        /// <summary>
        /// Contrôle avant ajout
        /// </summary>
        /// <param name="societesClassifications">liste des classifications sociétés</param>
        /// <param name="classification">Classification société</param>
        /// <remarks>Exception si RG non respectés</remarks>
        void AddingValidation(IEnumerable<SocieteClassificationEnt> societesClassifications, SocieteClassificationEnt classification);
    }
}
