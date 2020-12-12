using System.Collections.Generic;

namespace Fred.Entities.Models.Flux.Depense
{
  /// <summary>
  /// Model correspondant a la reponse a un job lancer a partir du flux depense
  /// </summary>
  public class DepenseFluxResponseModel
  {
    /// <summary>
    /// Le numero du job
    /// </summary>
    public string JobId { get; set; }

    /// <summary>
    /// La liste des depenses/reception ids
    /// </summary>
    public List<int> ReceptionsIds { get; set; }
  }
}
