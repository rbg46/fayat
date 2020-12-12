using System;
using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Personnel.Interimaire;
using Fred.Web.Shared.Models.Personnel.Interimaire;

namespace Fred.Business.Personnel.Interimaire
{
    /// <summary>
    ///   Gestionnaire des contrats d'intérimaires
    /// </summary>
    public interface IEtatContratInterimaireManager : IManager<EtatContratInterimaireEnt>
    {

        /// <summary>
        /// Récupére la liste des états d'un contrat intérimaire.
        /// </summary>
        /// <returns>List des état d'un contrat intérimaire.</returns>
        IEnumerable<EtatContratInterimaireEnt> GetEtatContratInterimaireList();

        /// <summary>
        /// Récupére un état contrat intérimaire pa code
        /// </summary>
        /// <param name="code">Code état contrat intérimaire</param>
        /// <returns>Létat du contrat intérimaire</returns>
        EtatContratInterimaireEnt GetEtatContratInterimaireByCode(string code);
    }
}
