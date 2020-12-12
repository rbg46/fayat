using System.Collections.Generic;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Web.Models;

namespace Fred.Business.Reception.Services
{
    public interface IReceptionExportExcelService : IService
    {
        IEnumerable<DepenseAchatEnt> TransformCommandesToDepensesForExport(IEnumerable<CommandeEnt> commandes);

        string CustomizeExcelFileForExport(string path, IEnumerable<ReceptionExportModel> receptions);

        byte[] GetReceptionsExcel(SearchDepenseEnt filtre);

        string GetReceptionsFilename();
    }
}
