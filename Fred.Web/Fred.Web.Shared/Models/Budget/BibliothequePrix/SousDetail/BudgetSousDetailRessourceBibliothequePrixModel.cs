using System;
using System.Linq.Expressions;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Web.Shared.Models.Budget.BibliothequePrix.SousDetail
{
    public class BudgetSousDetailRessourceBibliothequePrixModel
    {
        public decimal? BibliothequePrixMontant { get; set; }

        public string RessourceLibelle { get; set; }

        public string RessourceCode { get; set; }

        public int? TypeRessourceId { get; set; }

        public int RessourceId { get; set; }

        public UniteEnt Unite { get; set; }

        public bool IsRecommandee { get; set; }

        /// <summary>
        /// Selector permettant de constuire ce modèle a partir d'un sous chapitre.
        /// </summary>
        public static Func<RessourceEnt, BudgetSousDetailRessourceBibliothequePrixModel> Selector
        {
            get
            {
                return ressource => new BudgetSousDetailRessourceBibliothequePrixModel
                {
                    RessourceId = ressource.RessourceId,
                    RessourceCode = ressource.Code,
                    RessourceLibelle = ressource.Libelle,
                    TypeRessourceId = ressource.TypeRessourceId,
                    IsRecommandee = ressource.IsRecommandee
                };
            }
        }
    }
}
