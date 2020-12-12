using Fred.Entities.Rapport;
using Fred.Entities.Rapport.Search;

namespace Fred.Business.Rapport
{

    /// <summary>
    /// Fonctionnalité Create Read Update Delete des indemnités de déplacement
    /// </summary>
    public interface ISearchFeature
    {
        /// <summary>
        /// Récupère la liste des filtre pour la recherche des rapports
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <returns>Retourne un objet représentant les filtres</returns>
        SearchRapportEnt GetFiltersList(int utilisateurId);

        /// <summary>
        /// Recherche les rapport en fonction des options de recherche
        /// </summary>
        /// <param name="filter">Objet contenant l'ensemble des critères et filtres de la recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>IEnumerable contenant les rapports correspondants aux critères de recherche</returns>
        SearchRapportListWithFilterResult SearchRapportWithFilter(SearchRapportEnt filter, int? page = 1, int? pageSize = 20);

        /// <summary>
        /// Indique si un rapport peux être supprimé
        /// </summary>
        /// <param name="rapport">Rapport à supprimer</param>
        /// <returns>true si le rapport peux être supprimé</returns>
        bool RapportCanBeDeleted(RapportEnt rapport);
    }
}
