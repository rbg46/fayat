namespace Fred.Business.Personnel.Import
{
  /// <summary>
  /// Reponse de la mise a jour d'un personnel lors de l'import
  /// </summary>
  public class UpdatePersonnelInImportResult
  {
    /// <summary>
    /// indique si le personnel a changer
    /// </summary>
    public bool PersonnelAdressHasChanged { get; internal set; }

    /// <summary>
    /// indique si l'adresse du  personnel a changer
    /// </summary>
    public bool PersonnelHasChanged { get; internal set; }
  }
}