using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.ReferentielFixe;

namespace Fred.Web.Shared.Models.Budget.BibliothequePrix.SousDetail
{
    public class BudgetSousDetailChapitreBibliothequePrixModel
    {
        public int ChapitreId { get; set; }

        public string ChapitreCode { get; set; }

        public string ChapitreLibelle { get; set; }

        public IEnumerable<BudgetSousDetailSousChapitreBibliothequePrixModel> SousChapitres { get; set; }

        /// <summary>
        /// Selector permettant de constuire ce modèle a partir d'un chapitre.
        /// Exceptionnellement le Selector n'est pas une Expression mais simplement une Func car il est appelé 
        /// après l'exécution de la requete dans le repository
        /// </summary>
        public static Func<ChapitreEnt, BudgetSousDetailChapitreBibliothequePrixModel> Selector
        {
            get
            {
                return chapitre => new BudgetSousDetailChapitreBibliothequePrixModel
                {
                    ChapitreId = chapitre.ChapitreId,
                    ChapitreCode = chapitre.Code,
                    ChapitreLibelle = chapitre.Libelle,
                    SousChapitres = chapitre.SousChapitres.Select(BudgetSousDetailSousChapitreBibliothequePrixModel.Selector).ToList()
                };
            }
        }
    }
}
