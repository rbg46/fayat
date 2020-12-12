using System;
using System.Web.Services.Protocols;
using Fred.ImportExport.DataAccess.ExternalService.ExportPersonnelProxy;
using Fred.ImportExport.DataAccess.ExternalService.SalarieFesProxy;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Common;

namespace Fred.ImportExport.DataAccess.ExternalService.Tibco.Personnel
{
    /// <summary>
    /// Personnel tibco repository externe 
    /// </summary>
    public class PersonnelTibcoRepositoryExterne
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public PersonnelTibcoRepositoryExterne() { }

        /// <summary>
        /// Envoi du pointage des moyens à TIBCO
        /// </summary>
        /// <param name="records">Un tableau de classe qu'attends TIBCO GestionMoyenInRecord</param>
        /// <param name="url">Url de tibco</param>
        /// <returns>Le résultat de l'opération d'envoi</returns>
        public TibcoReturnResult SendPointageToTibco(PersonnelManagerInRecord[] records, string url)
        {
            try
            {
                string message = string.Empty;
                Exception exception = null;
                using (intfPersonnelManagerservice tibcoSoapWebService = new intfPersonnelManagerservice())
                {
                    tibcoSoapWebService.Url = url;

                    string output = tibcoSoapWebService.PersonnelManagerOp(records, out message);
                    return new TibcoReturnResult(output, message, exception);
                }
            }
            catch (SoapException fte)
            {
                return new TibcoReturnResult(Constantes.TibcoRetourErrorCode, fte.InnerException?.Message ?? fte.Message, fte);
            }
        }

        public SalarieOutRecord[] GetSalarieFesFromTibco(DateTime? dateModification, bool bypassDate)
        {
            using (intfSalarieservice service = new intfSalarieservice())
            {
                string tibcoUrl = Helper.GetUrlWebConfig(Constantes.TibcoImportSalarieFes);
                if (!string.IsNullOrEmpty(tibcoUrl))
                {
                    service.Url = tibcoUrl;
                }

                DateTime inputDate = bypassDate || !dateModification.HasValue ? new DateTime(1900, 1, 1) : dateModification.Value;
                return service.SalarieOp(string.Empty, inputDate, true);
            }
        }
    }
}
