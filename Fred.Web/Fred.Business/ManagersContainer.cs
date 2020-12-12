using CommonServiceLocator;

namespace Fred.Business
{
    /// <summary>
    /// Représente un conteneur de gestionnaire.
    /// </summary>
    public class ManagersContainer
    {
        /// <summary>
        /// Retoune un manager du type souhaité.
        /// </summary>
        /// <typeparam name="TManager">Le type du manager.</typeparam>
        /// <param name="manager">Le membre a utiliser pour le lazy loading.</param>
        /// <returns>Le manager du type souhaité.</returns>
        protected TManager LazyGetManager<TManager>(ref TManager manager)
        //where TManager : IManager
        {
            manager = ServiceLocator.Current.GetInstance<TManager>();
            return manager;
        }
    }
}
