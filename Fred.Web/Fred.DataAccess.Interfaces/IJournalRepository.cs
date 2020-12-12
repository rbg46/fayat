using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Journal;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données pour les journaux.
    /// </summary>
    public interface IJournalRepository : IRepository<JournalEnt>
    {
        /// <summary>
        ///   Retourne la liste de tout les journaux
        /// </summary>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        IEnumerable<JournalEnt> GetAllJournal();

        /// <summary>
        ///   Retourne la liste des journaux pour un id de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        IEnumerable<JournalEnt> GetJournaux(int societeId);

        /// <summary>
        /// Retourne la liste des journaux actifs pour un id de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        IReadOnlyList<JournalEnt> GetJournauxActifs(int societeId);

        /// <summary>
        ///   Retourne la liste des journaux pour un id de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="journalId">Identifiant unique du journal</param>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        JournalEnt GetLogImportBySocieteIdAndIdJournal(int societeId, int journalId);

        /// <summary>
        ///   Retourne la liste des journals à importer par code société.
        /// </summary>
        /// <param name="codeSociete">Code de la société</param>
        /// <returns>Liste des journals à importer.</returns>
        IEnumerable<JournalEnt> GetListJournalToImporFactureByCodeSociete(string codeSociete);

        /// <summary>
        ///   Retourne la liste des journaux pour un code de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        IEnumerable<JournalEnt> GetLogImportBySocieteCode(string societeId);

        /// <summary>
        ///   Retourne le journal dont le code est passé en paramètre
        /// </summary>
        /// <param name="code">Code du journal</param>
        /// <returns>Un journal</returns>
        JournalEnt GetJournalByCode(string code);

        /// <summary>
        ///   Retourne l'identifiant du journal portant le code journal indiqué.
        /// </summary>
        /// <param name="code">Code du journal dont l'identifiant est à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon nulle.</returns>
        int? GetJournalIdByCode(string code);

        /// <summary>
        ///   Retourne le journal dont l'id a été passé en paramètre
        /// </summary>
        /// <param name="id">Identifiant unique du journal</param>
        /// <returns>Un journal</returns>
        JournalEnt GetLogImportById(int id);

        /// <summary>
        /// Retourne la liste des journaux correspondant aux filtres spécifiés
        /// </summary>
        /// <param name="predicate">Conditions à vérifier par les journaux</param>
        /// <returns>Liste des journaux correspondant aux filtres spécifiés</returns>
        IEnumerable<JournalEnt> GetLogListByFilters(Expression<Func<JournalEnt, bool>> predicate);

        /// <summary>
        ///   Insertion en base d'un journal
        /// </summary>
        /// <param name="journal">Le journal à enregistrer</param>
        /// <returns>Retourne l'identifiant unique du journal</returns>
        JournalEnt AddJournal(JournalEnt journal);

        /// <summary>
        ///   Mise à jour d'un journal
        /// </summary>
        /// <param name="journal">Journal à mettre à jour</param>
        /// <returns>Journal mise à jour</returns>
        JournalEnt UpdateJournal(JournalEnt journal);

        /// <summary>
        ///   Supprime un journal
        /// </summary>
        /// <param name="journalId">Identifiant Journal à supprimer</param>    
        void DeleteJournal(int journalId);

        /// <summary>
        ///   Appel à une procédure stockée de vérification de dépendance d'un journal
        /// </summary>
        /// <param name="journalId">Id du journal comptable pour vérifier ses dépendances</param>
        /// <returns>True si le journal comptable est utilisé</returns>
        bool JournalHasDependencies(int journalId);

        /// <summary>
        /// Insère dans la base une liste de JournalEnt de façon transactionnelle.
        /// </summary>
        /// <param name="journauxComptableToInsert">journauxComptableToInsert</param>
        void InsertListByTransaction(List<JournalEnt> journauxComptableToInsert);

        /// <summary>
        /// Retourne la liste des journaux qui ne possèdent pas de famille.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des journaux qui ne possèdent pas de famille</returns>
        IEnumerable<JournalEnt> GetListJournauxWithoutFamille(int societeId);
    }
}
