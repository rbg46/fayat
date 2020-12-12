namespace Fred.Web.Models.Rapport
{
  /// <summary>
  /// Représente un grand déplacement (code déplacement de type "IGD") pour la liste des Etat Paie
  /// </summary>
  public class EtatPaieListeIgdModel
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
    public string CodeIGD { get; set; }

    /// <summary>
    /// Obtient ou définit le Quantite
    /// </summary>
    public double Quantite { get; set; }

    /// <summary>
    /// Obtient ou définit le type
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Obtient ou définit le nom
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    /// Obtient ou définit le matricule
    /// </summary>
    public string Matricule { get; set; }
  }
}