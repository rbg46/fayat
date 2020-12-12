using Fred.Entities.Budget.Avancement;

namespace Fred.Business.Budget.Avancement
{
  /// <summary>
  /// Interface des états budget.
  /// </summary>
  public interface IAvancementEtatManager : IManager<AvancementEtatEnt>
  {
    /// <summary>
    /// Retourne l'état de l'avancement en fonction d'un code
    /// </summary>
    /// <param name="code">Code de l'état</param>
    /// <returns>L'état de l'avancement</returns>
    AvancementEtatEnt GetByCode(string code);
  }
}
