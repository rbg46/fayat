using FluentValidation;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Rapport.Pointage;
using Fred.Entities;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Rapport;
using Fred.Web.Shared.App_LocalResources;
using System;
using System.Globalization;
using System.Linq;

namespace Fred.Business.Rapport
{
  /// <summary>
  ///   Valideur des Rapports
  /// </summary>
  public class RapportValidator : AbstractValidator<RapportEnt>, IRapportValidator
  {
    private readonly IDatesClotureComptableManager datesClotureComptableMgr;
    private readonly IPointageManager pointageManager;

    /// <summary>
    ///   Initialise une nouvelle instance de la classe <see cref="RapportValidator" />.
    /// </summary>    
    /// <param name="pointageManager">Manager des Pointages</param>
    /// <param name="datesClotureComptableMgr">Manager des Dates de cloture comptable</param>
    public RapportValidator(IPointageManager pointageManager, IDatesClotureComptableManager datesClotureComptableMgr)
    {
      this.pointageManager = pointageManager;
      this.datesClotureComptableMgr = datesClotureComptableMgr;
    }

#pragma warning disable S3776
        /// <summary>
        ///   Teste un Rapport personnel
        /// </summary>
        /// <param name="rapport">Le Rapport à vérifier</param>
        public void CheckRapport(RapportEnt rapport)
        {
            //// SHE: on m'a dit de mettre les ressources en dur pour l'instant alors je l'ai fait mais c'est sale baaah
            //// Faudra trouver un moyen d'appeler les ressources depuis le Business en les plaçant dans le framwork par exemple
            //// Test présence d'un CI
            if (rapport.CI == null)
            {
                rapport.ListErreurs.Add(FeatureRapport.RapportValidator_CI_Requis_Erreur);
            }

            //// Test présence de la date du rapport
            if (rapport.DateChantier.Date.CompareTo(DateTime.MinValue.Date) == 0)
            {
                rapport.ListErreurs.Add(FeatureRapport.RapportValidator_Date_Requise_Erreur);
            }
            else
            {
                if (this.datesClotureComptableMgr.IsTodayInPeriodeCloture(rapport.CiId, rapport.DateChantier.Month, rapport.DateChantier.Year))
                {
                    DatesClotureComptableEnt datesClotureComptable = this.datesClotureComptableMgr.Get(rapport.CiId, rapport.DateChantier.Year, rapport.DateChantier.Month);

                    rapport.ListErreurs.Add(string.Format(FeatureRapport.RapportValidator_Mois_Clos_Erreur,
                                                            rapport.DateChantier.ToString("MMM", CultureInfo.CurrentUICulture),
                                                            datesClotureComptable?.DateCloture.GetValueOrDefault().ToShortDateString()));
                }
            }

      if (!rapport.HoraireDebutM.HasValue && !rapport.HoraireFinM.HasValue && rapport.CI?.Societe?.Groupe?.Code == Constantes.CodeGroupeRZB)
      {
        rapport.ListErreurs.Add(FeatureRapport.Rapport_Detail_StatutValidation_Horaire_PremiereTrancheManquante_Error);
      }

      //// Test les tranches horaires du chantier (On se fout de la date! On gère ici des tranches horaires. impossible de saisir une heure de début sans heure de fin et inversement)
      if (rapport.HoraireDebutS.HasValue && !rapport.HoraireFinS.HasValue || !rapport.HoraireDebutS.HasValue && rapport.HoraireFinS.HasValue)
      {
        rapport.ListErreurs.Add(FeatureRapport.Rapport_Detail_StatutValidation_Horaire_Tranche_Erreur);
      }

      //// Test qu'il y ait au moins une ligne de rapport
      if (rapport.ListLignes.Where(o => !o.IsDeleted).ToList().Count > 0)
      {
        this.pointageManager.CheckListPointages(rapport.ListLignes);

                if (rapport.ListLignes.Where(o => !o.IsDeleted).SelectMany(s => s.ListErreurs).Any())
                {
                    rapport.ListErreurs.Add(FeatureRapport.RapportValidator_Lignes_Erreur);
                }
            }
            else
            {
                rapport.ListErreurs.Add(FeatureRapport.RapportValidator_Ligne_Requise_Erreur);
            }
    }
#pragma warning restore S3776

    /// <summary>
    /// Retourne Vrai si la date du chantier est dans une période clôturée
    /// </summary>
    /// <param name="rapport">Rapoort de chantier</param>
    /// <returns>Vrai si la date du chantier est dans une période clôturée</returns>
    public bool IsDateChantierInPeriodeCloture(RapportEnt rapport)
    {
      return this.datesClotureComptableMgr.IsPeriodClosed(rapport.CiId, rapport.DateChantier.Year, rapport.DateChantier.Month);
    }
  }
}
