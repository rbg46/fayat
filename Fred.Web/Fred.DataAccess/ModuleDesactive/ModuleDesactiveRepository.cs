using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ModuleDesactive;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.ModuleDesactive
{
    /// <summary>
    ///   Référentiel de données pour les ModuleDesactive
    /// </summary>
    public class ModuleDesactiveRepository : FredRepository<ModuleDesactiveEnt>, IModuleDesactiveRepository
    {
        public ModuleDesactiveRepository(FredDbContext context)
          : base(context)
        {

        }

        /// <summary>
        /// Desactive un module pour une societe
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        /// <param name="societeId">societeId</param>
        /// <returns>l'id de l'element ModuleDesactiveEnt nouvellement créé.</returns>
        public int DisableModuleForSocieteId(int moduleId, int societeId)
        {
            var newModuleDesactive = new ModuleDesactiveEnt()
            {
                SocieteId = societeId,
                ModuleId = moduleId
            };
            this.Insert(newModuleDesactive);
            return newModuleDesactive.ModuleDesactiveId;
        }

        /// <summary>
        /// Retourne une liste de ModuleDesactiveEnt.
        /// Un module est desactive des lors qu'il y a un 'entree'(ligne) dans la base.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Une liste de ModuleDesactiveEnt</returns>
        public IEnumerable<ModuleDesactiveEnt> GetInactifModulesForSocieteId(int societeId)
        {
            return Query()
              .Filter(m => m.SocieteId == societeId)
              .Get()
              .AsNoTracking()
              .ToList();
        }
    }
}