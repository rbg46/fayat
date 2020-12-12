using Fred.Entities;
using System.Collections.Generic;

namespace Fred.Business
{
  /// <summary>
  ///  Interface du Gestionnaire des AuthentificationLogEnt
  /// </summary>
  public interface IAuthentificationLogManager : IManager<AuthentificationLogEnt>
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
    /// Sauvegarde une erreur d'authentification depuis le formulaire
    /// </summary>
    /// <param name="login">login</param>
    /// <param name="requestedUrl">RequestedUrl</param>
    /// <param name="errorType">errorType</param>
    /// <param name="technicalError">technicalError</param>
    /// <param name="adressIp">adressIp</param>    
    void SaveApiAuthentificationError(string login, string requestedUrl, int errorType, string technicalError, string adressIp);

    /// <summary>
    /// Sauvegarde une erreur d'authentification depuis les Api
    /// </summary>
    /// <param name="login">login</param>
    /// <param name="requestedUrl">RequestedUrl</param>
    /// <param name="errorType">errorType</param>
    /// <param name="technicalError">technicalError</param>
    /// <param name="adressIp">adressIp</param>    
    void SaveFormsAuthentificationError(string login, string requestedUrl, int errorType, string technicalError, string adressIp);

    /// <summary>
    ///   Supprime des AuthentificationLogEnt
    /// </summary>
    /// <param name="authentificationLogIds">IDs à supprimés</param>
    void DeleteAuthentificationLogs(IEnumerable<int> authentificationLogIds);
  }
}