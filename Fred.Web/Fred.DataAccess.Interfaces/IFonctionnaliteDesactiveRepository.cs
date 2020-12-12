
using System.Collections.Generic;
using Fred.Entities.FonctionnaliteDesactive;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les FonctionnaliteDesactive.
    /// </summary>
    public interface IFonctionnaliteDesactiveRepository : IRepository<FonctionnaliteDesactiveEnt>
    {

        /// <summary>
        /// Desactive un Fonctionnalite pour une societe
        /// </summary>
        /// <param name="fonctionnaliteId">FonctionnaliteId</param>
        /// <param name="societeId">societeId</param>
        /// <returns>l'id de l'element FonctionnaliteDesactiveEnt nouvellement créé.</returns>
        int DisableFonctionnaliteForSocieteId(int fonctionnaliteId, int societeId);

        /// <summary>
        /// Retourne une liste de FonctionnaliteDesactiveEnt.
        /// Un Fonctionnalite est desactive des lors qu'il y a un 'entree'(ligne) dans la base.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Une liste de FonctionnaliteDesactiveEnt</returns>
        IEnumerable<FonctionnaliteDesactiveEnt> GetInactifFonctionnalitesForSocieteId(int societeId);
    }
}