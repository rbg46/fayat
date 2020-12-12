namespace Fred.Framework
{
  /// <summary>
  ///   Représente le contexte d'une page.
  /// </summary>
  public static class PagingContext
  {
    #region Constantes

    private const string Prefix = "PaggingContext_";
    ////private const string SkipName = "Skip";
    private const string TakeName = "Take";
    private const string PageName = "Page";

    #endregion

   

    /// <summary>
    ///   Obtient ou définit la page à afficher
    /// </summary>
    public static int? Page
    {
      get { return ContextHelper.GetData(Prefix + PageName) as int?; }

      set { ContextHelper.SetData(Prefix + PageName, value); }
    }

    /// <summary>
    ///   Obtient ou définit le nombre d'éléments à sélectionner.
    /// </summary>
    public static int? Take
    {
      get { return ContextHelper.GetData(Prefix + TakeName) as int?; }

      set { ContextHelper.SetData(Prefix + TakeName, value); }
    }

    /// <summary>
    ///   Obtient ou définit le nombre d'éléments à sélectionner.
    /// </summary>
    public static int? Total { get; set; }
  }
}