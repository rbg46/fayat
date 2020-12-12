
using System.Collections.Generic;
using Fred.Entities.Affectation;
using Fred.Entities.Personnel;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Equipe repository interface
    /// </summary>
    public interface IEquipeRepository : IRepository<EquipeEnt>
    {
        /// <summary>
        /// Creation d'une equipe
        /// </summary>
        /// <param name="equipe">equipe entity </param>
        /// <returns>l'id de l'equipe si l'insertion est bien sinn 0</returns>
        int? CreateEquipe(EquipeEnt equipe);

        /// <summary>
        /// Ajouter des personnels a une equipe
        /// </summary>
        /// <param name="equipeId">Equipe identifier</param>
        /// <param name="personnelsIds">List des personnels a ajouter dans l'equipe</param>
        void AddPersonnelsToEquipeFavorite(int equipeId, IList<int> personnelsIds);

        /// <summary>
        /// Suppression des personnels d'un equipe
        /// </summary>
        /// <param name="equipeId">Equipe identifier</param>
        /// <param name="equipePersonnels">List des personnels d'une equipe</param>
        void DeletePersonnelsEquipe(int equipeId, IEnumerable<EquipePersonnelEnt> equipePersonnels);

        /// <summary>
        /// Get une equipe par le proprietaire identifier
        /// </summary>
        /// <param name="proprietaireId">Proprietaire identifier</param>
        /// <returns>Aquipe entity</returns>
        IEnumerable<PersonnelEnt> GetEquipePersonnelsByProprietaireId(int proprietaireId);

        /// <summary>
        /// Get equipe personnel entity by equipe id aet personnel id 
        /// </summary>
        /// <param name="equipeId">Equipe identifier</param>
        /// <param name="peronnelId">Personnel identifier</param>
        /// <returns>Equipe personnel entity</returns>
        EquipePersonnelEnt GetEquipePersonnel(int equipeId, int peronnelId);

        /// <summary>
        /// Get equipe by proprietaire identifier
        /// </summary>
        /// <param name="proprietaireId">Proprietaire identifier</param>
        /// <returns>Equipe entity object</returns>
        EquipeEnt GetEquipeByProprietaireId(int proprietaireId);

        /// <summary>
        /// Check if the personnel is in the team
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="equipeId">Equipe identifier</param>
        /// <returns>True if personnel already in the team</returns>
        bool IsPersonnelInTeam(int personnelId, int equipeId);
    }
}
