
using System.Collections.Generic;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.ReferentielFixe;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les chapitres.
    /// </summary>
    public interface IChapitreRepository : IRepository<ChapitreEnt>
    {
        /// <summary>
        ///   Retourne le chapitre avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="chapitreId">Identifiant du chapitre à retrouver.</param>
        /// <returns>Le chapitre retrouvé, sinon null.</returns>
        ChapitreEnt GetById(int chapitreId);

        /// <summary>
        ///   Retourne la liste des chapitres.
        /// </summary>
        /// <returns>Liste des chapitres.</returns>
        IEnumerable<ChapitreEnt> GetList();

        /// <summary>
        ///   Obtient la collection des chapitres (suppprimés inclus)
        /// </summary>
        /// <returns>La collection des chapitres</returns>
        IEnumerable<ChapitreEnt> GetAllList();

        /// <summary>
        ///   Obtient la collection des chapitres appartenant à un groupe spécifié
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>La collection des chapitres</returns>
        IEnumerable<ChapitreEnt> GetChapitreListByGroupeId(int groupeId);

        /// <summary>
        ///   Indique si le code existe déjà pour les chapitres d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        bool IsCodeChapitreExist(string code, int groupeId);

        /// <summary>
        ///   Cherche une liste de Chapitre.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des Chapitres.</param>
        /// <returns>Une liste de Chapitre.</returns>
        IEnumerable<ChapitreEnt> SearchChapitres(string text);

        /// <summary>
        ///   Détermine si un chapitre peut être supprimé ou pas (en fonction de ses dépendances)
        /// </summary>
        /// <param name="chapter">élément à vérifier</param>
        /// <returns>Retourne Vrai si l'élément est supprimable, sinon Faux</returns>
        bool IsDeletable(ChapitreEnt chapter);

        /// <summary>
        ///   Cherche une liste des chapitres.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des chapitres.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Une liste de chapitres.</returns>
        IEnumerable<ChapitreEnt> SearchChapitres(string text, int groupeId);

        /// <summary>
        ///   Get Fes Chapitre List Moyen
        /// </summary>
        /// <returns>Une liste de chapitres.</returns>
        IEnumerable<int> GetFesChapitreListMoyen();

        /// <summary>
        /// Récupère les chapitres pour la comparaison de budget.
        /// </summary>
        /// <param name="chapitreIds">Les identifiants des chapitres concernés.</param>
        /// <returns>Les chapitres.</returns>
        List<AxeInfoDao> GetPourBudgetComparaison(IEnumerable<int> chapitreIds);
    }
}