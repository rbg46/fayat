using System;

namespace Fred.Business.Email.ActivitySummary
{
    /// <summary>
    /// Service qui analyse les travaux en cours pour certains utilisateurs 
    /// </summary>
    public interface IActivitySummaryService : IService
    {
        /// <summary>
        /// Execute l'analyse les activitées en cours pour tous les utilisateurs eyant souscrit a la mailling list 'ActivitySummary'
        /// genere les email pour chaque utilisateur et les envoie.
        /// </summary>
        /// <param name="dateOfExecution">date d'execution</param>
        void Execute(DateTime dateOfExecution, string svcDir);
    }
}
