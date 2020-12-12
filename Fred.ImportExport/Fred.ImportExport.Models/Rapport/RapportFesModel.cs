namespace Fred.ImportExport.Models.Rapport
{
  /// <summary>
  /// Représente le modèle d'un rapport Fes à exporter
  /// </summary>
  public class RapportFesModel
  {
    /// <summary>
    /// Obtient ou définit l'année
    /// </summary>
    public string Annee { get; set; }

    /// <summary>
    /// Obtient ou définit le mois
    /// </summary>
    public string Mois { get; set; }

    /// <summary>
    /// Obtient ou définit le jour
    /// </summary>
    public string Jour { get; set; }

    /// <summary>
    /// Obtient ou définit la date pièce
    /// </summary>
    public string DatePiece { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé pièce
    /// </summary>
    public string LibellePiece { get; set; }

    /// <summary>
    /// Obtient ou définit le débit ou le crédit
    /// </summary>
    public string DebitCredit { get; set; }

    /// <summary>
    /// Obtient ou définit le montant
    /// </summary>
    public decimal? Montant { get; set; }

    /// <summary>
    /// Obtient ou définit le code affaire
    /// </summary>
    public string CodeAffaire { get; set; }

    /// <summary>
    /// Obtient ou définit le code sous affaire
    /// </summary>
    public string CodeSousAffaire { get; set; }

    /// <summary>
    /// Obtient ou définit le matériel
    /// </summary>
    public string Materiel { get; set; }

    /// <summary>
    /// Obtient ou définit le montant quantité
    /// </summary>
    public decimal? MontantQuantite { get; set; }

    /// <summary>
    /// Obtient ou définit le code quantité
    /// </summary>
    public string CodeQuantite { get; set; }

    /// <summary>
    /// Obtient ou définit le matricule
    /// </summary>
    public string Matricule { get; set; }

    /// <summary>
    /// Obtient ou définit le taux
    /// </summary>
    public string Taux { get; set; }

    /// <summary>
    /// Obtient ou définit la nature analytique
    /// </summary>
    public string NatureAnalytique { get; set; }

    /// <summary>
    /// Obtient ou définit l'évènement pointage
    /// </summary>
    public string EvenementPointage { get; set; }
  }
}
