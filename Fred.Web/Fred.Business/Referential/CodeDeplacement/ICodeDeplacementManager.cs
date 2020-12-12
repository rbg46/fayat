using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Referential;

namespace Fred.Business.Referential.CodeDeplacement
{
    /// <summary>
    ///   Représente un référentiel de données pour les codes déplacement
    /// </summary>
    public interface ICodeDeplacementManager : IManager<CodeDeplacementEnt>
    {
        /// <summary>
        ///   liste des codes déplacement pour mobile
        /// </summary>
        /// <param name="sinceDate">The since date.</param>
        /// <param name="userId">Id utilisateur connecté.</param>
        /// <returns>Liste des codes déplacement pour le mobile.</returns>
        IQueryable<CodeDeplacementEnt> GetAllMobile(DateTime? sinceDate = null, int? userId = null);

        /// <summary>
        ///   Retourne la liste des codesDeplacement.
        /// </summary>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <param name="actif">True pour les actifs, false pour les inactifs, null pour tous.</param>
        /// <returns>La liste des codesDeplacement.</returns>
        IEnumerable<CodeDeplacementEnt> GetCodeDeplacementList(int societeId, bool? actif = null);

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes déplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId"> Identifiant de la société </param>
        /// <param name="text"> Texte recherché dans tous les codes déplacement </param>
        /// <param name="filters"> Filtres de recherche </param>
        /// <returns> Retourne la liste filtré de tous les codes déplacement </returns>
        IEnumerable<CodeDeplacementEnt> SearchCodeDepAllWithFilters(int societeId, string text, SearchCodeDeplacementEnt filters);

        /// <summary>
        ///   Permet de récupérer la liste des codes déplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans les codes déplacement</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré des codes déplacement</returns>
        IEnumerable<CodeDeplacementEnt> SearchCodeDepWithFilters(string text, SearchCodeDeplacementEnt filters);

        /// <summary>
        ///   Méthode de recherche des codes déplacement dans le référentiel
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <returns>Une liste de CodeDeplacementEnt</returns>
        IEnumerable<CodeDeplacementEnt> SearchLight(string text, int page, int pageSize, int? ciId = null, bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null);

        /// <summary>
        ///   Permet de connaître l'existence d'un code déplacement depuis son code.
        /// </summary>
        /// <param name="idCourant">L'Id courant</param>
        /// <param name="code">Le code de déplacement</param>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        bool CodeDeplacementExistForSocieteIdAndCode(int idCourant, string code, int societeId);

        /// <summary>
        ///   Permet de recuperer le code déplacement depuis son code et sa societeId.
        /// </summary>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <param name="code">Le code de déplacement</param>
        /// <returns>Le CodeDeplacement ayant le meme code pour une sociétéId donnée</returns>
        CodeDeplacementEnt GetBySocieteIdAndCode(int societeId, string code);

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de code déplacement.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Nouvelle instance de code déplacement intialisée</returns>
        CodeDeplacementEnt GetNewCodeDeplacement(int societeId);

        /// <summary>
        ///   Ajout un nouveau CodeDeplacement
        /// </summary>
        /// <param name="codeDeplacementEnt"> CodeDeplacement à ajouter</param>
        /// <returns>Le CodeDeplacement ajouté</returns>
        CodeDeplacementEnt AddCodeDeplacement(CodeDeplacementEnt codeDeplacementEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeDeplacement
        /// </summary>
        /// <param name="codeDeplacementEnt">CodeDeplacement à modifier</param>
        /// <returns>Le CodeDeplacementEnt mis a jour</returns>
        CodeDeplacementEnt UpdateCodeDeplacement(CodeDeplacementEnt codeDeplacementEnt);

        /// <summary>
        ///   Supprime un CodeDeplacement
        /// </summary>
        /// <param name="codeDeplacementId">Id du CodeDeplacement à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        bool DeleteCodeDeplacementById(int codeDeplacementId);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un CodeDeplacement
        /// </summary>
        /// <param name="codeDeplacementId">Code deplacement à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(int codeDeplacementId);

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);

        /// <summary>
        ///   Retourne le CodeDeplacement portant le code indiqué.
        /// </summary>
        /// <param name="codeDeplacement">Code déplacement à retrouver.</param>
        /// <returns>Le code déplacement retrouvé, sinon nulle.</returns>
        CodeDeplacementEnt GetCodeDeplacementByCode(string codeDeplacement);
    }
}
