using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Directory;
using Fred.EntityFramework;

namespace Fred.DataAccess.Directory
{
    /// <summary>
    ///   Représente un référentiel de l'external directory
    /// </summary>
    public class ExternalDirectoryRepository : FredRepository<ExternalDirectoryEnt>, IExternalDirectoryRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ExternalDirectoryRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public ExternalDirectoryRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Permet de récupérer l'external directory en fonction de son identifiant unique 
        /// </summary>
        /// <param name="fayatAccessDirectoryId">Identifiant unique de l'external directory</param>
        /// <returns>External Directory</returns>
        public ExternalDirectoryEnt GetExternalDirectoryById(int fayatAccessDirectoryId)
        {
            return Query()
                .Filter(e => e.FayatAccessDirectoryId == fayatAccessDirectoryId)
                .Get()
                .FirstOrDefault();
        }

        /// <summary>
        /// Permet de modifier l'external directory
        /// </summary>
        /// <param name="externalDirectoryEnt">External Directory</param>
        /// <returns>External Directory modifié</returns>
        public ExternalDirectoryEnt UpdateExternalDirectory(ExternalDirectoryEnt externalDirectoryEnt)
        {
            Update(externalDirectoryEnt);

            return externalDirectoryEnt;
        }

        /// <summary>
        /// Permet de récupérer l'external directory en fonction de son identifiant global unique
        /// </summary>
        /// <param name="guid">Identifiant global unique</param>
        /// <returns>External Directory</returns>
        public ExternalDirectoryEnt GetExternalDirectoryByGuid(string guid)
        {
            return Query()
                .Filter(e => e.Guid == guid)
                .Get()
                .FirstOrDefault();
        }

        /// <summary>
        /// Modifie une liste d'External diroctory liste
        /// </summary>
        /// <param name="externalDirectoriesToUpdate">Liste à modifier</param>
        public void UpdateExternalDirectoryList(List<ExternalDirectoryEnt> externalDirectoriesToUpdate)
        {
            if (externalDirectoriesToUpdate != null && externalDirectoriesToUpdate.Any())
            {
                foreach (var ext in externalDirectoriesToUpdate)
                {
                    Update(ext);
                }
            }
        }
    }
}
