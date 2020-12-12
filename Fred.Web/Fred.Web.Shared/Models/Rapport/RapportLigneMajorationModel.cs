using Fred.Web.Models.Referential;

namespace Fred.Web.Shared.Models.Rapport
{
  /// <summary>
  ///   Représente ou défini une majoration associées à une ligne de rapport
  /// </summary>
  public class RapportLigneMajorationModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique de la ligne majoration du rapport
    /// </summary>
    public int RapportLigneMajorationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de la ligne de rapport de rattachement
    /// </summary>
    public int RapportLigneId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'entité CodeMajoration
    /// </summary>
    public int CodeMajorationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité CodeMajoration
    /// </summary>
    public CodeMajorationModel CodeMajoration { get; set; }

    /// <summary>
    ///   Obtient ou définit le l'heure majorée
    /// </summary>
    public double HeureMajoration { get; set; }

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit en création
    /// </summary>
    public bool IsCreated { get; set; } = false;

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit en modification
    /// </summary>
    public bool IsUpdated { get; set; } = false;

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit à supprimer
    /// </summary>
    public bool IsDeleted { get; set; } = false;
  }
}
