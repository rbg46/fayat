using System.Collections.Generic;
using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;

namespace Fred.Web.Dtos.Mobile
{
  /// <summary>
  /// Représente une prime
  /// </summary>
  public class PrimeDto : DtoBase
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une prime.
    /// </summary>
    public int PrimeId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une prime.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une prime.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la prime est de type horaire ou non (dans ce cas journalière).
    /// </summary>
    public bool TypeHoraire { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre d'heures max dans le cas où la prime serait de type horaire.
    /// </summary>
    public double NombreHeuresMax { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la prime est active ou non.
    /// </summary>
    public bool Actif { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la prime est une prime partenaire ou non.
    /// </summary>
    public bool PrimePartenaire { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la prime est une prime publique ou non.
    /// </summary>
    public bool Publique { get; set; }

    /// <summary>
    /// Obtient ou définit l'ID du groupe associé.
    /// </summary>
    public int GroupeId { get; set; }
  }
}
