using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les codes d'absence
    /// </summary>
    public interface ICodeAbsenceRepository : IRepository<CodeAbsenceEnt>
    {
        /// <summary>
        ///   Retourne la liste de tous les codes d'absence.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        IEnumerable<CodeAbsenceEnt> GetCodeAbsListAll();

        /// <summary>
        ///   Retourne la liste de tous les codes d'absence pour la synchronisation mobile.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        IEnumerable<CodeAbsenceEnt> GetCodeAbsListAllSync();

        /// <summary>
        ///   La liste de tous les codes d'absence.
        /// </summary>
        /// <returns>Renvoie la liste de des codes d'absence active</returns>
        IEnumerable<CodeAbsenceEnt> GetCodeAbsList();

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeAbsence
        /// </summary>
        /// <param name="codeAbs">Code absence à modifier</param>
        void UpdateCodeAbsence(CodeAbsenceEnt codeAbs);

        /// <summary>
        ///   Ajout d'une code d'absence
        /// </summary>
        /// <param name="codeAbs">Code d'absence à ajouter</param>
        /// <returns>L'identifiant du code d'absence ajouté</returns>
        int AddCodeAbsence(CodeAbsenceEnt codeAbs);

        /// <summary>
        ///   Supprime un code d'absence
        /// </summary>
        /// <param name="id">L'identifiant du code d'absence à supprimer</param>
        void DeleteCodeAbsenceById(int id);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un code d'absence
        /// </summary>
        /// <param name="codeAbs">Code d'absence à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(CodeAbsenceEnt codeAbs);

        /// <summary>
        ///   Code d'absence via l'id
        /// </summary>
        /// <param name="id">Id du code d'absence</param>
        /// <returns>Renvoie un code d'absence</returns>
        CodeAbsenceEnt GetCodeAbsenceById(int id);

        /// <summary>
        ///   Retourne le codeAbsence correspondant au code
        /// </summary>
        /// <param name="code">Le code de l'absence</param>
        /// <returns>Renvoie un code d'absence</returns>
        CodeAbsenceEnt GetCodeAbsenceByCode(string code);

        /// <summary>
        ///   Code d'absence via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <returns>Renvoie un code d'absence</returns>
        IEnumerable<CodeAbsenceEnt> GetCodeAbsenceBySocieteId(int societeId);

        /// <summary>
        ///   Import des codes absences automatiques depuis la Holding
        /// </summary>
        /// <param name="holdingId"> Id du Holding</param>
        /// <param name="idNewSociete"> Id du de la nouvelle société</param>
        /// <returns>Renvoie un int</returns>
        int ImportCodeAbsFromHolding(int holdingId, int idNewSociete);

        /// <summary>
        ///   Permet de récupérer la liste des codes absences en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur les codes absences</param>
        /// <returns>Retourne la liste filtrée des codes absences</returns>
        IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceWithFilters(Expression<Func<CodeAbsenceEnt, bool>> predicate);

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes absences en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les codes absences</param>
        /// <returns>Retourne la liste filtrée de tous les codes absences</returns>
        IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceAllWithFilters(Expression<Func<CodeAbsenceEnt, bool>> predicate);

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes absences en fonction des critères de recherche par société.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les codes absences</param>
        /// <param name="societeId">Id de la societe</param>
        /// <returns>Retourne la liste filtrée de tous les codes absences</returns>
        IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceAllBySocieteIdWithFilters(Expression<Func<CodeAbsenceEnt, bool>> predicate, int societeId);

        /// <summary>
        ///   Permet de connaître l'existence d'une société depuis son code.
        /// </summary>
        /// <param name="idCourant">id courant</param>
        /// <param name="codeCodeAbsence">code CodeAbsence</param>
        /// <param name="societeId">Id societe</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        bool IsCodeAbsenceExistsByCode(int idCourant, string codeCodeAbsence, int societeId);

        /// <summary>
        ///   Ligne de recherche
        /// </summary>
        /// <param name="text">Le text recherché</param>
        /// <param name="societeId">ID de la société</param>
        /// <returns>Renvoie une liste</returns>
        IEnumerable<CodeAbsenceEnt> SearchLight(string text, int societeId);


        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);
    }
}