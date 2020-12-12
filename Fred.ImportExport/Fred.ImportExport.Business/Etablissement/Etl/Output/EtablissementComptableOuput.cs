using System.Threading.Tasks;
using Fred.Business.Referential;
using Fred.Entities.Referential;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;

namespace Fred.ImportExport.Business.Etablissement.Etl.Output
{


    /// <summary>
    /// Processus etl : Execution de la sortie de l'import des Etablissements Comptables
    /// Envoie dans Fred les établissements
    /// </summary>
    internal class EtablissementComptableOuput : IEtlOutput<EtablissementComptableEnt>
    {
        private readonly IEtablissementComptableManager etsComptaManager;

        public EtablissementComptableOuput(IEtablissementComptableManager etsComptaManager)
        {
            this.etsComptaManager = etsComptaManager;
        }


        /// <summary>
        /// Appelé par l'ETL
        /// Envoie les établissements à Fred
        /// </summary>
        /// <param name="result">liste des établissements à envoyer à Fred</param>
        public async Task ExecuteAsync(IEtlResult<EtablissementComptableEnt> result)
        {
            await Task.Run(() =>
            {
                var list = result.Items;
                etsComptaManager.ManageImportedEtablissementComptables(list);
            });
        }
    }
}