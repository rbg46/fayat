using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.Models
{
    /// <summary>
    /// Resultat d'une requette en base pour savoir si des receptions sont visables
    /// </summary>
    [DebuggerDisplay("ReceptionsVisables = {ReceptionsVisables.Count} ReceptionsNotVisables = {ReceptionsNotVisables.Count} ")]
    public class ReceptionVisablesResponse
    {
        /// <summary>
        /// Les reception visables
        /// </summary>
        public List<DepenseAchatEnt> ReceptionsVisables { get; set; } = new List<DepenseAchatEnt>();

        /// <summary>
        /// Les reception non visables
        /// </summary>
        public List<int> ReceptionsNotVisables { get; set; } = new List<int>();
    }
}
