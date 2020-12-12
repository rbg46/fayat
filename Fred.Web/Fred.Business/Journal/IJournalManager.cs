using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Journal;
using Fred.Entities.Referential;
using Fred.Web.Models.Journal;
using Fred.Web.Shared.Models.Journal;

namespace Fred.Business.Journal
{
    /// <summary>
    /// Interface du gestionnaire des journals.
    /// </summary>
    public interface IJournalManager
    {
        /// <summary>
        /// Récupération d'un journal des FAR
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>    
        /// <returns>Journal des FAR</returns>
        JournalEnt GetJournalFar(int societeId);

        /// <summary>
        /// Retourne la liste des journals à importer par code société.
        /// </summary>
        /// <param name="codeSociete">Code de la société</param>
        /// <returns>Liste des journals à importer.</returns>
        IEnumerable<JournalEnt> GetListJournalToImporFactureByCodeSociete(string codeSociete);

        /// <summary>
        /// Retourne le journal dont le code est passé en paramètre
        /// </summary>
        /// <param name="code">Code du journal</param>
        /// <returns>Un journal</returns>
        JournalEnt GetJournalByCode(string code);

        /// <summary>
        /// Retourne l'identifiant du journal portant le code journal indiqué.
        /// </summary>
        /// <param name="code">Code du journal dont l'identifiant est à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon nulle.</returns>
        int? GetJournalIdByCode(string code);

        /// <summary>
        /// Récupère la liste des journaux en fonction d'une société
        /// </summary>
        /// <param name="societeId">Identifiant d'une société</param>
        /// <returns>Liste des journaux d'une société donnée</returns>
        IEnumerable<JournalEnt> GetJournalList(int societeId);

        /// <summary>
        /// Récupère la liste des journaux en fonction de filtres
        /// </summary>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Liste des journaux</returns>
        IEnumerable<JournalEnt> GetFilteredJournalList(SearchCriteriaEnt<JournalEnt> filters);

        /// <summary>
        /// Ajoute un journal
        /// </summary>
        /// <param name="journal">Journal à ajouter</param>
        /// <returns>Nouveau journal</returns>
        JournalEnt AddJournal(JournalEnt journal);

        /// <summary>
        /// Mise à jour d'un journal
        /// </summary>
        /// <param name="journal">Journal à mettre à jour</param>
        /// <returns>Journal mis à jour</returns>
        JournalEnt UpdateJournal(JournalEnt journal);

        /// <summary>
        /// Supprime un journal s'il n'est pas utilisé
        /// </summary>
        /// <param name="journalId">Identifiant Journal à supprimer</param>    
        void DeleteJournal(int journalId);

        /// <summary>
        /// Gestion des journaux : Ajout/Modification/Supression
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="journalList">Liste de journal à traiter</param>
        /// <returns>Liste de journal traitée</returns>
        IEnumerable<JournalEnt> ManageJournalList(int societeId, IEnumerable<JournalEnt> journalList);

        /// <summary>
        /// Retourne un journal pour un id de societe passé en paramètre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="journalId">Identifiant unique du journal</param>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        JournalEnt GetJournal(int societeId, int journalId);

        /// <summary>
        /// Indique si le journal comptable est utilisé, en testant ses dépendances
        /// </summary>
        /// <param name="journalId">Identifiant du journal comptable</param>
        /// <returns>True si le journal comptable est utilisé</returns>
        bool IsAlreadyUsed(int journalId);

        /// <summary>
        /// Retourne la liste des journaux comptable pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns><see cref="JournalFamilleODModel"/></returns>
        List<JournalFamilleODModel> GetJournaux(int societeId);

        /// <summary>
        /// Retourne la liste des journaux comptable actifs pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns><see cref="JournalFamilleODModel" /></returns>
        List<JournalFamilleODModel> GetJournauxActifs(int societeId);

        /// <summary>
        /// Vérifie la validité et enregistre une liste de Journaux Comptable importé depuis ANAËL
        /// </summary>
        /// <param name="journauxComptableFromANAEL">Liste des journaux comptable dont il faut vérifier la validité</param>
        /// <param name="societeId">Societé des journaux comptable</param>
        void ManageImportedJournauxComptables(IList<JournalEnt> journauxComptableFromANAEL, int societeId);

        /// <summary>
        /// Mets à jour une liste de journaux comptable
        /// </summary>
        /// <param name="journaux"><see cref="JournalFamilleODModel"/></param>
        void UpdateJournaux(List<JournalFamilleODModel> journaux);

        /// <summary>
        /// Insère dans la base une liste de JournalEnt de façon transactionnelle.
        /// </summary>
        /// <param name="journauxComptableToInsert">journauxComptableToInsert</param>
        void InsertListByTransaction(List<JournalEnt> journauxComptableToInsert);

        /// <summary>
        /// Permet la mise à jour de certain champs du model <see cref="JournalModel"/>
        /// </summary>
        /// <param name="journal">Model <see cref="JournalModel"/></param>
        /// <param name="fieldsToUpdate">Liste des champs à mettre à jour</param>
        /// <returns>Journal mis à jour</returns>
        JournalModel UpdateJournal(JournalModel journal, List<Expression<Func<JournalEnt, object>>> fieldsToUpdate);

        /// <summary>
        /// Retourne la liste des journaux qui ne possèdent pas de famille.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des journaux qui ne possèdent pas de famille</returns>
        IEnumerable<JournalEnt> GetListJournauxWithoutFamille(int societeId);
    }
}
