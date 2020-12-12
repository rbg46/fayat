using System;
using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Personnel.Interimaire;
using Fred.Web.Shared.Models.Personnel.Interimaire;

namespace Fred.Business.Personnel.Interimaire
{
    /// <summary>
    ///   Gestionnaire des contrats d'intérimaires import
    /// </summary>
    public interface IContratInterimaireImportManager : IManager<ContratInterimaireImportEnt>
    {
        /// <summary>
        /// Permet d'ajouter un contrat à un personnel intérimaire import list
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant contrat inetrimaire</param>
        /// <param name="timestamp">Timestamp import</param>
        /// <param name="importList">List des message d'import</param>
        /// <returns>Le contrat intérimaire enregistré</returns>
        IEnumerable<ContratInterimaireImportEnt> AddContratInterimaireImportList(int contratInterimaireId, ulong timestamp, List<string> importList);
    }
}
