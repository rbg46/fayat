
using System;
using System.Collections.Generic;
using Fred.Entities.Carburant;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Référentiel de données pour les Budgets.
    /// </summary>
    public interface ICarburantOrganisationDeviseRepository : IFredRepository<CarburantOrganisationDeviseEnt>
    {
        /// <summary>
        ///   Ajout d'un paramétrage carburant.
        /// </summary>
        /// <param name="paramCarburant">Relation Carburant/Organisation/Devise à ajouter</param>
        /// <returns>Relation Carburant/Organisation/Devise créee</returns>
        CarburantOrganisationDeviseEnt AddParametrageCarburant(CarburantOrganisationDeviseEnt paramCarburant);

        /// <summary>
        ///   Mise à jour d'un paramétrage carburant.
        /// </summary>
        /// <param name="paramCarburant">Relation Carburant/Organisation/Devise à mettre à jour</param>
        /// <returns>Relation Relation Carburant/Organisation/Devise mise à jour</returns>
        CarburantOrganisationDeviseEnt UpdateParametrageCarburant(CarburantOrganisationDeviseEnt paramCarburant);

        /// <summary>
        ///   Récupération des paramétrages des prix des carburants en fonction de l'identifiant de l'organisation sur une période donnée (paramètre optionnel)
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation</param> 
        /// <returns>Liste des paramétrages des carburants</returns>
        IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantListByOrganisationId(int organisationId);

        /// <summary>
        ///   Récupération des paramétrages des prix des carburants en fonction de l'identifiant du carburant sur une période donnée (paramètre optionnel)
        /// </summary>
        /// <param name="carburantId">Identifiant du carburant</param>        
        /// <returns>Liste des paramétrages des carburants</returns>
        IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantListByCarburantId(int carburantId);

        /// <summary>
        ///   Récupération des paramétrages des prix des carburants en fonction de l'identifiant de l'organisation et de la devise sur une période donnée (paramètre optionnel)
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <param name="deviseId">Identifiant de la devise</param>    
        /// <returns>Liste des paramétrages des carburants</returns>
        IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantList(int organisationId, int deviseId);

        /// <summary>
        ///   Récupération des paramétrages des prix des carburants en fonction de l'identifiant du carburant, de l'organisation et de la devise sur une période donnée (paramètre optionnel)
        /// </summary>
        /// <param name="carburantId">Identifiant du carburant</param>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <returns>Liste des paramétrages des carburants</returns>
        IEnumerable<CarburantOrganisationDeviseEnt> GetParametrageCarburantList(int carburantId, int organisationId, int deviseId);

        /// <summary>
        ///   Récupération des paramétrages des prix des carburants en fonction de l'identifiant de l'organisation et de la devise sur une période donnée (paramètre optionnel) sous forme d'une liste de carburant
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <param name="periode">(Optionnel) Période d'application</param>
        /// <returns>Liste des paramétrages des carburants</returns>
        IEnumerable<CarburantEnt> GetParametrageCarburantListAsCarburantList(int organisationId, int deviseId, DateTime? periode = default(DateTime?));

        /// <summary>
        ///   Récupère un paramétrage de carburant en fonction de l'identifiant du carburant, de l'organisation et de la devise (periode optionnelle)
        /// </summary>
        /// <param name="carburantId">Identifiant du carburant</param>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <param name="periode">(Optionnel) Période d'application</param>   
        /// <returns>CarburantOrganisationDeviseEnt recherché</returns>
        CarburantOrganisationDeviseEnt GetParametrageCarburant(int carburantId, int organisationId, int deviseId, DateTime? periode = default(DateTime?));

    }
}