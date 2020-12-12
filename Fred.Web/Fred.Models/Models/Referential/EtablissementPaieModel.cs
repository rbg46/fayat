using Fred.Web.Models.Societe;

namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Représente un établissement comptable.
  /// </summary>
  public class EtablissementPaieModel : IReferentialModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un établissement de paie.
    /// </summary>
    public int EtablissementPaieId { get; set; }

    /// <summary>
    /// Obtient ou définit le code de l'établissement de paie.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé de l'établissement de paie.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit l'Adresse de l'établissement de paie.
    /// </summary>
    public string Adresse { get; set; }

    /// <summary>
    /// Obtient ou définit la latitude d'un l'établissement de paie.
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// Obtient ou définit la longitude d'un l'établissement de paie.
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// Obtient ou définit un établissement de paie est une agence de rattachement ou non.
    /// </summary>
    public bool IsAgenceRattachement { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'établissement de paie auquel l'établissement est rattaché.
    /// </summary>
    public int? AgenceRattachementId { get; set; }

    /// <summary>
    /// Obtient ou définit l'établissement de paie auquel l'établissement est rattaché.
    /// </summary>
    public EtablissementPaieModel AgenceRattachement { get; set; }

    /// <summary>
    /// Obtient ou définit un établissement de paie gère les indemnités de déplacement ou non.
    /// </summary>
    public bool GestionIndemnites { get; set; }

    /// <summary>
    /// Obtient ou définit un établissement de paie est hors région ou non.
    /// </summary>
    public bool HorsRegion { get; set; }

    /// <summary>
    /// Obtient ou définit un établissement de paie est actif ou non.
    /// </summary>
    public bool Actif { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la société de l'établissement
    /// </summary>
    public int? SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit la société de l'établissement
    /// </summary>
    public SocieteModel Societe { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle => this.Code + " - " + this.Libelle;

    //PickList

    /// <summary>
    /// Id du référentiel matétriel
    /// </summary>
    public string IdRef => this.EtablissementPaieId.ToString();

    /// <summary>
    /// Libelle du référentiel matétriel
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Libelle du référentiel matétriel
    /// </summary>
    public string CodeRef => this.Code;
  }
}
