using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.DataAccess.Interfaces;
using Fred.Entities.IndemniteDeplacement;

namespace Fred.Business.IndemniteDeplacement
{

    /// <inheritdoc />
    public class SearchFeature : ManagerFeature<IIndemniteDeplacementRepository>, ISearchFeature
    {

        /// <summary>
        /// Instancie un nouvel objet SearchFeature
        /// </summary>
        /// <param name="repository">le repository des Indemnités de déplacement</param>
        /// <param name="uow"> Unit of work</param>
        public SearchFeature(IIndemniteDeplacementRepository repository, IUnitOfWork uow)
          : base(uow, repository)
        {
        }

        /// <inheritdoc />
        public SearchIndemniteDeplacementEnt GetDefaultFilter()
        {
            var recherche = new SearchIndemniteDeplacementEnt();
            recherche.Ci = true;
            recherche.CodeDeplacement = true;
            recherche.CodeZoneDeplacement = false;
            return recherche;
        }


        /// <inheritdoc/>
        public IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementAllByPersonnelId(int personnelId)
        {
            return Repository.SearchIndemniteDeplacementAllByPersonnelIdWithFilters(personnelId);
        }



        /// <inheritdoc/>
        public IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementAllWithFilters(string text, SearchIndemniteDeplacementEnt filters)
        {
            return Repository.SearchIndemniteDeplacementAllWithFilters(GetPredicate(text, filters, null, null));
        }



        /// <inheritdoc/>
        public IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementWithFilters(string text, SearchIndemniteDeplacementEnt filters, int personnelId)
        {
            return Repository.SearchIndemniteDeplacementWithFilters(GetPredicate(text, filters), personnelId);
        }


#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'une indemnite de deplacement.
        /// </summary>
        /// <param name="text">Texte recherché dans les indemnites de deplacement</param>
        /// <param name="filters">Filtres pour la recherche</param>
        /// <param name="personnelId">Id du personnel recherché</param>
        /// <param name="ciId">Id du CI</param>
        /// <returns>Retourne la condition de recherche des indemnites de deplacement</returns>
        private Func<IndemniteDeplacementEnt, bool> GetPredicate(string text, SearchIndemniteDeplacementEnt filters, int? personnelId, int? ciId)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            if (string.IsNullOrEmpty(text))
            {
                return p => (filters.Actif ? !p.DateSuppression.HasValue : true)
                            && (personnelId.HasValue && personnelId.Value > 0 ? p.PersonnelId.Equals(personnelId.Value) : true)
                            && (ciId.HasValue && ciId.Value > 0 ? p.CiId.Equals(ciId.Value) : true);
            }

            return p => ((filters.Ci ? p.CI.Libelle.ToLower().Contains(text.ToLower()) : false)
                         || (filters.CodeDeplacement ? p.CodeDeplacement.Libelle.ToLower().Contains(text.ToLower()) : false)
                         || (filters.CodeZoneDeplacement ? p.CodeZoneDeplacement.Libelle.ToLower().Contains(text.ToLower()) : false))
                        && (filters.Actif ? !p.DateSuppression.HasValue : true)
                        && (personnelId.HasValue && personnelId.Value > 0 ? p.PersonnelId.Equals(personnelId.Value) : true)
                        && (ciId.HasValue && ciId.Value > 0 ? p.CiId.Equals(ciId.Value) : true);
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'une indemnite de deplacement.
        /// </summary>
        /// <param name="text">Texte recherché dans les indemnites de deplacement</param>
        /// <param name="filters">Filtres pour la recherche</param>
        /// <returns>Retourne la condition de recherche des indemnites de deplacement</returns>
        private Expression<Func<IndemniteDeplacementEnt, bool>> GetPredicate(string text, SearchIndemniteDeplacementEnt filters)
        {
            return p => string.IsNullOrEmpty(text)
                        || filters.Ci && (p.CI.Libelle.ToLower().Contains(text.ToLower()) || p.CI.Code.ToLower().Contains(text.ToLower()))
                        || filters.CodeDeplacement && p.CodeDeplacement.Libelle.ToLower().Contains(text.ToLower())
                        || filters.CodeZoneDeplacement && p.CodeZoneDeplacement.Libelle.ToLower().Contains(text.ToLower());
        }
    }
}
