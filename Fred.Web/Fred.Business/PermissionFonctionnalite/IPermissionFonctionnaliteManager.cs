using Fred.Entities.PermissionFonctionnalite;
using System.Collections.Generic;

namespace Fred.Business.PermissionFonctionnalite
{
  /// <summary>
  /// Interface pour PermissionFonctionnaliteManager
  /// </summary>
  public interface IPermissionFonctionnaliteManager : IManager<PermissionFonctionnaliteEnt>
  {
    /// <summary>
    /// Retourne la table de jointure entre permission et fonctionnalite
    /// </summary>
    /// <param name="fonctionnaliteId">fonctionnaliteId</param>
    /// <returns>Liste de PermissionFonctionnaliteEnt</returns>
    IEnumerable<PermissionFonctionnaliteEnt> GetPermissionFonctionnalites(int fonctionnaliteId);

    /// <summary>
    /// Retourne l'ensemble de la table de jointure entre permission et fonctionnalite
    /// </summary>
    /// <returns>Liste de PermissionFonctionnaliteEnt</returns>
    IEnumerable<PermissionFonctionnaliteEnt> GetAllPermissionFonctionnalites();

    /// <summary>
    /// Ajoute une jointure entre permission et fonctionnalite
    /// </summary>
    /// <param name="permissionId">permissionId</param>
    /// <param name="fonctionnaliteId">fonctionnaliteId</param>
    /// <returns>Le nouvel element</returns>
    PermissionFonctionnaliteEnt Add(int permissionId, int fonctionnaliteId);
    
    /// <summary>
    /// Supprime une PermissionFonctionnaliteEnt
    /// </summary>
    /// <param name="permissionFonctionnaliteId">permissionFonctionnaliteId</param>
    void Delete(int permissionFonctionnaliteId);
    
    /// <summary>
    /// Suppression physique des PermissionFonctionnalites en fonction de l'id de la fonctionnalité. 
    /// </summary>
    /// <param name="fonctionnaliteId">Id de la fonctionnalite</param>
    void DeletePermissionFonctionnaliteListByFonctionnaliteId(int fonctionnaliteId);

    /// <summary>
    /// Permet de savoir si on peux Rajouter une permission a une fonctionnalité.
    /// Une Permission est lié à une et une seule fonctionnalité. 
    /// </summary>
    /// <param name="permissionId">permissionId</param>   
    /// <returns>boolean</returns>
    bool CanAdd(int permissionId);
  }
}