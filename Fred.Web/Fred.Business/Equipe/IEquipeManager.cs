using Fred.Entities.Affectation;
using Fred.Entities.Personnel;
using Fred.Web.Shared.Models.CI;
using Fred.Web.Shared.Models.Personnel;
using System;
using System.Collections.Generic;

namespace Fred.Business.Equipe
{
  /// <summary>
  /// Gestionnaire des equipe
  /// </summary>
  public interface IEquipeManager : IManager<EquipeEnt>
  {
    /// <summary>
    /// Add une equipe favorite
    /// </summary>
    /// <returns>Id de l'equipe</returns>
    int? CreateEquipe();

    /// <summary>
    /// Ajouter des personnels a une equipe
    /// </summary>
    /// <param name="personnelsId">List des personnels</param>
    void AddPersonnelsToEquipeFavorite(IList<int> personnelsId);

    /// <summary>
    /// Manage les personnels d'une equipe
    /// </summary>
    /// <param name="personnelsIdToAdd">List des identifiers des personnels a jouter</param>
    /// <param name="personnelsIdToDelete">List des identifier des personnels a supprimer</param>
    void ManageEquipePersonnels(List<int> personnelsIdToAdd, List<int> personnelsIdToDelete);

    /// <summary>
    /// Suppression des personnels d'un equipe
    /// </summary>
    /// <param name="personnelsId">List des personnels</param>
    void DeletePersonnelsEquipe(IList<int> personnelsId);

    /// <summary>
    /// Get une equipe par le proprietaire identifier
    /// </summary>
    /// <returns>Aquipe entity</returns>
    IEnumerable<PersonnelEnt> GetEquipePersonnelsByProprietaireId();

    /// <summary>
    /// Get equipe by proprietaire identifier
    /// </summary>
    /// <param name="proprietaireId">Proprietaire identifier</param>
    /// <returns>Equipe entity object</returns>
    EquipeEnt GetEquipeByProprietaireId(int proprietaireId);

    /// <summary>
    /// Retourne la liste des personnel dont l'utilisateur en cours est responsable par statut de personnel .
    /// </summary>
    /// <param name="personnelStatut">Personnel statut</param>
    /// <param name="mondayDate">Date du lundi</param>
    /// <returns>IEnumerable of PersonnelSummaryPointageModel</returns>
    IEnumerable<PersonnelSummaryPointageModel> GetResponsablePersonnelListSummary(string personnelStatut, DateTime mondayDate);

    /// <summary>
    /// Retourne la liste des ci d'un responsable donné
    /// </summary>
    /// <param name="personnelStatut">Personnel statut</param>
    /// <param name="mondayDate">Date du lundi</param>
    /// <returns>Une liste de CiPointageSummary</returns>
    IEnumerable<CiSummaryPointageModel> GetResponsableCiListSummary(string personnelStatut, DateTime mondayDate);

    /// <summary>
    /// Retourne la liste des ouvriers et Cis pour le pointage d'un utilisateur donné
    /// </summary>
    /// <param name="personnelStatut">Personnel statut</param>
    /// <param name="mondayDate">Date du lundi</param>
    /// <returns>Rapport hebdo entree summary</returns>
    RapportHebdoEntreeSummary GetUserPointageHebdoSummary(string personnelStatut, DateTime mondayDate);
  }
}
