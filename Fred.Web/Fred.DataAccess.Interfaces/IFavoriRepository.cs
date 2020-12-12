using System.Collections.Generic;
using Fred.Entities.Favori;
using Fred.Entities.Search;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les Favoris.
    /// </summary>
    public interface IFavoriRepository : IRepository<FavoriEnt>
    {
        /// <summary>
        ///   Retourne la liste des Favoris.
        /// </summary>
        /// <returns>La liste des Favoris.</returns>
        IEnumerable<FavoriEnt> GetFavoriList();

        /// <summary>
        ///   Retourne la liste des favoris d'un utilisateur
        /// </summary>
        /// <param name="userId">Id de l'utilisateur</param>
        /// <returns>Renvoie la liste des favoris de l'utilisateur passé en parametre</returns>
        IEnumerable<FavoriEnt> GetFavoriList(int userId);

        /// <summary>
        ///   Retourne le Favori portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="favoriId">Identifiant du Favori à retrouver.</param>
        /// <returns>Le Favori retrouvé, sinon nulle.</returns>
        FavoriEnt GetFavoriById(int favoriId);

        /// <summary>
        ///   Ajout un nouveau favori.
        /// </summary>
        /// <param name="filters">Objet de recherche à enregistrer dans le favori</param>
        /// <param name="favori">L'objet favori à enregistrer en bdd</param>
        /// <param name="idUtil">Id de l'utilisateur auquel rattacher le favori</param>
        /// <returns>Favori ajouté.</returns>
        FavoriEnt AddFavori(AbstractSearch filters, FavoriEnt favori, int idUtil);

        /// <summary>
        ///   Supprime un Favoris
        /// </summary>
        /// <param name="id">L'identifiant du Favoris à supprimer</param>
        void DeleteFavoriById(int id);

        /// <summary>
        ///   Cherche une liste de Favoris par utilisateur.
        /// </summary>
        /// <param name="idUtilisateur">L'id de l'utilisateur chercher dans les propriétés des Favoris.</param>
        /// <returns>Une liste de Favoris.</returns>
        IEnumerable<FavoriEnt> SearchFavorisByIdUtilisateur(int idUtilisateur);
    }
}