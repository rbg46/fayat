using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Journal;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.Web.Models.Journal;
using Fred.Web.Shared.Models.Journal;
using static Fred.Entities.Constantes;

namespace Fred.Business.Journal
{
    public class JournalManager : Manager<JournalEnt, IJournalRepository>, IJournalManager
    {
        private readonly IUtilisateurManager userManager;
        private readonly IMapper mapper;

        public JournalManager(
            IUnitOfWork uow,
            IJournalRepository journalRepository,
            IJournalValidator validator,
            IUtilisateurManager userManager,
            IMapper mapper)
            : base(uow, journalRepository, validator)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Retourne la liste des journals à importer par code société.
        /// </summary>
        /// <param name="codeSociete">Code de la société</param>
        /// <returns>Liste des journals à importer.</returns>
        public IEnumerable<JournalEnt> GetListJournalToImporFactureByCodeSociete(string codeSociete)
        {
            return Repository.GetListJournalToImporFactureByCodeSociete(codeSociete);
        }

        /// <summary>
        /// Récupération d'un journal des FAR
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>    
        /// <returns>Journal des FAR</returns>
        public JournalEnt GetJournalFar(int societeId)
        {
            return Repository.GetJournaux(societeId).FirstOrDefault(x => x.TypeJournal.Equals(TypeJournal.Far));
        }

        /// <summary>
        /// Retourne le journal dont le code est passé en paramètre
        /// </summary>
        /// <param name="code">Code du journal</param>
        /// <returns>Un journal</returns>
        public JournalEnt GetJournalByCode(string code)
        {
            return Repository.GetJournalByCode(code);
        }

        /// <summary>
        /// Retourne l'identifiant du journal portant le code journal indiqué.
        /// </summary>
        /// <param name="code">Code du journal dont l'identifiant est à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon nulle.</returns>
        public int? GetJournalIdByCode(string code)
        {
            return Repository.GetJournalIdByCode(code);
        }

        /// <summary>
        /// Récupère la liste des journaux en fonction d'une société
        /// </summary>
        /// <param name="societeId">Identifiant d'une société</param>
        /// <returns>Liste des journaux d'une société donnée</returns>
        public IEnumerable<JournalEnt> GetJournalList(int societeId)
        {
            return Repository.GetJournaux(societeId);
        }

        /// <summary>
        /// Récupère la liste des journaux en fonction de filtres
        /// </summary>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Liste des journaux</returns>
        public IEnumerable<JournalEnt> GetFilteredJournalList(SearchCriteriaEnt<JournalEnt> filters)
        {
            return Repository.GetLogListByFilters(GetPredicate(filters));
        }

        /// <summary>
        /// Retourne le prédicat de recherche d'un journal comptable
        /// </summary>
        /// <param name="filters">Filtres pour la recherche</param>
        /// <returns>Retourne la condition de recherche des journaux comptables</returns>
        private Expression<Func<JournalEnt, bool>> GetPredicate(SearchCriteriaEnt<JournalEnt> filters)
        {
            if (filters.SearchExactly)
            {
                return p => (string.IsNullOrEmpty(filters.ValueText)
                            || (filters.Code && p.Code.Equals(filters.ValueText, StringComparison.InvariantCultureIgnoreCase))
                            || (filters.Libelle && p.Libelle.Equals(filters.ValueText, StringComparison.InvariantCultureIgnoreCase)))
                          && !filters.Actif
                          && (!filters.SocieteId.HasValue || p.SocieteId == filters.SocieteId.Value);
            }
            else
            {
                return p => (string.IsNullOrEmpty(filters.ValueText)
                            || (filters.Code && p.Code.Contains(filters.ValueText))
                            || (filters.Libelle && p.Libelle.Contains(filters.ValueText)))
                          && !filters.Actif
                          && (!filters.SocieteId.HasValue || p.SocieteId == filters.SocieteId.Value);
            }
        }

        /// <summary>
        /// Ajoute un journal
        /// </summary>
        /// <param name="journal">Journal à ajouter</param>
        /// <returns>Nouveau journal</returns>
        public JournalEnt AddJournal(JournalEnt journal)
        {
            BusinessValidation(journal);

            journal.DateCreation = DateTime.UtcNow;
            if (journal.AuteurCreationId == null)
            {
                journal.AuteurCreationId = this.userManager.GetContextUtilisateurId();
            }
            Repository.AddJournal(journal);
            Save();

            return journal;
        }

        /// <summary>
        /// Mise à jour d'un journal
        /// </summary>
        /// <param name="journal">Journal à mettre à jour</param>
        /// <returns>Journal mis à jour</returns>
        public JournalEnt UpdateJournal(JournalEnt journal)
        {
            BusinessValidation(journal);

            journal.DateModification = DateTime.UtcNow;
            journal.AuteurModificationId = this.userManager.GetContextUtilisateurId();
            Repository.UpdateJournal(journal);
            Save();

            return journal;
        }

        /// <summary>
        /// Supprime un journal s'il n'est pas utilisé
        /// </summary>
        /// <param name="journalId">Identifiant Journal à supprimer</param>  
        public void DeleteJournal(int journalId)
        {
            if (!IsAlreadyUsed(journalId))
            {
                Repository.DeleteJournal(journalId);
                Save();
            }
            else
            {
                throw new FredBusinessException(JournalResource.JournalUtilise);
            }
        }

        /// <summary>
        /// Gestion des journaux : Ajout/Modification/Supression
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="journalList">Liste de journal à traiter</param>
        /// <returns>Liste de journal traitée</returns>
        public IEnumerable<JournalEnt> ManageJournalList(int societeId, IEnumerable<JournalEnt> journalList)
        {
            List<int> existingJournalIdList = GetJournalList(societeId).Where(x => journalList.Select(j => j.Code).Contains(x.Code)).Select(x => x.JournalId).ToList();
            List<int> journalIdList = journalList.Select(x => x.JournalId).ToList();

            // Suppresion des relations journaux
            if (existingJournalIdList.Count > 0)
            {
                foreach (int journalId in existingJournalIdList)
                {
                    if (!journalIdList.Contains(journalId))
                    {
                        DeleteJournal(journalId);
                    }
                }
            }

            // Ajout et Mise à jour
            foreach (JournalEnt journal in journalList.ToList())
            {
                if (journal.JournalId.Equals(0))
                {
                    journal.DateCreation = DateTime.UtcNow;
                    journal.AuteurCreationId = this.userManager.GetContextUtilisateurId();
                    AddJournal(journal);
                }
                else
                {
                    journal.DateModification = DateTime.UtcNow;
                    journal.AuteurModificationId = this.userManager.GetContextUtilisateurId();
                    UpdateJournal(journal);
                }
            }
            return journalList;
        }

        /// <summary>
        /// Retourne un journal pour un id de societe passé en paramètre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="journalId">Identifiant unique du journal</param>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        public JournalEnt GetJournal(int societeId, int journalId)
        {
            return Repository.GetLogImportBySocieteIdAndIdJournal(societeId, journalId);
        }

        /// <summary>
        /// Indique si le journal comptable est utilisé, en testant ses dépendances
        /// </summary>
        /// <param name="journalId">Identifiant du journal comptable</param>
        /// <returns>True si le journal comptable est utilisé</returns>
        public bool IsAlreadyUsed(int journalId)
        {
            return Repository.JournalHasDependencies(journalId);
        }

        /// <summary>
        /// Vérifie la validité et enregistre une liste de Journaux Comptable importé depuis ANAËL
        /// </summary>
        /// <param name="journauxComptableFromANAEL">Liste des journaux comptable dont il faut vérifier la validité</param>
        /// <param name="societeId">Societé des journaux comptable</param>
        public void ManageImportedJournauxComptables(IList<JournalEnt> journauxComptableFromANAEL, int societeId)
        {
            List<JournalEnt> journauxToInsert = null;

            // On récupère tous les Journaux Comptable depuis notre BDD Fred IE pour la société demandée par le Flux d'import
            List<JournalEnt> journauxBDD = GetJournalList(societeId).ToList();

            // Journaux non existant dans notre BDD Fred
            List<JournalEnt> nonExistingJournaux = journauxComptableFromANAEL.Where(x => !journauxBDD.Select(j => j.Code).Contains(x.Code)).ToList();

            // Ce SelectMany retourne la liste de tous les doublons de nonExistingJournauxList
            List<JournalEnt> doublonsJournaux = nonExistingJournaux.GroupBy(x => x.Code)
                                                .Where(g => g.Count() > 1)
                                                .SelectMany(g => g)
                                                .ToList();

            journauxToInsert = nonExistingJournaux.Where(x => !doublonsJournaux.Select(j => j.Code).Contains(x.Code)).ToList();

            // Ajout en masse des Journaux Comptables
            InsertListByTransaction(journauxToInsert);
        }

        /// <summary>
        /// Insère dans la base une liste de JournalEnt de façon transactionnelle.
        /// </summary>
        /// <param name="journauxComptableToInsert">journauxComptableToInsert</param>
        public void InsertListByTransaction(List<JournalEnt> journauxComptableToInsert)
        {
            Repository.InsertListByTransaction(journauxComptableToInsert);
        }

        /// <summary>
        /// Permet la mise à jour de certain champs du model <see cref="JournalModel" />
        /// </summary>
        /// <param name="journal">Model <see cref="JournalModel" /></param>
        /// <param name="fieldsToUpdate">Liste des champs à mettre à jour</param>
        /// <returns>Journal mis à jour</returns>
        public JournalModel UpdateJournal(JournalModel journal, List<Expression<Func<JournalEnt, object>>> fieldsToUpdate)
        {
            JournalEnt journalEnt = mapper.Map<JournalEnt>(journal);
            BusinessValidation(journalEnt);

            journal.DateModification = DateTime.UtcNow;
            journal.AuteurModificationId = this.userManager.GetContextUtilisateurId();

            fieldsToUpdate.Add(x => x.DateModification);
            fieldsToUpdate.Add(x => x.AuteurModificationId);

            Repository.Update(journalEnt, fieldsToUpdate);

            Save();

            return mapper.Map<JournalModel>(journalEnt);
        }

        /// <summary>
        /// Retourne la liste des journaux comptable pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns><see cref="JournalFamilleODModel" /></returns>
        public List<JournalFamilleODModel> GetJournaux(int societeId)
        {
            List<JournalFamilleODModel> journalFamilleODModels = new List<JournalFamilleODModel>();
            List<JournalEnt> journalEnts = Repository.GetJournaux(societeId).ToList();

            foreach (JournalEnt journal in journalEnts)
            {
                journalFamilleODModels.Add(new JournalFamilleODModel
                {
                    JournalId = journal.JournalId,
                    Code = journal.Code,
                    Libelle = journal.Libelle,
                    ParentFamilyODWithOrder = journal.ParentFamilyODWithOrder,
                    ParentFamilyODWithoutOrder = journal.ParentFamilyODWithoutOrder
                });
            }
            return journalFamilleODModels;
        }

        /// <summary>
        /// Retourne la liste des journaux comptable actifs pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns><see cref="JournalFamilleODModel" /></returns>
        public List<JournalFamilleODModel> GetJournauxActifs(int societeId)
        {
            List<JournalFamilleODModel> journalFamilleODModels = new List<JournalFamilleODModel>();
            List<JournalEnt> journalEnts = Repository.GetJournauxActifs(societeId).ToList();

            foreach (JournalEnt journal in journalEnts)
            {
                journalFamilleODModels.Add(new JournalFamilleODModel
                {
                    JournalId = journal.JournalId,
                    Code = journal.Code,
                    Libelle = journal.Libelle,
                    ParentFamilyODWithOrder = journal.ParentFamilyODWithOrder,
                    ParentFamilyODWithoutOrder = journal.ParentFamilyODWithoutOrder
                });
            }
            return journalFamilleODModels;
        }

        /// <summary>
        /// Retourne la liste des journaux qui ne possèdent pas de famille.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des journaux qui ne possèdent pas de famille</returns>
        public IEnumerable<JournalEnt> GetListJournauxWithoutFamille(int societeId)
        {
            return Repository.GetListJournauxWithoutFamille(societeId);
        }

        /// <summary>
        /// Mets à jour une liste de journaux comptable
        /// </summary>
        /// <param name="journaux"><see cref="JournalFamilleODModel" /></param>
        public void UpdateJournaux(List<JournalFamilleODModel> journaux)
        {
            List<Expression<Func<JournalEnt, object>>> fieldsToUpdate = new List<Expression<Func<JournalEnt, object>>>
            {
                x => x.ParentFamilyODWithOrder,
                x => x.ParentFamilyODWithoutOrder,
                x => x.AuteurModificationId,
                x => x.DateModification,
                x => x.Libelle
            };

            List<JournalEnt> journalEnts = ConvertModelToEnt(journaux);

            if (journalEnts.Count != 0)
            {
                foreach (JournalEnt journal in journalEnts)
                {
                    Repository.Update(journal, fieldsToUpdate);
                }
                Save();
            }
        }

        private List<JournalEnt> ConvertModelToEnt(List<JournalFamilleODModel> journaux)
        {
            List<JournalEnt> journals = new List<JournalEnt>();

            foreach (JournalFamilleODModel journal in journaux)
            {
                journals.Add(new JournalEnt
                {
                    AuteurModificationId = userManager.GetContextUtilisateurId(),
                    DateModification = DateTime.UtcNow,
                    ParentFamilyODWithOrder = journal.ParentFamilyODWithOrder,
                    ParentFamilyODWithoutOrder = journal.ParentFamilyODWithoutOrder,
                    JournalId = journal.JournalId,
                    Code = journal.Code,
                    Libelle = journal.Libelle,
                    TypeJournal = journal.TypeJournal ?? string.Empty
                });
            }
            return journals;
        }
    }
}
