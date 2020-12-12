using System;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Import;
using Fred.Framework.Exceptions;

namespace Fred.Business.Import
{
    /// <summary>
    /// Gestionnaire des  des systèmes externes.
    /// </summary>
    public class SystemeExterneManager : Manager<SystemeExterneEnt, ISystemeExterneRepository>, ISystemeExterneManager
    {
        public SystemeExterneManager(IUnitOfWork uow, ISystemeExterneRepository systemeExterneRepository)
        : base(uow, systemeExterneRepository)
        { }

        /// <inheritdoc />
        public SystemeExterneEnt GetSystemeExterne(string code)
        {
            try
            {
                return Repository.Query()
                  .Filter(si => si.Code == code)
                  .Get()
                  .FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
