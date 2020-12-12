using Fred.Entities.Permission;
using System.Collections.Generic;

namespace Fred.Business.Habilitation.Interfaces
{
  /// <summary>
  ///  Manager Core pour les Habilitations.
  /// </summary>
  public interface IHabilitationCoreManager
  {
    /// <summary>
    /// Recupere les permissions pour l'utilisateur actuel et pour un organisationId
    /// </summary>
    /// <param name="organisationId">organisationId</param>
    /// <returns>Une liste de permissions</returns>
    IEnumerable<PermissionEnt> GetPermissionsForUtilisateurAndOrganisation(int? organisationId);

    /// <summary>
    ///  Recupere toutes les permissions pour l'utilisateur actuel.
    /// </summary>
    /// <param name="utilisateurId">utilisateurId</param>
    /// <returns>Une liste de permissions</returns>
    IEnumerable<PermissionEnt> GetPermissionsForUtilisateur(int utilisateurId);
  }
}