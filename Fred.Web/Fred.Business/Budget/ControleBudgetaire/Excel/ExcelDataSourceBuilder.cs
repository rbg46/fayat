using Fred.Business.Budget.Avancement;
using Fred.Business.Budget.Extensions;
using Fred.Business.Budget.Helpers;
using Fred.Business.Budget.Mapper;
using Fred.Business.Depense;
using Fred.Business.Depense.Services;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Fred.Web.Shared.Models.Budget.Depense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Fred.Business.Budget.Helpers.ComputeColumnValueForAxe;

namespace Fred.Business.Budget.ControleBudgetaire.Excel
{
    /// <summary>
    /// Les valeurs et le format du fichier excel nous empeche d'utiliser les builder du controle budgétaire au format web.
    /// Cette classe permet donc de le faire
    /// </summary>
    public class ExcelDataSourceBuilder
    {
        private List<BudgetDepenseModel> depensesMoisCourant;
        private List<BudgetDepenseModel> depensesCumulees;

        private readonly IAvancementManager avancementManager;
        private readonly IDepenseServiceMediator depenseServiceMediator;
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly IControleBudgetaireManager controleBudgetaireManager;
        private readonly AxeTreeDataSource sources = new AxeTreeDataSource();

        private IEnumerable<ControleBudgetaireValeursEnt> valeursControleBudgetairePourPeriode;
        private IEnumerable<ControleBudgetaireValeursEnt> valeursControleBudgetairePourPeriodePrecedente;
        private List<AvancementEnt> derniersAvancementsCumules;
        private List<AvancementEnt> avancementsCumules;
        private List<AvancementEnt> avancementsCumulesMoisPrecedent;

        /// <summary>
        /// Implémentation du constructeur. 
        /// /// </summary>
        /// <param name="avancementManager">Manager de l'avancement</param>
        /// <param name="depenseServiceMediator">service mediator de dépenses</param>
        /// <param name="budgetT4Manager">Manager de budget T4</param>
        /// <param name="controleBudgetaireManager">Manager de controle budgetaire</param>
        public ExcelDataSourceBuilder(IAvancementManager avancementManager, IDepenseServiceMediator depenseServiceMediator, IBudgetT4Manager budgetT4Manager, IControleBudgetaireManager controleBudgetaireManager)
        {
            this.avancementManager = avancementManager;
            this.depenseServiceMediator = depenseServiceMediator;
            this.budgetT4Manager = budgetT4Manager;
            this.controleBudgetaireManager = controleBudgetaireManager;

        }

        /// <summary>
        /// Construit la source de données pour l'export excel du controle budgétaire pour le mois en cours
        /// </summary>
        /// <param name="ciId">id du ci dont on veut le budget</param>
        /// <param name="budgetId">id du budget en application</param>
        /// <param name="periode">periode délimitant les valeurs qu'on va récupérer</param>
        /// <returns>Une source de données exploitable par l'AxeTreeBuilder</returns>
        public async Task<AxeTreeDataSource> BuildDataSourceAsync(int ciId, int budgetId, int periode)
        {
            sources.AddColumn("MontantBudget", SumChildrenValues);

            sources.AddColumn("MontantDadMoisCourant", SumChildrenValues);
            sources.AddColumn("MontantDepenseMoisCourant", SumChildrenValues);
            sources.AddColumn("PourcentageAvancementMoisCourant", ComputePourcentAvancement);

            sources.AddColumn("MontantDad", SumChildrenValues);
            sources.AddColumn("MontantDepense", SumChildrenValues);
            sources.AddColumn("PourcentageAvancement", ComputePourcentAvancement);

            sources.AddColumn("MontantAjustement", SumChildrenValues);
            sources.AddColumn("PfaMoisCourant", SumChildrenValues);
            sources.AddColumn("EcartMoisPrecedent", SumChildrenValues);

            var periodePrecedente = PeriodeHelper.GetPreviousPeriod(periode).Value;
            var budgetT4s = budgetT4Manager.GetByBudgetId(budgetId, true);

            depensesMoisCourant = await GetDepensesMoisAsync(ciId, periode, cumul: false).ConfigureAwait(false);
            depensesCumulees = await GetDepensesMoisAsync(ciId, periode, cumul: true).ConfigureAwait(false);

            //On commence par batir la source de données à partir des tâches budgétés
            valeursControleBudgetairePourPeriode = controleBudgetaireManager.GetControleBudgetaireValeurs(budgetId, periode);
            valeursControleBudgetairePourPeriodePrecedente = controleBudgetaireManager.GetControleBudgetaireValeurs(budgetId, periodePrecedente);
            derniersAvancementsCumules = avancementManager.GetLastAvancementAvantPeriodes(budgetId, periode);
            avancementsCumules = avancementManager.GetAvancements(budgetId, periode);
            avancementsCumulesMoisPrecedent = avancementManager.GetAvancements(budgetId, periodePrecedente);
            foreach (var t4 in budgetT4s)
            {
                foreach (var sd in t4.BudgetSousDetails)
                {
                    sources.Valeurs.Add(BuildDataSourceLineForSousDetail(sd, sources));
                }
            }

            var depensesNonBudgeteesMoisEnCours = depensesMoisCourant
                .Where(d => !sources.Valeurs.Any(v => v.Ressource.RessourceId == d.RessourceId && v.Tache3.TacheId == d.TacheId))
                .ToList();

            var depensesNonBudgeteesCumulees = depensesCumulees
                 .Where(d => !sources.Valeurs.Any(v => v.Ressource.RessourceId == d.RessourceId && v.Tache3.TacheId == d.TacheId))
                 .ToList();

            AddDepensesNonBudgetees(sources, depensesNonBudgeteesMoisEnCours, cumul: false);
            AddDepensesNonBudgetees(sources, depensesNonBudgeteesCumulees, cumul: true);


            return sources;
        }

        /// <summary>
        /// Construit la source de données pour l'export excel du controle budgétaire
        /// </summary>
        /// <param name="axeTrees">Liste de AxeTree</param>
        /// <returns>Liste de AxeTree avec les valeurs souhaitées</returns>
        public List<AxeTreeModel> BuildDataSource(List<AxeTreeModel> axeTrees)
        {
            foreach (AxeTreeModel axe in axeTrees)
            {
                axe.Valeurs.Add("MontantDadMoisCourant", 0m);
                axe.Valeurs.Add("MontantDepenseMoisCourant", 0m);
                axe.Valeurs.Add("PourcentageAvancementMoisCourant", 0m);
            }

            return axeTrees;
        }

        /// <summary>
        /// Construit une nouvelle ligne pour le sous détail
        /// Cette méthode n'appelle pas la classe de base, car elle est faite justement pour cacher l'implémentation de la classe de base
        /// </summary>
        /// <param name="sd">ligne de sous détail que l'on rajoute</param>
        /// <param name="sources">sources de données qu'on rempli</param>
        /// <returns>la ligne remplie avec toutes les données</returns>
        protected AxeTreeDataSourceRow BuildDataSourceLineForSousDetail(BudgetSousDetailEnt sd, AxeTreeDataSource sources)
        {
            var dernierAvancementCumule = derniersAvancementsCumules.FirstOrDefault(a => a.BudgetSousDetailId == sd.BudgetSousDetailId);
            var avancementCumule = avancementsCumules.FirstOrDefault(a => a.BudgetSousDetailId == sd.BudgetSousDetailId);
            var avancementCumuleMoisPrecedent = avancementsCumulesMoisPrecedent.FirstOrDefault(a => a.BudgetSousDetailId == sd.BudgetSousDetailId);

            var row = sources.NewRow();
            row.Ressource = sd.Ressource;
            row.Tache3 = sd.BudgetT4.T4.Parent;

            row["MontantBudget"] = sd.Montant;

            row["MontantDadMoisCourant"] = (avancementCumule?.DAD ?? 0) - (avancementCumuleMoisPrecedent?.DAD ?? 0);

            var pourcentageAvancement = avancementCumule?.GetPourcentageAvancementSousDetail(sd.BudgetT4, sd.Quantite.Value) ?? 0;
            var pourcentageAvancementMoisPrecedent = avancementCumuleMoisPrecedent?.GetPourcentageAvancementSousDetail(sd.BudgetT4, sd.Quantite.Value) ?? 0;
            row["PourcentageAvancementMoisCourant"] = pourcentageAvancement - pourcentageAvancementMoisPrecedent;


            row["MontantDad"] = dernierAvancementCumule?.DAD ?? 0;
            row["PourcentageAvancement"] = dernierAvancementCumule?.GetPourcentageAvancementSousDetail(sd.BudgetT4, sd.Quantite.Value) ?? 0;

            if (sources.Valeurs.Any(v => v.Ressource.RessourceId == sd.RessourceId && v.Tache3.TacheId == sd.BudgetT4.T4.ParentId))
            {
                //On a déjà une ligne de dépense pour cette ressource pas besoin de la recompter
                row["MontantDepense"] = 0;
                row["MontantDepenseMoisCourant"] = 0;
                row["MontantAjustement"] = 0;
                row["PfaMoisCourant"] = 0;
                row["EcartMoisPrecedent"] = 0;
            }
            else
            {
                row["MontantDepense"] = depensesCumulees.Where(d => d.RessourceId == sd.RessourceId && d.TacheId == sd.BudgetT4.T4.ParentId).Sum(d => d.MontantHT);
                row["MontantDepenseMoisCourant"] = depensesMoisCourant.Where(d => d.RessourceId == sd.RessourceId && d.TacheId == sd.BudgetT4.T4.ParentId).Sum(d => d.MontantHT);

                var valeursMoisCourant = valeursControleBudgetairePourPeriode?.FirstOrDefault(v => v.RessourceId == row.Ressource.RessourceId && v.TacheId == row.Tache3.TacheId);
                var valeursMoisPrecedent = valeursControleBudgetairePourPeriodePrecedente?.FirstOrDefault(v => v.RessourceId == row.Ressource.RessourceId && v.TacheId == row.Tache3.TacheId);

                row["MontantAjustement"] = valeursMoisCourant?.Ajustement ?? valeursMoisPrecedent?.Ajustement ?? 0;
                row["PfaMoisCourant"] = valeursMoisCourant?.Pfa;
                row["EcartMoisPrecedent"] = valeursMoisCourant?.Pfa - (valeursMoisPrecedent?.Pfa != null ? valeursMoisPrecedent?.Pfa : 0);
            }
            return row;
        }

        /// <summary>
        /// Ajoute a la source de données, l'ensemble des dépenses non budgétées.
        /// Ces dépenses peuvent concerné le mois en cours ou un cumul jusqu'au mois en cours.
        /// </summary>
        /// <param name="sources">sources de données à laquelle on va rajouter les dépenses</param>
        /// <param name="depensesNonBudgetees">la liste des dépenses non budgétées à rajouter</param>
        /// <param name="cumul">False si les dépenses ne concernent que le mois courant, True si elles sont cumulées</param>
        protected void AddDepensesNonBudgetees(AxeTreeDataSource sources, IEnumerable<BudgetDepenseModel> depensesNonBudgetees, bool cumul)
        {
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

                var columnName = cumul ? "MontantDepense" : "MontantDepenseMoisCourant";
                row[columnName] = depenseNonBudgete.Depense.Sum(d => d.MontantHT);

                //Si cette fonction est appelée pour par exemple traiter les dépenses cumulées, on peut être dans le cas ou 
                //La dépense actuelle (depenseNonBudgete) a été saisie sur le mois courant est donc existe déjà dans la source
                //Dans ce cas il ne faut pas la recompter car cela fausserait le calcul de l'ajustement
                var depenseDejaIncluse = sources.Valeurs.Any(v => v.Ressource?.RessourceId == depenseNonBudgete.Ressource?.RessourceId &&
                                                             v.Tache3.TacheId == depenseNonBudgete.Tache.TacheId);
                if (!depenseDejaIncluse)
                {
                    var valeursMoisCourant = valeursControleBudgetairePourPeriode?.FirstOrDefault(v => v.RessourceId == row.Ressource.RessourceId && v.TacheId == row.Tache3.TacheId);
                    var valeursMoisPrecedent = valeursControleBudgetairePourPeriodePrecedente?.FirstOrDefault(v => v.RessourceId == row.Ressource.RessourceId && v.TacheId == row.Tache3.TacheId);
                    row["MontantAjustement"] = valeursMoisCourant?.Ajustement ?? valeursMoisPrecedent?.Ajustement ?? 0;
                    row["PfaMoisCourant"] = valeursMoisCourant?.Pfa;
                    row["EcartMoisPrecedent"] = valeursMoisCourant?.Pfa - (valeursMoisPrecedent?.Pfa != null ? valeursMoisPrecedent?.Pfa : 0);
                }
                else
                {
                    row["MontantAjustement"] = 0;
                    row["PfaMoisCourant"] = 0;
                    row["EcartMoisPrecedent"] = 0;
                }

                sources.Valeurs.Add(row);
            }
        }

        /// <summary>
        /// Récupère toutes les dépenses pour le mois, sur ce CI
        /// </summary>
        /// <param name="ciId">Id du ci dont on récupère les dépenses</param>
        /// <param name="periode">Periode délimitant les dépenses récupérées</param>
        /// <param name="cumul">Si true, alors on récupère les dépenses de la création du CI jusqu'à la periode
        /// Si false alors on récupère les dépenses juste sur la periode en cours (donc sur un mois)</param>
        /// <returns>La liste des dépenses, peut être vide, mais jamais null</returns>
        protected async Task<List<BudgetDepenseModel>> GetDepensesMoisAsync(int ciId, int periode, bool cumul)
        {
            var lastDayOfMonthOfPeriode = PeriodeHelper.ToLastDayOfMonthDateTime(periode);
            var periodeDebut = cumul ? default(DateTime?) : lastDayOfMonthOfPeriode;

            var filtre = new SearchDepense
            {
                CiId = ciId,
                PeriodeDebut = periodeDebut,
                PeriodeFin = lastDayOfMonthOfPeriode
            };

            // Le ToList est indispensable ici pour avoir de bonne performance
            // Sans celui-ci, le mapper sera appelé à chaque énumération
            return BudgetDepenseMapper.Map(await depenseServiceMediator.GetAllDepenseExternetWithTacheAndRessourceAsync(filtre).ConfigureAwait(false)).ToList();
        }
    }
}
