using System;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    /// <summary>
    /// Class pour vérifier statut du rapport ligne lors d'ajout d'un nouveau pointage
    /// </summary>
    public class RapportHebdoNewPointageStatutModel
    {
        /// <summary>
        /// Obtient ou definit l'index du jour de la semaine
        /// </summary>
        public int DayOfWeekIndex { get; set; }

        /// <summary>
        /// Obtient ou definit la date du pointage
        /// </summary>
        public DateTime DatePointage { get; set; }

        /// <summary>
        /// Obtient ou definit statut du rapport ligne si valide2
        /// </summary>
        public bool IsRapportLigneValide2 { get; set; }

        /// <summary>
        /// Obtient ou definit statut du rapport ligne si verouiller
        /// </summary>
        public bool IsRapportLigneVerouiller { get; set; }
    }
}
