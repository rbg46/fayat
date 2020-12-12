
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les codes déplacement
    /// </summary>
    public interface ICodeDeplacementRepository : IRepository<CodeDeplacementEnt>
    {
        /// <summary>
        ///   Retourne la liste des codesDeplacement.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des codesDeplacement.</returns>
        IEnumerable<CodeDeplacementEnt> GetCodeDeplacementList(int societeId);

        /// <summary>
        ///   Retourne le CodeDeplacement portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="codeDeplacementId">Identifiant du CodeDeplacement à retrouver.</param>
        /// <returns>Le CodeDeplacement retrouvé, sinon nulle.</returns>
        CodeDeplacementEnt GetCodeDeplacementById(int codeDeplacementId);

        /// <summary>
        ///   Retourne le CodeDeplacement portant le code indiqué.
        /// </summary>
        /// <param name="codeDeplacement">Code déplacement à retrouver.</param>
        /// <returns>Le code déplacement retrouvé, sinon nulle.</returns>
        CodeDeplacementEnt GetCodeDeplacementByCode(string codeDeplacement);

        /// <summary>
        ///   Permet de connaître l'existence d'un code déplacement depuis son code.
        /// </summary>
        /// <param name="codeDeplacementId">Identifiant du code déplacement courant</param>
        /// <param name="code">Code de deplacement à vérifier</param>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon.</returns>
        bool CodeDeplacementExistForSocieteIdAndCode(int codeDeplacementId, string code, int societeId);

        /// <summary>
        ///   Méthode qui retourne un codeDeplacement pour un societeId et un code donné.
        /// </summary>
        /// <returns>Le codeDeplacement pour un code et un societeId</returns>
        /// <param name="societeId">Id de la société</param>
        /// <param name="code">Code a rechercher</param>
        CodeDeplacementEnt GetBySocieteIdAndCode(int societeId, string code);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un CodeDeplacement
        /// </summary>
        /// <param name="codeDeplacementId">ID du Code deplacement à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(int codeDeplacementId);

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes déplacement en fonction des critères de recherche.
        /// </summary>
        /// <returns>Retourne la liste filtrée de tous les codes déplacement</returns>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="predicate">Prédicat de recherche de codes déplacement</param>
        IEnumerable<CodeDeplacementEnt> SearchCodeDepAllWithFilters(int societeId, Expression<Func<CodeDeplacementEnt, bool>> predicate);

        /// <summary>
        ///   Permet de récupérer la liste des codes déplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Prédicat de recherche de codes déplacement</param>
        /// <returns>Retourne la liste filtrée des codes déplacement</returns>
        IEnumerable<CodeDeplacementEnt> SearchCodeDepWithFilters(Expression<Func<CodeDeplacementEnt, bool>> predicate);

        /// <summary>
        ///   Moteur de recherche pour la picklist
        /// </summary>
        /// <param name="text">Texte de la recherche</param>
        /// <param name="societeId">ID de la société associée</param>
        /// <returns>Une liste de codes déplacement</returns>
        IEnumerable<CodeDeplacementEnt> SearchLight(string text, int societeId);

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);
    }
}