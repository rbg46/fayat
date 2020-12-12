using System.Threading.Tasks;
using Fred.ImportExport.Business.CI.ImportMaterielEtl.Process;
using Fred.ImportExport.Models.Materiel;
namespace Fred.ImportExport.Business.Materiel.ImportMaterielEtl
{
    /// <summary>
    /// Cette classe est le manager qui permet l'import de Materiel dans Fred.
    /// </summary>
    public class ImportMaterielManager
    {

        public async Task ImportMaterielAsync(ImportMaterielModel materiel)
        {
            var importMaterielProcess = new ImportMaterielProcess();

            importMaterielProcess.Init(materiel);
            importMaterielProcess.Build();
            await importMaterielProcess.ExecuteAsync();
        }
    }
}
