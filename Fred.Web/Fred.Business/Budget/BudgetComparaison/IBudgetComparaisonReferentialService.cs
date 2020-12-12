using Fred.DataAccess.Interfaces;

namespace Fred.Business.Budget.BudgetComparaison
{
    /// <summary>
    /// Interface du service d'accès aux référentiels.
    /// </summary>
    public interface IBudgetComparaisonReferentialService : IService
    {
        /// <summary>
        /// Gestionnaire des tâches.
        /// </summary>
        ITacheRepository TacheRepository { get; }

        /// <summary>
        /// Gestionnaire des chapitres.
        /// </summary>
        IChapitreRepository ChapitreRepository { get; }

        /// <summary>
        /// Gestionnaire des sous-chapitres.
        /// </summary>
        ISousChapitreRepository SousChapitreRepository { get; }

        /// <summary>
        /// Gestionnaire des ressources.
        /// </summary>
        IRessourceRepository RessourceRepository { get; }
    }
}
