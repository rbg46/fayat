using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Adresse;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.Referential.Adresse
{
    /// <summary>
    ///   Référentiel de données pour les adresses.
    /// </summary>
    public class AdresseRepository : FredRepository<AdresseEnt>, IAdresseRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="AdresseRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        public AdresseRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Mise à jour d'une Adresse
        /// </summary>
        /// <param name="adresseId">id Adresse</param>
        /// <param name="adresseUpdate">Adresse  à mettre  à jour</param>
        public void UpdateAdresse(int adresseId, AdresseEnt adresseUpdate)
        {
            Update(GetAndUpdate(adresseId, adresseUpdate));
        }

        /// <summary>
        /// Récupérer et ajouter une adresse
        /// </summary>
        /// <param name="adresseId">Identifiant de l'adresse</param>
        /// <param name="adresseUpdate">Adresse à mettre à jour</param>
        /// <returns>Adresse à jour</returns>
        private AdresseEnt GetAndUpdate(int adresseId, AdresseEnt adresseUpdate)
        {
            var entity = Context.Adresses.Find(adresseId);

            entity.Ligne = adresseUpdate.Ligne;
            entity.CodePostal = adresseUpdate.CodePostal;
            entity.Ville = adresseUpdate.Ville;
            entity.PaysId = adresseUpdate.PaysId;

            return entity;
        }
    }
}
