namespace Fred.Web.Shared.Models.Affectation
{
  /// <summary>
  /// Class des equipes a importer
  /// </summary>
  public class ImportedEquipeModel
  {
    /// <summary>
    /// Obtient ou definit le personnel identifier
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    ///  Obtient ou definit le personnel nom
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    ///  Obtient ou definit le prenom du personnl
    /// </summary>
    public string Prenom { get; set; }

    /// <summary>
    ///  Obtient ou definit le statut du personnel
    /// </summary>
    public string Statut { get; set; }

    /// <summary>
    ///  Obtient ou definit le matricule du personnel
    /// </summary>
    public string Matricule { get; set; }

    /// <summary>
    /// Obtient ou definit le codeSociete
    /// </summary>
    public string CodeSociete { get; set; }

    /// <summary>
    /// Obtient ou definit le status isinterne
    /// </summary>
    public bool IsInterne { get; set; }

    /// <summary>
    /// Obtient ou definit le status is intérimaire
    /// </summary>
    public bool IsInterimaire { get; set; }

    /// <summary>
    /// Get the interne status pour un personnel
    /// </summary>
    public string InternStatus
    {
      get
      {
        return IsInterimaire ? Constantes.PersonnelInterimaire : (IsInterne ? Constantes.PersonnelInterne : string.Empty);
      }
    }

  }
}
