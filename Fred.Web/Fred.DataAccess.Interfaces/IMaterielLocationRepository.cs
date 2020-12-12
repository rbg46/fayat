
using System.Collections.Generic;
using Fred.Entities.Moyen;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les matériels de type location
    /// </summary>
    public interface IMaterielLocationRepository : IRepository<MaterielLocationEnt>
    {
        /// <summary>
        ///   Ajout d'un matériel de type location
        /// </summary>
        /// <param name="materielLocation">Matériel de type location</param>
        /// <returns>Identifiant du matériel ajouté</returns>
        int AddMaterielLocation(MaterielLocationEnt materielLocation);

        /// <summary>
        /// Update d'un matériel de type location
        /// </summary>
        /// <param name="materielLocation">Matériel de type location</param>
        /// <returns>Identifiant du matériel modifié</returns>
        int UpdateMaterielLocation(MaterielLocationEnt materielLocation);

        /// <summary>
        /// Chercher des immatriculations des moyens en fonction des critéres fournies en paramètres 
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>        
        /// <param name="chapitresIds">les Ids des chapitres moyens </param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des moyens</returns>
        IEnumerable<MaterielLocationEnt> SearchLightForImmatriculation(SearchImmatriculationMoyenEnt filters, IEnumerable<int> chapitresIds, int page = 1, int pageSize = 20);

        /// <summary>
        /// Retourne toutes les locations qui ont une date de suppression null 
        /// </summary>
        /// <returns>Retourne un enumerable des Locations</returns>
        IEnumerable<MaterielLocationEnt> GetAllActiveLocation();

        /// <summary>
        /// Supprimer une location 
        /// </summary>
        /// <param name="materielLocationId">L'id du materiel a supprimer</param>
        void DeleteMaterielLocation(int materielLocationId);
    }
}
