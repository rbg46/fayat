using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Web.Shared.Models.Budget.Details.SousDetail;

namespace Fred.Web.Shared.Models.Budget
{
    /// <summary>
    /// Modèles de chargement d'un sous-détail d'un budget.
    /// </summary>
    public class BudgetSousDetailLoad
    {
        #region Model

        /// <summary>
        /// Modèle de chargement d'un sous-détail d'un budget.
        /// </summary>
        [DebuggerDisplay("BudgetT4 (id = {BudgetT4Id})")]
        public class Model : ErreurResultModel
        {
            /// <summary>
            /// Identifiant du détail.
            /// </summary>
            public int BudgetT4Id { get; private set; }

            /// <summary>
            /// Elements du sous-détail.
            /// </summary>
            public List<ItemModel> Items { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="budgetT4Id">Identifiant du détail.</param>
            public Model(int budgetT4Id)
            {
                BudgetT4Id = budgetT4Id;
                Items = new List<ItemModel>();
            }
        }

        #endregion
        #region ItemModel

        /// <summary>
        /// Représente un élément d'un sous-détail d'une tâche T4.
        /// </summary>
        [DebuggerDisplay("Item (id = {BudgetSousDetailId})")]
        public class ItemModel : ErreurResultModel
        {
            /// <summary>
            /// L'identifiant du sous-détail.
            /// </summary>
            public int BudgetSousDetailId { get; private set; }

            /// <summary>
            /// Le prix unitaire.
            /// </summary>
            public decimal? PrixUnitaire { get; private set; }

            /// <summary>
            /// La quantité SD.
            /// </summary>
            public decimal? QuantiteSD { get; private set; }

            /// <summary>
            /// La quantité SD formule.
            /// </summary>
            public string QuantiteSDFormule { get; private set; }

            /// <summary>
            /// La quantité.
            /// </summary>
            public decimal? Quantite { get; private set; }

            /// <summary>
            /// La quantité.
            /// </summary>
            public string QuantiteFormule { get; private set; }

            /// <summary>
            /// Le montant.
            /// </summary>
            public decimal? Montant { get; private set; }

            /// <summary>
            /// Le chapitre correspondant.
            /// </summary>
            public ChapitreModel Chapitre { get; private set; }

            /// <summary>
            /// Le sous-chapitre correspondant.
            /// </summary>
            public SousChapitreModel SousChapitre { get; private set; }

            /// <summary>
            /// La ressource correspondante.
            /// </summary>
            public RessourceModel Ressource { get; private set; }

            /// <summary>
            /// Le commentaire.
            /// </summary>
            public string Commentaire { get; private set; }

            /// <summary>
            /// L'unité.
            /// </summary>
            public UniteEnt Unite { get; private set; }

            /// <summary>
            /// Représente le prix associé à la ressource dans la bibliotheque des prix pour le CI ou l'établissement contenant le budget
            /// </summary>
            public decimal? PuBibliothequePrix { get; private set; }

            /// <summary>
            /// Représente l'unité associée à la ressource dans la bibliotheque des prix pour le CI ou l'établissement contenant le budget
            /// </summary>
            public int? UniteIdBibliothequePrix { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="budgetSousDetailEnt">L'entité du sous-détail.</param>
            /// <param name="ressourceEnt">L'entié de la ressource associée.</param>
            /// <param name="bibliothequePrixItem">Bibliotheque des prix, l'unité doit être chargée, potentiellement null</param>
            public ItemModel(BudgetSousDetailEnt budgetSousDetailEnt, RessourceEnt ressourceEnt, BibliothequePrixItemModel bibliothequePrixItem)
            {
                BudgetSousDetailId = budgetSousDetailEnt.BudgetSousDetailId;
                PrixUnitaire = budgetSousDetailEnt.PU;
                QuantiteSD = budgetSousDetailEnt.QuantiteSD;
                QuantiteSDFormule = budgetSousDetailEnt.QuantiteSDFormule;
                Quantite = budgetSousDetailEnt.Quantite;
                QuantiteFormule = budgetSousDetailEnt.QuantiteFormule;
                Montant = budgetSousDetailEnt.Montant;
                Chapitre = new ChapitreModel(ressourceEnt.SousChapitre.Chapitre);
                SousChapitre = new SousChapitreModel(ressourceEnt.SousChapitre);
                Ressource = new RessourceModel(ressourceEnt);
                Commentaire = budgetSousDetailEnt.Commentaire;
                Unite = budgetSousDetailEnt.Unite;

                PuBibliothequePrix = bibliothequePrixItem?.Prix;
                UniteIdBibliothequePrix = bibliothequePrixItem?.UniteId;
            }
        }

        #endregion
        #region ChapitreModel

        /// <summary>
        /// Représente le chapitre d'un sous-détail.
        /// </summary>
        [DebuggerDisplay("Chapitre {Code} {Libelle} (id = {ChapitreId})")]
        public class ChapitreModel : ReferentielEtenduModel
        {
            /// <summary>
            /// Identifiant du chapitre.
            /// </summary>
            public int ChapitreId { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="chapitreEnt">L'entité du chapitre concerné.</param>
            public ChapitreModel(ChapitreEnt chapitreEnt)
              : base(chapitreEnt.Code, chapitreEnt.Libelle)
            {
                ChapitreId = chapitreEnt.ChapitreId;
            }
        }

        #endregion
        #region SousChapitreModel

        /// <summary>
        /// Représente le sous-chapitre d'un sous-détail.
        /// </summary>
        [DebuggerDisplay("Sous-chapitre {Code} {Libelle} (id = {SousChapitreId})")]
        public class SousChapitreModel : ReferentielEtenduModel
        {
            /// <summary>
            /// Identifiant du sous-chapitre.
            /// </summary>
            public int SousChapitreId { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="chapitreEnt">L'entité du sous-chapitre concerné.</param>
            public SousChapitreModel(SousChapitreEnt sousChapitreEnt)
              : base(sousChapitreEnt.Code, sousChapitreEnt.Libelle)
            {
                SousChapitreId = sousChapitreEnt.SousChapitreId;
            }
        }

        #endregion
        #region RessourceModel

        /// <summary>
        /// Représente la ressource d'un sous-détail.
        /// </summary>
        [DebuggerDisplay("Ressource {Code} {Libelle} (id = {RessourceId})")]
        public class RessourceModel : ReferentielEtenduModel
        {
            /// <summary>
            /// Identifiant de la ressource.
            /// </summary>
            public int RessourceId { get; private set; }

            /// <summary>
            /// Identifiant de l'unité ou 0.
            /// </summary>
            public int UniteId { get; private set; }

            /// <summary>
            /// Identifiant du type de la ressource.
            /// </summary>
            public int? TypeRessourceId { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="chapitreEnt">L'entité de la ressource concernée.</param>
            public RessourceModel(RessourceEnt ressourceEnt)
              : base(ressourceEnt.Code, ressourceEnt.Libelle)
            {
                RessourceId = ressourceEnt.RessourceId;
                TypeRessourceId = ressourceEnt.TypeRessourceId;

                if (ressourceEnt.ReferentielEtendus != null && ressourceEnt.ReferentielEtendus.Count > 0)
                {
                    var referentielEtendu = ressourceEnt.ReferentielEtendus.First();
                    if (referentielEtendu.ParametrageReferentielEtendus != null && referentielEtendu.ParametrageReferentielEtendus.Count > 0)
                    {
                        var parametrageReferentielEtendu = referentielEtendu.ParametrageReferentielEtendus.First();
                        UniteId = parametrageReferentielEtendu.UniteId;
                    }
                }
            }
        }

        #endregion
        #region ReferentielEtenduModel

        /// <summary>
        /// Classe de base pour le chapitre, sous-chapitre et ressource d'un sous-détail.
        /// </summary>
        [DebuggerDisplay("Referentiel étendu {Code} {Libelle}")]
        public class ReferentielEtenduModel : ResultModelBase
        {
            /// <summary>
            /// Code du référentiel.
            /// </summary>
            public string Code { get; private set; }

            /// <summary>
            /// Libellé du référentiel.
            /// </summary>
            public string Libelle { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="code">Code du référentiel.</param>
            /// <param name="libelle">Libellé du référentiel.</param>
            public ReferentielEtenduModel(string code, string libelle)
            {
                Code = code;
                Libelle = libelle;
            }
        }

        #endregion
    }
}
