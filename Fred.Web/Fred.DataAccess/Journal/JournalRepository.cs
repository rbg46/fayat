using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Journal;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Journal
{
    public class JournalRepository : FredRepository<JournalEnt>, IJournalRepository
    {
        public JournalRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Retourne la liste de tout les journaux
        /// </summary>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        public IEnumerable<JournalEnt> GetAllJournal()
        {
            foreach (JournalEnt log in Context.Journals.OrderBy(s => s.DateCloture))
            {
                yield return log;
            }
        }

        /// <summary>
        /// Retourne la liste des journaux pour un id de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        public IEnumerable<JournalEnt> GetJournaux(int societeId)
        {
            return Context.Journals.OrderBy(s => s.DateCloture).Where(s => s.SocieteId.Equals(societeId)).AsNoTracking();
        }

        /// <summary>
        /// Retourne la liste des journaux actifs pour un id de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        public IReadOnlyList<JournalEnt> GetJournauxActifs(int societeId)
        {
            return Context.Journals.Where(s => s.SocieteId.Equals(societeId) && !s.DateCloture.HasValue).AsNoTracking().ToList();
        }

        /// <summary>
        /// Retourne la liste des journaux pour un id de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="journalId">Identifiant unique du journal</param>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        public JournalEnt GetLogImportBySocieteIdAndIdJournal(int societeId, int journalId)
        {
            return Context.Journals.Where(s => s.SocieteId.Equals(societeId) && s.JournalId.Equals(journalId)).FirstOrDefault();
        }

        /// <summary>
        /// Retourne la liste des journaux pour un code de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        public IEnumerable<JournalEnt> GetLogImportBySocieteCode(string societeId)
        {
            return Context.Journals.Include(s => s.Societe).Where(s => s.Societe.SocieteId.Equals(societeId)).OrderBy(s => s.DateCloture);
        }

        /// <summary>
        /// Retourne le journal dont le code est passé en paramètre
        /// </summary>
        /// <param name="code">Code du journal</param>
        /// <returns>Un journal</returns>
        public JournalEnt GetJournalByCode(string code)
        {
            return Context.Journals.FirstOrDefault(j => j.Code.Equals(code));
        }

        /// <summary>
        /// Retourne l'identifiant du journal portant le code journal indiqué.
        /// </summary>
        /// <param name="code">Code du journal dont l'identifiant est à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon nulle.</returns>
        public int? GetJournalIdByCode(string code)
        {
            int? journalId = null;
            JournalEnt journal = Context.Journals.FirstOrDefault(j => j.Code == code);

            if (journal != null)
            {
                journalId = journal.JournalId;
            }

            return journalId;
        }

        /// <summary>
        /// Retourne le journal dont l'id a été passé en paramètre
        /// </summary>
        /// <param name="id">Identifiant unique du journal</param>
        /// <returns>Un journal</returns>
        public JournalEnt GetLogImportById(int id)
        {
            return Context.Journals.FirstOrDefault(s => s.JournalId.Equals(id));
        }

        /// <summary>
        /// Retourne la liste des journals à importer par code société.
        /// </summary>
        /// <param name="codeSociete">Code de la société</param>
        /// <returns>Liste des journals à importer.</returns>
        public IEnumerable<JournalEnt> GetListJournalToImporFactureByCodeSociete(string codeSociete)
        {
            return Context.Journals.Where(j => j.Societe.CodeSocieteComptable.Equals(codeSociete) && j.ImportFacture);
        }

        /// <summary>
        /// Retourne la liste des journaux correspondant aux filtres spécifiés
        /// </summary>
        /// <param name="predicate">Conditions à vérifier par les journaux</param>
        /// <returns>Liste des journaux correspondant aux filtres spécifiés</returns>
        public IEnumerable<JournalEnt> GetLogListByFilters(Expression<Func<JournalEnt, bool>> predicate)
        {
            return Context.Journals
                          .Where(predicate)
                          .OrderBy(n => n.Code);
        }

        /// <summary>
        /// Insertion en base d'un journal
        /// </summary>
        /// <param name="journal">Le journal à enregistrer</param>
        /// <returns>Retourne l'identifiant unique du journal</returns>
        public JournalEnt AddJournal(JournalEnt journal)
        {
            Insert(journal);

            return journal;
        }

        /// <inheritdoc />
        public JournalEnt UpdateJournal(JournalEnt journal)
        {
            Update(journal);

            return journal;
        }

        /// <inheritdoc />
        public void DeleteJournal(int journalId)
        {
            JournalEnt journal = FindById(journalId);

            if (journal != null)
            {
                Delete(journal);
            }
        }

        /// <summary>
        /// Appel à une procédure stockée de vérification de dépendance d'un journal
        /// </summary>
        /// <param name="journalId">Id du journal comptable pour vérifier ses dépendances</param>
        /// <returns>True si le journal comptable est utilisé</returns>
        public bool JournalHasDependencies(int journalId)
        {
            int nbDependances;

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            DbCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "VerificationDeDependance";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 240;

            sqlCommand.Parameters.Add(new SqlParameter("@origTableName", "FRED_JOURNAL"));
            sqlCommand.Parameters.Add(new SqlParameter("@exclusion", string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@dependance", string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@origineId", journalId));
            sqlCommand.Parameters.Add(new SqlParameter("@delimiter", '|'));
            sqlCommand.Parameters.Add(new SqlParameter("@resu", SqlDbType.Int) { Direction = ParameterDirection.Output });

            sqlConnection.Open();
            sqlCommand.ExecuteReader();
            sqlConnection.Close();

            nbDependances = (int)sqlCommand.Parameters["@resu"].Value;


            return nbDependances > 0;
        }

        /// <summary>
        /// Insère dans la base une liste de JournalEnt de façon transactionnelle.
        /// </summary>
        /// <param name="journauxComptableToInsert">journauxComptableToInsert</param>
        public void InsertListByTransaction(List<JournalEnt> journauxComptableToInsert)
        {
            if (journauxComptableToInsert.Count == 0)
            {
                return;
            }

            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    // Disable detection of changes
                    Context.ChangeTracker.AutoDetectChangesEnabled = false;

                    // Ajout des Journaux Comptables
                    Context.Journals.AddRange(journauxComptableToInsert);

                    Context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
                finally
                {
                    // Re-enable detection of changes
                    Context.ChangeTracker.AutoDetectChangesEnabled = true;
                }
            }

        }

        /// <summary>
        /// Retourne la liste des journaux qui ne possèdent pas de famille.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des journaux qui ne possèdent pas de famille</returns>
        public IEnumerable<JournalEnt> GetListJournauxWithoutFamille(int societeId)
        {
            return Context.Journals
                .Where(j => j.ParentFamilyODWithOrder == 0
                && j.ParentFamilyODWithoutOrder == 0
                && !j.DateCloture.HasValue
                && j.SocieteId == societeId);
        }
    }
}
