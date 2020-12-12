using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Framework.Images;

namespace Fred.Business.Personnel
{
    /// <summary>
    ///   Gestionnaire des images du personnel
    /// </summary>
    public class PersonnelImageManager : Manager<PersonnelImageEnt, IPersonnelImageRepository>, IPersonnelImageManager
    {
        public PersonnelImageManager(IUnitOfWork uow, IPersonnelImageRepository personnelImageRepository, IPersonnelImageValidator validator)
          : base(uow, personnelImageRepository, validator)
        {
        }

        /// <inheritdoc />
        public byte[] GetSignature(int personnelId, int? width, int? height)
        {
            PersonnelImageEnt persoImage = Repository.Get(personnelId);

            if (!(persoImage?.Signature?.Length > 0))
            {
                return null;
            }

            if (width.HasValue && height.HasValue)
            {
                return ImageHelpers.GetThumbnailImage(persoImage.Signature, width.Value, height.Value);
            }

            return persoImage.Signature;
        }

        /// <inheritdoc/>
        public PersonnelImageEnt GetPersonnelImage(int personnelId)
        {
            var persoImage = Repository.Get(personnelId);

            return persoImage ?? new PersonnelImageEnt { PersonnelId = personnelId };
        }

        /// <inheritdoc/>
        public PersonnelImageEnt AddOrUpdatePersonnelImage(PersonnelImageEnt persoImage)
        {
            persoImage.PhotoProfil = ImageHelpers.ProcessImage(persoImage?.PhotoProfil, 200, 200);
            persoImage.Signature = ImageHelpers.ProcessImage(persoImage?.Signature, 500, 200);

            this.BusinessValidation(persoImage);
            this.Repository.AddOrUpdate(persoImage);
            Save();

            return persoImage;
        }
    }
}