using Fred.Entities.Rapport;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Rapport
{

  /// <summary>
  /// Fonctionnalité Create Read Update Delete des indemnités de déplacement
  /// </summary>
  public interface IUtilitiesFeature
  {
    /// <summary>
    ///   Détermine si le rapport peut être supprimé ou non
    ///   //[TODO] /!\ Gérer les rôles par organisation
    /// </summary>
    /// <param name="rapport">rapport pour lequel déterminer la possibilité de suppression</param>
    /// <param name="userConnected">User connecté pour tracer la suppression</param>
    /// <returns>Booleen indiquant si le rapport peut être supprimé</returns>
    bool GetCanBeDeleted(RapportEnt rapport, UtilisateurEnt userConnected);

    /// <summary>
    ///   Détermine si le rapport peut être validé ou non
    /// </summary>
    /// <param name="rapport">rapport pour lequel déterminer la possibilité de valider</param>
    /// <returns>Booleen indiquant si le rapport peut être validé</returns>
    bool GetCanBeValidated(RapportEnt rapport);

    /// <summary>
    ///   Détermine si le rapport peut être Edité ou non
    /// </summary>
    /// <param name="rapport">rapport pour lequel déterminer la possibilité d'Editer</param>
    /// <returns>Booleen indiquant si le rapport peut être édité</returns>
    bool GetValidationSuperieur(RapportEnt rapport);

    /// <summary>
    ///   Détermine si le statut du rapport est Vérouillé
    /// </summary>
    /// <param name="rapport">rapport pour lequel déterminer des rapports</param>
    /// <returns>booléen indiquant si le statut du rapport est vérouillé</returns>
    bool IsStatutVerrouille(RapportEnt rapport);

    /// <summary>
    ///   Renvoi vrai si aujourd'hui est dans la periode de clôture
    /// </summary>
    /// <param name="rapport">Le rapport</param>
    /// <returns>Booleen indiquant si aujourd'hui est dans la periode de clôture</returns>
    bool IsTodayInPeriodeCloture(RapportEnt rapport);

    /// <summary>
    ///   Calcul le statut courant d'un rapport
    /// </summary>
    /// <param name="rapport">rapport à partir uquel déterminer le statut</param>
    void SetStatut(RapportEnt rapport);

    /// <summary>
    ///   Détermine si le rapport peut être verrouillé ou pas
    /// </summary>
    /// <param name="rapport">rapport pour lequel déterminer la possibilité de verrouiller</param>
    /// <returns>Booleen indiquant si le rapport peut être verrouillé</returns>
    /// <remarks>Le validateur de rapport doit être utilisé avant d'utiliser cette fonction.</remarks>
    bool GetCanBeLocked(RapportEnt rapport);
  }
}