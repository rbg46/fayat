using Fred.Entities.Permission;
using System.Collections.Generic;

namespace Fred.Business.Habilitation.Interfaces
{
  /// <summary>
  /// Interface pour genere les habilitation contextuelles des ci. 
  /// </summary>
  public interface IHabilitationForCiManager : IContextuelHabilitation
  {
    /// <summary>
    /// Get the list of the current user's contextual authorizations for a specific permission key
    /// </summary>
    /// <param name="ciId">CI identifier</param>
    /// <param name="permissionKey">Identifier of the permission</param>
    /// <returns>A list of contextual permission</returns>
    IEnumerable<PermissionEnt> GetContextualAuthorization(int ciId, string permissionKey);
  }
}