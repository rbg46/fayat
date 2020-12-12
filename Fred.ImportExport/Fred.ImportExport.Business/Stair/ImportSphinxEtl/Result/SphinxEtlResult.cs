using System.Collections.Generic;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Models.Stair.Sphinx;

namespace Fred.ImportExport.Business.Stair.ImportSphinxEtl.Result
{
    public class SphinxEtlResult : IEtlResult<SphinxFormulaireModel>
    {
        public IList<SphinxFormulaireModel> Items { get; set; } = new List<SphinxFormulaireModel>();
    }
}
