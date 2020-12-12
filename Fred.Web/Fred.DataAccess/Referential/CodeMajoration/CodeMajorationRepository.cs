using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.CodeMajoration
{
    /// <summary>
    ///   Référentiel de données pour les codes majoration.
    /// </summary>
    public class CodeMajorationRepository : FredRepository<CodeMajorationEnt>, ICodeMajorationRepository
    {
        private readonly ILogManager logManager;

        public CodeMajorationRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        protected bool AppelTraitementSqlVerificationDesDependances(int codeMajorationId)
        {
            int? rapportLigneId;
            rapportLigneId = GetRapportLigneIdByCodeMajorationId(codeMajorationId);

            int? ciCodeMajorationId;
            ciCodeMajorationId = GetCiCodeMajorationIdByCodeMajorationId(codeMajorationId);

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            DbCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "VerificationDeDependance";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 240;

            sqlCommand.Parameters.Add(new SqlParameter("@origTableName", "FRED_CODE_MAJORATION"));
            sqlCommand.Parameters.Add(new SqlParameter("@exclusion", string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@dependance", rapportLigneId.HasValue ? $"'FRED_RAPPORT_LIGNE',{rapportLigneId} | 'FRED_CI_CODE_MAJORATION',{ciCodeMajorationId}" : string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@origineId", codeMajorationId));
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
        ///   Permet de récupérer l'id d'une ligne d'un rapport lié au code de déplacement spécifiée.
        /// </summary>
        /// <param name="id">Identifiant du code de Majoration</param>
        /// <returns>Retourne l'identifiant de la 1ere ligne de rapport</returns>
        private int? GetRapportLigneIdByCodeMajorationId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.RapportLignes.Where(s => s.CodeMajorationId == id).Select(s => s.RapportLigneId).FirstOrDefault();
        }

        /// <summary>
        ///   Permet de récupérer l'id d'une ligne d'un rapport lié au code de déplacement spécifiée.
        /// </summary>
        /// <param name="id">Identifiant du code de Majoration</param>
        /// <returns>Retourne l'identifiant de la 1ere ligne de rapport</returns>
        private int? GetCiCodeMajorationIdByCodeMajorationId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.CICodeMajorations.Where(s => s.CodeMajorationId == id).Select(s => s.CiCodeMajorationId).FirstOrDefault();
        }

        /// <summary>
        ///   Retourne la liste des codesMajoration.
        /// </summary>
        /// <param name="recherche"> Texte de la recherche</param>
        /// <returns>La liste des codesMajoration.</returns>
        public IEnumerable<CodeMajorationEnt> GetCodeMajorationList(string recherche = null)
        {
            IQueryable<CodeMajorationEnt> majorations = Context.CodesMajoration.Include(cm => cm.Groupe).OrderBy(cm => cm.Code);

            if (!string.IsNullOrEmpty(recherche))
            {
                majorations = majorations.Where(cm => cm.Libelle.Contains(recherche) || cm.Code.Contains(recherche));
            }

            foreach (CodeMajorationEnt codeMaj in majorations)
            {
                yield return codeMaj;
            }
        }

        /// <summary>
        ///   Retourne la liste des codesMajoration pour la synchronisaiton mobile.
        /// </summary>
        /// <param name="recherche">Texte de recherche</param>
        /// <returns>La liste des codesMajoration.</returns>
        public IEnumerable<CodeMajorationEnt> GetCodeMajorationListSync(string recherche = null)
        {
            IQueryable<CodeMajorationEnt> majorations = Context.CodesMajoration.AsNoTracking();

            if (!string.IsNullOrEmpty(recherche))
            {
                majorations = majorations.Where(cm => cm.Libelle.Contains(recherche) || cm.Code.Contains(recherche));
            }

            return majorations;
        }

        /// <summary>
        ///   Retourne le CodeMajoration portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="codeMajorationId">Identifiant du CodeMajoration à retrouver.</param>
        /// <returns>Le CodeMajoration retrouvé, sinon nulle.</returns>
        public CodeMajorationEnt GetCodeMajorationById(int codeMajorationId)
        {
            return Context.CodesMajoration.Find(codeMajorationId);
        }

        /// <summary>
        ///   Retourne la liste de codes majoration associés à la société
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe associé aux code majoration à retourner</param>
        /// <returns>La liste des codes majoration associés à la société</returns>
        public IEnumerable<CodeMajorationEnt> GetCodeMajorationListByGroupeId(int groupeId)
        {
            var codesMajo = new List<CodeMajorationEnt>();
            foreach (var codeMaj in Context.CodesMajoration
                                           .Where(cm => cm.GroupeId == groupeId)
                                           .Select(cm => new
                                           {
                                               CodeMajorationEnt = cm,
                                               CICodes = cm.CICodesMajoration.Select(cicm => cicm.CodeMajorationId)
                                           })
                                           .ToList())
            {
                CodeMajorationEnt codeMajo = new CodeMajorationEnt
                {
                    CodeMajorationId = codeMaj.CodeMajorationEnt.CodeMajorationId,
                    Code = codeMaj.CodeMajorationEnt.Code,
                    Libelle = codeMaj.CodeMajorationEnt.Libelle,
                    EtatPublic = codeMaj.CodeMajorationEnt.EtatPublic,
                    IsActif = codeMaj.CodeMajorationEnt.IsActif
                };
                codesMajo.Add(codeMajo);
            }

            return codesMajo;
        }

        /// <summary>
        ///   Retourne la liste de codes majoration actifs associés à la société et à un CI
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe associée aux code majoration à retourner</param>
        /// <param name="ciId">Identifiant du CI associé aux code majoration à retourner</param>
        /// <returns>La liste des codes majoration associés au groupe et à un CI</returns>
        public IEnumerable<CodeMajorationEnt> GetActifAllowedCodeMajorationListForCi(int groupeId, int ciId)
        {
            return QueryPagingHelper.ApplyScrollPaging(Context.CodesMajoration
                                                              .Where(p => p.GroupeId.Equals(groupeId) && p.IsActif &&
                                                                          (p.EtatPublic || (!p.EtatPublic && Context.CICodeMajorations.Any(cp => p.CodeMajorationId == cp.CodeMajorationId && cp.CiId == ciId))))
                                                              .AsQueryable());
        }

        /// <summary>
        ///   Retourne la liste de codes majoration actifs associés à la société + les publics
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe associé aux code majoration à retourner</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>La liste des codes majoration actifs associés à la société</returns>
        public IEnumerable<CodeMajorationEnt> GetActifsCodeMajorationListByGroupeId(int groupeId, int ciId)
        {
            var codesMajo = new List<CodeMajorationEnt>();
            foreach (var codeMaj in Context.CodesMajoration
                                           .Where(cm => cm.GroupeId.Equals(groupeId) && cm.IsActif)
                                           .Select(cm => new
                                           {
                                               CodeMajorationEnt = cm,
                                               CICodes = cm.CICodesMajoration.Where(cip => cip.CiId == ciId).Select(cicm => cicm.CodeMajorationId)
                                           })
                                           .OrderBy(cm => cm.CodeMajorationEnt.Code)
                                           .ToList())
            {
                CodeMajorationEnt codeMajo = new CodeMajorationEnt
                {
                    CodeMajorationId = codeMaj.CodeMajorationEnt.CodeMajorationId,
                    Code = codeMaj.CodeMajorationEnt.Code,
                    Libelle = codeMaj.CodeMajorationEnt.Libelle,
                    EtatPublic = codeMaj.CodeMajorationEnt.EtatPublic,
                    IsActif = codeMaj.CodeMajorationEnt.IsActif,
                    IsLinkedToCI = codeMaj.CICodes.Contains(codeMaj.CodeMajorationEnt.CodeMajorationId)
                };

                codesMajo.Add(codeMajo);
            }

            return codesMajo;
        }

        /// <summary>
        ///   Ajout un nouveau CodeMajoration
        /// </summary>
        /// <param name="codeMajorationEnt"> CodeMajoration à ajouter</param>
        /// <returns> L'identifiant du CodeMajoration ajouté</returns>
        public int AddCodeMajoration(CodeMajorationEnt codeMajorationEnt)
        {
            try
            {
                ////if (this.Context.Entry<ReceptionEnt>(reception).State == EntityState.Detached)
                ////  this.AttachDependancies(reception);
                codeMajorationEnt.Groupe = null;
                Context.CodesMajoration.Add(codeMajorationEnt);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return codeMajorationEnt.CodeMajorationId;
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeMajoration
        /// </summary>
        /// <param name="codeMajorationEnt">CodeMajoration à modifier</param>
        public void UpdateCodeMajoration(CodeMajorationEnt codeMajorationEnt)
        {
            try
            {
                ////if (this.Context.Entry<CodeMajorationEnt>(reception).State == EntityState.Detached)
                ////  this.AttachDependancies(reception);

                Context.Entry(codeMajorationEnt).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Supprime un CodeMajoration
        /// </summary>
        /// <param name="id">L'identifiant du CodeMajoration à supprimer</param>
        public void DeleteCodeMajorationById(int id)
        {
            CodeMajorationEnt codeMaj = Context.CodesMajoration.Find(id);
            if (codeMaj == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.CodesMajoration.Remove(codeMaj);

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
        ///   Vérifie qu'un code majoration peut être supprimée
        /// </summary>
        /// <param name="codeMajorationEnt">Le code majoration à vérifier</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(CodeMajorationEnt codeMajorationEnt)
        {
            return AppelTraitementSqlVerificationDesDependances(codeMajorationEnt.CodeMajorationId);
        }

        /// <summary>
        ///   Méthode vérifiant l'existence d'une nature via son code pour une société donnée.
        /// </summary>
        /// <param name="idCourant"> id courant</param>
        /// <param name="code">Code Nature</param>
        /// <param name="groupeId">Identifiant de la société</param>
        /// <returns>Retourne vrai si la nature existe, faux sinon</returns>
        public bool IsCodeMajorationExistsByCodeInGroupe(int idCourant, string code, int groupeId)
        {
            if (groupeId > 0)
            {
                if (idCourant != 0)
                {
                    return Context.CodesMajoration.Where(x => x.GroupeId == groupeId).Any(s => s.Code == code && s.CodeMajorationId != idCourant);
                }

                return Context.CodesMajoration.Where(x => x.GroupeId == groupeId).Any(s => s.Code == code);
            }

            return false;
        }

        /// <summary>
        ///   Cherche une liste de CodeMajoration.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des codesMajoration.</param>
        /// <param name="groupeId">L'identifiant du groupe associé au code majoration</param>
        /// <param name="ciId">L'identifiant du CI associé au code majoration</param>
        /// <returns>Une liste de CodeMajoration.</returns>
        public IEnumerable<CodeMajorationEnt> SearchCodeMajorations(string text, int groupeId, int ciId)
        {
            if (string.IsNullOrEmpty(text))
            {
                return GetActifAllowedCodeMajorationListForCi(groupeId, ciId);
            }

            return QueryPagingHelper.ApplyScrollPaging(Context.CodesMajoration.Where(c => (c.GroupeId.Equals(groupeId) && c.Code.Contains(text))
                                                                                          || c.Libelle.Contains(text))
                                                              .AsQueryable());
        }

        /// <inheritdoc />
        public IEnumerable<CodeMajorationEnt> GetCodeMajorations(int? societeId, DateTime lastModification)
        {
            var result = from c in Context.CodesMajoration
                         join s in Context.Societes on c.GroupeId equals s.GroupeId
                         where s.SocieteId == societeId
                         select c;

            // Synchornisation delta.
            if (lastModification != default(DateTime))
            {
                result.Where(t => t.DateModification >= lastModification);
            }

            return result.ToList();
        }

        /// <summary>
        ///   Retourne la liste de codes majoration associés à la société
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe associé aux code majoration à retourner</param>
        /// <returns>La liste des codes majoration associés à la société</returns>
        public IEnumerable<CodeMajorationEnt> GetCodeMajorationListByGroupeIdAndIsHeurNuit(int groupeId)
        {
            var codesMajo = new List<CodeMajorationEnt>();
            foreach (var codeMaj in Context.CodesMajoration
                                           .Where(cm => cm.GroupeId == groupeId && cm.IsHeureNuit)
                                           .Select(cm => new
                                           {
                                               CodeMajorationEnt = cm,
                                               CICodes = cm.CICodesMajoration.Select(cicm => cicm.CodeMajorationId)
                                           })
                                           .ToList())
            {
                CodeMajorationEnt codeMajo = new CodeMajorationEnt
                {
                    CodeMajorationId = codeMaj.CodeMajorationEnt.CodeMajorationId,
                    Code = codeMaj.CodeMajorationEnt.Code,
                    Libelle = codeMaj.CodeMajorationEnt.Libelle,
                    EtatPublic = codeMaj.CodeMajorationEnt.EtatPublic,
                    IsActif = codeMaj.CodeMajorationEnt.IsActif
                };
                codesMajo.Add(codeMajo);
            }

            return codesMajo;
        }

        #region CICodeMajoration

        /// <summary>
        ///   Retourne la liste complète de CICodeMajorations
        /// </summary>
        /// <returns>Une liste de CICodeMajorations</returns>
        public IEnumerable<CICodeMajorationEnt> GetCiCodeMajorationList()
        {
            foreach (CICodeMajorationEnt codeMaj in Context.CICodeMajorations)
            {
                yield return codeMaj;
            }
        }

        /// <summary>
        ///   Retourne la liste complète de CICodeMajorations pour la synchronisation mobile
        /// </summary>
        /// <returns>Une liste de CICodeMajorations</returns>
        public IEnumerable<CICodeMajorationEnt> GetCiCodeMajorationListSync()
        {
            return Context.CICodeMajorations.AsNoTracking();
        }

        /// <summary>
        ///   Cherche une liste d'ID de code majoration en fonction d'un ID de CI
        /// </summary>
        /// <param name="ciId">ID du CI pour lequel on recherche les IDs de codes majoration correspndants</param>
        /// <returns>Une liste d'ID de CodeMajoration.</returns>
        public List<int> GetCodeMajorationIdsByCiId(int ciId)
        {
            return Context.CICodeMajorations
                          .Where(cicm => cicm.CiId == ciId)
                          .Select(cicm => cicm.CodeMajorationId)
                          .ToList();
        }

        /// <summary>
        ///   Création d'une nouvelle association CI/Code majoration
        /// </summary>
        /// <param name="codeMaj"> Association CI/Code majoration à ajouter</param>
        /// <returns>CICodeMajoration ajouté ou mis à jour</returns>
        public CICodeMajorationEnt AddOrUpdateCICodeMajoration(CICodeMajorationEnt codeMaj)
        {
            Context.CICodeMajorations.Add(codeMaj);

            return codeMaj;
        }

        /// <summary>
        ///   Création et/ou mise à jour d'une liste d'associations CI/Code majoration
        /// </summary>    
        /// <param name="ciCodeMajorationList"> Association CI/Code majoration à ajouter</param>
        /// <returns>Liste des CICodeMajoration ajouté et/ou mis à jour</returns>
        public IEnumerable<CICodeMajorationEnt> AddOrUpdateCICodeMajorationList(IEnumerable<CICodeMajorationEnt> ciCodeMajorationList)
        {
            foreach (CICodeMajorationEnt cm in ciCodeMajorationList.ToList())
            {
                if (cm.CiCodeMajorationId.Equals(0))
                {
                    Context.CICodeMajorations.Add(cm);
                }
                else
                {
                    Context.CICodeMajorations.Update(cm);
                }
            }

            return ciCodeMajorationList;
        }

        /// <summary>
        ///   Supprime une association entre un code majoration et un CI
        /// </summary>
        /// <param name="codeMajId">L'identifiant du CodeMajoration à supprimer</param>
        /// <param name="ciId">L'identifiant du ciId à supprimer</param>
        public void DelteCICodeMajoration(int codeMajId, int ciId)
        {
            CICodeMajorationEnt ciCm = Context.CICodeMajorations.FirstOrDefault(cm => cm.CodeMajorationId == codeMajId && cm.CiId == ciId);
            Context.CICodeMajorations.Remove(ciCm);
        }

        #endregion CICodeMajoration


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