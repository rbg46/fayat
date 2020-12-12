using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ValidationPointage;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.ValidationPointage
{
    /// <summary>
    ///   Classe LotPointageRepository
    /// </summary>
    public class LotPointageRepository : FredRepository<LotPointageEnt>, ILotPointageRepository
    {
        /// <summary>
        ///   Constructeur <seealso cref="LotPointageRepository"/>
        /// </summary>
        /// <param name="logMgr">Gestionnaire des logs</param>
        /// <param name="uow">Unit of work</param>
        public LotPointageRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <inheritdoc />
        public LotPointageEnt AddLotPointage(LotPointageEnt lotPointage, int utilisateurId)
        {
            Insert(lotPointage);
            Context.SaveChangesWithAudit(utilisateurId);
            return lotPointage;
        }

        /// <inheritdoc />
        public void DeleteLotPointage(int lotPointageId)
        {
            DeleteById(lotPointageId);
        }

        /// <inheritdoc />
        public LotPointageEnt Get(int lotPointageId)
        {
            LotPointageEnt ret = GetLotPointageById(lotPointageId);
            if (ret != null)
            {
                ret.RapportLignes = ret.RapportLignes.Where(rl => !rl.DateSuppression.HasValue).ToList();
            }
            return ret;
        }

        public async Task<LotPointageEnt> FindByIdNotTrackedAsync(int id)
        {
            return await Context.LotPointages.AsNoTracking().FirstOrDefaultAsync(t => t.LotPointageId == id);
        }

        /// <summary>
        ///   Récupère un Lot de Pointage en fonction de son Identifiant
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>
        /// <param name="includes">Includes lors du get</param>
        /// <returns>Lot de pointage</returns>
        public LotPointageEnt Get(int lotPointageId, List<Expression<Func<LotPointageEnt, object>>> includes)
        {

            var ret = this.Get(null, null, includes, null, null).AsNoTracking().FirstOrDefault(x => x.LotPointageId == lotPointageId);
            if (ret != null)
            {
                DateTime periode = ret.Periode;
                ret.RapportLignes = ret.RapportLignes.Where(rl => !rl.DateSuppression.HasValue && (rl.Personnel == null || !rl.Personnel.DateSortie.HasValue ||
                                                            (rl.Personnel.DateSortie.Value.Year >= periode.Year && rl.Personnel.DateSortie.Value.Month >= periode.Month)))
                                                     .ToList();
            }
            return ret;
        }

        /// <inheritdoc />
        public LotPointageEnt Get(int utilisateurId, DateTime periode)
        {
            var ret = GetLotPointageByUserIdAndPeriode(utilisateurId, periode);
            if (ret != null)
            {
                ret.RapportLignes = ret.RapportLignes.Where(rl => !rl.DateSuppression.HasValue).ToList();
            }
            return ret;
        }

        /// <inheritdoc />
        public LotPointageEnt GetWithoutLines(int utilisateurId, DateTime periode)
        {
            return Context.LotPointages.AsNoTracking().FirstOrDefault(x => x.AuteurCreationId == utilisateurId && x.Periode.Month == periode.Month && x.Periode.Year == periode.Year);
        }

        /// <inheritdoc />
        public int? GetLotPointageId(int utilisateurId, DateTime periode)
        {
            LotPointageEnt lot = Context.LotPointages.AsNoTracking()
                .FirstOrDefault(x => x.AuteurCreationId == utilisateurId && x.Periode.Month == periode.Month && x.Periode.Year == periode.Year);
            if (lot != null)
            {
                return lot.LotPointageId;
            }
            else
            {
                return null;
            }
        }


        /// <inheritdoc />
        public IEnumerable<LotPointageEnt> GetAll()
        {
            var ret = GetDefaultQuery();
            foreach (var lotPointage in ret)
            {
                lotPointage.RapportLignes = lotPointage.RapportLignes.Where(rl => !rl.DateSuppression.HasValue).ToList();
            }
            return ret;
        }

        /// <summary>
        ///   Requête par défaut de récupération des Lots de pointage
        /// </summary>
        /// <returns>Requête par défaut</returns>
        private IQueryable<LotPointageEnt> GetDefaultQuery()
        {
            return Query()
                .Include(x => x.AuteurCreation.Personnel)
                .Include(x => x.AuteurVisa.Personnel)
                .Include(x => x.ControlePointages)
                .Filter(x => x.RapportLignes.Count > 0)
                .Include(x => x.RapportLignes.Select(oo => oo.Rapport))
                .Include(x => x.RapportLignes.Select(oo => oo.Ci.Societe))
                .Include(x => x.RapportLignes.Select(oo => oo.Ci.EtablissementComptable.Societe))
                .Include(x => x.RapportLignes.Select(oo => oo.Personnel.Societe.Groupe))
                .Include(x => x.RapportLignes.Select(oo => oo.Personnel.EtablissementPaie))
                .Include(x => x.RapportLignes.Select(oo => oo.CodeAbsence))
                .Include(x => x.RapportLignes.Select(oo => oo.CodeMajoration))
                .Include(x => x.RapportLignes.Select(oo => oo.CodeDeplacement))
                .Include(x => x.RapportLignes.Select(oo => oo.CodeZoneDeplacement))
                .Include(x => x.RapportLignes.Select(oo => oo.Materiel))
                .Include(x => x.RapportLignes.Select(oo => oo.ListRapportLignePrimes.Select(xoo => xoo.Prime)))
                .Include(x => x.RapportLignes.Select(oo => oo.ListRapportLigneTaches.Select(xoo => xoo.Tache)))
                .Get();
        }

        /// <inheritdoc />
        public IEnumerable<LotPointageEnt> GetList(int auteurCreationId)
        {
            var ret = GetDefaultQuery().Where(x => x.AuteurCreationId == auteurCreationId);
            foreach (var lotPointage in ret)
            {
                lotPointage.RapportLignes = lotPointage.RapportLignes.Where(rl => !rl.DateSuppression.HasValue).ToList();
            }
            return ret;
        }

        /// <inheritdoc />
        public IEnumerable<LotPointageEnt> GetList(DateTime periode)
        {
            var ret = GetDefaultQuery().Where(x => x.Periode.Month == periode.Month && x.Periode.Year == periode.Year);
            foreach (var lotPointage in ret)
            {
                lotPointage.RapportLignes = lotPointage.RapportLignes.Where(rl => !rl.DateSuppression.HasValue).ToList();
            }
            return ret;
        }

        /// <inheritdoc />
        public LotPointageEnt UpdateLotPointage(LotPointageEnt lotPointage, int utilisateurId)
        {
            Update(lotPointage);
            Context.SaveChangesWithAudit(utilisateurId);
            return lotPointage;
        }

        /// <summary>
        /// Get lot pointage by userId and periode
        /// </summary>
        /// <param name="utilisateursIds">List des utilisateurs ids</param>
        /// <param name="periode">Période</param>
        /// <returns>Lot de pointage</returns>
        public IEnumerable<LotPointageEnt> GetLotPointageByListUserIdAndPeriode(List<int> utilisateursIds, DateTime periode)
        {
            var query = Context.LotPointages.Where(x => utilisateursIds.Contains(x.AuteurCreationId.Value) && x.Periode.Month == periode.Month && x.Periode.Year == periode.Year && x.RapportLignes.Count > 0);
            return GetAttachEntitiesForLotPointage(query);
        }

        /// <summary>
        /// Get lot pointage by userId and periode
        /// </summary>
        /// <param name="utilisateurId">Utilisateur id</param>
        /// <param name="periode">Période</param>
        /// <returns>Lot de pointage</returns>
        private LotPointageEnt GetLotPointageByUserIdAndPeriode(int utilisateurId, DateTime periode)
        {
            var query = Context.LotPointages.Where(x => x.AuteurCreationId == utilisateurId && x.Periode.Month == periode.Month && x.Periode.Year == periode.Year && x.RapportLignes.Count > 0);
            return GetAttachEntitiesForLotPointage(query).FirstOrDefault();
        }

        private IQueryable<LotPointageEnt> GetAttachEntitiesForLotPointage(IQueryable<LotPointageEnt> lotPointagesQuery)
        {
            lotPointagesQuery.Include(x => x.AuteurCreation.Personnel).Load();
            lotPointagesQuery.Include(x => x.AuteurVisa.Personnel).Load();
            lotPointagesQuery.Include(x => x.ControlePointages).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.Rapport).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.Ci.Societe).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.Ci.EtablissementComptable.Societe).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.Personnel.Societe).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.Personnel.EtablissementPaie).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.CodeAbsence).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.CodeMajoration).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.CodeDeplacement).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.CodeZoneDeplacement).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.Materiel).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.ListRapportLignePrimes).ThenInclude(xoo => xoo.Prime).Load();
            lotPointagesQuery.Include(x => x.RapportLignes).ThenInclude(oo => oo.ListRapportLigneTaches).ThenInclude(xoo => xoo.Tache).Load();
            return lotPointagesQuery;
        }

        private LotPointageEnt GetLotPointageById(int lotPointageId)
        {
            return Context.LotPointages.Where(x => x.LotPointageId == lotPointageId)
                  .Include(x => x.AuteurCreation.Personnel)
                  .Include(x => x.AuteurVisa.Personnel)
                  .Include(x => x.ControlePointages)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.Rapport)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.Ci.Societe)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.Ci.EtablissementComptable.Societe)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.Personnel.Societe.Groupe)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.Personnel.EtablissementPaie)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.CodeAbsence)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.CodeMajoration)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.CodeDeplacement)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.CodeZoneDeplacement)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.Materiel)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.ListRapportLignePrimes).ThenInclude(xoo => xoo.Prime)
                  .Include(x => x.RapportLignes).ThenInclude(oo => oo.ListRapportLigneTaches).ThenInclude(xoo => xoo.Tache)
                  .AsNoTracking().FirstOrDefault();
        }
    }
}
