using System;
using Fred.Web.Models.CodeZoneDeplacement;
using Fred.Web.Models.Referential;
using Fred.Web.Models.CI;
using Fred.Web.Models.Personnel;

namespace Fred.Web.Models.IndemniteDeplacement
{
  /// <summary>
  /// Représente une indemnite deplacement
  /// </summary>
  public class IndemniteDeplacementModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une indemnité de déplacement.
    /// </summary>
    public int IndemniteDeplacementId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du personnel auquel est rattachée l'indemnité
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du CI auquel est rattachée l'indemnité
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit une distance en kilometre
    /// </summary>
    public double NombreKilometres { get; set; }

    /// <summary>
    /// Obtient ou définit une distance à vol d'oiseau en kilometre entre le domicile du personnel et l'etablissement de rattachement
    /// </summary>
    public double? NombreKilometreVODomicileRattachement { get; set; }

    /// <summary>
    /// Obtient ou définit une distance à vol d'oiseau en kilometre entre le domicile du personnel et le chantier
    /// </summary>
    public double? NombreKilometreVODomicileChantier { get; set; }

    /// <summary>
    /// Obtient ou définit une distance à vol d'oiseau en kilometre entre le chantier et l'etablissement de rattachement
    /// </summary>
    public double? NombreKilometreVOChantierRattachement { get; set; }

    /// <summary>
    /// Obtient ou définit la date de dernier calcul de l'indemnité
    /// </summary>
    public DateTime? DateDernierCalcul { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si il s'agit d'une indemnité voyage détente
    /// </summary>
    public bool IVD { get; set; }

    /// <summary>
    /// Obtient ou définit le code déplacement de l'indemnité
    /// </summary>
    public int? CodeDeplacementId { get; set; }

    /// <summary>
    /// Obtient ou définit le code zone déplacement de l'indemnité
    /// </summary>
    public int? CodeZoneDeplacementId { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si il s'agit d'une saisie manuelle
    /// </summary>
    public bool SaisieManuelle { get; set; }

    /// <summary>
    /// Obtient ou définit la date de création de l'indemnité
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'auteur de la création de l'indemnité
    /// </summary>
    public int? AuteurCreation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de modification de l'indemnité
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'auteur de la modification de l'indemnité
    /// </summary>
    public int? AuteurModification { get; set; }

    /// <summary>
    /// Obtient ou définit la date de suppression de l'indemnité
    /// </summary>
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'auteur de la suppression de l'indemnité
    /// </summary>
    public int? AuteurSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si Actif
    /// </summary>
    public bool Actif { get; set; }

    /// <summary>
    /// Obtient le libelle d'une affaire
    /// </summary>
    public string CiLibelle
    {
      get { return CI != null ? CI.Libelle : string.Empty; }
    }

    /// <summary>
    /// Obtient le libelle code déplacement
    /// </summary>
    public string CodeDeplacementLibelle
    {
      get { return CodeDeplacement != null ? CodeDeplacement.Libelle : string.Empty; }
    }

    /// <summary>
    /// Obtient le libelle code zone deplacement
    /// </summary>
    public string CodeZoneDeplacementLibelle
    {
      get { return CodeZoneDeplacement != null ? CodeZoneDeplacement.Libelle : string.Empty; }
    }
    /// <summary>
    /// Obtient ou définit le code zone déplacement concernant l'indeminité de déplacement
    /// </summary>
    public CodeZoneDeplacementModel CodeZoneDeplacement { get; set; }

    /// <summary>
    /// Obtient ou définit le code déplacement concernant l'indeminité de déplacement
    /// </summary>
    public CodeDeplacementModel CodeDeplacement { get; set; }

    /// <summary>
    /// Obtient ou définit l'affaire concernant l'indeminité de déplacement
    /// </summary>
    public CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit le personnel concerné par l'indeminité de déplacement
    /// </summary>
    public PersonnelModel Personnel { get; set; }

    /// <summary>
    /// Fait une copie de l'objet courant
    /// </summary>
    /// <returns>Retourne une copie de l'objet courant</returns>
    public IndemniteDeplacementModel Copy()
    {
      return (IndemniteDeplacementModel)this.MemberwiseClone();
    }

  }
}
 