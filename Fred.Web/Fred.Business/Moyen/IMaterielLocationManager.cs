using System.Collections.Generic;
using Fred.Entities.Moyen;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Business.Moyen
{
    /// <summary>
    /// Gestionnaire des locations des moyens
    /// </summary>
    public interface IMaterielLocationManager : IManager<MaterielLocationEnt>
    {
        /// <summary>
        /// Add materiel location
        /// </summary>
        /// <param name="materielLocationEnt">Materiel location to add</param>
        /// <returns>Identifiant du materiel location ajouté</returns>
        int AddMaterielLocation(MaterielLocationEnt materielLocationEnt);

        /// <summary>
        /// Update d'un matériel de type location
        /// </summary>
        /// <param name="materielLocation">Matériel de type location</param>
        /// <returns>Identifiant du matériel modifé</returns>
        int UpdateMaterielLocation(MaterielLocationEnt materielLocation);

        /// <summary>
        /// Chercher des immatriculations des moyens en fonction des critéres fournies en paramètres 
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="chapitresIds">les Ids des chapitres moyens </param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des moyens</returns>
        IEnumerable<MoyenImmatriculationModel> SearchLightForImmatriculation(SearchImmatriculationMoyenEnt filters, IEnumerable<int> chapitresIds, int page = 1, int pageSize = 20);

        /// <summary>
        /// Retourne toutes les locations qui ont une date de suppression null 
        /// </summary>
        /// <returns>List des Locations</returns>
        IEnumerable<MaterielLocationModelFull> GetAllActiveLocation();

        /// <summary>
        /// Supprimer une location 
        /// </summary>
        /// <param name="materielLocationId">L'id du materiel a supprimer</param>
        void DeleteMaterielLocation(int materielLocationId);
    }
}
