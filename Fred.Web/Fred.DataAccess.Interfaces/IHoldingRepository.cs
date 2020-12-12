using System.Collections.Generic;
using Fred.Entities.Holding;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente un référentiel de données pour les holdings.
  /// </summary>
  public interface IHoldingRepository : IFredRepository<HoldingEnt>
  {
    /// <summary>
    /// Retourne la liste des holdings.
    /// </summary>
    /// <returns>Renvoie la liste des holdings.</returns>
    IEnumerable<HoldingEnt> GetHoldings();

    /// <summary>
    /// Retourne la holding portant l'identifiant unique indiqué.
    /// </summary>
    /// <param name="holdingId">Identifiant de la holding à retrouver.</param>
    /// <returns>la holding retrouvée, sinon null.</returns>
    HoldingEnt GetHolding(int holdingId);

    /// <summary>
    /// Retourne la holding portant l'organisation Id unique indiqué.
    /// </summary>
    /// <param name="organisationId">Identifiant de l'organisation.</param>
    /// <returns>la holding retrouvée, sinon null.</returns>
    HoldingEnt GetHoldingByOrganisationId(int organisationId);
  }
}