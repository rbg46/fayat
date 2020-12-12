namespace Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel
{
    public class LignePrimeSelectModel
    {
        /// <summary>
        ///   Obtient ou définit l'entité Prime
        /// </summary>
        public string PrimeCode { get; set; }

        /// <summary>
        ///   Obtient ou définit l'heure de la prime uniquement si TypeHoraire est en heure
        /// </summary>
        public double? HeurePrime { get; set; }
    }
}
