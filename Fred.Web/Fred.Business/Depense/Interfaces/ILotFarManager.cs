using Fred.Entities.Depense;

namespace Fred.Business.Depense
{
  /// <summary>
  ///   Gestionnaire des lot de Far
  /// </summary>
  public interface ILotFarManager : IManager<LotFarEnt>
  {
    /// <summary>
    ///   Ajout d'un lot de far
    /// </summary>
    /// <param name="lf">Lot de fait</param>
    /// <returns>Lot de far créé</returns>
    LotFarEnt AddLotFar(LotFarEnt lf);
  }
}