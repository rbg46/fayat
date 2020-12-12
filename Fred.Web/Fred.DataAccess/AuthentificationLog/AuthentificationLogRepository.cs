using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.EntityFramework;

namespace Fred.DataAccess
{
    /// <summary>
    ///    Représente le repo de l'entité AuthentificationLogRepository
    /// </summary>
    public class AuthentificationLogRepository : FredRepository<AuthentificationLogEnt>, IAuthentificationLogRepository
    {
        public AuthentificationLogRepository(FredDbContext context)
          : base(context)
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
            if (string.IsNullOrEmpty(login))
            {
                return this.Context.AuthentificationLogs.OrderByDescending(a => a.DateCreation).Skip(skip).Take(take).ToList();
            }
            return this.Context.AuthentificationLogs.Where(a => a.Login.ToLower().Contains(login.ToLower())).OrderByDescending(a => a.DateCreation).Skip(skip).Take(take).ToList();
        }

        /// <summary>
        /// Retourne un AuthentificationLogEnt par son Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>AuthentificationLogEnt</returns>   
        public AuthentificationLogEnt GetById(int id)
        {
            return this.Context.AuthentificationLogs.FirstOrDefault(a => a.AuthentificationLogId == id);
        }

        /// <summary>
        /// Creation d'un AuthentificationLogEnt
        /// </summary>
        /// <param name="authentificationLog">authentificationLog</param>   
        public void Add(AuthentificationLogEnt authentificationLog)
        {
            this.Insert(authentificationLog);
        }


        /// <summary>
        /// Mise a jour d'un AuthentificationLogEnt
        /// </summary>
        /// <param name="authentificationLogIds">authentificationLogIds</param>   
        public void DeleteAuthentificationLogs(IEnumerable<int> authentificationLogIds)
        {
            foreach (var authentificationLogId in authentificationLogIds)
            {
                DeleteById(authentificationLogId);
            }
        }


    }
}