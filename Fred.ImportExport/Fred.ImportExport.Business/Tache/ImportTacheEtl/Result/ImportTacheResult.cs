using Fred.Entities.Referential;
using Fred.ImportExport.Framework.Etl.Result;
using System.Collections.Generic;

namespace Fred.ImportExport.Business.CI.ImportTacheEtl.Result
{
    /// <summary>
    /// Processus etl : Contiens le résultat des transformations.
    /// </summary>
    public class ImportTacheResult : IEtlResult<TacheEnt>
    {
        public IList<TacheEnt> Items { get; set; } = new List<TacheEnt>();
    }
}
