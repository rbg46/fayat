namespace Fred.Framework
{
  /// <summary>
  ///   Représente un tri.
  /// </summary>
  public class SortingData
  {
    /// <summary>
    ///   Obtient ou définit le nom du champ utilisé pour le tri.
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    ///   Obtient ou définit la direction du tri.
    /// </summary>
    public string Dir { get; set; }
  }
}