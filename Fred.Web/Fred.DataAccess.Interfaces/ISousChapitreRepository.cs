using System.Collections.Generic;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.ReferentielFixe;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les sous-chapitres.
    /// </summary>
    public interface ISousChapitreRepository : IRepository<SousChapitreEnt>
    {
        /// <summary>
        ///   Retourne le sousChapitre avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="sousChapitreId">Identifiant du sousChapitre à retrouver.</param>
        /// <returns>Le sousChapitre retrouvé, sinon null.</returns>
        SousChapitreEnt GetById(int sousChapitreId);

        /// <summary>
        ///   Retourne la liste des sousChapitres.
        /// </summary>
        /// <returns>Liste des sousChapitres.</returns>
        IEnumerable<SousChapitreEnt> GetList();

        /// <summary>
        ///   Obtient la collection des sous-chapitres (suppprimés inclus)
        /// </summary>
        /// <returns>La collection des sous-chapitres</returns>
        IEnumerable<SousChapitreEnt> GetAllList();

        /// <summary>
        ///   Obtient la collection des sous-chapitres appartenant à un chapitre spécifié
        /// </summary>
        /// <param name="chapitreId">Identifiant du chapitre.</param>
        /// <returns>La collection des sous-chapitres</returns>
        IEnumerable<SousChapitreEnt> GetListByChapitreId(int chapitreId);

        /// <summary>
        ///   Indique si le code existe déjà pour les sous-chapitres d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        bool IsCodeSousChapitreExist(string code, int groupeId);

        /// <summary>
        ///   Cherche une liste de SousChapitre.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des SousChapitres.</param>
        /// <returns>Une liste de SousChapitre.</returns>
        IEnumerable<SousChapitreEnt> SearchSousChapitres(string text);

        /// <summary>
        ///   Cherche une liste des sousChapitres.
        /// </summary>
        /// <param name="groupId">Groupe Id du chapitre.</param>
        /// <returns>Une liste de sousChapitres.</returns>
        IEnumerable<SousChapitreEnt> SearchSousChapitres(int groupId);

        /// <summary>
        ///   Cherche une liste des sousChapitres.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des sousChapitres.</param>
        /// <param name="groupId">Groupe Id du chapitre.</param>
        /// <returns>Une liste de sousChapitres.</returns>
        IEnumerable<SousChapitreEnt> SearchSousChapitres(string text, int groupId);

        /// <summary>
        ///   Détermine si un sous chapitre peut être supprimé ou pas (en fonction de ses dépendances)
        /// </summary>
        /// <param name="subChapter">Elément à vérifier</param>
        /// <returns>Retourne Vrai si l'élément est supprimable, sinon Faux</returns>
        bool IsDeletable(SousChapitreEnt subChapter);

        /// <summary>
        /// Récupère les sous-chapitres pour la comparaison de budget.
        /// </summary>
        /// <param name="sousChapitreIds">Les identifiants des sous-chapitres concernés.</param>
        /// <returns>Les sous-chapitres.</returns>
        List<AxeInfoDao> GetPourBudgetComparaison(IEnumerable<int> sousChapitreIds);
    }
}
