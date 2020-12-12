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
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.CodeDeplacement
{
    /// <summary>
    ///   Référentiel de données pour les codes déplacement.
    /// </summary>
    public class CodeDeplacementRepository : FredRepository<CodeDeplacementEnt>, ICodeDeplacementRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CodeDeplacementRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        public CodeDeplacementRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        ///   Retourne la liste des codesDeplacement.
        /// </summary>
        /// <returns>La liste des codesDeplacement.</returns>
        /// <param name="societeId"> Identifiant de la société</param>
        public IEnumerable<CodeDeplacementEnt> GetCodeDeplacementListBySocieteId(int societeId)
        {
            foreach (CodeDeplacementEnt codeDep in Context.CodesDeplacement.Where(c => c.SocieteId.Equals(societeId) && c.Actif))
            {
                yield return codeDep;
            }
        }

        /// <summary>
        ///   Appel à une procédure stockée
        /// </summary>
        /// <param name="codeDeplacementId">ID du Code déplacement a vérifier les dépendances</param>
        /// <returns>Renvoie un bool</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(int codeDeplacementId)
        {
            var pointageId = GetPointageAnticipeIdByCodeDeplacementId(codeDeplacementId);

            var rapportLigneId = GetRapportLigneIdByCodeDeplacementId(codeDeplacementId);

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            DbCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "VerificationDeDependance";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 240;

            sqlCommand.Parameters.Add(new SqlParameter("@origTableName", "FRED_CODE_DEPLACEMENT"));
            sqlCommand.Parameters.Add(new SqlParameter("@exclusion", string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@dependance", (rapportLigneId.HasValue || pointageId.HasValue) ? $"'FRED_RAPPORT_LIGNE',{rapportLigneId} | 'FRED_POINTAGE_ANTICIPE',{pointageId}" : string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@origineId", codeDeplacementId));
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

        /// <summary>
        ///   Permet de récupérer l'id d'un pointage anticipé lié au code de déplacement spécifiée.
        /// </summary>
        /// <param name="id">Identifiant du code de déplacement</param>
        /// <returns>Retourne l'identifiant du 1er pointage anticipé</returns>
        private int? GetPointageAnticipeIdByCodeDeplacementId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.PointagesAnticipes.Where(s => s.CodeDeplacementId == id).Select(s => s.PointageAnticipeId).FirstOrDefault();
        }

        /// <summary>
        ///   Permet de récupérer l'id d'une ligne d'un rapport lié au code de déplacement spécifiée.
        /// </summary>
        /// <param name="id">Identifiant du code de déplacement</param>
        /// <returns>Retourne l'identifiant de la 1ere ligne de rapport</returns>
        private int? GetRapportLigneIdByCodeDeplacementId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.RapportLignes.Where(s => s.CodeDeplacementId == id).Select(s => s.RapportLigneId).FirstOrDefault();
        }

        /// <summary>
        ///   Retourne la liste des codesDeplacement.
        /// </summary>
        /// <param name="societeId"> Identifiant de l'utilisateur</param>
        /// <returns>La liste des codesDeplacement.</returns>
        public IEnumerable<CodeDeplacementEnt> GetCodeDeplacementList(int societeId)
        {
            foreach (CodeDeplacementEnt codeDep in Context.CodesDeplacement.Where(c => c.SocieteId.Equals(societeId)))
            {
                yield return codeDep;
            }
        }

        /// <summary>
        ///   Retourne le CodeDeplacement portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="codeDeplacementId">Identifiant du CodeDeplacement à retrouver.</param>
        /// <returns>Le CodeDeplacement retrouvé, sinon nulle.</returns>
        public CodeDeplacementEnt GetCodeDeplacementById(int codeDeplacementId)
        {
            return Context.CodesDeplacement.Find(codeDeplacementId);
        }

        /// <summary>
        ///   Retourne le CodeDeplacement portant le code indiqué.
        /// </summary>
        /// <param name="codeDeplacement">Code déplacement à retrouver.</param>
        /// <returns>Le code déplacement retrouvé, sinon nulle.</returns>
        public CodeDeplacementEnt GetCodeDeplacementByCode(string codeDeplacement)
        {
            return Context.CodesDeplacement.FirstOrDefault(c => c.Code.Equals(codeDeplacement));
        }

        /// <summary>
        ///   Permet de connaître l'existence d'un code déplacement depuis son code.
        /// </summary>
        /// <param name="codeDeplacementId">Identifiant du code déplacement courant</param>
        /// <param name="code">Code de deplacement à vérifier</param>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon.</returns>
        public bool CodeDeplacementExistForSocieteIdAndCode(int codeDeplacementId, string code, int societeId)
        {
            CodeDeplacementEnt codeDeplacementEnt = GetBySocieteIdAndCode(societeId, code);
            if (codeDeplacementEnt != null && codeDeplacementEnt.CodeDeplacementId != codeDeplacementId)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Méthode qui retourne un codeDeplacement pour un societeId et un code donné.
        /// </summary>
        /// <param name="societeId">Id de la société</param>
        /// <param name="code">Code a rechercher</param>
        /// <returns>Le codeDeplacement pour un code et un societeId</returns>
        public CodeDeplacementEnt GetBySocieteIdAndCode(int societeId, string code)
        {
            return Context.CodesDeplacement.FirstOrDefault(c => c.SocieteId.Equals(societeId) && c.Code == code);
        }

        /// <summary>
        ///   Vérifie qu'un code deplacement peut être supprimé
        /// </summary>
        /// <param name="codeDeplacementId">Le code déplacement à vérifier</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(int codeDeplacementId)
        {
            return AppelTraitementSqlVerificationDesDependances(codeDeplacementId);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes déplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="predicate">Filtres de recherche sur tous les codes déplacement</param>
        /// <returns>Retourne la liste filtré de tous les codes déplacement</returns>
        public IEnumerable<CodeDeplacementEnt> SearchCodeDepAllWithFilters(int societeId, Expression<Func<CodeDeplacementEnt, bool>> predicate)
        {
            return Context.CodesDeplacement.Where(c => c.SocieteId.Equals(societeId)).Where(predicate).OrderBy(s => s.Code);
        }

        /// <summary>
        ///   Permet de récupérer la liste des codes déplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur les codes déplacement</param>
        /// <returns>Retourne la liste filtré des codes déplacement</returns>
        public IEnumerable<CodeDeplacementEnt> SearchCodeDepWithFilters(Expression<Func<CodeDeplacementEnt, bool>> predicate)
        {
            return Context.CodesDeplacement.Where(s => s.Actif).Where(predicate).OrderBy(s => s.Code);
        }

        /// <summary>
        ///   Cherche une liste de CodeMajoration.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des codesMajoration.</param>
        /// <param name="societeId">Identifiant de la société liée.</param>
        /// <returns>Une liste de CodeMajoration.</returns>
        public IEnumerable<CodeDeplacementEnt> SearchLight(string text, int societeId)
        {
            if (string.IsNullOrEmpty(text))
            {
                return QueryPagingHelper.ApplyScrollPaging(GetCodeDeplacementListBySocieteId(societeId).AsQueryable());
            }

            return QueryPagingHelper.ApplyScrollPaging(Context.CodesDeplacement.Where(c => c.Code.Contains(text)
                                                                                           || c.Libelle.Contains(text))
                                                              .AsQueryable());
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