using System;
using Fred.Web.Models.CI;

namespace Fred.Web.Shared.Models.Moyen
{
    public class RapportMoyenLigneModel
    {
        /// <summary>
        /// Obtient ou définit l'affectation moyen
        /// </summary>
        public AffectationMoyenModel AffectationMoyen { get; set; }

        /// <summary>
        /// Obtient ou définit les heures machine
        /// </summary>
        public double HeuresMachine { get; set; }

        /// <summary>
        /// Obtient ou définit la date du pointage
        /// </summary>
        public DateTime? DatePointage { get; set; }

        /// <summary>
        /// Obtient ou définit l'entié CI associée au pointage
        /// </summary>
        public CIModel Ci { get; set; }

    }
}
