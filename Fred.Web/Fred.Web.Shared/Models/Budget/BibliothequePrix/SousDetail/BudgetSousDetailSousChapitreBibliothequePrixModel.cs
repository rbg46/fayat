using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.ReferentielFixe;

namespace Fred.Web.Shared.Models.Budget.BibliothequePrix.SousDetail
{
    public class BudgetSousDetailSousChapitreBibliothequePrixModel
    {
        public int SousChapitreId { get; set; }

        public string SousChapitreCode { get; set; }

        public string SousChapitreLibelle { get; set; }

        public IEnumerable<BudgetSousDetailRessourceBibliothequePrixModel> Ressources { get; set; }


        /// <summary>
        /// Selector permettant de constuire ce modèle a partir d'un sous chapitre.
        /// </summary>
        public static Func<SousChapitreEnt, BudgetSousDetailSousChapitreBibliothequePrixModel> Selector
        {
            get
            {
                return sousChapitre => new BudgetSousDetailSousChapitreBibliothequePrixModel
                {
                    SousChapitreId = sousChapitre.SousChapitreId,
                    SousChapitreCode = sousChapitre.Code,
                    SousChapitreLibelle = sousChapitre.Libelle,
                    Ressources = sousChapitre.Ressources.Select(BudgetSousDetailRessourceBibliothequePrixModel.Selector).ToList()
                };
            }
        }
    }
}
