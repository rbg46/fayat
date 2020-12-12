using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Gestionnaire des type dépenses.
    /// </summary>
    public class TypeDepenseManager : Manager<TypeDepenseEnt, ITypeDepenseRepository>, ITypeDepenseManager
    {
        public TypeDepenseManager(IUnitOfWork uow, ITypeDepenseRepository typeDepenseRepo)
          : base(uow, typeDepenseRepo)
        {
        }

        /// <summary>
        ///   Retourne la liste des type dépenses.
        /// </summary>
        /// <returns>Liste des type dépenses.</returns>
        public IEnumerable<TypeDepenseEnt> GetTypeDepenseList()
        {
            return this.Repository.GetTypeDepenseList() ?? new TypeDepenseEnt[] { };
        }

        /// <summary>
        ///   Retourne le type dépenses dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="typeDepenseId">Identifiant du type dépenses à retrouver.</param>
        /// <returns>Le type dépenses retrouvé, sinon nulle.</returns>
        public TypeDepenseEnt GetTypeDepenseById(int typeDepenseId)
        {
            return this.Repository.GetTypeDepenseById(typeDepenseId);
        }

        /// <summary>
        ///   Ajout un nouveau type dépenses
        /// </summary>
        /// <param name="typeDepenseEnt">Pays à ajouter</param>
        /// <returns>L'identifiant du type dépenses ajouté</returns>
        public int AddTypeDepense(TypeDepenseEnt typeDepenseEnt)
        {
            return this.Repository.AddTypeDepense(typeDepenseEnt);
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un type dépenses
        /// </summary>
        /// <param name="typeDepenseEnt">Pays à modifier</param>
        public void UpdateTypeDepense(TypeDepenseEnt typeDepenseEnt)
        {
            this.Repository.UpdateTypeDepense(typeDepenseEnt);
        }

        /// <summary>
        ///   Supprime un type dépenses
        /// </summary>
        /// <param name="id">L'identifiant du type dépenses à supprimer</param>
        public void DeleteTypeDepenseById(int id)
        {
            this.Repository.DeleteTypeDepenseById(id);
        }

        /// <summary>
        ///   Recherche une liste de type dépenses.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des type dépenses.</param>
        /// <returns>Une liste de type dépenses.</returns>
        public IEnumerable<TypeDepenseEnt> SearchTypeDepenses(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return GetTypeDepenseList();
            }

            return this.Repository.SearchTypeDepense(text);
        }
    }
}