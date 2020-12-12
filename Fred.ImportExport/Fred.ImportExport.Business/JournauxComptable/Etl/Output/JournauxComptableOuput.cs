using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.Journal;
using Fred.Entities.Journal;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;

namespace Fred.ImportExport.Business.JournauxComptable.Etl.Output
{
    /// <summary>
    /// Processus etl : Execution de la sortie de l'import des Journaux Comptables
    /// Envoie dans Fred les Journaux Comptable
    /// </summary>
    internal class JournauxComptableOuput : IEtlOutput<JournalEnt>
    {
        private readonly IJournalManager journalComptableManager;

        public JournauxComptableOuput(IJournalManager journalComptableManager)
        {
            this.journalComptableManager = journalComptableManager;
        }

        /// <summary>
        /// Appelé par l'ETL
        /// Envoie les Journaux Comptable à Fred
        /// </summary>
        /// <param name="result">liste des Journaux Comptable à envoyer à Fred</param>
        public async Task ExecuteAsync(IEtlResult<JournalEnt> result)
        {
            await Task.Run(() =>
            {
                IList<JournalEnt> list = result.Items;
                if (list.Count != 0)
                {
                    journalComptableManager.ManageImportedJournauxComptables(list, list[0].SocieteId);
                }
            });
        }
    }
}
