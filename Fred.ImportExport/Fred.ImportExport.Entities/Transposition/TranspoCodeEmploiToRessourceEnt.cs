using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities.Transposition
{
  /// <summary>
  ///   Classe Transposition Code Emploi vers Ressource
  /// </summary>
  [Table("TranspoCodeEmploiToRessource")]
  public class TranspoCodeEmploiToRessourceEnt
  {
    /// <summary>
    ///   Obtient l'identifiant unique de la transposition
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TranspoCodeEmploiToRessourceId { get; set; }

    /// <summary>
    ///   Obtient ou définit le code la société d'import.
    /// </summary>
    public string CodeSocieteImport { get; set; }

    /// <summary>
    ///  Obtient ou définit le code emploi.
    /// </summary>
    public string CodeEmploi { get; set; }

    /// <summary>
    ///   Obtient ou définit les code ressource.
    /// </summary>
    public string CodeRessource { get; set; }
  }
}
