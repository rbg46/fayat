using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    /// <summary>
    /// Majorations pointage rapport hebdo cell
    /// </summary>
    public class MajorationPointageHebdoCell : PointageCell
    {
        /// <summary>
        /// Obtient or definit code majoration identifier
        /// </summary>
        public int CodeMajorationId { get; set; }

        /// <summary>
        /// Obtient or definit Code Majoration libelle
        /// </summary>
        public string CodeMajoration { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si le code majoration est une majoration de nuit ou non.
        /// </summary>
        public bool IsHeureNuit { get; set; }

        /// <summary>
        /// Obtient or definit list des majorations pointage par jour
        /// </summary>
        public List<MajorationHeurePerDay> MajorationHeurePerDayList { get; set; }
    }
}
