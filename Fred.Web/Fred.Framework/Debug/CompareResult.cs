using System.Diagnostics;

namespace Fred.Framework.Debugger
{


    /// <summary>
    /// Resultat d'une comparaison
    /// </summary>
    [DebuggerDisplay("IsEqual = {IsEqual} ")]
    public class CompareResult
    {
        /// <summary>
        /// La source
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// La cible
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// Dit si le resultat est identique
        /// </summary>
        public bool IsEqual { get; set; }
        /// <summary>
        /// L'index de l'operation
        /// </summary>
        public int Index { get; set; }
    }

}
