using Fred.Entities.Organisation;
using Fred.Entities.Personnel.Interimaire;
using System.Collections.Generic;

namespace Fred.Business.Personnel.Interimaire
{
    /// <summary>
    ///   Gestionnaire des zones de travail
    /// </summary>
    public interface IZoneDeTravailManager : IManager<ZoneDeTravailEnt>
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
        /// <param name="zoneDeTravailEnt">Zone de travail</param>
        /// <returns>Zone de Travail enregistré</returns>
        ZoneDeTravailEnt AddZoneDeTravail(ZoneDeTravailEnt zoneDeTravailEnt);

        /// <summary>
        /// Permet de supprimer un contrat d'intérimaire en fonction de son identifiant
        /// </summary>
        /// <param name="zoneDeTravailEnt">Zone de travail</param>
        void DeleteZoneDeTravail(ZoneDeTravailEnt zoneDeTravailEnt);


        /// <summary>
        /// Permet de supprimer les zone de travail d'un contrat interimaire
        /// </summary>
        /// <param name="contratInterimaireId">Contrat interimaire Id</param>
        void DeleteZonesDeTravailByContratInterimaire(int contratInterimaireId);

        /// <summary>
        /// Retourne les organisations (Etablissement et Ci) appartenant lié à un contrat intérimaire pour picklist
        /// </summary>
        /// <param name="contratInterimaireId">identifiant unique du contrat intérimaire</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">taille de la page</param>
        /// <param name="recherche">texte de recherche</param>
        /// <returns>Liste des organisations appartenant à un contrat intérimaire</returns>
        IEnumerable<OrganisationEnt> SearchLightForContratInterimaireId(int contratInterimaireId, int page = 1, int pageSize = 20, string recherche = "");

    }
}
