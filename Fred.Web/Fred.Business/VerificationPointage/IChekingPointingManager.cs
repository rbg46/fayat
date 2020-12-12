using Fred.Entities.VerificationPointage;

namespace Fred.Business.VerificationPointage
{
    /// <summary>
    ///   Inerface Gestionnaire Checking Pointing
    /// </summary>
    public interface IChekingPointingManager :IManager
    {
        /// <summary>
        /// génerer un  Fihier Excel en byte
        /// </summary>
        /// <param name="param">Liste des dépenses</param>
        /// <returns>une liste des données pour faire le rapporting</returns>
        byte[] ChekingPointing(FilterChekingPointing param);
    }
}
