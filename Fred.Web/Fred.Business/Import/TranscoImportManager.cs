using System;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Import;
using Fred.Framework.Exceptions;

namespace Fred.Business.Import
{
    /// <summary>
    /// Gestionnaire des  des transco d'import.
    /// </summary>
    public class TranscoImportManager : Manager<TranscoImportEnt, ITranscoImportRepository>, ITranscoImportManager
    {
        public TranscoImportManager(IUnitOfWork uow, ITranscoImportRepository transcoImportRepository)
        : base(uow, transcoImportRepository) { }

        /// <inheritdoc />
        public TranscoImportEnt GetTranscoImport(string codeExterne, int societeId, int systemeImportId)
        {
            try
            {
                return Repository.Query()
                  .Get()
                  .FirstOrDefault(x => x.CodeExterne == codeExterne && x.SocieteId == societeId && x.SystemeImportId == systemeImportId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
