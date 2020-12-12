using System.Collections.Generic;
using Fred.Entities.Societe.Classification;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Représente un référentiel de données pour les classifications sociétés.
    /// </summary>
    public interface ISocieteClassificationRepository : IFredRepository<SocieteClassificationEnt>
    {
        /// <summary>
        /// Retourner la requête de récupération des classifications sociétés active
        /// </summary>
        /// <returns>Une liste classifications sociétés actives</returns>
        IEnumerable<SocieteClassificationEnt> GetOnlyActive();

        /// <summary>
        /// Retourner la requête de récupération des classifications sociétés par groupe
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <param name = "onlyActive" > flag pour avoir seulement les actifs</param>
        /// <returns>Une liste classifications sociétés.</returns>
        IEnumerable<SocieteClassificationEnt> GetByGroupeId(int groupeId, bool? onlyActive);

        /// <summary>
        ///   Retourner la requête de récupération des classifications sociétés.
        /// </summary>
        /// <param name="searchText">Le filtre</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <returns>Une liste classifications sociétés.</returns>
        IEnumerable<SocieteClassificationEnt> Search(string searchText, int? page = null, int? pageSize = null);

        /// <summary>
        /// Permet de joindre la liste des sociétés avec une classification
        /// </summary>
        /// <param name="classification">classification societe</param>
        /// <returns>une classification join societes</returns>
        SocieteClassificationEnt PopulateSocietes(SocieteClassificationEnt classification);
    }
}
