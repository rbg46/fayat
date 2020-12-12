using System.Collections.Generic;
using Fred.Entities;
using Fred.Entities.Fonctionnalite;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// repo de Fonctionnalite
    /// </summary>
    public interface IFonctionnaliteRepository : IRepository<FonctionnaliteEnt>
    {
        /// <summary>
        ///   Retourne la liste des fonctionnalités.
        /// </summary>
        /// <returns>Liste des fonctionnalités.</returns>
        IEnumerable<FonctionnaliteEnt> GetFeatureList();

        /// <summary>
        ///   Retourne la liste de toutes les fonctionnalités.
        /// </summary>
        /// <returns>Liste des fonctionnalités.</returns>
        IEnumerable<FonctionnaliteEnt> GetAllFeatureList();

        /// <summary>
        ///   Retourne le Fonctionnalite l'identifiant unique indiqué.
        /// </summary>
        /// <param name="featureId">Identifiant du Fonctionnalite à retrouver.</param>
        /// <returns>Le Fonctionnalite retrouvé, sinon null.</returns>
        FonctionnaliteEnt GetFeatureById(int featureId);

        /// <summary>
        ///   Retourne le Fonctionnalite par le code indiqué.
        /// </summary>
        /// <param name="code">Code de la Fonctionnalite à retrouver.</param>
        /// <returns>Le Fonctionnalite retrouvé, sinon null.</returns>
        FonctionnaliteEnt GetFeatureByCode(string code);

        /// <summary>
        ///   Retourne une liste de fonctionnalités via l'identifiant du module lié.
        /// </summary>
        /// <param name="moduleId">Identifiant du module lié aux fonctionnalités à retrouver.</param>
        /// <returns>Une liste de fonctionnalités.</returns>
        IEnumerable<FonctionnaliteEnt> GetFeatureListByModuleId(int moduleId);

        /// <summary>
        ///   Ajoute un nouveau Fonctionnalite
        /// </summary>
        /// <param name="feature">Fonctionnalite à ajouter</param>
        /// <returns> L'identifiant du Fonctionnalite ajouté</returns>
        FonctionnaliteEnt AddFeature(FonctionnaliteEnt feature);

        /// <summary>
        ///   Supprime un Fonctionnalite
        /// </summary>
        /// <param name="id">L'identifiant du Fonctionnalite à supprimer</param>
        void DeleteFeatureById(int id);

        /// <summary>
        ///   Supprime toutes les fonctionnaltiés liées à un module
        /// </summary>
        /// <param name="moduleId">L'identifiant du module dont les fonctionnalités sont à supprimer</param>
        void DeleteFeatureListByModuleId(int moduleId);

        /// <summary>
        ///   Met à jour une fonctionnalite.
        /// </summary>
        /// <param name="feature">L'identifiant de la fonctionnalite à mettre à jour</param>
        /// <returns>id fonctionnalite</returns>
        FonctionnaliteEnt UpdateFeature(FonctionnaliteEnt feature);

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
        /// Retourne les fonctionnalitees inactives pour une liste de societes
        /// </summary>
        /// <param name="societeIds"> societe Ids</param>
        /// <returns>Liste de FonctionnaliteInactiveResponse</returns>
        List<FonctionnaliteInactiveResponse> GetFonctionnalitesInactives(List<int> societeIds);

        /// <summary>
        /// Retourne les fonctionnalitees pour une permission, une liste de societes et une liste de modes autorisés
        /// </summary>
        /// <param name="permissionId">permissionId</param>
        /// <param name="societeIds">societeIds</param>
        /// <param name="modesAutorized">modesAutorized</param>
        /// <returns>Liste de FonctionnaliteForPermissionResponse</returns>
        List<FonctionnaliteForPermissionResponse> GetFonctionnalitesForPermission(int permissionId, List<int> societeIds, List<FonctionnaliteTypeMode> modesAutorized);
    }
}
