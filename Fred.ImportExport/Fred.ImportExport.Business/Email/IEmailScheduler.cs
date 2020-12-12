using Fred.Business;

namespace Fred.ImportExport.Business.Email
{
    /// <summary>
    /// Mail scheduler
    /// </summary>
    public interface IEmailScheduler : IService
    {
        /// <summary>
        /// Initialize les campagne d'email
        /// </summary>
        void RegisterEmailCampaigns();
    }
}
