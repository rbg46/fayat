using System.Collections.Generic;
using Fred.Web.Models.Referential;

namespace Fred.Web.Models.ReferentielEtendu.Light
{
  /// <summary>
  ///   Représente un référentiel étendu allégé en données membres
  /// </summary>
  public class ReferentielEtenduLightModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'un référentiel étendu.
    /// </summary>
    public int ReferentielEtenduId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une societe.
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une ressource.
    /// </summary>
    public int RessourceId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une nature.
    /// </summary>
    public int? NatureId { get; set; }


    /// <summary>
    ///   Obtient ou définit la nature
    /// </summary>
    public NatureModel Nature { get; set; }

    /// <summary>
    ///   Obtient ou définit le paramétrage associé au référentiel étendu
    /// </summary>
    public ICollection<ParametrageReferentielEtenduLightModel> ParametrageReferentielEtendus { get; set; }
  }
}