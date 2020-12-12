using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Referential;
using Fred.Web.Shared.Models.Nature;

namespace Fred.Business.Referential.Nature
{
    /// <summary>
    /// Interface des gestionnaires des codes d'absence
    /// </summary>
    public interface INatureManager : IManager<NatureEnt>
    {
        /// <summary>
        /// Méthode d'ajout d'une nature
        /// </summary>
        /// <param name="nature">objet Nature à ajouter</param>
        /// <returns>Identifiant de la nature ajoutée</returns>
        int AddNature(NatureEnt nature);

        /// <summary>
        /// Méthode de sauvegarde des modifications d'une nature
        /// </summary>
        /// <param name="nature">Objet Nature modifié</param>
        /// <param name="fieldsToUpdate">Liste des champs à mettre à jour</param>
        void UpdateNature(NatureEnt nature, List<Expression<Func<NatureEnt, object>>> fieldsToUpdate);

        /// <summary>
        /// Retourne une nature précise via son identifiant
        /// </summary>
        /// <param name="natureId">Identifiant d'une nature</param>
        /// <returns>Objet Nature correspondant</returns>
        NatureEnt GetNatureById(int natureId);

        /// <summary>
        /// Retourne l'identifiant de la nature portant le code devise indiqué.
        /// </summary>
        /// <param name="natureCode">Code de la nature à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon null.</returns>
        int? GetNatureIdByCode(string natureCode);

        /// <summary>
        /// Retourne la liste de toutes les natures à l'exception des supprimées
        /// </summary>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> GetNatureList();

        /// <summary>
        /// Retourne la liste de toutes les natures avec les supprimées.
        /// </summary>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> GetNatureListAll();

        /// <summary>
        /// Retourne la liste de toutes les natures d'une société donnée
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> GetNatureBySocieteId(int societeId);

        /// <summary>
        /// Retourne la liste des natures d'une société donnée
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> GetNatureActiveBySocieteId(int societeId);

        /// <summary>
        /// Permet de récupérer une nature.
        /// </summary>
        /// <param name="code">Le code.</param>
        /// <param name="societeId">L'identifiant de la société.</param>
        /// <returns>Une nature.</returns>
        NatureEnt GetNatureActive(string code, int societeId);

        /// <summary>
        /// Méthode vérifiant l'existence d'une nature via son code.
        /// </summary>
        /// <param name="natureId">Identifiant courant</param>
        /// <param name="natureCode">Code Nature</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne vrai si la nature existe, faux sinon</returns>
        bool IsNatureExistsByCode(int natureId, string natureCode, int societeId);

        /// <summary>
        /// Retourne une liste de natures filtrées selon des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans les natures</param>
        /// <param name="filters">Filtres de recherche à prendre en compte</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> SearchNatureWithFilters(string text, SearchCriteriaEnt<NatureEnt> filters);

        /// <summary>
        /// Méthod permettant de supprimer physiquement une nature
        /// </summary>
        /// <param name="natureEnt">La nature à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        bool DeleteNatureById(NatureEnt natureEnt);

        /// <summary>
        /// Permet l'initialisation d'une nouvelle instance de nature.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Nouvelle instance de nature</returns>
        NatureEnt GetNewNature(int societeId);

        /// <summary>
        /// Permet de récupérer les champs de recherche lié à une nature.
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'une nature</returns>
        SearchCriteriaEnt<NatureEnt> GetDefaultFilter();

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);

        /// <summary>
        /// Retourne une nature précise via son identifiant et le code société comptabilité
        /// </summary>
        /// <param name="code">Code Nature</param>
        /// <param name="codeSocieteCompta">Code société comptable (code anael)</param>
        /// <returns>Objet Nature correspondant</returns>
        NatureEnt GetNature(string code, string codeSocieteCompta);

        /// <summary>
        /// Retourne la liste des natures qui ne possèdent pas de famille.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des natures qui ne possèdent pas de famille</returns>
        IEnumerable<NatureEnt> GetListNaturesWithoutFamille(int societeId);

        /// <summary>
        /// Retourne une nature en fonction de sont code de sa société
        /// </summary>
        /// <param name="code">Code de la nature</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns><see cref="NatureEnt"/></returns>
        NatureEnt GetNature(string code, int societeId);

        /// <summary>
        /// Retourne une liste de nature en fonction de leurs identifiants et d'une societé
        /// </summary>
        /// <param name="natureIds">Identifiants des natures</param>
        /// <param name="societeIds">Liste d'identifiant de la société</param>
        /// <returns>Une liste de <see cref="NatureEnt"/></returns>
        IReadOnlyList<NatureEnt> GetNatures(List<int> natureIds, List<int> societeIds);

        /// <summary>
        /// Retourne une liste de nature en fonction de leurs code et d'une societé
        /// </summary>
        /// <param name="codeNatures">Identifiants des natures</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Une liste de <see cref="NatureEnt"/></returns>
        IReadOnlyList<NatureEnt> GetNatures(List<string> codeNatures, List<int> societeIds);

        /// <summary>
        /// Retourne la liste des code natures pour une liste de code pour une société donnée
        /// </summary>
        /// <param name="codes">Liste de code</param>
        /// <param name="societeIds">Liste d'identifiant de societe</param>
        /// <returns>Liste de <see cref="NatureFamilleOdModel"/></returns>
        IReadOnlyList<NatureFamilleOdModel> GetCodeNatureAndFamilliesOD(List<string> codeNatures, List<int> societeIds);

        /// <summary>
        /// Mets à jour une liste de natures
        /// </summary>
        /// <param name="natures"><see cref="NatureFamilleOdModel" /></param>
        void UpdateNatures(List<NatureFamilleOdModel> natures);

        /// <summary>
        /// Retourne la liste de toutes les natures actives d'une société donnée
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureFamilleOdModel> GetNatureActiveFamilleOds(int societeId);

        /// <summary>
        /// Retourne la liste de toutes les natures pour une ressource donnée
        /// </summary>
        /// <param name="ressourceId">Identifient de la ressource</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> GetNatureListByRessourceId(int ressourceId);
    }
}
