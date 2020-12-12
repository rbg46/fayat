using Fred.Business.Depense;
using Fred.Business.OperationDiverse;
using Fred.Business.Reception;
using Fred.Business.Valorisation;
using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Valorisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Framework.Extensions;

namespace Fred.Business.DepenseGlobale
{
    /// <summary>
    /// Gestionnaire des dépenses globales (Dépenses Achat, Valorisations, Operations Diverse)
    /// </summary>
    public class DepenseGlobaleManager : Manager, IDepenseGlobaleManager
    {
        private readonly IValorisationManager valorisationManager;
        private readonly IOperationDiverseManager operationDiverseManager;
        private readonly IRemplacementTacheManager remplacementTacheManager;
        private readonly IReceptionManager receptionManager;

        public DepenseGlobaleManager(IValorisationManager valorisationManager, IOperationDiverseManager operationDiverseManager, IRemplacementTacheManager remplacementTacheManager, IReceptionManager receptionManager)
        {
            this.valorisationManager = valorisationManager;
            this.operationDiverseManager = operationDiverseManager;
            this.remplacementTacheManager = remplacementTacheManager;
            this.receptionManager = receptionManager;
        }

        /// <summary>
        /// Liste des dépenses achats filtrée
        /// </summary>
        /// <param name="filtre">Filtre dépense globale</param>    
        /// <returns>Liste des dépenses achats</returns>
        public async Task<IEnumerable<DepenseAchatEnt>> GetDepenseAchatListAsync(DepenseGlobaleFiltre filtre)
        {
            IEnumerable<DepenseAchatEnt> receptions = await receptionManager.GetReceptionsAsync(new List<DepenseGlobaleFiltre> { filtre }).ConfigureAwait(false);

            if (filtre.LastReplacedTask)
            {
                await SetDerniereTacheRemplaceeAsync(receptions.ToList(), filtre.PeriodeFin).ConfigureAwait(false);
            }

            if (filtre.TacheId.HasValue)
            {
                return receptions.Where(x => x.TacheId == filtre.TacheId.Value);
            }

            return receptions;
        }

        /// <summary>
        /// Liste des dépenses achat avec dernière tâche remplacée pour plusieurs filtre
        /// </summary>
        /// <param name="filtreByCi">Liste de filtre dépense globale</param>    
        /// <returns>Liste des dépenses achats avec dernière tâche remplacée</returns>
        public async Task<IEnumerable<DepenseAchatEnt>> GetDepenseAchatListAsync(List<DepenseGlobaleFiltre> filtreByCi)
        {
            IEnumerable<DepenseAchatEnt> receptions = await receptionManager.GetReceptionsAsync(filtreByCi).ConfigureAwait(false);

            IEnumerable<DepenseAchatEnt> depenseAchatWithGroupRemplacementTacheId = receptions.Where(q => q.GroupeRemplacementTacheId > 0);
            IReadOnlyList<RemplacementTacheEnt> remplacementTaches = await remplacementTacheManager.GetLastAsync(depenseAchatWithGroupRemplacementTacheId.Select(q => q.GroupeRemplacementTacheId.Value), filtreByCi.FirstOrDefault().PeriodeFin).ConfigureAwait(false);

            foreach (DepenseGlobaleFiltre filtre in filtreByCi.Where(q => q.LastReplacedTask))
            {
                SetDerniereTacheRemplaceeForDepenseAchats(depenseAchatWithGroupRemplacementTacheId.Where(q => q.CiId == filtre.CiId).ToList(), remplacementTaches);
            }

            if (filtreByCi.Any(q => q.TacheId.HasValue))
            {
                List<int> listTache = filtreByCi.Where(x => x.TacheId.HasValue).Select(y => y.TacheId.Value).ToList();
                return receptions.Where(q => listTache.Contains(q.TacheId.Value));
            }

            return receptions;
        }

        /// <summary>
        /// Liste des dépenses valorisation avec dernière tâche remplacée
        /// </summary>
        /// <param name="filtre">Filtre dépense globale</param>    
        /// <returns>Liste des dépenses valorisation avec dernières tâche remplacée</returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationListAsync(DepenseGlobaleFiltre filtre)
        {
            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationListAsync(filtre.CiId, filtre.RessourceId, filtre.PeriodeDebut, filtre.PeriodeFin, filtre.DeviseId).ConfigureAwait(false);

            if (filtre.LastReplacedTask)
            {
                await SetDerniereTacheRemplaceeAsync(valos.ToList(), filtre.PeriodeFin).ConfigureAwait(false);
            }

            if (filtre.TacheId.HasValue)
            {
                return valos.Where(x => x.TacheId == filtre.TacheId.Value);
            }

            return valos;
        }

        /// <summary>
        /// Liste des dépenses valorisation sans reception intérimaire avec dernière tâche remplacée
        /// </summary>
        /// <param name="filtreByCi">Liste de filtre dépense globale</param>    
        /// <returns>Liste des dépenses valorisation avec dernières tâche remplacée</returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationListWithoutReceptionInterimaireAsync(List<DepenseGlobaleFiltre> filtreByCi)
        {
            List<int> ciIds = filtreByCi.Select(q => q.CiId).ToList();
            DateTime periodeDebut = filtreByCi.Min(q => q.PeriodeDebut).Value;
            DateTime periodeFin = filtreByCi.Max(q => q.PeriodeFin).Value;

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationsWithoutReceptionInterimaireAsync(ciIds, periodeDebut, periodeFin).ConfigureAwait(false);
            return valos;
        }

        /// <summary>
        /// Liste des dépenses valorisation 
        /// </summary>
        /// <param name="filtreByCi">Liste de filtre dépense globale</param>    
        /// <returns>Liste des dépenses valorisation avec dernières tâche remplacée</returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationListAsync(List<DepenseGlobaleFiltre> filtreByCi)
        {
            List<int> ciIds = filtreByCi.Select(q => q.CiId).ToList();
            DateTime periodeDebut = filtreByCi.Min(q => q.PeriodeDebut).Value;
            DateTime periodeFin = filtreByCi.Max(q => q.PeriodeFin).Value;

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationsAsync(ciIds, periodeDebut, periodeFin).ConfigureAwait(false);
            return valos;
        }

        /// <summary>
        /// Liste des dépenses opérations diverse avec dernière tâche remplacée
        /// </summary>
        /// <param name="filtre">Filtre dépense globale</param>    
        /// <returns>Liste des dépenses opérations diverse avec dernières tâche remplacée</returns>
        public async Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListAsync(DepenseGlobaleFiltre filtre)
        {
            List<OperationDiverseEnt> ods = (await operationDiverseManager.GetOperationDiverseListAsync(filtre.CiId, filtre.RessourceId, filtre.PeriodeDebut, filtre.PeriodeFin, filtre.DeviseId).ConfigureAwait(false)).ToList();
            if (filtre.LastReplacedTask)
            {
                await SetDerniereTacheRemplaceeAsync(ods, filtre.PeriodeFin).ConfigureAwait(false);
            }

            if (filtre.TacheId.HasValue)
            {
                return ods.Where(x => x.TacheId == filtre.TacheId.Value);
            }

            return ods;
        }

        /// <summary>
        /// Liste des dépenses opérations diverse avec dernière tâche remplacée
        /// </summary>
        /// <param name="filtreByCi">Liste de filtre dépense globale</param>    
        /// <returns>Liste des dépenses opérations diverse avec dernières tâche remplacée</returns>
        public async Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListAsync(List<DepenseGlobaleFiltre> filtreByCi)
        {
            return await operationDiverseManager.GetOperationDiverseListAsync(filtreByCi).ConfigureAwait(false);
        }

        /// <summary>
        /// Cumul des dépenses Achats
        /// </summary>
        /// <param name="depenseAchats">Liste des dépenses achats</param>
        /// <returns>Total Montant HT des dépenses Achats</returns>
        public decimal GetDepenseAchatMontantHtTotal(IEnumerable<DepenseAchatEnt> depenseAchats)
        {
            return GetTotalMontantHt(depenseAchats.ToList());
        }

        /// <summary>
        /// Retourne le montant du cumul des dépenses achats par CI
        /// </summary>
        /// <param name="depenseAchats">Liste des dépenses achat</param>
        /// <param name="ciIds">Liste des indentifiants des CI</param>
        /// <returns>Dictionnaire CI / Montant </returns>
        public Dictionary<int, decimal> GetDepenseAchatMontantHtTotal(IEnumerable<DepenseAchatEnt> depenseAchats, List<int> ciIds)
        {
            Dictionary<int, decimal> amountByCi = new Dictionary<int, decimal>();

            ciIds.ForEach(ciid => amountByCi.Add(ciid, GetTotalMontantHt(depenseAchats.Where(x => x.CiId == ciid).ToList())));

            return amountByCi;
        }

        private async Task SetDerniereTacheRemplaceeAsync(List<ValorisationEnt> valorisationEnts, DateTime? periodeFin)
        {
            IEnumerable<ValorisationEnt> valorisationWithGroupRemplacementTacheId = valorisationEnts.Where(q => q.GroupeRemplacementTacheId > 0);
            IReadOnlyList<RemplacementTacheEnt> remplacementTaches = await remplacementTacheManager.GetLastAsync(valorisationWithGroupRemplacementTacheId.Select(q => q.GroupeRemplacementTacheId.Value), periodeFin).ConfigureAwait(false);

            foreach (ValorisationEnt valo in valorisationWithGroupRemplacementTacheId)
            {
                RemplacementTacheEnt remplacementTache = remplacementTaches.Where(q => q.GroupeRemplacementTacheId == valo.GroupeRemplacementTacheId).OrderByDescending(q => q.RangRemplacement).FirstOrDefault();

                if (remplacementTache != null)
                {
                    valo.Date = remplacementTache.DateComptableRemplacement.Value;
                    valo.TacheId = remplacementTache.TacheId;
                    valo.Tache = remplacementTache.Tache;
                }
            }
        }

        private async Task SetDerniereTacheRemplaceeAsync(List<OperationDiverseEnt> operationDiverseEnts, DateTime? periodeFin)
        {
            IEnumerable<OperationDiverseEnt> operationDiverseWithGroupRemplacementTacheId = operationDiverseEnts.Where(q => q.GroupeRemplacementTacheId > 0);
            IReadOnlyList<RemplacementTacheEnt> remplacementTaches = await remplacementTacheManager.GetLastAsync(operationDiverseWithGroupRemplacementTacheId.Select(q => q.GroupeRemplacementTacheId.Value), periodeFin).ConfigureAwait(false);

            foreach (OperationDiverseEnt od in operationDiverseWithGroupRemplacementTacheId)
            {
                RemplacementTacheEnt remplacementTache = remplacementTaches.Where(q => q.GroupeRemplacementTacheId == od.GroupeRemplacementTacheId).OrderByDescending(q => q.RangRemplacement).FirstOrDefault();
                if (remplacementTache != null)
                {
                    od.DateComptable = remplacementTache.DateComptableRemplacement.Value;
                    od.TacheId = remplacementTache.TacheId;
                    od.Tache = remplacementTache.Tache;
                }
            }
        }

        private async Task SetDerniereTacheRemplaceeAsync(List<DepenseAchatEnt> depenseAchatEnts, DateTime? periodeFin)
        {
            IEnumerable<DepenseAchatEnt> depenseAchatWithGroupRemplacementTacheId = depenseAchatEnts.Where(q => q.GroupeRemplacementTacheId > 0);
            IReadOnlyList<RemplacementTacheEnt> remplacementTaches = await remplacementTacheManager.GetLastAsync(depenseAchatWithGroupRemplacementTacheId.Select(q => q.GroupeRemplacementTacheId.Value), periodeFin).ConfigureAwait(false);

            SetDerniereTacheRemplaceeForDepenseAchats(depenseAchatWithGroupRemplacementTacheId, remplacementTaches);
        }

        private void SetDerniereTacheRemplaceeForDepenseAchats(IEnumerable<DepenseAchatEnt> depenseAchatWithGroupRemplacementTacheId, IReadOnlyList<RemplacementTacheEnt> remplacementTaches)
        {
            foreach (DepenseAchatEnt depenseAchat in depenseAchatWithGroupRemplacementTacheId)
            {
                RemplacementTacheEnt remplacementTache = remplacementTaches
                    .Where(q => q.GroupeRemplacementTacheId == depenseAchat.GroupeRemplacementTacheId)
                    .OrderByDescending(q => q.RangRemplacement).FirstOrDefault();

                if (remplacementTache != null)
                {
                    depenseAchat.DateComptable = remplacementTache.DateComptableRemplacement.Value;
                    depenseAchat.TacheId = remplacementTache.TacheId;
                    depenseAchat.Tache = remplacementTache.Tache;
                }
            }
        }

        private decimal GetTotalMontantHt(List<DepenseAchatEnt> depenses, int? tacheId = null)
        {
            IEnumerable<DepenseAchatEnt> filteredDepenses = depenses.Where(x => !tacheId.HasValue || x.TacheId == tacheId.Value);
            return filteredDepenses.Sum(q => q.Quantite * q.PUHT);
        }

        /// <summary>
        /// Renvoie la somme de tous les dépenses par mois
        /// Filtre sur la tache si elle est définie, sinon toutes les dépenses sont prises en compte
        /// </summary>
        /// <param name="depenses">Liste des dépense</param>
        /// <param name="tacheId">Tache sur laquelle effectuer le calcul de total</param>
        /// <returns>La somme des dépenses par mois</returns>
        public List<Tuple<DateTime, decimal>> GetDepenseAchatMontantHtTotalByMonth(List<DepenseAchatEnt> depenses, int? tacheId)
        {
            return depenses.Where(x => !tacheId.HasValue || x.TacheId == tacheId.Value)
                .GroupBy(l => new { l.DateComptable.Value.Year, l.DateComptable.Value.Month })
                .Select(cl => new Tuple<DateTime, decimal>(new DateTime(cl.Key.Year, cl.Key.Month, 15), cl.Sum(q => q.PUHT * q.Quantite)))
                .ToList();
        }

        /// <summary>
        /// US 7011
        /// Ne plus du tout prendre en compte les réceptions intérimaires et matériels externes dans la famille ayant le code « ACH »
        /// </summary>
        /// <param name="depense"></param>
        /// <returns></returns>
        public List<DepenseAchatEnt> GetDepenseWithoutReceptionInterimAndMatExterneForAchFamily(List<DepenseAchatEnt> depense)
        {
            List<DepenseAchatEnt> depenseReceptionInterimaire = depense.Where(d =>
                d.DepenseTypeId == DepenseType.Reception.ToIntValue()
                && (d.Libelle.Contains("Réception générée automatiquement S") || (d.Libelle.Substring(0, 1) == "S" && d.Libelle.Contains("- Intérim")))
            ).ToList();

            List<DepenseAchatEnt> depenseReceptionMatExterne = depense.Where(d =>
                d.DepenseTypeId == DepenseType.Reception.ToIntValue()
                && d.Libelle.Substring(0, 1) == "S"
                && d.Libelle.Contains("- Désignation")
            ).ToList();

            return depense.Except(depenseReceptionInterimaire).Except(depenseReceptionMatExterne).ToList();
        }
    }
}
