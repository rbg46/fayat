using Fred.Business.Commande;
using Fred.Business.Habilitation.Interfaces;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Habilitation;
using Fred.Entities.Permission;
using Fred.Framework.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.Habilitation
{
  /// <summary>
  /// Manager pour les habilitation des commandes
  /// </summary>
  public class HabilitationForCommandeManager : IHabilitationForCommandeManager
  {
    private readonly IHabilitationManager habilitationManager;
    private readonly ICommandeManager commandeManager;
    private readonly IUtilisateurManager utilisateurManager;

    /// <summary>
    /// ctor
    /// </summary>     
    /// <param name="habilitationManager">habilitationManager</param>
    /// <param name="utilisateurManager">utilisateurManager</param>
    /// <param name="commandeManager">commandeManager</param>
    public HabilitationForCommandeManager(IHabilitationManager habilitationManager,
      IUtilisateurManager utilisateurManager,
      ICommandeManager commandeManager)
    {
      this.habilitationManager = habilitationManager;
      this.commandeManager = commandeManager;
      this.utilisateurManager = utilisateurManager;
    }

    /// <summary>
    /// Retourne les habilitations en fonction de l'id de l'entité.
    /// Si id = null cela renvoie les habilitations globales.
    /// </summary>   
    /// <param name="id">id</param>
    /// <returns>HabilitationEnt</returns>
    public HabilitationEnt GetHabilitationForEntityId(int? id = null)
    {
      HabilitationEnt globalsHabilitation = habilitationManager.GetHabilitation();
      globalsHabilitation.PermissionsContextuelles = GetContextuellesPermissionsForEntityId(id);

      return globalsHabilitation;
    }

    /// <summary>
    /// Retourne une liste de permissions contextuelles en fonction de l'id de l'entité.
    /// </summary>
    /// <param name="id">id de l'entité</param>
    /// <returns>Liste de permission contextuelles</returns>
    public IEnumerable<PermissionEnt> GetContextuellesPermissionsForEntityId(int? id = null)
    {
      if (id.HasValue)
      {
        int? organisationId = this.commandeManager.GetOrganisationIdByCommandeId(id.Value);

        if (organisationId.HasValue)
        {
          return habilitationManager.GetContextuellesPermissionsForUtilisateurAndOrganisation(organisationId);
        }
      }
      return new List<PermissionEnt>();
    }

    /// <summary>
    /// Determine si on a accès a l'entité. 
    /// </summary>
    /// <param name="id">id de l'entité</param>
    /// <param name="nullIsOk">Si on passe null alors nous somme Authorisé</param>
    /// <returns>true si authorisé</returns>
    public bool IsAuthorizedForEntity(int? id = default(int?), bool nullIsOk = true)
    {
      if (id.HasValue)
      {
        int? organisationId = this.commandeManager.GetOrganisationIdByCommandeId(id.Value);
        if (!organisationId.HasValue) { return false; }

        int utilisateurId = utilisateurManager.GetContextUtilisateurId();
        var orgaList = utilisateurManager.GetOrganisationAvailableByUserAndByTypeOrganisation(utilisateurId, OrganisationType.Ci.ToIntValue());

        return orgaList.Any(o => o.OrganisationId == organisationId);
      }
      return nullIsOk;
    }
  }
}