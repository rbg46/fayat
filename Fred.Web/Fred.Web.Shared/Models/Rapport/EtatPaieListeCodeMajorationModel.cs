namespace Fred.Web.Models.Rapport
{
  /// <summary>
  /// Représente un model pour l'édition récapitulative des heures spécifiques
  /// </summary>
  public class EtatPaieListeCodeMajorationModel
  {
    /// <summary>
    /// Obtient ou définit le Etablissement
    /// </summary>
    public string Etablissement { get; set; }

    /// <summary>
    /// Obtient ou définit le Personnel
    /// </summary>
    public string Personnel { get; set; }

    /// <summary>
    /// Obtient ou définit le Affaire
    /// </summary>
    public string Affaire { get; set; }

    /// <summary>
    /// Obtient ou définit le Code
    /// </summary>
    public string CodeMajoration { get; set; }

    /// <summary>
    /// Obtient ou définit le Quantite
    /// </summary>
    public double Quantite { get; set; }

    /// <summary>
    /// Obtient ou définit le nom
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    /// Obtient ou définit le matricule
    /// </summary>
    public string Matricule { get; set; }

    /// <summary>
    /// Obtient ou définit si c'est un type Heure Nuit
    /// </summary>
    public bool IsHeureNuit { get; set; }
  }
}
