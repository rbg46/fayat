namespace Fred.Framework
{
  /// <summary>
  ///   Classe de gestion du contexte de tri.
  /// </summary>
  public static class SortingContext
  {
    #region Constantes

    private const string Prefix = "SortingContext_";
    private const string SortName = "Sort";

    #endregion

    /// <summary>
    ///   Obtient ou définit le mode de tri de la sélection.
    /// </summary>
    public static SortingData Sorts
    {
      get { return ContextHelper.GetData(Prefix + SortName) as SortingData; }

      set { ContextHelper.SetData(Prefix + SortName, value); }
    }
  }
}