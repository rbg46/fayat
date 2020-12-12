using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Email;
using Fred.Entities.EmailSouscription.Souscription;

namespace Fred.Business.Email.Subscription
{
    /// <summary>
    /// Manager des souscription de mail
    /// </summary>
    public class EmailSubscriptionManager : Manager<EmailSouscriptionEnt, IEmailSouscriptionRepository>, IEmailSubscriptionManager
    {
        public EmailSubscriptionManager(IUnitOfWork uow, IEmailSouscriptionRepository emailSouscriptionRepository)
          : base(uow, emailSouscriptionRepository)
        {

        }

        /// <summary>
        /// Retourne la liste des personnel eyant souscrit a une malling liste
        /// </summary>
        /// <param name="emailSouscriptionKey">emailSouscriptionKey</param>
        /// <param name="dateDernierEnvoieToExclude">Date qui exclue les souscribers dont la dateDernierEnvoie correspond à dateDernierEnvoieToExclude</param>
        /// <returns>la liste des personnels ID</returns>
        public List<int> GetSubscribersToMaillingList(EmailSouscriptionKey emailSouscriptionKey, DateTime dateDernierEnvoieToExclude)
        {
            return this.Repository.GetSubscribersToMaillingList(emailSouscriptionKey, dateDernierEnvoieToExclude);
        }

        /// <summary>
        /// Souscription a une mailling liste
        /// </summary>      
        /// <param name="emailSouscriptionKey">clé de la mailling list</param>
        /// <param name="personnelIds">Id des personnels</param>
        public void SubscribeToMaillingList(EmailSouscriptionKey emailSouscriptionKey, params int[] personnelIds)
        {
            this.Repository.SubscribeToMaillingList(emailSouscriptionKey, personnelIds);
            this.Save();
        }

        /// <summary>
        /// Permet de désouscrire a une mailling liste
        /// </summary>
        /// <param name="emailSouscriptionKey">clé de la mailling list</param>
        /// <param name="personnelIds">Id des personnels</param>
        public void UnSubscribeToMaillingList(EmailSouscriptionKey emailSouscriptionKey, params int[] personnelIds)
        {
            this.Repository.UnSubscribeToMaillingList(emailSouscriptionKey, personnelIds);
            this.Save();
        }


        /// <summary>
        /// Permet de savoir si les personnels(personnelIdsRequested) on souscrit a une mailling liste
        /// </summary>
        /// <param name="emailSouscriptionKey">emailSouscriptionKey</param>
        /// <param name="personnelIdsRequested">les personnel ids dont on cherche a savoir si ils ont souscrit a la mailling liste</param>
        /// <returns>Liste de HasSubscribeToMaillingListResult </returns>
        public List<HasSubscribeToMaillingListResult> HasSubscribeToMaillingList(EmailSouscriptionKey emailSouscriptionKey, params int[] personnelIdsRequested)
        {
            var result = new List<HasSubscribeToMaillingListResult>();

            var subscribersIds = this.Repository.GetSubscribersToMaillingList(emailSouscriptionKey, personnelIdsRequested);

            foreach (var personnelIdRequested in personnelIdsRequested)
            {
                var hasSubscribe = subscribersIds.Contains(personnelIdRequested);
                if (hasSubscribe)
                {
                    result.Add(new HasSubscribeToMaillingListResult()
                    {
                        PersonnelId = personnelIdRequested,
                        HasSusbcribeToMaillingList = true
                    });
                }
                else
                {
                    result.Add(new HasSubscribeToMaillingListResult()
                    {
                        PersonnelId = personnelIdRequested,
                        HasSusbcribeToMaillingList = false
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// Permet de savoir si un personnel a souscrit a une mail liste a partir d'une liste de HasSubscribeToMaillingListResult
        /// IL FAUT D ABORD FAIRE UN APPEL A L' AUTRE SURCHAGE DE HasSubscribeToMaillingList !!!!!! (Performance en tete)
        /// </summary>
        /// <param name="hasSubscribeToMaillingListResults">Liste de HasSubscribeToMaillingListResult</param>
        /// <param name="personnelId">personnel Id</param>
        /// <returns>vrai si le personnel a souscrit</returns>
        public bool HasSubscribeToMaillingList(List<HasSubscribeToMaillingListResult> hasSubscribeToMaillingListResults, int personnelId)
        {
            var result = false;
            var hasSubscribe = hasSubscribeToMaillingListResults.Any(r => r.HasSusbcribeToMaillingList && r.PersonnelId == personnelId);
            if (hasSubscribe)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Met a jour la table EmailSouscriptionEnt avec la date de dernier envoie
        /// </summary>
        /// <param name="emailSouscriptionKey">la cle EmailSouscriptionKey</param>
        /// <param name="subscribersIdsOnSuccess">les ids des personnel dont les mail ont resussit</param>
        /// <param name="dateOfExecution">Date d'execution du dernier envoie reussi</param>
        public void UpdateDateEnvoieOfMaillingList(EmailSouscriptionKey emailSouscriptionKey, List<int> subscribersIdsOnSuccess, DateTime dateOfExecution)
        {
            this.Repository.UpdateDateEnvoieOfMaillingList(emailSouscriptionKey, subscribersIdsOnSuccess, dateOfExecution);
            this.Save();
        }


    }
}
