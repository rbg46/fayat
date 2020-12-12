using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.Extensions;
using Fred.Business.Budget.Helpers;
using Fred.Business.CI;
using Fred.Business.Depense.Services;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Fred.Web.Shared.Models.Budget.Depense;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace Fred.Business.Budget.ControleBudgetaire.Helpers
{
    /// <summary>
    /// Implémente l'interface pour générer un controle budgétaire avec des valeurs cumulées
    /// jusqu'a la période donnée
    /// </summary>
    public class ValeursCumuleesDataSourceBuilder : AbstractControleBudgetaireDataSourceBuilder
    {
        private List<AvancementEnt> avancementsCumules;
        private List<BudgetDepenseModel> depensesCumulees;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ValeursCumuleesDataSourceBuilder" />.
        /// </summary>
        /// <param name="controleBudgetaireManager">Manager du controle budgétaire</param>
        /// <param name="depenseServiceMediator">Service mediator des dépenses</param>
        /// <param name="budgetManager">Manager de budget</param>
        /// <param name="budgetT4Manager">Manager de budget T4</param>
        /// <param name="ciManager">Manager de CI</param>
        /// <param name="avancementManager">Manager de l'avancements</param>
        public ValeursCumuleesDataSourceBuilder(
            IControleBudgetaireManager controleBudgetaireManager,
            IDepenseServiceMediator depenseServiceMediator,
            IBudgetManager budgetManager,
            IBudgetT4Manager budgetT4Manager,
            ICIManager ciManager,
            IAvancementManager avancementManager)
            : base(controleBudgetaireManager, depenseServiceMediator, budgetManager, budgetT4Manager, ciManager, avancementManager)
        {
        }

        /// <inheritdoc/>
        public override async Task<AxeTreeDataSource> BuildDataSourceAsync(int ciId, int budgetId, int periode)
        {
            //Pas besoin de récupérer la valeur de retour, qui nous envoie la variable sources
            //qui est accessible ici par héritage
            await base.BuildDataSourceAsync(ciId, budgetId, periode).ConfigureAwait(false);

            var budgetT4s = BudgetT4Manager.GetByBudgetId(budgetEnApplicationId, true);
            depensesCumulees = await GetDepensesMoisAsync(ciId, periode, cumul: true).ConfigureAwait(false);

            //On commence par batir la source de données à partir des tâches budgétés
            avancementsCumules = AvancementManager.GetLastAvancementAvantPeriodes(budgetEnApplicationId, periode);
            foreach (var t4 in budgetT4s)
            {
                foreach (var sd in t4.BudgetSousDetails)
                {
                    sources.Valeurs.Add(BuildDataSourceLineForSousDetail(sd, sources));
                }
            }

            //Il est possible qu'une dépense ait été faite sur une ressource non budgétée
            //Par exemple on avait prévu d'utiliser une ressource mais la réalité du chantier
            //a fait qu'un pointage a été réalisé sur une autre ressource. Il faut donc ajouter une ligne 
            //liée à cette dépense dans l'arbre en plus de la ligne concernant la ressource initialement budgétée
            //D'ou la récupération de toutes les dépenses qui ne sont pas encore intégrées dans l'abre
            var depensesNonBudgetees = depensesCumulees.Where(d =>
              !sources.Valeurs.Any(v => v.Ressource.RessourceId == d.RessourceId && v.Tache3.TacheId == d.TacheId)
            );

            AddDepensesNonBudgetees(sources, depensesNonBudgetees);

            CalculProjectionLineaire();

            return sources;
        }

        /// <inheritdoc/>
        protected override AxeTreeDataSourceRow BuildDataSourceLineForSousDetail(BudgetSousDetailEnt sd, AxeTreeDataSource sources)
        {
            var row = base.BuildDataSourceLineForSousDetail(sd, sources);
            var avancementCumule = avancementsCumules.FirstOrDefault(a => a.BudgetSousDetailId == sd.BudgetSousDetailId);
            var depensesCumuleesPourTacheEtRessource = depensesCumulees.Where(d => d.RessourceId == sd.RessourceId && d.TacheId == sd.BudgetT4.T4.ParentId).ToList();

            var uniteDepense = depensesCumuleesPourTacheEtRessource.GetUniteAssocieeDepense();
            row["MontantDad"] = avancementCumule?.DAD ?? 0;
            row["PourcentageAvancement"] = avancementCumule?.GetPourcentageAvancementSousDetail(sd.BudgetT4, sd.Quantite.Value) ?? 0;

            if (sources.Valeurs.Any(v => v.Ressource.RessourceId == sd.RessourceId && v.Tache3.TacheId == sd.BudgetT4.T4.ParentId))
            {
                //On a déjà une ligne de dépense pour cette ressource pas besoin de la recompter
                row["MontantDepense"] = 0;
                row["QuantiteDepense"] = new RessourceUnite()
                {
                    Quantite = 0,
                    Unite = uniteDepense
                };
            }
            else
            {
                row["MontantDepense"] = depensesCumuleesPourTacheEtRessource.Sum(d => d.MontantHT);
                //On n'utilise pas la variable uniteDansBaremeBudget car elle peut contenir l'unité de la ressource, (càd l'unité utilisée dans le budget)
                //et ca peut poser des problèmes : imaginons qu'on ait budgété une ressource MO en Jours, les pointages (donc les dépenses) seront en Heures
                //Cette différence entre les deux unités doit être présentes dans le controle budgétaire
                row["QuantiteDepense"] = new RessourceUnite()
                {
                    Quantite = depensesCumuleesPourTacheEtRessource.Sum(d => d.Quantite),
                    Unite = uniteDepense
                };
            }

            return row;
        }
    }
}
