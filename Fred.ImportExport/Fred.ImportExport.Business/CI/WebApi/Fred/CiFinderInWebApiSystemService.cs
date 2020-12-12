using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.CI;

namespace Fred.ImportExport.Business.CI.WebApi.Fred
{
    public class CiFinderInWebApiSystemService : ICiFinderInWebApiSystemService
    {

        private readonly Func<CIEnt, CIEnt, bool> ciExistFunc = (currentCi, ciToFind) =>
                                                                    currentCi.Code == ciToFind.Code &&
                                                                    currentCi.SocieteId == ciToFind.SocieteId &&
                                                                    currentCi.EtablissementComptableId == ciToFind.EtablissementComptableId;


        /// <summary>
        /// Recherche un ci dans une liste
        /// </summary>
        /// <param name="existingCIs">liste de cis</param>
        /// <param name="webApiCi">i web api mappé en cient</param>
        /// <returns>le ci qui match</returns>
        public CIEnt GetCiIn(List<CIEnt> existingCIs, CIEnt webApiCi)
        {
            return existingCIs.SingleOrDefault(currentCi => ciExistFunc(currentCi, webApiCi));
        }

        /// <summary>
        /// dermine si un ci est contenu dans la liste
        /// </summary>
        /// <param name="existingCIs">liste de cis</param>
        /// <param name="webApiCi">ci web api mappé en cient</param>
        /// <returns>oui si il existe dans la liste</returns>
        public bool CiExistIn(List<CIEnt> existingCIs, CIEnt webApiCi)
        {
            return existingCIs.Any(currentCi => ciExistFunc(currentCi, webApiCi));
        }

    }
}
