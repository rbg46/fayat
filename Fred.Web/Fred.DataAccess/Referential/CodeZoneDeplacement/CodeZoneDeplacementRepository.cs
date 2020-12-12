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

namespace Fred.DataAccess.Referential.CodeZoneDeplacement
{
    /// <summary>
    ///   Référentiel de données pour les codes zones déplacements.
    /// </summary>
    public class CodeZoneDeplacementRepository : FredRepository<CodeZoneDeplacementEnt>, ICodeZoneDeplacementRepository
    {
        private readonly ILogManager logManager;

        public CodeZoneDeplacementRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Appel à une procédure stockée
        /// </summary>
        /// <param name="holdingId">Id du Holding</param>
        /// <returns>Renvoie un int</returns>
        protected int AppelTraitementSqlImpCodeZoneDeplacementFromHolding(int holdingId)
        {
            int nbcmd = 0;

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            DbCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "ImportCodeZoneDeplacementFromHolding";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 240;

            sqlCommand.Parameters.Add(new SqlParameter("@holdingId", holdingId));
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
        /// <param name="codeZoneDeplacementId">Code zone déplacement a vérifier les dépendances</param>
        /// <returns>Renvoie un bool</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(int codeZoneDeplacementId)
        {
            var pointageId = GetPointageAnticipeIdByCodeZoneDeplacementId(codeZoneDeplacementId);

            var rapportLigneId = GetRapportLigneIdByCodeZoneDeplacementId(codeZoneDeplacementId);

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            DbCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "VerificationDeDependance";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 240;

            sqlCommand.Parameters.Add(new SqlParameter("@origTableName", "FRED_CODE_ZONE_DEPLACEMENT"));
            sqlCommand.Parameters.Add(new SqlParameter("@exclusion", string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@dependance", (rapportLigneId.HasValue || pointageId.HasValue) ? $"'FRED_RAPPORT_LIGNE',{rapportLigneId} | 'FRED_POINTAGE_ANTICIPE',{pointageId}" : string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@origineId", codeZoneDeplacementId));
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
        private int? GetPointageAnticipeIdByCodeZoneDeplacementId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.PointagesAnticipes.Where(s => s.CodeZoneDeplacementId == id).Select(s => s.PointageAnticipeId).FirstOrDefault();
        }

        /// <summary>
        ///   Permet de récupérer l'id d'une ligne d'un rapport lié au code de déplacement spécifiée.
        /// </summary>
        /// <param name="id">Identifiant du code de déplacement</param>
        /// <returns>Retourne l'identifiant de la 1ere ligne de rapport</returns>
        private int? GetRapportLigneIdByCodeZoneDeplacementId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.RapportLignes.Where(s => s.CodeZoneDeplacementId == id).Select(s => s.RapportLigneId).FirstOrDefault();
        }

        /// <summary>
        ///   Permet la récupération d'une zone de déplacement en fonction de l'identifiant de sa société de paramétrage et de son
        ///   code.
        /// </summary>
        /// <param name="societeId">Identifiant de la société de paramétrage de la zone de déplacement</param>
        /// <param name="codeZone">Code de la zone de déplacement</param>
        /// <returns>Retourne la zone de déplacement en fonction du code passé en paramètre, null si inexistant.</returns>
        public CodeZoneDeplacementEnt GetZoneBySocieteIdAndCode(int societeId, string codeZone)
        {
            CodeZoneDeplacementEnt zone;

            try
            {
                zone = Context.CodeZoneDeplacement.FirstOrDefault(z => z.SocieteId.Equals(societeId) && z.Code.Equals(codeZone));
            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return zone;
        }

        /// <summary>
        ///   Ajout une nouvelle société
        /// </summary>
        /// <param name="codeZoneDeplacement"> Code zone déplacement à ajouter</param>
        /// <returns>L'identifiant du code zone déplacement ajouté</returns>
        public int AddCodeZoneDeplacement(CodeZoneDeplacementEnt codeZoneDeplacement)
        {
            try
            {
                Context.CodeZoneDeplacement.Add(codeZoneDeplacement);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return codeZoneDeplacement.CodeZoneDeplacementId;
        }

        /// <summary>
        ///   Supprime un code zone deplacement
        /// </summary>
        /// <param name="id">L'identifiant du code zone deplacement à supprimer</param>
        /// <param name="idUtilisateur">Identifiant de l'utilisateur ayant fait l'action</param>
        public void DeleteCodeZoneDeplacementById(int id, int idUtilisateur)
        {
            CodeZoneDeplacementEnt codeZoneDeplacement = Context.CodeZoneDeplacement.Find(id);
            if (codeZoneDeplacement == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.Entry(codeZoneDeplacement).State = EntityState.Modified;

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Vérifie qu'une societe peut être supprimée
        /// </summary>
        /// <param name="codeZoneDeplacementEnt">Le code zone déplacement à vérifier</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(CodeZoneDeplacementEnt codeZoneDeplacementEnt)
        {
            return AppelTraitementSqlVerificationDesDependances(codeZoneDeplacementEnt.CodeZoneDeplacementId);
        }

        /// <summary>
        ///   Retourne la liste des codes zone déplacement par société
        /// </summary>
        /// <returns>Renvoie la liste de des codes zone déplacement active</returns>
        public IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementList()
        {
            foreach (CodeZoneDeplacementEnt codeZoneDeplacement in Context.CodeZoneDeplacement.Where(y => y.IsActif))
            {
                yield return codeZoneDeplacement;
            }
        }

        /// <summary>
        ///   La liste des codes zone déplacement par société
        /// </summary>
        /// <returns>Renvoie la liste des tous les codes zones déplacements</returns>
        public IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementListAll()
        {
            foreach (CodeZoneDeplacementEnt codeZoneDeplacement in Context.CodeZoneDeplacement)
            {
                yield return codeZoneDeplacement;
            }
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeZoneDeplacement
        /// </summary>
        /// <param name="codeZoneDeplacement">Code zone deplacement à modifier</param>
        public void UpdateCodeZoneDeplacement(CodeZoneDeplacementEnt codeZoneDeplacement)
        {
            try
            {
                Context.Entry(codeZoneDeplacement).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Code zone déplacement via l'id
        /// </summary>
        /// <param name="id">Id du code zone déplacement</param>
        /// <returns>Renvoie un code zone déplacement</returns>
        public CodeZoneDeplacementEnt GetCodeZoneDeplacementById(int id)
        {
            return Context.CodeZoneDeplacement.Find(id);
        }

        /// <summary>
        ///   Code zone déplacement via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <param name="actif">True pour les actifs, false pour les inactifs, null pour tous.</param>
        /// <returns>Renvoie un code zone déplacement</returns>
        public IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementBySocieteId(int societeId, bool? actif = null)
        {
            return Context.CodeZoneDeplacement.Where(x => x.SocieteId == societeId && (!actif.HasValue || x.IsActif == actif.Value)).OrderBy(s => s.Code);
        }

        /// <summary>
        ///   Import des codes zones deplacements automatiques depuis la Holding
        /// </summary>
        /// <param name="holdingId"> Id du Holding</param>
        /// <returns>Renvoie les codes zones déplacements</returns>
        public int ImportCodeZoneDeplacementFromHolding(int holdingId)
        {
            return AppelTraitementSqlImpCodeZoneDeplacementFromHolding(holdingId);
        }

        /// <summary>
        ///   Permet de récupérer la liste des codes zones deplacements en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur les codes zones deplacements</param>
        /// <returns>Retourne la liste filtrée des codes zones deplacements</returns>
        public IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementWithFilters(Expression<Func<CodeZoneDeplacementEnt, bool>> predicate)
        {
            return Context.CodeZoneDeplacement.Where(predicate).OrderBy(x => x.Code);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes zones deplacements en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les codes zones deplacements</param>
        /// <returns>Retourne la liste filtrée de tous les codes zones deplacements</returns>
        public IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementAllWithFilters(Expression<Func<CodeZoneDeplacementEnt, bool>> predicate)
        {
            return Context.CodeZoneDeplacement.Where(predicate).OrderBy(s => s.Code);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes zones deplacements en fonction des critères de recherche par société.
        /// </summary>
        /// <param name="societeId">Id de la societe</param>
        /// <param name="predicate">Filtres de recherche sur tous les codes zones deplacements</param>
        /// <returns>Retourne la liste filtrée de tous les codes zones deplacements</returns>
        public IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementAllBySocieteIdWithFilters(int societeId, Expression<Func<CodeZoneDeplacementEnt, bool>> predicate)
        {
            return Context.CodeZoneDeplacement.Where(predicate).Where(c => c.SocieteId == societeId).OrderBy(s => s.Code);
        }

        /// <summary>
        ///   Permet de connaître l'existence d'une société depuis son code.
        /// </summary>
        /// <param name="idCourant">id courant</param>
        /// <param name="codeZoneDeplacement">code CodeZoneDeplacement</param>
        /// <param name="societeId">Id societe</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool IsCodeZoneDeplacementExistsByCode(int idCourant, string codeZoneDeplacement, int societeId)
        {
            if (idCourant != 0)
            {
                return Context.CodeZoneDeplacement.Where(x => x.SocieteId == societeId).Any(s => s.Code == codeZoneDeplacement && s.CodeZoneDeplacementId != idCourant);
            }

            return Context.CodeZoneDeplacement.Where(x => x.SocieteId == societeId).Any(s => s.Code == codeZoneDeplacement);
        }

        /// <summary>
        ///   Permet de savoir si le code zone deplacement existe déjà pour une societe
        /// </summary>
        /// <param name="codeZoneDeplacement">code CodeZoneDeplacement</param>
        /// <param name="societeId">Id societe</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool IsCodeZoneDeplacementExistsBySoc(string codeZoneDeplacement, int societeId)
        {
            return Context.CodeZoneDeplacement.Where(x => x.SocieteId == societeId).Any(s => s.Code == codeZoneDeplacement);
        }

        /// <summary>
        ///   Recherche code zone deplacement sur le champ code uniquement
        /// </summary>
        /// <param name="text">Texte à rechercher dans le champs code</param>
        /// <returns>Retourne une liste de code zone deplacement</returns>
        public IEnumerable<CodeZoneDeplacementEnt> SearchLight(string text)
        {
            return Context.CodeZoneDeplacement.Where(c => c.Code.Contains(text)
                                                          || c.Libelle.Contains(text));
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

        /// <summary>
        ///  Récupère le code zone le plus avantageux 
        /// </summary>
        /// <param name="codeZone1">Premier code zone à comparer</param>
        /// <param name="codeZone2">Deuxième code zone à comparer</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Le code le plus avantageux</returns>
        public string GetCompareBiggestCodeZondeDeplacement(string codeZone1, string codeZone2, int societeId)
        {
            return Context.CodeZoneDeplacement
                .Where(x => (x.Code == codeZone1 || x.Code == codeZone2) && x.SocieteId == societeId)
                .OrderByDescending(x => x.KmMaxi)
                .Select(x => x.Code)
                .FirstOrDefault();

        }

        /// <summary>
        /// Permet de récupérer le code zone deplacement en fonction du kilométrage
        /// </summary>
        /// <param name="societeId">société concerné</param>
        /// <param name="km">kilomètre</param>
        /// <returns>code zone déplacement</returns>
        public CodeZoneDeplacementEnt GetCodeZoneDeplacementByKm(int societeId, double km)
        {
            return Context.CodeZoneDeplacement.FirstOrDefault(c => c.KmMini <= km && km <= c.KmMaxi && c.SocieteId == societeId);
        }
    }
}
