using System.Collections.Generic;
using Fred.Business.Role;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Utilisateur
{
  /// <summary>
  ///   Classe permettant de hiérarchiser les rôles
  /// </summary>
  public sealed class UtilisateurRoleComparer : IComparer<AffectationSeuilUtilisateurEnt>
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
    int IComparer<AffectationSeuilUtilisateurEnt>.Compare(AffectationSeuilUtilisateurEnt x, AffectationSeuilUtilisateurEnt y)
    {
      return RoleManager.CompareRoleNiveauPaie(x.Role, y.Role);
    }
  }
}