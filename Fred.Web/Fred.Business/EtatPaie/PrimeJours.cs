using System.Collections.Generic;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Objet prime jour : contient un code et un dictionnaire pour chaque jour du mois
    /// </summary>
    public class PrimeJours
    {
        /// <summary>
        /// Code de la prime
        /// </summary>
        public string CodePrime { get; set; }

        /// <summary>
        /// Dictionnaire indiquant la présence d'une prime en fonction de la date
        /// </summary>
        public Dictionary<int, string> DicoPrimes { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// Indique si le code est celui de la prime
        /// </summary>
        /// <param name="code">Code recherché</param>
        /// <returns>Retourne vrai si la PrimeJours contient le code recherché</returns>
        public bool Contains(string code)
        {
            if (!string.IsNullOrEmpty(CodePrime) && CodePrime == code)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
