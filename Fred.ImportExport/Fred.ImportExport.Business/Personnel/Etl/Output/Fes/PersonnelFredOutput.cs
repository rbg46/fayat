using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Personnel;
using Fred.Entities.Personnel.Import;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;

namespace Fred.ImportExport.Business.Personnel.Etl.Output
{
    /// <summary>
    /// Pour FES et FON
    /// </summary>
    public class PersonnelFredOutput : IEtlOutput<PersonnelAffectationResult>
    {
        private readonly IPersonnelManager personnelManager;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="personnelManager">Gestionnaire des personnels</param>      
        public PersonnelFredOutput(IPersonnelManager personnelManager)
        {
            this.personnelManager = personnelManager;
        }

        /// <summary>
        /// Appelé par l'ETL
        /// Envoie les personnels à Fred.
        /// Cette sortie mettra à jour le personnel, le manager et les affectations.
        /// </summary>
        /// <param name="result">liste des personnels à envoyer à Fred</param>
        public async Task ExecuteAsync(IEtlResult<PersonnelAffectationResult> result)
        {
            await Task.Run(() =>
            {
                // Import vers FRED       
                personnelManager.ImportPersonnelsAffectations(result.Items.ToList());
            });
        }
    }
}
