using FluentValidation;
using Fred.Business.ObjectifFlash.Models;
using Fred.Entities.ObjectifFlash;
using Fred.Entities.ObjectifFlash.Search;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.ObjectifFlash
{
    /// <summary>
    ///   Gestionnaire des ObjectifFlashs
    /// </summary>
    public interface IObjectifFlashManager : IManager<ObjectifFlashEnt>
    {
        /// <summary>
        /// Récupère la liste des objectifs flashs filtrés en fonction des critères
        /// </summary>
        /// <param name="filter">filtres</param>
        /// <param name="page">page index</param>
        /// <param name="pageSize">nombre d'items par page</param>
        /// <returns>Liste des Objectif Flashs</returns>
        /// <exception cref="ValidationException">Erreur levée si la date de début est postérieure à la date de fin</exception>
        Task<SearchObjectifFlashListWithFilterResult> SearchObjectifFlashListWithFilterAsync(SearchObjectifFlashEnt filter, int page = 1, int pageSize = 20);

        /// <summary>
        /// Retourne l'objectif flash portant l'identifiant unique indiqué avec taches.
        /// </summary>
        /// <param name="objectifFlashId">Identifiant de l'objectif flash à retrouver.</param>
        /// <returns>L'objectif flash retrouvée, sinon null.</returns>
        ObjectifFlashEnt GetObjectifFlashWithTachesById(int objectifFlashId);

        /// <summary>
        /// Retourne l'objectif flash portant l'identifiant unique indiqué avec taches et realisations.
        /// </summary>
        /// <param name="objectifFlashId">Identifiant de l'objectif flash à retrouver.</param>
        /// <param name="dateDebut">date de début de réalisations</param>
        /// <param name="dateFin">date de fin de réalisations</param>
        /// <returns>L'objectif flash retrouvée, sinon null.</returns>
        ObjectifFlashEnt GetObjectifFlashWithRealisationsById(int objectifFlashId, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        ///   Retourne un nouvel Objectif flash initialisé.
        /// </summary>
        /// <returns>Un nouvel Objectif flash</returns>
        ObjectifFlashEnt GetNewObjectifFlash();

        /// <summary>
        ///   Ajout d'un nouveau Objectif flash.
        /// </summary>
        /// <param name="objectifFlash">Objectif Flash à ajouter.</param>
        /// <returns>Commande ajoutée</returns>
        ObjectifFlashEnt AddObjectifFlash(ObjectifFlashEnt objectifFlash);

        /// <summary>
        ///   Sauvegarde les modifications d'un Objectif flash.
        /// </summary>
        /// <param name="objectifFlash">Objectif Flash à modifier</param>
        /// <returns>Commande mise à jour</returns>
        ObjectifFlashEnt UpdateObjectifFlash(ObjectifFlashEnt objectifFlash);

        /// <summary>
        /// Activation d'un objectif flash
        /// </summary>
        /// <param name="objectifFlashId">identifiant d'objectif flash</param>
        /// <returns>true si activé</returns>
        bool ActivateObjectifFlash(int objectifFlashId);

        /// <summary>
        /// Duplication d'un objectif flash
        /// </summary>
        /// <param name="objectifFlashId">identifiant d'objectif flash</param>
        /// <param name="dateDebut">date de début</param>
        /// <returns>identifiant de l'objectif flash dupliqué</returns>
        int DuplicateObjectifFlash(int objectifFlashId, DateTime dateDebut);

        /// <summary>
        ///   Supprime logiquement un Objectif flash.
        /// </summary>
        /// <param name="objectifFlashId">L'identifiant du Objectif flash à supprimer / clôturer.</param>
        void DeleteObjectifFlashById(int objectifFlashId);

        /// <summary>
        ///   Retourne une nouvelle recherche d'Objectif flash initialisée.
        /// </summary>
        /// <returns>Une nouvelle recherche d'Objectif flash</returns>
        SearchObjectifFlashEnt GetNewFilter();

        /// <summary>
        /// Retourne un Objectif flash initialisé avec une nouvelle journalisation ventilée.
        /// </summary>
        /// <param name="objectifFlashEnt">objectifFlashEnt</param>
        /// <returns>ObjectifFlashEnt</returns>
        ObjectifFlashEnt GetNewJournalisation(ObjectifFlashEnt objectifFlashEnt);

        /// <summary>
        /// Retourne un Objectif flash avec une journalisation reportée à partir de la date de début.
        /// </summary>
        /// <param name="objectifFlash">objectifFlashEnt</param>
        /// <param name="dateDebut">date de début de journalisation</param>
        /// <returns>ObjectifFlashEnt</returns>
        ObjectifFlashEnt GetReportJournalisation(ObjectifFlashEnt objectifFlash, DateTime dateDebut);

        /// <summary>
        /// Retourne une liste de dépenses sous forme de BilanFlashDepenseModel
        /// </summary>
        /// <param name="objectifFlashList">Liste d'objectifs flash</param>
        /// <returns>Liste de dépenses</returns>
        Task<List<ObjectifFlashDepenseModel>> GetObjectifFlashListDepensesAsync(IEnumerable<ObjectifFlashEnt> objectifFlashList);

        /// <summary>
        /// Retourne la liste des dépenses de l'objectifFlash comprises entre la date de début et la date de fin sous forme de BilanFlashDepenseModel
        /// </summary>
        /// <param name="objectifFlash">objectifs flash</param>
        /// <param name="dateDebut">date de debut</param>
        /// <param name="dateFin">date de fin</param>
        /// <returns>Liste de dépenses</returns>
        Task<List<ObjectifFlashDepenseModel>> GetObjectifFlashDepensesAsync(ObjectifFlashEnt objectifFlash, DateTime? dateDebut, DateTime? dateFin);
    }
}
