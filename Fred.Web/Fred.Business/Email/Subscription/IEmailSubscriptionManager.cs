using System;
using System.Collections.Generic;
using Fred.Entities.Email;
using Fred.Entities.EmailSouscription.Souscription;

namespace Fred.Business.Email.Subscription
{
    /// <summary>
    /// Manager des souscriptions d'email
    /// </summary>
    public interface IEmailSubscriptionManager : IManager<EmailSouscriptionEnt>
    {
        /// <summary>
        /// Souscription a une mailling liste
        /// </summary>      
        /// <param name="emailSouscriptionKey">clé de la mailling list</param>
        /// <param name="personnelIds">Id des personnels</param>
        void SubscribeToMaillingList(EmailSouscriptionKey emailSouscriptionKey, params int[] personnelIds);

        /// <summary>
        /// Permet de désouscrire a une mailling liste
        /// </summary>
        /// <param name="emailSouscriptionKey">clé de la mailling list</param>
        /// <param name="personnelIds">Id des personnels</param>
        void UnSubscribeToMaillingList(EmailSouscriptionKey emailSouscriptionKey, params int[] personnelIds);

        /// <summary>
        /// Retourne la liste des personnel eyant souscrit a une malling liste
        /// </summary>
        /// <param name="emailSouscriptionKey">emailSouscriptionKey</param>
        /// <param name="dateDernierEnvoieToExclude">Date qui exclue les souscribers dont la dateDernierEnvoie correspond à dateDernierEnvoieToExclude</param>
        /// <returns>la liste des personnels ID</returns>
        List<int> GetSubscribersToMaillingList(EmailSouscriptionKey emailSouscriptionKey, System.DateTime dateDernierEnvoieToExclude);

        /// <summary>
        /// Permet de savoir si les personnels(personnelIdsRequested) on souscrit a une mailling liste
        /// </summary>
        /// <param name="emailSouscriptionKey">emailSouscriptionKey</param>
        /// <param name="personnelIdsRequested">les personnel ids dont on cherche a savoir si ils ont souscrit a la mailling liste</param>
        /// <returns>Liste de HasSubscribeToMaillingListResult </returns>
        List<HasSubscribeToMaillingListResult> HasSubscribeToMaillingList(EmailSouscriptionKey emailSouscriptionKey, params int[] personnelIdsRequested);

        /// <summary>
        /// Permet de savoir si un personnel a souscrit a une mail liste a partir d'une liste de HasSubscribeToMaillingListResult
        /// IL FAUT D ABORD FAIRE UN APPEL A L' AUTRE SURCHAGE DE HasSubscribeToMaillingList !!!!!! (Performance en tete)
        /// </summary>
        /// <param name="hasSubscribeToMaillingListResults">Liste de HasSubscribeToMaillingListResult</param>
        /// <param name="personnelId">personnel Id</param>
        /// <returns>vrai si le personnel a souscrit</returns>
        bool HasSubscribeToMaillingList(List<HasSubscribeToMaillingListResult> hasSubscribeToMaillingListResults, int personnelId);

        /// <summary>
        /// Met a jour la table EmailSouscriptionEnt avec la date de dernier envoie
        /// </summary>
        /// <param name="emailSouscriptionKey">la cle EmailSouscriptionKey</param>
        /// <param name="subscribersIdsOnSuccess">les ids des personnel dont les mail ont resussit</param>
        /// <param name="dateOfExecution">Date d'execution du dernier envoie reussi</param>
        void UpdateDateEnvoieOfMaillingList(EmailSouscriptionKey emailSouscriptionKey, List<int> subscribersIdsOnSuccess, DateTime dateOfExecution);
    }
}
