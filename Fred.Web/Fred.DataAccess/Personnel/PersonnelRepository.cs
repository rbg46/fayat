using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Affectation;
using Fred.Entities.Budget.Dao.Budget.ExcelExport;
using Fred.Entities.Groupe;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Import;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using static Fred.Entities.Constantes;

namespace Fred.DataAccess.Personnel
{
    /// <summary>
    ///   Référentiel de données pour du personnel.
    /// </summary>
    public class PersonnelRepository : FredRepository<PersonnelEnt>, IPersonnelRepository
    {
        public PersonnelRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        ///   Permet de détacher les entités dépendantes pour éviter de les prendre en compte dans la sauvegarde du contexte.
        /// </summary>
        /// <param name="persoInterne">objet dont les dépendances sont à détachées</param>
        public void DetachDependancies(PersonnelEnt persoInterne)
        {
            if (persoInterne.Societe != null)
            {
                persoInterne.SocieteId = persoInterne.Societe.SocieteId;
                persoInterne.Societe = null;
            }

            if (persoInterne.EtablissementPaie != null)
            {
                persoInterne.EtablissementPaieId = persoInterne.EtablissementPaie.EtablissementPaieId;
                persoInterne.EtablissementPaie = null;
            }

            if (persoInterne.EtablissementRattachement != null)
            {
                persoInterne.EtablissementRattachementId = persoInterne.EtablissementRattachement.EtablissementPaieId;
                persoInterne.EtablissementRattachement = null;
            }

            if (persoInterne.Ressource != null)
            {
                persoInterne.RessourceId = persoInterne.Ressource.RessourceId;
                persoInterne.Ressource = null;
            }

            if (persoInterne.Pays != null)
            {
                persoInterne.PaysId = persoInterne.Pays.PaysId;
                persoInterne.Pays = null;
            }
        }

        /// <summary>
        /// Retourne la requête de récupération du personnel
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        public List<PersonnelEnt> Get(List<Expression<Func<PersonnelEnt, bool>>> filters,
                                        Func<IQueryable<PersonnelEnt>, IOrderedQueryable<PersonnelEnt>> orderBy = null,
                                        List<Expression<Func<PersonnelEnt, object>>> includeProperties = null,
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
        ///   Retourne la liste des membres du personnel.
        /// </summary>
        /// <returns>Liste du personnel.</returns>
        public IEnumerable<PersonnelEnt> GetPersonnelList()
        {
            return Query()
                .Include(u => u.Utilisateur)
                .Include(s => s.Societe)
                .Include(m => m.Materiel)
                .Get()
                .AsNoTracking()
                .OrderBy(m => m.Matricule)
                .ThenBy(s => s.Societe.Code);
        }

        /// <summary>
        ///   Retourne la liste des membres du personnel.
        /// </summary>
        /// <returns>Liste du personnel.</returns>
        public IEnumerable<PersonnelEnt> GetOutgoingPersonnelsList(string suffixDisableLogin = "-old")
        {
            return Context.Personnels
                .Include(u => u.Utilisateur.AffectationsRole)
                .Include(u => u.Utilisateur.ExternalDirectory)
                .Where(p => p.DateSortie != null &&
                             p.DateSortie < DateTime.Now &&
                             !string.IsNullOrEmpty(p.Utilisateur.Login) &&
                             p.Utilisateur.Login.IndexOf(suffixDisableLogin, StringComparison.CurrentCultureIgnoreCase) <= 0)
                .ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<PersonnelEnt> GetPersonnellListForExportExcel(SearchPersonnelEnt filter, bool isConnectedUserSuperAdmin, int? connectedUserGroupId)
        {
            return Query()
                .Include(u => u.Utilisateur)
                .Include(s => s.Societe)
                .Include(p => p.EtablissementPaie)
                .Include(p => p.Ressource)
                .Include(p => p.Pays)
                .Filter(filter.GetPredicateWhere())
                .Filter(x => isConnectedUserSuperAdmin || connectedUserGroupId == x.Societe.GroupeId)
                .OrderBy(filter.ApplyOrderBy)
                .Get();
        }

        /// <summary>
        ///   Retourne la liste des membres du personnel pour la synchronisation mobile.
        /// </summary>
        /// <returns>Liste du personnel.</returns>
        public IEnumerable<PersonnelEnt> GetPersonnelListSync()
        {
            return Context.Personnels.AsNoTracking();
        }

        /// <inheritdoc/>
        public PersonnelEnt GetPersonnel(int societeId, string matricule)
        {
            return GetDefaultQuery()
                   .Include(x => x.Societe.Organisation)
                   .Get()
                   .AsNoTracking()
                   .FirstOrDefault(p => p.SocieteId.HasValue && p.SocieteId.Value == societeId && p.Matricule.Trim() == matricule.Trim());
        }

        /// <inheritdoc/>
        public PersonnelEnt GetPersonnel(int personnelId, bool withDependencies = false)
        {
            IQueryable<PersonnelEnt> getQuery = GetDefaultQuery().Get().AsNoTracking().Include(p => p.Societe);

            if (!withDependencies)
            {
                getQuery = getQuery.Include(p => p.Manager).Include(p => p.Manager.Societe).Include(g => g.Societe.Groupe);
            }

            return getQuery.FirstOrDefault(p => p.PersonnelId.Equals(personnelId));
        }

        /// <inheritdoc/>
        public PersonnelEnt GetSimplePersonnel(int personnelId)
        {
            return Query().Get().AsNoTracking().FirstOrDefault(p => p.PersonnelId.Equals(personnelId));
        }

        /// <inheritdoc/>
        public PersonnelEnt GetPersonnelByNomPrenom(string nom, string prenom, int? groupeId)
        {
            return GetDefaultQuery()
                   .Get()
                   .AsNoTracking()
                   .FirstOrDefault(ss => string.Compare(ss.Nom.Trim(), nom.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0 &&
                                         string.Compare(ss.Prenom.Trim(), prenom.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0 &&
                                         (!groupeId.HasValue || groupeId.HasValue && groupeId.Value == ss.Societe.GroupeId));
        }

        /// <inheritdoc/>
        public PersonnelEnt GetPersonnelById(int? personnelId)
        {
            return GetDefaultQuery()
                  .Include(p => p.EtablissementPaie)
                  .Include(p => p.Societe)
                  .Include(p => p.Societe.Groupe)
                  .Get()
                  .AsNoTracking()
                  .FirstOrDefault(i => i.PersonnelId == personnelId);
        }

        /// <summary>
        ///   Retourne le personnel en fonction de son email
        /// </summary>
        /// <param name="email">Email du personnel</param>
        /// <returns>Personnel</returns>
        public PersonnelEnt GetPersonnelByEmail(string email)
        {
            return Query()
                    .Filter(p => p.Email == email)
                    .Include(p => p.Utilisateur)
                    .Include(p => p.Utilisateur.ExternalDirectory)
                    .Get()
                    .AsNoTracking()
                    .FirstOrDefault();
        }

        /// <inheritdoc/>
        public MaterielEnt GetMaterielDefault(int personnelId)
        {
            var personnel = GetDefaultQuery()
                              .Include(p => p.Materiel.Societe)
                              .Get()
                              .AsNoTracking()
                              .FirstOrDefault(p => p.PersonnelId.Equals(personnelId));
            if (personnel != null)
            {
                return personnel.Materiel;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///   Retourne la liste des personnels interne liés à une société spécifique
        /// </summary>
        /// <param name="codeSociete">Le code de la société</param>
        /// <returns>Retourne la liste des personnels de la société spécifiée</returns>
        public IEnumerable<PersonnelEnt> GetPersonnelListByCodeSocietePaye(string codeSociete)
        {
            return Query()
              .Include(x => x.Societe)
              .Include(x => x.MatriculeExterne)
              .Filter(x => x.Societe.CodeSocietePaye == codeSociete)
              .Get()
              .AsNoTracking();
        }

        /// <summary>
        ///   Ajoute un personnel
        /// </summary>
        /// <param name="personnel">Le personnel interne à ajouter</param>
        /// <returns>Retourne l'identifiant unique du personnel ajouté</returns>
        public PersonnelEnt AddPersonnel(PersonnelEnt personnel)
        {
            personnel.Utilisateur = null;
            if (Context.Entry(personnel).State == EntityState.Detached)
            {
                DetachDependancies(personnel);
            }
            Insert(personnel);

            return personnel;
        }

        /// <summary>
        ///   Mise à jour d'un personnel
        /// </summary>
        /// <param name="personnel">Le personnel à ajouter</param>
        /// <returns>Personnel mis à jour</returns>
        public PersonnelEnt UpdatePersonnel(PersonnelEnt personnel)
        {
            if (personnel.PersonnelId != personnel.UtilisateurId)
            {
                personnel.Utilisateur = null;
            }
            DetachDependancies(personnel);
            Update(personnel);

            return personnel;
        }

        /// <summary>
        ///   Récupère la liste des affectations d'un intérimaire en fonction de l'identifiant Personnel intérimaire
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personne intérimaire</param>
        /// <returns>Liste des affectation du personnel intérimaire</returns>
        public IEnumerable<ContratInterimaireEnt> GetContratInterimaireList(int personnelId)
        {
            return Context.ContratInterimaires
              .Include(p => p.Interimaire)
              .Include(f => f.Fournisseur)
              .Include(s => s.Societe)
              .Include(c => c.ZonesDeTravail)
              .Where(p => p.InterimaireId == personnelId)
              .AsNoTracking();
        }

        /// <summary>
        ///   Récupère l'affectation intérimaire active pour une date donnée
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="date">Date</param>
        /// <returns>l'affectation intérimaire active pour une date donnée</returns>
        public ContratInterimaireEnt GetContratInterimaireActive(int personnelId, DateTime date)
        {
            date = date.Date;
            ContratInterimaireEnt contratActif;
            var listContrats = this.Context.ContratInterimaires
             .Include(c => c.Interimaire)
             .Include(c => c.Fournisseur)
             .Include(c => c.Societe)
             .Include(c => c.ZonesDeTravail)
             .Where(c => c.InterimaireId == personnelId && c.DateDebut <= date && date <= c.DateFin.AddDays(c.Souplesse));

            contratActif = listContrats.FirstOrDefault(c => c.DateDebut <= date && date <= c.DateFin);
            if (contratActif == null)
            {
                List<ContratInterimaireEnt> listContratsActifs;
                listContratsActifs = listContrats.Where(c => c.DateFin < date && date <= c.DateFin.AddDays(c.Souplesse)).ToList();
                contratActif = listContratsActifs.OrderByDescending(c => c.DateDebut).FirstOrDefault();
            }
            return contratActif;
        }

        /// <summary>
        ///   Récupère la liste de toutes les affectations des intérimaires
        /// </summary>
        /// <returns>Liste des affectation des intérimaires</returns>
        public IEnumerable<ContratInterimaireEnt> GetContratInterimaireList()
        {
            return Context.ContratInterimaires
              .Include(p => p.Interimaire)
              .Include(f => f.Fournisseur)
              .Include(s => s.Societe)
              .AsNoTracking();
        }

        /// <summary>
        ///   Récupère la liste de toutes les affectations des intérimaires avec pagination
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personne intérimaire</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des affectation des intérimaires</returns>
        public IEnumerable<ContratInterimaireEnt> GetContratInterimaireList(int personnelId, int page, int pageSize)
        {
            return Context.ContratInterimaires
             .Include(p => p.Interimaire)
             .Include(f => f.Fournisseur)
             .Include(s => s.Societe)
             .Include(a => a.Ressource)
             .Where(p => p.InterimaireId == personnelId)
             .OrderBy(a => a.DateFin)
             .Skip((page - 1) * pageSize)
             .Take(pageSize)
             .AsNoTracking();
        }

        /// <summary>
        /// Ajoute une nouvelle affectation d'un intérimaire
        /// </summary>
        /// <param name="affectation">Affectation d'un intérimaire à ajouter</param>
        /// <returns>Nouvelle affectation</returns>
        public ContratInterimaireEnt AddContratInterimaire(ContratInterimaireEnt affectation)
        {
            affectation.Societe = null;
            affectation.Fournisseur = null;
            affectation.Interimaire = null;
            affectation.Ressource = null;
            Context.ContratInterimaires.Add(affectation);

            return affectation;
        }

        /// <summary>
        /// Mise à jour d'une affectation d'un intérimaire
        /// </summary>
        /// <param name="affectation">Affectation d'un intérimaire à mettre à jour</param>
        /// <returns>Affectation mise à jour</returns>
        public ContratInterimaireEnt UpdateContratInterimaire(ContratInterimaireEnt affectation)
        {
            affectation.Societe = null;
            affectation.Fournisseur = null;
            affectation.Interimaire = null;
            affectation.Ressource = null;
            Context.ContratInterimaires.Update(affectation);

            return affectation;
        }

        /// <summary>
        ///   Requête de filtrage du personnel par défaut
        /// </summary>
        /// <returns>Retourne le prédicat de filtrage du personnel par défaut</returns>
        private IRepositoryQuery<PersonnelEnt> GetDefaultQuery()
        {
            return Query()
                .Include(ep => ep.EtablissementPaie)
                .Include(er => er.EtablissementRattachement)
                .Include(p => p.Pays)
                .Include(s => s.Societe)
                .Include(u => u.Utilisateur)
                .Include(m => m.Materiel)
                .Include(uad => uad.Utilisateur.ExternalDirectory)
                .Include(r => r.Ressource);
        }

        /// <inheritdoc/>
        public void AddOrUpdatePersonnelList(IEnumerable<PersonnelEnt> personnels)
        {
            /* using (var ctxt = new FredDbContext()) 
         * N'a pas été utilisé ici car les mises à jour ne fonctionnent pas avec ce nouveau contexte...
         * ....mystère et boule de gomme d'entity framework.....
         */
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {

                    AddOrUpdatePersonnelsInBatch(personnels);
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


        private void AddOrUpdatePersonnelsInBatch(IEnumerable<PersonnelEnt> personnels)
        {
            // disable detection of changes
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
            int count = 0;
            const int commitCount = 100;

            // Mise à jour des personnels en BDD
            foreach (PersonnelEnt personnel in personnels.Where(x => x.PersonnelId > 0))
            {
                ++count;
                DetachDependancies(personnel);
                personnel.Utilisateur = null;
                Update(personnel);

                // A chaque 100 opérations, on sauvegarde le contexte.
                if (count % commitCount == 0)
                {
                    Context.SaveChanges();
                }
            }

            // Ajout des personnels en BDD
            var addedList = personnels.Where(x => x.PersonnelId == 0);
            Context.Personnels.AddRange(addedList);

            Context.SaveChanges();
        }

        /// <summary>
        /// Creation ou mise a jour des affectation suite a un import du personnel
        /// </summary>
        /// <param name="personnelAffectationResults">Liste d'affectation a mettre a jour ou a créer</param>
        public void ImportPersonnelsAffectations(List<PersonnelAffectationResult> personnelAffectationResults)
        {
            if (personnelAffectationResults == null || !personnelAffectationResults.Any())
            {
                return;
            }

            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    //Mise a jours des personnel
                    var personnels = personnelAffectationResults.Where(par => par.PersonnelIsNew || par.PersonnelIsModified).Select(i => i.Personnel).ToList();
                    if (personnels.Any())
                    {
                        AddOrUpdatePersonnelsInBatch(personnels);
                    }
                    var personnelsWithNewManager = new List<PersonnelEnt>();
                    foreach (PersonnelAffectationResult personnelAffectationResult in personnelAffectationResults.Where(par => par.ManagerIsNotCreatedInFred))
                    {
                        personnelAffectationResult.Personnel.ManagerId = personnelAffectationResult.Manager.PersonnelId;
                        personnelsWithNewManager.Add(personnelAffectationResult.Personnel);
                    }

                    if (personnelsWithNewManager.Any())
                    {
                        AddOrUpdatePersonnelsInBatch(personnelsWithNewManager);
                    }

                    var affectationsToProcessed = new List<AffectationEnt>();
                    foreach (PersonnelAffectationResult personnelAffectationResult in personnelAffectationResults)
                    {
                        personnelAffectationResult.ManageAffectationBeforeInsertionInFred();
                        affectationsToProcessed.AddRange(personnelAffectationResult.AffectationCanBeInsertedInFred());
                    }

                    if (affectationsToProcessed.Any())
                    {
                        AddOrUpdateAffectationBatch(affectationsToProcessed);
                    }

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

        private void AddOrUpdateAffectationBatch(IEnumerable<AffectationEnt> affectations)
        {
            // disable detection of changes
            Context.ChangeTracker.AutoDetectChangesEnabled = false;
            int count = 0;
            const int commitCount = 100;

            // Mise à jour des personnels en BDD
            foreach (AffectationEnt affectation in affectations.Where(x => x.AffectationId > 0))
            {
                ++count;
                var entry = Context.Entry(affectation);
                if (entry.State == EntityState.Detached)
                {
                    Context.Affectation.Attach(affectation);
                }
                entry.State = EntityState.Modified;
                // A chaque 100 opérations, on sauvegarde le contexte.
                if (count % commitCount == 0)
                {
                    Context.SaveChanges();
                }
            }
            // Ajout des personnels en BDD
            var addedList = affectations.Where(x => x.AffectationId == 0);
            Context.Affectation.AddRange(addedList);
            Context.SaveChanges();
        }

        /// <summary>
        /// Permet de récupérer une liste des délégués potentiels
        /// </summary>
        /// <param name="delegantId">Identifiant unique du délégant</param>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche2">Prénom recherché</param>
        /// <param name="recherche3">Autres infos recherchées</param>
        /// <returns>LA liste des  des délégués potentiels </returns>
        public IEnumerable<PersonnelEnt> GetDelegue(int delegantId, int societeId, string recherche, int page, int pageSize, string recherche2, string recherche3)
        {
            return Query()
              .Filter(p =>

              (string.IsNullOrEmpty(recherche) || p.Nom.ToLower().Contains(recherche.ToLower()))
                                                && (string.IsNullOrEmpty(recherche2) || p.Prenom.ToLower().Contains(recherche2.ToLower()))
                                                && (string.IsNullOrEmpty(recherche3) || p.Matricule.ToLower().Contains(recherche3.ToLower()))
              )
              .Filter(p => p.Utilisateur != null && p.PersonnelId != delegantId)
              .Filter(p => p.SocieteId == societeId)
              .Filter(p => p.DateEntree < DateTime.UtcNow && (p.DateSortie > DateTime.UtcNow || p.DateSortie == null) && p.DateSuppression == null)
              .OrderBy(p => p.OrderBy(l => l.Matricule))
              .GetPage(page, pageSize);

        }

        /// <summary>
        /// Permet de récupérer une liste des délégués potentiels
        /// </summary>
        /// <param name="delegantId">Identifiant unique du délégant</param>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <param name="recherche">Recherche</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>LA liste des  des délégués potentiels </returns>
        public IEnumerable<PersonnelEnt> GetDelegue(int delegantId, int societeId, string recherche, int page, int pageSize)
        {
            return Query()
              .Filter(p =>
#pragma warning disable RCS1155 // Use StringComparison when comparing strings.
                string.IsNullOrEmpty(recherche) || p.Matricule.ToLower().Contains(recherche.ToLower())
                            || p.Nom.ToLower().Contains(recherche.ToLower())
                            || p.Prenom.ToLower().Contains(recherche.ToLower()))
              .Filter(p => p.Utilisateur != null && p.PersonnelId != delegantId)
              .Filter(p => p.DateEntree < DateTime.UtcNow && (p.DateSortie > DateTime.UtcNow || p.DateSortie == null) && p.DateSuppression == null)
              .OrderBy(p => p.OrderBy(l => l.Matricule))
              .GetPage(page, pageSize);
#pragma warning restore RCS1155
        }

        /// <summary>
        /// Retourne la liste des intérimaires actifs pour un Ci donné
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>La liste des intérimaires actifs pour un Ci donné</returns>
        public IRepositoryQuery<PersonnelEnt> GetInterimaireActifList(int ciId)
        {
            var ci = this.Context.CIs.SingleOrDefault(c => c.CiId == ciId);
            var today = DateTime.Today;
            return this.Query().Filter(x => x.IsInterimaire)
                                     .Include(p => p.ContratInterimaires.Select(c => c.ZonesDeTravail))
                                     .Filter(p => p.ContratInterimaires.Any(c => c.DateDebut >= today && c.DateFin <= today && (c.CiId == ciId || c.ZonesDeTravail.Any(z => z.EtablissementComptableId == ci.EtablissementComptableId)))
                                                  || (p.ContratInterimaires.Any(c => c.DateDebut >= today && c.DateFin <= today && (c.CiId == ciId || c.ZonesDeTravail.Any(z => z.EtablissementComptableId == ci.EtablissementComptableId)))
                                                     && p.ContratInterimaires.Any(c => c.DateFin >= today && c.DateFin.AddDays(c.Souplesse) <= today && (c.CiId == ciId || c.ZonesDeTravail.Any(z => z.EtablissementComptableId == ci.EtablissementComptableId)))));
        }

        /// <summary>
        /// Récupérer les identifiants des personnels qui sont sous la responsabilité d'un manager
        /// </summary>
        /// <param name="managerId">L'identifiant du manager</param>
        /// <returns>La liste des identifiants</returns>
        public List<int> GetManagedPersonnelIds(int managerId)
        {
            return Context.Personnels.AsNoTracking()
                          .Where(p => p.ManagerId.HasValue
                                      && p.ManagerId.Value == managerId
                                      && p.Statut != null
                                      && (!p.DateSortie.HasValue || (p.DateSortie.Value.Year >= DateTime.UtcNow.Year && p.DateSortie.Value.Month >= DateTime.UtcNow.Month)) && p.DateSuppression == null)
                          .Select(p => p.PersonnelId)
                          .ToList();
        }

        /// <summary>
        /// Récupérer les identifiants des personnels qui sont sous la responsabilité d'un manager
        /// </summary>
        /// <param name="managerId">L'identifiant du manager</param>
        /// <returns>La liste des identifiants</returns>
        public List<int> GetManagedEmployeeIdList(int managerId)
        {
            return Context.Personnels
                          .Where(p => p.ManagerId.HasValue && p.ManagerId.Value == managerId)
                          .Select(p => p.PersonnelId)
                          .ToList();
        }

        /// <summary>
        /// Vérifier si un personnel est un ETAM ou IAC
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <returns>Booléan indiquant si le personnel est un ETAM \ IAC</returns>
        public bool IsEtamOrIac(int personnelId)
        {
            return Context.Personnels.Count(p => p.PersonnelId == personnelId && p.Societe.Groupe.Code.Equals("GFES")
                                               && p.Statut != null && (p.Statut.Equals(TypePersonnel.ETAM) || p.Statut.Equals(TypePersonnel.Cadre) ||
                                               p.Statut.Equals(TypePersonnel.ETAMArticle36) || p.Statut.Equals(TypePersonnel.ETAMBureau))) > 0;
        }

        /// <summary>
        /// Get groupe du personnel by personnel identifier
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>Groupe entité</returns>
        public GroupeEnt GetPersonnelGroupebyId(int personnelId)
        {
            return Context.Personnels.Where(x => x.PersonnelId == personnelId).Select(x => x.Societe.Groupe).FirstOrDefault();
        }

        /// <summary>
        /// Récupérer les identifiants des ETAM \ IAC qui sont sous la responsabilité d'un manager
        /// </summary>
        /// <param name="managerId">L'identifiant du manager</param>
        /// <returns>La liste des identifiants</returns>
        public List<int> GetManagedEtamsAndIacs(int managerId)
        {
            return Context.Personnels.AsNoTracking()
                          .Where(p => p.ManagerId.HasValue
                                      && p.ManagerId.Value == managerId
                                      && p.Statut != null
                                      && (p.Statut.Equals(TypePersonnel.ETAM) || p.Statut.Equals(TypePersonnel.Cadre) ||
                                          p.Statut.Equals(TypePersonnel.ETAMArticle36) || p.Statut.Equals(TypePersonnel.ETAMBureau))
                                      && (!p.DateSortie.HasValue || (p.DateSortie.Value.Year >= DateTime.UtcNow.Year && p.DateSortie.Value.Month >= DateTime.UtcNow.Month)) && p.DateSuppression == null)
                          .Select(p => p.PersonnelId)
                          .ToList();
        }

        /// <summary>
        /// Retourne une liste de personnel Fes dernièrement créé ou mise à jour pour l'export vers tibco
        /// </summary>
        /// <param name="byPassDate">booléan qui indique si l'on se base sur la dernière date d'execution ou non</param>
        /// <param name="lastExecutionDate">DateTime dernière date d'execution du flux</param>
        /// <returns>Une liste de personnel fes</returns>
        public IEnumerable<PersonnelEnt> GetPersonnelFesForExportToTibco(bool byPassDate, DateTime? lastExecutionDate)
        {
            return Query()
                    .Include(p => p.Societe.Groupe)
                    .Include(p => p.EtablissementPaie.EtablissementComptable)
                    .Include(p => p.Utilisateur)
                    .Include(p => p.Manager)
                    .Filter(p => p.Societe.Groupe.Code == Constantes.CodeGroupeFES)
                    .Filter(p => p.IsInterne)
                    .Filter(p => byPassDate ||
                                    !lastExecutionDate.HasValue ||
                                    p.DateCreation > lastExecutionDate.Value ||
                                    p.DateModification > lastExecutionDate.Value ||
                                    (p.Utilisateur != null &&
                                        (p.Utilisateur.DateCreation > lastExecutionDate.Value ||
                                            p.Utilisateur.DateModification > lastExecutionDate.Value)
                                    )
                            )
                    .Get()
                    .ToList();
        }

        /// <summary>
        /// Retourne si true si le personnel est manager d'un autre personnel
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>true ou false</returns>
        public bool PersonnelIsManager(int personnelId)
        {
            return Query().Get().Any(p => p.ManagerId == personnelId);
        }

        /// <summary>
        /// Get personnels list by etablissement paie identifiant
        /// </summary>
        /// <param name="etablissementPaieId">Etablissement paie Id</param>
        /// <returns>List des personnels</returns>
        public IEnumerable<PersonnelEnt> GetPersonnelsListByEtabPaieId(int etablissementPaieId)
        {
            return this.Context.Personnels.Where(x => x.EtablissementPaieId.HasValue && x.EtablissementPaieId.Value == etablissementPaieId).ToList();
        }

        /// <summary>
        /// Retourne un personnel en fonction de son identifiant pour l'export Excel de la comparaison de budget.
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel.</param>
        /// <returns>Le personnel.</returns>
        public PersonnelDao GetPersonnelPourExportExcelHeader(int personnelId)
        {
            return Context.Personnels
                .Where(p => p.PersonnelId == personnelId)
                .Select(p => new PersonnelDao
                {
                    Matricule = p.Matricule,
                    Nom = p.Nom,
                    Prenom = p.Prenom,
                    SocieteId = p.SocieteId
                })
                .FirstOrDefault();
        }

        public async Task<List<PersonnelEnt>> GetPersonnelEnteteCommandeAsync(Expression<Func<PersonnelEnt, bool>> predicateForPersonelWithAffectation, Expression<Func<PersonnelEnt, bool>> predicatePersonnelWithHabilitation, int? page = 1, int? pageSize = 20)
        {
            var queryPersonelWithAffectation = Context.Personnels
                            .AsExpandable().Where(predicateForPersonelWithAffectation)
                            .Include(p => p.Societe)
                            .Include(p => p.Ressource)
                            .Include(p => p.EtablissementPaie)
                            .Include(p => p.EtablissementRattachement)
                            .Include(p => p.ContratInterimaires.Select(c => c.ZonesDeTravail));

            var queryPersonnelWithHabilitation = Context.Personnels
                                    .AsExpandable().Where(predicatePersonnelWithHabilitation)
                                    .Include(p => p.Societe)
                                    .Include(p => p.Ressource)
                                    .Include(p => p.EtablissementPaie)
                                    .Include(p => p.EtablissementRattachement)
                                    .Include(p => p.ContratInterimaires.Select(c => c.ZonesDeTravail));


            var result = queryPersonelWithAffectation
                            .Union(queryPersonnelWithHabilitation)
                            .Distinct()
                            .OrderBy(pe => pe.Societe.Code).ThenBy(pe => pe.Matricule)
                            .Skip((page.Value - 1) * pageSize.Value)
                            .Take(pageSize.Value);

            return await result.ToAsyncEnumerable().ToList();
        }

        /// <summary>
        /// Retourne une liste de personnel 
        /// </summary>
        /// <param name="personnelIds">Id des personnels</param>
        /// <returns>List de personnel</returns>
        public List<PersonnelEnt> GetPersonnelByListPersonnelId(List<int?> personnelIds)
        {
            return GetDefaultQuery()
                       .Include(p => p.EtablissementPaie)
                       .Include(p => p.Societe)
                       .Include(p => p.Societe.Groupe)
                       .Filter(p => personnelIds.Contains(p.PersonnelId))
                       .Get()
                       .AsNoTracking().ToList();
        }
        /// <summary>
        /// Cherche un interimaire par matricule externe et groupe code
        /// </summary>
        /// <param name="matriculeExterne">Matricule externe</param>
        /// <param name="groupeCode">Groupe code</param>
        /// <param name="systemeInterimaire">System interimaire (Pixid par exemple)</param>
        /// <returns>Personnel interimaire si existe</returns>
        public PersonnelEnt GetPersonnelInterimaireByExternalMatriculeAndGroupeId(string matriculeExterne, string groupeCode, string systemeInterimaire)
        {
            return this.Context.Personnels.Where(p => p.DateSuppression == null &&
                                                    p.IsInterimaire &&
                                                    p.Societe.Groupe.Code == groupeCode &&
                                                    p.MatriculeExterne.Any(m => m.Matricule == matriculeExterne && m.Source == systemeInterimaire))
                                          .FirstOrDefault();
        }

        /// <summary>
        /// Recupére une liste de personnel appartenant a une société
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <returns>Liste de Personnels</returns>
        public List<PersonnelEnt> GetPersonnelBySociete(int societeId, DateTime datedebut)
        {
            return this.Context.Personnels
                .Include(p => p.Societe)
                .Where(p => p.SocieteId.Equals(societeId) && (!p.DateSortie.HasValue || p.DateSortie.HasValue && p.DateSortie > datedebut))
                .ToList();
        }

        /// <summary>
        /// Recupére une liste de personnel appartenant a une société
        /// </summary>
        /// <param name="societeId">identifiant de la société</param>
        /// <param name="statutPersonnelList">List des personnels statut</param>
        /// <param name="dateDebut">Date debut</param>
        /// <returns>Liste de Personnels</returns>
        public async Task<List<PersonnelEnt>> GetPersonnelBySocieteId(int societeId, IEnumerable<string> statutPersonnelList, DateTime dateDebut)
        {
            return await this.Context.Personnels.Include(p => p.Societe).Where(p => p.SocieteId.Equals(societeId) && statutPersonnelList.Contains(p.Statut)
                                                          && (!p.DateSortie.HasValue || p.DateSortie.HasValue && p.DateSortie.Value.Date > dateDebut.Date))
                             .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Recupére une liste de personnels appartenant a des établissement comptables
        /// </summary>
        /// <param name="etablissementComptablesId">Liste des ids des etablissement Comptables </param>
        /// <returns>Liste de Personnels</returns>
        public List<PersonnelEnt> GetPersonnelByEtablissementComptable(List<int> etablissementComptablesId, DateTime datedebut)
        {
            var listEtablissementPaie = this.Context.EtablissementsPaie
                .Where(e => etablissementComptablesId.Contains(e.EtablissementComptableId.Value))
                .Select(ss => ss.EtablissementPaieId).Distinct()
                .ToList();
            return this.Context.Personnels
                .Where(x => listEtablissementPaie.Contains(x.EtablissementPaieId.Value) && (!x.DateSortie.HasValue || x.DateSortie.HasValue && x.DateSortie > datedebut))
                .ToList();
        }

        /// <summary>
        /// Recupére une liste de personnel appartenant a une List des sociétés
        /// </summary>
        /// <param name="societeIdsList">identifiant de la société</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="statutPersonnelList">List des personnels statut</param>
        /// <returns>Liste de Personnels</returns>
        public async Task<List<PersonnelEnt>> GetPersonnelsBySocieteIdsListAsync(List<int> societeIdsList, DateTime dateDebut, IEnumerable<string> statutPersonnelList)
        {
            if (!societeIdsList.Any()) return new List<PersonnelEnt>();

            return await this.Context.Personnels.Include(p => p.Societe)
                .Where(p => societeIdsList.Contains(p.SocieteId.Value) && statutPersonnelList.Contains(p.Statut) && p.IsInterne && (!p.DateSortie.HasValue || p.DateSortie.HasValue && p.DateSortie.Value.Date > dateDebut.Date))
                .AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Search Personnels With Filters Optimized
        /// </summary>
        /// <param name="filters">Filtre de recherche</param>
        /// <param name="page">Numero de page à récupéré</param>
        /// <param name="pageSize">Nombre d'éléments par page</param>
        /// <param name="splitIsActived">split is activated</param>
        /// <param name="isSuperAdmin">is super admin</param>
        /// <param name="currentUserGroupeId">idntifient du groupe de l'utilisateur courant</param>
        /// <param name="totalCount">total count</param>
        /// <returns>Retourne la condition de recherche du personnel</returns>
        public IEnumerable<PersonnelListResultViewModel> GetPersonnelsByFilterOptimzed(SearchPersonnelEnt filters, int page, int pageSize, bool splitIsActived, bool isSuperAdmin, int? currentUserGroupeId, out int totalCount)
        {
            totalCount = 0;
            var result = Context.Query<PersonnelListResultViewModel>().FromSql("Fred_Picklist_GetPersonnelsByFilter @ValueText = {0}, @ValueTextNom = {1}, @ValueTextPrenom = {2}, @EtablissementPaieCode = {3}, " +
               "@SocieteCode = {4}, @IsActif = {5}, @IsInterne = {6}, @IsUser = {7}, @IsNonPointable = {8}, @IsSuperAdmin = {9}, @CurrentGroupId = {10}, @IsInterimaire = {11}, @WithSplitFilters = {12}, @Page = {13}, @PageSize = {14}"
               , filters.ValueText, filters.ValueTextNom, filters.ValueTextPrenom, filters.EtablissementPaieCode, filters.SocieteCode, filters.IsActif, filters.IsInterne, filters.IsUtilisateur,
               filters.IsPersonnelsNonPointables, isSuperAdmin, currentUserGroupeId, filters.IsInterimaire, splitIsActived, page, pageSize).ToList();

            totalCount = Context.Personnels.Where(filters.GetPredicateWhere(splitIsActived))
                .Where(x => isSuperAdmin || currentUserGroupeId == x.Societe.GroupeId).Count();

            return result;
        }
    }
}
