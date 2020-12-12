
using System.Collections.Generic;
using Fred.Entities;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente le repo de l'entité AuthentificationLogEnt
    /// </summary>
    public interface IAuthentificationLogRepository : IRepository<AuthentificationLogEnt>
    {

        /// <summary>
        /// Retourne une liste d' AuthentificationLogEnt
        /// </summary>
        /// <param name="login"> critere de recherche sur le login </param>
        /// <param name="skip">skip</param>
        /// <param name="take">take</param>   
        /// <returns>liste d'AuthentificationLogEnt</returns>   
        IEnumerable<AuthentificationLogEnt> GetByLogin(string login, int skip, int take);

        /// <summary>
        /// Retourne un AuthentificationLogEnt par son Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>AuthentificationLogEnt</returns>
        AuthentificationLogEnt GetById(int id);

        /// <summary>
        /// Creation d'un AuthentificationLogEnt
        /// </summary>
        /// <param name="authentificationLog">authentificationLog</param>   
        void Add(AuthentificationLogEnt authentificationLog);

        /// <summary>
        /// Mise a jour d'un AuthentificationLogEnt
        /// </summary>
        /// <param name="authentificationLogIds">authentificationLogIds</param>   
        void DeleteAuthentificationLogs(IEnumerable<int> authentificationLogIds);


    }
}