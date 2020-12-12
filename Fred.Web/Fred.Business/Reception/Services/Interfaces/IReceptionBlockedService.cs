using Fred.Entities.Depense;
using System.Collections.Generic;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Gere les receptions qui sont bloquées 'en reception'
    /// </summary>
    public interface IReceptionBlockedService : IService
    {
        /// <summary>
        ///   Détermine s'il y a au moins une réception dont la date est comprise dans une période bloquée en réception
        /// </summary>
        /// <param name="receptions">Liste des réceptions</param>
        /// <returns>Vrai s'il existe une réception bloquée, sinon faux</returns>
        bool CheckAnyReceptionsIsBlocked(List<DepenseAchatEnt> receptions);

        /// <summary>
        /// Met a jours la dateComptable d'une liste de receptions.
        /// Si la ci est cloturé, alors c'est le prochain mois dipsonible qui est pris.
        /// </summary>
        /// <param name="receptions">receptions a mettre a jour</param>
        void SetDateComtapleOfReceptions(List<DepenseAchatEnt> receptions);
    }
}
