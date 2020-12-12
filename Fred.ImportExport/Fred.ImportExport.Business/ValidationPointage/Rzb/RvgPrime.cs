using System;

namespace Fred.ImportExport.Business.ValidationPointage
{
  /// <summary>
  /// Représente une prime RVG.
  /// </summary>
  public class RvgPrime
  {
    /// <summary>
    /// Obtient ou définit le code affaire.
    /// </summary>
    public string CodeAffaire { get; set; }

    /// <summary>
    /// Obtient ou définit le matricule du personnel.
    /// </summary>
    public string MatriculePersonnel { get; set; }

    /// <summary>
    /// Obtient ou définit la date.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Obtient ou définit le code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit la quantité.
    /// </summary>
    public decimal Quantite { get; set; }

    /// <summary>
    /// Obtient ou définit si la prime est du type horaire.
    /// </summary>
    public bool TypeHoraire { get; set; }
  }
}
