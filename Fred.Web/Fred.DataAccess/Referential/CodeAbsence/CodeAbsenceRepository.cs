using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.CodeAbsence
{
    /// <summary>
    ///   Référentiel de données pour les codes d'absence.
    /// </summary>
    public class CodeAbsenceRepository : FredRepository<CodeAbsenceEnt>, ICodeAbsenceRepository
    {
        private readonly ILogManager logManager;

        public CodeAbsenceRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Appel à une procédure stockée
        /// </summary>
        /// <param name="holdingId">Id du Holding</param>
        /// <param name="idNewSociete">Id de la nouvelle société</param>
        /// <returns>Renvoie un int</returns>
        protected int AppelTraitementSqlImpCodeAbsFromHolding(int holdingId, int idNewSociete)
        {
            int nbcmd = 0;

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            DbCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "ImportCodeAbsFromHolding";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 240;

            sqlCommand.Parameters.Add(new SqlParameter("@holdingId", holdingId));
            sqlCommand.Parameters.Add(new SqlParameter("@idNewSociete", idNewSociete));
            sqlCommand.Parameters.Add(new SqlParameter("@nbcmd", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });

            sqlConnection.Open();
            sqlCommand.ExecuteReader();
            sqlConnection.Close();

            nbcmd = (int)sqlCommand.Parameters["@nbcmd"].Value;

            return nbcmd;
        }

        /// <summary>
        ///   Appel à une procédure stockée
        /// </summary>
        /// <param name="codeAbsenceId">Code d'absence a vérifier les dépendances</param>
        /// <returns>Renvoie un bool</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(int codeAbsenceId)
        {
            try
            {
                var pointageId = GetPointageAnticipeIdByCodeAbsenceId(codeAbsenceId);

                var rapportLigneId = GetRapportLigneIdByCodeAbsenceId(codeAbsenceId);

                DbConnection sqlConnection = Context.Database.GetDbConnection();
                DbCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "VerificationDeDependance";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 240;

                sqlCommand.Parameters.Add(new SqlParameter("@origTableName", "FRED_CODE_ABSENCE"));
                sqlCommand.Parameters.Add(new SqlParameter("@exclusion", string.Empty));
                sqlCommand.Parameters.Add(new SqlParameter("@dependance", (rapportLigneId.HasValue || pointageId.HasValue) ? $"'FRED_RAPPORT_LIGNE',{rapportLigneId} | 'FRED_POINTAGE_ANTICIPE',{pointageId}" : string.Empty));
                sqlCommand.Parameters.Add(new SqlParameter("@origineId", codeAbsenceId));
                sqlCommand.Parameters.Add(new SqlParameter("@delimiter", '|'));
                sqlCommand.Parameters.Add(new SqlParameter("@resu", SqlDbType.Int) { Direction = ParameterDirection.Output });

                sqlConnection.Open();
                sqlCommand.ExecuteReader();
                sqlConnection.Close();

                int nbcmd = (int)sqlCommand.Parameters["@resu"].Value;

                if (nbcmd == 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                throw new FredRepositoryException("Un problème est survenu pendant la vérification des dépendances des codes d'absences.");
            }
        }

        /// <summary>
        ///   Permet de récupérer l'id d'un pointage anticipé lié au code de déplacement spécifiée.
        /// </summary>
        /// <param name="id">Identifiant du code de déplacement</param>
        /// <returns>Retourne l'identifiant du 1er pointage anticipé</returns>
        private int? GetPointageAnticipeIdByCodeAbsenceId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.PointagesAnticipes.Where(s => s.CodeAbsenceId == id).Select(s => s.PointageAnticipeId).FirstOrDefault();
        }

        /// <summary>
        ///   Permet de récupérer l'id d'une ligne d'un rapport lié au code de déplacement spécifiée.
        /// </summary>
        /// <param name="id">Identifiant du code de déplacement</param>
        /// <returns>Retourne l'identifiant de la 1ere ligne de rapport</returns>
        private int? GetRapportLigneIdByCodeAbsenceId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.RapportLignes.Where(s => s.CodeAbsenceId == id).Select(s => s.RapportLigneId).FirstOrDefault();
        }

        /// <summary>
        ///   Ajout une nouvelle société
        /// </summary>
        /// <param name="codeAbs"> Code d'absence à ajouter</param>
        /// <returns>L'identifiant du code d'absence ajouté</returns>
        public int AddCodeAbsence(CodeAbsenceEnt codeAbs)
        {
            try
            {
                Context.CodeAbsences.Add(codeAbs);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                logManager.TraceException(exception.Message, exception);
                throw;
            }

            return codeAbs.CodeAbsenceId;
        }

        /// <summary>
        ///   Supprime le code d'absence par Id
        /// </summary>
        /// <param name="id">Id du code d'absence à supprimer</param>
        public void DeleteCodeAbsenceById(int id)
        {
            CodeAbsenceEnt codeAbs = Context.CodeAbsences.Find(id);
            if (codeAbs == null)
            {
                DataException objectNotFoundException = new DataException();
                logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.CodeAbsences.Remove(codeAbs);

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Vérifie qu'un code d'absence peut être supprimé
        /// </summary>
        /// <param name="codeAbs">Le code d'absence à vérifier</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(CodeAbsenceEnt codeAbs)
        {
            return AppelTraitementSqlVerificationDesDependances(codeAbs.CodeAbsenceId);
        }

        /// <summary>
        ///   Retourne la liste des codes d'absence par société
        /// </summary>
        /// <returns>Renvoie la liste de des codes d'absence active</returns>
        public IEnumerable<CodeAbsenceEnt> GetCodeAbsList()
        {
            foreach (CodeAbsenceEnt codeAbs in Context.CodeAbsences.Where(y => y.Actif.Equals(true)))
            {
                yield return codeAbs;
            }
        }

        /// <summary>
        ///   La liste des codes d'absence par société
        /// </summary>
        /// <returns>Renvoie la liste des tous les codes d'absence</returns>
        public IEnumerable<CodeAbsenceEnt> GetCodeAbsListAll()
        {
            foreach (CodeAbsenceEnt codeAbs in Context.CodeAbsences)
            {
                yield return codeAbs;
            }
        }

        /// <summary>
        ///   Retourne la liste de tous les codes d'absence pour la synchronisation mobile.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        public IEnumerable<CodeAbsenceEnt> GetCodeAbsListAllSync()
        {
            return Context.CodeAbsences.AsNoTracking();
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeAbsence
        /// </summary>
        /// <param name="codeAbs">Code absence à modifier</param>
        public void UpdateCodeAbsence(CodeAbsenceEnt codeAbs)
        {
            try
            {
                Context.Entry(codeAbs).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Code d'absence via l'id
        /// </summary>
        /// <param name="id">Id du code d'absence</param>
        /// <returns>Renvoie un code d'absence</returns>
        public CodeAbsenceEnt GetCodeAbsenceById(int id)
        {
            return Context.CodeAbsences.Find(id);
        }

        /// <summary>
        ///   Retourne le codeAbsence correspondant au code
        /// </summary>
        /// <param name="code">Le code de l'absence</param>
        /// <returns>Renvoie un code d'absence</returns>
        public CodeAbsenceEnt GetCodeAbsenceByCode(string code)
        {
            return Context.CodeAbsences.Where(o => o.Code.Equals(code)).FirstOrDefault();
        }

        /// <summary>
        ///   Code d'absence via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <returns>Renvoie un code d'absence</returns>
        public IEnumerable<CodeAbsenceEnt> GetCodeAbsenceBySocieteId(int societeId)
        {
            return Context.CodeAbsences.Where(x => x.SocieteId == societeId);
        }

        /// <summary>
        ///   Import des codes absences automatiques depuis la Holding
        /// </summary>
        /// <param name="holdingId"> Id du Holding</param>
        /// <param name="idNewSociete"> Id du de la nouvelle société</param>
        /// <returns>Renvoie un int</returns>
        public int ImportCodeAbsFromHolding(int holdingId, int idNewSociete)
        {
            return AppelTraitementSqlImpCodeAbsFromHolding(holdingId, idNewSociete);
        }

        /// <summary>
        ///   Permet de récupérer la liste des codes absences en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur les codes absences</param>
        /// <returns>Retourne la liste filtrée des codes absences</returns>
        public IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceWithFilters(Expression<Func<CodeAbsenceEnt, bool>> predicate)
        {
            return Context.CodeAbsences.Where(predicate).OrderBy(x => x.Code);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes absences en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les codes absences</param>
        /// <returns>Retourne la liste filtrée de tous les codes absences</returns>
        public IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceAllWithFilters(Expression<Func<CodeAbsenceEnt, bool>> predicate)
        {
            return Context.CodeAbsences.Where(predicate).OrderBy(s => s.Code);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes absences en fonction des critères de recherche par société.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les codes absences</param>
        /// <param name="societeId">Id de la societe</param>
        /// <returns>Retourne la liste filtrée de tous les codes absences</returns>
        public IEnumerable<CodeAbsenceEnt> SearchCodeAbsenceAllBySocieteIdWithFilters(Expression<Func<CodeAbsenceEnt, bool>> predicate, int societeId)
        {
            return Context.CodeAbsences.Where(c => c.SocieteId == societeId).Where(predicate).OrderBy(s => s.Code);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes absences en fonction des critères de recherche par société.
        /// </summary>
        /// <param name="text">Filtres de recherche sur tous les codes absences</param>
        /// <param name="societeId">Id de la societe</param>
        /// <returns>Retourne la liste filtrée de tous les codes absences</returns>
        public IEnumerable<CodeAbsenceEnt> SearchLight(string text, int societeId)
        {
            if (string.IsNullOrEmpty(text))
            {
                return QueryPagingHelper.ApplyScrollPaging(GetCodeAbsenceBySocieteId(societeId).AsQueryable());
            }

            return QueryPagingHelper.ApplyScrollPaging(Context.CodeAbsences.Where(c => c.SocieteId.Equals(societeId) && (c.Code.ToLower().Contains(text.ToLower())
                                                                                                                         || c.Libelle.ToLower().Contains(text.ToLower())))
                                                              .AsQueryable());
        }

        /// <summary>
        ///   Permet de connaître l'existence d'une société depuis son code.
        /// </summary>
        /// <param name="idCourant">id courant</param>
        /// <param name="codeCodeAbsence">code CodeAbsence</param>
        /// <param name="societeId">Id societe</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool IsCodeAbsenceExistsByCode(int idCourant, string codeCodeAbsence, int societeId)
        {
            if (societeId > 0)
            {
                if (idCourant != 0)
                {
                    return Context.CodeAbsences.Where(x => x.SocieteId == societeId).Any(c => c.Code == codeCodeAbsence && c.CodeAbsenceId != idCourant);
                }

                return Context.CodeAbsences.Where(x => x.SocieteId == societeId).Any(s => s.Code == codeCodeAbsence);
            }

            return false;
        }

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        public bool IsAlreadyUsed(int id)
        {
            return !AppelTraitementSqlVerificationDesDependances(id);
        }
    }
}