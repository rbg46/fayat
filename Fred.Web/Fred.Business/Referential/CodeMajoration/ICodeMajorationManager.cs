using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Représente un référentiel de données pour les codesMajoration.
    /// </summary>
    public interface ICodeMajorationManager : IManager<CodeMajorationEnt>
    {
        /// <summary>
        ///   Retourne la liste des codesMajoration.
        /// </summary>
        /// <param name="utilisateur">utilisater pour lequel récupérer le code Majoration</param>
        /// <param name="recherche">critère textuel de recherche</param>
        /// <returns>La liste des codesMajoration.</returns>
        IEnumerable<CodeMajorationEnt> GetCodeMajorationList(UtilisateurEnt utilisateur = null, string recherche = null);

        /// <summary>
        ///   Retourne la liste des codesMajoration pour la synchronisation mobile.
        /// </summary>
        /// <param name="utilisateur">utilisater pour lequel récupérer le code Majoration</param>
        /// <param name="recherche">critère textuel de recherche</param>
        /// <returns>La liste des codesMajoration.</returns>
        IEnumerable<CodeMajorationEnt> GetCodeMajorationListSync(UtilisateurEnt utilisateur = null, string recherche = null);

        /// <summary>
        ///   Retourne le CodeMajoration portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="codeMajorationID">Identifiant du CodeMajoration à retrouver.</param>
        /// <returns>Le CodeMajoration retrouvé, sinon nulle.</returns>
        CodeMajorationEnt GetCodeMajorationById(int codeMajorationID);

        /// <summary>
        ///   Retourne la liste de codes majoration associés à la société + les publics
        /// </summary>
        /// <param name="groupeId">Identifiant de la société associée aux code majoration à retourner</param>
        /// <returns>La liste des codes majoration associés à la société</returns>
        IEnumerable<CodeMajorationEnt> GetCodeMajorationListByGroupeId(int groupeId);

        /// <summary>
        ///   Retourne la liste de codes majoration actifs associés à la société + les publics
        /// </summary>
        /// <param name="societeId">Identifiant de la société associée aux code majoration à retourner</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns> La liste des codes majoration actifs associés à la société </returns>
        IEnumerable<CodeMajorationEnt> GetActifsCodeMajorationListBySocieteId(int societeId, int ciId);

        /// <summary>
        ///   Ajout un nouveau CodeMajoration
        /// </summary>
        /// <param name="codeMajorationEnt"> CodeMajoration à ajouter</param>
        /// <param name="createur">createur du code majoration</param>
        /// <returns> L'identifiant du CodeMajoration ajouté</returns>
        int AddCodeMajoration(CodeMajorationEnt codeMajorationEnt, UtilisateurEnt createur);

        /// <summary>
        ///   Ajout ou mise à jour ou suppression d'une liste d'associations CI/Code majoration
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ciCodeMajorationList">Liste des relations CI/CodeMajoration à ajouter ou mettre à jour</param> 
        /// <returns>Liste des CICodeMajoration crée et/ou mis à jour</returns>
        IEnumerable<CICodeMajorationEnt> ManageCICodeMajoration(int ciId, IEnumerable<CICodeMajorationEnt> ciCodeMajorationList);

        /// <summary>
        ///   Cherche une liste d'ID de code majoration en fonction d'un ID de CI
        /// </summary>
        /// <param name="ciId">ID du CI pour lequel on recherche les IDs de codes majoration correspndants</param>
        /// <returns>Une liste d'ID de CodeMajoration.</returns>
        List<int> GetCodeMajorationIdsByCiId(int ciId);

        /// <summary>
        ///   Ajout un nouvelle association CI/Code majoration
        /// </summary>
        /// <param name="codesMajorationIds"> Codes majoration à associer</param>
        /// <param name="ciId"> CI à associer</param>
        void AddCiCodesMajoration(int codesMajorationIds, int ciId);

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeMajoration
        /// </summary>
        /// <param name="codeMajorationEnt">CodeMajoration à modifier</param>
        void UpdateCodeMajoration(CodeMajorationEnt codeMajorationEnt);

        /// <summary>
        ///   Supprime un CodeMajoration
        /// </summary>
        /// <param name="codeMajorationEnt">CodeMajoration à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        bool DeleteCodeMajorationById(CodeMajorationEnt codeMajorationEnt);

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de code majoration.
        /// </summary>
        /// <returns>Nouvelle instance de code majoration intialisée</returns>
        CodeMajorationEnt GetNewCodeMajoration();

        /// <summary>
        ///   Méthode vérifiant l'existence d'un code majoration via son code.
        /// </summary>
        /// <param name="idCourant"> id courant</param>
        /// <param name="code">Code du code majoration</param>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Retourne vrai si la nature existe, faux sinon</returns>
        bool IsCodeMajorationExistsByCodeInGroupe(int idCourant, string code, int groupeId);

        /// <summary>
        ///   Supprime un CICodeMajoration à partir des ses IDs Code Majoration et CI
        /// </summary>
        /// <param name="codeMajId">ID du code majoration référencé</param>
        /// <param name="ciId">ID du CI référence</param>
        void DeleteCICodeMajorationById(int codeMajId, int ciId);

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
        ///   Moteur de recherche des codes majoration pour picklist
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="groupeId">identifiant du groupe</param>
        /// <param name="ciId">Identifiant CI</param>
        /// <param name="isHeureNuit">Filter pour ETAM/IAC ou ouvrier horarire codes</param>
        /// <returns>Retourne une liste d'items de référentiel</returns>
        IEnumerable<CodeMajorationEnt> SearchLight(string text, int page, int pageSize, int? groupeId, int? ciId, bool? isHeureNuit, bool? isOuvrier = null, bool? isETAM = null, bool? isCadre = null);

        /// <summary>
        /// Permet de récupérer les Code Majoration à synchroniser.
        /// </summary>
        /// <param name="lastModification">La date de modification</param>
        /// <returns>Une liste de code majoration</returns>
        IEnumerable<CodeMajorationEnt> GetSyncCodeMajorations(DateTime lastModification = default(DateTime));

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);

        /// <summary>
        ///   Retourne la liste de codes majoration associés à la société
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe associé aux code majoration à retourner</param>
        /// <returns>La liste des codes majoration associés à la société</returns>
        IEnumerable<CodeMajorationEnt> GetCodeMajorationListByGroupeIdAndIsHeurNuit(int groupeId);
    }
}
