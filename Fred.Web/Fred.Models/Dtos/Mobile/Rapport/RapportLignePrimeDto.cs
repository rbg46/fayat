using Fred.Web.Models.Referential;

namespace Fred.Web.Dtos.Mobile.Rapport
{
  /// <summary>
  /// Dto RapportLignePrime
  /// </summary>
  public class RapportLignePrimeDto
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique de la ligne prime du rapport
    /// </summary>
    public int RapportLignePrimeId { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de la ligne de rapport de rattachement
    /// </summary>
    public int RapportLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de la prime
    /// </summary>
    public int PrimeId { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la prime soit checkée
    /// </summary>
    public bool IsChecked { get; set; }

    /// <summary>
    /// Obtient ou définit l'heure de la prime uniquement si TypeHoraire est en heure
    /// </summary>
    public double? HeurePrime { get; set; }
  }
}