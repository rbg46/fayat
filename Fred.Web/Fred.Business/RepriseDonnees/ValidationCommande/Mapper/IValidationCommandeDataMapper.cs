using Fred.Entities.Commande;

namespace Fred.Business.RepriseDonnees.ValidationCommande.Mapper
{
    /// <summary>
    /// Marque la commande comme validée
    /// </summary>
    public interface IValidationCommandeDataMapper : IService
    {
        /// <summary>
        /// Marque la commande comme validée et stoque le jobId dans la commande 
        /// </summary>
        /// <param name="commande">commande</param>      
        /// <param name="statutCommandeValideeId">statutCommandeValideeId</param>
        void SetCommandeAsValidate(CommandeEnt commande, int statutCommandeValideeId);
        /// <summary>
        /// Marque la commande comme validée et stoque le jobId dans la commande 
        /// </summary>
        /// <param name="commande">commande</param>
        /// <param name="jobId">jobId</param>  
        void SetHangfireJobId(CommandeEnt commande, string jobId);
    }
}
