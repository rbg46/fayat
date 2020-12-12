using System;
using Fred.ImportExport.Business.Email.ActivitySummary;
using Fred.ImportExport.Framework.Exceptions;
using Hangfire;
using NLog;

namespace Fred.ImportExport.Business.Email
{
    public class EmailScheduler : IEmailScheduler
    {
        private const string EmailActivitySummaryJobId = "EMAIL_RECAPITULATIF_ACTIVITE";

        private readonly IEmailActivitySummaryHanfireJob emailActivitySummaryHanfireJob;
        private readonly string cron = Cron.Daily(7, 0);

        public EmailScheduler(IEmailActivitySummaryHanfireJob emailActivitySummaryHanfireJob)
        {
            this.emailActivitySummaryHanfireJob = emailActivitySummaryHanfireJob;
        }

        public void RegisterEmailCampaigns()
        {
            try
            {
                RecurringJob.AddOrUpdate(EmailActivitySummaryJobId, () => emailActivitySummaryHanfireJob.SendEmailJob(), cron, TimeZoneInfo.Local);
            }
            catch (Exception e)
            {
                var exception = new FredIeBusinessException(e.Message, e);
                LogManager.GetCurrentClassLogger().Error(exception);
                throw;
            }
        }
    }
}
