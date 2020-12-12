using System.Collections.Generic;
using Fred.Entities.Favori;
using Fred.Entities.Search;

namespace Fred.Business.Favori
{
    /// <summary>
    ///   Gestionnaire des favoris.
    /// </summary>
    public interface IFavoriManager : IManager<FavoriEnt>
    {
        /// <summary>
        ///   Retourne la liste des favoris.
        /// </summary>
        /// <returns>Renvoie la liste des favoris.</returns>
        IEnumerable<FavoriEnt> GetFavoriList();

        /// <summary>
        ///   Retourne la liste des favoris d'un utilisateur
        /// </summary>
        /// <param name="userId">Id de l'utilisateur</param>
        /// <returns>Renvoie la liste des favoris de l'utilisateur passé en parametre</returns>
        IEnumerable<FavoriEnt> GetFavoriList(int userId);

        /// <summary>
        ///   Récupère la liste des favoris de l'utilisateur connecté
        /// </summary>
        /// <returns>Renvoie la liste des favoris de l'utilisateur connecté</returns>
        IEnumerable<FavoriEnt> GetUtilisateurFavoriList();

        /// <summary>
        ///   Retourne le favori portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="favoriId">Identifiant du favori à retrouver.</param>
        /// <returns>Le favori retrouvé, sinon nulle.</returns>
        FavoriEnt GetFavoriById(int favoriId);

        /// <summary>
        ///   Retourne une nouvelle instance de favori.
        /// </summary>
        /// <param name="type">Type de favori</param>
        /// <returns>Retourne un nouveau favori.</returns>
        FavoriEnt GetNewFavori(string type);

        /// <summary>
        ///   Ajout un nouveau favori.
        /// </summary>
        /// <param name="filters">Objet de recherche à enregistrer dans le favori</param>
        /// <param name="favori">L'objet favori à enregistrer en bdd</param>    
        /// <returns>Favori ajouté.</returns>
        FavoriEnt AddFavori(AbstractSearch filters, FavoriEnt favori);

        /// <summary>
        ///   Ajout un nouveau favori.
        /// </summary>
        /// <param name="favori">L'objet favori à enregistrer en bdd</param>
        /// <returns>Favori ajouté.</returns>
        FavoriEnt AddFavori(FavoriEnt favori);

        /// <summary>
        ///   Sauvegarde les modifications d'un favori.
        /// </summary>
        /// <param name="favoriEnt">Favori à modifier</param>
        /// <returns>Favori mis à jour</returns>
        FavoriEnt UpdateFavori(FavoriEnt favoriEnt);

        /// <summary>
        ///   Supprime un favori.
        /// </summary>
        /// <param name="id">L'identifiant du favori à supprimer.</param>
        void DeleteFavoriById(int id);

        /// <summary>
        ///   Chercher une liste de favoris
        /// </summary>
        /// <param name="idUtilisateur">L'id utilisateur a chercher dans les propriétés des favoris.</param>
        /// <returns>Une liste de favoris</returns>
        IEnumerable<FavoriEnt> SearchFavoris(int idUtilisateur);

        /// <summary>
        /// Handle Add favoris
        /// </summary>
        /// <param name="favEnt">Favori entity</param>
        /// <param name="filtre">Filtres</param>
        /// <returns>Favori</returns>
        FavoriEnt HandleAddFavoris(FavoriEnt favEnt, object filtre);
    }
}