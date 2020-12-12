using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.Framework;

namespace Fred.Business.RapportStatut
{
    public class RapportStatutManager : Manager<RapportStatutEnt>, IRapportStatutManager
    {
        private readonly ICacheManager cacheManager;
        private const string CacheKeyRapportStatutByCode = "RapportSatutCode_";

        public RapportStatutManager(
            IUnitOfWork uow,
            IRepository<RapportStatutEnt> repository,
            ICacheManager cacheManager)
            : base(uow, repository)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        ///   Retourne le statut du rapport en fonction du code statut
        /// </summary>
        /// <param name="statutCode">Le code du statut</param>
        /// <returns>Un statut de rapport</returns>
        public RapportStatutEnt GetRapportStatutByCode(string statutCode)
        {
            return cacheManager.GetOrCreate(
                $"{CacheKeyRapportStatutByCode}{statutCode}",
                () => Repository.Query().Filter(rs => rs.Code == statutCode).Get().Single(),
                new System.TimeSpan(1, 0, 0, 0));
        }

        /// <summary>
        ///   Retourne la liste des statuts d'un rapport.
        /// </summary>
        /// <returns>Renvoie la liste des statuts d'un rapport.</returns>
        public IEnumerable<RapportStatutEnt> GetRapportStatutList()
        {
            IEnumerable<RapportStatutEnt> rapportStatuts = Repository.Get().ToList();
            foreach (var rapportStatut in rapportStatuts)
            {
                cacheManager.GetOrCreate($"{CacheKeyRapportStatutByCode}{rapportStatut.Code}", () => rapportStatut, new System.TimeSpan(1, 0, 0, 0));
            }

            return rapportStatuts;
        }
    }
}
