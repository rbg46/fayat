using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.ImportExport.DataAccess.ExternalService.GetDetailContratProxy;
using Fred.ImportExport.DataAccess.ExternalService.GetListeContratATraiterProxy;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Common;

namespace Fred.ImportExport.DataAccess.ExternalService.Tibco.ContratInterimaire
{
    /// <summary>
    /// Import contrat interimaire repository externe
    /// </summary>
    public class ImportContratInterimaireRepositoryExterne
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ImportContratInterimaireRepositoryExterne() { }

        /// <summary>
        /// Récupére le détail du contrat depuis Tibco
        /// </summary>
        /// <param name="contratId">Identifiant contrat</param>
        /// <returns>Détail contrat Tibco</returns>
        public GetDetailContratResult GetDetailContratFromTibco(int contratId)
        {
            try
            {
                string messageCode = string.Empty;
                GetDetailContratOutRecord[] output;
                using (intfGetDetailContratservice service = new intfGetDetailContratservice())
                {
                    string tibcoUrl = Helper.GetUrlWebConfig(Constantes.TibcoImportGetDetailContrat);
                    if (!string.IsNullOrEmpty(tibcoUrl))
                    {
                        service.Url = tibcoUrl;
                    }

                    var message = service.GetDetailContratOp(contratId, Constantes.Fred, out messageCode, out output);
                    return new GetDetailContratResult(output, messageCode, message);
                }
            }
            catch (Exception fte)
            {
                return new GetDetailContratResult(fte.InnerException?.Message ?? fte.Message);
            }
        }

        /// <summary>
        /// Récupére la liste des contrats à traiter depuis Tibco
        /// </summary>
        /// <param name="dateDerniereExecution">La derniere date d'execution</param>
        /// <returns>La list des identifiant de contrat interimaires à traiter</returns>
        public GetListContratATraiterResult GetListContratATraiter(DateTime dateDerniereExecution)
        {
            try
            {
                string message = string.Empty;
                GetListeContratATraiterOutRecord[] output;
                using (intfGetListeContratATraiterservice service = new intfGetListeContratATraiterservice())
                {
                    string tibcoUrl = Helper.GetUrlWebConfig(Constantes.TibcoImportGetListeContratATraiter);
                    if (!string.IsNullOrEmpty(tibcoUrl))
                    {
                        service.Url = tibcoUrl;
                    }

                    service.GetListeContratATraiterOp(Constantes.Pixid, Constantes.GroupeRazelBec, dateDerniereExecution, out message, out output);
                    return new GetListContratATraiterResult(output, message);
                }
            }
            catch (Exception fte)
            {
                return new GetListContratATraiterResult(fte.InnerException?.Message ?? fte.Message);
            }
        }
    }
}
