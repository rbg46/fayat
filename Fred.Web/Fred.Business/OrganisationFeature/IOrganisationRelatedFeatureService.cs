namespace Fred.Business.OrganisationFeature
{
    /// <summary>
    /// Service de récuperation de l'état (activé ou désactivé) d'une feature par rapport à une organisation
    /// </summary>
    public interface IOrganisationRelatedFeatureService : IService
    {
        /// <summary>
        /// récuperation de l'état (activé ou désactivé) d'une feature par rapport à l'organisation de l'utilisateur courant
        /// </summary>
        /// <param name="featureKey">la clé de la feature</param>
        /// <param name="defaultValue">la valeur par défaut à retourner si aucune clé n'est présente</param>
        /// <returns>Activé ou désactivée</returns>
        bool IsEnabledForCurrentUser(string featureKey, bool defaultValue);
    }
}
