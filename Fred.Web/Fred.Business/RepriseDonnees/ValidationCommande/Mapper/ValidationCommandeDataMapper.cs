using Fred.Entities.Commande;

namespace Fred.Business.RepriseDonnees.ValidationCommande.Mapper
{
    /// <summary>
    /// Mappe les données du ci excel vers le ciEnt
    /// </summary>
    public class ValidationCommandeDataMapper : IValidationCommandeDataMapper
    {
        /// <summary>
        /// Marque la commande comme validée et stoque le jobId dans la commande 
        /// </summary>
        /// <param name="commande">commande</param>       
        /// <param name="statutCommandeValideeId">statutCommandeValideeId</param>
        public void SetCommandeAsValidate(CommandeEnt commande, int statutCommandeValideeId)
        {
            commande.StatutCommandeId = statutCommandeValideeId;
        }

        /// <summary>
        /// Marque la commande comme validée et stoque le jobId dans la commande 
        /// </summary>
        /// <param name="commande">commande</param>
        /// <param name="jobId">jobId</param>     
        public void SetHangfireJobId(CommandeEnt commande, string jobId)
        {
            commande.HangfireJobId = jobId;
        }
    }
}
