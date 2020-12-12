using Fred.Web.Models.CI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Représente une prime
  /// </summary>
  public class PrimeModel : IReferentialModel
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
    /// Obtient ou définit une prime est de type horaire ou non (dans ce cas journalière).
    /// </summary>
    public bool TypeHoraire { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre d'heures max dans le cas où la prime serait de type horaire.
    /// </summary>
    public double? NombreHeuresMax { get; set; }

    /// <summary>
    /// Obtient ou définit la prime est active ou non.
    /// </summary>
    public bool Actif { get; set; }

    /// <summary>
    /// Obtient ou définit si la prime est une prime partenaire ou non.
    /// </summary>
    public bool PrimePartenaire { get; set; }

    /// <summary>
    /// Obtient ou définit si la prime est une prime publique ou non.
    /// </summary>
    public bool Publique { get; set; }

    /// <summary>
    /// Obtient ou définit l'ID de la société associée.
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des CIs associés.
    /// </summary>
    public ICollection<CIPrimeModel> CIPrimes { get; set; }

    /// <summary>
    /// Obtient ou définit si la prime est associée à un CI ou non.
    /// </summary>
    public bool IsLinkedToCI { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du référentiel prime
    /// </summary>
    public string IdRef => Convert.ToString(this.PrimeId);

    /// <summary>
    /// Obtient ou définit le libelle du référentiel prime
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient ou définit le code du référentiel prime
    /// </summary>
    public string CodeRef => this.Code + " - " + ((this.TypeHoraire) ? "Horaire" : "Journalière");
  }
}
