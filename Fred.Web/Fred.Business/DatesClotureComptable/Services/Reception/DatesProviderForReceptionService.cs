using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.DatesClotureComptable.Reception.Models;
using Fred.DataAccess.Interfaces;
using Fred.Entities.DatesClotureComptable;

namespace Fred.Business.DatesClotureComptable.Services
{
    public class DatesProviderForReceptionService : IDatesProviderForReceptionService
    {
        private readonly IDatesClotureComptableRepository datesClotureComptableRepository;

        public DatesProviderForReceptionService(IDatesClotureComptableRepository datesClotureComptableRepository)
        {
            this.datesClotureComptableRepository = datesClotureComptableRepository;
        }

        /// <summary>
        /// Permet de savoir si des receptions sont 'bloqué en reception'.        
        /// </summary>
        /// <param name="receptionBlockedResquests">Liste de demande d'information (ciid, year et month) </param>
        /// <returns>Liste de responses pour chaque demande</returns>
        public List<ReceptionBlockedResponse> IsBlockedInReception(List<ReceptionBlockedResquest> receptionBlockedResquests)
        {
            var result = new List<ReceptionBlockedResponse>();

            // groupe les requettes, evite les clause where en doublons
            var groupedRequests = receptionBlockedResquests.GroupBy(x => new { x.CiId, x.Year, x.Month }).Select(x => x.First()).ToList();

            // je prepare mes requettes OR
            var queries = new List<Expression<Func<DatesClotureComptableEnt, bool>>>();

            foreach (var resquest in groupedRequests)
            {
                Expression<Func<DatesClotureComptableEnt, bool>> query = x => !x.Historique && x.Annee == resquest.Year && x.CiId == resquest.CiId && x.Mois == resquest.Month && (x.DateCloture.HasValue || x.DateTransfertFAR.HasValue);

                queries.Add(query);
            }

            var datesClotureComptables = datesClotureComptableRepository.ExecuteOrQueries(queries);

            // je formatte le resultat
            foreach (var resquest in receptionBlockedResquests)
            {
                var hasResultOnDate = datesClotureComptables.Any(x => x.CiId == resquest.CiId && x.Annee == resquest.Year && x.Mois == resquest.Month);
                if (hasResultOnDate)
                {
                    result.Add(new ReceptionBlockedResponse()
                    {
                        Resquest = resquest,
                        IsBlockedInReception = true
                    });
                }
                else
                {
                    result.Add(new ReceptionBlockedResponse()
                    {
                        Resquest = resquest,
                        IsBlockedInReception = false
                    });
                }
            }
            return result;

        }

        /// <summary>
        /// Permet de savoir la prochaine datecomptable disponible pour une reception.        
        /// </summary>
        /// <param name="resquests">Liste de demande d'information (ciid, starDate) </param>
        /// <returns>Liste de responses pour chaque demande</returns>
        public List<NextDateUnblockedInReceptionResponse> GetNextDatesUnblockedInReception(List<NextDateUnblockedInReceptionResquest> resquests)
        {
            var result = new List<NextDateUnblockedInReceptionResponse>();

            // optimisation de la requette, je groupe par ciid et je prend l'annee la plus petite.
            var groupedRequests = resquests.GroupBy(x => new { x.CiId }).Select(x => x.OrderBy(y => y.StartDate).First()).ToList();

            var queries = new List<Expression<Func<DatesClotureComptableEnt, bool>>>();

            // je recupere les DateComptables apres une date donnée
            foreach (var resquest in groupedRequests)
            {
                Expression<Func<DatesClotureComptableEnt, bool>> query = x => !x.Historique && resquest.CiId == x.CiId && (100 * x.Annee) + x.Mois >= (100 * resquest.StartDate.Year) + resquest.StartDate.Month && (x.DateCloture.HasValue || x.DateTransfertFAR.HasValue);

                queries.Add(query);
            }

            var datesClotureComptables = datesClotureComptableRepository.ExecuteOrQueries(queries);

            foreach (var resquest in resquests)
            {
                var currentDate = resquest.StartDate;
                var isBlockedInReception = datesClotureComptables.Any(x => x.CiId == resquest.CiId && x.Annee == resquest.StartDate.Year && x.Mois == resquest.StartDate.Month);

                while (isBlockedInReception)
                {
                    currentDate = currentDate.AddMonths(1);
                    isBlockedInReception = datesClotureComptables.Any(x => x.CiId == resquest.CiId && x.Annee == currentDate.Year && x.Mois == currentDate.Month);
                }

                var nextDate = new DateTime(currentDate.Year, currentDate.Month, 1);

                result.Add(new NextDateUnblockedInReceptionResponse()
                {
                    Resquest = resquest,
                    NextDate = nextDate
                });

            }
            return result;

        }

        /// <summary>
        /// Permet de savoir la prochaine DateTransfertFAR pour un ci, une année et un mois.        
        /// </summary>
        /// <param name="resquests">Liste de demande d'information (ciid,  une année et un mois) </param>
        /// <returns>Liste de responses pour chaque demande</returns>
        public List<GetDateTransfertFarResponse> GetDateTransfertFars(List<GetDateTransfertFarResquest> resquests)
        {
            var result = new List<GetDateTransfertFarResponse>();

            // optimisation de la requette, je groupe par ciid ,année et mois
            var groupedRequests = resquests.GroupBy(x => new { x.CiId, x.Year, x.Month }).Select(x => x.First()).ToList();

            var queries = new List<Expression<Func<DatesClotureComptableEnt, bool>>>();

            // je recupere les DateComptables apres une date donnée
            foreach (var resquest in groupedRequests)
            {
                Expression<Func<DatesClotureComptableEnt, bool>> query = x => !x.Historique && x.Annee == resquest.Year && x.CiId == resquest.CiId && x.Mois == resquest.Month;

                queries.Add(query);
            }

            var datesClotureComptables = datesClotureComptableRepository.ExecuteOrQueries(queries);

            foreach (var resquest in resquests)
            {
                var dcc = datesClotureComptables.FirstOrDefault(x => x.CiId == resquest.CiId && x.Annee == resquest.Year && x.Mois == resquest.Month);
                result.Add(new GetDateTransfertFarResponse()
                {
                    Resquest = resquest,
                    DateTransfertFar = dcc?.DateTransfertFAR
                });

            }
            return result;
        }
    }
}
