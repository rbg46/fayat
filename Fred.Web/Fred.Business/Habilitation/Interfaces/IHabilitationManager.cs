using Fred.Entities.Habilitation;
using Fred.Entities.Permission;
using Fred.Entities.Utilisateur;
using System.Collections.Generic;
using System.Security.Claims;

namespace Fred.Business.Habilitation.Interfaces
{
  /// <summary>
  /// IHabilitationManager
  /// </summary>
  public interface IHabilitationManager
  {  

    /// <summary>
    /// Retourne les habilitation en fonction de l'organisation.
    /// </summary>
    /// <param name="organisationId">organisationId</param>
    /// <returns>HabilitationEnt</returns>
    HabilitationEnt GetHabilitation(int? organisationId = null);
       
    /// <summary>
    /// Retourne la liste de claims,qui correspond aux permissions globlales de l'utilisateur.
    /// Si l'utilisateur est superadmin il aura aussi le claim 'SuperAdmin'
    /// </summary>    
    /// /// <param name="utilisateur">utilisateur</param>
    /// <returns>la liste de claims</returns>
    IEnumerable<Claim> GetGlobalsClaims(UtilisateurEnt utilisateur);

    /// <summary>
    /// Recupere les permissions pour l'utilisateur actuel et pour un organisationId
    /// </summary>
    /// <param name="organisationId">organisationId</param>
    /// <returns>Une liste de permissions</returns>
    IEnumerable<PermissionEnt> GetContextuellesPermissionsForUtilisateurAndOrganisation(int? organisationId);
  }
}