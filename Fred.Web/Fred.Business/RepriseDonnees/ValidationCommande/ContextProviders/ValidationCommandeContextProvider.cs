using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.ValidationCommande.ContextProviders
{
    public class ValidationCommandeContextProvider : IValidationCommandeContextProvider
    {
        private readonly IRepriseDonneesRepository repriseDonneesRepository;
        private readonly IRepriseCommandeRepository repriseCommandeRepository;

        public ValidationCommandeContextProvider(
            IRepriseDonneesRepository repriseDonneesRepository,
            IRepriseCommandeRepository repriseCommandeRepository)
        {
            this.repriseDonneesRepository = repriseDonneesRepository;
            this.repriseCommandeRepository = repriseCommandeRepository;
        }

        /// <summary>
        /// Fournit les données necessaires la validation en masse des commandes
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelValidationCommandes">repriseExcelValidationCommandes</param>
        /// <returns>les données necessaires la validation en masse des commandes</returns>
        public ContextForValidationCommande GetContextForValidationCommandes(int groupeId, List<RepriseExcelValidationCommande> repriseExcelValidationCommandes)
        {
            var result = new ContextForValidationCommande();

            var numeroCommandeExternes = repriseExcelValidationCommandes.Select(x => x.NumeroCommandeExterne).Distinct().ToList();

            result.CommandesUsedInExcel = repriseCommandeRepository.GetCommandes(numeroCommandeExternes);

            result.StatutCommandeValidee = repriseDonneesRepository.GetStatusCommandeValidee();

            return result;
        }

    }
}
