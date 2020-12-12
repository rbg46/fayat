using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Personnel
{
    /// <summary>
    ///   Représente la classe d'accès aux images du personnel
    /// </summary>
    public class PersonnelImageRepository : FredRepository<PersonnelImageEnt>, IPersonnelImageRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="PersonnelRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public PersonnelImageRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <inheritdoc/>
        public PersonnelImageEnt AddOrUpdate(PersonnelImageEnt images)
        {
            if (images.PersonnelImageId > 0)
            {
                Update(images);
            }
            else
            {
                Insert(images);
            }

            return images;
        }

        /// <inheritdoc/>
        public bool Delete(int personnelImageId)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public PersonnelImageEnt Get(int personnelId)
        {
            return this.Query().Filter(x => x.PersonnelId == personnelId).Get().AsNoTracking().FirstOrDefault();
        }
    }
}