using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.Extensions;
using Fred.Business.Budget.Helpers;
using Fred.Business.Budget.Mapper;
using Fred.Business.CI;
using Fred.Business.Depense;
using Fred.Business.Depense.Services;
using Fred.Entities.Budget;
using Fred.Entities.Societe;
using Fred.Web.Shared.Models.Budget.Depense;
using Fred.Web.Shared.Models.Depense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;
using static Fred.Business.Budget.ControleBudgetaire.Helpers.CustomComputeColumnValueForAxe;
using static Fred.Business.Budget.Helpers.ComputeColumnValueForAxe;

namespace Fred.Business.Budget.ControleBudgetaire.Helpers
{
    public abstract class AbstractControleBudgetaireDataSourceBuilder : ManagersAccess
    {
        protected int periode;
        protected int periodePrecedenteControleBudgetaire;
        protected int budgetEnApplicationId;
        protected int ciId;
        protected SocieteEnt societe;
        protected readonly AxeTreeDataSource sources = new AxeTreeDataSource();
        protected IEnumerable<ControleBudgetaireValeursEnt> valeursControleBudgetairePourPeriode;
        protected IEnumerable<ControleBudgetaireValeursEnt> valeursControleBudgetairePourPeriodePrecedente;

        protected AbstractControleBudgetaireDataSourceBuilder(
            IControleBudgetaireManager controleBudgetaireManager,
            IDepenseServiceMediator depenseServiceMediator,
            IBudgetManager budgetManager,
            IBudgetT4Manager budgetT4Manager,
            ICIManager ciManager,
            IAvancementManager avancementManager)
        {
            ControleBudgetaireManager = controleBudgetaireManager;
            DepenseServiceMediator = depenseServiceMediator;
            BudgetManager = budgetManager;
            BudgetT4Manager = budgetT4Manager;
            CIManager = ciManager;
            AvancementManager = avancementManager;
        }

        protected IControleBudgetaireManager ControleBudgetaireManager { get; set; }

        protected IDepenseServiceMediator DepenseServiceMediator { get; set; }

        protected IBudgetManager BudgetManager { get; set; }

        protected IBudgetT4Manager BudgetT4Manager { get; set; }

        protected ICIManager CIManager { get; set; }

        protected IAvancementManager AvancementManager { get; set; }

        public virtual async Task<AxeTreeDataSource> BuildDataSourceAsync(int ciId, int budgetId, int periode)
        {
            var dateMiseEnApplicationBudget = BudgetManager.GetDateMiseEnApplicationBudgetSurCi(ciId).Value;

            sources.AddColumn("IsNewTache", IsNewTache, dateMiseEnApplicationBudget);
            sources.AddColumn("MontantBudget", SumChildrenValues);
            sources.AddColumn("PuBudget", GetPu, "MontantBudget", "QuantiteBudget");
            sources.AddColumn("QuantiteBudget", GetQuantite);
            sources.AddColumn("MontantAjustement", SumChildrenValues);
            sources.AddColumn("CommentaireAjustement", GetAjustementCommentaire);
            sources.AddColumn("PfaMoisPrecedent", SumChildrenValues);

            //Ces colonnes ci ne sont pas gérées par cette classe abstraite car les valeurs peuvent changer
            //selon que l'affichage est cumulée ou non.
            sources.AddColumn("MontantDad", SumChildrenValues);
            sources.AddColumn("PourcentageAvancement", ComputeAverageValueOfColumn);
            sources.AddColumn("MontantDepense", SumChildrenValues);
            sources.AddColumn("QuantiteDepense", GetQuantite);
            sources.AddColumn("DisplayQtePu", GetDisplayQtePuFlag);
            sources.AddColumn("ProjectionLineaire", GetProjectionLineaire);

            this.periode = periode;
            // dernière période de controle budgetaire renseignée
            int? latestPeriodePrecedente = ControleBudgetaireManager.GetControleBudgetaireLatestPeriode(budgetId, periode);
            if (latestPeriodePrecedente.HasValue)
            {
                periodePrecedenteControleBudgetaire = latestPeriodePrecedente.Value;
            }
            else
            {
                periodePrecedenteControleBudgetaire = PeriodeHelper.GetPreviousPeriod(periode).Value;
            }
            budgetEnApplicationId = budgetId;
            this.ciId = ciId;
            societe = CIManager.GetSocieteByCIId(ciId);

            valeursControleBudgetairePourPeriode = ControleBudgetaireManager.GetControleBudgetaireValeurs(budgetId, periode);
            valeursControleBudgetairePourPeriodePrecedente = ControleBudgetaireManager.GetControleBudgetaireValeurs(budgetId, periodePrecedenteControleBudgetaire);

            return sources;
        }

        protected virtual void AddDepensesNonBudgetees(AxeTreeDataSource sources, IEnumerable<BudgetDepenseModel> depensesNonBudgetees)
        {
            //A FAIRE add propertie to show tooltip on New task (Task.DateCreation > Bareme.DatemiseEnApplication)
            var grouppedAutreDepenses = depensesNonBudgetees.GroupBy(d => new
            {
                d.RessourceId,
                d.TacheId
            })
                .Select(gad => new
                {
                    Tache = gad.First().Tache,
                    Ressource = gad.First().Ressource,
                    Depense = gad.ToList()
                }).Where(gad => gad.Ressource != null);

            foreach (var depenseNonBudgete in grouppedAutreDepenses)
            {
                var row = sources.NewRow();
                row.Ressource = depenseNonBudgete.Ressource;
                //Dans l'objet depense, la tache est déjà une tache de niveau 3
                row.Tache3 = depenseNonBudgete.Tache;
                row["MontantDepense"] = depenseNonBudgete.Depense.Sum(d => d.MontantHT);
                row["QuantiteDepense"] = new RessourceUnite()
                {
                    Quantite = depenseNonBudgete.Depense.Sum(d => d.Quantite),
                    Unite = depenseNonBudgete.Depense.GetUniteAssocieeDepense()
                };

                var valeursMoisCourant = valeursControleBudgetairePourPeriode?.FirstOrDefault(v =>
                    v.RessourceId == row.Ressource.RessourceId && v.TacheId == row.Tache3.TacheId);
                var valeursMoisPrecedent = valeursControleBudgetairePourPeriodePrecedente?.FirstOrDefault(v =>
                    v.RessourceId == row.Ressource.RessourceId && v.TacheId == row.Tache3.TacheId);
                row["MontantAjustement"] = valeursMoisCourant?.Ajustement ?? valeursMoisPrecedent?.Ajustement ?? 0;
                row["CommentaireAjustement"] = valeursMoisCourant?.CommentaireAjustement ??
                                               valeursMoisPrecedent?.CommentaireAjustement;
                row["PfaMoisPrecedent"] = valeursMoisPrecedent?.Pfa;

                sources.Valeurs.Add(row);
            }
        }

        protected virtual AxeTreeDataSourceRow BuildDataSourceLineForSousDetail(BudgetSousDetailEnt sd, AxeTreeDataSource sources)
        {
            var uniteDansBaremeBudget = sd.Ressource.GetUniteAssocieeRessourceDansBaremeBudget(societe);
            var uniteSousDetail = sd.Unite?.Code;

            var row = sources.NewRow();
            row.Ressource = sd.Ressource;
            row.Tache3 = sd.BudgetT4.T3 ?? sd.BudgetT4.T4.Parent;
            row["MontantBudget"] = sd.Montant;
            row["PuBudget"] = sd.PU;

            row["QuantiteBudget"] = new RessourceUnite()
            {
                Quantite = sd.Quantite,
                Unite = uniteSousDetail ?? uniteDansBaremeBudget
            };

            var valeursMoisCourant = valeursControleBudgetairePourPeriode?.FirstOrDefault(v => v.RessourceId == row.Ressource.RessourceId && v.TacheId == row.Tache3.TacheId);
            var valeursMoisPrecedent = valeursControleBudgetairePourPeriodePrecedente?.FirstOrDefault(v => v.RessourceId == row.Ressource.RessourceId && v.TacheId == row.Tache3.TacheId);
            if (sources.Valeurs.Any(v => v.Ressource.RessourceId == sd.RessourceId && v.Tache3.TacheId == sd.BudgetT4.T4.ParentId))
            {
                row["MontantAjustement"] = 0;
                row["PfaMoisPrecedent"] = 0;
            }
            else
            {
                row["MontantAjustement"] = valeursMoisCourant?.Ajustement ?? valeursMoisPrecedent?.Ajustement ?? 0;
                row["PfaMoisPrecedent"] = valeursMoisPrecedent?.Pfa;
            }

            row["CommentaireAjustement"] = valeursMoisCourant?.CommentaireAjustement ?? valeursMoisPrecedent?.CommentaireAjustement;

            return row;
        }

        protected async Task<List<BudgetDepenseModel>> GetDepensesMoisAsync(int ciId, int periode, bool cumul)
        {
            var lastDayOfMonthOfPeriode = PeriodeHelper.ToLastDayOfMonthDateTime(periode);
            var periodeDebut = cumul ? default(DateTime?) : lastDayOfMonthOfPeriode;

            SearchDepense filtre = new SearchDepense
            {
                CiId = ciId,
                PeriodeDebut = periodeDebut,
                PeriodeFin = lastDayOfMonthOfPeriode
            };

            // Le ToList est indispensable ici pour avoir de bonne performance
            // Sans celui-ci, le mapper sera appelé à chaque énumération
            IEnumerable<DepenseExhibition> depenses = await DepenseServiceMediator.GetAllDepenseExternetWithTacheAndRessourceAsync(filtre).ConfigureAwait(false);
            return BudgetDepenseMapper.Map(depenses).ToList();
        }

        protected decimal CalculProjectionLineaireDepense(decimal montantBudget, decimal montantDepense, decimal montantDad)
        {
            if (montantBudget == 0)
            {
                return montantDepense;
            }

            if (montantDad == 0 || montantDepense == 0)
            {
                // Retourne Rad + montant dépense
                return montantDepense + (montantBudget - montantDad);
            }

            return montantBudget * montantDepense / montantDad;
        }

        protected void CalculProjectionLineaire()
        {
            // GroupBy sur les ressources + T3 afin d'avoir le sous-détail par ressource / tache
            var groupedByRessource = sources.Valeurs.GroupBy(x => new { x.Ressource.RessourceId, x.Tache3.TacheId }).Select(x => new
            {
                x.Key,
                Sources = x.ToList()
            });

            groupedByRessource.ForEach(x =>
            {
                decimal? depenses = 0;
                decimal? budget = 0;
                decimal? dad = 0;

                // ForEach sur chaque sous détail du couple ressource / tache
                // Calcul la somme des dépenses / budget / DAD
                x.Sources.ForEach(w =>
                {
                    depenses += w["MontantDepense"] ?? 0;
                    budget += w["MontantBudget"] ?? 0;
                    dad += w["MontantDad"] ?? 0;
                });

                // Calcul de la projection linéaire pour chaque sous détail avec en paramètre la somme des dépenses / budget / DAD du couple ressource / tache
                // La raison du calcul de la somme est que certainne dépense pour la ressource ne sont pas forcément rempli pour le bon sous détail ce qui donne par moment des calcules impossibles
                x.Sources.ForEach(w =>
                {
                    w["ProjectionLineaire"] = CalculProjectionLineaireDepense(budget.Value, depenses.Value, dad.Value) / x.Sources.Count;
                });
            });
        }
    }
}
