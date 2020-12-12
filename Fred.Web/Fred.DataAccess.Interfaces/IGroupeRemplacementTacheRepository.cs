
using Fred.Entities.Depense;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  /// Représente un référentiel de données pour les lot de remplacements de taches
  /// </summary>
  public interface IGroupeRemplacementTacheRepository : IRepository<GroupeRemplacementTacheEnt>
  {
    /// <summary>
    /// Ajout d'une tâche de remplacement
    /// </summary>
    /// <param name="groupe">La tache de remplacement</param>
    /// <returns>La liste des Taches de remplacement.</returns>
    GroupeRemplacementTacheEnt AddGroupeRemplacementTache(GroupeRemplacementTacheEnt groupe);

    /// <summary>
    ///   Supprime un groupe de dépenses
    /// </summary>
    /// <param name="groupeId">L'identifiant du groupe à supprimer</param>
    void DeleteGroupeRemplacementTacheById(int groupeId);

    /// <summary>
    /// Retourne la taches.
    /// </summary>
    /// <param name="groupeId">Identifiant du groupe</param>
    /// <returns>La tache de remplacement</returns>
    GroupeRemplacementTacheEnt GetGroupeRemplacementTacheById(int groupeId);

  }
}