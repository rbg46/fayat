using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Fonctionnalite;
using Fred.Business.ModuleDesactive;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Module;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.Module
{
    public class ModuleManager : Manager<ModuleEnt, IModuleRepository>, IModuleManager
    {
        private readonly IFonctionnaliteManager fonctionnaliteManager;
        private readonly IModuleDesactiveManager moduleDesactiveManager;

        public ModuleManager(
            IUnitOfWork uow,
            IModuleRepository moduleRepository,
            IModuleValidator moduleValidator,
            IFonctionnaliteManager fonctionnaliteManager,
            IModuleDesactiveManager moduleDesactiveManager)
          : base(uow, moduleRepository, moduleValidator)
        {
            this.fonctionnaliteManager = fonctionnaliteManager;
            this.moduleDesactiveManager = moduleDesactiveManager;
        }

        /// <summary>
        ///   Retourne la liste des modules.
        /// </summary>
        /// <returns>Renvoie la liste des modules.</returns>
        public IEnumerable<ModuleEnt> GetModuleList()
        {
            return Repository.GetModuleList().OrderBy(c => c.Code);
        }

        /// <summary>
        ///   Retourne le module portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="moduleId">Identifiant d'une module à retrouver.</param>
        /// <returns>Le module retrouvé, sinon null.</returns>
        public ModuleEnt GetModuleById(int moduleId)
        {
            return Repository.GetModuleById(moduleId);
        }

        /// <summary>
        ///   Retourne le module portant le code indiqué.
        /// </summary>
        /// <param name="code">Le code d'un module à retrouver.</param>
        /// <returns>Le module retrouvé, sinon null.</returns>
        public ModuleEnt GetModuleByCode(string code)
        {
            return Repository.GetModuleByCode(code);
        }

        /// <summary>
        ///   Ajoute un nouveau module
        /// </summary>
        /// <param name="module">module à ajouter</param>
        /// <returns>L'identifiant d'une module ajouté</returns>
        public ModuleEnt AddModule(ModuleEnt module)
        {
            BusinessValidation(module);
            Repository.Insert(module);
            Save();

            return module;
        }

        /// <summary>
        ///   Supprime un module
        /// </summary>
        /// <param name="id">L'identifiant d'un module à supprimer</param>
        public void DeleteModuleById(int id)
        {
            if (Repository.IsDeletable(id))
            {
                ModuleEnt module = Repository.FindById(id);

                module.DateSuppression = DateTime.Now;
                fonctionnaliteManager.DeleteFeatureListByModuleId(id);
                Repository.Update(module);
                Save();
            }
            else
            {
                throw new FredBusinessException(ModuleResources.Module_SuppressionImpossible);
            }
        }

        /// <summary>
        ///   Met à jour un module
        /// </summary>
        /// <param name="module">Module à mettre à jour</param>
        /// <returns>Le Module mis à jour</returns>
        public ModuleEnt UpdateModule(ModuleEnt module)
        {
            BusinessValidation(module);
            Repository.Update(module);
            Save();

            return module;
        }

        /// <summary>
        /// Retourne la liste des modules disponibles pour la societe.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>La liste des modules disponibles pour la societe.</returns>
        public IEnumerable<ModuleEnt> GetModulesAvailablesForSocieteId(int societeId)
        {
            var result = new List<ModuleEnt>();
            var moduleDesactivesIds = this.moduleDesactiveManager.GetInactifModulesForSocieteId(societeId).Select(md => md.ModuleId);
            var modules = this.Repository.Query()
                                          .Filter(m => m.DateSuppression == null)
                                          .Get()
                                          .AsNoTracking()
                                          .ToList();
            foreach (var module in modules)
            {
                if (!moduleDesactivesIds.Contains(module.ModuleId))
                {
                    result.Add(module);
                }
            }
            return result;
        }
    }
}