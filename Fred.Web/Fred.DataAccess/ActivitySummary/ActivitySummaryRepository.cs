using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.ActivitySummary;
using Fred.Entities.Commande;
using Fred.Entities.Rapport;
using Fred.Framework.Extensions;
using static Fred.Entities.Constantes;

namespace Fred.DataAccess.ActivitySummary
{
    /// <summary>
    /// Permet de faire un rapport,etat des lieux les activités en cours des utilisateurs FRED
    /// </summary>
    public class ActivitySummaryRepository : IActivitySummaryRepository
    {
        private readonly EntityFramework.FredDbContext fredDbContext;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="fredDbContext">fredDbContext</param>
        public ActivitySummaryRepository(EntityFramework.FredDbContext fredDbContext)
        {
            this.fredDbContext = fredDbContext;
        }


        /// <summary>
        /// Retourne les informations necessaires pour calculer les travaux en cours
        /// </summary>
        /// <returns>List de ActivitySummaryRequestResult</returns>
        public List<ActivityRequestWithCountResult> GetDataForCalculateWorkInProgress()
        {
            var result = new List<ActivityRequestWithCountResult>();

            var commandesAvalider = GetCommandesAValider();

            var rapportsAvalide1 = GetRapportsAvalide1();

            var receptionsAviser = GetReceptionsAviser();

            var budgetAvalider = GetBudgetAvalider();

            var avancementAValider = GetAvancementAvalider();

            var controleBudgetaireAvalider = GetControleBudgetaireAvalider();


            result.AddRange(commandesAvalider);

            result.AddRange(rapportsAvalide1);

            result.AddRange(receptionsAviser);

            result.AddRange(budgetAvalider);

            result.AddRange(avancementAValider);

            result.AddRange(controleBudgetaireAvalider);



            return result;
        }





        /// <summary>
        /// Retourne la liste des jalons
        /// </summary>
        /// <returns>List de ActivityRequestWithDateResult</returns>
        public List<ActivityRequestWithDateResult> GetJalons()
        {
            var result = new List<ActivityRequestWithDateResult>();

            var jalonClotureDepenses = GetJalonClotureDepenses();

            var jalonTransfertFars = GetJalonTransfertFars();

            var jalonValidationAvancements = GetJalonValidationAvancements();

            var jalonValidationControleBudgetaire = GetJalonValidationControleBudgetaire();

            result.AddRange(jalonClotureDepenses);

            result.AddRange(jalonTransfertFars);

            result.AddRange(jalonValidationAvancements);

            result.AddRange(jalonValidationControleBudgetaire);

            return result;
        }

        /// <summary>
        /// Retourne les ci actifs
        /// </summary>
        /// <returns>Liste des ids des ci actifs</returns>
        public List<int> GetCiActifs()
        {
            return GetCiActifs(false);
        }

        /// <summary>
        /// Retourne les ci actifs
        /// </summary>
        /// <param name="onlyChantierFred">Indique si seuls les chantiers gérés par FRED sont retournés</param>
        /// <returns>Liste des ids des ci actifs</returns>
        public List<int> GetCiActifs(bool onlyChantierFred)
        {
            return this.fredDbContext.CIs.Where(p => (p.DateFermeture == null || p.DateFermeture >= DateTime.Today)
                        && (p.DateOuverture == null || p.DateOuverture <= DateTime.Today) && (!onlyChantierFred || p.ChantierFRED)
            ).Select(s => s.CiId).ToList();
        }

        /// <summary>
        /// Retourne la liste des Emails pour ine liste de personnelIds
        /// </summary>
        /// <param name="personnelIds">personnelIds</param>
        /// <returns>Liste de EmailOfPersonnelResult</returns>
        public List<PersonnelInfoForSendEmailResult> GetPersonnelsDataForSendEmail(List<int> personnelIds)
        {
            return this.fredDbContext.Personnels.Where(p => personnelIds.Contains(p.PersonnelId)).Select(p => new PersonnelInfoForSendEmailResult()
            {
                PersonnelId = p.PersonnelId,
                Email = p.Email,
                Nom = p.Nom,
                Prenom = p.Prenom,
                Matricule = p.Matricule
            })
            .ToList();
        }


        private List<ActivityRequestWithCountResult> GetCommandesAValider()
        {

            var typeCodes = new List<string> { CommandeTypeEnt.CommandeTypeF, CommandeTypeEnt.CommandeTypeL, CommandeTypeEnt.CommandeTypeP };

            var commandesAvalider = (from c in fredDbContext.Commandes
                                     where c.StatutCommande.Code == StatutCommandeEnt.CommandeStatutAV
                                     && c.CiId.HasValue
                                     && !c.DateSuppression.HasValue
                                     && typeCodes.Contains(c.Type.Code)
                                     group c by c.CiId.Value into g
                                     select new ActivityRequestWithCountResult()
                                     {
                                         RequestName = TypeActivity.CommandeAvalider,
                                         CiId = g.Key,
                                         Count = g.Count()
                                     })
                                     .ToList();
            return commandesAvalider;
        }

        private List<ActivityRequestWithCountResult> GetRapportsAvalide1()
        {
            var rapportsAvalide1 = (from c in fredDbContext.Rapports
                                    where c.RapportStatutId == RapportStatutEnt.RapportStatutValide1.Key
                                    && !c.IsGenerated
                                    && !c.DateSuppression.HasValue
                                    group c by c.CiId into g
                                    select new ActivityRequestWithCountResult()
                                    {
                                        RequestName = TypeActivity.RapportsAvalide1,
                                        CiId = g.Key,
                                        Count = g.Count()
                                    })
             .ToList();
            return rapportsAvalide1;
        }

        private List<ActivityRequestWithCountResult> GetReceptionsAviser()
        {
            var depenseType = Entities.DepenseType.Reception.ToIntValue();

            var receptionsAviser = (from c in fredDbContext.DepenseAchats
                                    where c.DepenseType.Code == depenseType
                                    && c.CiId.HasValue
                                    && !c.DateSuppression.HasValue
                                    && c.DateComptable.HasValue
                                    && ((c.DateVisaReception == null || c.HangfireJobId == null) && !c.IsReceptionInterimaire)
                                    group c by c.CiId.Value into g
                                    select new ActivityRequestWithCountResult()
                                    {
                                        RequestName = TypeActivity.ReceptionsAviser,
                                        CiId = g.Key,
                                        Count = g.Count()
                                    })
             .ToList();
            return receptionsAviser;
        }

        private List<ActivityRequestWithCountResult> GetBudgetAvalider()
        {
            var budgetAvalider = (from b in fredDbContext.Budgets
                                  where b.BudgetEtat.Code == EtatBudget.AValider
                                  && !b.DateSuppressionBudget.HasValue
                                  group b by b.CiId into g
                                  select new ActivityRequestWithCountResult()
                                  {
                                      RequestName = TypeActivity.BudgetAvalider,
                                      CiId = g.Key,
                                      Count = g.Count()
                                  })
             .ToList();
            return budgetAvalider;
        }

        private List<ActivityRequestWithCountResult> GetAvancementAvalider()
        {
            var periodeString = DateTime.UtcNow.ToString("yyyyMM");

            var periodeInt = int.Parse(periodeString);

            var avancementAvalider = (from b in fredDbContext.Avancements
                                      where b.Periode == periodeInt
                                      group b by b.CiId into g
                                      select g.OrderByDescending(gg => gg.Periode).FirstOrDefault()).
                                      Where(b => b.AvancementEtat.Code == EtatAvancement.AValider)

                                     .Select(a => new ActivityRequestWithCountResult()
                                     {
                                         RequestName = TypeActivity.AvancementAvalider,
                                         CiId = a.CiId,
                                         Count = 1
                                     }).ToList();


            return avancementAvalider;
        }

        private List<ActivityRequestWithCountResult> GetControleBudgetaireAvalider()
        {

            var controleBudgetaireAvalider = (from cb in fredDbContext.ControleBudgetaires
                                              group cb by cb.ControleBudgetaireId into g
                                              select g.OrderByDescending(gg => gg.Periode).FirstOrDefault()).
                                      Where(b => b.ControleBudgetaireEtat.Code == EtatBudget.AValider)

                                     .Select(a => new ActivityRequestWithCountResult()
                                     {
                                         RequestName = TypeActivity.ControleBudgetAvalider,
                                         CiId = a.Budget.CiId,
                                         Count = 1
                                     }).ToList();
            return controleBudgetaireAvalider;
        }

        private List<ActivityRequestWithDateResult> GetJalonTransfertFars()
        {

            var jalonTransfertFars = (from dcc in fredDbContext.DatesCloturesComptables
                                      where dcc.Historique == false
                                      group dcc by dcc.CiId into g
                                      select g.OrderByDescending(gg => gg.DateTransfertFAR).FirstOrDefault())
                                     .Select(a => new ActivityRequestWithDateResult()
                                     {
                                         RequestName = TypeJalon.JalonTransfertFar,
                                         CiId = a.CiId,
                                         Date = a.DateTransfertFAR
                                     }).ToList();

            return jalonTransfertFars;
        }

        private List<ActivityRequestWithDateResult> GetJalonClotureDepenses()
        {

            var jalonClotureDepenses = (from dcc in fredDbContext.DatesCloturesComptables
                                        where dcc.Historique == false
                                        group dcc by dcc.CiId into g
                                        select g.OrderByDescending(gg => gg.Annee).ThenBy(gg => gg.Mois).FirstOrDefault())
                                     .Select(a => new ActivityRequestWithDateResult()
                                     {
                                         RequestName = TypeJalon.JalonClotureDepense,
                                         CiId = a.CiId,
                                         Date = a.DateCloture
                                     }).ToList();
            return jalonClotureDepenses;
        }


        private List<ActivityRequestWithDateResult> GetJalonValidationAvancements()
        {
            var jalonAvancementValides = (from b in fredDbContext.Avancements
                                          group b by b.CiId into g
                                          select g.OrderByDescending(gg => gg.Periode).FirstOrDefault())
                                        .Where(b => b.AvancementEtat.Code == EtatAvancement.Valide)
                                        .ToList();

            var jalonAvancementValidesResult = jalonAvancementValides.Select(a => new ActivityRequestWithDateResult()
            {
                RequestName = TypeJalon.JalonAvancementValider,
                CiId = a.CiId,
                Date = DateTime.ParseExact(a.Periode.ToString(), "yyyyMM", System.Globalization.CultureInfo.InvariantCulture)
            })
            .ToList();
            return jalonAvancementValidesResult;
        }

        private List<ActivityRequestWithDateResult> GetJalonValidationControleBudgetaire()
        {

            var sources = (from cb in fredDbContext.ControleBudgetaires
                           where cb.ControleBudgetaireEtat.Code == EtatBudget.EnApplication
                           join b in fredDbContext.Budgets on cb.ControleBudgetaireId equals b.BudgetId
                           group new { ControleBudgetaire = cb, Budget = b } by cb.ControleBudgetaireId into g
                           select new
                           {
                               ControleBudgetaire = g.OrderByDescending(cb => cb.ControleBudgetaire.Periode).FirstOrDefault(),
                               Budget = g.Select(x => x.Budget).FirstOrDefault()
                           }).ToList();


            var jalonValidationControleBudgetaire = sources.Select(a => new ActivityRequestWithDateResult()
            {
                RequestName = TypeJalon.JalonValidationControleBudgetaire,
                CiId = a.Budget.CiId,
                Date = DateTime.ParseExact(a.ControleBudgetaire.ControleBudgetaire.Periode.ToString(), "yyyyMM", System.Globalization.CultureInfo.InvariantCulture)
            }).ToList();


            return jalonValidationControleBudgetaire;

        }
    }
}
