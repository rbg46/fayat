using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces
{
    public interface INatureRepository : IRepository<NatureEnt>
    {
        void AddNature(NatureEnt nature);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer une nature
        /// </summary>
        /// <param name="natureEnt">La nature à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(NatureEnt natureEnt);

        /// <summary>
        ///   Retourne une nature précise via son identifiant
        /// </summary>
        /// <param name="natureId">Identifiant d'une nature</param>
        /// <returns>Objet Nature correspondant</returns>
        NatureEnt GetNatureById(int natureId);

        /// <summary>
        ///   Retourne l'identifiant de la nature portant le code devise indiqué.
        /// </summary>
        /// <param name="natureCode">Code de la nature à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon null.</returns>
        int? GetNatureIdByCode(string natureCode);

        /// <summary>
        ///   Retourne la liste de toutes les natures à l'exception des supprimées
        /// </summary>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> GetNatureList();

        /// <summary>
        ///   Retourne la liste de toutes les natures avec les supprimées.
        /// </summary>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> GetNatureListAll();

        /// <summary>
        ///   Retourne la liste de toutes les natures d'une société donnée
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> GetNatureBySocieteId(int societeId);

        /// <summary>
        ///   Méthode vérifiant l'existence d'une nature via son code.
        /// </summary>
        /// <param name="natureId">Identifiant courant</param>
        /// <param name="natureCode">Code Nature</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne vrai si la nature existe, faux sinon</returns>
        bool IsNatureExistsByCode(int natureId, string natureCode, int societeId);

        /// <summary>
        ///   Retourne une liste de natures filtrées selon des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur les natures</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> SearchNatureWithFilters(Expression<Func<NatureEnt, bool>> predicate);

        /// <summary>
        ///   Retourne une liste de natures dont le code ou le libellé contient un texte donné
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> SearchLight(string text);

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);

        /// <summary>
        /// Retourne la liste des natures qui ne possèdent pas de famille.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des natures qui ne possèdent pas de famille</returns>
        IEnumerable<NatureEnt> GetListNaturesWithoutFamille(int societeId);

        /// <summary>
        /// Retourne la liste des natures pour un code societe et une liste de code nature
        /// </summary>
        /// <param name="codes">Liste de code nature</param>
        /// <param name="societeIds">Liste d'identifiant de la société</param>
        /// <returns>Liste de <see cref="NatureEnt" /></returns>
        IEnumerable<NatureEnt> GetNatureList(List<string> codes, List<int> societeIds);


        /// <summary>
        /// Retourne la liste de toutes les natures pour une ressource donnée
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <returns>Liste d'objet Nature correspondant</returns>
        IEnumerable<NatureEnt> GetNatureListByRessourceId(int ressourceId);

        /// <summary>
        /// Retourne une nature en fonction de son code de sa société
        /// </summary>
        /// <param name="codeNatures">Code de la nature</param>
        /// <param name="societeId">Identifiant du CI</param>
        /// <returns>Liste de <see cref="NatureEnt" /></returns>
        IEnumerable<NatureEnt> GetNatures(List<string> codeNatures, List<int> societeIds);
    }
}
