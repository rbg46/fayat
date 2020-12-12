
using System.Collections.Generic;
using Fred.Entities.Directory;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de l'external directory
    /// </summary>
    public interface IExternalDirectoryRepository : IRepository<ExternalDirectoryEnt>
    {
        /// <summary>
        /// Permet de récupérer l'external directory en fonction de son identifiant unique 
        /// </summary>
        /// <param name="fayatAccessDirectoryId">Identifiant unique de l'external directory</param>
        /// <returns>External Directory</returns>
        ExternalDirectoryEnt GetExternalDirectoryById(int fayatAccessDirectoryId);

        /// <summary>
        /// Permet de modifier l'external directory
        /// </summary>
        /// <param name="externalDirectoryEnt">External Directory</param>
        /// <returns>External Directory modifié</returns>
        ExternalDirectoryEnt UpdateExternalDirectory(ExternalDirectoryEnt externalDirectoryEnt);

        /// <summary>
        /// Permet de récupérer l'external directory en fonction de son identifiant global unique
        /// </summary>
        /// <param name="guid">Identifiant global unique</param>
        /// <returns>External Directory</returns>
        ExternalDirectoryEnt GetExternalDirectoryByGuid(string guid);

        /// <summary>
        /// Modifie une liste d'External diroctory liste
        /// </summary>
        /// <param name="externalDirectoriesToUpdate">Liste à modifier</param>
        void UpdateExternalDirectoryList(List<ExternalDirectoryEnt> externalDirectoriesToUpdate);
    }
}
