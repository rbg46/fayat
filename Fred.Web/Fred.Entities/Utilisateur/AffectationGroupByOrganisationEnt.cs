using Fred.Entities.Organisation;

namespace Fred.Entities.Utilisateur
{
  /// <summary>
  ///   Les affectations regroupées par organisation
  /// </summary>
  public class AffectationGroupByOrganisationEnt
  {
    /// <summary>
    ///   Role de regroupement
    /// </summary>
    public OrganisationEnt Organisation { get; set; }

    /// <summary>
    ///   Liste des affectations regroupées
    /// </summary>
    public AffectationSeuilUtilisateurEnt[] Affectations { get; set; }
  }
}