using System.Collections.Generic;
using Fred.ImportExport.DataAccess.ExternalService.ExportMoyenPointageProxy;
using Fred.ImportExport.Framework.Etl.Result;

namespace Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Transform
{
    /// <summary>
    /// Processus etl : Resultat de la transformation.
    /// </summary>
    public class ExportPointageMoyenResult : IEtlResult<GestionMoyenInRecord>
    {
        /// <summary>
        /// Type de données contenant le résultat des transformations
        /// </summary>
        public IList<GestionMoyenInRecord> Items { get; set; }
    }
}
