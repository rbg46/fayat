using Fred.Entities.Fonctionnalite;
using System.Collections.Generic;

namespace Fred.Business.Fonctionnalite
{
  /// <summary>
  ///  Manager des fonctionnalites
  /// </summary>
  public interface IFonctionnaliteManager : IManager<FonctionnaliteEnt>
  {

    /// <summary>
    ///   Retourne la liste des fonctionnalités
    /// </summary>
    /// <returns>Renvoie la liste des fonctionnalités.</returns>
    IEnumerable<FonctionnaliteEnt> GetFeatureList();

    /// <summary>
    ///   Retourne la fonctionnalite portant l'identifiant unique indiqué
    /// </summary>
    /// <param name="featureId">Identifiant de la fonctionnalite à retrouver</param>
    /// <returns>La fonctionnalite retrouvée, sinon null</returns>
    FonctionnaliteEnt GetFeatureById(int featureId);

    /// <summary>
    ///   Retourne une liste de fonctionnalités via l'identifiant du module lié.
    /// </summary>
    /// <param name="moduleId">Identifiant du module lié aux fonctionnalités à retrouver.</param>
    /// <returns>Une liste de fonctionnalités.</returns>
    IEnumerable<FonctionnaliteEnt> GetFeatureListByModuleId(int moduleId);

    /// <summary>
    ///   Ajout une nouvelle fonctionnalite.
    /// </summary>
    /// <param name="feature">fonctionnalite à ajouter.</param>
    /// <returns>L'identifiant de la fonctionnalite ajoutée</returns>
    FonctionnaliteEnt AddFeature(FonctionnaliteEnt feature);

    /// <summary>
    ///   Supprime une fonctionnalite.
    /// </summary>
    /// <param name="id">L'identifiant de la fonctionnalite à supprimer</param>
    void DeleteFeatureById(int id);

    /// <summary>
    ///   Met à jour une fonctionnalite.
    /// </summary>
    /// <param name="feature">L'identifiant de la fonctionnalite à mettre à jour</param>
    /// <returns>Renvoie un boléen</returns>
    FonctionnaliteEnt UpdateFeature(FonctionnaliteEnt feature);
    
    /// <summary>
    ///   Supprime toutes les fonctionnaltiés liées à un module
    /// </summary>
    /// <param name="moduleId">L'identifiant du module dont les fonctionnalités sont à supprimer</param>
    void DeleteFeatureListByModuleId(int moduleId);

    /// <summary>
    /// Recupere les fonctionnalites pour un role donné.
    /// le role doit etre actif sinon rien n'est retourné.
    /// Le module lié a la fonctionnalite ne doit pas etre desactive voir table FRED_MODULE_DESACTIVE sinon aucune fonctionnalite du module n'est retournée.
    /// </summary>
    /// <param name="roleId">roleId</param>
    /// <returns>Liste de fonctionnalités 'active'</returns>
    IEnumerable<FonctionnaliteEnt> GetFonctionnalitesForRoleId(int roleId);
    
    /// <summary>
    /// Recupere les fonctionnalites pour un utilisateur donné.
    /// le role doit etre actif sinon rien n'est retourné.
    /// Le module lié a la fonctionnalite ne doit pas etre desactive voir table FRED_MODULE_DESACTIVE sinon aucune fonctionnalite du module n'est retournée.
    /// </summary>
    /// <param name="utilisateurId">utilisateurId</param>
    /// <returns>Liste de fonctionnalités 'active'</returns>
    IEnumerable<FonctionnaliteEnt> GetFonctionnalitesForUtilisateur(int utilisateurId);

    /// <summary>
    /// Retourne la liste des fonctionnalites disponibles pour la societe et le module.
    /// </summary>
    /// <param name="societeId">societeId</param>
    /// <param name="moduleId">moduleId</param>
    /// <returns>La liste des fonctionnalites disponibles pour la societe.</returns>
    IEnumerable<FonctionnaliteEnt> GetFonctionnaliteAvailablesForSocieteIdAndModuleId(int societeId,int moduleId);
  }
}
