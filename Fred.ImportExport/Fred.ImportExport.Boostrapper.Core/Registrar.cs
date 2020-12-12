using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Unity;
using Unity.Lifetime;

namespace Fred.ImportExport.Boostrapper.Core
{
    public static class Registrar
    {
        public static void RegisterUnitOfWork(IUnityContainer container)
        {
            #region Unit of Work

            //TransientLifetimeManager: Creates a new object of requested type every time you call Resolve or ResolveAll method.
            container.RegisterType<DbContext, FredDbContext>(new TransientLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new TransientLifetimeManager());

            #endregion
        }
    }
}
