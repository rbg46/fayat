namespace Fred.Web.Shared.Models.BilanFlash
{
  /// <summary>
  /// Model to generate Bilan Flash report rows
  /// </summary>
  public class PrintBilanFlashRowModel
  {
    /// <summary>
    /// Nale of the resource
    /// </summary>
    public string ResourceName { get; set; }

    /// <summary>
    /// Quantity target
    /// </summary>
    public double Target { get; set; }

    /// <summary>
    /// Unity of the resource
    /// </summary>
    public string Unity { get; set; }

    /// <summary>
    /// Quantity realised
    /// </summary>
    public double Realization { get; set; }
  }
}
