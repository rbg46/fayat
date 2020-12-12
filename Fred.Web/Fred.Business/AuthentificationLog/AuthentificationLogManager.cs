using System;
using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Framework.Extensions;

namespace Fred.Business
{
    /// <summary>
    ///   Gestionnaire des AuthentificationLogEnt
    /// </summary>
    public class AuthentificationLogManager : Manager<AuthentificationLogEnt, IAuthentificationLogRepository>, IAuthentificationLogManager
    {
        public AuthentificationLogManager(IUnitOfWork uow, IAuthentificationLogRepository authentificationLogRepository)
          : base(uow, authentificationLogRepository)
        {
        }

        /// <summary>
        /// Retourne une liste d' AuthentificationLogEnt
        /// </summary>
        /// <param name="login"> critere de recherche sur le login </param>
        /// <param name="skip">skip</param>
        /// <param name="take">take</param>   
        /// <returns>liste d'AuthentificationLogEnt</returns>   
        public IEnumerable<AuthentificationLogEnt> GetByLogin(string login, int skip, int take)
        {
            return Repository.GetByLogin(login, skip, take);
        }

        /// <summary>
        /// Retourne un AuthentificationLogEnt par son Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>AuthentificationLogEnt</returns>
        public AuthentificationLogEnt GetById(int id)
        {
            return Repository.GetById(id);
        }

        /// <summary>
        ///   Ajoute un nouveau AuthentificationLogEnt
        /// </summary>
        /// <param name="authentificationLog"> authentificationLogEnt à ajouter </param>   
        public void Add(AuthentificationLogEnt authentificationLog)
        {
            authentificationLog.DateCreation = DateTime.Now;
            Repository.Add(authentificationLog);
            Save();
        }

        /// <summary>
        /// Sauvegarde une erreur d'authentification depuis le formulaire
        /// </summary>
        /// <param name="login">login</param>
        /// <param name="requestedUrl">RequestedUrl</param>
        /// <param name="errorType">errorType</param>
        /// <param name="technicalError">technicalError</param>
        /// <param name="adressIp">adressIp</param>
        /// 
        public void SaveApiAuthentificationError(string login, string requestedUrl, int errorType, string technicalError, string adressIp)
        {
            var authentificationLog = CreateError(login, requestedUrl, errorType, technicalError, adressIp);
            authentificationLog.ErrorOrigin = ConnexionErrorOrigin.Api.ToIntValue();
            Repository.Add(authentificationLog);
            Save();
        }

        /// <summary>
        /// Sauvegarde une erreur d'authentification depuis les Api
        /// </summary>
        /// <param name="login">login</param>
        /// <param name="requestedUrl">RequestedUrl</param>
        /// <param name="errorType">errorType</param>
        /// <param name="technicalError">technicalError</param>
        /// <param name="adressIp">adressIp</param>
        /// 
        public void SaveFormsAuthentificationError(string login, string requestedUrl, int errorType, string technicalError, string adressIp)
        {
            var authentificationLog = CreateError(login, requestedUrl, errorType, technicalError, adressIp);
            authentificationLog.ErrorOrigin = ConnexionErrorOrigin.Forms.ToIntValue();
            Repository.Add(authentificationLog);
            Save();
        }

        private AuthentificationLogEnt CreateError(string login, string requestedUrl, int errorType, string technicalError, string adressIp)
        {
            return new AuthentificationLogEnt()
            {
                Login = login,
                RequestedUrl = requestedUrl,
                ErrorType = errorType,
                AdressIp = adressIp,
                DateCreation = DateTime.UtcNow,
                TechnicalError = technicalError
            };
        }


        /// <summary>
        ///   Supprime des AuthentificationLogEnt
        /// </summary>
        /// <param name="authentificationLogIds">IDs à supprimés</param>
        public void DeleteAuthentificationLogs(IEnumerable<int> authentificationLogIds)
        {
            Repository.DeleteAuthentificationLogs(authentificationLogIds);
            Save();
        }

    }
}
