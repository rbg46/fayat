using Fred.Entities.ModuleDesactive;
using System.Collections.Generic;

namespace Fred.Business.ModuleDesactive
{
  /// <summary>
  ///   Interface du gestionnaire des ModuleDesactive.
  /// </summary>
  public interface IModuleDesactiveManager : IManager<ModuleDesactiveEnt>
  {
    /// <summary>
    /// Retourne une liste de ModuleDesactiveEnt.
    /// Un module est desactive des lors qu'il y a un 'entree'(ligne) dans la base.
    /// </summary>
    /// <param name="societeId">societeId</param>
    /// <returns>Une liste de ModuleDesactiveEnt</returns>
    IEnumerable<ModuleDesactiveEnt> GetInactifModulesForSocieteId(int societeId);

    /// <summary>
    /// Desactive un module pour une societe
    /// </summary>
    /// <param name="moduleId">moduleId</param>
    /// <param name="societeId">societeId</param>
    /// <returns>l'id de l'element ModuleDesactiveEnt nouvellement créé.</returns>
    int DisableModuleForSocieteId(int moduleId, int societeId);

    /// <summary>
    /// Active un module pour une societeId et un moduleId
    /// </summary>
    /// <param name="moduleId">moduleId</param>
    /// <param name="societeId">societeId</param>    
    void EnableModuleForSocieteId(int moduleId, int societeId);

    /// <summary>
    /// Retourne la listes des societes inactives pour un module donné.
    /// </summary>
    /// <param name="moduleId">moduleId</param>
    /// <returns>Liste d'organisationIDs des societes désactivées.</returns>
    IEnumerable<int> GetInactivesSocietesForModuleId(int moduleId);

    /// <summary>
    /// Desactive un module pour une liste d' organisationId de societes et un module donné.
    /// </summary>
    /// <param name="moduleId">Id du module </param>
    /// <param name="organisationIdsOfSocietesToDisable"> liste d'organisationId de societes a désactiver</param>  
    /// <returns>Liste de societeId désactivés</returns> 
    IEnumerable<int> DisableModuleByOrganisationIdsOfSocietesAndModuleId(int moduleId, List<int> organisationIdsOfSocietesToDisable);

    /// <summary>
    /// Active un module pour une liste d' organisationId de societes et un module donné.
    /// </summary>
    /// <param name="moduleId">Id du module </param>
    /// <param name="organisationIdsOfSocietesToEnable"> liste d'organisationId de societes a sactiver</param>
    /// <returns>Liste de societeId activés</returns> 
    IEnumerable<int> EnableModuleByOrganisationIdsOfSocietesAndModuleId(int moduleId, List<int> organisationIdsOfSocietesToEnable);

    /// <summary>
    /// Retourne une liste d'id de module qui sont desactive sur au moins une societe
    /// </summary>
    /// <returns>liste d'id de module</returns>
    IEnumerable<int> GetIdsOfModulesPartiallyDisabled();
  }

}