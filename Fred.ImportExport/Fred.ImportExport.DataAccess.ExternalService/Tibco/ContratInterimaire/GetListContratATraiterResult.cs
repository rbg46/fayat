using System;
using System.Collections.Generic;
using System.Linq;
using Fred.ImportExport.DataAccess.ExternalService.GetListeContratATraiterProxy;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Common;

namespace Fred.ImportExport.DataAccess.ExternalService.Tibco.ContratInterimaire
{
    /// <summary>
    /// Cette classe représente le retour de TIBCO
    /// </summary>
    public class GetListContratATraiterResult
    {
        public GetListContratATraiterResult(string message)
        {
            Message = message;
        }

        public GetListContratATraiterResult(GetListeContratATraiterOutRecord[] output, string message)
        {
            Message = message;
            IdContratsATraiter = output?.Select(x => x.transfert_contrat_id).ToList();
        }

        /// <summary>
        /// Message d'erreur
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Les identifiants des contrats à traiter
        /// </summary>
        public List<int> IdContratsATraiter { get; set; }
    }
}
