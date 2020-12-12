using System.Collections.Generic;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Objet absence jour : contient un code et un dictionnaire pour chaque jour du mois
    /// </summary>
    public class Absence
    {
        /// <summary>
        /// Code de l'absence
        /// </summary>
        public string CodeAbsence { get; set; }

        /// <summary>
        /// libelle de Code absence
        /// </summary>
        public string LibelleCodeAbsence { get; set; }
        /// <summary>
        /// Libelle de CI
        /// </summary>
        public string CILibelle { get; set; }
        /// <summary>
        /// Dictionnaire indiquant la présence d'une absence en fonction de la date
        /// </summary>
        public Dictionary<int, string> DicoAbsence { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// Indique si le code est celui de la absence
        /// </summary>
        /// <param name="code">Code recherché</param>
        /// <returns>Retourne vrai si l'Absence contient le code recherché</returns>
        public bool Contains(string code)
        {
            if (!string.IsNullOrEmpty(CodeAbsence) && CodeAbsence == code)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Indique si le code et CI est celui de la absence
        /// </summary>
        /// <param name="code">Code recherché</param>
        /// <param name="ciLibelle">IdCI</param>
        /// <returns>Retourne vrai si l'Absence contient le code et CI recherché</returns>
        public bool Contains(string code, string ciLibelle)
        {
            return (!string.IsNullOrEmpty(CodeAbsence) && CodeAbsence == code) && (!string.IsNullOrEmpty(CILibelle) && CILibelle == ciLibelle);
        }
    }
}
