using FluentValidation;
using Fred.Entities.ObjectifFlash;

namespace Fred.Business.ObjectifFlash
{
    /// <summary>
    ///   Interface du valideur des Objectif Flash
    /// </summary>
    public interface IObjectifFlashValidator : IValidator<ObjectifFlashEnt>
    {
        /// <summary>
        /// Controle de l'objectif flash et renseigne les listes d'erreurs
        /// </summary>
        /// <param name="objectifFlash">objectifFlash à controler</param>
        /// <returns>booleen indicant si l'objectif flash comporte des erreurs</returns>
        bool CheckObjectifFlashErrors(ObjectifFlashEnt objectifFlash);
    }
}
