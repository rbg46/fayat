using System.Collections.Generic;
using Fred.Entities.Role;

namespace Fred.Business.Role
{
  /// <summary>
  ///   Classe permettant de hiérarchiser les rôles
  /// </summary>
  public sealed class RoleComparer : IComparer<RoleEnt>
  {
    /// <summary>
    ///   Compare la hiérarchie des deux rôles
    /// </summary>
    /// <param name="x">premier rôle</param>
    /// <param name="y">second rôle</param>
    /// <returns>
    ///   entier représentant la hiérarchie entre les deux rôles :
    ///   -1 : X est supérieur
    ///   0 : égalité de rôle
    ///   1 : y est supérieur
    /// </returns>
    int IComparer<RoleEnt>.Compare(RoleEnt x, RoleEnt y)
    {
      return RoleManager.CompareRoleNiveauPaie(x, y);
    }
  }
}