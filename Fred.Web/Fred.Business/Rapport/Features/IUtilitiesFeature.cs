using Fred.Entities.Rapport;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Rapport
{

  /// <summary>
  /// Fonctionnalit� Create Read Update Delete des indemnit�s de d�placement
  /// </summary>
  public interface IUtilitiesFeature
  {
    /// <summary>
    ///   D�termine si le rapport peut �tre supprim� ou non
    ///   //[TODO] /!\ G�rer les r�les par organisation
    /// </summary>
    /// <param name="rapport">rapport pour lequel d�terminer la possibilit� de suppression</param>
    /// <param name="userConnected">User connect� pour tracer la suppression</param>
    /// <returns>Booleen indiquant si le rapport peut �tre supprim�</returns>
    bool GetCanBeDeleted(RapportEnt rapport, UtilisateurEnt userConnected);

    /// <summary>
    ///   D�termine si le rapport peut �tre valid� ou non
    /// </summary>
    /// <param name="rapport">rapport pour lequel d�terminer la possibilit� de valider</param>
    /// <returns>Booleen indiquant si le rapport peut �tre valid�</returns>
    bool GetCanBeValidated(RapportEnt rapport);

    /// <summary>
    ///   D�termine si le rapport peut �tre Edit� ou non
    /// </summary>
    /// <param name="rapport">rapport pour lequel d�terminer la possibilit� d'Editer</param>
    /// <returns>Booleen indiquant si le rapport peut �tre �dit�</returns>
    bool GetValidationSuperieur(RapportEnt rapport);

    /// <summary>
    ///   D�termine si le statut du rapport est V�rouill�
    /// </summary>
    /// <param name="rapport">rapport pour lequel d�terminer des rapports</param>
    /// <returns>bool�en indiquant si le statut du rapport est v�rouill�</returns>
    bool IsStatutVerrouille(RapportEnt rapport);

    /// <summary>
    ///   Renvoi vrai si aujourd'hui est dans la periode de cl�ture
    /// </summary>
    /// <param name="rapport">Le rapport</param>
    /// <returns>Booleen indiquant si aujourd'hui est dans la periode de cl�ture</returns>
    bool IsTodayInPeriodeCloture(RapportEnt rapport);

    /// <summary>
    ///   Calcul le statut courant d'un rapport
    /// </summary>
    /// <param name="rapport">rapport � partir uquel d�terminer le statut</param>
    void SetStatut(RapportEnt rapport);

    /// <summary>
    ///   D�termine si le rapport peut �tre verrouill� ou pas
    /// </summary>
    /// <param name="rapport">rapport pour lequel d�terminer la possibilit� de verrouiller</param>
    /// <returns>Booleen indiquant si le rapport peut �tre verrouill�</returns>
    /// <remarks>Le validateur de rapport doit �tre utilis� avant d'utiliser cette fonction.</remarks>
    bool GetCanBeLocked(RapportEnt rapport);
  }
}