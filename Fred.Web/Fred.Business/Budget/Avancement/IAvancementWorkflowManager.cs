using Fred.Entities.Budget.Avancement;

namespace Fred.Business.Budget.Avancement
{
  /// <summary>
  /// Interface de workflow d'avancement.
  /// </summary>
  public interface IAvancementWorkflowManager : IManager<AvancementWorkflowEnt>
  {
    /// <summary>
    /// Ajoute un workflow d'avancement
    /// </summary>
    /// <param name="avancement">avancement concerné par le workflow</param>
    /// <param name="etatCibleId">Identifiant de l'état cible</param>
    /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
    /// <param name="creation">Si on est dans le processus de création d'un avancement</param>
    void Add(AvancementEnt avancement, int etatCibleId, int utilisateurId, bool creation = false);
  }
}