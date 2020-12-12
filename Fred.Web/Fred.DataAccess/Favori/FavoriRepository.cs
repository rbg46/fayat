using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Favori;
using Fred.Entities.Search;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Tool;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Favori
{
    public class FavoriRepository : FredRepository<FavoriEnt>, IFavoriRepository
    {
        private readonly ILogManager logManager;

        public FavoriRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Retourne la liste des Favoris.
        /// </summary>
        /// <returns>Liste des Favoris.</returns>
        public IEnumerable<FavoriEnt> GetFavoriList()
        {
            foreach (FavoriEnt favoris in Context.Favori)
            {
                yield return favoris;
            }
        }

        /// <summary>
        ///   Retourne la liste des favoris d'un utilisateur
        /// </summary>
        /// <param name="userId">Id de l'utilisateur</param>
        /// <returns>Renvoie la liste des favoris de l'utilisateur passé en parametre</returns>
        public IEnumerable<FavoriEnt> GetFavoriList(int userId)
        {
            return Context.Favori.Where(f => f.UtilisateurId == userId).ToList();
        }

        /// <summary>
        ///   Retourne le Favoris dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="favoriId">Identifiant du Favoris à retrouver.</param>
        /// <returns>Le Favoris retrouvé, sinon nulle.</returns>
        public FavoriEnt GetFavoriById(int favoriId)
        {
            return Context.Favori.Find(favoriId);
        }

        /// <summary>
        ///   Ajout un nouveau favori.
        /// </summary>
        /// <param name="filters">Objet de recherche à enregistrer dans le favori</param>
        /// <param name="favori">Objet favori à enregistrer</param>
        /// <param name="idUtil">Id de l'utilisateur auquel rattacher le favori</param>
        /// <returns> L'identifiant du favori ajouté.</returns>
        public FavoriEnt AddFavori(AbstractSearch filters, FavoriEnt favori, int idUtil)
        {
            FavoriEnt favoriEnt = new FavoriEnt();
            try
            {
                favoriEnt.UtilisateurId = idUtil;
                favoriEnt.Libelle = favori != null ? favori.Libelle : string.Empty;
                favoriEnt.Couleur = favori != null ? favori.Couleur : string.Empty;
                favoriEnt.UrlFavori = favori != null ? favori.UrlFavori : string.Empty;
                favoriEnt.TypeFavori = favori != null ? favori.TypeFavori : string.Empty;
                favoriEnt.Search = SerialisationTools.Serialisation(filters);
                Context.Favori.Add(favoriEnt);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return favoriEnt;
        }

        /// <summary>
        ///   Supprime un Favoris
        /// </summary>
        /// <param name="id">L'identifiant du Favori à supprimer</param>
        public void DeleteFavoriById(int id)
        {
            FavoriEnt favori = Context.Favori.Find(id);
            if (favori == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.Favori.Remove(favori);

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Cherche une liste de Favoris.
        /// </summary>
        /// <param name="idUtilisateur">Le texte a chercher dans les propriétés des Favoris.</param>
        /// <returns>Une liste de Favoris.</returns>
        public IEnumerable<FavoriEnt> SearchFavorisByIdUtilisateur(int idUtilisateur)
        {
            var favoris = Context.Favori.Where(c => c.UtilisateurId.Equals(idUtilisateur));

            foreach (FavoriEnt favori in favoris)
            {
                yield return favori;
            }
        }
    }
}
