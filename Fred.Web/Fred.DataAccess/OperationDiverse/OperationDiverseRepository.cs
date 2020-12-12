using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.OperationDiverse;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace Fred.DataAccess.OperationDiverse
{
    /// <summary>
    ///  Repository des OperationDiverseEnt
    /// </summary>
    public class OperationDiverseRepository : FredRepository<OperationDiverseEnt>, IOperationDiverseRepository
    {
        private readonly FredDbContext context;

        public OperationDiverseRepository(ILogManager logManager, IUnitOfWork uow, FredDbContext context)
            : base(context)
        {
            this.context = context;
        }


        /// <summary>
        /// Mets à jour un OD
        /// </summary>
        /// <param name="operationDiverse">Opération Diverse</param>
        /// <returns>OD Mise à jour</returns>
        public OperationDiverseEnt UpdateOD(OperationDiverseEnt operationDiverse)
        {
            ProcessMandatoryFieldsOperationDiverse(operationDiverse);
            Update(operationDiverse);
            return operationDiverse;
        }

        /// <summary>
        /// Mets à jour une liste d'ODs
        /// </summary>
        /// <param name="operationsDiverses">Liste de OperationDiverse</param>
        /// <returns>Liste d'ODs mise à jour</returns>
        public List<OperationDiverseEnt> UpdateListOD(List<OperationDiverseEnt> operationsDiverses)
        {
            operationsDiverses = ProcessMandatoryFieldsOperationsDiverses(operationsDiverses).ToList();
            this.UpdateRange(operationsDiverses);
            return operationsDiverses;
        }

        public async Task<List<OperationDiverseEnt>> UpdateListODAsync(List<OperationDiverseEnt> operationsDiverses)
        {
            IEnumerable<int> operationsDiversesIds = operationsDiverses.Select(o => o.OperationDiverseId);

            IEnumerable<OperationDiverseEnt> odsToUpdate = await GetByIdsAsync(operationsDiversesIds).ConfigureAwait(false);

            if (odsToUpdate != null)
            {
                odsToUpdate = ProcessMandatoryFieldsOperationsDiverses(odsToUpdate).ToList();

                UpdateRange(odsToUpdate);
            }

            return odsToUpdate.ToList();
        }

        /// <summary>
        /// Ajout d'une liste d'opérations diverses
        /// </summary>
        /// <param name="operationsDiverses">Liste d'opérations diverses à ajouter</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt"></see> ajoutées</returns>
        public IReadOnlyCollection<OperationDiverseEnt> AddListOD(IEnumerable<OperationDiverseEnt> operationsDiverses)
        {
            operationsDiverses = ProcessMandatoryFieldsOperationsDiverses(operationsDiverses);
            this.SaveRange(operationsDiverses);
            return operationsDiverses.ToList();
        }

        /// <summary>
        /// Supprime une liste d'opérations diverses
        /// </summary>
        /// <param name="operationsDiverses">Liste d'opérations diverses à supprimer</param>
        public void DeleteListOD(List<OperationDiverseEnt> operationsDiverses)
        {
            operationsDiverses.ForEach(od => this.DeleteById(od.OperationDiverseId));
        }

        /// <summary>
        /// Récupère une liste d'OD
        /// </summary>
        /// <param name="groupRemplacementId">L'id groupe de remplacement</param>
        /// <returns>Liste d'ODs</returns>
        public async Task<IEnumerable<OperationDiverseEnt>> GetByGroupRemplacementIdAsync(int groupRemplacementId)
        {
            return await context.OperationDiverses.Where(d => d.GroupeRemplacementTacheId == groupRemplacementId).Include(x => x.Tache).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<OperationDiverseEnt>> GetByOperationDiverseMereIdAbonnementAsync(int operationDiverseMereIdAbonnement)
        {
            return await context.OperationDiverses
                .Where(od => od.OperationDiverseMereIdAbonnement.HasValue && od.OperationDiverseMereIdAbonnement == operationDiverseMereIdAbonnement)
                .OrderBy(od => od.OperationDiverseId)
                .ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Récupère une OD
        /// </summary>
        /// <param name="odId">L'id de l'OD</param>
        /// <returns>OD</returns>
        public OperationDiverseEnt GetById(int odId)
        {
            return context.OperationDiverses.Where(d => d.OperationDiverseId == odId).Include(x => x.Tache).AsNoTracking().FirstOrDefault();
        }

        public async Task<IReadOnlyList<OperationDiverseEnt>> GetByIdsAsync(IEnumerable<int> odIds)
        {
            return await context.OperationDiverses
                .Where(d => odIds.Contains(d.OperationDiverseId))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des OD qui ont une quantité et un PUHT diiférent de  0  pour Ci
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <returns>Liste de<see cref="OperationDiverseEnt"/></returns>
        public async Task<IReadOnlyList<OperationDiverseEnt>> GetAsync(int ciId)
        {
            return await context.OperationDiverses
                              .Include(t => t.Tache.Parent.Parent)
                              .Include(t => t.Ressource.SousChapitre.Chapitre)
                              .Include(d => d.Unite)
                              .Include(a => a.CI)
                              .Include(d => d.Devise)
                              .Include(d => d.FamilleOperationDiverse)
                              .Where(c => c.CiId == ciId && c.Quantite != 0 && c.PUHT != 0)
                              .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des OD qui ont une quantité et un PUHT différent de 0 pour Ci, en incluant la Nature via l'EcritureComptable
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <returns>Liste de<see cref="OperationDiverseEnt"/></returns>
        public async Task<IReadOnlyList<OperationDiverseEnt>> GetWithNatureAsync(int ciId)
        {
            return await context.OperationDiverses
                              .Include(t => t.Tache.Parent.Parent)
                              .Include(t => t.Ressource.SousChapitre.Chapitre)
                              .Include(d => d.Unite)
                              .Include(a => a.CI)
                              .Include(d => d.Devise)
                              .Include(o => o.EcritureComptable.Nature)
                              .Where(c => c.CiId == ciId && c.Quantite != 0 && c.PUHT != 0)
                              .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne l'abonnement d'opération diverse
        /// </summary>
        /// <param name="operationDiverseIdMere">ID de la première opération diverse d'un abonnement (OD mère)</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt"></see></returns>
        public IReadOnlyCollection<OperationDiverseEnt> GetAbonnementByODMere(int operationDiverseIdMere)
        {
            return context.OperationDiverses
                .Where(d => d.OperationDiverseId == operationDiverseIdMere || (d.OperationDiverseMereIdAbonnement.HasValue && d.OperationDiverseMereIdAbonnement.Value == operationDiverseIdMere))
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Creation des operations diverses
        /// </summary>
        /// <param name="operationsDiverses">Les operation Diverses</param>
        public void SaveRange(IEnumerable<OperationDiverseEnt> operationsDiverses)
        {
            if (!operationsDiverses.Any())
            {
                return;
            }
            using (var dbcontextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    // disable detection of changes
                    context.ChangeTracker.AutoDetectChangesEnabled = false;

                    context.OperationDiverses.AddRange(operationsDiverses);

                    context.SaveChanges();

                    dbcontextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbcontextTransaction.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
                finally
                {
                    // re-enable detection of changes
                    context.ChangeTracker.AutoDetectChangesEnabled = true;
                }
            }
        }

        /// <summary>
        /// Mise à jour des opérations diverses
        /// </summary>
        /// <param name="operationsDiverses">Les OperationDiverse</param>
        private void UpdateRange(IEnumerable<OperationDiverseEnt> operationsDiverses)
        {
            if (!operationsDiverses.Any())
            {
                return;
            }

            using (var dbcontextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    // disable detection of changes
                    context.ChangeTracker.AutoDetectChangesEnabled = false;

                    InsertOrUpdate(x => new { x.OperationDiverseId }, operationsDiverses);

                    context.SaveChanges();

                    dbcontextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbcontextTransaction.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
                finally
                {
                    // re-enable detection of changes
                    context.ChangeTracker.AutoDetectChangesEnabled = true;
                }
            }
        }

        /// <summary>
        /// Met à jour les champs nécessaires d'une OD dans le contexte DB
        /// </summary>
        /// <param name="operationDiverse">Opération Diverse</param>
        private void ProcessMandatoryFieldsOperationDiverse(OperationDiverseEnt operationDiverse)
        {
            if (operationDiverse.Tache != null)
            {
                context.Taches.Attach(operationDiverse.Tache);
            }

            if (operationDiverse.Ressource != null)
            {
                context.Ressources.Attach(operationDiverse.Ressource);
            }

            if (operationDiverse.Unite != null)
            {
                context.Unites.Attach(operationDiverse.Unite);
            }

            if (operationDiverse.Devise != null)
            {
                context.Devise.Attach(operationDiverse.Devise);
            }
            if (operationDiverse.OperationDiverseMereAbonnement != null)
            {
                context.OperationDiverses.Attach(operationDiverse.OperationDiverseMereAbonnement);
            }
        }

        /// <summary>
        /// Met à jour les champs nécessaires d'une liste d'ODs dans le contexte DB.
        /// </summary>
        /// <param name="operationsDiversesList">Liste d'opérations diverses à traiter</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt" /></returns>
        private IEnumerable<OperationDiverseEnt> ProcessMandatoryFieldsOperationsDiverses(IEnumerable<OperationDiverseEnt> operationsDiversesList)
        {
            return operationsDiversesList.Select(x =>
            {
                x.Tache = null;
                x.Ressource = null;
                x.Devise = null;
                x.Unite = null;
                x.OperationDiverseMereAbonnement = null;
                return x;
            });
        }

        /// <summary>
        /// Récupére la liste des OD pour une période comptable donnée
        /// </summary>
        /// <param name="ciIds">Liste d'indentifiant de CI</param>
        /// <param name="periodeDebut">Periode de début</param>
        /// <param name="periodeFin">Période de fin</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt" /></returns>
        public async Task<IEnumerable<OperationDiverseEnt>> GetODsAsync(List<int> ciIds, List<DateTime?> periodeDebut, List<DateTime?> periodeFin)
        {
            MonthLimits monthLimitsStart = periodeDebut.Min(q => q.Value).GetLimitsOfMonth();
            MonthLimits monthLimitsEnd = periodeFin.Max(q => q.Value).GetLimitsOfMonth();
            return await context.OperationDiverses.Where(x => ciIds.Contains(x.CiId) && (x.DateComptable.Value >= monthLimitsStart.StartDate && x.DateComptable.Value <= monthLimitsEnd.EndDate)).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Récupére la liste des OD pour un mois
        /// </summary>
        /// <param name="ciIds">Liste d'indentifiant de CI</param>
        /// <param name="dateComptable">Date comptable</param>
        /// <param name="withIncludes">si true, alors la requête vas retourner les entités liées</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt" /></returns>
        public async Task<IReadOnlyList<OperationDiverseEnt>> GetODsAsync(List<int> ciIds, DateTime dateComptable, bool withIncludes)
        {
            MonthLimits dateLimits = dateComptable.GetLimitsOfMonth();
            Expression<Func<OperationDiverseEnt, bool>> filter = (od) => ciIds.Contains(od.CiId) && od.DateComptable >= dateLimits.StartDate && od.DateComptable <= dateLimits.EndDate;
            if (withIncludes)
            {
                var query = context.OperationDiverses.Where(filter);
                query.Include(op => op.Tache).Load();
                query.Include(op => op.Devise).Load();
                query.Include(op => op.Ressource).Load();
                query.Include(op => op.Unite).Load();
                return await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
            }
            else
            {
                return await context.OperationDiverses.Where(filter).AsNoTracking().ToListAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Insere une liste d'OperationDiverse
        /// </summary>
        /// <param name="operationDiverses">Liste de <see cref="OperationDiverseEnt" /></param>
        public void Insert(List<OperationDiverseEnt> operationDiverses)
        {
            context.OperationDiverses.AddRange(operationDiverses);
        }

        /// <summary>
        /// Retourne la liste des OD en fonction d'une liste d'identifiant d'écriture comptable
        /// </summary>
        /// <param name="ecritureComptableIds">Liste d'identifiant d'écriture comptable</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt" /> </returns>
        public async Task<IReadOnlyList<OperationDiverseEnt>> GetODsAsync(List<int> ecritureComptableIds)
        {
            return await context.OperationDiverses.Where(q => q.EcritureComptableId.HasValue && ecritureComptableIds.Contains(q.EcritureComptableId.Value)).ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des opérations diverses selon les paramètres
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>    
        /// <param name="periodeDebut">Date periode début</param>
        /// <param name="periodeFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <returns>Liste d'opération diverse</returns>
        public async Task<IReadOnlyList<OperationDiverseEnt>> GetOperationDiverseListAsync(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId)
        {
            return await context.OperationDiverses
                      .Include(t => t.Tache.Parent.Parent)
                      .Include(t => t.Ressource.SousChapitre.Chapitre)
                      .Include(d => d.Unite)
                      .Include(a => a.CI)
                      .Include(d => d.Devise)
                      .Where(x => x.CiId == ciId
                                   && (!ressourceId.HasValue || x.RessourceId == ressourceId)
                                   && (!deviseId.HasValue || x.DeviseId == deviseId)
                                   && (!periodeDebut.HasValue || (x.DateComptable.HasValue && (100 * x.DateComptable.Value.Year) + x.DateComptable.Value.Month >= (100 * periodeDebut.Value.Year) + periodeDebut.Value.Month))
                                   && (!periodeFin.HasValue || (x.DateComptable.HasValue && (100 * x.DateComptable.Value.Year) + x.DateComptable.Value.Month <= (100 * periodeFin.Value.Year) + periodeFin.Value.Month)))
                      .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste de tous les OperationDiverseEnt pour un ci et une dateComptable;
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <param name="withIncludes">inclut les objets sous jacents</param>
        /// <returns>liste de OperationDiverseEnt</returns>
        public async Task<IReadOnlyList<OperationDiverseEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptable, bool withIncludes)
        {
            MonthLimits dateLimits = dateComptable.GetLimitsOfMonth();
            Expression<Func<OperationDiverseEnt, bool>> filter = (od) => od.CiId == ciId && od.DateComptable >= dateLimits.StartDate && od.DateComptable <= dateLimits.EndDate;
            if (withIncludes)
            {
                return await context.OperationDiverses
                                      .Include(op => op.Tache)
                                      .Include(op => op.Devise)
                                      .Include(op => op.Ressource)
                                      .Include(op => op.Unite)
                                      .Where(filter)
                                      .AsNoTracking().ToListAsync().ConfigureAwait(false);
            }
            else
            {
                return await context.OperationDiverses.Where(filter).AsNoTracking().ToListAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Retourne la liste de tous les OperationDiverseEnt pour un ci et une periode;
        /// </summary>
        /// <param name="ciId">CiId</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin </param>
        /// <returns>liste de OperationDiverseEnt</returns>
        public async Task<IReadOnlyList<OperationDiverseEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            MonthLimits dateLimitsStart = dateComptableDebut.GetLimitsOfMonth();
            MonthLimits dateLimitsEnd = dateComptableFin.GetLimitsOfMonth();
            Expression<Func<OperationDiverseEnt, bool>> filter = (od) => od.CiId == ciId && od.DateComptable >= dateLimitsStart.StartDate && od.DateComptable <= dateLimitsEnd.EndDate;

            return await context.OperationDiverses.Where(filter).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des opérations diverses en fonction d'une liste d'identifiant de commande
        /// </summary>
        /// <param name="commandeIds">Liste d'identifiant de commande</param>
        /// <returns>Liste d'operation diverse</returns>
        public async Task<IReadOnlyList<OperationDiverseEnt>> GetByCommandeIdsAsync(List<int> commandeIds)
        {
            return await context.OperationDiverses.Where(q => q.CommandeId.HasValue && commandeIds.Contains(q.CommandeId.Value)).ToListAsync().ConfigureAwait(false);
        }
    }
}
