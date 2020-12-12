using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.TypeDepense
{
    /// <summary>
    ///   Référentiel de données pour les types de dépense.
    /// </summary>
    public class TypeDepenseRepository : FredRepository<TypeDepenseEnt>, ITypeDepenseRepository
    {
        private readonly ILogManager logManager;

        public TypeDepenseRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Retourne la liste des Types de dépense.
        /// </summary>
        /// <returns>Liste des types de dépense.</returns>
        public IEnumerable<TypeDepenseEnt> GetTypeDepenseList()
        {
            foreach (TypeDepenseEnt typeDepense in Context.TypesDepense)
            {
                yield return typeDepense;
            }
        }

        /// <summary>
        ///   Retourne le type de dépense dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="typeDepenseId">Identifiant du type de dépense à retrouver.</param>
        /// <returns>le type de dépense retrouvé, sinon nulle.</returns>
        public TypeDepenseEnt GetTypeDepenseById(int typeDepenseId)
        {
            return Context.TypesDepense.Find(typeDepenseId);
        }

        /// <summary>
        ///   Ajout un nouveau type de dépense
        /// </summary>
        /// <param name="typeDepenseEnt"> Type dépense à ajouter</param>
        /// <returns> L'identifiant du type de dépense ajouté</returns>
        public int AddTypeDepense(TypeDepenseEnt typeDepenseEnt)
        {
            Context.TypesDepense.Add(typeDepenseEnt);

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return typeDepenseEnt.TypeDepenseId;
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un type de dépense.
        /// </summary>
        /// <param name="typeDepenseEnt">type de dépenses à modifier</param>
        public void UpdateTypeDepense(TypeDepenseEnt typeDepenseEnt)
        {
            try
            {
                this.Update(typeDepenseEnt);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Supprime un type de dépense
        /// </summary>
        /// <param name="id">L'identifiant du type de dépense à supprimer</param>
        public void DeleteTypeDepenseById(int id)
        {
            TypeDepenseEnt typeDepense = Context.TypesDepense.Find(id);
            if (typeDepense == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.TypesDepense.Remove(typeDepense);

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
        ///   Recherche une liste de type de dépense.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des type de dépense.</param>
        /// <returns>Une liste de type de dépense.</returns>
        public IEnumerable<TypeDepenseEnt> SearchTypeDepense(string text)
        {
            var typesDepenses = Context.TypesDepense.Where(c =>
                                                             c.Code.ToLower().Contains(text.ToLower()) ||
                                                             c.Libelle.ToLower().Contains(text.ToLower()));

            foreach (TypeDepenseEnt typeDepense in typesDepenses)
            {
                yield return typeDepense;
            }
        }
    }
}