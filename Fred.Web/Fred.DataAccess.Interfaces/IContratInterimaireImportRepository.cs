using System.Collections.Generic;
using Fred.Entities.Personnel.Interimaire;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données des contrat d'intérimaire import
    /// </summary>
    public interface IContratInterimaireImportRepository : IRepository<ContratInterimaireImportEnt>
    {

        /// <summary>
        /// Permet d'ajouter un contrat à un personnel intérimaire import liste
        /// </summary>
        /// <param name="contratInterimaireImportEntList">Liste de contrat Intérimaire import</param>
        /// <returns>Le contrat intérimaire import enregistré</returns>
        IEnumerable<ContratInterimaireImportEnt> AddContratInterimaireImportList(IEnumerable<ContratInterimaireImportEnt> contratInterimaireImportEntList);

        IEnumerable<ContratInterimaireImportEnt> GetByContratInterimaireIdAndTimestamp(int contratInterimaireId, ulong timestamp);
    }
}
