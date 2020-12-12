using System.Collections.Generic;

namespace Fred.Business.FeatureFlipping.Model
{
  /// <summary>
  /// Mapping 
  /// </summary>
  public class FeatureFlippingForTagHelperModel
  {
    /// <summary>
    /// ctor
    /// </summary>
    public FeatureFlippingForTagHelperModel() { }

    /// <summary>
    /// Nom de la Feature
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Indique si la feature est active ou non
    /// </summary>
    public bool IsActived { get; set; }


  }
}
