using Fred.Entities.Image;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.Images
{
    /// <summary>
    /// Interface du Manager pour les images
    /// </summary>
    public interface IImageManager : IManager<ImageEnt>
    {
        /// <summary>
        /// Retourne la liste des images pour les logos
        /// </summary>
        /// <returns>Une liste d'images</returns>
        IEnumerable<ImageEnt> GetLogoImages();

        /// <summary>
        ///  Retourne la liste des images pour les logins
        /// </summary>
        /// <returns>Une liste d'images</returns>
        IEnumerable<ImageEnt> GetLoginImages();

        /// <summary>
        /// Retourne l'image du login pour une societe donnée
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>L'image du login</returns>
        ImageEnt GetLoginImage(int societeId);

        /// <summary>
        /// Retourne l'image du logo pour une societe donnée
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>L'image du logo</returns>
        ImageEnt GetLogoImage(int societeId);

        Task<ImageEnt> GetLogoImageByOrganisationIdAsync(int organisationId);

        /// <summary>
        /// Retourne le path des CGA pour une societe donnée et un type de commande
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="commandeType">Type de la commande</param>
        /// <returns>L'image du logo</returns>
        string GetCGAPath(int societeId, string commandeType);

        /// <summary>
        /// Met a jour  l'image du login pour une societe donnée
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="imageId">imageId</param>
        /// <returns>L'image du login</returns>
        Task<ImageEnt> UpdateSocieteLoginImageAsync(int societeId, int imageId);

        /// <summary>
        /// Met a jour  l'image du logo pour une societe donnée
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="imageId">imageId</param>
        /// <returns>L'image du logo</returns>
        Task<ImageEnt> UpdateSocieteLogoImageAsync(int societeId, int imageId);
    }
}
