using System;
using System.Collections.Generic;

using System.Linq;
using Fred.Business.Achat.Calculation;
using Fred.Business.Achat.Calculation.Interfaces;
using Fred.Business.Commande;
using Fred.Business.Depense;
using Fred.Business.Facturation;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Framework.Extensions;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Service qui renvoie les reception avec toutes les données necessaire au front.
    /// Ne doit etre utilisé que pour le front.
    /// </summary>
    public class SearchReceptionsService : ISearchReceptionsService
    {
        private readonly IMontantHtReceptionneService montantHtReceptionneService;
        private readonly IMontantHtService montantHtService;
        private readonly IDepenseRepository depenseRepository;
        private readonly IDateTransfertFarProviderService dateTransfertFarProviderService;
        private readonly IReceptionNatureProviderService receptionNatureProviderService;
        private readonly IFacturationTypeManager facturationTypeManager;
        private readonly IReceptionsProviderWithFilterService receptionsProviderWithFilterService;
        private readonly IVisableReceptionProviderService visableReceptionProviderService;
        private readonly ISoldeFarService soldeFarService;
        private readonly IMontantFactureService montantFactureService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="montantHtReceptionneService">montantHtReceptionneService</param>
        public SearchReceptionsService(IMontantHtReceptionneService montantHtReceptionneService,
            IMontantHtService montantHtService,
            IDepenseRepository depenseRepository,
            IDateTransfertFarProviderService dateTransfertFarProviderService,
            IReceptionNatureProviderService receptionNatureProviderService,
            IFacturationTypeManager facturationTypeManager,
            IReceptionsProviderWithFilterService receptionsProviderWithFilterService,
            IVisableReceptionProviderService visableReceptionProviderService,
            ISoldeFarService soldeFarService,
            IMontantFactureService montantFactureService)
        {
            this.montantHtReceptionneService = montantHtReceptionneService;
            this.montantHtService = montantHtService;
            this.depenseRepository = depenseRepository;
            this.dateTransfertFarProviderService = dateTransfertFarProviderService;
            this.receptionNatureProviderService = receptionNatureProviderService;
            this.facturationTypeManager = facturationTypeManager;
            this.receptionsProviderWithFilterService = receptionsProviderWithFilterService;
            this.visableReceptionProviderService = visableReceptionProviderService;
            this.soldeFarService = soldeFarService;
            this.montantFactureService = montantFactureService;
        }

        /// <summary>
        ///   Récupération des réceptions en fonction du filtre
        ///  ATTENTION : remonte toutes les données necessaire a l'affichage. A utiliser avec minutie
        /// </summary>
        /// <param name="filter">Filtre des réceptions</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page (nombre de résultat par page)</param>
        /// <returns>Objet résultat</returns>
        public TableauReceptionResult SearchReceptionsWithTotals(SearchDepenseEnt filter, int? page, int? pageSize)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            TableauReceptionResult tableauReception = CalculateTotals(filter);

            List<int> paginatedReceptionsIds = Paginate(page, pageSize, tableauReception.AllReceptionsIds);

            List<DepenseAchatEnt> partialReceptions = depenseRepository.GetReceptionsWithAllIncludes(paginatedReceptionsIds);

            partialReceptions.ComputeAll(filter.DateFrom, filter.DateTo);

            ResolveCalculatedFields(filter, partialReceptions);

            tableauReception.Receptions = partialReceptions;

            return tableauReception;
        }

        private static List<int> Paginate(int? page, int? pageSize, List<int> allReceptionsIds)
        {
            return page.HasValue && pageSize.HasValue ? allReceptionsIds.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList() : allReceptionsIds;
        }

        /// <summary>
        ///  Récupération des réceptions en fonction d'une liste d'id
        ///  ATTENTION : remonte toutes les données necessaire a l'affichage. A utiliser avec minutie
        /// </summary>
        /// <param name="dateFrom">dateFrom</param>
        /// <param name="dateTo">dateTo</param>
        /// <param name="receptionsIds">receptionsIds</param>
        /// <returns>Une liste de receptions</returns>
        public List<DepenseAchatEnt> SearchReceptionsByIds(DateTime? dateFrom, DateTime? dateTo, List<int> receptionsIds)
        {
            var filter = new SearchDepenseEnt();

            filter.DateFrom = dateFrom;

            filter.DateTo = dateTo;

            var receptions = depenseRepository.GetReceptionsWithAllIncludes(receptionsIds);

            receptions.ComputeAll(dateFrom, dateTo);

            ResolveCalculatedFields(filter, receptions);

            return receptions;
        }

        private TableauReceptionResult CalculateTotals(SearchDepenseEnt filter)
        {
            TableauReceptionResult result = new TableauReceptionResult();

            result.AllReceptionsIds = receptionsProviderWithFilterService.GetReceptionsIdsWithFilter(filter);

            result.AllVisableReceptionIds = visableReceptionProviderService.GetReceptionsVisablesIds(result.AllReceptionsIds).ToList();

            result.Count = result.AllReceptionsIds.Count;

            result.MontantHTTotal = montantHtService.CalculateMontantHtTotal(result.AllReceptionsIds);

            result.SoldeFarTotal = soldeFarService.CalculateSoldeFarTotal(result.AllReceptionsIds, filter.DateFrom, filter.DateTo);

            return result;
        }

        private void ResolveCalculatedFields(SearchDepenseEnt filter, List<DepenseAchatEnt> receptions)
        {
            SetFacturationsOfReceptions(receptions, filter);

            if (filter.DateTo.HasValue)
            {
                dateTransfertFarProviderService.SetDateTransfertFarOfReceptions(receptions, filter.DateTo.Value.Year, filter.DateTo.Value.Month);
            }

            MapReceptionsData(receptions, filter);

            MapCommandesData(receptions);
        }

        private void SetFacturationsOfReceptions(List<DepenseAchatEnt> receptions, SearchDepenseEnt filter)
        {
            var typeFacturation = facturationTypeManager.Get(FacturationType.Facturation.ToIntValue());

            foreach (var reception in receptions)
            {
                reception.FacturationsReception = reception.FacturationsReception?.Where(x => x.FacturationTypeId == typeFacturation.FacturationTypeId)?.ToList();
                reception.CommandeLigne?.Commande?.ComputeAll(filter.DateFrom, filter.DateTo);
            }
        }

        private void MapReceptionsData(List<DepenseAchatEnt> receptions, SearchDepenseEnt filter)
        {
            receptionNatureProviderService.SetNatureOfReceptions(receptions);

            montantFactureService.GetAndMapMontantFacture(receptions, filter.DateFrom, filter.DateTo); // colonne facturé

            soldeFarService.GetAndMapSoldeFar(receptions, filter.DateFrom, filter.DateTo); //colonne solde far
        }

        private void MapCommandesData(List<DepenseAchatEnt> receptions)
        {
            var commandes = receptions.Select(x => x.CommandeLigne).Select(x => x.Commande).ToList();

            montantHtService.GetAndMapMontantHT(commandes); // colonne cumul commandé

            montantHtReceptionneService.GetAndMapMontantHTReceptionne(commandes); // colonne cumul récéptionné
        }
    }
}
