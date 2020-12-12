
using System.Collections.Generic;
using Fred.Entities.ModuleDesactive;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les ModuleDesactive.
    /// </summary>
    public interface IModuleDesactiveRepository : IRepository<ModuleDesactiveEnt>
    {

        /// <summary>
        /// Desactive un module pour une societe
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        /// <param name="societeId">societeId</param>
        /// <returns>l'id de l'element ModuleDesactiveEnt nouvellement créé.</returns>
        int DisableModuleForSocieteId(int moduleId, int societeId);

        /// <summary>
        /// Retourne une liste de ModuleDesactiveEnt.
        /// Un module est desactive des lors qu'il y a un 'entree'(ligne) dans la base.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Une liste de ModuleDesactiveEnt</returns>
        IEnumerable<ModuleDesactiveEnt> GetInactifModulesForSocieteId(int societeId);
    }
}