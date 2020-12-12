using Fred.Entities.Module;
using System.Collections.Generic;

namespace Fred.Business.Module
{
  /// <summary>
  ///   Interface du gestionnaire des modules.
  /// </summary>
  public interface IModuleManager : IManager<ModuleEnt>
  {
    #region Modules

    /// <summary>
    ///   Retourne la liste des modules.
    /// </summary>
    /// <returns>Renvoie la liste des modules.</returns>
    IEnumerable<ModuleEnt> GetModuleList();

    /// <summary>
    ///   Retourne le module portant l'identifiant unique indiqué.
    /// </summary>
    /// <param name="moduleId">Identifiant du module à retrouver.</param>
    /// <returns>Le module retrouvé, sinon null</returns>
    ModuleEnt GetModuleById(int moduleId);

   
    /// <summary>
    ///   Ajout un nouveau module.
    /// </summary>
    /// <param name="module">module à ajouter.</param>
    /// <returns>L'identifiant du module ajouté.</returns>
    ModuleEnt AddModule(ModuleEnt module);

    /// <summary>
    ///   Supprime un module.
    /// </summary>
    /// <param name="id">L'identifiant du module à supprimer.</param>
    void DeleteModuleById(int id);

    /// <summary>
    ///   Met à jour un module
    /// </summary>
    /// <param name="module">Module à mettre à jour</param>
    /// <returns>Retourne un booléan</returns>
    ModuleEnt UpdateModule(ModuleEnt module);

    /// <summary>
    /// Retourne la liste des modules disponibles pour la societe.
    /// </summary>
    /// <param name="societeId">societeId</param>
    /// <returns>La liste des modules disponibles pour la societe.</returns>
    IEnumerable<ModuleEnt> GetModulesAvailablesForSocieteId(int societeId);

    #endregion


  }
}