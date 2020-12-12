using System;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Import;
using Fred.Framework.Exceptions;

namespace Fred.Business.Import
{
    /// <summary>
    /// Gestionnaire des  des systèmes d'import.
    /// </summary>
    public class SystemeImportManager : Manager<SystemeImportEnt, ISystemeImportRepository>, ISystemeImportManager
    {
        public SystemeImportManager(IUnitOfWork uow, ISystemeImportRepository systemeImportRepository)
        : base(uow, systemeImportRepository)
        { }

        /// <inheritdoc />
        public SystemeImportEnt GetSystemeImport(string code)
        {
            try
            {
                return Repository.Query().Get()
                                 .FirstOrDefault(x => x.Code == code);

            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
