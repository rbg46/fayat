namespace Fred.Web.Shared.Models.Rapport
{
    public class RapportDuplicateResultModel
    {
        /// <summary>
        /// Permet de savoir si la duplication est dans un mois cloturer
        /// </summary>
        public bool HasDatesInClosedMonth { get; set; }

        /// <summary>
        /// Permet de savoir si la duplication est partielle sur differentes periodes
        /// </summary>
        public bool HasPartialDuplicationInDifferentZoneDeTravail { get; set; }
    }
}
