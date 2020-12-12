using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Extensions;
using Fred.Web.Shared.Extentions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Rapport
{
    /// <summary>
    ///   Référentiel de données pour du rapport
    /// </summary>
    public class RapportRepository : FredRepository<RapportEnt>, IRapportRepository
    {
        private readonly ILogManager logManager;

        public RapportRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Récupère un Rapport en fonction de son Identifiant
        /// </summary>
        /// <param name="rapportId">Identifiant du RapportId</param>
        /// <param name="includes">Includes lors du get</param>
        /// <returns>Un Rapport</returns>
        public RapportEnt Get(int rapportId, List<Expression<Func<RapportEnt, object>>> includes)
        {

            var result = this.Get(null, null, includes, null, null).FirstOrDefault(x => x.RapportId == rapportId);

            if (result != null && result.DateSuppression.HasValue)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        ///   liste rapport
        /// </summary>
        /// <returns>Liste des Rapports.</returns>
        public IQueryable<RapportEnt> GetDefaultQuery()
        {
            return Context.Rapports
              .Include(o => o.RapportStatut)
              .Include(o => o.AuteurCreation)
              .Include(o => o.AuteurModification)
              .Include(o => o.AuteurSuppression)
              .Include(o => o.AuteurCreation.Personnel)
              .Include(o => o.AuteurModification.Personnel)
              .Include(o => o.AuteurSuppression.Personnel)
              .Include(o => o.AuteurVerrou.Personnel)
              .Include(o => o.CI)
              .Include(o => o.CI.Societe)
              .Include(o => o.CI.Societe.Groupe)
              .Include(o => o.CI.Societe.TypeSociete)
              .Include(o => o.CI.Organisation)
              .Include(o => o.ListCommentaires)
              .Include(o => o.ListCommentaires).ThenInclude(oo => oo.Tache)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.Personnel.Societe)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.Personnel.EtablissementPaie)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.Personnel.EtablissementRattachement)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.CodeAbsence)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.CodeDeplacement)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.CodeZoneDeplacement)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.CodeMajoration)
              .Include(o => o.ListLignes).ThenInclude(y => y.ListRapportLigneMajorations).ThenInclude(x => x.CodeMajoration)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.ListRapportLignePrimes).ThenInclude(ooo => ooo.Prime)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.ListRapportLigneTaches).ThenInclude(ooo => ooo.Tache)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.ListRapportLigneAstreintes)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.Materiel.Societe)
              .Include(o => o.ListLignes).ThenInclude(oo => oo.LotPointage)
              .Include(o => o.ValideurCDC)
              .Include(o => o.ValideurCDC.Personnel)
              .Include(o => o.ValideurCDT)
              .Include(o => o.ValideurCDT.Personnel)
              .Include(o => o.ValideurDRC)
              .Include(o => o.ValideurDRC.Personnel)
              .AsNoTracking();
        }

        /// <summary>
        /// Retourne les CI des rapports
        /// </summary>
        /// <returns>Requête des CI des rapports</returns>
        /// <remarks>GROSSE METHODE DE FIX POUR LA MEP. A VIRER !!!!!!!!!!!!! </remarks>
        public IQueryable<RapportEnt> GetRapportsCis()
        {
            return Context.Rapports.Include(o => o.CI).AsNoTracking();
        }

        /// <summary>
        ///   Recuperation des lignes de Rapport
        /// </summary>
        /// <returns>Renvoie les lignes du rapport</returns>
        public IEnumerable<RapportLigneEnt> GetRapportLigneAll()
        {
            foreach (RapportLigneEnt rapportligne in Context.RapportLignes)
            {
                yield return rapportligne;
            }
        }

        /// <summary>
        ///   Recupère le commentaire d'un rapport pour un tache particulière
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport</param>
        /// <param name="tacheId">Identifiant de la tache</param>
        /// <returns>Retourne le commentaire pour un rapport et une tache</returns>
        public string GetCommentairesByRapportIdAndTacheId(int rapportId, int tacheId)
        {
            try
            {
                string chaine = Context.RapportTache.Where(rt => rt.RapportId == rapportId && rt.TacheId == tacheId).Select(rt => rt.Commentaire).FirstOrDefault();
                if (string.IsNullOrEmpty(chaine))
                {
                    return string.Empty;
                }
                else
                {
                    return chaine;
                }
            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

        }

        /// <summary>
        ///   Permet de détacher les entités dépendantes des rapport pour éviter de les prendre en compte dans la sauvegarde du
        ///   contexte.
        /// </summary>
        /// <param name="rapport">Rapport dont les dépendances sont à détacher</param>
        /// <param name="vidageLigne">
        ///   booléen indiquant si la dépendances des lignes doit être supprimer /!\ à corriger lors du
        ///   refacto EF
        /// </param>
        public void AttachDependancies(RapportEnt rapport, bool vidageLigne = false)
        {
            rapport.RapportStatut = null;
            rapport.CI = null;
            rapport.AuteurCreation = null;
            rapport.AuteurModification = null;
            rapport.AuteurSuppression = null;
            rapport.ValideurCDC = null;
            rapport.ValideurCDT = null;
            if (vidageLigne)
            {
                rapport.ListLignes = null;
            }
        }

        /// <summary>
        ///   liste rapport
        /// </summary>
        /// <returns>Liste complète des Rapports.</returns>
        public IQueryable<RapportEnt> GetAll()
        {
            return Context.Rapports;
        }

        /// <summary>
        ///   liste rapport pour la synchronisation mobile.
        /// </summary>
        /// <returns>Liste complète des Rapports.</returns>
        public IQueryable<RapportEnt> GetAllSync()
        {
            return Context.Rapports.AsNoTracking();
        }

        /// <summary>
        ///   Retourne une liste de rapports
        /// </summary>
        /// <returns>liste rapport</returns>
        public IQueryable<RapportEnt> GetLightQuery()
        {
            return Context.Rapports.Include(r => r.CI.Organisation).OrderBy(r => r.RapportId).AsQueryable();
        }

        /// <summary>
        ///   Retourne la liste des rapports.
        /// </summary>
        /// <returns>Une liste de rapport</returns>
        public IEnumerable<RapportEnt> GetRapportList()
        {
            foreach (RapportEnt rapport in GetDefaultQuery())
            {
                UpdateRapportLignes(rapport);
                yield return rapport;
            }
        }

        /// <summary>
        ///   Recherche la liste des rapports correspondants aux prédicats
        /// </summary>
        /// <param name="predicateWhere">expression lambda contenant les critères de recherche</param>
        /// <param name="etablissementPaieIdList">list d'identifiant unique d'établissement de paie</param>
        /// <param name="sortFilter">Chaine de caractère conteneant l'expression de tri</param>
        /// <param name="totalCount">le total de rapports de la requête.</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>IEnumerable contenant les rapports correspondants aux critères de recherche</returns>
        public IEnumerable<RapportEnt> SearchRapportWithFilter(Expression<Func<RapportEnt, bool>> predicateWhere, List<int?> etablissementPaieIdList, string sortFilter, out int totalCount, int? page = 1, int? pageSize = 20)
        {
            var query = Context.Rapports.Where(predicateWhere);
            query = query.Include(o => o.ListLignes).ThenInclude(l => l.Personnel);
            if (etablissementPaieIdList.Count > 0)
            {
                query = query.Where(o => o.ListLignes.Any(l => etablissementPaieIdList.Contains(l.Personnel.EtablissementPaieId)));
            }
            totalCount = query.Count();

            query = query.Include(a => a.RapportStatut)
            .Include(o => o.CI)
            .Include(o => o.AuteurCreation.Personnel)
            .Include(o => o.ValideurCDC.Personnel)
            .Include(o => o.ValideurCDT.Personnel)
            .Include(o => o.AuteurModification.Personnel)
            .Include(o => o.AuteurSuppression.Personnel)
            .Include(o => o.AuteurVerrou.Personnel);

            query = query.OrderBy(sortFilter);

            if (page.HasValue && pageSize.HasValue)
            {
                query = query
                  .Skip((page.Value - 1) * pageSize.Value)
                  .Take(pageSize.Value);
            }

            return query.ToList();
        }

        /// <summary>
        ///   Retourne le rapport portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport à retrouver.</param>
        /// <returns>le rapport retrouvée, sinon nulle.</returns>
        public RapportEnt GetRapportById(int rapportId)
        {
            var rapport = GetDefaultQuery()
                        .Include(r => r.ListLignes).ThenInclude(l => l.Materiel)
                        .SingleOrDefault(c => c.RapportId.Equals(rapportId));
            UpdateRapportLignes(rapport);
            return rapport;
        }

        /// <summary>
        /// Récupération de liste des rapports en fonction d'une liste d'identifiants de rapport
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapport</param>
        /// <returns>Liste de rapport</returns>
        public IEnumerable<RapportEnt> GetRapportListWithRapportLignesNoTracking(IEnumerable<int> rapportIds)
        {
            return Context.Rapports
                .Include(r => r.ListLignes)
                .Where(r => rapportIds.Contains(r.RapportId))
                .AsNoTracking();
        }

        /// <summary>
        ///   Supprime de ligne de Rapport en fonction de leur existence
        /// </summary>
        /// <param name="rl"> Lignes de Rapports</param>
        public void DeleteRapportLigne(RapportLigneEnt rl)
        {
            if (rl == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.RapportLignes.Remove(rl);
        }

        /// <summary>
        ///   Supprime la liaison entre un rapport et une tache
        /// </summary>
        /// <param name="rt"> RapportTache</param>
        public void DeleteRapportTache(RapportTacheEnt rt)
        {
            if (rt == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.RapportTache.Remove(rt);
        }

        /// <summary>
        ///   Ajoute une ligne de Prime
        /// </summary>
        /// <param name="rlp"> Lignes de Prime</param>
        public void AddRapportLignePrime(RapportLignePrimeEnt rlp)
        {
            Context.RapportLignePrimes.Add(rlp);
        }

        /// <summary>
        ///   Update une ligne de Prime
        /// </summary>
        /// <param name="rlp"> Lignes de Prime</param>
        public void UpdateRapportLignePrime(RapportLignePrimeEnt rlp)
        {
            try
            {
                Context.Entry(rlp).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Ajoute une ligne de Tache
        /// </summary>
        /// <param name="rlt"> Lignes de Tache</param>
        public void AddRapportLigneTache(RapportLigneTacheEnt rlt)
        {
            Context.RapportLigneTaches.Add(rlt);
        }

        /// <summary>
        ///   Update une ligne de Tache
        /// </summary>
        /// <param name="rlt"> Lignes de Prime</param>
        public void UpdateRapportLigneTache(RapportLigneTacheEnt rlt)
        {
            Context.Entry(rlt).State = EntityState.Modified;
        }

        /// <summary>
        ///   Retourne le statut du rapport en fonction du code statut
        /// </summary>
        /// <param name="statutCode">Le code du statut</param>
        /// <returns>Un statut de rapport</returns>
        public RapportStatutEnt GetRapportStatutByCode(string statutCode)
        {
            return Context.RapportStatuts.AsNoTracking().FirstOrDefault(o => o.Code.Equals(statutCode));
        }

        /// <summary>
        ///   Retourne la liste des statuts d'un rapport.
        /// </summary>
        /// <param name="forMobile">Si la liste est destinée au mobile</param>
        /// <returns>Renvoie la liste des statuts d'un rapport.</returns>
        public IQueryable<RapportStatutEnt> GetRapportStatutList(bool forMobile)
        {
            IQueryable<RapportStatutEnt> query = Context.RapportStatuts;
            return query;
        }

        /// <inheritdoc />
        public IEnumerable<RapportEnt> GetRapportsMobile(DateTime? sinceDate = null)
        {
            return Context.Rapports.Where(x => sinceDate == null || x.DateModification > sinceDate)
              .Include(x => x.ListLignes)
              .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneTaches)
              .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLignePrimes);
        }

        /// <summary>
        /// Permet d'extraire une liste de rapports pour l'exportation
        /// </summary>
        /// <param name="filterRapport">filtres des rapports</param>
        /// <returns>liste de rapports</returns>
        public IEnumerable<RapportEnt> GetRapportsExportApi(FilterRapportFesExport filterRapport)
        {
            return this.Context.Rapports
              .Include(r => r.CI)
              .Include(r => r.CI.Societe)
              .Include(r => r.CI.EtablissementComptable)
              .Include(r => r.ListLignes)
              .Include(r => r.ListLignes).ThenInclude(l => l.Personnel)
              .Include(r => r.ListLignes).ThenInclude(l => l.Materiel)
              .Include(r => r.ListLignes).ThenInclude(l => l.CodeAbsence)
              .Include(r => r.ListLignes).ThenInclude(l => l.ListRapportLignePrimes)
              .Include(r => r.ListLignes).ThenInclude(l => l.ListRapportLigneTaches)
              .Include(r => r.ListLignes).ThenInclude(l => l.ListRapportLigneTaches).ThenInclude(lt => lt.Tache.RessourceTaches).ThenInclude(rt => rt.Ressource)
              .Where(r =>
                      r.DateCreation >= filterRapport.DateDebut
                      && r.DateCreation <= filterRapport.DateFin
                      && r.CI.Societe.CodeSocieteComptable == filterRapport.CodeSociete)
              .AsNoTracking()
              .ToList();
        }

        /// <summary>
        /// Permet de récupérer la liste des sorties astreintes associées à une ligne de rapport
        /// </summary>
        /// <param name="rapportLigneId">L'identifiant de la ligne du rapport</param>
        /// <returns>Liste des sorties astreintes</returns>
        public IEnumerable<RapportLigneAstreinteEnt> GetRapportLigneAstreintes(int rapportLigneId)
        {
            return Context.RapportLigneAstreintes.Where(l => l.RapportLigneId == rapportLigneId).AsNoTracking();
        }

        /// <summary>
        /// Permet de suprimer une sortie astreinte
        /// </summary>
        /// <param name="rapportLigneAstreinte">La sortie astreinte</param>
        public void DeleteRapportLigneAstreinte(RapportLigneAstreinteEnt rapportLigneAstreinte)
        {
            if (rapportLigneAstreinte != null)
            {
                Context.Entry(rapportLigneAstreinte).State = EntityState.Deleted;
            }
        }

        /// <summary>
        /// Permet de suprimer une liste des sorties astreintes
        /// </summary>
        /// <param name="rapportLigneAstreintes">La liste des sorties astreintes</param>
        public void DeleteRapportLigneAstreintes(IEnumerable<RapportLigneAstreinteEnt> rapportLigneAstreintes)
        {
            if (rapportLigneAstreintes != null && rapportLigneAstreintes.Any())
            {
                foreach (RapportLigneAstreinteEnt rapportLigneAstreinte in rapportLigneAstreintes)
                {
                    Context.Entry(rapportLigneAstreinte).State = EntityState.Deleted;
                }
            }
        }

        /// <summary>
        /// Permet de mettre à jour une sortie astreinte associée à une ligne de rapport
        /// </summary>
        /// <param name="rapportLigneAstreinte">La sortie astreinte</param>
        public void UpdateRapportLigneAstreinte(RapportLigneAstreinteEnt rapportLigneAstreinte)
        {
            Context.Entry(rapportLigneAstreinte).State = EntityState.Modified;
        }

        /// <summary>
        /// Ajouter une sortie astreinte
        /// </summary>
        /// <param name="rapportLigneAstreinte">La sortie astreinte</param>
        public void AddRapportLigneAstreinte(RapportLigneAstreinteEnt rapportLigneAstreinte)
        {
            Context.RapportLigneAstreintes.Add(rapportLigneAstreinte);
        }

        /// <summary>
        /// Supprimer la liste des sorties astreintes pour une ligne de rapport
        /// </summary>
        /// <param name="rapportLigneId">L'identifiant de la ligne de rapport</param>
        public void DeleteRapportLigneAstreintes(int rapportLigneId)
        {
            IEnumerable<RapportLigneAstreinteEnt> rapportLigneAstreintes = this.Context.RapportLigneAstreintes.Where(a => a.RapportLigneId == rapportLigneId);
            Context.RapportLigneAstreintes.RemoveRange(rapportLigneAstreintes);
        }

        /// <summary>
        /// Get list rapport journalier pour le rapport hebdo
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="personnelsIds">List des personnel ids</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>List des rapports</returns>
        public List<RapportEnt> GetCiRapportHebdomadaire(int ciId, IEnumerable<int> personnelsIds, DateTime dateDebut, DateTime dateFin)
        {
            var firstQyery = Context.Rapports.Where(x => x.CiId == ciId && x.DateChantier >= dateDebut && x.DateChantier <= dateFin && x.DateSuppression == null);
            firstQyery.Include(x => x.ListLignes).Load();
            firstQyery = firstQyery.Where(y => y.ListLignes.Any(z => z.PersonnelId.HasValue && personnelsIds.Contains(z.PersonnelId.Value) && !z.DateSuppression.HasValue));
            firstQyery.Include(x => x.CI).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(o => o.CodeAbsence).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneAstreintes).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneMajorations).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneMajorations).ThenInclude(z => z.CodeMajoration).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLignePrimes).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLignePrimes).ThenInclude(z => z.Prime).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneTaches).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneTaches).ThenInclude(o => o.Tache).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.Personnel).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.Personnel.Societe).Load();

            List<RapportEnt> rapportList = firstQyery.OrderBy(x => x.DateChantier).GroupBy(x => x.DateChantier).Select(grp => grp.FirstOrDefault()).ToList();
            foreach (var rapport in rapportList)
            {
                rapport.ListLignes = rapport.ListLignes.Where(x => !x.DateSuppression.HasValue && x.PersonnelId.HasValue && personnelsIds.Contains(x.PersonnelId.Value)).ToList();
            }

            return rapportList;
        }

        /// <summary>
        /// Get list rapport journalier pour le rapport hebdo
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>List des rapports</returns>
        public List<RapportEnt> GetCiRapportHebdomadaire(int ciId, DateTime dateDebut, DateTime dateFin)
        {
            return Context.Rapports
                .Where(x => x.CiId == ciId && x.DateChantier >= dateDebut && x.DateChantier <= dateFin && x.DateSuppression == null)
                .Include(x => x.CI)
                .Include(x => x.ListLignes).ThenInclude(s => s.RapportLigneStatut)
                .Include(x => x.ListLignes).ThenInclude(o => o.CodeAbsence)
                .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneAstreintes)
                .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneMajorations).ThenInclude(z => z.CodeMajoration)
                .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLignePrimes).ThenInclude(z => z.Prime)
                .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneTaches).ThenInclude(o => o.Tache)
                .Include(x => x.ListLignes).ThenInclude(y => y.Personnel.Societe)
                .GroupBy(x => x.DateChantier)
                .Select(x => x.FirstOrDefault())
                .OrderBy(x => x.DateChantier)
            .ToList();
        }


        /// <summary>
        /// Get list rapport journalier pour le rapport hebdo
        /// </summary>
        /// <param name="ciIds">Ci identifiers</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <param name="dateFin">Date fin</param>
        /// <param name="statut">Statut Personnel</param>
        /// <returns>List des rapports</returns>
        public Dictionary<int, List<RapportEnt>> GetCiRapportHebdomadaire(IEnumerable<int> ciIds, DateTime dateDebut, DateTime dateFin, int statut)
        {
            var all = Context.Rapports
                .Where(x => ciIds.Contains(x.CiId) && x.DateChantier >= dateDebut && x.DateChantier <= dateFin && x.DateSuppression == null && x.TypeStatutRapport == statut)
                .Include(x => x.CI)
                .Include(x => x.ListLignes).Where(rl => rl.DateSuppression == null)
                .Include(x => x.ListLignes).ThenInclude(o => o.CodeAbsence)
                .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneAstreintes)
                .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneMajorations)
                .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneMajorations).ThenInclude(z => z.CodeMajoration)
                .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLignePrimes)
                .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLignePrimes).ThenInclude(z => z.Prime)
                .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneTaches)
                .Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneTaches).ThenInclude(o => o.Tache)
                .Include(x => x.ListLignes).ThenInclude(y => y.Personnel)
                .Include(x => x.ListLignes).ThenInclude(y => y.Personnel.Societe)
                .GroupBy(x => new { x.CiId, x.DateChantier })
                .Select(x => x.FirstOrDefault())
                .ToList();

            var groupes = all.GroupBy(x => x.CiId);
            var ret = new Dictionary<int, List<RapportEnt>>();
            foreach (var groupe in groupes)
            {
                var rapports = groupe.OrderBy(x => x.DateChantier).ToList();
                ret.Add(groupe.Key, rapports);
            }

            return ret;
        }

        /// <summary>
        /// Get list rapport journalier pour le rapport hebdo
        /// </summary>
        /// <param name="ciIds">Ci identifiers</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <param name="dateFin">Date fin</param>
        /// <param name="statut">Statut du Personnel</param>
        /// <returns>List des rapports</returns>
        public Dictionary<int, List<RapportEnt>> GetCiRapportHebdomadaireEmployee(IEnumerable<int> ciIds, int personnelId, DateTime dateDebut, DateTime dateFin)
        {
            var firstQyery = Context.Rapports.Where(x => ciIds.Contains(x.CiId) && x.DateChantier >= dateDebut && x.DateChantier <= dateFin && x.DateSuppression == null);
            firstQyery.Include(x => x.ListLignes).Load();
            firstQyery = firstQyery.Where(y => y.ListLignes.Any(z => z.PersonnelId.HasValue && z.PersonnelId.Value == personnelId && !z.DateSuppression.HasValue));
            firstQyery.Include(x => x.CI).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(o => o.CodeAbsence).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneAstreintes).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneMajorations).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneMajorations).ThenInclude(z => z.CodeMajoration).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLignePrimes).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLignePrimes).ThenInclude(z => z.Prime).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneTaches).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneTaches).ThenInclude(o => o.Tache).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.Personnel).Load();
            firstQyery.Include(x => x.ListLignes).ThenInclude(y => y.Personnel.Societe).Load();

            var all = firstQyery.GroupBy(x => x.CiId);

            var ret = new Dictionary<int, List<RapportEnt>>();

            foreach (var groupe in all)
            {
                var rapports = groupe.OrderBy(x => x.DateChantier).GroupBy(x => x.DateChantier).Select(grp => grp.FirstOrDefault()).ToList();
                ret.Add(groupe.Key, rapports);
            }

            return ret;
        }

        /// <summary>
        /// Récuperer la liste des sorties astreintes pour un rapport
        /// </summary>
        /// <param name="rapportId">L'identifiant du rapport</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="ciId">L'identifiant du CI</param>
        /// <param name="datePointage">Date de pointage</param>
        /// <returns>La liste des sorties des astreintes</returns>
        public List<RapportLigneAstreinteEnt> GetRapportLigneAstreintes(int rapportId, int personnelId, int ciId, DateTime datePointage)
        {
            return this.Context.RapportLigneAstreintes
              .Where(a => a.RapportLigne.RapportId == rapportId && a.RapportLigne.PersonnelId == personnelId && a.RapportLigne.CiId == ciId && a.RapportLigne.DatePointage == datePointage && a.RapportLigne.DateSuppression == null)
              .AsNoTracking()
              .ToList();
        }

        /// <summary>
        /// Check rapport pour la societe FES
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="dateChantier">Date de chantier</param>
        /// <returns>List des rapports journaliers</returns>
        public RapportEnt CheckRepportsForFES(int ciId, DateTime dateChantier)
        {
            return this.Context.Rapports.FirstOrDefault(x => x.CiId == ciId && x.DateChantier.Equals(dateChantier) && x.CI != null && x.CI.Societe != null && x.CI.Societe.Groupe != null && x.CI.Societe.Groupe.Code.Equals("GFES"));
        }

        /// <summary>
        /// Check if a rapport exist instead of create new one 
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="dateChantier">Date chantier</param>
        /// <returns>Rapport identifier</returns>
        public int CheckRapportExistance(int ciId, DateTime dateChantier)
        {
            RapportEnt rapport = this.Context.Rapports.FirstOrDefault(x => x.CiId == ciId && x.DateChantier == dateChantier && !x.DateSuppression.HasValue);
            return rapport != null ? rapport.RapportId : 0;
        }

        /// <summary>
        /// Retourne liste de rapport ligne par Ci après une date de cloture étant vérouiller et contenant des intérimaires
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <param name="statut">Statut du ci</param>
        /// <returns>Liste de rapport ligne</returns>
        public IEnumerable<RapportLigneEnt> GetRapportLigneVerrouillerByCiIdForReceptionInterimaire(int ciId, int statut)
        {
            return this.Context.RapportLignes
              .Include(rl => rl.Personnel)
              .Include(rl => rl.Ci.Taches)
              .Where(rl => rl.CiId == ciId && rl.Rapport.RapportStatutId == statut && rl.Personnel.IsInterimaire)
              .Where(rl => !rl.ReceptionInterimaire)
              .Where(rl => rl.DateSuppression == null)
              .AsNoTracking()
              .ToList();
        }

        /// <summary>
        /// Filtre une liste des Identifiant Ci après une date de cloture étant vérouiller et contenant des intérimaires
        /// </summary>
        /// <param name="ciIds">Liste d'Identifiant unique d'un CI</param>
        /// <returns>Liste d'identifiant ci</returns>
        public IEnumerable<int> GetCiIdsAvailablesForReceptionInterimaire(IEnumerable<int> ciIds)
        {
            return this.Context.RapportLignes
              .Include(rl => rl.Personnel)
              .Where(rl => ciIds.Contains(rl.CiId) && RapportStatutEnt.RapportStatutVerrouille.Key.Equals(rl.Rapport.RapportStatutId) && rl.Personnel.IsInterimaire)
              .Where(rl => !rl.ReceptionInterimaire)
              .Where(rl => rl.DateSuppression == null)
              .AsNoTracking()
              .Select(rl => rl.CiId).Distinct();
        }

        /// <summary>
        /// Retourne liste de rapport ligne par Ci après une date de cloture étant vérouiller et contenant des Materiels externe
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <param name="statut">Statut du ci</param>
        /// <returns>Liste de rapport ligne</returns>
        public IEnumerable<RapportLigneEnt> GetRapportLigneVerrouillerByCiIdForReceptionMaterielExterne(int ciId, int statut)
        {
            return this.Context.RapportLignes
              .Include(rl => rl.Materiel)
              .Include(rl => rl.Ci.Taches)
              .Where(rl => rl.CiId == ciId && rl.Rapport.RapportStatutId == statut && rl.Materiel.MaterielLocation)
              .Where(rl => !rl.ReceptionMaterielExterne)
              .AsNoTracking()
              .ToList();
        }

        /// <summary>
        /// Récupérer le total des heures normales pointées d'un personnel (travail sans majorations et absences) sur toutes les affaires
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <returns>Nombre des heures</returns>
        public double GetTotalHoursWithoutMajorations(int personnelId, DateTime datePointage)
        {
            double result = 0;
            var query = Context.RapportLignes
                    .Where(l => l.PersonnelId == personnelId && l.DatePointage == datePointage && l.DateSuppression == null);

            if (query.Count() > 0)
            {
                return query.Sum(e => e.HeureAbsence
                              + (e.ListRapportLigneTaches.Any() ? e.ListRapportLigneTaches.Sum(t => t.HeureTache) : 0)
                              - (e.ListRapportLigneMajorations.Any() ? e.ListRapportLigneMajorations.Sum(m => m.HeureMajoration) : 0)
                          );

            }
            return result;
        }

        /// <summary>
        /// Récupérer le total des heures pointées d'un personnel (travail et absences et majoration de nuit TNH1 et TNH2) sur toutes les affaires
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <returns>Nombre des heures</returns>
        public double GetTotalHoursWorkAndAbsence(int personnelId, DateTime datePointage)
        {
            double result = 0;
            var query = Context.RapportLignes
              .Where(l => l.PersonnelId == personnelId && l.DatePointage == datePointage && l.DateSuppression == null);
            if (query.Count() > 0)
            {
                return query.Sum(e => e.HeureAbsence
                        + (e.ListRapportLigneTaches.Any() ? e.ListRapportLigneTaches.Sum(t => t.HeureTache) : 0)
                        + (e.ListRapportLigneMajorations.Any(m => m.CodeMajoration.IsHeureNuit)
                            ?
                            e.ListRapportLigneMajorations.Where(m => m.CodeMajoration.IsHeureNuit).Sum(m => m.HeureMajoration)
                            : 0)
                    );
            }
            return result;
        }

        /// <summary>
        /// Get total hours work and absence and majoration for validation
        /// </summary>
        /// <param name="personnelId">Personneel identifier</param>
        /// <param name="datePointage">Date du chantier</param>
        /// <returns>Total des heures</returns>
        public double GetTotalHoursWorkAndAbsenceWithMajoration(int personnelId, DateTime datePointage)
        {
            return Context.RapportLignes.Where(l => l.PersonnelId == personnelId && l.DatePointage == datePointage && l.DateSuppression == null).Sum(x => x.HeureTotalTravail + x.HeureAbsence);
        }

        /// <summary>
        /// Met à jour le CI des lignes du rapport avec le CI du rapport.
        /// </summary>
        /// <param name="rapport">Le rapport concerné.</param>
        private void UpdateRapportLignes(RapportEnt rapport)
        {
            if (rapport != null && rapport.CI.Organisation != null)
            {
                rapport.CI.Organisation.CI = null;
                rapport.ListLignes.ForEach(ll => ll.Ci = rapport.CI);
            }
        }

        /// <summary>
        /// Ajout en masse de rapports
        /// </summary>
        /// <param name="rapportList">La liste des rappports à ajouter</param>
        public void AddRangeRapportList(IEnumerable<RapportEnt> rapportList)
        {
            if (!rapportList.IsNullOrEmpty())
            {
                Context.Rapports.AddRange(rapportList);
            }
        }

        /// <summary>
        /// Renvoie la liste des rapport entre 2 dates . Les rapports retournés conçernent les ci envoyés
        /// </summary>
        /// <param name="ciList">Liste des Cis</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>List de rapports en les 2 dates</returns>
        public IEnumerable<RapportEnt> GetRapportListBetweenDatesByCiList(IEnumerable<int> ciList, DateTime startDate, DateTime endDate)
        {
            if (ciList.IsNullOrEmpty())
            {
                return new List<RapportEnt>();
            }

            return Query().Filter(r => ciList.Contains(r.CiId)
                                    && !r.DateSuppression.HasValue
                                    && r.DateChantier >= startDate
                                    && r.DateChantier <= endDate)
                            .Get().ToList();
        }

        /// <summary>
        /// Retourne un rapport journalier en fonction d'un CiId et d'une date
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="date">Date du chantier</param>
        /// <returns>Un rapport journalier en fonction d'un CiId et d'une date</returns>
        public RapportEnt GetRapportByCiIdAndDate(int ciId, DateTime date, int? statutRapport = null)
        {
            if (statutRapport.HasValue)
            {
                return Context.Rapports.Include(r => r.ListLignes).FirstOrDefault(r => r.CiId == ciId
                                                                                    && r.DateChantier == date
                                                                                    && r.TypeStatutRapport.Equals(statutRapport)
                                                                                    && !r.DateSuppression.HasValue);
            }
            return Context.Rapports.Include(r => r.ListLignes).FirstOrDefault(r => r.CiId == ciId && r.DateChantier == date && !r.DateSuppression.HasValue);
        }

        /// <summary>
        /// Renvoie la liste des rapport entre 2 dates . Les rapports retournés conçernent les ci envoyés
        /// </summary>
        /// <param name="ciList">Liste des Cis</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>List de rapports en les 2 dates</returns>
        public IEnumerable<RapportEnt> GetRapportListWithRapportLignesBetweenDatesByCiList(IEnumerable<int> ciList, DateTime startDate, DateTime endDate)
        {
            if (ciList.IsNullOrEmpty())
            {
                return new List<RapportEnt>();
            }

            var query = Context.Rapports.Where(r => ciList.Contains(r.CiId) && r.DateChantier >= startDate && r.DateChantier <= endDate && !r.DateSuppression.HasValue);
            query.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneTaches).Load();
            query.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneAstreintes).Load();
            query.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneMajorations).Load();
            query.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLignePrimes).Load();
            return query;
        }

        /// <summary>
        /// Retourne un rapportEnt en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">liste des ci CI</param>
        /// <param name="date">La date du pointage</param>
        /// <param name="personnelId">personnelId</param>
        /// <returns>rapportEnt </returns>
        public RapportEnt GetRapportByPersonnelIdAndDatePointagesFiggo(int ciId, DateTime date, int personnelId, int typePersonnel)
        {
            var query = Context.Rapports.Where(r => r.CiId.Equals(ciId) && r.DateChantier == date && !r.DateSuppression.HasValue && r.TypeStatutRapport.Equals(typePersonnel));
            query.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneTaches).Load();
            query.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneMajorations).Load();
            query.Include(x => x.ListLignes).ThenInclude(y => y.CodeMajoration).Load();
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Retourne un rapportEnt en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">liste des ci CI</param>
        /// <param name="date">La date du pointage</param>
        /// <param name="personnelId">personnelId</param>
        /// <returns>rapportEnt </returns>
        public List<RapportEnt> GetAllRapportByPersonnelIdAndDatePointagesFiggo(List<int> ciId, DateTime date, int personnelId, int typePersonnel)
        {
            var query = Context.Rapports.Where(r => ciId.Contains(r.CiId) && r.DateChantier == date && !r.DateSuppression.HasValue && r.TypeStatutRapport == typePersonnel);
            query.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneTaches).Load();
            query.Include(x => x.ListLignes).ThenInclude(y => y.ListRapportLigneMajorations).Load();
            return query.ToList();
        }

        /// <summary>
        /// Récupere une liste de rapportLigne définit par sa date et un personnelId et un ciId
        /// </summary>
        /// <param name="date">date du pointage</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="ciId">identifiant du ci</param>
        /// <returns>List des rapportLigne</returns>
        public List<RapportLigneEnt> GetRapportLigneByDateAndPersonnelAndCi(DateTime date, int personnelId, int ciId)
        {
            return Context.RapportLignes.Where(d => d.DatePointage == date
                                                && !d.DateSuppression.HasValue
                                                && d.PersonnelId == personnelId
                                                && d.CiId == ciId).ToList();
        }

        /// <summary>
        /// Récupere une liste de rapportLigne définit par sa date et un personnelId
        /// </summary>
        /// <param name="ciId">identifiant du ci</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="date">date pointage</param>
        /// <returns>List des rapportLigne</returns>
        public List<RapportLigneEnt> GetRapportLigneByCiAndPersonnel(int ciId, int personnelId, DateTime date)
        {
            return Context.RapportLignes.Where(d => d.CiId.Equals(ciId)
                                                && !d.DateSuppression.HasValue
                                                && d.PersonnelId == personnelId
                                                && d.DatePointage == date).ToList();
        }

        /// <summary>
        /// Retourne  un pointage réels en fonction d'un ci et d'une date
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="datePointage">La date du pointage</param>
        /// <returns>Une liste de pointages </returns>
        public RapportEnt GetRapportsByCilIdAndDatePointagesFiggo(int ciId, DateTime datePointage, int typePersonnel)
        {
            return Context.Rapports.Where(r => r.CiId.Equals(ciId)
            && r.DateChantier == datePointage
            && r.TypeStatutRapport.Equals(typePersonnel)
            && !r.DateSuppression.HasValue)
                .FirstOrDefault();
        }

        /// <summary>
        /// Recupere liste des rapports by List des Cis
        /// </summary>
        /// <param name="ciList">List des Cis Ids</param>
        /// <returns></returns>
        public List<RapportEnt> GetRapportsListbyCiList(List<int> ciList)
        {
            return Context.Rapports.Where(r => ciList.Contains(r.CiId) && !r.DateSuppression.HasValue).ToList();
        }

        /// <summary>
        /// Get Rapport ligne list for validation per affaire
        /// </summary>
        /// <param name="ciList">List Ci Ids</param>
        /// <param name="personnelIds">List personnel Ids</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="dateFin">Date Fin</param>
        /// <returns>Task</returns>
        public async Task<IEnumerable<RapportLigneEnt>> GetRapportsLignesValidationAffairesByResponsableAsync(IEnumerable<int> ciList, IEnumerable<int> personnelIds, DateTime dateDebut, DateTime dateFin)
        {
            return await Context.RapportLignes.Where(r => ciList.Contains(r.CiId) && personnelIds.Contains(r.PersonnelId.Value) && r.DatePointage >= dateDebut && r.DatePointage <= dateFin && !r.DateSuppression.HasValue)
            .Include(y => y.RapportLigneStatut)
            .Include(y => y.ListRapportLigneTaches)
            .Include(y => y.ListRapportLigneAstreintes)
            .Include(y => y.ListRapportLigneMajorations).ThenInclude(x => x.CodeMajoration)
            .Include(y => y.ListRapportLignePrimes).ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get Rapport For Verrouillage
        /// </summary>
        /// <param name="rapportIds">list des Id de rapport</param>
        /// <returns>List de rapports</returns>
        public List<RapportEnt> GetRapportToLock(IEnumerable<int> rapportIds)
        {
            var query = Context.Rapports.Where(r => rapportIds.Contains(r.RapportId)
                                                && r.RapportStatutId != RapportStatutEnt.RapportStatutVerrouille.Key
                                                && !r.DateSuppression.HasValue);
            query.Include(o => o.RapportStatut);
            query.Include(o => o.ListLignes).ThenInclude(l => l.Materiel).Load();
            query.Include(o => o.ListLignes).ThenInclude(l => l.Personnel).Load();
            return query.ToList();
        }

        public IReadOnlyList<RapportLigneEnt> GetAllRapportLigneBasedOnPersonnelAffectation(int personnelId, DateTime datePointage)
        {
            return Context.RapportLignes
                 .Include(a => a.AffectationMoyen)
                 .Include(t => t.ListRapportLigneTaches)
                 .Where(x => x.AffectationMoyen.PersonnelId == personnelId && x.DatePointage == datePointage)
                 .AsNoTracking()
                 .ToList();
        }
    }
}
