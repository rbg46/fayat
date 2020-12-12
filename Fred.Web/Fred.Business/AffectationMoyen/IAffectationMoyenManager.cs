using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Business.AffectationMoyen
{
    /// <summary>
    /// Interface des gestionnaires des affectations des moyens
    /// </summary>
    public interface IAffectationMoyenManager : IManager<AffectationMoyenEnt>
    {
        /// <summary>
        /// Permet de récupérer la liste des affectation des moyens en fonction des critères de recherche.
        /// </summary>
        /// <param name="searchFilters">Filtres de recherche</param>
        /// <param name="page">Page actuelle</param>
        /// <param name="pageSize">Taille de page</param>
        /// <returns>Retourne la liste filtré des affectations des moyens</returns>
        IEnumerable<AffectationMoyenEnt> SearchWithFilters(SearchAffectationMoyenEnt searchFilters, int page, int pageSize);

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
        /// Get Affectation moyen famille by type
        /// </summary>
        /// <returns>Affectation moyen famille by type model</returns>
        IEnumerable<AffectationMoyenFamilleByTypeModel> GetAffectationMoyenFamilleByType();

        /// <summary>
        /// Validate affectation moyen
        /// </summary>
        /// <param name="model">Update affectation moyen model</param>
        void ValidateAffectationMoyen(ValidateAffectationMoyenModel model);

        /// <summary>
        /// Retourne la liste des affectations éligible au pointage matériel
        /// </summary>
        /// <param name="datesPredicate">Predicat à utiliser pour les dates des afféctations</param>
        /// <param name="typePredicate">Predicate pour les types d'affectation</param>
        /// <returns>La liste des affectations dans l'intervalle des dates défini par le Predicate </returns>
        IEnumerable<AffectationMoyenEnt> GetPointageMoyenAffectations(
                            Expression<Func<AffectationMoyenEnt, bool>> datesPredicate,
                            Expression<Func<AffectationMoyenEnt, bool>> typePredicate);

        /// <summary>
        /// Récupére la liste des codes ci pour les restitutions et les maintenances
        /// </summary>
        /// <param name="codeList">La liste des codes</param>
        /// <returns>List des codes</returns>
        IEnumerable<string> GetRestitutionAndMaintenanceCiCodes(IEnumerable<string> codeList);

        /// <summary>
        /// Permet de supprimer une affection apres la suppression de location 
        /// </summary>
        /// <param name="idMaterielLocation">L'id de materiel en location </param>
        void DeleteAffectationMoyen(int idMaterielLocation);

        /// <summary>
        /// Return affectation by materielLocationId
        /// </summary>
        /// <param name="materielLocationId">Materiel location Id</param>
        /// <returns>Retourne une enumerable des affectations moyens associes a un materiel en location</returns>
        IEnumerable<AffectationMoyenEnt> GetAllAffectationByMaterielLocationId(int materielLocationId);

        /// <summary>
        /// Add range affectation list
        /// </summary>
        /// <param name="affectationList">Affectation list</param>
        /// <param name="isAdd">Is an add opration</param>
        void AddOrUpdateRangeAffectationList(IEnumerable<AffectationMoyenEnt> affectationList, bool isAdd);

        /// <summary>
        /// Supprimer les anciens rapports lignes lors de la réaffectation
        /// </summary>
        /// <param name="rapportLigneToUpdate">Rapport ligne à supprimer</param>
        void DeleteRapportLigneWithPointage(IEnumerable<RapportLigneEnt> rapportLigneToUpdate);
    }
}
