using System;
using System.Linq;
using FluentValidation;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Module;

namespace Fred.Business.Module
{
    /// <summary>
    ///   Valideur de modules
    /// </summary>
    public class ModuleValidator : AbstractValidator<ModuleEnt>, IModuleValidator
    {
        private readonly IModuleRepository moduleRepository;

        public ModuleValidator(IModuleRepository moduleRepository, IFonctionnaliteRepository fonctionnaliteRepository)
        {
            this.moduleRepository = moduleRepository;
            IFonctionnaliteValidator featureValidator = new FonctionnaliteValidator(fonctionnaliteRepository);

            RuleFor(m => m.Code)
              .NotEmpty().WithMessage(BusinessResources.CodeObligatoire)
              .Must(CodeNotExist).WithMessage(string.Format(BusinessResources.CodeDejaExistant, BusinessResources.Module));

            RuleFor(p => p.Libelle).NotEmpty().WithMessage(BusinessResources.LibelleObligatoire);

            RuleFor(c => c.Fonctionnalites).SetCollectionValidator(c => featureValidator);
        }

        /// <summary>
        ///   Vérifie si le code module existe déjà ou pas
        /// </summary>
        /// <param name="module">Module à vérifier</param>
        /// <param name="code">Code à vérifier</param>
        /// <returns>True si le code existe déjà, sinon False</returns>
        private bool CodeNotExist(ModuleEnt module, string code)
        {
            var moduleList = moduleRepository.GetModuleList().ToList();

            if (!moduleList.Any())
            {
                return true;
            }

            foreach (ModuleEnt m in moduleList)
            {
                if (string.Equals(m.Code, code, StringComparison.CurrentCultureIgnoreCase))
                {
                    return module.ModuleId == m.ModuleId;
                }
            }

            return true;
        }
    }
}
