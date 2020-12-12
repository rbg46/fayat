using Fred.ImportExport.Models.Commande;

namespace Fred.ImportExport.Business.Commande
{
    public interface ICommandeStormImporter
    {
        void Import(CommandeSapModel model);
    }
}
