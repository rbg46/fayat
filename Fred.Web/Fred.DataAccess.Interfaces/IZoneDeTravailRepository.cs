
using System.Collections.Generic;
using Fred.Entities.Personnel.Interimaire;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Référentiel de données pour les zones de travail 
    /// </summary>
    public interface IZoneDeTravailRepository : IRepository<ZoneDeTravailEnt>
    {

        /// <summary>
        /// Permet de récupérer une liste des zones de travail en fonction d'un contrat id
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique du contrat</param>
        /// <returns>Liste des zones de travail</returns>
        List<ZoneDeTravailEnt> GetZoneDeTravailByContratId(int contratInterimaireId);

        /// <summary>
        /// Permet d'ajouter un zone de travail
        /// </summary>
        /// <param name="etablissementComptableId">Identifiant unique d'un établissement comptable</param>
        /// <param name="contratInterimaireId">Identifiant unique d'un Contrat Intérimaire</param>
        /// <returns>Zone de Travail enregistré</returns>
        ZoneDeTravailEnt AddZoneDeTravail(int? etablissementComptableId, int contratInterimaireId);

        /// <summary>
        /// Permet de supprimer un contrat d'intérimaire en fonction de son identifiant
        /// </summary>
        /// <param name="zoneDeTravailEnt">Zone de travail</param>
        void DeleteZoneDeTravail(ZoneDeTravailEnt zoneDeTravailEnt);

        /// <summary>
        /// Retourne les organisations (Etablissement) appartenant lié à un contrat intérimaire pour picklist
        /// </summary>
        /// <param name="contratInterimaireId">identifiant unique du contrat intérimaire</param>
        /// <returns>Liste des zones de travail appartenant un contrat intérimaire</returns>
        IEnumerable<ZoneDeTravailEnt> SearchLightForContratInterimaireId(int contratInterimaireId);

        /// <summary>
        /// Permet de supprimer les zone de travail
        /// </summary>
        /// <param name="contratInterimaireId">Liste des zones de travail</param>
        void DeleteZonesDeTravailList(List<ZoneDeTravailEnt> zonesDeTravail);

        /// <summary>
        /// Permet de récupérer une liste des zones de travail en fonction d'un contrat id
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique du contrat</param>
        /// <returns>Liste des zones de travail sans objets complexes</returns>
        List<ZoneDeTravailEnt> GetOnlyZonesDeTravailListByContratId(int contratInterimaireId);
    }
}
