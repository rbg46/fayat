using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Image;
using Fred.Framework;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Images
{
    /// <summary>
    /// Repository des images
    /// </summary>
    public class ImageRepository : FredRepository<ImageEnt>, IImageRepository
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logMgr">logMgr</param>
        /// <param name="uow">uow</param>
        public ImageRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Retourne toutes les images pour un type donné
        /// </summary>
        /// <param name="typeFilter">typeFilter</param>
        /// <returns>List d'images</returns>
        public IQueryable<ImageEnt> GetAllOfType(int typeFilter)
        {
            return Context.Images.Where(i => i.Type.Equals(typeFilter));
        }

        /// <summary>
        /// Retourn l'image par default pour un type donné
        /// </summary>
        /// <param name="typeImage">typeImage</param>
        /// <returns>ImageEnt</returns>
        public ImageEnt GetDefaultImageOfType(int typeImage)
        {
            return Context.Images.FirstOrDefault(i => i.Type.Equals(typeImage) && i.IsDefault.Value);
        }
    }
}
