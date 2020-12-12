using System.Collections.Generic;
using Fred.Entities.Societe.Classification;

namespace Fred.Business.Societe.Interfaces
{
    /// <summary>
    /// Interface du gestionnaire des classifications sociétés
    /// </summary>
    public interface ISocieteClassificationManager : IManager<SocieteClassificationEnt>
    {
        /// <summary>
        /// Retourne la liste des classifications Sociétés
        /// </summary>
        /// <param name="onlyActive">flag pour avoir que les actifs</param>
        /// <returns>Renvoie toute la liste des classifications sociétés.</returns>
        IEnumerable<SocieteClassificationEnt> GetAll(bool onlyActive);

        /// <summary>
        /// Retourne la liste des classifications Sociétés du groupe
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe de la société</param>
        /// <param name="onlyActive">flag pour avoir seulement les actifs</param>
        /// <returns>Renvoie la liste des classifications sociétés.</returns>
        IEnumerable<SocieteClassificationEnt> GetByGroupeId(int groupeId, bool? onlyActive);

        /// <summary>
        /// Insertion et Mise à jour d'une liste des classifications
        /// </summary>        
        /// <param name="classifications">Liste des classifications à maj</param>
        /// <returns>Liste des classifications avec maj</returns>
        IEnumerable<SocieteClassificationEnt> CreateOrUpdateRange(IEnumerable<SocieteClassificationEnt> classifications);

        /// <summary>
        /// Suppression d'une liste des classifications par leur identifiants        
        /// </summary>
        /// <param name="classifications">Liste des classifications</param>
        void DeleteRange(IEnumerable<SocieteClassificationEnt> classifications);

        /// <summary>
        /// Retourner la requête de récupération des classifications 
        /// </summary>
        /// <param name="recherche">Le texte recherché</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <returns>La liste des classifications recherchées</returns>
        IEnumerable<SocieteClassificationEnt> Search(string recherche, int? page = null, int? pageSize = null);
    }
}
