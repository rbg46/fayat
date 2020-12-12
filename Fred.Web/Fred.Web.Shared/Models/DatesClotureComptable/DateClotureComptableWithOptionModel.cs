namespace Fred.Web.Shared.Models.DatesClotureComptable
{
    public class DateClotureComptableWithOptionModel
    {

        public int Annee { get; set; }

        public int CiId { get; set; }

        public int Mois { get; set; }

        /// <summary>
        /// Indique l'option choisi
        /// </summary>
        public string Option { get; set; }
    }
}
