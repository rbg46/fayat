using Fred.Entities.Carburant;
using System;
using System.Collections.Generic;

namespace Fred.Business.Carburant
{
  /// <summary>
  ///   Gestionnaire des Prix des carburant par organisation et par devise.
  /// </summary>
  public interface IParametrageCarburantManager : IManager<CarburantOrganisationDeviseEnt>
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
    ///   Suppression d'un paramétrage carburant.
    /// </summary>
    /// <param name="paramCarburant">Relation Carburant/Organisation/Devise à supprimer</param>    
    void DeleteParametrageCarburant(CarburantOrganisationDeviseEnt paramCarburant);

    /// <summary>
    ///   Gestion d'une liste de paramétrage carburant (Ajout, Modification ou Suppression)
    /// </summary>
    /// <param name="parametrageCarburantList">Liste de paramétrage carburant</param>
    /// <returns>Liste de paramétrage carburant à jour</returns>
    IEnumerable<CarburantOrganisationDeviseEnt> ManageParametrageCarburant(IEnumerable<CarburantOrganisationDeviseEnt> parametrageCarburantList);

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
    /// <param name="periode">(Optionnel) Date de début de période</param>   
    /// <returns>CarburantOrganisationDeviseEnt recherché</returns>
    CarburantOrganisationDeviseEnt GetParametrageCarburant(int carburantId, int organisationId, int deviseId, DateTime? periode = default(DateTime?));
  }
}