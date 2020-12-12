using Fred.Entities.Permission;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.Habilitation.Core
{
  /// <summary>
  /// Methodes d'extension pour PermissionEnt
  /// </summary>
  public static class PermissionEntExtension
  {

    private static Predicate<PermissionEnt> predicateContextuellesPermissions = (permission) => permission.PermissionContextuelle;
    private static Predicate<PermissionEnt> predicateGlobalesPermissions = (permission) => !permission.PermissionContextuelle;


    /// <summary>
    /// Supprime les doublons
    /// </summary>
    /// <param name="permissions">permissions</param>
    /// <returns>Une liste sans doublons</returns>
    public static IEnumerable<PermissionEnt> RemoveDuplicatesPermissions(this IEnumerable<PermissionEnt> permissions)
    {
      var result = new List<PermissionEnt>();

      foreach (var permission in permissions)
      {
        if (!result.Any(p => p.PermissionId == permission.PermissionId))
        {
          result.Add(permission);
        }
      }
      return result;
    }


    /// <summary>
    /// Selectionne les permissions Contextuelles
    /// </summary>
    /// <param name="permissions">permissions</param>
    /// <returns>Une liste de permissions Contextuelles</returns>
    public static IEnumerable<PermissionEnt> SelectGlobalsPermissions(this IEnumerable<PermissionEnt> permissions)
    {
      return SelectorPermissions(permissions, predicateGlobalesPermissions);
    }


    /// <summary>
    /// Selectionne les permissions Contextuelles
    /// </summary>
    /// <param name="permissions">permissions</param>
    /// <returns>Une liste de permissions Contextuelles</returns>
    public static IEnumerable<PermissionEnt> SelectContextuellesPermissions(this IEnumerable<PermissionEnt> permissions)
    {
      return SelectorPermissions(permissions, predicateContextuellesPermissions);
    }


    /// <summary>
    /// Selectionne les permissions Contextuelles
    /// </summary>
    /// <param name="permissions">permissions</param>
    /// <param name="predicate">predicate</param>
    /// <returns>Une liste de permissions Contextuelles</returns>
    private static IEnumerable<PermissionEnt> SelectorPermissions(this IEnumerable<PermissionEnt> permissions, Predicate<PermissionEnt> predicate)
    {
      var result = new List<PermissionEnt>();

      foreach (var permission in permissions)
      {
        if (predicate(permission))
        {
          result.Add(permission);
        }
      }
      return result;
    }



  }
}
