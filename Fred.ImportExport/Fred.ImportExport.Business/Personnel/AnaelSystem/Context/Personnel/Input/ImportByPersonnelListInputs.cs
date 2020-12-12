using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Personnel.Input
{
    /// <summary>
    /// Parametre d'entree pour l'import par liste de personnel ids
    /// </summary>
    [DebuggerDisplay("PersonnelIds = {PersonnelIds.Count}")]
    public class ImportByPersonnelListInputs
    {
        /// <summary>
        /// Liste de ci a inclure dans le flux
        /// </summary>
        public List<int> PersonnelIds { get; set; } = new List<int>();

    }
}
