using System;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace Fred.ImportExport.Bootstrapper.Hangfire
{
    /// <summary>
    /// Prolongation de la durée d'affichage des jobs réussis
    /// Expiration des jobs 30 jours plus tard
    /// </summary>
    public class ProlongExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        private readonly TimeSpan expiration = TimeSpan.FromDays(30);

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = expiration;
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = expiration;
        }
    }
}
