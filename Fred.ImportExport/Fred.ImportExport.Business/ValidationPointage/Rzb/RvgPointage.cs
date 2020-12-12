namespace Fred.ImportExport.Business.ValidationPointage
{
  /// <summary>
  /// Représente un pointage personnel de RVG.
  /// </summary>
  public class RvgPointage
  {
    /// <summary>
    /// Obtient ou définit le code affaire.
    /// </summary>
    public string CodeAffaire { get; set; }

    /// <summary>
    /// Obtient ou définit le code affaire prestation.
    /// </summary>
    public string CodeAffairePrestation { get; set; }

    /// <summary>
    /// Obtient ou définit le matricule du personnel pointé.
    /// </summary>
    public string MatriculePersonnel { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'établissement de paie du personnel pointé.
    /// </summary>
    public string EtablissementPaieIdPersonnel { get; set; }

    /// <summary>
    /// Obtient ou définit le code absence.
    /// </summary>
    public string CodeAbsence { get; set; }

    /// <summary>
    /// Obtient ou définit le code de déplacement.
    /// </summary>
    public string CodeDeplacement { get; set; }

    /// <summary>
    /// Obtient ou définit s'il s'agit d'un voyage détente : "0" pour non et "1" pour oui.
    /// </summary>
    public string VoyageDetente { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre d'heure normale.
    /// </summary>
    public decimal HeureNormale { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre d'heure majorée.
    /// </summary>
    public decimal HeureMajoration { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre d'heure d'absence.
    /// </summary>
    public decimal HeureAbsence { get; set; }

    /// <summary>
    /// Obtient ou définit la semaine de l'intempérie.
    /// </summary>
    public int NumSemaineIntemperieAbsence { get; set; }

    /// <summary>
    /// Obtient ou définit l'année du rapport.
    /// </summary>
    public int AnneeRapport { get; set; }

    /// <summary>
    /// Obtient ou définit le mois du rapport.
    /// </summary>
    public int MoisRapport { get; set; }

    /// <summary>
    /// Obtient ou définit le jour du rapport.
    /// </summary>
    public int JourRapport { get; set; }

    /// <summary>
    /// Obtient ou définit le code de la zone de déplacement.
    /// </summary>
    public string CodeZoneDeplacement { get; set; }

    /// <summary>
    /// Obtient ou définit le code de la majoration.
    /// </summary>
    public string CodeMajoration { get; set; }
  }
}
