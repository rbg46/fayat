using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Business.Achat.Calculation;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Service qui renvoie les receptions en fonction d'un filtre
    /// </summary>
    public class ReceptionsProviderWithFilterService : IReceptionsProviderWithFilterService
    {
        private readonly IDepenseRepository depenseRepository;
        private readonly IReceptionFilterCiProvider receptionFilterCiProvider;
        private readonly ISoldeFarService soldeFarService;

        public ReceptionsProviderWithFilterService(IDepenseRepository depenseRepository,
            IReceptionFilterCiProvider receptionFilterCiProvider,
            ISoldeFarService soldeFarService)
        {
            this.depenseRepository = depenseRepository;
            this.receptionFilterCiProvider = receptionFilterCiProvider;
            this.soldeFarService = soldeFarService;
        }

        public List<int> GetReceptionsIdsWithFilter(SearchDepenseEnt filter)
        {
            if (filter == null)
                throw new System.ArgumentNullException(nameof(filter));

            receptionFilterCiProvider.InitializeCisOnFilter(filter, byPassCurrentUser: false);

            if (filter.Far)
            {
                var result = depenseRepository.GetReceptionsIdsWithFilter(filter);
                return soldeFarService.SelectReceptionsWithSoldeFar(result, filter.DateFrom, filter.DateTo);
            }
            else
            {
                return depenseRepository.GetReceptionsIdsWithFilter(filter);
            }
        }

        public List<DepenseAchatEnt> GetReceptionsWithFilter(SearchDepenseEnt filter)
        {
            if (filter == null)
                throw new System.ArgumentNullException(nameof(filter));

            receptionFilterCiProvider.InitializeCisOnFilter(filter, byPassCurrentUser: true);

            var resultIds = depenseRepository.GetReceptionsIdsWithFilter(filter);


            var filters = new List<Expression<Func<DepenseAchatEnt, bool>>>
            {
                x => resultIds.Contains(x.DepenseId)
            };

            var receptionsDbVisables = this.depenseRepository.Get(filters, asNoTracking: true);

            var result = depenseRepository.Get(filters, asNoTracking: true);

            return result;
        }
    }
}
