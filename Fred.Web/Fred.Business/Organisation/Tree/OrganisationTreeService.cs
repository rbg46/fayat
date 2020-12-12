using System;
using System.Diagnostics;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Organisation.Tree;
using Fred.Framework;
using Microsoft.ApplicationInsights;

namespace Fred.Business.Organisation.Tree
{

    /// <summary>
    ///  Service qui retourne l'arbre des organisations de Fayat
    /// </summary>
    public class OrganisationTreeService : IOrganisationTreeService
    {
        private readonly IOrganisationTreeRepository organisationTreeRepository;
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="organisationTreeRepository">OrganisationTreeRepository</param>
        /// <param name="cacheManager">cacheManager</param>
        public OrganisationTreeService(IOrganisationTreeRepository organisationTreeRepository, ICacheManager cacheManager)
        {
            this.organisationTreeRepository = organisationTreeRepository;
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// Retourne l'arbre des organisations de Fayat ou le recupere du cache s'il existe en cache.
        /// Construit l"abre des organisations de fred avec les organisations et les affectations de tous les utilisateurs
        /// </summary>
        /// <param name="forceCreation">Force la creation de l'arbre</param>
        /// <returns>l'arbre des organisations de Fayat</returns>
        public OrganisationTree GetOrganisationTree(bool forceCreation = false)
        {
            var timer = Stopwatch.StartNew();
            var date = DateTimeOffset.Now;

            if (forceCreation)
            {
                cacheManager.Remove(OrganisationTreeCacheKey.FredOrganisationTreeCacheKey);
            }

            var a = organisationTreeRepository.GetOrganisationTree();

            timer.Stop();

            // Sending elapsed duration to AppInsight
            string filter = $"forceCreation : {forceCreation}";
            new TelemetryClient().TrackDependency("SQL", "GetOrganisationTree", filter, date, timer.Elapsed.Duration(), true);

            return a;
        }

    }
}
