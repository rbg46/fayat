using Fred.Entities.Depense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fred.Business.Depense
{
    /// <summary>
    ///   Gestionnaire des dépenses.
    /// </summary>
    public interface IDepenseManager : IManager<DepenseAchatEnt>
    {
        /// <summary>
        /// Retourne la liste des dépenses.
        /// </summary>
        /// <returns>La liste des dépenses.</returns>
        IEnumerable<DepenseAchatEnt> GetDepenseList();

        /// <summary>
        /// Retourne la liste des dépenses selon un CI choisit
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>La liste des dépenses.</returns>
        Task<IEnumerable<DepenseAchatEnt>> GetDepenseListAsync(int ciId);

        /// <summary>
        /// Retourne la liste des dépenses en incluant les tahces et les ressources liées
        /// ainsi que toutes les factuartions
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>La liste des dépenses.</returns>
        Task<IEnumerable<DepenseAchatEnt>> GetDepensesListWithMinimumIncludesAsync(int ciId);

        /// <summary>
        /// Retourne la liste des dépenses filtrée, triée (avec pagination)
        /// </summary>
        /// <param name="filter">Objet de recherche et de tri des dépense</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Retourne la liste des dépenses filtrée, triée et paginée</returns>
        IEnumerable<DepenseAchatEnt> SearchDepenseListWithFilter(SearchDepenseEnt filter, int page, int pageSize);

        /// <summary>
        /// Retourne le montant total de la liste des dépenses filtrée, triée
        /// </summary>
        /// <param name="filter">Objet de recherche et de tri des dépense</param>
        /// <returns>Retourne le montant total de la liste des dépenses filtrée, triée et paginée</returns>
        double GetMontantTotal(SearchDepenseEnt filter);

        /// <summary>
        /// Retourne la dépense portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="depenseID">Identifiant de la dépense à retrouver.</param>
        /// <returns>La dépense retrouvée, sinon nulle.</returns>
        DepenseAchatEnt GetDepenseById(int depenseID);

        /// <summary>
        /// Ajout une nouvelle dépense.
        /// </summary>
        /// <param name="depense">dépense à ajouter</param>
        /// <returns>Dépense ajoutée</returns>
        DepenseAchatEnt AddDepense(DepenseAchatEnt depense);

        /// <summary>
        /// Ajout une nouvelle dépense.
        /// </summary>
        /// <param name="depense">dépense à ajouter</param>
        /// <param name="utilisateurId">L'identifant de l'auteur créateur.</param>
        /// <returns>Dépense ajoutée</returns>
        DepenseAchatEnt AddDepense(DepenseAchatEnt depense, int? utilisateurId);

        /// <summary>
        /// Sauvegarde les modifications d'une dépense.
        /// </summary>
        /// <param name="depense">dépense à modifier</param>
        /// <returns>Dépense modifiée</returns>
        DepenseAchatEnt UpdateDepense(DepenseAchatEnt depense);

        /// <summary>
        /// Sauvegarde les modifications d'une liste de dépenses
        /// </summary>
        /// <param name="depenses">Liste de dépenses à modifier</param>
        /// <param name="auteurModificationId">Identifiant de l'utilisateur ayant effectué la modification</param>
        /// <returns>Liste de Dépenses modifiées</returns>
        List<DepenseAchatEnt> UpdateDepense(List<DepenseAchatEnt> depenses, int auteurModificationId);

        /// <summary>
        /// Supprime une dépense.
        /// </summary>
        /// <param name="depenseId">Identifiant unique de la dépense à supprimer</param>
        void DeleteDepenseById(int depenseId);

        /// <summary>
        /// Initialise une nouvelle instance de dépense <see cref="DepenseAchatEnt" /> selon les règles de gestion établies.
        /// </summary>
        /// <param name="commandeLigneId">La ligne de commande à dépense</param>
        /// <returns>Retourne une instance de dépense.</returns>
        DepenseAchatEnt GetNewDepense(int commandeLigneId);

        /// <summary>
        /// Initialise une nouvelle instance de la classe de recherche des dépenses
        /// </summary>
        /// <returns>Objet de filtrage + tri des dépenses initialisé</returns>
        SearchDepenseEnt GetNewFilter();

        /// <summary>
        /// Renvoie la dépense associée au groupe de remplacement
        /// </summary>
        /// <param name="groupRemplacementId">L'id du groupe de remplacement</param>
        /// <returns>Dépense</returns>
        DepenseAchatEnt GetByGroupRemplacementId(int groupRemplacementId);

        /// <summary>
        /// Retourner la requête de récupération des dépenses
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <returns>Une requête</returns>
        IEnumerable<DepenseAchatEnt> Search(List<Expression<Func<DepenseAchatEnt, bool>>> filters,
                                            Func<IQueryable<DepenseAchatEnt>, IOrderedQueryable<DepenseAchatEnt>> orderBy = null,
                                            List<Expression<Func<DepenseAchatEnt, object>>> includeProperties = null,
                                            int? page = null,
                                            int? pageSize = null);
    }
}
