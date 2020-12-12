using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fred.Framework.Exceptions;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Database.ImportExport;
using Fred.ImportExport.Entities.Transposition;

namespace Fred.ImportExport.DataAccess.Transposition
{
    public class TranspoCodeEmploiToRessourceRepository : AbstractRepository<TranspoCodeEmploiToRessourceEnt>, ITranspoCodeEmploiToRessourceRepository
    {
        public TranspoCodeEmploiToRessourceRepository(ImportExportContext context)
          : base(context)
        {
        }

        public IEnumerable<TranspoCodeEmploiToRessourceEnt> GetList(string codeSociete)
        {
            try
            {
                return Get().Where(x => x.CodeSocieteImport == codeSociete).AsNoTracking();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        public TranspoCodeEmploiToRessourceEnt GetByCodeSocieteAndCodeEmploi(string codeSociete, string codeEmploi)
        {
            try
            {
                return Get().Where(x => x.CodeSocieteImport == codeSociete && x.CodeEmploi.Trim() == codeEmploi.Trim()).AsNoTracking().FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }
    }
}
