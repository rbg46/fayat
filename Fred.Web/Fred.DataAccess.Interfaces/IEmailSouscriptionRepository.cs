using System.Collections.Generic;

using Fred.Entities.Email;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repo de souscription d'email
    /// </summary>
    public interface IEmailSouscriptionRepository : IRepository<EmailSouscriptionEnt>
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
        /// Retourne la liste des personnels eyant souscrit a une malling liste et qui sont dans la liste des personnelIdsRequested
        /// </summary>
        /// <param name="emailSouscriptionKey">Cle de souscription</param>
        /// <param name="personnelIdsRequested">Liste des personnels avec laquelle on fait notre recherche</param>
        /// <returns>la liste des personnels ID</returns>
        List<int> GetSubscribersToMaillingList(EmailSouscriptionKey emailSouscriptionKey, params int[] personnelIdsRequested);

        /// <summary>
        /// Met a jour la table EmailSouscriptionEnt avec la date de dernier envoie
        /// </summary>
        /// <param name="emailSouscriptionKey">la cle EmailSouscriptionKey</param>
        /// <param name="subscribersIdsOnSuccess">les ids des personnel dont les mail ont resussit</param>
        /// <param name="dateOfExecution">Date d'execution du dernier envoie reussi</param>
        void UpdateDateEnvoieOfMaillingList(EmailSouscriptionKey emailSouscriptionKey, List<int> subscribersIdsOnSuccess, System.DateTime dateOfExecution);
    }
}
