using Fred.Entities.Referential;

namespace Fred.Entities.ReferentielEtendu
{
  /// <summary>
  ///   Représente une liaison référentiel étendu/unité.
  /// </summary>
  public class UniteReferentielEtenduEnt
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une liaison référentiel étendu/unité.
    /// </summary>
    public int UniteReferentielEtenduId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une référentiel étendu.
    /// </summary>
    public int ReferentielEtenduId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'objet référentiel étendu.
    /// </summary>
    public ReferentielEtenduEnt ReferentielEtendu { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une unité.
    /// </summary>
    public int UniteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'objet unité.
    /// </summary>
    public UniteEnt Unite { get; set; }

    /// <summary>
    /// Indique si l'entité est supprimée
    /// </summary>
    public bool IsDeleted { get; set; } = false;
  }
}
