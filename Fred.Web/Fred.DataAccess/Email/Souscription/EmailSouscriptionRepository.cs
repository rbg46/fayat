using Fred.DataAccess.Common;
using Fred.Entities.Email;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using Fred.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Email.Souscription
{
    /// <summary>
    /// Repo de souscription d'email
    /// </summary>
    public class EmailSouscriptionRepository : FredRepository<EmailSouscriptionEnt>, IEmailSouscriptionRepository
    {

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="EmailSouscriptionRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public EmailSouscriptionRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Souscription a une mailling liste
        /// </summary>      
        /// <param name="emailSouscriptionKey">clé de la mailling list</param>
        /// <param name="personnelIds">Id des personnels</param>
        public void SubscribeToMaillingList(EmailSouscriptionKey emailSouscriptionKey, params int[] personnelIds)
        {

            List<int> existingSubscribersIds = GetSubscribersToMaillingList(emailSouscriptionKey, personnelIds);

            List<int> newSubscribersIds = personnelIds.Except(existingSubscribersIds).ToList();

            foreach (int personnelId in newSubscribersIds)
            {
                this.Insert(new EmailSouscriptionEnt()
                {
                    DateDernierEnvoie = null,
                    PersonnelId = personnelId,
                    SouscriptionKey = emailSouscriptionKey
                });
            }
        }

        /// <summary>
        /// Permet de désouscrire a une mailling liste
        /// </summary>
        /// <param name="emailSouscriptionKey">clé de la mailling list</param>
        /// <param name="personnelIds">Id des personnels</param>
        public void UnSubscribeToMaillingList(EmailSouscriptionKey emailSouscriptionKey, params int[] personnelIds)
        {
            List<EmailSouscriptionEnt> subscriptions = this.Query()
                                    .Filter(s => personnelIds.Contains(s.PersonnelId) && s.SouscriptionKey == emailSouscriptionKey)
                                    .Get()
                                    .AsNoTracking()
                                    .ToList();

            foreach (EmailSouscriptionEnt subscription in subscriptions)
            {
                this.DeleteById(subscription.EmailSouscriptionId);
            }
        }

        /// <summary>
        /// Retourne la liste des personnels eyant souscrit a une malling liste et qui sont dans la liste des personnelIdsRequested
        /// </summary>
        /// <param name="emailSouscriptionKey">Cle de souscription</param>
        /// <param name="personnelIdsRequested">Liste des personnels avec laquelle on fait notre recherche</param>
        /// <returns>la liste des personnels ID</returns>
        public List<int> GetSubscribersToMaillingList(EmailSouscriptionKey emailSouscriptionKey, params int[] personnelIdsRequested)
        {
            List<int> subscribersIds = this.Query()
                                    .Filter(s => s.SouscriptionKey == emailSouscriptionKey && personnelIdsRequested.Contains(s.PersonnelId))
                                    .Get()
                                    .AsNoTracking()
                                    .Select(s => s.PersonnelId)
                                    .ToList();
            return subscribersIds;
        }

        /// <summary>
        /// Met a jour la table EmailSouscriptionEnt avec la date de dernier envoie
        /// </summary>
        /// <param name="emailSouscriptionKey">la cle EmailSouscriptionKey</param>
        /// <param name="subscribersIdsOnSuccess">les ids des personnel dont les mail ont resussit</param>
        /// <param name="dateOfExecution">Date d'execution du dernier envoie reussi</param>
        public void UpdateDateEnvoieOfMaillingList(EmailSouscriptionKey emailSouscriptionKey, List<int> subscribersIdsOnSuccess, DateTime dateOfExecution)
        {

            List<EmailSouscriptionEnt> subscribers = this.Query()
                                    .Filter(s => s.SouscriptionKey == emailSouscriptionKey && subscribersIdsOnSuccess.Contains(s.PersonnelId))
                                    .Filter(s => subscribersIdsOnSuccess.Contains(s.PersonnelId))
                                    .Get()
                                    .ToList();
            DateTime normalizedDate = new DateTime(dateOfExecution.Year, dateOfExecution.Month, dateOfExecution.Day);
            foreach (EmailSouscriptionEnt subscriber in subscribers)
            {
                subscriber.DateDernierEnvoie = normalizedDate;
            }
        }

        /// <summary>
        /// Retourne la liste des personnel eyant souscrit a une malling liste
        /// </summary>
        /// <param name="emailSouscriptionKey">emailSouscriptionKey</param>
        /// <param name="dateDernierEnvoieToExclude">Date qui exclue les souscribers dont la dateDernierEnvoie correspond à dateDernierEnvoieToExclude</param>
        /// <returns>la liste des personnels ID</returns>
        public List<int> GetSubscribersToMaillingList(EmailSouscriptionKey emailSouscriptionKey, DateTime dateDernierEnvoieToExclude)
        {
            DateTime normalizedDate = new DateTime(dateDernierEnvoieToExclude.Year, dateDernierEnvoieToExclude.Month, dateDernierEnvoieToExclude.Day);
            List<int> subscribersIds = this.Query()
                                    .Filter(s => s.SouscriptionKey == emailSouscriptionKey)
                                    .Filter(s => s.DateDernierEnvoie != normalizedDate)
                                    .Get()
                                    .AsNoTracking()
                                    .Select(s => s.PersonnelId)
                                    .ToList();
            return subscribersIds;
        }
    }
}
