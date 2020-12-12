using System.Collections.Generic;
using Fred.Entities.CI;

namespace Fred.ImportExport.Business.CI.WebApi.Fred
{
    /// <summary>
    /// service qui aide a la recherche de ci dans l'import des ci apr api
    /// </summary>
    public interface ICiFinderInWebApiSystemService : IFredIEService
    {

        /// <summary>
        /// Recherche un ci dans une liste
        /// </summary>
        /// <param name="existingCIs">liste de cis</param>
        /// <param name="webApiCi">i web api mappé en cient</param>
        /// <returns>le ci qui match</returns>
        CIEnt GetCiIn(List<CIEnt> existingCIs, CIEnt webApiCi);
        /// <summary>
        /// dermine si un ci est contenu dans la liste
        /// </summary>
        /// <param name="existingCIs">liste de cis</param>
        /// <param name="webApiCi">ci web api mappé en cient</param>
        /// <returns>oui si il existe dans la liste</returns>
        bool CiExistIn(List<CIEnt> existingCIs, CIEnt webApiCi);

    }
}
