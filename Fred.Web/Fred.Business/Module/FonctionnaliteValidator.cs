using System;
using System.Linq;
using FluentValidation;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Fonctionnalite;

namespace Fred.Business.Module
{
    /// <summary>
    ///   Valideur de Fonctionnalités
    /// </summary>
    public class FonctionnaliteValidator : AbstractValidator<FonctionnaliteEnt>, IFonctionnaliteValidator
    {
        private readonly IFonctionnaliteRepository fonctionnaliteRepository;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="FonctionnaliteValidator" />.
        /// </summary>
        public FonctionnaliteValidator(IFonctionnaliteRepository fonctionnaliteRepository)
        {
            this.fonctionnaliteRepository = fonctionnaliteRepository;

            RuleFor(m => m.Code)
              .NotEmpty().WithMessage(BusinessResources.CodeObligatoire)
              .Must(CodeNotExist).WithMessage(string.Format(BusinessResources.CodeDejaExistant, BusinessResources.Fonctionnalite));

            RuleFor(p => p.Libelle).NotEmpty().WithMessage(BusinessResources.LibelleObligatoire);
        }

        /// <summary>
        ///   Vérifie si le code Fonctionnalité existe déjà ou pas
        /// </summary>
        /// <param name="feature">Fonctionnalité à vérifier</param>
        /// <param name="code">Code Fonctionnalité à vérifier</param>
        /// <returns>True si le code existe déjà, sinon False</returns>
        private bool CodeNotExist(FonctionnaliteEnt feature, string code)
        {
            var featureList = this.fonctionnaliteRepository.GetAllFeatureList().ToList();

            if (!featureList.Any())
            {
                return true;
            }

            foreach (FonctionnaliteEnt f in featureList)
            {
                if (string.Equals(f.Code, code, StringComparison.CurrentCultureIgnoreCase))
                {
                    return f.FonctionnaliteId == feature.FonctionnaliteId;
                }
            }

            return true;
        }
    }
}
