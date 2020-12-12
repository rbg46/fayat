namespace Fred.Entities.Achat.Calculation.Commande
{
    /// <summary>
    /// Resultat du calcul du Montant HT Receptionne d'une commande
    /// </summary>
    public class CommandeMontantHTReceptionneModel
    {

        /// <summary>
        /// Id de la commande
        /// </summary>
        public int CommandeId { get; set; }

        /// <summary>
        /// Montant HT Receptionne
        /// </summary>
        public decimal MontantHTReceptionne { get; set; }
    }
}
