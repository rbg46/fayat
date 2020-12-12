
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les pays.
    /// </summary>
    public interface IPaysRepository : IFredRepository<PaysEnt>
    {
        /// <summary>
        ///   Retourne la liste des pays.
        /// </summary>
        /// <returns>La liste des pays.</returns>
        IEnumerable<PaysEnt> GetList();

        Task<IEnumerable<PaysEnt>> GetListAsync();

        /// <summary>
        ///   Search une liste de pays.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des pays.</param>
        /// <returns>Une liste de pays.</returns>
        IEnumerable<PaysEnt> Search(string text);

        /// <summary>
        ///   Ligne de recherche
        /// </summary>
        /// <param name="text">Le text recherché</param>
        /// <returns>Renvoie une liste</returns>
        IEnumerable<PaysEnt> SearchLight(string text);
    }
}