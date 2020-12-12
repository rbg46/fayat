using Fred.Entities.Habilitation;
using Fred.Entities.Permission;
using System.Collections.Generic;

namespace Fred.Business.Habilitation.Interfaces
{
  /// <summary>
  /// Interface pour genere les habilitation contextuelles.
  /// Pour chaque entité il y a une facon de recuperer l'organisationId.
  /// Cette organsiationId va nous permettre de recuperer les habilitations/permissions
  /// </summary>
  public interface IContextuelHabilitation
  {
    /// <summary>
    /// Retourne les habilitations en fonction de l'id de l'entité.
    /// Si id = null cela renvoie que les habilitations globales.
    /// </summary>   
    /// <param name="id">ciId</param>
    /// <returns>HabilitationEnt</returns>
    HabilitationEnt GetHabilitationForEntityId(int? id = null);

    /// <summary>
    /// Retourne une liste de permissions contextuelles en fonction de l'id de l'entité.
    /// </summary>
    /// <param name="id">id de l'entité</param>
    /// <returns>Liste de permissions contextuelles</returns>
    IEnumerable<PermissionEnt> GetContextuellesPermissionsForEntityId(int? id = null);

    /// <summary>
    /// Determine si on a accès a l'entité. 
    /// </summary>
    /// <param name="id">id de l'entité</param>
    /// <param name="nullIsOk">Si on passe null alors nous somme Authorisé</param>
    /// <returns>true si authorisé</returns>
    bool IsAuthorizedForEntity(int? id = null, bool nullIsOk = true);
  }
}