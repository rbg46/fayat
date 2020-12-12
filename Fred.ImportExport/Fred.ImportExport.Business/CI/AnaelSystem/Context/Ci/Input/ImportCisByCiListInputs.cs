using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context.Ci.Input
{
    /// <summary>
    /// Parametre d'entree pour l'import par liste de ci ids
    /// </summary>
    [DebuggerDisplay("CiIds = {CiIds.Count}")]
    public class ImportCisByCiListInputs
    {
        /// <summary>
        /// Liste de ci a inclure dans le flux
        /// </summary>
        public List<int> CiIds { get; set; } = new List<int>();

    }
}
