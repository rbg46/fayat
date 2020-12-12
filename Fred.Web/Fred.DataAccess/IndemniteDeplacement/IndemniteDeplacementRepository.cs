using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.IndemniteDeplacement;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.IndemniteDeplacement

{
    /// <summary>
    ///   Référentiel de données pour les indemnites de deplacement.
    /// </summary>
    public class IndemniteDeplacementRepository : FredRepository<IndemniteDeplacementEnt>, IIndemniteDeplacementRepository
    {
        private readonly ILogManager logManager;

        public IndemniteDeplacementRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Appel à une procédure stockée
        /// </summary>
        /// <param name="holdingId">Id du Holding</param>
        /// <returns>Renvoie un int</returns>
        protected int AppelTraitementSqlImpIndemniteDeplacementFromHolding(int holdingId)
        {
            int nbcmd = 0;

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            DbCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "ImportIndemniteDeplacementFromHolding";
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
        ///   Permet de détacher les entités dépendantes pour éviter de les prendre en compte dans la sauvegarde du contexte.
        /// </summary>
        /// <param name="indeDep">objet dont les dépendances sont à détacher</param>
        private void DetachDependancies(IndemniteDeplacementEnt indeDep)
        {
            indeDep.CI = null;
            indeDep.CodeDeplacement = null;
            indeDep.CodeZoneDeplacement = null;
            indeDep.Personnel = null;
        }

        /// <summary>
        ///   Ajout une nouvelle société
        /// </summary>
        /// <param name="indemniteDeplacement"> Indemnite de deplacement à ajouter</param>
        /// <returns>L'identifiant de l'indemnite de deplacement ajouté</returns>
        public int AddIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacement)
        {
            try
            {
                if (Context.Entry(indemniteDeplacement).State == EntityState.Detached)
                {
                    DetachDependancies(indemniteDeplacement);
                }

                Context.IndemniteDeplacement.Add(indemniteDeplacement);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return indemniteDeplacement.IndemniteDeplacementId;
        }

        /// <summary>
        ///   Supprime une indemnite de deplacement
        /// </summary>
        /// <param name="id">L'identifiant de l'indemnite de deplacement à supprimer</param>
        /// <param name="idUtilisateur">Identifiant de l'utilisateur ayant fait l'action</param>
        public void DeleteIndemniteDeplacementById(int id, int idUtilisateur)
        {
            IndemniteDeplacementEnt indemniteDeplacement = Context.IndemniteDeplacement.Find(id);
            if (indemniteDeplacement == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.Entry(indemniteDeplacement).State = EntityState.Modified;
            indemniteDeplacement.AuteurSuppression = idUtilisateur;
            indemniteDeplacement.DateSuppression = DateTime.Now;

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
        ///   Supprime PHYSIQUEMENT une indemnite de deplacement
        /// </summary>
        /// <param name="id">L'identifiant de l'indemnite de deplacement à supprimer</param>
        public void RemoveIndemniteDeplacementById(int id)
        {
            IndemniteDeplacementEnt indemniteDeplacement = Context.IndemniteDeplacement.Find(id);
            if (indemniteDeplacement == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.Entry(indemniteDeplacement).State = EntityState.Deleted;

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
        ///   La liste de tous les indemnites de deplacement pour un personnel.
        /// </summary>
        /// <param name="personnelId">Id unique d'un personnel</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement pour un personnel</returns>
        public IQueryable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnel(int personnelId)
        {
            return Context.IndemniteDeplacement.Where(y => y.PersonnelId.Equals(personnelId))
                          .Include(o => o.CodeDeplacement)
                          .Include(oo => oo.CodeZoneDeplacement)
                          .Include(ooo => ooo.CI)
                          .Include(oooo => oooo.Personnel)
                          .Include(oooo => oooo.Personnel.EtablissementRattachement)
                          .Include(oooo => oooo.Personnel.EtablissementRattachement.AgenceRattachement);
        }

        /// <summary>
        ///   Retourne l'indemnité de déplacement paramétrée pour un personnel et un Ci
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="ciId">L'identifiant du CI</param>
        /// <returns>Une indemnité de déplacement</returns>
        public IndemniteDeplacementEnt GetIndemniteDeplacementByPersonnelIdAndCiId(int personnelId, int ciId)
        {
            return Context.IndemniteDeplacement.Include(o => o.CodeDeplacement)
                          .Include(oo => oo.CodeZoneDeplacement)
                          .Include(ooo => ooo.CI)
                          .Include(oooo => oooo.Personnel)
                          .Include(oooo => oooo.Personnel.EtablissementRattachement)
                          .Include(oooo => oooo.Personnel.EtablissementRattachement.AgenceRattachement)
                          .Where(i => i.PersonnelId.Equals(personnelId) && i.CiId.Equals(ciId) && !i.DateSuppression.HasValue)
                          .FirstOrDefault();
        }

        /// <summary>
        ///   La liste de tous les indemnites de deplacement pour un personnel et une affaire.
        /// </summary>
        /// <param name="personnelId">Id unique d'un personnel</param>
        /// <param name="ciId">Id unique d'une affaire</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement pour un personnel et une affaire</returns>
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCi(int personnelId, int ciId)
        {
            foreach (IndemniteDeplacementEnt indemniteDeplacement in Context.IndemniteDeplacement
                                                                            .Include(o => o.CodeDeplacement)
                                                                            .Where(y => y.PersonnelId.Equals(personnelId) && y.CiId.Equals(ciId)))
            {
                yield return indemniteDeplacement;
            }
        }

        /// <summary>
        ///   Retourne la liste des indemnites de deplacement par société
        /// </summary>
        /// <returns>Renvoie la liste de des indemnites de deplacement active</returns>
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementList()
        {
            foreach (IndemniteDeplacementEnt indemniteDeplacement in Context.IndemniteDeplacement.Where(y => y.DateSuppression.Equals(null)))
            {
                yield return indemniteDeplacement;
            }
        }

        /// <summary>
        ///   La liste des indemnites de deplacement par société
        /// </summary>
        /// <returns>Renvoie la liste des tous les indemnites de deplacement</returns>
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementListAll()
        {
            foreach (IndemniteDeplacementEnt indemniteDeplacement in Context.IndemniteDeplacement)
            {
                yield return indemniteDeplacement;
            }
        }

        /// <summary>
        ///   La liste de tous les indemnites de deplacement par personnel.
        /// </summary>
        /// <param name="personnelId">Id unique d'un personnel</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement active</returns>
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelListAll(int personnelId)
        {
            foreach (IndemniteDeplacementEnt indemniteDeplacement in Context.IndemniteDeplacement.Where(y => y.PersonnelId.Equals(personnelId)))
            {
                yield return indemniteDeplacement;
            }
        }

        /// <summary>
        ///   La liste de tous les indemnites de deplacement.
        /// </summary>
        /// <param name="personnelId">Id unique d'un personnel</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement active par personnel</returns>
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelList(int personnelId)
        {
            foreach (IndemniteDeplacementEnt indemniteDeplacement in Context.IndemniteDeplacement.Where(y => y.DateSuppression.Equals(null) && y.PersonnelId.Equals(personnelId)))
            {
                yield return indemniteDeplacement;
            }
        }

        /// <summary>
        ///   La liste de tous les indemnites de deplacement par CI.
        /// </summary>
        /// <param name="idCi">id unique d'un CI</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement active</returns>
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCiListAll(int idCi)
        {
            foreach (IndemniteDeplacementEnt indemniteDeplacement in Context.IndemniteDeplacement.Where(y => y.CiId.Equals(idCi)))
            {
                yield return indemniteDeplacement;
            }
        }

        /// <summary>
        ///   La liste de tous les indemnites de deplacement active par CI.
        /// </summary>
        /// <param name="idCi">id unique d'un CI</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement active</returns>
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCiList(int idCi)
        {
            foreach (IndemniteDeplacementEnt indemniteDeplacement in Context.IndemniteDeplacement.Where(y => y.DateSuppression.Equals(null) && y.CiId.Equals(idCi)))
            {
                yield return indemniteDeplacement;
            }
        }

        /// <summary>
        ///   Sauvegarde les modifications d'une IndemniteDeplacement
        /// </summary>
        /// <param name="indemniteDeplacement">Indemnite de deplacement à modifier</param>
        /// <returns>L'identifiant de l'indemnite de deplacement modifiée</returns>
        public int UpdateIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacement)
        {
            try
            {
                if (Context.Entry(indemniteDeplacement).State == EntityState.Detached)
                {
                    DetachDependancies(indemniteDeplacement);
                }

                Context.Entry(indemniteDeplacement).State = EntityState.Modified;
                Context.SaveChanges();

                return indemniteDeplacement.IndemniteDeplacementId;
            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Indemnite de deplacement via l'id
        /// </summary>
        /// <param name="id">Id de l'indemnite de deplacement</param>
        /// <returns>Renvoie une indemnite de deplacement</returns>
        public IndemniteDeplacementEnt GetIndemniteDeplacementById(int id)
        {
            return Context.IndemniteDeplacement.Where(x => x.IndemniteDeplacementId == id)
                          .Include(o => o.CodeDeplacement)
                          .Include(oo => oo.CodeZoneDeplacement)
                          .Include(ooo => ooo.CI)
                          .Include(oooo => oooo.Personnel)
                          .Include(oooo => oooo.Personnel.EtablissementRattachement)
                          .Include(oooo => oooo.Personnel.EtablissementRattachement.AgenceRattachement)
                          .First();
        }

        /// <summary>
        ///   Indemnite de deplacement via personnelid
        /// </summary>
        /// <param name="personnelId">Id du personnel</param>
        /// <returns>Renvoie une indemnite de deplacement</returns>
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelId(int personnelId)
        {
            return Context.IndemniteDeplacement.Where(x => x.PersonnelId == personnelId);
        }

        /// <summary>
        ///   Import des indemnites de deplacement automatiques depuis la Holding
        /// </summary>
        /// <param name="holdingId"> Id du Holding</param>
        /// <returns>Renvoie les indemnites de deplacement</returns>
        public int ImportIndemniteDeplacementFromHolding(int holdingId)
        {
            return AppelTraitementSqlImpIndemniteDeplacementFromHolding(holdingId);
        }

        /// <summary>
        ///   Permet de récupérer la liste des indemnites de deplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur les indemnites de deplacement</param>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>Retourne la liste filtrée des indemnites de deplacement</returns>
        public IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementWithFilters(Expression<Func<IndemniteDeplacementEnt, bool>> predicate, int personnelId)
        {
            return Context.IndemniteDeplacement.Include(i => i.CodeDeplacement)
                          .Include(i => i.CodeZoneDeplacement)
                          .Include(i => i.CI)
                          .Include(i => i.Personnel)
                          .Include(i => i.Personnel.EtablissementRattachement)
                          .Include(i => i.Personnel.EtablissementRattachement.AgenceRattachement)
                          .Where(i => i.PersonnelId == personnelId && !i.DateSuppression.HasValue)
                          .Where(predicate)
                          .OrderBy(i => i.IndemniteDeplacementId);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les indemnites de deplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les indemnites de deplacement</param>
        /// <returns>Retourne la liste filtrée de tous les indemnites de deplacement</returns>
        public IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementAllWithFilters(Func<IndemniteDeplacementEnt, bool> predicate)
        {
            return Context.IndemniteDeplacement.Where(predicate).OrderBy(s => s.IndemniteDeplacementId);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les indemnites de deplacement en fonction des critères de recherche par
        ///   personnel.
        /// </summary>
        /// <param name="personnelId">Id personnel</param>
        /// <returns>Retourne la liste filtrée de tous les indemnites de deplacement</returns>
        public IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementAllByPersonnelIdWithFilters(int personnelId)
        {
            return Context.IndemniteDeplacement.Where(c => c.PersonnelId == personnelId).OrderBy(s => s.IndemniteDeplacementId);
        }

        /// <summary>
        /// Retourne les indemnités de déplacement à utiliser lors de l'export KLM.
        /// </summary>
        /// <param name="societeId">Identifiant de a société</param>
        /// <returns>Les indemnités de déplacement à utiliser lors de l'export KLM</returns>
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementForExportKlm(int societeId)
        {
            return Context.IndemniteDeplacement
              .Include(id => id.CI)
              .Include(id => id.Personnel)
              .Include(id => id.Personnel.Societe)
              .Include(id => id.CodeDeplacement)
              .Include(id => id.CodeZoneDeplacement)
              .Where(id => id.Personnel != null
                        && id.CI != null
                        && id.Personnel.SocieteId == societeId
                        && id.Personnel.IsInterne
                        && !id.DateSuppression.HasValue
                        && id.CI.ChantierFRED);
        }
    }
}
