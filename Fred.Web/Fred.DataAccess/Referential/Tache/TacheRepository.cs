using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Referential.Common;
using Fred.Entities;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Web.Shared.Models.OperationDiverse;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.Tache
{
    /// <summary>
    ///   Référentiel de données pour les tache.
    /// </summary>
    public class TacheRepository : FredRepository<TacheEnt>, ITacheRepository
    {
        private readonly ILogManager logManager;
        private readonly ITacheSearchHelper taskSearchHelper;

        public TacheRepository(FredDbContext context, ILogManager logManager, ITacheSearchHelper taskSearchHelper)
          : base(context)
        {
            this.logManager = logManager;
            this.taskSearchHelper = taskSearchHelper;
        }

        /// <summary>
        ///   Detache une tache
        /// </summary>
        /// <param name="tacheEnt">L'entité tâche</param>
        public void DetachEntity(TacheEnt tacheEnt)
        {
            TacheEnt tache = Context.Taches.Find(tacheEnt.TacheId);
            var tacheDbEntityEntry = Context.Entry(tache);
            tacheDbEntityEntry.State = EntityState.Detached;
        }

        /// <summary>
        ///   Recherche les taches par le CI
        /// </summary>
        /// <param name="text">Un texte</param>
        /// <param name="ciId">L'identifiant CI</param>
        /// <returns>Une liste de tâches</returns>
        public IEnumerable<TacheEnt> SearchTachesByCiTousNiveaux(string text, int ciId)
        {
            foreach (var tache in Context.Taches.Where(t => (t.Code.ToLower().Contains(text.ToLower()) || t.Libelle.ToLower().Contains(text.ToLower())) && t.CiId.Equals(ciId)))
            {
                if (!taskSearchHelper.IsTacheEcart(tache))
                {
                    yield return tache;
                }
            }
        }

        /// <summary>
        ///   Appel la procédure stockée VerificationDeDependance qui permet de vérifier les dépendances d'une tâche
        /// </summary>
        /// <param name="tache">Tâche à vérifier</param>
        /// <returns>Retourne Vrai si la tâche est supprimable, sinon Faux</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(TacheEnt tache)
        {
            bool isDeletable = false;
            SqlParameter[] parameters =
            {
                new SqlParameter("origTableName", "FRED_TACHE"),
                new SqlParameter("exclusion", string.Empty),
                new SqlParameter("dependance", string.Empty),
                new SqlParameter("origineId", tache.TacheId),
                new SqlParameter("delimiter", string.Empty),
                new SqlParameter("resu", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            // ReSharper disable once CoVariantArrayConversion
            Context.Database.ExecuteSqlCommand("VerificationDeDependance @origTableName, @exclusion, @dependance, @origineId, @delimiter, @resu OUTPUT", parameters);

            // Vérifie s'il y a aucune dépendances (paramètre "resu")
            if (Convert.ToInt32(parameters.First(x => x.ParameterName == "resu").Value) == 0)
            {
                isDeletable = true;
            }

            return isDeletable;
        }

        /// <summary>
        ///   Retourne la liste des toutes les taches.
        /// </summary>
        /// <returns>Liste des tache.</returns>
        public IEnumerable<TacheEnt> GetAllTacheList()
        {
            foreach (var tache in Context.Taches)
            {
                if (!taskSearchHelper.IsTacheEcart(tache))
                {
                    yield return tache;
                }
            }
        }

        /// <summary>
        ///   Retourne la liste des toutes les taches pour la synchronisation mobile.
        /// </summary>
        /// <returns>Liste des tache.</returns>
        public IEnumerable<TacheEnt> GetAllTacheListSync()
        {
            foreach (var tache in Context.Taches.AsNoTracking())
            {
                if (!taskSearchHelper.IsTacheEcart(tache))
                {
                    yield return tache;
                }
            }
        }

        /// <summary>
        ///   Retourne la liste des tache.
        /// </summary>
        /// <returns>Liste des tache.</returns>
        public IEnumerable<TacheEnt> GetTacheList()
        {
            foreach (var tache in Context.Taches.Where(s => !s.DateSuppression.HasValue))
            {
                if (!taskSearchHelper.IsTacheEcart(tache))
                {
                    yield return tache;
                }
            }
        }

        /// <summary>
        ///   Retourne la liste des tache pour un CI donné.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="includeTacheEcart">Indique si les tâches d'écart doivent être incluse.</param>
        /// <returns>Liste des tache.</returns>
        public IEnumerable<TacheEnt> GetTacheListByCiId(int ciId, bool includeTacheEcart = false)
        {
            foreach (var tache in Context.Taches.Where(t => t.CiId.Equals(ciId) && !t.DateSuppression.HasValue))
            {
                if (includeTacheEcart || !taskSearchHelper.IsTacheEcart(tache))
                {
                    yield return tache;
                }
            }
        }

        /// <summary>
        ///   Retourne la liste des tache de Niveau 1 ainsi que leurs tâches enfants pour un CI donné.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="includeTacheEcart">Indique si les tâches d'écart doivent être incluse.</param>
        /// <returns>Liste des tache.</returns>
        public IEnumerable<TacheEnt> GetTacheLevel1ByCiId(int ciId, bool includeTacheEcart = false)
        {
            var tasks = Context.Taches
                .Include(t => t.TachesEnfants)
                .ThenInclude(te => te.TachesEnfants)
                .Where(t => t.CiId == ciId && !t.DateSuppression.HasValue && t.Niveau == 1);

            foreach (var task in tasks)
            {
                if (includeTacheEcart || !taskSearchHelper.IsTacheEcart(task))
                {
                    yield return task;
                }
            }
        }

        /// <summary>
        ///   Retourne la liste des taches pour un CI et une tache parent.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="parentId">Identifiant du parent</param>
        /// <returns>Liste des tache.</returns>
        public IEnumerable<TacheEnt> GetTacheListByCiIdAndParentId(int ciId, int parentId)
        {
            foreach (var tache in Context.Taches.Where(t => t.CiId.Equals(ciId) && t.ParentId.Equals(parentId) && !t.DateSuppression.HasValue))
            {
                if (!taskSearchHelper.IsTacheEcart(tache))
                {
                    yield return tache;
                }
            }
        }

        /// <summary>
        ///   Retourne la liste des taches pour un CI et un niveau de tache.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="niveau">Niveau de la tache</param>
        /// <returns>Liste des tache.</returns>
        public IEnumerable<TacheEnt> GetTacheListByCiIdAndNiveau(int ciId, int niveau)
        {
            foreach (var tache in Context.Taches.Where(t => t.CiId.Equals(ciId) && t.Niveau.HasValue && t.Niveau.Value.Equals(niveau) && !t.DateSuppression.HasValue))
            {
                if (!taskSearchHelper.IsTacheEcart(tache))
                {
                    yield return tache;
                }
            }
        }

        /// <summary>
        /// Retourne la liste des taches actives pour un CI et un niveau de tache.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="niveau">Niveau de la tache</param>
        /// <returns>Liste des taches actives.</returns>
        public IEnumerable<TacheEnt> GetActiveTacheListByCiIdAndNiveau(int ciId, int niveau)
        {
            foreach (var tache in Context.Taches.Where(tache => tache.CiId == ciId && tache.Niveau.HasValue && tache.Niveau == niveau && tache.Active && !tache.DateSuppression.HasValue))
            {
                if (!taskSearchHelper.IsTacheEcart(tache))
                {
                    yield return tache;
                }
            }
        }

        public async Task<IEnumerable<TacheEnt>> GetActiveTacheListByCiIdAndNiveauAsync(int ciId, int niveau)
        {
            return await Context.Taches
                .Where(tache => tache.CiId == ciId
                                && tache.Niveau.HasValue
                                && tache.Niveau == niveau
                                && tache.Active &&
                                !tache.DateSuppression.HasValue)
                .ToListAsync();
        }

        /// <summary>
        ///   Retourne le tache dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="tacheId">Identifiant de la tache à retrouver.</param>
        /// <returns>Le tache retrouvé, sinon nulle.</returns>
        public TacheEnt GetTacheById(int tacheId)
        {
            TacheEnt tache = Context.Taches.Where(t => t.TacheId == tacheId).FirstOrDefault();
            if (tache != null)
            {
                tache.TachesEnfants = Context.Taches.Where(t => t.ParentId == tacheId && !t.DateSuppression.HasValue).ToList();
            }
            return tache;
        }

        public TacheEnt GetParentTacheById(int tacheId)
        {
            return Context.Taches.Where(t => t.TacheId == tacheId).FirstOrDefault();
        }

        /// <summary>
        ///   Vérifie que le CI possède une tache par défaut
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Vrai si le CI contient une tâche  par défaut.</returns>
        public bool CheckDefaultTache(int ciId)
        {
            int count = Context.Taches.Where(t => t.CiId.Equals(ciId) && t.TacheParDefaut).Count();
            return count >= 1;
        }

        /// <summary>
        ///   Ajout une nouvelle tache
        /// </summary>
        /// <param name="tacheEnt"> tache à ajouter</param>
        /// <returns> L'identifiant de la tache ajouté</returns>
        public int AddTache(TacheEnt tacheEnt)
        {
            Insert(tacheEnt);

            return tacheEnt.TacheId;
        }

        /// <summary>
        ///   Insertion de masse de tâches
        /// </summary>
        /// <param name="taches">liste de tâches</param>
        public void BulkInsert(IEnumerable<TacheEnt> taches)
        {
            using (var ctxt = new FredDbContext())
            {
                using (var dbContextTransaction = ctxt.Database.BeginTransaction())
                {
                    try
                    {
                        // disable detection of changes
                        ctxt.ChangeTracker.AutoDetectChangesEnabled = false;

                        // Ajout des tâches en masse en BDD           
                        ctxt.Taches.AddRange(taches);

                        ctxt.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new FredRepositoryException(e.Message, e);
                    }
                    finally
                    {
                        // re-enable detection of changes
                        ctxt.ChangeTracker.AutoDetectChangesEnabled = true;
                    }
                }
            }
        }

        /// <summary>
        ///   Sauvegarde les modifications d'une tache.
        /// </summary>
        /// <param name="tacheEnt">tache à modifier</param>
        public void UpdateTache(TacheEnt tacheEnt)
        {
            try
            {
                DetachEntity(tacheEnt);
                if (tacheEnt.TachesEnfants != null && tacheEnt.TachesEnfants.Count > 0)
                {
                    foreach (TacheEnt child2 in tacheEnt.TachesEnfants)
                    {
                        DetachEntity(child2);
                        child2.Parent = null;
                        if (child2.TachesEnfants != null && child2.TachesEnfants.Count > 0)
                        {
                            foreach (TacheEnt child3 in child2.TachesEnfants)
                            {
                                child3.Parent = null;
                                DetachEntity(child3);
                            }
                        }
                    }
                }

                Update(tacheEnt);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Supprime une tache
        /// </summary>
        /// <param name="id">L'identifiant de la tache à supprimer</param>
        public void DeleteTacheById(int id)
        {
            DeleteById(id);
        }

        /// <summary>
        ///   Vérifie que le code de la tâche n'est pas déjà utilisé
        /// </summary>
        /// <param name="code">Code de la tache à comparer</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Vrai si le code de la tâche existe, faux sinon</returns>
        public bool IsCodeTacheExist(string code, int ciId)
        {
            foreach (TacheEnt tache in Context.Taches.Where(t => t.CiId.Equals(ciId)))
            {
                if (tache.Code.ToLower().Equals(code.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///   Recherche une liste de tache pour un CI et un niveau donné.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des tache.</param>
        /// <param name="ciId">L'identifiant CI propriétaire des tâches.</param>
        /// <param name="niveau">Le niveau de la tâche</param>
        /// <returns>Une liste de tache.</returns>
        public IEnumerable<TacheEnt> SearchTachesByCiAndNiveau(string text, int ciId, int niveau)
        {
            foreach (var tache in Context.Taches.Where(t => (t.Code.ToLower().Contains(text.ToLower()) || t.Libelle.ToLower().Contains(text.ToLower())) && t.CiId.Equals(ciId) && t.Niveau.Equals(niveau)))
            {
                if (!taskSearchHelper.IsTacheEcart(tache))
                {
                    yield return tache;
                }
            }
        }

        /// <summary>
        ///   Permet la récupération de la tâche par défaut.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne la tâche par défaut trouvée, sinon null</returns>
        public TacheEnt GetTacheParDefaut(int ciId)
        {
            return Context.Taches.FirstOrDefault(t => t.TacheParDefaut && t.CiId == ciId && t.Niveau == 3);
        }

        /// <summary>
        ///   Détermine si une tâche peut être supprimée ou pas (en fonction de ses dépendances)
        /// </summary>
        /// <param name="tache">Tâche à vérifier</param>
        /// <returns>Retourne Vrai si la tâche est supprimable, sinon Faux</returns>
        public bool IsDeletable(TacheEnt tache)
        {
            if (taskSearchHelper.IsTacheEcart(tache))
            {
                return false;
            }
            return AppelTraitementSqlVerificationDesDependances(tache);
        }

        /// <inheritdoc />
        public IEnumerable<TacheEnt> GetTaches(IEnumerable<int> ciIds, DateTime lastModification = default(DateTime))
        {
            var result = from e in Context.Taches where ciIds.Contains(e.CiId) select e;

            // Synchornisation delta.
            if (lastModification != default(DateTime))
            {
                result = result.Where(t => t.DateModification >= lastModification);
            }

            foreach (var tache in result)
            {
                if (!taskSearchHelper.IsTacheEcart(tache))
                {
                    yield return tache;
                }
            }
        }

        /// <summary>
        /// Retourne les taches T1 liée à ce CI est créée avant la date passée en paramètre
        /// </summary>
        /// <param name="ciId">Identifiant du CI.</param>
        /// <param name="date">Date limite de création des tâches récupérées</param>
        /// <param name="includeTacheEcart">inclure les tâches d'écart</param>
        /// <returns>Un IEnumerable potentiellement vide, jamais null</returns>
        public IEnumerable<TacheEnt> GetAllT1ByCiId(int ciId, DateTime? date, bool includeTacheEcart = false)
        {
            foreach (var tache in Context.Taches.Where(t => t.CiId.Equals(ciId) && !t.DateSuppression.HasValue && (date == null || t.DateCreation < date)))
            {
                if ((includeTacheEcart || !taskSearchHelper.IsTacheEcart(tache)) && tache.Niveau == 1)
                {
                    yield return tache;
                }
            }
        }

        /// <summary>
        /// Retourne les taches T1,T2,T3 liées aux T1 passées en pramétre. 
        /// </summary>
        /// <param name="listIdTaches">Identifiants des taches niveau 1.</param>
        /// <returns>Un IEnumerable des taches</returns>
        public IEnumerable<TacheEnt> GetTAssocitedToT1(List<int> listIdTaches)
        {
            return Context.Taches.Where(t => listIdTaches.Distinct().Any(x => x == t.TacheId)).Include(y => y.TachesEnfants).ThenInclude(x => x.TachesEnfants).ToList();
        }

        /// <summary>
        /// Permet de récupérer une tâches.
        /// </summary>
        /// <param name="code">Le code de la tache.</param>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une tâche.</returns>
        public TacheEnt GetTache(string code, int ciId, bool asNoTracking = false)
        {
            try
            {
                if (asNoTracking)
                {
                    return Context.Taches.AsNoTracking().FirstOrDefault(t => t.CiId == ciId && t.Code == code);
                }
                return Context.Taches.FirstOrDefault(t => t.CiId == ciId && t.Code == code);
            }
            catch (Exception exception)
            {
                throw new FredRepositoryException(exception.Message, exception);
            }
        }

        /// <inheritdoc />
        public void InsertInMass(List<TacheEnt> taches)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    foreach (TacheEnt tache in taches)
                    {
                        // On ajoute une date de clôture comptable.
                        tache.DateModification = tache.DateCreation = DateTime.UtcNow;
                        Context.Taches.Add(tache);
                    }

                    Context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// Retourne les tâches d'un CI du niveau désiré.
        /// </summary>
        /// <typeparam name="TTache">Le type de tâche souhaité.</typeparam>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <param name="niveau">Le niveau désiré.</param>
        /// <param name="onlyActive">Indique si seules les tâches actives sont concernées.</param>
        /// <param name="includeTacheEcart">Indique si les tâches d'écart doivent être incluse.</param>
        /// <param name="selector">Selector permettant de construire un TTache a partir d'une unité.</param>
        /// <returns>Les tâches de niveau 4 du CI.</returns>
        public List<TTache> GetTaches<TTache>(int ciId, int niveau, bool onlyActive, bool includeTacheEcart, Expression<Func<TacheEnt, TTache>> selector)
        {
            var query = Context.Taches.Where(t => t.CiId == ciId && !t.DateSuppression.HasValue && t.Niveau == niveau);
            if (!includeTacheEcart)
            {
                query = query.Where(taskSearchHelper.IsNotTacheEcartExpression);
            }
            if (onlyActive)
            {
                query = query.Where(taskSearchHelper.IsTacheActiveAndNotDeletedExpression);
            }
            return query
                .Select(selector)
                .ToList();
        }

        /// <summary>
        /// Récupère les tâches pour la comparaison de budget.
        /// </summary>
        /// <param name="tacheIds">Les identifiants des tâches concernées.</param>
        /// <returns>Les tâches.</returns>
        public List<AxeInfoDao> GetPourBudgetComparaison(IEnumerable<int> tacheIds)
        {
            return Context.Taches
                .Where(t => tacheIds.Contains(t.TacheId))
                .Select(t => new AxeInfoDao
                {
                    Id = t.TacheId,
                    Code = t.Code,
                    Libelle = t.Libelle
                })
                .ToList();
        }

        /// <summary>
        ///   Méthode de recherche de tâches dans le référentiel
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="activeOnly">Tache active ou pas</param>
        /// <param name="niveauTache">Niveau de la tâche, 3 par défaut</param>
        /// <param name="isTechnicalTask">Détermine si l'on prend les tâches techniques ou pas</param>
        /// <returns>Retourne une liste d'items de référentiels</returns>
        public IEnumerable<TacheEnt> SearchLight(string text, int page, int pageSize, int? ciId, bool activeOnly, int niveauTache, bool isTechnicalTask)
        {
            int[] technicalTasks = taskSearchHelper.GetTechnicalTasks();

            var tachesReturned = Context.Taches
                .Include(x => x.Parent.Parent)
                .Where(t => (!ciId.HasValue || t.CiId == ciId.Value)
                            && !t.DateSuppression.HasValue
                            && (!activeOnly || t.Active)
                            && (isTechnicalTask || !technicalTasks.Contains(t.TacheType)
                                && t.Niveau == niveauTache
                                &&
                                (string.IsNullOrEmpty(text)
                                 || t.Code.Contains(text)
                                 || t.Libelle.Contains(text))
                            ))
                .OrderBy(m => m.Code)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return tachesReturned;
        }

        public async Task<IEnumerable<TacheEnt>> SearchLightAsync(int ciId, int page, int pageSize, string recherche, bool activeOnly, int niveauTache = 3)
        {
            return await Context.Taches
                .Include(t => t.Parent).ThenInclude(t => t.Parent)
                .Where(t => t.CiId == ciId
                            && !t.DateSuppression.HasValue
                            && t.Active
                            && t.Niveau == niveauTache
                            && (string.IsNullOrEmpty(recherche) || t.Code.Contains(recherche) ||
                                t.Libelle.Contains(recherche)))
                .OrderBy(t => t.Code)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        ///   Génère des propositions de nouveaux codes pour des tâches enfant en fonction de la tâche parente
        /// </summary>
        /// <param name="parentTask">La tâche parente</param>
        /// <returns>Retourne les nouveaux codes</returns>
        public List<TacheEnt> GetParentTache(TacheEnt parentTask)
        {
            return Context.Taches.Where(t => t.ParentId == parentTask.TacheId).ToList();
        }


        /// <summary>
        /// Récupère les tâches pour des paires CiId + CodeTache
        /// </summary>
        /// <param name="listCiIdAndTacheCode">Paire CiId + CodeTache</param>
        /// <returns>Les tâches</returns>
        public List<TacheEnt> Get(List<CiIdAndTacheCodeModel> listCiIdAndTacheCode)
        {
            var ciIds = listCiIdAndTacheCode.Select(x => x.CiId).ToList();

            var codes = listCiIdAndTacheCode.Select(x => x.Code).ToList();

            var taches = Context.Taches.Where(x => ciIds.Contains(x.CiId) && codes.Contains(x.Code))
                                       .Select(x => new { x.TacheId, x.CiId, x.Code }).ToList();

            var tachesIds = (from tache in taches
                             join ciIdAndTacheCode in listCiIdAndTacheCode on new { tache.CiId, tache.Code } equals new { ciIdAndTacheCode.CiId, ciIdAndTacheCode.Code }
                             select tache.TacheId).ToList();

            var result = Context.Taches.Include(x => x.Parent.Parent).Where(x => tachesIds.Contains(x.TacheId)).ToList();

            return result;
        }

        public TacheEnt GetTacheLitigeByCiId(int ciId)
        {
            return Context.Taches.FirstOrDefault(t => t.CiId == ciId && !t.DateSuppression.HasValue && t.Code == Constantes.TacheSysteme.CodeTacheLitige);
        }

        public int? GetTacheIdInterimByCiId(int ciId)
        {
            return Context.Taches
                .FirstOrDefault(tache => tache.CiId == ciId && tache.TacheType == TacheType.EcartInterim.ToIntValue() && tache.Active && tache.Niveau == 3)?.TacheId;
        }

        public List<TacheEnt> GetListByCi(int ciId)
        {
            return Context.Taches
                .Where(t => t.CiId == ciId)
                .ToList();
        }
    }
}
