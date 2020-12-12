using System.Collections.Generic;
using System.Linq;
using Fred.ImportExport.DataAccess.ExternalService.GetDetailContratProxy;

namespace Fred.ImportExport.DataAccess.ExternalService.Tibco.ContratInterimaire
{
    /// <summary>
    /// Cette classe représente le retour du service GetDetailContratResult de Tibco
    /// </summary>
    public class GetDetailContratResult
    {
        public GetDetailContratResult(string message)
        {
            Message = message;
        }

        public GetDetailContratResult(GetDetailContratOutRecord[] output, string messageCode, string message)
        {
            MessageCode = messageCode;
            Message = message;
            DetailContratOutRecord = output?.OrderByDescending(x => x.Timestamp).FirstOrDefault();
        }

        /// <summary>
        /// Code message de l'erreur
        /// </summary>
        public string MessageCode { get; set; }

        /// <summary>
        /// Message d'erreur
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Les identifiants des contrats à traiter
        /// </summary>
        public GetDetailContratOutRecord DetailContratOutRecord { get; set; }
    }
}
