namespace Fred.Business.Societe
{
  /// <summary>
  /// Classe renfermant le resultat de l'existance d'une societe
  /// </summary>
  public class SocieteExistResult
  {
    /// <summary>
    /// CodeIdentique
    /// </summary>
    public bool CodeIdentique { get; internal set; } = false;
    /// <summary>
    /// LibelleIdentique
    /// </summary>
    public bool LibelleIdentique { get; internal set; } = false;
  }
}