using System.Collections.Generic;
using System.Linq;
using Fred.Business.Budget.BudgetComparaison.ExcelExport;
using Fred.Business.Budget.Helpers;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Framework.Extensions;

namespace Fred.Business.Budget.BudgetComparaison
{
    /// <summary>
    /// Gestionnaire de la comparaison de budget.
    /// </summary>
    public class BudgetComparaisonManager : IBudgetComparaisonManager
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IBudgetComparer budgetComparer;
        private readonly IBudgetComparaisonExcelExporter budgetComparaisonExcelExporter;
        private readonly IBudgetRepository budgetRepository;

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="utilisateurManager">Gestionnaire des utilisateurs.</param>
        /// <param name="budgetComparer">Comparateur de budget.</param>
        /// <param name="budgetComparaisonExcelExporter">Exporteur Excel d'une comparaison de budget.</param>
        /// <param name="budgetRepository">Référentiel de données pour les budgets.</param>
        public BudgetComparaisonManager(
            IUtilisateurManager utilisateurManager,
            IBudgetComparer budgetComparer,
            IBudgetComparaisonExcelExporter budgetComparaisonExcelExporter,
            IBudgetRepository budgetRepository)
        {
            this.budgetComparer = budgetComparer;
            this.budgetComparaisonExcelExporter = budgetComparaisonExcelExporter;
            this.budgetRepository = budgetRepository;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Recherche des révisions de budget d'un CI.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI concerné.</param>
        /// <param name="page">L'index de la page.</param>
        /// <param name="pageSize">La taille d'un page.</param>
        /// <param name="recherche">Le texte recherché.</param>
        /// <returns>Les révisions de budget concernées.</returns>
        public List<Dto.BudgetRevisionDto> SearchBudgetRevisions(int ciId, int page, int pageSize, string recherche)
        {
            // Toutes les révisions du CI doivent être remontées car la recherche s'effectue en partie sur les
            //  périodes de début et de fin d'une révision de budget qui dépendent de la culture (ex : "janvier 2019")
            //  une convertion DAO vers DTO est requise pour cela
            // Il faut noter que pour le moment c'est la culture du serveur qui est utilisée et non celle de l'utilisateur...
            var utilisateurId = utilisateurManager.GetContextUtilisateurId();
            return budgetRepository.GetBudgetRevisionsPourBudgetComparaison(ciId, utilisateurId)
                .Select(r => new Dto.BudgetRevisionDto
                {
                    BudgetId = r.BudgetId,
                    Revision = r.Revision,
                    Etat = r.Etat,
                    PeriodeDebut = r.PeriodeDebut.HasValue ? PeriodeHelper.FormatCulture(r.PeriodeDebut.Value) : null,
                    PeriodeFin = r.PeriodeFin.HasValue ? PeriodeHelper.FormatCulture(r.PeriodeFin.Value) : null
                })
                .Search(recherche, r => new { r.Revision, r.Etat, r.PeriodeDebut, r.PeriodeFin })
                .Pagine(page, pageSize)
                .ToList();
        }

        /// <summary>
        /// Compare des budgets.
        /// </summary>
        /// <param name="request">La requête de comparaison.</param>
        /// <returns>Le résultat de la comparaison.</returns>
        public Dto.Comparaison.ResultDto Compare(Dto.Comparaison.RequestDto request)
        {
            return budgetComparer.Compare(request);
        }

        /// <summary>
        /// Exporte la comparaison au format Excel.
        /// </summary>
        /// <param name="request">La requête de l'export Excel.</param>
        /// <returns>Le résultat de l'export.</returns>
        public Dto.ExcelExport.ResultDto ExcelExport(Dto.ExcelExport.RequestDto request)
        {
            return budgetComparaisonExcelExporter.Export(request);
        }
    }
}
