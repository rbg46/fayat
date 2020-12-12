namespace Fred.Web.Shared.Models.DatesClotureComptable
{
    public class PeriodeClotureModel
    {
        /// <summary>
        /// Obtient ou définit l'année.
        /// </summary>
        public int Annee { get; set; }

        /// <summary>
        /// Obtient ou définit le mois.
        /// </summary>
        public int Mois { get; set; }

        /// <summary>
        /// Champ calculé
        /// </summary>
        public bool IsClosed { get; set; }
    }
}
