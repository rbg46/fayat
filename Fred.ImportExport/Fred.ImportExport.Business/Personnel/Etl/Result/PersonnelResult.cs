using System.Collections.Generic;
using Fred.Entities.Personnel.Import;
using Fred.ImportExport.Framework.Etl.Result;

namespace Fred.ImportExport.Business.Personnel.Etl.Result
{
	 public class PersonnelResult : IEtlResult<PersonnelAffectationResult>
	 {
			public IList<PersonnelAffectationResult> Items { get; set; } = new List<PersonnelAffectationResult>();
	 }
}
