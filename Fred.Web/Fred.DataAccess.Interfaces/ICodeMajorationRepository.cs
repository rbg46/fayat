
using System;
using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les codesMajoration.
    /// </summary>
    public interface ICodeMajorationRepository : IRepository<CodeMajorationEnt>
    {
        /// <summary>
        ///   Retourne la liste des codesMajoration.
        /// </summary>
        /// <param name="recherche">Texte de recherche</param>
        /// <returns>La liste des codesMajoration.</returns>
        IEnumerable<CodeMajorationEnt> GetCodeMajorationList(string recherche = null);

        /// <summary>
        ///   Retourne la liste des codesMajoration pour la synchronisaiton mobile.
        /// </summary>
        /// <param name="recherche">Texte de recherche</param>
        /// <returns>La liste des codesMajoration.</returns>
        IEnumerable<CodeMajorationEnt> GetCodeMajorationListSync(string recherche = null);

        /// <summary>
        ///   Retourne le CodeMajoration portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="codeMajorationId">Identifiant du CodeMajoration à retrouver.</param>
        /// <returns>Le codeMajoration retrouvé, sinon nulle.</returns>
        CodeMajorationEnt GetCodeMajorationById(int codeMajorationId);

        /// <summary>
        ///   Retourne la liste de codes majoration associés à la société + les publics
        /// </summary>
        /// <param name="groupeId">Identifiant de la société associée aux code majoration à retourner</param>
        /// <returns>La liste des codes majoration associés à la société</returns>
        IEnumerable<CodeMajorationEnt> GetCodeMajorationListByGroupeId(int groupeId);

        /// <summary>
        ///   Retourne la liste de codes majoration actifs associés à la société + les publics
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe associé aux code majoration à retourner</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>La liste des codes majoration actifs associés à la société</returns>
        IEnumerable<CodeMajorationEnt> GetActifsCodeMajorationListByGroupeId(int groupeId, int ciId);

        /// <summary>
        ///   Ajout un nouveau CodeMajoration
        /// </summary>
        /// <param name="codeMajorationEnt"> CodeMajoration à ajouter</param>
        /// <returns> L'identifiant du CodeMajoration ajouté</returns>
        int AddCodeMajoration(CodeMajorationEnt codeMajorationEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeMajoration
        /// </summary>
        /// <param name="codeMajorationEnt">CodeMajoration à modifier</param>
        void UpdateCodeMajoration(CodeMajorationEnt codeMajorationEnt);

        /// <summary>
        ///   Supprime un CodeMajoration
        /// </summary>
        /// <param name="id">L'identifiant du CodeMajoration à supprimer</param>
        void DeleteCodeMajorationById(int id);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer un CodeMajoration
        /// </summary>
        /// <param name="codeMajorationEnt">Code majoration à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(CodeMajorationEnt codeMajorationEnt);

        /// <summary>
        ///   Méthode vérifiant l'existence d'une nature via son code pour une société donnée.
        /// </summary>
        /// <param name="idCourant"> id courant</param>
        /// <param name="code">Code Nature</param>
        /// <param name="groupeId">Identifiant de la société</param>
        /// <returns>Retourne vrai si la nature existe, faux sinon</returns>
        bool IsCodeMajorationExistsByCodeInGroupe(int idCourant, string code, int groupeId);

        /// <summary>
        ///   Cherche une liste de CodeMajoration.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des codesMajoration.</param>
        /// <param name="groupeId">L'identifiant du groupe associé au code majoration</param>
        /// <param name="ciId">L'identifiant du CI associé au code majoration</param>
        /// <returns>Une liste de CodeMajoration.</returns>
        IEnumerable<CodeMajorationEnt> SearchCodeMajorations(string text, int groupeId, int ciId);

        /// <summary>
        ///   Retourne la liste de codes majoration actifs associés à la société et à un CI
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe associé aux code majoration à retourner</param>
        /// <param name="ciId">Identifiant du CI associé aux code majoration à retourner</param>
        /// <returns>La liste des codes majoration associés au groupe et à un CI</returns>
        IEnumerable<CodeMajorationEnt> GetActifAllowedCodeMajorationListForCi(int groupeId, int ciId);

        #region Gestion Associations Codes majoration et CI

        /// <summary>
        ///   Retourne la liste complète de CICodeMajorations
        /// </summary>
        /// <returns>Une liste de CICodeMajorations</returns>
        IEnumerable<CICodeMajorationEnt> GetCiCodeMajorationList();

        /// <summary>
        ///   Retourne la liste complète de CICodeMajorations pour la synchronisation mobile.
        /// </summary>
        /// <returns>Une liste de CICodeMajorations</returns>
        IEnumerable<CICodeMajorationEnt> GetCiCodeMajorationListSync();

        /// <summary>
        ///   Cherche une liste d'ID de code majoration en fonction d'un ID de CI
        /// </summary>
        /// <param name="ciId">ID du CI pour lequel on recherche les IDs de codes majoration correspndants</param>
        /// <returns>Une liste d'ID de CodeMajoration.</returns>
        List<int> GetCodeMajorationIdsByCiId(int ciId);

        /// <summary>
        ///   Création d'une nouvelle association CI/Code majoration
        /// </summary>
        /// <param name="codeMaj"> Association CI/Code majoration à ajouter</param>
        /// <returns>CICodeMajoration ajouté ou mis à jour</returns>
        CICodeMajorationEnt AddOrUpdateCICodeMajoration(CICodeMajorationEnt codeMaj);

        /// <summary>
        ///   Création et/ou mise à jour d'une liste d'associations CI/Code majoration
        /// </summary>    
        /// <param name="ciCodeMajorationList"> Association CI/Code majoration à ajouter</param>
        /// <returns>Liste des CICodeMajoration ajouté et/ou mis à jour</returns>
        IEnumerable<CICodeMajorationEnt> AddOrUpdateCICodeMajorationList(IEnumerable<CICodeMajorationEnt> ciCodeMajorationList);

        /// <summary>
        ///   Supprime une association entre un code majoration et un CI
        /// </summary>
        /// <param name="codeMajId">L'identifiant du CodeMajoration à supprimer</param>
        /// <param name="ciId">L'identifiant du ciId à supprimer</param>
        void DelteCICodeMajoration(int codeMajId, int ciId);

        /// <summary>
        /// Permet de récupérer une liste de Code Majoration d'un groupe d'une société depuis une date
        /// </summary>
        /// <param name="societeId">L'identifant de la société</param>
        /// <param name="lastModification">La date de modification</param>
        /// <returns>Une liste de tâche</returns>
        IEnumerable<CodeMajorationEnt> GetCodeMajorations(int? societeId, DateTime lastModification);

        /// <summary>
        ///   Retourne la liste de codes majoration associés à la société
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe associé aux code majoration à retourner</param>
        /// <returns>La liste des codes majoration associés à la société</returns>
        IEnumerable<CodeMajorationEnt> GetCodeMajorationListByGroupeIdAndIsHeurNuit(int groupeId);

        #endregion

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);
    }
}