using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.Entities.Valorisation;
using Fred.EntityFramework;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Valorisation
{
    /// <summary>
    /// Repository de la valorisation
    /// </summary>
    public class ValorisationRepository : FredRepository<ValorisationEnt>, IValorisationRepository
    {
        public ValorisationRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Ajoute une valorisation
        /// </summary>
        /// <param name="valorisationEnt">Objet valorisation</param>
        /// <returns>La valorisation</returns>
        public ValorisationEnt AddValorisation(ValorisationEnt valorisationEnt)
        {
            Insert(valorisationEnt);

            return valorisationEnt;
        }

        /// <summary>
        /// Retourne des lignes de valorisations
        /// </summary>
        /// <param name="ciId">Identifiant de ci</param>
        /// <param name="periode">Période</param>
        /// <returns>Liste de valorisations</returns>
        public IEnumerable<ValorisationEnt> GetByCiAndPeriod(int ciId, DateTime periode)
        {
            try
            {
                DateTime period = periode.GetPeriode();
                DateTime nextPeriod = periode.GetNextPeriode();
                return Get().AsNoTracking().Where(v => v.CiId == ciId && v.Date > period && v.Date < nextPeriod)
                                           .OrderBy(v => v.Date)
                                           .OrderBy(v => v.TacheId)
                                           .Include(v => v.Tache)
                                           .Include(v => v.Materiel)
                                           .Include(v => v.Personnel)
                                           .Include(v => v.ReferentielEtendu.Ressource);
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne des lignes de valorisations
        /// </summary>
        /// <param name="ciId">Identifiant de ci</param>
        /// <param name="annee">Année</param>
        /// <param name="mois">Mois</param>
        /// <returns>Liste de valorisations</returns>
        public IEnumerable<ValorisationEnt> GetByCiAndYearAndMonth(int ciId, int annee, int mois)
        {
            try
            {
                return Get().Where(v => v.CiId == ciId && v.Date.Year == annee && v.Date.Month == mois);
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne des lignes de valorisations
        /// </summary>
        /// <param name="ciId">Identifiant de ci</param>
        /// <param name="annee">Année</param>
        /// <param name="mois">Mois</param>
        /// <returns>Liste de valorisations</returns>
        public IReadOnlyList<ValorisationEnt> GetByCiAndYearAndMonth(List<int> ciIds, int annee, int mois)
        {
            return Context.Valorisations.Where(v => ciIds.Contains(v.CiId) && v.Date.Year == annee && v.Date.Month == mois).ToList();
        }

        /// <summary>
        /// Supprime les valorisations en paramètres
        /// </summary>
        /// <param name="listValo">Liste de valorisation</param>
        public void DeleteValorisations(List<ValorisationEnt> listValo)
        {
            Context.Valorisations.RemoveRange(listValo);
        }

        /// <summary>
        /// Renvoi le calcul d'un total de valorisation pour un CI et une période
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        public decimal Total(int ciId, DateTime dateDebut, DateTime dateFin)
        {
            try
            {
                decimal sum = 0;
                var listValo = Get().Where(v => v.CiId.Equals(ciId) && v.Date >= dateDebut && v.Date < dateFin).ToList();
                foreach (var valo in listValo)
                {
                    sum += valo.PUHT * valo.Quantite;
                }

                return sum;
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Renvoi le calcul d'un total de valorisation pour un CI et une période
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <param name="ressourceId">Identifiant d'une ressource de sélection</param>
        /// <param name="tacheId">Identifiant d'une tache de sélection</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        public decimal Total(int ciId, DateTime dateDebut, DateTime dateFin, int? ressourceId, int? tacheId)
        {
            try
            {
                decimal sum = 0;
                List<ValorisationEnt> listValo;
                if (tacheId.HasValue)
                {
                    listValo = Get().Where(v => v.CiId.Equals(ciId) && v.TacheId.Equals(tacheId.Value) && v.Date >= dateDebut && v.Date < dateFin).ToList();
                }
                else
                {
                    listValo = Get().Where(v => v.CiId.Equals(ciId) && v.Date >= dateDebut && v.Date < dateFin).ToList();
                }
                foreach (var valo in listValo)
                {
                    sum += valo.PUHT * valo.Quantite;
                }

                return sum;
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Renvoi le total de valorisation pour un chapitre
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <param name="chapitreId">Identifiant d'un chapitre de sélection</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        public decimal TotalByChapitreId(int ciId, DateTime dateDebut, DateTime dateFin, int chapitreId)
        {
            try
            {
                decimal sum = 0;
                List<ValorisationEnt> listValo = Get().Where(v => v.CiId.Equals(ciId) && v.ChapitreId.Equals(chapitreId) && v.Date >= dateDebut && v.Date < dateFin).ToList();
                foreach (var valo in listValo)
                {
                    sum += valo.PUHT * valo.Quantite;
                }

                return sum;
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Renvoi le total de valorisation pour une liste de chapitre codes
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <param name="chapitreCodes">liste de chapitre codes</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        public decimal TotalByChapitreCodes(int ciId, DateTime dateDebut, DateTime dateFin, List<string> chapitreCodes)
        {
            decimal sum = 0;
            var listValo = this.Query().Include(v => v.Chapitre).Get().Where(v => v.CiId.Equals(ciId) && chapitreCodes.Contains(v.Chapitre.Code) && v.Date >= dateDebut && v.Date < dateFin).ToList();
            foreach (var valo in listValo)
            {
                sum += valo.PUHT * valo.Quantite;
            }

            return sum;
        }

        /// <summary>
        /// Retourne la liste des valorisations associées à un rapport
        /// </summary>GetByRapportId
        /// <param name="rapportId">identifiant du rapport</param>
        /// <returns>valorisations</returns>
        public IEnumerable<ValorisationEnt> GetByRapportId(int rapportId)
        {
            return Context.Valorisations.Where(v => v.RapportLigneId == rapportId).AsNoTracking();
        }

        /// <summary>
        /// Retourne des lignes de valorisations
        /// </summary>
        /// <param name="listRapportLigneId">Liste d'identifiant unique de rapport ligne</param>
        /// <returns>Liste de valorisations</returns>
        public IEnumerable<ValorisationEnt> GetByListRapportLigneId(List<int> listRapportLigneId)
        {
            return Context.Valorisations.Where(valo => listRapportLigneId.Contains(valo.RapportLigneId)).AsNoTracking();
        }

        /// <summary>
        /// Retourne une valorisation
        /// </summary>
        /// <param name="groupRemplacementId">L'id du groupe de remplacement</param>
        /// <returns>Une valo</returns>
        public IEnumerable<ValorisationEnt> GetByGroupRemplacementId(int groupRemplacementId)
        {
            return Context.Valorisations
              .Where(v => v.GroupeRemplacementTacheId == groupRemplacementId)
              .Include(x => x.Tache)
              .Include(x => x.RapportLigne)
              .AsNoTracking();
        }

        /// <summary>
        /// Retourne la liste des valorisations sansa reception intérimaire selon les paramètres
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant du CI</param>
        /// <param name="periodeDebut">Date comptable periode début</param>
        /// <param name="periodeFin">Date comptable periode fin</param>
        /// <returns>Liste de dépense achat</returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationsWithoutReceptionInterimaireAsync(List<int> ciIds, DateTime periodeDebut, DateTime periodeFin)
        {
            MonthLimits monthLimitsStart = periodeDebut.GetLimitsOfMonth();
            MonthLimits monthLimitsEnd = periodeFin.GetLimitsOfMonth();

            //Attention le chargement des entité par Load(); provoque des fuites mémoire avec l'entité Personnel
            //Ne pas utiliser pour le moment
            return await Context.Valorisations
                .Include(x => x.RapportLigne)
                .Include(x => x.Personnel)
                .Include(d => d.Materiel)
                .Where(x => ciIds.Contains(x.CiId) && (x.Date >= monthLimitsStart.StartDate && x.Date <= monthLimitsEnd.EndDate) && !x.RapportLigne.ReceptionInterimaire)
                .ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des valorisations selon les paramètres
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant du CI</param>
        /// <param name="periodeDebut">Date comptable periode début</param>
        /// <param name="periodeFin">Date comptable periode fin</param>
        /// <returns>Liste de dépense achat</returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationsAsync(IEnumerable<int> ciIds, DateTime periodeDebut, DateTime periodeFin)
        {
            MonthLimits monthLimitsStart = periodeDebut.GetLimitsOfMonth();
            MonthLimits monthLimitsEnd = periodeFin.GetLimitsOfMonth();

            //Attention le chargement des entité par Load(); provoque des fuites mémoire avec l'entité Personnel
            //Ne pas utiliser pour le moment
            return await Context.Valorisations
                .Include(x => x.Personnel)
                .Include(d => d.Materiel)
                .Where(x => ciIds.Contains(x.CiId) && x.Date >= monthLimitsStart.StartDate && x.Date <= monthLimitsEnd.EndDate)
                .ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Récupération des valorisations (explorateur des dépenses)
        /// </summary>
        /// <param name="ciId">Identifiant de ci</param>
        /// <returns>Liste de valorisations</returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationsListAsync(int ciId)
        {
            return await Context.Valorisations
                               .Include(x => x.ReferentielEtendu.Ressource.SousChapitre.Chapitre)
                               .Include(x => x.ReferentielEtendu.Nature)
                               .Include(c => c.CI)
                               .Include(c => c.Tache.Parent.Parent)
                               .Include(d => d.Unite)
                               .Include(d => d.Devise)
                               .Include(p => p.Personnel.ContratInterimaires).ThenInclude(y => y.Fournisseur)
                               .Include(p => p.Personnel.ContratInterimaires).ThenInclude(y => y.Societe)
                               .Include(d => d.Personnel.Societe)
                               .Include(d => d.RapportLigne)
                               .Include(d => d.Materiel.Societe)
                               .Where(c => c.CiId == ciId
                                   && c.Quantite != 0
                                   && c.PUHT > 0)
                               .AsNoTracking()
                               .ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Récupération des valorisations
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="ressourceId"> Identifiant de la Ressource </param>
        /// <param name="periodeDebut">Date de début</param>
        /// <param name="periodeFin">Date de fin</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <returns>Enumarable de <see cref="ValorisationEnt" /></returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationsListAsync(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId)
        {
            IEnumerable<ValorisationEnt> valorisations = await GetValorisationsListAsync(ciId).ConfigureAwait(false);
            return valorisations.Where(x => x.CiId == ciId
                                 && (!ressourceId.HasValue || x.ReferentielEtendu.RessourceId == ressourceId)
                                 && (!deviseId.HasValue || x.DeviseId == deviseId)
                                 && (!periodeDebut.HasValue || x.Date >= periodeDebut)
                                 && (!periodeFin.HasValue || x.Date <= periodeFin));
        }

        public IReadOnlyList<ValorisationEnt> GetValorisationWithoutInterimaireOrMaterielExterneFromCiIdAfterPeriodeWithQuantityGreaterThanZeroAndWithoutLockPeriod(int ciId, DateTime periode)
        {
            return Context.Valorisations
                .Where(v => v.CiId == ciId && v.Date >= periode && !v.VerrouPeriode && v.Quantite > 0
                 && ((v.PersonnelId.HasValue && !v.Personnel.IsInterimaire) || (v.MaterielId.HasValue && !v.Materiel.MaterielLocation))).ToList();
        }

        public async Task<IReadOnlyList<ValorisationEnt>> GetValorisationWithoutInterimaireOrMaterielExterneFromCiIdsAfterPeriodeWithQuantityGreaterThanZeroAndWithoutLockPeriod(List<int> ciIds, DateTime periode)
        {
            return await Context.Valorisations
                .Where(v => ciIds.Contains(v.CiId) && v.Date >= periode && !v.VerrouPeriode && v.Quantite > 0
                && ((v.PersonnelId.HasValue && !v.Personnel.IsInterimaire) || (v.MaterielId.HasValue && !v.Materiel.MaterielLocation))).ToListAsync().ConfigureAwait(false);
        }

        public IEnumerable<ValorisationEnt> GetByRapportIdAndRapportLigneId(int rapportId, int rapportLigneId)
        {
            return Context.Valorisations.Where(valo => valo.RapportId == rapportId && valo.RapportLigneId == rapportLigneId).AsNoTracking();
        }

        public List<RapportRapportLigneVerrouPeriode> GetVerrouPeriodesList(List<RapportLigneEnt> rapportLignes)
        {
            var rapportIds = rapportLignes.Select(x => x.RapportId).ToList();
            var rapportLignesIds = rapportLignes.Select(x => x.RapportLigneId).ToList();
            return this.Context.Valorisations.Where(valo => rapportIds.Contains(valo.RapportId) && rapportLignesIds.Contains(valo.RapportLigneId))
                .Select(x => new RapportRapportLigneVerrouPeriode()
                {
                    RapportId = x.RapportId,
                    RapportLigneId = x.RapportLigneId,
                    VerrouPeriode = x.VerrouPeriode
                })
                .ToList();
        }

        public List<ValorisationEnt> GetValorisationsByRapporLignesIds(List<int> rapportLignesIds)
        {
            var result = new List<ValorisationEnt>();

            var allValorisations = this.Context.Valorisations.Where(v => rapportLignesIds.Contains(v.RapportLigneId)).ToList();

            return allValorisations;
        }
    }
}
