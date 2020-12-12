using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel.Interimaire;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Personnel.Interimaire
{
    /// <summary>
    ///   Référentiel de données pour les Contrats Interimaire 
    /// </summary>
    public class ContratInterimaireImportRepository : FredRepository<ContratInterimaireImportEnt>, IContratInterimaireImportRepository
    {
        public ContratInterimaireImportRepository(FredDbContext context)
          : base(context) { }


        /// <summary>
        /// Permet d'ajouter un contrat à un personnel intérimaire import liste
        /// </summary>
        /// <param name="contratInterimaireImportEntList">Liste de contrat Intérimaire import</param>
        /// <returns>Le contrat intérimaire import enregistré</returns>
        public IEnumerable<ContratInterimaireImportEnt> AddContratInterimaireImportList(IEnumerable<ContratInterimaireImportEnt> contratInterimaireImportEntList)
        {
            if (contratInterimaireImportEntList.Any())
            {
                foreach (var contratInterimaireImportEnt in contratInterimaireImportEntList)
                {
                    Insert(contratInterimaireImportEnt);
                }

                return contratInterimaireImportEntList;
            }

            return new List<ContratInterimaireImportEnt>();
        }

        public IEnumerable<ContratInterimaireImportEnt> GetByContratInterimaireIdAndTimestamp(int contratInterimaireId, ulong timestamp)
        {
            return Query()
                    .Filter(x => x.ContratInterimaireId == contratInterimaireId && x.TimestampImport == timestamp)
                    .Get()
                    .AsNoTracking();
        }
    }
}
