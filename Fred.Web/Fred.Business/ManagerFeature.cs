using Fred.DataAccess.Interfaces;
namespace Fred.Business
{
    /// <summary>
    /// Implémentation d'une sous-fonctionnalité d'un manager qui necessite l'accès au repository
    /// Permet d'implémenter les algorithmes d'un manager dans des classes.
    /// Permet donc de réduire la taille du manager et favorise la POO.
    /// Basé sur le Design Pattern Impl en c++, ou un dérivé du Design Pattern Commande)
    /// 
    /// Pour la doc voir le fichier HowToWriteAManager.md
    /// 
    /// </summary>
    /// <typeparam name="TRepository">Le repo</typeparam>
    public abstract class ManagerFeature<TRepository> : IManagerFeature where TRepository : IRepository
    {
        private readonly IUnitOfWork uow;

        protected TRepository Repository { get; }

        protected ManagerFeature(IUnitOfWork uow, TRepository repository)
        {
            this.uow = uow;

            Repository = repository;
        }

        protected void Save()
        {
            uow.Save();
        }
    }
}
