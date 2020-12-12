using FluentValidation;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport
{
  /// <summary>
  ///   Interface du valideur des Pointages
  /// </summary>
  public interface IRapportValidator : IValidator<RapportEnt>
  {
    /// <summary>
    ///   Teste un Rapport personnel
    /// </summary>
    /// <param name="rapport">Le Rapport à vérifier</param>
    void CheckRapport(RapportEnt rapport);

    /// <summary>
    /// Retourne Vrai si la date du chantier est dans une période clôturée
    /// </summary>
    /// <param name="rapport">Rapoort de chantier</param>
    /// <returns>Vrai si la date du chantier est dans une période clôturée</returns>
    bool IsDateChantierInPeriodeCloture(RapportEnt rapport);
  }
}