using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.Societe;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Image;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using static Fred.Entities.Constantes;

namespace Fred.Business.Images
{
    /// <summary>
    /// Manager pour les images
    /// </summary>
    public class ImageManager : Manager<ImageEnt, IImageRepository>, IImageManager
    {
        private readonly ISocieteManager societeManager;

        public ImageManager(IUnitOfWork uow, IImageRepository imageRepository, ISocieteManager societeManager)
          : base(uow, imageRepository)
        {
            this.societeManager = societeManager;
        }

        /// <summary>
        /// Retourne la liste des images pour les logos
        /// </summary>
        /// <returns>Une liste d'images</returns>
        public IEnumerable<ImageEnt> GetLogoImages()
        {
            return Repository.GetAllOfType(TypeImage.Logo.ToIntValue());
        }

        /// <summary>
        ///  Retourne la liste des images pour les logins
        /// </summary>
        /// <returns>Une liste d'images</returns>
        public IEnumerable<ImageEnt> GetLoginImages()
        {
            return Repository.GetAllOfType(TypeImage.Login.ToIntValue());
        }

        /// <summary>
        /// Retourne l'image du login pour une societe donnée
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>L'image du login</returns>
        public ImageEnt GetLoginImage(int societeId)
        {
            var societe = societeManager.FindById(societeId);
            if (societe == null || societe.ImageLoginId == null)
            {
                return Repository.GetDefaultImageOfType(TypeImage.Login.ToIntValue());
            }
            return Repository.FindById(societe.ImageLoginId);
        }

        /// <summary>
        /// Retourne l'image du logo pour une societe donnée
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>L'image du logo</returns>
        public ImageEnt GetLogoImage(int societeId)
        {
            var societe = societeManager.FindById(societeId);
            if (societe == null || societe.ImageLogoId == null)
            {
                return Repository.GetDefaultImageOfType(TypeImage.Logo.ToIntValue());
            }
            return Repository.FindById(societe.ImageLogoId);
        }

        public async Task<ImageEnt> GetLogoImageByOrganisationIdAsync(int organisationId)
        {
            int? logoId = await societeManager.GetSocieteImageLogoIdByCodeAsync(organisationId);

            if (!logoId.HasValue)
            {
                return Repository.GetDefaultImageOfType(TypeImage.Logo.ToIntValue());
            }

            return await Repository.FindByIdAsync(logoId.Value);

        }

        /// <summary>
        /// Retourne le path des CGA pour une societe donnée et un type de commande
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="commandeType">Type de la commande</param>
        /// <returns>L'image du logo</returns>
        public string GetCGAPath(int societeId, string commandeType)
        {
            var societe = societeManager.FindById(societeId);
            switch (commandeType)
            {
                case CommandeType.Fourniture:
                    if (societe == null || societe.CGAFournitureId == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return Repository.FindById(societe.CGAFournitureId)?.Path;
                    }

                case CommandeType.Location:
                    if (societe == null || societe.CGALocationId == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return Repository.FindById(societe.CGALocationId)?.Path;
                    }

                case CommandeType.Prestation:
                case CommandeType.PrestationAvenant:
                case CommandeType.PrestationAvenantCommande:
                case CommandeType.PrestationDernierAvenant:
                    if (societe == null || societe.CGAPrestationId == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return Repository.FindById(societe.CGAPrestationId)?.Path;
                    }

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Met a jour  l'image du login pour une societe donnée
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="imageId">imageId</param>
        /// <returns>L'image du login</returns>
        public async Task<ImageEnt> UpdateSocieteLoginImageAsync(int societeId, int imageId)
        {
            var societe = GetSocieteOrThrowException(societeId);
            var image = GetImageOrThrowException(imageId);
            societe.ImageLoginId = imageId;
            societe.ImageLogin = null;
            await societeManager.UpdateSocieteAsync(societe).ConfigureAwait(false);
            return image;
        }

        /// <summary>
        /// Met a jour  l'image du logo pour une societe donnée
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="imageId">imageId</param>
        /// <returns>L'image du logo</returns>
        public async Task<ImageEnt> UpdateSocieteLogoImageAsync(int societeId, int imageId)
        {
            var societe = GetSocieteOrThrowException(societeId);
            var image = GetImageOrThrowException(imageId);

            societe.ImageLogoId = imageId;
            societe.ImageLogo = null;
            await societeManager.UpdateSocieteAsync(societe).ConfigureAwait(false);
            return image;
        }

        /// <summary>
        /// Protecteur contre une mauvaise requete.
        /// Retourne la societe ou souleve une exception
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>SocieteEnt</returns>   
        private SocieteEnt GetSocieteOrThrowException(int societeId)
        {
            var societe = societeManager.FindById(societeId);
            if (societe == null)
            {
                throw new FredBusinessNotFoundException("La société doit exister pour etre mise à jour");
            }
            return societe;
        }

        /// <summary>
        /// Protecteur contre une mauvaise requete
        /// Retourne une ImageEnt ou souleve une exception
        /// </summary>
        /// <param name="imageId">imageId</param>  
        /// <returns>ImageEnt</returns>   
        private ImageEnt GetImageOrThrowException(int imageId)
        {
            var image = Repository.FindById(imageId);
            if (image == null)
            {
                throw new FredBusinessNotFoundException("L'image doit exister pour l'affecter au login d'une société.");
            }
            return image;
        }
    }
}
