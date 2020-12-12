using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.EtablissementPaie
{
    /// <summary>
    ///   Référentiel de données pour les établissements comptables.
    /// </summary>
    public class EtablissementPaieRepository : FredRepository<EtablissementPaieEnt>, IEtablissementPaieRepository
    {
        private readonly ILogManager logManager;

        public EtablissementPaieRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Appel à une procédure stockée
        /// </summary>
        /// <param name="etablissementPaieEnt">L' établissement paie à vérifier les dépendances</param>
        /// <returns>Renvoie un bool</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(EtablissementPaieEnt etablissementPaieEnt)
        {
            int etabPaieFilleId = 0;
            if (etablissementPaieEnt.IsAgenceRattachement)
            {
                etabPaieFilleId = GetEtabPaieFilledByEtabComptId(etablissementPaieEnt.EtablissementPaieId);
            }

            if (etabPaieFilleId == 0)
            {
                int? personnelId = GetPersonnelIdByEtabComptId(etablissementPaieEnt.EtablissementPaieId);

                DbConnection sqlConnection = Context.Database.GetDbConnection();
                DbCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "VerificationDeDependance";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 240;

                sqlCommand.Parameters.Add(new SqlParameter("@origTableName", "FRED_ETABLISSEMENT_PAIE"));
                sqlCommand.Parameters.Add(new SqlParameter("@exclusion", string.Empty));
                sqlCommand.Parameters.Add(new SqlParameter("@dependance", personnelId.HasValue ? $"'FRED_PERSONNEL',{personnelId}" : string.Empty));
                sqlCommand.Parameters.Add(new SqlParameter("@origineId", etablissementPaieEnt.EtablissementPaieId));
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
            }

            return false;
        }

        /// <summary>
        ///   Permet de récupérer l'id d'un etablisement paie fille lié à l'établissement paie mère.
        /// </summary>
        /// <param name="id">Identifiant de l'établissement paie</param>
        /// <returns>Retourne l'identifiant de l'agence fille</returns>
        private int GetEtabPaieFilledByEtabComptId(int? id)
        {
            if (id == null)
            {
                return 0;
            }

            return Context.EtablissementsPaie.Where(s => s.AgenceRattachementId == id).Select(s => s.EtablissementPaieId).FirstOrDefault();
        }

        /// <summary>
        ///   Permet de récupérer l'id d'un personnel lié à l'établissement paie.
        /// </summary>
        /// <param name="id">Identifiant de l'établissement paie</param>
        /// <returns>Retourne l'identifiant de la personne</returns>
        private int? GetPersonnelIdByEtabComptId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.Personnels.Where(s => s.EtablissementPaieId == id).Select(s => s.PersonnelId).FirstOrDefault();
        }

        /// <summary>
        ///   Retourne la liste des établissements comptables.
        /// </summary>
        /// <returns>La liste des établissements comptables.</returns>
        public IEnumerable<EtablissementPaieEnt> GetEtablissementPaieList()
        {
            foreach (EtablissementPaieEnt etablissementPaie in Context.EtablissementsPaie)
            {
                yield return etablissementPaie;
            }
        }

        /// <summary>
        ///   Méthode GET de récupération de tous les établissements de paie éligibles à être une agence de rattachement
        /// </summary>
        /// <param name="currentEtabPaieId">ID de l'établissement de paie à exclure de la recherche</param>
        /// <returns>Retourne une nouvelle instance d'établissement de paie intialisée</returns>
        public IEnumerable<EtablissementPaieEnt> AgencesDeRattachement(int currentEtabPaieId)
        {
            IEnumerable<EtablissementPaieEnt> etablissementPaieEnts = Context.EtablissementsPaie
                .Where(ep => ep.Actif && ep.EtablissementPaieId != currentEtabPaieId && ep.IsAgenceRattachement);

            foreach (EtablissementPaieEnt etablissementPaie in etablissementPaieEnts)
            {
                yield return etablissementPaie;
            }
        }

        /// <summary>
        ///   Retourne l'établissement comptable portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="etablissementPaieId">Identifiant de l'établissement comptable à retrouver.</param>
        /// <returns>L'établissement comptable retrouvé, sinon nulle.</returns>
        public EtablissementPaieEnt GetEtablissementPaieById(int etablissementPaieId)
        {
            return Context.EtablissementsPaie.Find(etablissementPaieId);
        }

        /// <summary>
        ///   Permet de connaître l'existence d'un établissement de paie depuis son code et son libellé.
        /// </summary>
        /// <param name="idCourant">id etablissement</param>
        /// <param name="code">code eablissement</param>
        /// <param name="libelle">libelle etablissement</param>
        /// <returns>Retourne vrai si le code et le libellé passés en paramètres existent déjà, faux sinon</returns>
        public bool IsEtablissementPaieExistsByCodeLibelle(int idCourant, string code, string libelle)
        {
            if (idCourant != 0)
            {
                return Context.EtablissementsPaie.Any(ep => ep.Code == code && ep.EtablissementPaieId != idCourant && ep.Libelle == libelle);
            }

            return Context.EtablissementsPaie.Any(ep => ep.Code == code && ep.Libelle == libelle);
        }

        /// <summary>
        ///   Ajoute un nouvel établissement comptable.
        /// </summary>
        /// <param name="etablissementPaie">Etablissement comptable à ajouter</param>
        /// <returns>L'identifiant de l'établissement comptable ajouté</returns>
        public int AddEtablissementPaie(EtablissementPaieEnt etablissementPaie)
        {
            try
            {
                etablissementPaie.Pays = null;
                etablissementPaie.EtablissementComptable = null;
                Context.EtablissementsPaie.Add(etablissementPaie);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return etablissementPaie.EtablissementPaieId;
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un établissement comptable.
        /// </summary>
        /// <param name="etablissementPaie">Etablissement comptable à modifier</param>
        public void UpdateEtablissementPaie(EtablissementPaieEnt etablissementPaie)
        {
            try
            {
                etablissementPaie.Pays = null;
                etablissementPaie.EtablissementComptable = null;
                etablissementPaie.AgenceRattachement = null;

                Context.Entry(etablissementPaie).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Supprime un établissement comptable.
        /// </summary>
        /// <param name="etablissementPaieId">L'identifiant du établissement comptable à supprimer</param>
        public void DeleteEtablissementPaieById(int etablissementPaieId)
        {
            EtablissementPaieEnt etablissement = Context.EtablissementsPaie.Find(etablissementPaieId);
            if (etablissement == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.EtablissementsPaie.Remove(etablissement);

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
        ///   Vérifie s'il est possible de supprimer un établissement paie
        /// </summary>
        /// <param name="etablissementPaieEnt">L'établissement paie à supprimer</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(EtablissementPaieEnt etablissementPaieEnt)
        {
            return AppelTraitementSqlVerificationDesDependances(etablissementPaieEnt);
        }

        /// <summary>
        ///   Ligne de recherche
        /// </summary>
        /// <param name="text">Le text recherché</param>
        /// <returns>Renvoie une liste</returns>
        public IEnumerable<EtablissementPaieEnt> SearchLight(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return QueryPagingHelper.ApplyScrollPaging(GetEtablissementPaieList().AsQueryable());
            }

            return QueryPagingHelper.ApplyScrollPaging(Context.EtablissementsPaie.Where(m => m.Actif)
                                                              .Include(x => x.AgenceRattachement)
#pragma warning disable RCS1155 // Use StringComparison when comparing strings.
                                                        .Where(c => c.Code.ToLower().Contains(text.ToLower()) || c.Libelle.ToLower().Contains(text.ToLower()))
#pragma warning restore RCS1155 // Use StringComparison when comparing strings.
                                                        .AsQueryable());
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les établissements en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="predicate">Prédicat de recherche de l'etablissement paie</param>
        /// <returns>Retourne la liste filtrée de tous les etablissements paie</returns>
        public IEnumerable<EtablissementPaieEnt> SearchEtablissementPaieAllWithFilters(int societeId, Expression<Func<EtablissementPaieEnt, bool>> predicate)
        {
            return Context.EtablissementsPaie
                .Include(x => x.Pays)
                .Include(x => x.EtablissementComptable)
                .Where(c => c.SocieteId == societeId)
                .Where(predicate).OrderBy(s => s.Code);
        }

        /// <summary>
        ///   Permet de connaître l'existence d'un code déplacement depuis son code.
        /// </summary>
        /// <param name="idCourant">Identifiant du code déplacement courant</param>
        /// <param name="code">Code de deplacement à vérifier</param>
        /// <param name="societeId">Identifiant de la société à laquelle les codes deplacements sont rattachés</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon.</returns>
        public bool IsCodeEtablissementPaieExistsByCode(int idCourant, string code, int societeId)
        {
            if (idCourant != 0)
            {
                return Context.EtablissementsPaie.Where(c => c.SocieteId == societeId).Any(cd => cd.Code.Equals(code) && cd.EtablissementPaieId != idCourant);
            }

            return Context.EtablissementsPaie.Where(c => c.SocieteId == societeId).Any(cd => cd.Code.Equals(code));
        }

        /// <summary>
        /// Get etablissement paie by list des societes ids
        /// </summary>
        /// <param name="societesIdList">List des societes id</param>
        /// <returns>List des etabs Paie</returns>
        public async Task<List<EtablissementPaieEnt>> GetEtabPaieBySocieteIdList(List<int> societesIdList)
        {
            if (!societesIdList.Any()) return new List<EtablissementPaieEnt>();
            return await Context.EtablissementsPaie.Where(c => societesIdList.Contains(c.SocieteId.Value)).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }
    }
}
