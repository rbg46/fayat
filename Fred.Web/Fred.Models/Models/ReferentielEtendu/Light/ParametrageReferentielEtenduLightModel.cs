using Fred.Web.Models.Referential;
using Fred.Web.Models.Referential.Light;

namespace Fred.Web.Models.ReferentielEtendu.Light
{
  /// <summary>
  ///   Représente un paramétrage de référentiel étendu
  /// </summary>
  public class ParametrageReferentielEtenduLightModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique de l'entité.
    /// </summary>
    public int ParametrageReferentielEtenduId { get; set; }


    public OrganisationLight Organisation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une organisation.
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une devise.
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité devise.
    /// </summary>
    public DeviseLightModel Devise { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un référentiel étendu.
    /// </summary>
    public int ReferentielEtenduId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité référentiel étendu.
    /// </summary>
    public ReferentielEtenduLightModel ReferentielEtendu { get; set; }

    

    public int UniteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité Unité.
    /// </summary>   
    public UniteModel Unite { get; set; }

    /// <summary>
    ///   Obtient ou définit le montant
    /// </summary>
    public virtual decimal? Montant { get; set; }
  }


  public class OrganisationLight
  {
    public int OrganisationId { get; set; }

    public int TypeOrganisationId { get; set; }
  }
}