using System.Collections.Generic;
using Fred.ImportExport.Entities.Transposition;

namespace Fred.ImportExport.DataAccess.Interfaces
{
    public interface ITranspoCodeEmploiToRessourceRepository
    {
        TranspoCodeEmploiToRessourceEnt GetByCodeSocieteAndCodeEmploi(string codeSociete, string codeEmploi);
        IEnumerable<TranspoCodeEmploiToRessourceEnt> GetList(string codeSociete);
    }
}
