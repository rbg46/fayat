
using System.Collections.Generic;
using Fred.Entities.Module;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données pour les modules.
    /// </summary>
    public interface IModuleRepository : IRepository<ModuleEnt>
    {
        /// <summary>
        ///   Retourne la liste des modules.
        /// </summary>
        /// <returns>Liste des modules.</returns>
        IEnumerable<ModuleEnt> GetModuleList();

        /// <summary>
        ///   Retourne le module l'identifiant unique indiqué.
        /// </summary>
        /// <param name="moduleId">Identifiant du module à retrouver.</param>
        /// <returns>Le module retrouvé, sinon null.</returns>
        ModuleEnt GetModuleById(int moduleId);

        /// <summary>
        ///   Récupère un module en fonction de son code.
        /// </summary>
        /// <param name="code">Code du module à retrouver.</param>
        /// <returns>Le module retrouvé, sinon null.</returns>
        ModuleEnt GetModuleByCode(string code);

        /// <summary>
        ///   Détermine si un module peut être supprimé
        /// </summary>
        /// <param name="moduleId">Module à vérifier</param>
        /// <returns>True si le module peut être supprimé, sinon Faux</returns>
        bool IsDeletable(int moduleId);
    }
}