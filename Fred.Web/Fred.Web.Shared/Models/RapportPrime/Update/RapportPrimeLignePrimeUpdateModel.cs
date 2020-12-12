namespace Fred.Web.Shared.Models.RapportPrime.Update
{
    public class RapportPrimeLignePrimeUpdateModel
    {
        public int RapportPrimeLignePrimeId { get; set; }
        public int PrimeId { get; set; }
        public double? Montant { get; set; }
        public bool IsCreated { get; set; }
        public bool IsDeleted { get; set; }
    }
}