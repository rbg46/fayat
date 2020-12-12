using System.Collections.Generic;
using Fred.Entities.Journal;
using Fred.ImportExport.Framework.Etl.Result;

namespace Fred.ImportExport.Business.JournauxComptable.Etl.Result
{
    /// <summary>
    /// Processus etl : Résultat de la transformation des Journaux Comptable
    /// </summary>
    public class JournauxComptableResult : IEtlResult<JournalEnt>
    {
        /// <summary>
        /// Type de données contenant le résultat des transformations
        /// </summary>
        public IList<JournalEnt> Items { get; set; } = new List<JournalEnt>();
    }
}
