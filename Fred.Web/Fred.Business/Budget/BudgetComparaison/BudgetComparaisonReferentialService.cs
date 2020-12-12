using Fred.DataAccess.Interfaces;

namespace Fred.Business.Budget.BudgetComparaison
{
    /// <summary>
    /// Service d'accès aux référentiels.
    /// </summary>
    public class BudgetComparaisonReferentialService : IBudgetComparaisonReferentialService
    {
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="tacheRepository">Référentiel de données pour les tâches.</param>
        /// <param name="chapitreRepository">Référentiel de données pour les chapitres.</param>
        /// <param name="sousChapitreRepository">Référentiel de données pour les sous-chapitres.</param>
        /// <param name="ressourceRepository">Référentiel de données pour les ressources.</param>
        public BudgetComparaisonReferentialService(
            ITacheRepository tacheRepository,
            IChapitreRepository chapitreRepository,
            ISousChapitreRepository sousChapitreRepository,
            IRessourceRepository ressourceRepository)
        {
            TacheRepository = tacheRepository;
            ChapitreRepository = chapitreRepository;
            SousChapitreRepository = sousChapitreRepository;
            RessourceRepository = ressourceRepository;
        }

        /// <summary>
        /// Gestionnaire des tâches.
        /// </summary>
        public ITacheRepository TacheRepository { get; }

        /// <summary>
        /// Gestionnaire des chapitres.
        /// </summary>
        public IChapitreRepository ChapitreRepository { get; }

        /// <summary>
        /// Gestionnaire des sous-chapitres.
        /// </summary>
        public ISousChapitreRepository SousChapitreRepository { get; }

        /// <summary>
        /// Gestionnaire des ressources.
        /// </summary>
        public IRessourceRepository RessourceRepository { get; }
    }
}
