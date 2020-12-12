using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel.Interimaire;
using Fred.Framework.Exceptions;

namespace Fred.Business.Personnel.Interimaire
{
    /// <summary>
    ///   Gestionnaire des contrats d'intérimaires
    /// </summary>
    public class ContratInterimaireImportManager : Manager<ContratInterimaireImportEnt, IContratInterimaireImportRepository>, IContratInterimaireImportManager
    {
        public ContratInterimaireImportManager(IUnitOfWork uow, IContratInterimaireImportRepository contratInterimaireImportRepository)
         : base(uow, contratInterimaireImportRepository)
        {
        }


        /// <summary>
        /// Permet d'ajouter des messages d'import à un contrat intérimaire import list
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant contrat inetrimaire</param>
        /// <param name="timestamp">Timestamp import</param>
        /// <param name="importList">List des message d'import</param>
        /// <returns>Le contrat intérimaire enregistré</returns>
        public IEnumerable<ContratInterimaireImportEnt> AddContratInterimaireImportList(int contratInterimaireId, ulong timestamp, List<string> importList)
        {
            try
            {
                if (!importList.Any())
                {
                    return null;
                }

                var toAdd = new List<ContratInterimaireImportEnt>();
                foreach (var message in importList)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        toAdd.Add(new ContratInterimaireImportEnt()
                        {
                            ContratInterimaireId = contratInterimaireId,
                            TimestampImport = timestamp,
                            HistoriqueImport = message
                        });
                    }
                }
                Repository.AddContratInterimaireImportList(toAdd);
                Save();
                var import = toAdd.First();
                return Repository.GetByContratInterimaireIdAndTimestamp(import.ContratInterimaireId, import.TimestampImport.Value);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
