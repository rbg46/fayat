using Fred.Entities;

namespace Fred.Business.Parametre
{
  /// <summary>
  ///   Gestionnaire des parametres.
  /// </summary>
  public interface IParametreManager : IManager<ParametreEnt>
  {
    /// <summary>
    ///   Retourne un ensemble de parametre pour le contrôle des appels Google API
    /// </summary>
    /// <returns>Objet GoogleAPIParams.</returns>
    GoogleApiParam GetGoogleApiParams();

    /// <summary>
    ///   Mise à jour des paramètres pour les appels à l'API google
    /// </summary>
    /// <param name="param">Objet contenant les parametres</param>
    void UpdateGoogleApiParams(GoogleApiParam param);

    /// <summary>
    ///   Retourne l'URL d'un groupe permettant d'acceder au scan des Factures
    /// </summary>
    /// <param name="groupeId">Identifiant du groupe</param>
    /// <returns>Url du Groupe</returns>
    string GetScanShareUrl(int groupeId);

    /// <summary>
    /// Récupère le prix par défaut des barèmes exploitation.
    /// </summary>
    /// <returns>Le prix par défaut des barèmes exploitation.</returns>
    double GetBaremeExploitationPrixDefaut();

    /// <summary>
    /// Récupère le prix chauffeur par défaut des barèmes exploitation.
    /// </summary>
    /// <returns>Le prix chauffeur par défaut des barèmes exploitation.</returns>
    double GetBaremeExploitationPrixChauffeurDefaut();
  }
}
