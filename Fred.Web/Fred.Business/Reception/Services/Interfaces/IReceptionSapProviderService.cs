using System.Collections.Generic;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.FredIe
{
    /// <summary>
    /// Service qui fournie les Receptions a envoyé a sap
    /// </summary>
    public interface IReceptionSapProviderService : IService
    {
        /// <summary>
        /// Permet de récupérer une liste de réceptions en fonction des identifiants filtrés pour sap
        /// </summary>
        /// <param name="receptionIds">La liste des identifants à récupérer</param>
        /// <returns>Une liste de dépenses.</returns>
        IEnumerable<DepenseAchatEnt> GetReceptionsFilteredForSap(List<int> receptionIds);

        /// <summary>
        /// Permet de récupérer une liste de réceptions en fonction des identifiants
        /// </summary>
        /// <param name="receptionIds">La liste des identifants à récupérer</param>
        /// <returns>Une liste de dépenses.</returns>
        IEnumerable<DepenseAchatEnt> GetReceptions(List<int> receptionIds);

        /// <summary>
        ///  Mise à jour d'une liste de réceptions sans passer par la validation 
        /// </summary>
        /// <param name="receptions">Liste de réceptions à mettre à jour</param>
        /// <param name="auteurModificationId">Identifaint de l'utilisateur effectuant la modification</param>
        void UpdateAndSavesWithoutValidation(IEnumerable<DepenseAchatEnt> receptions, int? auteurModificationId);

        /// <summary>
        /// Recupere une DepenseAchatEnt par sont Id
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>une DepenseAchatEnt</returns>
        DepenseAchatEnt FindById(int rapportId);
        IEnumerable<DepenseAchatEnt> GetReceptionsPositivesAndNegativesFilteredForSap(List<int> receptionIds);
    }
}
