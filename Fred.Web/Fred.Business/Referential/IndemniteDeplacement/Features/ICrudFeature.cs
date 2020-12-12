using Fred.Entities.IndemniteDeplacement;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.IndemniteDeplacement
{

  /// <summary>
  /// Fonctionnalit� Create Read Update Delete des indemnit�s de d�placement
  /// </summary>
  public interface ICrudFeature
  {
    /// <summary>
    ///   Retourne la liste de tous les indemnites de deplacement.
    /// </summary>
    /// <returns>List de toutes les soci�t�s</returns>
    IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementListAll();

    /// <summary>
    ///   La liste de tous les indemnites de deplacement.
    /// </summary>
    /// <returns>Renvoie la liste de des indemnites de deplacement active</returns>
    IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementList();

    /// <summary>
    ///   La liste de tous les indemnites de deplacement par personnel.
    /// </summary>
    /// <param name="personnelID">Id unique d'un personnel</param>
    /// <returns>Renvoie la liste de des indemnites de deplacement active</returns>
    IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelListAll(int personnelID);

    /// <summary>
    ///   La liste de tous les indemnites de deplacement.
    /// </summary>
    /// <param name="personnelID">Id unique d'un personnel</param>
    /// <returns>Renvoie la liste de des indemnites de deplacement active par personnel</returns>
    IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelList(int personnelID);

    /// <summary>
    ///   La liste de tous les indemnites de deplacement pour un personnel.
    /// </summary>
    /// <param name="personnelID">Id unique d'un personnel</param>
    /// <returns>Renvoie la liste de des indemnites de deplacement pour un personnel</returns>
    IQueryable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnel(int personnelID);

    /// <summary>
    ///   La liste de tous les indemnites de deplacement pour un personnel et une affaire.
    /// </summary>
    /// <param name="personnelID">Id unique d'un personnel</param>
    /// <param name="ciId">Id unique d'une affaire</param>
    /// <returns>Renvoie la liste de des indemnites de deplacement pour un personnel et une affaire</returns>
    IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCi(int personnelID, int ciId);

    /// <summary>
    ///   Retourne l'indemnites de deplacement pour un personnel et une affaire.
    /// </summary>
    /// <param name="personnelId">Id unique d'un personnel</param>
    /// <param name="ciId">Id unique d'une affaire</param>
    /// <returns>Renvoie une indemnite de deplacement pour un personnel et une affaire</returns>
    IndemniteDeplacementEnt GetIndemniteDeplacementByPersonnelIdAndCiId(int personnelId, int ciId);

    /// <summary>
    ///   La liste de tous les indemnites de deplacement par CI.
    /// </summary>
    /// <param name="idCI">id unique d'un CI</param>
    /// <returns>Renvoie la liste de des indemnites de deplacement active</returns>
    IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCIListAll(int idCI);

    /// <summary>
    ///   La liste de tous les indemnites de deplacement active par CI.
    /// </summary>
    /// <param name="idCI">id unique d'un CI</param>
    /// <returns>Renvoie la liste de des indemnites de deplacement active</returns>
    IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCIList(int idCI);

    /// <summary>
    ///   Sauvegarde les modifications d'un IndemniteDeplacement
    /// </summary>
    /// <param name="indemniteDeplacement">Indemnite de deplacement � modifier</param>
    /// <returns>int</returns>
    int UpdateIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacement);

    /// <summary>
    ///   Permet de sauvegarder une ind�mnit� de d�placement sans perdre l'historique.
    /// </summary>
    /// <param name="indDepNew">Ind�mnit� � ajouter</param>
    /// <param name="indDepOld">Ind�mnit� � supprimer physiquement</param>
    /// <param name="toRemove">toRemove</param>
    /// <returns>int</returns>
    int UpdateIndemniteDeplacementWithHistorical(IndemniteDeplacementEnt indDepNew, IndemniteDeplacementEnt indDepOld = null, bool toRemove = false);

    /// <summary>
    ///   Ajout d'une indemnite de deplacement
    /// </summary>
    /// <param name="indemniteDeplacement">Indemnite de deplacement � ajouter</param>
    /// <returns>L'identifiant du Indemnite de deplacement ajout�</returns>
    int AddIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacement);

    /// <summary>
    ///   Supprime un Indemnite de deplacement
    /// </summary>
    /// <param name="id">L'identifiant du Indemnite de deplacement � supprimer</param>    
    void DeleteIndemniteDeplacementById(int id);

    /// <summary>
    ///   Indemnite de deplacement via l'id
    /// </summary>
    /// <param name="id">Id du Indemnite de deplacement</param>
    /// <returns>Renvoie un Indemnite de deplacement</returns>
    IndemniteDeplacementEnt GetIndemniteDeplacementById(int id);

    /// <summary>
    ///   Indemnite Deplacement via personnelId
    /// </summary>
    /// <param name="personnelId">identifiant unique du personnel</param>
    /// <returns>Renvoie une indemnite de deplacement</returns>
    IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelId(int personnelId);

    /// <summary>
    ///   Permet l'initialisation d'une nouvelle instance de Indemnite de deplacement.
    /// </summary>
    /// <returns>Nouvelle instance de Indemnite de deplacement intialis�e</returns>
    IndemniteDeplacementEnt GetNewIndemniteDeplacement();



    /// <summary>
    ///   Import des indemnites de deplacement automatiques depuis la Holding
    /// </summary>
    /// <param name="holdingId"> Id du Holding</param>
    /// <returns>Renvoie int</returns>
    int ImportIndemniteDeplacementFromHolding(int holdingId);

    /// <summary>
    /// Permet de v�rifier si l'ind�mnit� de d�placement est unique pour un personnel et un ci donn�
    /// L'unicit� est v�rifi�e sur les ind�mnit�s ACTIVES (non supprim�es logiquement)
    /// </summary>
    /// <param name="personnelId">Id du personnel</param>
    /// <param name="ciId">Id du CI</param>
    /// <param name="indId">indId</param>
    /// <returns>True si l'ind�mnit� est unique</returns>
    bool IsIndemniteDeplacementUnique(int personnelId, int ciId, int indId = 0);
  }
}