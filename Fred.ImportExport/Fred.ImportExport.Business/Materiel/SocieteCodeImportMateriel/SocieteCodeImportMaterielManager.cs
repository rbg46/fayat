using System.Collections.Generic;
using System.Linq;
using Fred.Framework.Exceptions;
using Fred.ImportExport.DataAccess.Interfaces;

namespace Fred.ImportExport.Business.Materiel.SocieteCodeImportMateriel
{
    public class SocieteCodeImportMaterielManager : ISocieteCodeImportMaterielManager
    {
        private readonly ISocieteCodeImportMaterielRepository societeCodeImportMaterielRepository;

        public SocieteCodeImportMaterielManager(ISocieteCodeImportMaterielRepository societeCodeImportMaterielRepository)
        {
            this.societeCodeImportMaterielRepository = societeCodeImportMaterielRepository;
        }

        public List<string> GetAll(int groupCode)
        {
            try
            {
                return societeCodeImportMaterielRepository.GetList(groupCode).ToList();
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
