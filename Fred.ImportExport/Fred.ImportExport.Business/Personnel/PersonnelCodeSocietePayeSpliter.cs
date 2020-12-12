using System.Collections.Generic;
using System.Linq;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.Business.Personnel.EtlFactory
{

    /// <summary>
    /// Permet de recuperer les code societe payes du flux
    /// </summary>
    public static class PersonnelCodeSocietePayeSpliter
    {
        public static List<string> GetCodeSocietePayesInFlux(FluxEnt flux)
        {
            var result = new List<string>();

            if (string.IsNullOrEmpty(flux.SocieteCode))
            {
                return result;
            }
            return flux.SocieteCode.Split(',').ToList();

        }
    }
}
