using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Depense.Converter;
using Fred.Business.OperationDiverse;
using Fred.Business.Reception;
using Fred.Business.Societe;
using Fred.Business.Valorisation;
using Fred.Entities;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Valorisation;
using Fred.Framework.Extensions;
using Fred.Web.Shared.Models.Depense;

namespace Fred.Business.Depense.Services
{
    public class DepenseServiceMediator : IDepenseServiceMediator
    {
        private readonly ICIManager cIManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IOperationDiverseManager operationDiverseManager;
        private readonly IDepenseManager depenseManager;
        private readonly IValorisationManager valorisationManager;
        private readonly IDepenseAchatService depenseAchatService;
        private readonly IRemplacementTacheManager remplacementTacheManager;
        private readonly IRemplacementTachesProviderService remplacementTachesProviderService;
        private readonly IReceptionManager receptionManager;
        private readonly ISepService sepService;


        public DepenseServiceMediator(ICIManager cIManager,
                                      IDatesClotureComptableManager datesClotureComptableManager,
                                      IOperationDiverseManager operationDiverseManager,
                                      IDepenseManager depenseManager,
                                      IValorisationManager valorisationManager,
                                      IDepenseAchatService depenseAchatService,
                                      IRemplacementTacheManager remplacementTacheManager,
                                      IRemplacementTachesProviderService remplacementTachesProviderService,
                                      IReceptionManager receptionManager,
                                      ISepService sepService)
        {
            this.cIManager = cIManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.operationDiverseManager = operationDiverseManager;
            this.depenseManager = depenseManager;
            this.valorisationManager = valorisationManager;
            this.depenseAchatService = depenseAchatService;
            this.remplacementTacheManager = remplacementTacheManager;
            this.remplacementTachesProviderService = remplacementTachesProviderService;
            this.receptionManager = receptionManager;
            this.sepService = sepService;
        }

        /// <summary>
        /// Retourne la liste des dépenses pour un export
        /// </summary>
        /// <param name="filter"><see cref="SearchDepense"/>Filtre</param>
        /// <returns><see cref="DepenseExhibition"/>DepenseExhibition</returns>
        public async Task<IEnumerable<DepenseExhibition>> GetAllDepenseExterneForExportAsync(SearchDepense filter)
        {
            IEnumerable<DepenseExhibition> depenseExhibitionAchats, depenseExhibitionValorisations, depenseExhibitionODs;
            List<DatesClotureComptableEnt> dateClotureComptableList = GetDateClotureComptableList(filter);
            IEnumerable<DepenseAchatEnt> depenses = await depenseManager.GetDepenseListAsync(filter.CiId).ConfigureAwait(false);
            IEnumerable<DepenseAchatEnt> depAchats = ComputeDepenseAchat(filter, depenses);
            IEnumerable<OperationDiverseEnt> ods = await ComputeOdAsync(filter).ConfigureAwait(false);

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationListAsync(filter).ConfigureAwait(false);

            await remplacementTachesProviderService.FillRemplacementTachesOnEntitiesAsync(depAchats.ToList(), ods.ToList(), valos.ToList()).ConfigureAwait(false);

            DepenseValorisationConverter depenseValorisationConverter = new DepenseValorisationConverter();
            DepenseOdConverter depenseOdConverter = new DepenseOdConverter();
            DepenseAchatConverter depenseAchatConverter = new DepenseAchatConverter();

            depenseExhibitionAchats = depenseAchatConverter.ConvertForExport(depAchats.ComputeAll(filter.PeriodeDebut, filter.PeriodeFin).ToList());
            depenseExhibitionValorisations = depenseValorisationConverter.ConvertForExport(valos.ToList(), filter.PeriodeDebut, filter.PeriodeFin, dateClotureComptableList);
            depenseExhibitionODs = depenseOdConverter.ConvertForExport(ods.ToList(), dateClotureComptableList);

            depenseExhibitionAchats = ApplyFilterForMontants(filter, depenseExhibitionAchats);

            // Fusion des trois listes de dépenses différentes 
            List<DepenseExhibition> fullDepenseList = depenseExhibitionAchats.Concat(depenseExhibitionValorisations).Concat(depenseExhibitionODs).ToList();

            await SetDerniereTacheRemplaceeAsync(fullDepenseList, filter.PeriodeFin.Value).ConfigureAwait(false);

            return fullDepenseList.Where(filter.GetPredicateWhere().Compile());
        }

        private IEnumerable<DepenseAchatEnt> ComputeDepenseAchat(SearchDepense filtre, IEnumerable<DepenseAchatEnt> depenseAchats)
        {
            depenseAchats = depenseAchats.Where(x => (!x.DateOperation.HasValue || ((!filtre.PeriodeDebut.HasValue || (100 * x.DateOperation.Value.Year) + x.DateOperation.Value.Month >= (100 * filtre.PeriodeDebut.Value.Year) + filtre.PeriodeDebut.Value.Month) && (100 * x.DateOperation.Value.Year) + x.DateOperation.Value.Month <= (100 * filtre.PeriodeFin.Value.Year) + filtre.PeriodeFin.Value.Month)));
            IEnumerable<DepenseAchatEnt> depAchats = depenseAchats.ComputeAll(filtre.PeriodeDebut, filtre.PeriodeFin).ToList();
            depenseAchatService.ComputeNature(depAchats);
            return depAchats;
        }

        private List<DatesClotureComptableEnt> GetDateClotureComptableList(SearchDepense filtre)
        {
            DateTime periodeDebut;

            if (!filtre.PeriodeDebut.HasValue)
            {
                DateTime? dateOuverture = cIManager.GetDateOuvertureCi(ciId: filtre.CiId);
                periodeDebut = dateOuverture ?? new DateTime(DateTime.UtcNow.Year, 1, 1);
            }
            else
            {
                periodeDebut = filtre.PeriodeDebut.Value;
            }

            return datesClotureComptableManager.GetListDatesClotureComptableByCiGreaterThanPeriode(filtre.CiId, periodeDebut.Month, periodeDebut.Year).ToList();
        }

        private async Task SetDerniereTacheRemplaceeAsync(List<DepenseExhibition> depenseExhibitions, DateTime periodeFin)
        {
            IEnumerable<DepenseExhibition> depenseExhibitionsWithGroupRemplacementTacheId = depenseExhibitions.Where(q => q.GroupeRemplacementTacheId > 0);
            IReadOnlyList<RemplacementTacheEnt> remplacementTaches = await remplacementTacheManager.GetLastAsync(depenseExhibitionsWithGroupRemplacementTacheId.Select(q => q.GroupeRemplacementTacheId), periodeFin).ConfigureAwait(false);

            foreach (DepenseExhibition depenseExhibition in depenseExhibitionsWithGroupRemplacementTacheId)
            {
                RemplacementTacheEnt remplacementTache = remplacementTaches.Where(q => q.GroupeRemplacementTacheId == depenseExhibition.GroupeRemplacementTacheId).OrderByDescending(q => q.RangRemplacement).FirstOrDefault();

                if (remplacementTache != null)
                {
                    depenseExhibition.TacheOrigineCodeLibelle = depenseExhibition.Tache.Code + " - " + depenseExhibition.Tache.Libelle;
                    depenseExhibition.TacheOrigineId = depenseExhibition.TacheId;
                    depenseExhibition.TacheOrigine = depenseExhibition.Tache;

                    depenseExhibition.DateComptableRemplacement = remplacementTache.DateComptableRemplacement.Value;
                    depenseExhibition.TacheId = remplacementTache.TacheId;
                    depenseExhibition.Tache = remplacementTache.Tache;
                }
            }

        }

        /// <summary>
        /// Retourne la liste des dépenses pour avec tache et ressources
        /// </summary>
        /// <param name="filter"><see cref="SearchDepense"/>Filtre</param>
        /// <returns><see cref="DepenseExhibition"/>DepenseExhibition</returns>
        public async Task<IEnumerable<DepenseExhibition>> GetAllDepenseExternetWithTacheAndRessourceAsync(SearchDepense filter)
        {
            IEnumerable<DepenseAchatEnt> depAchats = ComputeDepenseAchat(filter, await depenseManager.GetDepensesListWithMinimumIncludesAsync(filter.CiId).ConfigureAwait(false));

            List<DatesClotureComptableEnt> dateClotureComptableList = GetDateClotureComptableList(filter);

            IEnumerable<OperationDiverseEnt> ods = await ComputeOdAsync(filter).ConfigureAwait(false);

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationListAsync(filter).ConfigureAwait(false);
            IEnumerable<DepenseExhibition> expDepAchats, expDepValos, expDepOds;

            DepenseValorisationConverter depenseValorisationConverter = new DepenseValorisationConverter();
            DepenseOdConverter depenseOdConverter = new DepenseOdConverter();
            DepenseAchatConverter depenseAchatConverter = new DepenseAchatConverter();

            expDepAchats = depenseAchatConverter.Convert(depAchats.ComputeAll(filter.PeriodeDebut, filter.PeriodeFin).ToList());
            expDepValos = depenseValorisationConverter.Convert(valos.ToList(), filter.PeriodeDebut, filter.PeriodeFin, dateClotureComptableList);
            expDepOds = depenseOdConverter.Convert(ods.ToList(), dateClotureComptableList);
            expDepAchats = ApplyFilterForMontants(filter, expDepAchats);

            // Fusion des trois listes de dépenses différentes 
            List<DepenseExhibition> fullDepenseList = expDepAchats.Concat(expDepValos).Concat(expDepOds).ToList();
            await SetDerniereTacheRemplaceeAsync(fullDepenseList, filter.PeriodeFin.Value).ConfigureAwait(false);
            return fullDepenseList.Where(filter.GetPredicateWhere().Compile());
        }

        private IEnumerable<DepenseExhibition> ApplyFilterForMontants(SearchDepense filtre, IEnumerable<DepenseExhibition> expDepAchats)
        {
            // On applique les éventuels filtres sur les MontantHTDebut et MontantHTFin
            return expDepAchats.Where(x =>
            (
            !filtre.MontantHTDebut.HasValue || ((x.TypeDepense == Constantes.DepenseType.Facturation || x.SousTypeDepense == Constantes.DepenseSousType.Avoir) && x.MontantHtInitial >= filtre.MontantHTDebut.Value) || (x.PUHT * x.Quantite >= filtre.MontantHTDebut.Value)
            )
            &&
            (
            !filtre.MontantHTFin.HasValue || ((x.TypeDepense == Constantes.DepenseType.Facturation || x.SousTypeDepense == Constantes.DepenseSousType.Avoir) && x.MontantHtInitial <= filtre.MontantHTFin.Value) || (x.PUHT * x.Quantite <= filtre.MontantHTFin.Value))
            );
        }

        /// <summary>
        /// Vérifie si un CI est attaché une société SEP ou non
        /// </summary>
        /// <param name="ciId">id du Ci</param>
        /// <returns>Renvoie true si le CI est attaché à une société SEP sinon faux</returns>
        public bool IsSep(int ciId)
        {
            return sepService.IsSep(ciId);
        }

        /// <summary>
        /// Retourne la liste des réceptions selon les paramètres
        /// </summary>
        /// <param name="ciIdList">Liste d'identifiant de CI</param>
        /// <param name="tacheIdList">Liste d'identifiant de tâche</param>
        /// <param name="dateDebut">Date de début</param>
        /// <param name="dateFin">Date de fin</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <returns><see cref="DepenseExhibition"/>DepenseExhibition</returns>
        public async Task<IReadOnlyList<DepenseExhibition>> GetReceptionsAsync(List<int> ciIdList, List<int> tacheIdList, DateTime? dateDebut, DateTime? dateFin, int? deviseId)
        {
            List<DepenseExhibition> receptionList = new List<DepenseExhibition>();
            IEnumerable<DepenseAchatEnt> depenseAchatEntList = await receptionManager.GetReceptionsAsync(ciIdList, tacheIdList, dateDebut, dateFin, deviseId).ConfigureAwait(false);

            depenseAchatEntList.ForEach(x => receptionList.Add(new DepenseExhibition
            {
                DepenseId = x.DepenseId,
                CommandeLigneId = x.CommandeLigneId,
                CiId = x.CiId,
                FournisseurId = x.FournisseurId,
                Libelle = x.Libelle,
                TacheId = x.TacheId.Value,
                RessourceId = x.RessourceId.Value,
                Quantite = x.Quantite,
                PUHT = x.PUHT,
                Date = x.Date,
                AuteurCreationId = x.AuteurCreationId,
                DateCreation = x.DateCreation,
                AuteurModificationId = x.AuteurModificationId,
                DateModification = x.DateModification,
                AuteurSuppressionId = x.AuteurSuppressionId,
                DateSuppression = x.DateSuppression,
                Commentaire = x.Commentaire,
                DeviseId = x.DeviseId.Value,
                NumeroBL = x.NumeroBL,
                DepenseParentId = x.DepenseParentId,
                UniteId = x.UniteId.Value,
                DepenseTypeId = x.DepenseTypeId,
                DateComptable = x.DateComptable,
                DateVisaReception = x.DateVisaReception,
                DateFacturation = x.DateFacturation,
                AuteurVisaReceptionId = x.AuteurVisaReceptionId,
                QuantiteDepense = x.QuantiteDepense,
                FarAnnulee = x.FarAnnulee,
                HangfireJobId = x.HangfireJobId,
                AfficherPuHt = x.AfficherPuHt,
                AfficherQuantite = x.AfficherQuantite,
                CompteComptable = x.CompteComptable,
                ErreurControleFar = x.ErreurControleFar,
                DateControleFar = x.DateControleFar,
                StatutVisaId = x.StatutVisaId,
                DateOperation = x.DateOperation,
                MontantHtInitial = x.MontantHtInitial,
                GroupeRemplacementTacheId = x.GroupeRemplacementTacheId.Value,
                IsReceptionInterimaire = x.IsReceptionInterimaire,
                IsReceptionMaterielExterne = x.IsReceptionMaterielExterne,
            }));

            return receptionList;
        }

        private async Task<IEnumerable<OperationDiverseEnt>> ComputeOdAsync(SearchDepense filter)
        {
            IEnumerable<OperationDiverseEnt> ods = await operationDiverseManager.GetOperationDiverseListAsync(filter.CiId).ConfigureAwait(false);

            ods = operationDiverseManager.ComputeOdsWithoutCorrectTache(ods);
            return ods;
        }
    }
}
