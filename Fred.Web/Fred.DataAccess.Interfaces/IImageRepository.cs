using System.Linq;
using Fred.Entities.Image;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Interface du ripo image
    /// </summary>
    public interface IImageRepository : IFredRepository<ImageEnt>
    {
        /// <summary>
        /// Retourne
        /// </summary>
        /// <param name="typeFilter">typeFilter</param>
        /// <returns>Liste d'images</returns>
        IQueryable<ImageEnt> GetAllOfType(int typeFilter);

        /// <summary>
        /// Retourn l'image par default pour un type donné
        /// </summary>
        /// <param name="typeImage">typeImage</param>
        /// <returns>ImageEnt</returns>
        ImageEnt GetDefaultImageOfType(int typeImage);
    }
}
