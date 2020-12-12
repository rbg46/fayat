namespace Fred.Web.Shared.Models.RapportPrime.Get
{
    public class RapportPrimeLignePrimeGetModel
    {
        public int RapportPrimeLignePrimeId { get; set; }
        public int PrimeId { get; set; }
        public PrimeGetModel Prime { get; set; }
        public int RapportPrimeLigneId { get; set; }
        public double? Montant { get; set; }
        public bool IsCreated { get; set; }
        public bool IsDeleted { get; set; }
    }
}