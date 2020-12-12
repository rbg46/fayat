namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Interface d'un référentiel modèle
  /// </summary>
  public interface IReferentialModel
  {
    /// <summary>
    /// Obtient le libelle du référentiel
    /// </summary>
    string LibelleRef { get; }

    /// <summary>
    /// Définit le code du référentiel
    /// </summary>
    string CodeRef { get; }

    /// <summary>
    /// Définit l'id du référentiel
    /// </summary>
    string IdRef { get; }
  }
}