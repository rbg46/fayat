using System.Collections.Generic;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Objet Déplacement : contient un code et un dictionnaire pour chaque jour du mois
    /// </summary>
    public class Deplacements
    {
        /// <summary>
        /// Le code du déplacement
        /// </summary>
        public string CodeDeplacement { get; set; }

        /// <summary>
        /// Dictionnaire indiquant la présence d'un déplacement en fonction de la date
        /// </summary>
        public Dictionary<int, string> DicoDeplacement { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// Indique si le code est celui du déplacement
        /// </summary>
        /// <param name="code">Code recherché</param>
        /// <returns>Retourne vrai si la PrimeJours contient le code recherché</returns>
        public bool Contains(string code)
        {
            if (!string.IsNullOrEmpty(CodeDeplacement) && CodeDeplacement == code)
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
