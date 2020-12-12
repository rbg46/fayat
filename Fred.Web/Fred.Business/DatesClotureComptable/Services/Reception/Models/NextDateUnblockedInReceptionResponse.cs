using System;
using System.Diagnostics;

namespace Fred.Business.DatesClotureComptable.Reception.Models
{
    /// <summary>
    /// Object qui donne une reponse a une requette pour savoir qu'elle est la prochaine date disponible en reception
    /// </summary>
    [DebuggerDisplay("Resquest = {Resquest} NextDate = {NextDate}")]
    public class NextDateUnblockedInReceptionResponse
    {
        /// <summary>
        /// La requette
        /// </summary>
        public NextDateUnblockedInReceptionResquest Resquest { get; set; }
        /// <summary>
        /// La prochaine date disponible 
        /// </summary>
        public DateTime NextDate { get; set; }
    }
}
