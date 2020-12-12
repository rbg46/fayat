using Fred.Entities.Rapport;

namespace Fred.Web.Shared.Models.Rapport
{
    public class PointageDuplicateResultModel
    {

        /// <summary>
        /// Permet de savoir si la duplication est dans un mois cloturer
        /// </summary>
        public bool HasDatesInClosedMonth { get; set; }

        /// <summary>
        /// Etat de la duplication apres le filtrage sur les contrat interimaire
        /// </summary>
        public InterimaireDuplicationState InterimaireDuplicationState { get; set; }

        /// <summary>
        /// Permet de savoir si la duplication demander ne se fait que sur un weekend
        /// </summary>
        public bool DuplicationOnlyOnWeekendOrHoliday { get; set; }

        /// <summary>
        /// Permet de savoir si la duplication n'est que sur un personnel inactif sur la periode demandée
        /// </summary>
        public bool PersonnelIsInactiveInPeriode { get; set; }

        /// <summary>
        /// Permet de savoir si la duplication est sur une période contenant une zone identique et une zone différente de la période dupliquée
        /// </summary>
        public bool HasPartialDuplicationInDifferentZoneDeTravail { get; set; }
    }
}
