using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Fred.ImportExport.DataAccess.ExternalService.ExportMoyenPointageProxy;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Common;

namespace Fred.ImportExport.DataAccess.ExternalService.Tibco.Moyen
{
    /// <summary>
    /// Moyen repository externe 
    /// </summary>
    public class MoyenTibcoRepositoryExterne
    {
        /// <summary>
        /// Moyen tibco repository externe
        /// </summary>
        public MoyenTibcoRepositoryExterne() { }

        /// <summary>
        /// Envoi du pointage des moyens à TIBCO
        /// </summary>
        /// <param name="records">Un tableau de classe qu'attends TIBCO GestionMoyenInRecord</param>
        /// <returns>Le résultat de l'opération d'envoi</returns>
        public EnvoiPointageMoyenResult SendPointageToTibco(GestionMoyenInRecord[] records)
        {
            try
            {
                string message = string.Empty;
                using (intfGestionMoyenservice tibcoSoapWebService = new intfGestionMoyenservice())
                {
                    string tibcoUrl = Helper.GetUrlWebConfig(Constantes.TibcoExportMoyenKey);
                    if (!string.IsNullOrEmpty(tibcoUrl))
                    {
                        tibcoSoapWebService.Url = tibcoUrl;
                    }

                    string output = tibcoSoapWebService.GestionMoyenOp(records, out message);
                    return new EnvoiPointageMoyenResult(output, message);
                }
            }
            catch (Exception fte)
            {
                return new EnvoiPointageMoyenResult(Constantes.TibcoRetourErrorCode, fte.InnerException?.Message ?? fte.Message);
            }
        }
    }
}
