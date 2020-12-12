using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Achat.Calculation.Common;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Utilisateur;
using Fred.EntityFramework;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.Linq;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Depense
{
    /// <summary>
    /// Référentiel de données pour les dépenses.
    /// </summary>
    public class DepenseRepository : FredRepository<DepenseAchatEnt>, IDepenseRepository
    {
        public DepenseRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Permet de détacher les entités dépendantes des dépenses pour éviter de les prendre en compte dans la sauvegarde du
        /// contexte.
        /// </summary>
        /// <param name="depense">dépense dont les dépendances sont à détachées</param>
        private void CleanDependencies(DepenseAchatEnt depense)
        {
            depense.CommandeLigne = null;
            depense.Fournisseur = null;
            depense.Ressource = null;
            depense.Tache = null;
            depense.AuteurCreation = null;
            depense.AuteurModification = null;
            depense.AuteurSuppression = null;
            depense.CI = null;
            depense.Devise = null;
            depense.AuteurVisaReception = null;
            depense.Unite = null;
            depense.FacturationsFacture = null;
            depense.FacturationsReception = null;
            depense.FacturationsFactureEcart = null;
            depense.FacturationsFar = null;
            depense.Depenses = null;
            depense.DepenseType = null;
            depense.PiecesJointesReception = null;
        }

        /// <summary>
        /// Requête de filtrage des depenses par défaut
        /// </summary>
        /// <returns>Retourne le prédicat de filtrage des depenses par défaut</returns>
        private IQueryable<DepenseAchatEnt> GetDefaultListQuery()
        {
            return Context.DepenseAchats.Include(d => d.CI)
                          .Include(d => d.CommandeLigne.Commande)
                          .Include(d => d.Fournisseur)
                          .Include(d => d.Ressource)
                          .Include(d => d.Tache)
                          .Include(d => d.Unite)
                          .Include(d => d.AuteurCreation.Personnel)
                          .Include(d => d.AuteurModification.Personnel)
                          .Include(d => d.Devise)
                          .Include(d => d.DepenseType)
                          .AsNoTracking()
                          .Where(d => !d.DateSuppression.HasValue);
        }

        /// <summary>
        /// Ajout une nouvelle dépense.
        /// </summary>
        /// <param name="depense">dépense à ajouter</param>
        /// <returns> L'identifiant de la dépense ajoutée</returns>
        public DepenseAchatEnt AddDepense(DepenseAchatEnt depense)
        {
            if (Context.Entry(depense).State == EntityState.Detached)
            {
                CleanDependencies(depense);
            }

            Insert(depense);

            return depense;
        }

        public DepenseAchatEnt Add(DepenseAchatEnt depense)
        {
            if (Context.Entry(depense).State == EntityState.Detached)
            {
                CleanDependencies(depense);
            }

            Insert(depense);

            return depense;
        }

        /// <summary>
        /// Sauvegarde les modifications d'une dépense.
        /// </summary>
        /// <param name="depense">dépense à modifier</param>
        /// <returns>Dépense modifiée</returns>
        public DepenseAchatEnt UpdateDepense(DepenseAchatEnt depense)
        {
            if (Context.Entry(depense).State == EntityState.Detached)
            {
                CleanDependencies(depense);
            }

            Update(depense);

            return depense;
        }

        /// <summary>
        /// Mise a jour d'une depense
        /// </summary>
        /// <param name="depense">La depense</param>       
        public void UpdateDepenseWithoutSave(DepenseAchatEnt depense)
        {
            if (Context.Entry(depense).State == EntityState.Detached)
            {
                CleanDependencies(depense);
            }
            Update(depense);
        }

        /// <summary>
        /// Retourne la dépense portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="depenseId">Identifiant de la dépense à retrouver.</param>
        /// <returns>La dépense retrouvée, sinon nulle.</returns>
        public DepenseAchatEnt GetDepenseById(int depenseId)
        {
            return Context.DepenseAchats.Include(t => t.Tache)
                          .Include(d => d.Ressource)
                          .Include(d => d.Unite)
                          .Include(a => a.CI)
                          .Include(f => f.Fournisseur)
                          .Include(d => d.CommandeLigne.Commande)
                          .Include(d => d.Devise)
                          .Include(d => d.DepenseType)
                          .AsNoTracking()
                          .SingleOrDefault(r => r.DepenseId.Equals(depenseId));
        }

        /// <summary>
        /// Retourne la liste des dépenses.
        /// </summary>
        /// <returns>La liste des dépenses.</returns>
        public IEnumerable<DepenseAchatEnt> GetDepenseList()
        {
            foreach (DepenseAchatEnt depense in Context.DepenseAchats
                                                  .Include(t => t.Tache)
                                                  .Include(d => d.Ressource)
                                                  .Include(d => d.Unite)
                                                  .Include(a => a.CI)
                                                  .Include(f => f.Fournisseur)
                                                  .Include(d => d.CommandeLigne.Commande)
                                                  .Include(d => d.Devise)
                                                  .Include(d => d.DepenseType)
                                                  .AsNoTracking()
                                                  .Where(r => !r.DateSuppression.HasValue))
            {
                yield return depense;
            }
        }

        /// <summary>
        /// Retourne la liste des dépenses en incluant les tahces et les ressources liées
        /// ainsi que toutes les facturations
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>La liste des dépenses.</returns>
        public async Task<IEnumerable<DepenseAchatEnt>> GetDepensesListWithMinimumIncludesAsync(int ciId)
        {
            return await Context.DepenseAchats.Where(d => d.CiId == ciId && !d.DateSuppression.HasValue && d.PUHT != 0)
                  .Include(x => x.CI)
                  .Include(d => d.Tache.Parent.Parent)
                  .Include(d => d.Ressource.SousChapitre.Chapitre)
                  .Include(x => x.FacturationsFacture)
                  .Include(x => x.FacturationsFactureEcart)
                  .Include(x => x.FacturationsReception)
                  .Include(x => x.FacturationsFar)
                  .Include(x => x.DepenseType)
                  .Include(x => x.Unite)
                  .ToListAsync()
                  .ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne une liste de dépenses en fonction d'un identifiant de CI et d'une date comptable
        /// et qui n'ont pas été supprimées
        /// </summary>
        /// <param name="ciId">Identifiant du CI associé</param>
        /// <param name="dateComptable">Date comptable de la dépense</param>
        /// <returns>Une liste de dépenses</returns>
        public IEnumerable<DepenseAchatEnt> GetDepenseList(int ciId, DateTime dateComptable)
        {
            DateTime startDate = dateComptable.GetLimitsOfMonth().StartDate;
            DateTime endDate = dateComptable.GetLimitsOfMonth().EndDate;
            foreach (DepenseAchatEnt depense in Context.DepenseAchats
                                                  .Include(t => t.Tache)
                                                  .Include(d => d.Ressource)
                                                  .Include(d => d.Unite)
                                                  .Include(a => a.CI)
                                                  .Include(f => f.Fournisseur)
                                                  .Include(d => d.CommandeLigne.Commande)
                                                  .Include(d => d.Devise)
                                                  .Include(d => d.DepenseType)
                                                  .AsNoTracking()
                                                  .Where(r => !r.DateSuppression.HasValue && r.Date >= startDate && r.Date <= endDate && r.CiId == ciId))
            {
                yield return depense;
            }
        }

        /// <summary>
        /// Retourne la liste complète des dépenses en fonction d'un identifiant de CI et d'une date comptable
        /// </summary>
        /// <param name="ciId">Identifiant du CI associé</param>
        /// <param name="dateComptable">Date comptable de la dépense</param>
        /// <returns>Total des dépenses</returns>
        public double GetMontantTotal(int ciId, DateTime dateComptable)
        {
            return (double)Context.DepenseAchats
                                  .Where(r => r.Date == dateComptable && r.CiId == ciId)
                                  .AsNoTracking()
                                  .Sum(d => d.MontantHT);
        }

        /// <summary>
        /// Retourne la liste des depenses filtrées selon les critères de recherche.
        /// </summary>
        /// <param name="searchParameters">Paramètres de recherche</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>La liste des depenses filtrées selon les critères de recherche et ordonnées selon les critères de tri</returns>
        public IEnumerable<DepenseAchatEnt> SearchDepenseListWithFilter(SearchDepenseEnt searchParameters, int page, int pageSize = 20)
        {
            return Query()
              .Include(d => d.CI)
              .Include(d => d.CommandeLigne.Commande)
              .Include(d => d.Fournisseur)
              .Include(d => d.Ressource)
              .Include(d => d.Tache)
              .Include(d => d.Unite)
              .Include(d => d.AuteurCreation.Personnel)
              .Include(d => d.AuteurModification.Personnel)
              .Include(d => d.Devise)
              .Include(d => d.DepenseType)
              .Filter(d => !d.DateSuppression.HasValue)
              .Filter(searchParameters.GetPredicateWhere())
              .OrderBy(searchParameters.ApplyOrderBy)
              .GetPage(page, pageSize)
              .AsNoTracking();
        }

        /// <summary>
        /// Retourne le montant total de la liste des depenses filtrée selon les critères de recherche.
        /// </summary>
        /// <param name="predicateWhere">Prédicat de filtrage des depenses</param>
        /// <returns>
        /// le montant total de La liste des depenses filtrées selon les critères de recherche et ordonnées selon les
        /// critères de tri
        /// </returns>
        public double GetMontantTotal(Expression<Func<DepenseAchatEnt, bool>> predicateWhere)
        {
            return (double)GetDefaultListQuery()
              .Where(predicateWhere)
              .AsNoTracking()
              .Sum(d => d.MontantHT);
        }

        /// <summary>
        /// Permet de récupérer une liste de dépenses
        /// </summary>
        /// <param name="ids">La liste des identifants à récupérer</param>
        /// <returns>Une liste de dépenses.</returns>
        public IEnumerable<DepenseAchatEnt> GetDepenses(List<int> ids)
        {
            try
            {
                return GetDefaultListQuery()
                  .Include(d => d.CommandeLigne.Commande.CI)
                  .Include(d => d.CommandeLigne.Commande.CI.Organisation)
                  .Include(d => d.CommandeLigne.Commande)
                  .Include(d => d.CI.Organisation)
                  .Where(x => ids.Contains(x.DepenseId));
            }
            catch (Exception exception)
            {
                throw new FredRepositoryException(exception.Message, exception);
            }
        }

        /// <summary>
        /// Permet de récupérer une liste de dépenses
        /// </summary>
        /// <param name="groupRemplacementId">L'identifants du groupe de remplacement</param>
        /// <returns>Une liste de dépenses.</returns>
        public IEnumerable<DepenseAchatEnt> GetByGroupRemplacementId(int groupRemplacementId)
        {
            return Context.DepenseAchats.Where(d => d.GroupeRemplacementTacheId == groupRemplacementId).Include(x => x.Tache).Include(d => d.DepenseType).AsNoTracking();
        }

        /// <summary>
        /// Retourner la requête de récupération des dépenses
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête (bah non en fait, ça return une liste)</returns>
        public List<DepenseAchatEnt> Get(List<Expression<Func<DepenseAchatEnt, bool>>> filters,
                                                Func<IQueryable<DepenseAchatEnt>, IOrderedQueryable<DepenseAchatEnt>> orderBy = null,
                                                List<Expression<Func<DepenseAchatEnt, object>>> includeProperties = null,
                                                int? page = null,
                                                int? pageSize = null,
                                                bool asNoTracking = true)
        {
            if (asNoTracking)
            {
                return base.Get(filters, orderBy, includeProperties, page, pageSize).AsNoTracking().ToList();
            }

            return base.Get(filters, orderBy, includeProperties, page, pageSize).ToList();
        }

        /// <summary>
        /// Récupération d'une réception en fonction de son identifiant
        /// </summary>
        /// <param name="receptionId">Identifiant de la réception</param>
        /// <returns>Réception trouvée</returns>
        public DepenseAchatEnt GetReception(int receptionId)
        {
            int depenseType = DepenseType.Reception.ToIntValue();
            return Query().Include(t => t.Tache)
                        .Include(d => d.Ressource)
                        .Include(d => d.Unite)
                        .Include(a => a.CI)
                        .Include(f => f.Fournisseur)
                        .Include(d => d.CommandeLigne.Commande)
                        .Include(d => d.Devise)
                        .Include(d => d.DepenseType)
                        .Filter(x => x.DepenseType.Code == depenseType)
                        .Get()
                        .AsNoTracking()
                        .FirstOrDefault(r => r.DepenseId.Equals(receptionId));
        }

        /// <summary>
        /// Requête par défaut de récupération d'une réception
        /// </summary>
        /// <returns>Requête EF par défaut</returns>
        public IRepositoryQuery<DepenseAchatEnt> GetDefaultQuery()
        {
            return Query()
                    .Include(r => r.CommandeLigne.Commande.Fournisseur)
                    .Include(r => r.CommandeLigne.Commande.CI)
                    .Include(t => t.Tache)
                    .Include(r => r.Ressource.ReferentielEtendus.Select(x => x.Nature))
                    .Include(u => u.Unite)
                    .Include(ac => ac.AuteurCreation.Personnel)
                    .Include(am => am.AuteurModification.Personnel)
                    .Include(av => av.AuteurVisaReception.Personnel)
                    .Include(d => d.Devise)
                    .Include(f => f.FacturationsReception)
                    .Include(f => f.DepenseType)
                    .Include(depEnfants => depEnfants.Depenses.Select(x => x.DepenseType));
        }

        /// <summary>
        /// Retourne la liste des dépenses selon les paramètres
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>    
        /// <param name="periodeDebut">Date periode début</param>
        /// <param name="periodeFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <returns>Liste de dépense achat</returns>
        public IEnumerable<DepenseAchatEnt> GetReceptions(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId)
        {
            return Context.DepenseAchats
                .Where(x => x.CiId == ciId
                                            && (!periodeDebut.HasValue || (x.DateComptable.HasValue && (100 * x.DateComptable.Value.Year) + x.DateComptable.Value.Month >= (100 * periodeDebut.Value.Year) + periodeDebut.Value.Month))
                                            && (!periodeFin.HasValue || (x.DateComptable.HasValue && (100 * x.DateComptable.Value.Year) + x.DateComptable.Value.Month <= (100 * periodeFin.Value.Year) + periodeFin.Value.Month))
                                            && (x.DateSuppression == null))
                .AsNoTracking();
        }

        /// <summary>
        /// Retourne la liste des dépenses (réception)
        /// </summary>
        /// <param name="ciIds">Liste d'indentifiant de CI</param>
        /// <param name="periodeDebut">Liste de date de début</param>
        /// <param name="periodeFin">Liste de date de fin</param>
        /// <returns>Liste de dépense achat</returns>
        public IEnumerable<DepenseAchatEnt> GetReceptions(List<int> ciIds, List<DateTime?> periodeDebut, List<DateTime?> periodeFin)
        {
            MonthLimits dateDebut = periodeDebut.Min(q => q.Value).GetLimitsOfMonth();
            MonthLimits dateFin = periodeFin.Max(q => q.Value).GetLimitsOfMonth();
            return Context.DepenseAchats.Where(x => ciIds.Contains(x.CiId.Value)
                                            && (x.DateSuppression == null)
                                            && (x.DateComptable.Value >= dateDebut.StartDate)
                                            && (x.DateComptable.Value <= dateFin.EndDate));
        }
        public async Task<IEnumerable<DepenseAchatEnt>> GetReceptionsAsync(List<int> ciIds, List<DateTime?> periodeDebut, List<DateTime?> periodeFin)
        {
            MonthLimits dateDebut = periodeDebut.Min(q => q.Value).GetLimitsOfMonth();
            MonthLimits dateFin = periodeFin.Max(q => q.Value).GetLimitsOfMonth();
            return await Context.DepenseAchats.Where(x => ciIds.Contains(x.CiId.Value)
                                            && (x.DateSuppression == null)
                                            && (x.DateComptable.Value >= dateDebut.StartDate)
                                            && (x.DateComptable.Value <= dateFin.EndDate)).ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des réceptions selon les paramètres
        /// </summary>
        /// <param name="ciIdList">liste d'identifiants de CIs</param>
        /// <param name="tacheIdList">liste d'identifiants de taches</param>    
        /// <param name="dateDebut">Date periode début</param>
        /// <param name="dateFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <param name="includeProperties">include des navigation properties</param>
        /// <returns>Liste de dépense de type réception</returns>
        public async Task<IEnumerable<DepenseAchatEnt>> GetReceptionsAsync(List<int> ciIdList, List<int> tacheIdList, DateTime? dateDebut, DateTime? dateFin, int? deviseId, bool includeProperties = false)
        {
            IRepositoryQuery<DepenseAchatEnt> query = Query();
            if (includeProperties)
            {
                query.Include(x => x.Ressource)
                    .Include(x => x.Unite);
            }

            int typeReceptionCode = DepenseType.Reception.ToIntValue();

            return await query.Filter(x => ciIdList.Contains(x.CiId.Value)
                                           && (tacheIdList.Contains(x.TacheId.Value))
                                           && (!deviseId.HasValue || x.CommandeLigne.Commande.DeviseId == deviseId)
                                           && (!dateDebut.HasValue || x.DateComptable.Value.Date >= dateDebut.Value.Date)
                                           && (!dateFin.HasValue || x.DateComptable.Value.Date <= dateFin.Value.Date)
                                           && x.DepenseType.Code == typeReceptionCode
                                           && (x.DateSuppression == null))
               .Get()
               .AsNoTracking()
               .ToListAsync()
               .ConfigureAwait(false);
        }

        /// <summary>
        /// Récupère la dernière réception de chaque ligne de commande en fonction de sa Date
        /// </summary>
        /// <param name="commandeLigneIds">Liste d'identifiant d'une ligne de commande</param>
        /// <returns>Dictionnaire (CommandeLigneId, DepenseAchatEnt)</returns>
        public Dictionary<int, DepenseAchatEnt> GetLastReceptionByCommandeLigneId(List<int> commandeLigneIds)
        {
            Dictionary<int, DepenseAchatEnt> dico = new Dictionary<int, DepenseAchatEnt>();
            int typeReceptionCode = DepenseType.Reception.ToIntValue();
            DepenseAchatEnt dep;
            List<DepenseAchatEnt> receptions = Context.DepenseAchats.Where(x => x.CommandeLigneId.HasValue && commandeLigneIds.Contains(x.CommandeLigneId.Value) && !x.DateSuppression.HasValue && x.DepenseType.Code == typeReceptionCode).ToList();

            foreach (int cmdLigneId in commandeLigneIds)
            {
                dep = receptions.Where(x => x.CommandeLigneId.Value == cmdLigneId)
                                .OrderByDescending(x => x.Date)
                                .ThenByDescending(x => x.DateCreation)
                                .FirstOrDefault();

                dico.Add(cmdLigneId, dep);
            }

            return dico;
        }

        /// <summary>
        /// Récupère les identifiant unique des réceptions intérimaire qui n'ont pas été envoyé à SAP
        /// </summary>
        /// <param name="listeCiId">liste d'identifiant unique de ci</param>
        /// <returns>Réception</returns>
        public IEnumerable<int> GetReceptionInterimaireToSend(List<int> listeCiId)
        {
            return Context.DepenseAchats.Where(r => r.HangfireJobId == null && r.IsReceptionInterimaire && listeCiId.Contains(r.CiId.Value)).Select(r => r.DepenseId);
        }

        /// <summary>
        /// Récupère les identifiant unique des réceptions matériel externe qui n'ont pas été envoyé à SAP
        /// </summary>
        /// <returns>Réception</returns>
        public IEnumerable<int> GetReceptionMaterielExterneToSend()
        {
            return Context.DepenseAchats.Where(r => r.HangfireJobId == null && r.IsReceptionMaterielExterne).Select(r => r.DepenseId);
        }


        public List<int> GetReceptionsIdsWithFilter(SearchDepenseEnt filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return Context.DepenseAchats
                            .AsExpandable()
                            .Where(filter.GetReceptionPredicateWhere())
                            .OrderByDescending(x => x.Date)
                            .Select(x => x.DepenseId)
                            .ToList();
        }

        /// <summary>
        /// Retourne les receptions avec tous les includes en fonction d'une liste d'id 
        /// Mais c'est quoi cette méthode de merde ! 
        /// </summary>
        /// <param name="receptionIds">Liste d'id</param>
        /// <returns>Liste des receptions</returns>
        public List<DepenseAchatEnt> GetReceptionsWithAllIncludes(List<int> receptionIds)
        {
            List<DepenseAchatEnt> result = new List<DepenseAchatEnt>();

            foreach (IEnumerable<int> batch in receptionIds.Batch(10000))
            {
                List<int> receptionIdsInBatch = batch.ToList();

                List<DepenseAchatEnt> partialResult = this.Context.DepenseAchats
                           .Include(f => f.FacturationsReception)
                           .Include(depEnfants => depEnfants.Depenses).ThenInclude(x => x.DepenseType)
                           .Include(f => f.PiecesJointesReception)
                           .Where(x => receptionIdsInBatch.Contains(x.DepenseId))
                           .OrderByDescending(x => x.Date)
                           .AsNoTracking()
                           .ToList();

                List<CommandeLigneEnt> commandesLignes = GetCommandesLignes(partialResult);

                List<TacheEnt> taches = GetTaches(partialResult);

                List<RessourceEnt> ressources = GetRessources(partialResult);

                List<CIEnt> cis = GetCis(partialResult);

                List<UniteEnt> unites = GetUnites(partialResult);

                List<UtilisateurEnt> auteurs = GetUtilisateurs(partialResult);

                List<DeviseEnt> devises = GetDevises(partialResult);

                List<DepenseTypeEnt> depenseTypes = GetDepenseTypes(partialResult);

                foreach (DepenseAchatEnt reception in partialResult)
                {
                    //https://stackoverflow.com/questions/14032709/performance-of-find-vs-firstordefault
                    reception.CommandeLigne = commandesLignes.Find(x => x.CommandeLigneId == reception.CommandeLigneId);
                    reception.CI = cis.Find(x => x.CiId == reception.CiId);
                    reception.Tache = taches.Find(x => x.TacheId == reception.TacheId);
                    reception.Ressource = ressources.Find(x => x.RessourceId == reception.RessourceId);
                    reception.Unite = unites.Find(x => x.UniteId == reception.UniteId);
                    reception.DepenseType = depenseTypes.First(x => x.DepenseTypeId == reception.DepenseTypeId);
                    reception.AuteurCreation = auteurs.Find(x => x.UtilisateurId == reception.AuteurCreationId);
                    reception.AuteurModification = auteurs.Find(x => x.UtilisateurId == reception.AuteurModificationId);
                    reception.AuteurVisaReception = auteurs.Find(x => x.UtilisateurId == reception.AuteurVisaReceptionId);
                    reception.Devise = devises.Find(x => x.DeviseId == reception.DeviseId);
                }
                result.AddRange(partialResult);
            }
            return result;
        }

        private List<CommandeLigneEnt> GetCommandesLignes(List<DepenseAchatEnt> partialResult)
        {
            List<int?> commandesLignesIds = partialResult.Select(x => x.CommandeLigneId).Distinct().ToList();
            return Context.CommandeLigne
                  .Include(x => x.Commande.Fournisseur)
                  .Include(x => x.Commande.CI)
                  .Include(x => x.Commande.PiecesJointesCommande)
                  .Where(x => commandesLignesIds.Contains(x.CommandeLigneId))
                  .AsNoTracking()
                  .ToList();
        }

        private List<CIEnt> GetCis(List<DepenseAchatEnt> partialResult)
        {
            List<int?> ciIds = partialResult.Select(x => x.CiId).Distinct().ToList();
            return Context.CIs.Include(x => x.EtablissementComptable).Where(x => ciIds.Contains(x.CiId)).AsNoTracking().ToList();
        }

        private List<TacheEnt> GetTaches(List<DepenseAchatEnt> partialResult)
        {
            List<int?> tachesIds = partialResult.Select(x => x.TacheId).Distinct().ToList();
            return Context.Taches.Where(x => tachesIds.Contains(x.TacheId)).AsNoTracking().ToList();
        }

        private List<RessourceEnt> GetRessources(List<DepenseAchatEnt> partialResult)
        {
            List<int?> ressourcesIds = partialResult.Select(x => x.RessourceId).Distinct().ToList();
            return Context.Ressources.Include(x => x.ReferentielEtendus).ThenInclude(y => y.Nature).Where(x => ressourcesIds.Contains(x.RessourceId)).AsNoTracking().ToList();
        }

        private List<UniteEnt> GetUnites(List<DepenseAchatEnt> partialResult)
        {
            List<int?> uniteIds = partialResult.Select(x => x.UniteId).Distinct().ToList();
            return Context.Unites.Where(x => uniteIds.Contains(x.UniteId)).AsNoTracking().ToList();
        }

        private List<UtilisateurEnt> GetUtilisateurs(List<DepenseAchatEnt> partialResult)
        {
            List<int?> allPersonnels = partialResult.Select(x => x.AuteurCreationId).ToList();
            List<int?> auteurModifications = partialResult.Select(x => x.AuteurModificationId).ToList();
            List<int?> auteurVisaReceptions = partialResult.Select(x => x.AuteurVisaReceptionId).ToList();
            allPersonnels.AddRange(auteurModifications);
            allPersonnels.AddRange(auteurVisaReceptions);
            allPersonnels = allPersonnels.Distinct().ToList();

            return Context.Utilisateurs.Include(x => x.Personnel).Where(x => allPersonnels.Contains(x.UtilisateurId)).AsNoTracking().ToList();
        }

        private List<DeviseEnt> GetDevises(List<DepenseAchatEnt> partialResult)
        {
            List<int?> deviseIds = partialResult.Select(x => x.DeviseId).Distinct().ToList();
            return Context.Devise.Where(x => deviseIds.Contains(x.DeviseId)).AsNoTracking().ToList();
        }

        private List<DepenseTypeEnt> GetDepenseTypes(List<DepenseAchatEnt> result)
        {
            List<int?> depenseTypesIds = result.Select(x => x.DepenseTypeId).Distinct().ToList();
            return Context.DepenseTypes.Where(x => depenseTypesIds.Contains(x.DepenseTypeId)).AsNoTracking().ToList();
        }

        /// <summary>
        /// Specifie les champs qui doivent etre mis a jour lors d'un update
        /// </summary>
        /// <param name="receptions">Liste des réceptions dont il faut mettre le champ a jour</param>
        /// <param name="updatedProperties">champs qui doivent etre mis a jour</param>      
        public void MarkFieldsAsUpdated(List<DepenseAchatEnt> receptions, params Expression<Func<DepenseAchatEnt, object>>[] updatedProperties)
        {
            foreach (DepenseAchatEnt reception in receptions)
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<DepenseAchatEnt> entry = Context.Entry(reception);
                if (entry.State == EntityState.Detached)
                {
                    this.Context.DepenseAchats.Attach(reception);
                }
                foreach (Expression<Func<DepenseAchatEnt, object>> updatedProperty in updatedProperties)
                {
                    this.Context.Entry(reception).Property(updatedProperty).IsModified = true;
                }
            }
        }

        /// <summary>
        /// Specifie les champs qui doivent etre mis a jour lors de la mise a jour et sauvegarde dans une transaction
        /// </summary>
        /// <param name="receptions">Liste des réceptions dont il faut mettre le champ a jour</param>
        /// <param name="updatedProperties">champs qui doivent etre mis a jour</param>      
        public void MarkFieldsAsUpdatedAndSaveInOneTransaction(List<DepenseAchatEnt> receptions, params Expression<Func<DepenseAchatEnt, object>>[] updatedProperties)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.ChangeTracker.AutoDetectChangesEnabled = false;

                    int count = 0;

                    const int commitCount = 100;

                    foreach (DepenseAchatEnt reception in receptions)
                    {
                        ++count;

                        CleanDependencies(reception);
                        bool tracking = Context.ChangeTracker.Entries<DepenseAchatEnt>().Any(x => x.Entity.DepenseId == reception.DepenseId);

                        if (!tracking)
                        {
                            Context.DepenseAchats.Attach(reception);
                        }

                        foreach (Expression<Func<DepenseAchatEnt, object>> updatedProperty in updatedProperties)
                        {
                            this.Context.Entry(reception).Property(updatedProperty).IsModified = true;
                        }


                        // A chaque 100 opérations, on sauvegarde le contexte.
                        if (count % commitCount == 0)
                        {
                            Context.SaveChanges();
                        }
                    }

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
                    // re-enable detection of changes
                    Context.ChangeTracker.AutoDetectChangesEnabled = true;
                }
            }
        }

        /// <summary>
        /// Retourne la liste des dépenses 
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Liste de dépense</returns>
        public async Task<IEnumerable<DepenseAchatEnt>> GetDepenseListAsync(int ciId)
        {
            return await Context.DepenseAchats
                         .Include(t => t.Tache.Parent.Parent)
                         .Include(d => d.Ressource.SousChapitre.Chapitre)
                         .Include(d => d.Ressource.ReferentielEtendus).ThenInclude(x => x.Nature)
                         .Include(d => d.Unite)
                         .Include(a => a.CI)
                         .Include(f => f.Fournisseur)
                         .Include(cl => cl.CommandeLigne.Commande)
                         .Include(d => d.Devise)
                         .Include(x => x.FacturationsFacture)
                         .Include(x => x.FacturationsFactureEcart)
                         .Include(x => x.FacturationsReception)
                         .Include(x => x.FacturationsFar)
                         .Include(x => x.DepenseType)
                         .Include(x => x.Depenses).ThenInclude(y => y.DepenseType) // Dépenses achat enfants                                    
                         .Where(c => c.CiId == ciId && !c.DateSuppression.HasValue && c.PUHT != 0)
                         .Where(q => q.Tache.CiId == ciId)
                         .AsNoTracking()
                         .ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des receptions en fonction des identifiants de ligne de commande
        /// </summary>
        /// <param name="commandeLigneIds">Liste d'identifiant des lignes de commmande</param>
        /// <returns>Liste des receptions</returns>
        public IReadOnlyList<DepenseAchatEnt> GetReceptions(List<int> commandeLigneIds)
        {
            return Context.DepenseAchats.Where(q => q.CommandeLigneId.HasValue && commandeLigneIds.Contains(q.CommandeLigneId.Value)).ToList();
        }


        public HasAnyReceptionAlreadyViseeResultModel HasAnyReceptionAlreadyVisee(List<int> receptionsIds)
        {
            var achatCommonExpressionsFiltersHelper = new AchatCommonExpressionsFiltersHelper();

            var anyVisee = Context.DepenseAchats.Where(x => receptionsIds.Contains(x.DepenseId))
                                          .Where(achatCommonExpressionsFiltersHelper.GetIsReceptionFilter())
                                          .Any(x => x.DateVisaReception.HasValue);
            return new HasAnyReceptionAlreadyViseeResultModel()
            {
                HasAnyReceptionsAlreadyVisee = anyVisee
            };
        }
    }
}
