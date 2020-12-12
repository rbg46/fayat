using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Moyen;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Interface de référentiel de données des affectations des moyens
    /// </summary>
    public interface IAffectationMoyenRepository : IFredRepository<AffectationMoyenEnt>
    {
        /// <summary>
        /// Permet de récupérer la liste des affectation des moyens en fonction des critères de recherche.
        /// </summary>
        /// <param name="searchFilters">Filtres de recherche</param>
        /// <param name="affectationMoyenRolesFilters">Filtre en se basant sur les rôles de l'utilisateur</param>
        /// <param name="page">Page actuelle</param>
        /// <param name="pageSize">Taille de page</param>
        /// <returns>Retourne la liste filtré des affectations des moyens</returns>
        IEnumerable<AffectationMoyenEnt> SearchWithFilters(SearchAffectationMoyenEnt searchFilters, AffectationMoyenRolesFiltersEnt affectationMoyenRolesFilters, int page, int pageSize);

        /// <summary>
        /// Retourne la liste de toutes les affectations des moyens.
        /// </summary>
        /// <returns>La liste de toutes les affectations des moyens.</returns>
        IEnumerable<AffectationMoyenEnt> GetAffectationMoyens();


        /// <summary>
        /// Permet d'ajouter une affectation de moyen.
        /// </summary>
        /// <param name="affectationMoyen">Une affectation de moyen.</param>
        /// <returns>L'affectation de moyen ajoutée.</returns>
        AffectationMoyenEnt AddAffectationMoyen(AffectationMoyenEnt affectationMoyen);

        /// <summary>
        /// Get affectation moyen list by dates
        /// </summary>
        /// <param name="datesPredicate">Predicat à utiliser pour les dates des afféctations</param>
        /// <param name="typePredicate">Predicate à utiliser pour restreindre les types éligible au pointage matériel</param>
        /// <returns>La liste des affectations dans l'intervalle des dates défini par le Predicate</returns>
        IEnumerable<AffectationMoyenEnt> GetPointageMoyenAffectations(
            Expression<Func<AffectationMoyenEnt, bool>> datesPredicate,
            Expression<Func<AffectationMoyenEnt, bool>> typePredicate);

        /// <summary>
        /// Permet de supprimer une affection apres la suppression de location 
        /// </summary>
        /// <param name="idMaterielLocation">L'id du materiel location dont on veut suuprimer leurs affectations</param>
        void DeleteAffectationMoyen(int idMaterielLocation);

        /// <summary>
        /// Retourne un enumerable des affectations moyens en fonction materielLocationId
        /// </summary>
        /// <param name="materielLocationId">Materiel location Id</param>
        /// <returns>Retourne un enumerable  des affectations associes a un materiel location</returns>
        IEnumerable<AffectationMoyenEnt> GetAllAffectationByMaterielLocationId(int materielLocationId);

        /// <summary>
        /// Ajouter une liste des affectations moyens
        /// </summary>
        /// <param name="affectationMoyenList">Liste des affectations moyens</param>
        void AddAffectationMoyenList(IEnumerable<AffectationMoyenEnt> affectationMoyenList);

        /// <summary>
        /// Récupére liste des affectations moyen par identifiants
        /// </summary>
        /// <param name="affectationMoyenIds">Liste des Affectations moyen identifiants</param>
        /// <returns>Liste des affectations moyens</returns>
        List<AffectationMoyenEnt> GetListAffectationMoyenByIds(IEnumerable<int> affectationMoyenIds);

        /// <summary>
        /// Recuperer list des affectations moyens idantifiants par personnel id
        /// </summary>
        /// <param name="personnelId">Personnel idantifiant</param>
        /// <returns>List des affectations moyen ids</returns>
        List<int> GetAffectationMoyenIdListByPersonnelId(int personnelId);
    }
}
