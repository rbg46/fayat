namespace Fred.ImportExport.Models.Commande
{
  /// <summary>
  /// Représente une ligne d'avenant de commande à envoyer vers SAP.
  /// </summary>
  public class CommandeLigneAvenantSapModel : CommandeLigneSapModel
  {
    /// <summary>
    /// Le numéro d'avenant.
    /// </summary>
    public int AvenantNumero { get; set; }

    /// <summary>
    /// Indique s'il s'agit d'une diminution ou non.
    /// </summary>
    public bool Diminution { get; set; }

    /// <summary>
    /// Le type de modification :
    /// - I pour insertion
    /// - U pour modification
    /// - D pour suppression
    /// Dans notre cas, il s'agit toujours d'une insertion.
    /// </summary>
    public string Modification_type { get { return "I"; } }
  }
}
