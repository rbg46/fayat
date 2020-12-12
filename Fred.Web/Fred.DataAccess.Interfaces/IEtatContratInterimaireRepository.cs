using System.Collections.Generic;
using Fred.Entities.Personnel.Interimaire;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données des états contrat d'intérimaire
    /// </summary>
    public interface IEtatContratInterimaireRepository : IRepository<EtatContratInterimaireEnt>
    {
        /// <summary>
        /// Récupére la liste des états d'un contrat intérimaire.
        /// </summary>
        /// <returns>List des état d'un contrat intérimaire.</returns>
        IEnumerable<EtatContratInterimaireEnt> GetEtatContratInterimaireList();
    }
}
