namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Genere automatiquement les reception pour les commandes abonnement
    /// </summary>
    public interface IReceptionsAutomaticGenerator : IService
    {
        /// <summary>
        /// Génération automatique des réceptions des commandes abonnement
        /// </summary>
        void GenerateReceptions();
    }
}
