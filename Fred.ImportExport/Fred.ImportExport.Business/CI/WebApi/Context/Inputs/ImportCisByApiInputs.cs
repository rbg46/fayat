using System.Collections.Generic;
using System.Diagnostics;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.WebApi.Context.Inputs
{
    /// <summary>
    /// ENTRANT POUR L4IMPORT PAR API
    /// </summary>
    [DebuggerDisplay("CiIds = {CiIds.Count}")]
    public class ImportCisByApiInputs
    {
        /// <summary>
        /// Liste des cis qui viennent de l'api
        /// </summary>
        public List<WebApiCiModel> WebApiCis { get; set; } = new List<WebApiCiModel>();
    }
}
