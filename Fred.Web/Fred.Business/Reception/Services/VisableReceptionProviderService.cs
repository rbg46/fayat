using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.Reception.Models;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Service qui permet de savoir quelles sont les receptions visables
    /// </summary>
    public class VisableReceptionProviderService : IVisableReceptionProviderService
    {
        private readonly IDepenseRepository depenseRepository;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="depenseRepository">depenseRepository</param>
        public VisableReceptionProviderService(IDepenseRepository depenseRepository)
        {
            this.depenseRepository = depenseRepository;
        }


        /// <summary>
        /// Retourne les reception qui ne sont pas encore visées par rapport a une liste de reception passé en parametre
        /// </summary>
        /// <param name="receptionsIds">reception (ids) dont on cherche a savoir si elles sont visables</param>
        /// <returns>Liste de reception non visées</returns>
        public ReceptionVisablesResponse GetReceptionsVisables(List<int> receptionsIds)
        {
            if (receptionsIds == null)
                throw new ArgumentNullException(nameof(receptionsIds));

            var result = new ReceptionVisablesResponse();


            List<Expression<Func<DepenseAchatEnt, bool>>> filters = new List<Expression<Func<DepenseAchatEnt, bool>>>
            {
                x => receptionsIds.Contains(x.DepenseId),
                x=> (x.DateVisaReception == null || x.HangfireJobId == null) && !x.IsReceptionInterimaire
            };

            var receptionsDbVisables = this.depenseRepository.Get(filters, asNoTracking: true);

            foreach (var receptionId in receptionsIds)
            {
                var receptionVisable = receptionsDbVisables.FirstOrDefault(r => r.DepenseId == receptionId);
                if (receptionVisable != null)
                {
                    result.ReceptionsVisables.Add(receptionVisable);
                }
                else
                {
                    result.ReceptionsNotVisables.Add(receptionId);
                }
            }

            return result;
        }

        public List<int> GetReceptionsVisablesIds(List<int> receptionsIds)
        {
            var visableReceptionsResponse = this.GetReceptionsVisables(receptionsIds);

            return visableReceptionsResponse.ReceptionsVisables.Select(r => r.DepenseId).ToList();

        }

        /// <summary>
        /// Indique si la réception est encore visable
        /// </summary>
        /// <param name="depense">Dépense</param>
        /// <returns>true si la reception est visable, false sinon</returns>
        public bool IsVisable(DepenseAchatEnt depense)
        {
            return (depense.DateVisaReception == null || depense.HangfireJobId == null) && !depense.IsReceptionInterimaire;
        }

    }
}
