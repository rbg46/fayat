using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.CI;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.CI
{
    /// <summary>
    /// Référentiel de données pour les CI (Centre d'Imputation)
    /// </summary>
    public class CIRepository : FredRepository<CIEnt>, ICIRepository
    {
        private readonly IOrganisationRepository userRepo;

        public CIRepository(FredDbContext context, IOrganisationRepository userRepo)
          : base(context)
        {
            this.userRepo = userRepo;
        }

        /// <summary>
        /// Recherche un ci selon les filtres definis
        /// </summary>
        /// <param name="filters">les filtres</param>
        /// <param name="orderBy">les orderby</param>
        /// <param name="includeProperties">les includes</param>
        /// <param name="page">la page corrante</param>
        /// <param name="pageSize">la taille de la page</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Liste de ci</returns>
        public List<CIEnt> Search(List<Expression<Func<CIEnt, bool>>> filters,
                                              Func<IQueryable<CIEnt>, IOrderedQueryable<CIEnt>> orderBy = null,
                                              List<Expression<Func<CIEnt, object>>> includeProperties = null,
                                              int? page = null,
                                              int? pageSize = null,
                                              bool asNoTracking = true)
        {
            if (asNoTracking)
            {
                return Get(filters, orderBy, includeProperties, page, pageSize).AsNoTracking().ToList();
            }
            else
            {
                return Get(filters, orderBy, includeProperties, page, pageSize).ToList();
            }

        }

        /// <summary>
        /// Permet d'attacher les entités dépendantes des cis si besoin.
        /// </summary>
        /// <param name="ci">CI dont les dépendances sont à attacher</param>
        private void AttachDependancies(CIEnt ci)
        {
            if (ci.Societe != null)
            {
                ci.Societe = Context.Societes.Find(ci.SocieteId);
            }

            if (ci.EtablissementComptable != null)
            {
                ci.EtablissementComptable.Societe = Context.Societes.Find(ci.EtablissementComptable.SocieteId);
                if (ci.EtablissementComptable.Pays != null)
                {
                    ci.EtablissementComptable.Pays = Context.Pays.Find(ci.EtablissementComptable.PaysId);
                }
            }

            //Fix bug 7964 : Responsable chantier est le meme que le responsable administratif. 
            //Probleme entity framework qui ne peut pas attacher les deux memes objets (...)
            if (ci.ResponsableAdministratif != null && ci.PersonnelResponsableChantier != null
                && ci.ResponsableAdministratif.PersonnelId == ci.PersonnelResponsableChantier.PersonnelId)
            {
                ci.ResponsableAdministratif = null;
            }

            AttachResponsableAdministratif(ci.ResponsableAdministratif);
            AttachPersonnelResponsableChantier(ci.PersonnelResponsableChantier);

            if (ci.Taches?.Count > 0)
            {
                List<TacheEnt> taches = new List<TacheEnt>();
                foreach (var tache in ci.Taches)
                {
                    taches.Add(Context.Taches.Find(tache.TacheId));
                }
                ci.Taches = taches;
            }

            if (ci.MontantDevise != null)
            {
                ci.MontantDevise = Context.Devise.Find(ci.MontantDeviseId);
            }

            ci.Pays = Context.Pays.Find(ci.PaysId);
            ci.PaysLivraison = Context.Pays.Find(ci.PaysLivraisonId);
            ci.PaysFacturation = Context.Pays.Find(ci.PaysFacturationId);

            if (ci.CompteInterneSepId.HasValue)
            {
                ci.CompteInterneSep = Context.CIs.Find(ci.CompteInterneSepId.Value);
            }
        }

        private void AttachResponsableAdministratif(PersonnelEnt ra)
        {
            if (ra == null)
            {
                return;
            }

            ra.Societe = Context.Societes.Find(ra.SocieteId);
            ra.Pays = Context.Pays.Find(ra.PaysId);

            if (ra.EtablissementPaie != null)
            {
                ra.EtablissementPaie.Societe = ra.EtablissementPaie.Societe != null ? Context.Societes.Find(ra.EtablissementPaie.SocieteId) : null;
                ra.EtablissementPaie.Pays = ra.EtablissementPaie.Pays != null ? Context.Pays.Find(ra.EtablissementPaie.PaysId) : null;
                ra.EtablissementPaie = Context.EtablissementsPaie.Find(ra.EtablissementPaie.EtablissementPaieId);
            }

            if (ra.EtablissementRattachement != null)
            {
                ra.EtablissementRattachement.Societe = ra.EtablissementRattachement.Societe != null ? Context.Societes.Find(ra.EtablissementRattachement.SocieteId) : null;
                ra.EtablissementRattachement.Pays = ra.EtablissementRattachement.Pays != null ? Context.Pays.Find(ra.EtablissementRattachement.PaysId) : null;
                ra.EtablissementRattachement = Context.EtablissementsPaie.Find(ra.EtablissementRattachement.EtablissementPaieId);
            }

            if (ra.Manager != null)
            {
                AttachManagerResponsableAdministratif(ra);
            }
        }

        private void AttachManagerResponsableAdministratif(PersonnelEnt ra)
        {
            ra.Manager.Societe = ra.Manager.Societe != null ? Context.Societes.Find(ra.Manager.SocieteId) : null;
            ra.Manager.Pays = ra.Manager.Pays != null ? Context.Pays.Find(ra.Manager.PaysId) : null;
            if (ra.Manager.EtablissementRattachement != null)
            {
                ra.Manager.EtablissementRattachement.Societe = ra.Manager.EtablissementRattachement.Societe != null ? Context.Societes.Find(ra.Manager.EtablissementRattachement.SocieteId) : null;
                ra.Manager.EtablissementRattachement.Pays = ra.Manager.EtablissementRattachement.Pays != null ? Context.Pays.Find(ra.Manager.EtablissementRattachement.PaysId) : null;
                ra.Manager.EtablissementRattachement = Context.EtablissementsPaie.Find(ra.Manager.EtablissementRattachement.EtablissementPaieId);
            }
            if (ra.Manager.EtablissementPaie != null)
            {
                ra.Manager.EtablissementPaie.Societe = ra.Manager.EtablissementPaie.Societe != null ? Context.Societes.Find(ra.Manager.EtablissementPaie.SocieteId) : null;
                ra.Manager.EtablissementPaie.Pays = ra.Manager.EtablissementPaie.Pays != null ? Context.Pays.Find(ra.Manager.EtablissementPaie.PaysId) : null;
                ra.Manager.EtablissementPaie = Context.EtablissementsPaie.Find(ra.Manager.EtablissementPaie.EtablissementPaieId);
            }
        }

        private void AttachPersonnelResponsableChantier(PersonnelEnt rc)
        {
            if (rc == null)
            {
                return;
            }

            rc.Societe = Context.Societes.Find(rc.SocieteId);
            rc.Pays = Context.Pays.Find(rc.PaysId);

            if (rc.EtablissementPaie != null)
            {
                rc.EtablissementPaie.Societe = rc.EtablissementPaie.Societe != null ? Context.Societes.Find(rc.EtablissementPaie.SocieteId) : null;
                rc.EtablissementPaie.Pays = rc.EtablissementPaie.Pays != null ? Context.Pays.Find(rc.EtablissementPaie.PaysId) : null;
                rc.EtablissementPaie = Context.EtablissementsPaie.Find(rc.EtablissementPaie.EtablissementPaieId);
            }

            if (rc.EtablissementRattachement != null)
            {
                rc.EtablissementRattachement.Societe = rc.EtablissementRattachement.Societe != null ? Context.Societes.Find(rc.EtablissementRattachement.SocieteId) : null;
                rc.EtablissementRattachement.Pays = rc.EtablissementRattachement.Pays != null ? Context.Pays.Find(rc.EtablissementRattachement.PaysId) : null;
                rc.EtablissementRattachement = Context.EtablissementsPaie.Find(rc.EtablissementRattachement.EtablissementPaieId);
            }
        }

        /// <summary>
        /// Retourne le ci par rapport à son identifiant unique 
        /// </summary>
        /// <param name="id">Identifiant unique</param>
        /// <param name="withSocieteInclude">Indique si on inclut la société</param>
        /// <returns>Renvoie le ci.</returns>
        public CIEnt GetCiById(int id, bool withSocieteInclude)
        {
            if (withSocieteInclude)
            {
                return Context.CIs
                    .Include(c => c.Societe.TypeSociete)
                    .Include(c => c.Societe.AssocieSeps).ThenInclude(a => a.TypeParticipationSep)
                    .FirstOrDefault(c => c.CiId == id);
            }

            return Context.CIs.FirstOrDefault(c => c.CiId == id);
        }

        /// <summary>
        /// Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="entity">L'entité concernée.</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <exception cref="System.UnauthorizedAccessException">Utilisateur n'a pas l'autorisation</exception>
        public override void CheckAccessToEntity(CIEnt entity, int userId)
        {
            if (entity.Organisation == null)
            {
                PerformEagerLoading(entity, x => x.Organisation);
            }

            if (entity.Organisation != null)
            {
                var orgaList = userRepo.GetOrganisationsAvailable(null, new List<int> { entity.Organisation.TypeOrganisationId }, userId);
                if (orgaList.All(o => o.OrganisationId != entity.Organisation.OrganisationId))
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }

        #region Gestion des CI

        /// <summary>
        /// Récupère une liste de CI selon l'identifiant de l'organisation du CI
        /// </summary>
        /// <param name="organisationIds">Liste d'identifiants d'organisation</param>
        /// <param name="loadNestedObjects">Permet de charger les objets liés (par défaut à true)</param>
        /// <returns>Liste de CI</returns>
        public IEnumerable<CIEnt> GetByOrganisationId(IEnumerable<int> organisationIds, bool loadNestedObjects = true)
        {
            IQueryable<CIEnt> cisSet = Context.CIs.AsQueryable();
            if (loadNestedObjects)
            {
                cisSet = cisSet.Include(e => e.EtablissementComptable.Societe)
                             .Include(s => s.Societe)
                             .Include(s => s.ResponsableAdministratif.Societe)
                             .Include(s => s.MontantDevise)
                             .Include(s => s.Organisation)
                             .Include(x => x.Pays)
                             .Include(x => x.PaysLivraison)
                             .Include(x => x.PaysFacturation);
            }

            IEnumerable<CIEnt> cis = cisSet.Where(c => organisationIds.Contains(c.Organisation.OrganisationId)).AsNoTracking();
            foreach (var ci in cis)
            {
                if (ci.Organisation != null)
                {
                    ci.Organisation.CI = null;
                }
            }

            return cis;
        }

        /// <summary>
        /// Récupération de la liste des CI dont l'organisationId est compris dans la liste en paramètre
        /// </summary>
        /// <param name="organisationIds">Liste d'identifiants d'organisation</param>
        /// <returns>Liste des identifiants CI</returns>

        public IEnumerable<int> GetCIIdListByOrganisationId(IEnumerable<int> organisationIds)
        {
            return Query()
                   .Filter(x => organisationIds.Contains(x.Organisation.OrganisationId))
                   .Get()
                   .AsNoTracking()
                   .Select(x => x.CiId);
        }

        /// <summary>
        /// Récupération de la liste des CI dont l'organisationId est compris dans la liste en paramètre de manière async
        /// </summary>
        /// <param name="organisationIds">Liste d'identifiants d'organisation</param>
        /// <returns>Liste des identifiants CI</returns>
        public async Task<IEnumerable<int>> GetCIIdListByOrganisationIdAsync(IEnumerable<int> organisationIds)
        {
            return await Context.CIs
                .Where(c => organisationIds.Contains(c.Organisation.OrganisationId))
                .AsNoTracking()
                .Select(c => c.CiId)
                .ToListAsync();
        }

        /// <summary>
        /// Retourne la liste des cis.
        /// </summary>
        /// <param name="onlyChantierFred">Indique si seuls les chantiers gérés par FRED sont retournés</param>
        /// <param name="groupeId">L'identifiant du groupe concerné ou null pour tous les groupes.</param>
        /// <returns>Liste des cis.</returns>
        public IEnumerable<CIEnt> Get(bool onlyChantierFred = false, int? groupeId = null)
        {
            return Query()
                    .Include(e => e.EtablissementComptable.Societe)
                    .Include(o => o.Organisation)
                    .Include(s => s.Societe)
                    .Include(x => x.Pays)
                    .Include(x => x.PaysLivraison)
                    .Include(x => x.PaysFacturation)
                    .Filter(s =>
                        (s.DateFermeture >= DateTime.Today || s.DateFermeture == null)
                        && (!onlyChantierFred || s.ChantierFRED)
                        && (!groupeId.HasValue || s.Societe.GroupeId == groupeId.Value))
                    .Get()
                    .AsNoTracking();
        }


        /// <summary>
        /// Retourne le ci portant l'identifiant unique indiqué.
        /// /!\ LCO : laisser ci.Organisation.CI = null jusqu'à correction du problème de stack overflow lors d'un changement de
        /// CI sur un pointage
        /// </summary>
        /// <param name="ciID">Identifiant du ci à retrouver.</param>
        /// <returns>le ci retrouvée, sinon nulle.</returns>
        public CIEnt Get(int ciID)
        {
            CIEnt ci = Context.CIs.Include(e => e.EtablissementComptable.Societe.Groupe)
                                  .Include(e => e.EtablissementComptable.Pays)
                                  .Include(s => s.Societe.Groupe)
                                  .Include(s => s.Societe.TypeSociete)
                                  .Include(s => s.ResponsableAdministratif.Societe)
                                  .Include(s => s.PersonnelResponsableChantier.Societe)
                                  .Include(s => s.MontantDevise)
                                  .Include(s => s.Organisation)
                                  .Include(x => x.Pays)
                                  .Include(x => x.PaysLivraison)
                                  .Include(x => x.PaysFacturation)
                                  .Include(x => x.CIType)
                                  .Include(x => x.Taches)
                                  .Include(x => x.CompteInterneSep)
                                  .AsNoTracking()
                                  .SingleOrDefault(c => c.CiId.Equals(ciID));
            if (ci?.Sep == false && ci?.Societe == null)
            {
                ci.Societe = ci?.EtablissementComptable?.Societe;
            }

            if (ci?.Organisation != null)
            {
                ci.Organisation.CI = null;
            }

            return ci;
        }

        /// <summary>
        /// Ajoute une nouvelle ci
        /// </summary>
        /// <param name="ciEnt">CI à ajouter</param>
        /// <returns>CI ajouté</returns>
        public CIEnt AddCI(CIEnt ciEnt)
        {
            ciEnt.Societe = null;
            ciEnt.EtablissementComptable = null;
            if (Context.Entry(ciEnt).State != EntityState.Detached && Context.Entry(ciEnt).State != EntityState.Unchanged)
            {
                AttachDependancies(ciEnt);
            }
            Insert(ciEnt);

            return ciEnt;
        }

        /// <summary>
        /// Sauvegarde les modifications d'une ci.
        /// </summary>
        /// <param name="ciEnt">CI à modifier</param>
        /// <returns>CI mis à jour</returns>
        public CIEnt UpdateCI(CIEnt ciEnt)
        {
            try
            {
                AttachDependancies(ciEnt);
                Update(ciEnt);
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
            return ciEnt;
        }

        /// <inheritdoc/>
        public IEnumerable<CIEnt> AddOrUpdateCIList(IEnumerable<CIEnt> cis, bool updateOrganisation = false)
        {
            /* using (var ctxt = new FredDbContext()) 
             * N'a pas été utilisé ici car les mises à jour ne fonctionnent pas avec ce nouveau contexte...
             * ....mystère et boule de gomme d'entity framework.....
             */
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    // disable detection of changes
                    Context.ChangeTracker.AutoDetectChangesEnabled = false;
                    int count = 0;
                    const int commitCount = 100;

                    // Mise à jour des CIs en BDD
                    foreach (CIEnt ci in cis.Where(x => x.CiId > 0))
                    {
                        ++count;
                        Update(ci);
                        if (updateOrganisation)
                        {
                            var entry = Context.Entry(ci.Organisation);
                            entry.State = EntityState.Modified;
                        }
                        // A chaque 100 opérations, on sauvegarde le contexte.
                        if (count % commitCount == 0)
                        {
                            Context.SaveChanges();
                        }
                    }

                    // Ajout des CIs en BDD
                    var addedList = cis.Where(x => x.CiId == 0 && x.Organisation != null).ToList();
                    Context.CIs.AddRange(addedList);

                    Context.SaveChanges();
                    dbContextTransaction.Commit();
                    return addedList;
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
        /// Supprime une ci
        /// </summary>
        /// <param name="id">L'identifiant de l'ci à supprimer</param>
        public void DeleteCIById(int id)
        {
            CIEnt ci = Context.CIs.Find(id);
            if (ci == null)
            {
                throw new FredTechnicalException("Impossible de trouver le CI dont l'identifiant est : " + id.ToString());
            }
            Delete(ci);
        }



        /// <inheritdoc/>
        public int? GetOrganisationIdByCiId(int ciId)
        {
            var organisations =
              Query()
              .Get()
              .Where(ci => ci.CiId == ciId)
              .Select(ci => ci.Organisation.OrganisationId)
              .ToList(); //On fait un ToList direct ici pour éviter d'avoir à appeler deux fois la base de données 
                         //(une fois pour savoir si l'organisation existe et une fois pour récupérer son id)

            if (organisations.Count == 0)
            {
                return null;
            }

            //L'id du CI étant unique, on peut sans risque retourner le premier élément de la liste
            //https://stackoverflow.com/questions/25606934/difference-between-linq-first-vs-array0
            return organisations[0];
        }

        /// <summary>
        ///   Permet de récupérer tous les identifiants des organisations de chaque CI dans 'ciIds'
        /// </summary>
        /// <param name="ciIds">Liste d'identifiants de CI</param>
        /// <returns>Dictionnaire contenant (CiId, OrganisationId)</returns>
        public Dictionary<int, int?> GetOrganisationIdByCiId(IEnumerable<int> ciIds)
        {
            Dictionary<int, int?> dico = new Dictionary<int, int?>();

            ciIds = ciIds.Distinct().ToList();

            var cis = Query()
                        .Filter(ci => ciIds.Contains(ci.CiId))
                        .Include(x => x.Organisation)
                        .Get()
                        .ToList(); //On fait un ToList direct ici pour éviter d'avoir à appeler deux fois la base de données 
                                   //(une fois pour savoir si l'organisation existe et une fois pour récupérer son id)

            foreach (int ciId in ciIds)
            {
                CIEnt ci = cis.Find(x => x.CiId == ciId);

                dico.Add(ciId, ci?.Organisation?.OrganisationId);
            }

            return dico;
        }

        /// <inheritdoc/>
        public DateTime? GetDateOuvertureCi(int ciId)
        {
            return Query()
                .Get()
                .Where(ci => ci.CiId == ciId)
                .Select(ci => ci.DateOuverture)
                .FirstOrDefault();
        }

        /// <summary>
        /// Permet de récupérer la liste des types de CI.
        /// </summary>
        /// <returns>Liste des types de CI</returns>
        public IEnumerable<CITypeEnt> GetCITypes()
        {
            return Context.CIType.AsNoTracking();
        }



        /// <summary>
        /// Retourne les cis appartenant à un établissement comptable pour picklist
        /// </summary>
        /// <param name="organisationId">identifiant unique de l'organisation de l'établissemet comptable</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">taille de la page</param>
        /// <returns>Liste de Ci appartenant à une société</returns>
        public IEnumerable<CIEnt> SearchLightOrganisationCiByOrganisationPereId(int organisationId, int page, int pageSize)
        {
            return Query()
              .Include(z => z.Organisation)
              .Include(z => z.Organisation.TypeOrganisation)
              .Filter(p => p.Organisation.PereId == organisationId)
              .OrderBy(p => p.OrderBy(l => l.Code))
              .GetPage(page, pageSize)
              .ToList();
        }

        /// <summary>
        /// Get Ci by liste d'identifiant unique de société
        /// </summary>
        /// <param name="societeId">liste d'identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        public List<CIEnt> GetCIListBySocieteId(int societeId)
        {
            return Query()
              .Filter(c => c.SocieteId == societeId)
              .Get()
              .ToList();
        }

        /// <summary>
        /// Get liste ciid par identifiant unique de société
        /// </summary>
        /// <param name="societeId">identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        public List<int> GetCiIdListBySocieteId(int societeId)
        {
            return Query()
              .Filter(c => c.SocieteId == societeId)
              .Get()
              .Select(c => c.CiId)
              .ToList();
        }

        /// <summary>
        /// Get liste ciid par identifiant unique de société pour traitement des exports reception intérimaire
        /// </summary>
        /// <param name="societeId">identifiant unique de société</param>
        /// <returns>Liste de CIs</returns>
        public List<int> GetCiIdListBySocieteIdForInterimaire(int societeId)
        {
            return Query()
              .Filter(c => c.SocieteId == societeId ||
                      c.Societe.SocieteGeranteId == societeId ||
                      c.Societe.AssocieSeps.Select(a => a.SocieteAssocieeId).Contains(societeId))
              .Get()
              .Select(c => c.CiId)
              .ToList();
        }

        /// <summary>
        /// Renvoie la liste des Ci par liste des codes
        /// </summary>
        /// <param name="codeList">Liste des codes des Cis à renvoyer</param>
        /// <returns>Liste des Cis qui corresponds aux codes demandés</returns>
        public IEnumerable<CIEnt> GetCiByCodeList(IEnumerable<string> codeList)
        {
            return Query()
                    .Filter(x => codeList.Contains(x.Code))
                    .Get()
                    .ToList();
        }

        /// <summary>
        /// Renvoie la liste des identifiants d'organisation des cis cloturés
        /// </summary>
        /// <returns>Liste des identifiants d'organisation des cis cloturés</returns>
        public List<int> GetOrganisationIdCiClose()
        {
            return Query()
                    .Include(o => o.Organisation)
                    .Filter(x => x.DateFermeture.HasValue && ((x.DateFermeture.Value.Year * 100) + x.DateFermeture.Value.Month < (DateTime.Today.Year * 100) + DateTime.Today.Month))
                    .Get()
                    .Select(x => x.Organisation.OrganisationId)
                    .ToList();
        }

        /// <summary>
        /// Permet d'obtenir la liste des cis generique absence
        /// </summary>
        /// <returns>Liste de CIEnt</returns>
        public List<CIEnt> GetCisAbsenceGenerique()
        {
            return Context.CIs.Where(c => c.IsAbsence).Include(x => x.Societe.EtablissementComptables).AsNoTracking().ToList();
        }

        /// <summary>
        /// Permet d'avoir le ci à partir de l'établissementComptaId
        /// </summary>
        /// <param name="etablissementComptaId">Identifiant etab compta</param>
        /// <returns>retourne une ci</returns>
        public CIEnt GetCiAbsenceGeneriqueByEtabId(int etablissementComptaId)
        {
            return Context.CIs.FirstOrDefault(c => c.IsAbsence && etablissementComptaId == c.EtablissementComptableId);
        }

        #endregion

        #region Gestion des Devises

        /// <summary>
        /// Retourne la liste des devises  d'un CI.
        /// </summary>
        /// <returns>Liste des devises  d'un CI</returns>
        /// <param name="ciId">identifiant du ci à mettre a jour</param>
        public IEnumerable<CIDeviseEnt> GetCIDevise(int ciId)
        {
            return Context.CIDevises
                          .Include(cid => cid.Devise)
                          .AsNoTracking()
                          .Where(cid => cid.CiId.Equals(ciId));
        }

        /// <summary>
        /// Récupère les devises pour la comparaison de budget.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI du budget.</param>
        /// <returns>Les devises.</returns>
        public List<DeviseDao> GetDevisesPourBudgetComparaison(int ciId)
        {
            return Context.CIDevises
                .Where(cd => cd.CiId == ciId)
                .Select(cd => new DeviseDao
                {
                    DeviseId = cd.DeviseId,
                    Symbole = cd.Devise.Symbole,
                    IsoCode = cd.Devise.IsoCode,
                    Libelle = cd.Devise.Libelle
                })
                .ToList();
        }

        /// <summary>
        /// Retourne la devise de référence d'un CI.
        /// </summary>
        /// <returns>Devise de référence d'un CI</returns>
        /// <param name="ciId">identifiant du ci à mettre a jour</param>
        public DeviseEnt GetDeviseRef(int ciId)
        {
            return Context.CIDevises
                          .Include(s => s.Devise)
                          .Where(x => x.CiId.Equals(ciId) && x.Reference)
                          .Select(d => d.Devise)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        /// <summary>
        /// Retourne la liste de devise secondaire du CI passé en paramètre
        /// </summary>
        /// <param name="idCI"> Identifiant du CI </param>
        /// <returns> Liste de toutes les sociétés/Devises </returns>
        public IEnumerable<DeviseEnt> GetCIDeviseSecList(int idCI)
        {
            return Context.CIDevises
                          .Include(d => d.Devise)
                          .Where(x => x.CiId.Equals(idCI) && !x.Reference)
                          .Select(d => d.Devise)
                          .AsNoTracking();
        }

        /// <summary>
        /// Evalue si le Ci possèdent plusieurs Devises
        /// </summary>
        /// <param name="idCI"> Identifiant du CI </param>
        /// <returns> Vrai si le Ci possède plusieurs Devises, faux sinon </returns>
        public bool IsCiHaveManyDevises(int idCI)
        {
            return Context.CIDevises
                          .Include(d => d.Devise)
                          .Where(x => x.CiId.Equals(idCI))
                          .Count() > 1;
        }

        /// <summary>
        /// Sauvegarde les modifications d'une ci.
        /// </summary>
        /// <param name="ciDeviseEnt">relation Ci Devise</param>
        /// <returns>CIDevise crée</returns>
        public CIDeviseEnt AddCIDevise(CIDeviseEnt ciDeviseEnt)
        {
            Context.CIDevises.Add(ciDeviseEnt);

            return ciDeviseEnt;
        }

        /// <summary>
        /// Insertion de masse des CIDevise
        /// </summary>
        /// <param name="ciDeviseList">Liste de CIDevise</param>
        public void BulkAddCIDevise(IEnumerable<CIDeviseEnt> ciDeviseList)
        {
            using (var ctxt = new FredDbContext())
            {
                using (var dbContextTransaction = ctxt.Database.BeginTransaction())
                {
                    try
                    {
                        // disable detection of changes
                        ctxt.ChangeTracker.AutoDetectChangesEnabled = false;

                        // Ajout des CIDevise en BDD           
                        ctxt.CIDevises.AddRange(ciDeviseList);

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
        /// Mise à jour d'un CIDevise 
        /// </summary>
        /// <param name="ciDeviseEnt">CIDEvise à mettre à jour</param>
        /// <returns>CIDevise mis à jour</returns>
        public CIDeviseEnt UpdateCIDevise(CIDeviseEnt ciDeviseEnt)
        {
            Context.CIDevises.Update(ciDeviseEnt);

            return ciDeviseEnt;
        }

        /// <summary>
        /// Mise à jour des Devises d'un CI
        /// </summary>
        /// <param name="listCIDevise">liste des relation Ci Devise</param>    
        /// <returns>Liste des CIDevise mise à jour</returns>
        public IEnumerable<CIDeviseEnt> AddOrUpdateCIDevises(IEnumerable<CIDeviseEnt> listCIDevise)
        {
            foreach (var ciDevise in listCIDevise.ToList())
            {
                if (ciDevise.Devise != null)
                {
                    Context.Devise.Attach(ciDevise.Devise);
                }

                if (ciDevise.CiDeviseId.Equals(0))
                {
                    Context.CIDevises.Add(ciDevise);
                }
                else
                {
                    Context.CIDevises.Update(ciDevise);
                }
            }

            return listCIDevise;
        }

        /// <inheritdoc />
        public void DeleteCIDevise(int ciDeviseId)
        {
            var ciDevise = new CIDeviseEnt { CiDeviseId = ciDeviseId };
            Context.CIDevises.Remove(ciDevise);
        }

        #endregion

        #region Gestion des Ressources

        /// <inheritdoc />
        public CIRessourceEnt AddCIRessource(CIRessourceEnt ciRessource)
        {
            Context.CIRessources.Add(ciRessource);

            return ciRessource;
        }

        /// <inheritdoc />
        public CIRessourceEnt UpdateCIRessource(CIRessourceEnt ciRessource)
        {
            Context.CIRessources.Update(ciRessource);

            return ciRessource;
        }

        /// <inheritdoc />
        public void DeleteCIRessource(int ciRessourceId)
        {
            var ciRessource = new CIRessourceEnt { CiRessourceId = ciRessourceId };
            Context.CIRessources.Remove(ciRessource);
        }

        #endregion

        /// <summary>
        /// Récupère le ci de passage d'un ci sep ainsi que sa société
        /// </summary>
        /// <param name="ciId">Identifiant unique du ci sep </param>
        /// <returns>Ci de passage avec sa societe inclus</returns>
        public CIEnt GetCICompteInterneByCiid(int ciId)
        {
            var ciSep = Query()
                .Include(c => c.CompteInterneSep.Societe)
                .Filter(c => c.CiId == ciId)
                .Get()
                .FirstOrDefault();

            return ciSep.CompteInterneSep;
        }

        /// <summary>
        /// Récupére une liste de CI en fonction d'une liste d'identifiant (version light sans societe)
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant</param>
        /// <returns>Liste de CI</returns>
        public IEnumerable<CIEnt> GetCIsByIdsLight(List<int> ciIds)
        {
            //YCO : Attention, code smell : Methode GetCIsByIds et GetCIsByIdsLight a remplacer par un systeme de DTO.
            //La suppression de la clause AsNoTracking() de la methode GetCIsByIds() a entrainer des effets de bords sur l'import des CIs (BUG 8421).
            return Context.CIs.Where(q => ciIds.Contains(q.CiId));
        }

        /// <summary>
        /// Récupére une liste de CI en fonction d'une liste d'identifiant
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant</param>
        /// <returns>Liste de CI</returns>
        public IEnumerable<CIEnt> GetCIsByIds(List<int> ciIds)
        {
            return Context.CIs
                    .Where(x => ciIds.Contains(x.CiId))
                    .AsNoTracking();
        }

        public IReadOnlyList<int> GetCiIdsBySocieteIds(List<int> societeIds)
        {
            return Context.CIs
                .Where(x => societeIds.Contains(x.SocieteId.Value))
                .Select(c => c.CiId).ToList();
        }

        /// <summary>
        /// Recupere les ci pour la vue 'liste des cis'
        /// </summary>
        /// <param name="filters">filtre de la vue</param>
        /// <param name="includeInCiList">liste de ci dans laquelle la recherche doit etre faites</param>
        /// <param name="includeInTypeSocietes">liste de type de societe dans laquelle la recherche doit etre faites</param>
        /// <param name="page">la page</param>
        /// <param name="pageSize">la taille de la page</param>
        /// <returns>Liste de ci en fonction de tous les filtre</returns>
        public List<CIEnt> GetForCiListView(SearchCIEnt filters, List<int> includeInCiList, List<int?> includeInTypeSocietes, int page, int pageSize)
        {
            return Query()
                    .Include(e => e.EtablissementComptable.Societe)
                    .Include(e => e.Societe)
                    .Include(e => e.CIType)
                    .Filter(c => includeInCiList.Contains(c.CiId))
                    .Filter(filters.GetPredicateWhere())
                    .Filter(e => includeInTypeSocietes.Contains(e.Societe.TypeSocieteId))
                    .OrderBy(c => c.OrderBy(ci => ci.Code))
                    .GetPage(page, pageSize)
                    .AsNoTracking()
                    .ToList();
        }


        /// <summary>
        /// SearchLight pour Lookup des CI Sep
        /// CI visibles par l’utilisateur ET rattachés à une société de type SEP.
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="searchedText">Texte a rechercher</param>
        /// <param name="ciIdList">Liste des Cis Utilisateur</param>
        /// <returns>Liste de CI</returns>
        public List<CIEnt> SearchLightCiSep(int page, int pageSize, string searchedText, List<int> ciIdList)
        {
            return Context.CIs
                .Include(x => x.Societe)
                .Where(c => ciIdList.Count == 0 || ciIdList.Contains(c.CiId))
                .Where(ci => ci.DateFermeture == null || ci.DateFermeture > DateTime.Now)
                .Where(ci => string.IsNullOrEmpty(searchedText) || ci.Code.Contains(searchedText) || ci.Libelle.Contains(searchedText))
                .Where(x => x.Societe.TypeSociete.Code == Constantes.TypeSociete.Sep)
                .OrderBy(ci => ci.Code)
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// recuperer la liste de CI afféctés à un personnel
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">page size</param>
        /// <param name="personnelId">personnel id</param>
        /// <returns>retourne liste des CI d'un personnel donné</returns>
        public IEnumerable<CIEnt> SearchLightByPersonnelIdStandard(string text, int page, int pageSize, int personnelId)
        {
            return Query()
                        .Include(e => e.EtablissementComptable.Societe)
                        .Include(s => s.Societe)
                        .Include(s => s.Societe.Groupe)
                        .Include(pl => pl.PaysLivraison)
                        .Include(pf => pf.PaysFacturation)
                        .Include(p => p.Pays)
                        .Include(c => c.Organisation)
                        .Include(a => a.Affectations)
                        .Include(t => t.CIType)
                        .Filter(ci => ci.DateFermeture == null || ci.DateFermeture > DateTime.Now)
                        .Filter(ci => string.IsNullOrEmpty(text) || ci.Code.Contains(text.ToLower()) || ci.Libelle.Contains(text.ToLower()))
                        .Filter(ci => ci.Affectations.Any(a => a.PersonnelId == personnelId && !a.IsDelete))
                        .OrderBy(o => o.OrderBy(ci => ci.Code))
                        .GetPage(page, pageSize);
        }

        /// <summary>
        /// Get ci for affectation by ci id
        /// </summary>
        /// <param name="ciId">Ci Identifier</param>
        /// <returns>Ci ent</returns>
        public CIEnt GetCiForAffectationByCiId(int ciId)
        {
            return Context.CIs.Include(c => c.Societe.Groupe).Include(c => c.Organisation).FirstOrDefault(x => x.CiId == ciId);
        }

        /// <summary>
        /// Récupère le ci par code et l'identifiant de la société 
        /// </summary>
        /// <param name="code">Code CI</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Ci de passage avec sa societe inclus</returns>
        public CIEnt GetCIByCodeAndSocieteId(string code, int societeId)
        {
            return Context.CIs.Where(x => x.Code == code && x.SocieteId == societeId).FirstOrDefault();
        }

        public List<CiDateOuvertureDateFermeture> GetDateOuvertureFermeturesCis(List<int> ciIds)
        {
            return this.Context.CIs.Where(x => ciIds.Contains(x.CiId))
                                                             .Select(x => new CiDateOuvertureDateFermeture
                                                             {
                                                                 CiId = x.CiId,
                                                                 DateOuverture = x.DateOuverture,
                                                                 DateFermeture = x.DateFermeture
                                                             })
                                                             .AsNoTracking()
                                                             .ToList();
        }
    }
}
