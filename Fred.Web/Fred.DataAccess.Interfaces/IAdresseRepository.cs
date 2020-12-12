using Fred.Entities.Adresse;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données pour les adresses.
    /// </summary>
    public interface IAdresseRepository : IFredRepository<AdresseEnt>
    {
        /// <summary>
        /// Mise à jour d'une Adresse
        /// </summary>
        /// <param name="adresseId">id Adresse</param>
        /// <param name="adresseUpdate">Adresse  à mettre  à jour</param>
        void UpdateAdresse(int adresseId, AdresseEnt adresseUpdate);
    }
}
