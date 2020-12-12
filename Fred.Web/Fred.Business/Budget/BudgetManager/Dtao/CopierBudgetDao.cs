using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Budget;
using Fred.Entities.Referential;

namespace Fred.Business.Budget.BudgetManager.Dtao
{
    /// <summary>
    /// DAO utilisé dans la copie de budget.
    /// </summary>
    internal abstract class CopierBudgetDao
    {
        /// <summary>
        /// Représente le budget source.
        /// </summary>
        public class BudgetSourceDao
        {
            /// <summary>
            /// Selector permettant de construire ce DAO à partir d'une entité.
            /// </summary>
            public Expression<Func<BudgetEnt, BudgetSourceDao>> Selector
            {
                get
                {
                    return budgetEnt => new BudgetSourceDao
                    {
                        BudgetT4s = budgetEnt.BudgetT4s.Select(bt4 => new BudgetSourceBudgetT4Dao
                        {
                            T4 = new BudgetSourceTache4Dao
                            {
                                Code = bt4.T4.Code,
                                Libelle = bt4.T4.Libelle,
                                Active = bt4.T4.Active,
                                TacheType = bt4.T4.TacheType
                            },
                            UniteId = bt4.UniteId,
                            QuantiteARealiser = bt4.QuantiteARealiser,
                            Commentaire = bt4.Commentaire,
                            TypeAvancement = bt4.TypeAvancement,
                            QuantiteDeBase = bt4.QuantiteDeBase,
                            VueSD = bt4.VueSD,
                            T3Code = bt4.T3 != null ? bt4.T3.Code : bt4.T4.Parent.Code,
                            BudgetSousDetails = bt4.BudgetSousDetails
                                .Where(bsd => !bsd.Ressource.IsRessourceSpecifiqueCi)
                                .Select(bsd => new BudgetSourceSousDetailDao
                                {
                                    RessourceId = bsd.RessourceId,
                                    Quantite = bsd.Quantite,
                                    QuantiteFormule = bsd.QuantiteFormule,
                                    PU = bsd.PU,
                                    Montant = bsd.Montant,
                                    QuantiteSD = bsd.QuantiteSD,
                                    QuantiteSDFormule = bsd.QuantiteSDFormule,
                                    Commentaire = bsd.Commentaire,
                                    UniteId = bsd.UniteId
                                })
                        })
                    };
                }
            }

            /// <summary>
            /// T4 associés à ce budget
            /// </summary>
            public IEnumerable<BudgetSourceBudgetT4Dao> BudgetT4s { get; set; }
        }

        /// <summary>
        /// Représente un BudgetT4 du budget source.
        /// </summary>
        [DebuggerDisplay("{T4?.Code}")]
        public class BudgetSourceBudgetT4Dao
        {
            /// <summary>
            /// Tâche de niveau 4.
            /// </summary>
            public BudgetSourceTache4Dao T4 { get; set; }

            /// <summary>
            /// Identifiant l'unité.
            /// </summary>
            public int? UniteId { get; set; }

            /// <summary>
            /// Quantité à réaliser.
            /// </summary>
            public decimal? QuantiteARealiser { get; set; }

            /// <summary>
            /// Commentaire.
            /// </summary>
            public string Commentaire { get; set; }

            /// <summary>
            /// Type d'avancement.
            /// </summary>
            public int? TypeAvancement { get; set; }

            /// <summary>
            /// Quantité de base.
            /// </summary>
            public decimal? QuantiteDeBase { get; set; }

            /// <summary>
            /// True pour vue sous-détail, false pour vue T4.
            /// </summary>
            public int VueSD { get; set; }

            /// <summary>
            /// Le code du T3.
            /// </summary>
            public string T3Code { get; set; }

            /// <summary>
            /// Ressources composant le sous-détail.
            /// </summary>
            public IEnumerable<BudgetSourceSousDetailDao> BudgetSousDetails { get; set; }
        }

        /// <summary>
        /// Représente un sous-détail d'un BudgetT4 du budget source.
        /// </summary>
        public class BudgetSourceSousDetailDao
        {
            /// <summary>
            /// Identifiant de la ressource.
            /// </summary>
            public int RessourceId { get; set; }

            /// <summary>
            /// Quantité de base calculée.
            /// </summary>
            public decimal? Quantite { get; set; }

            /// <summary>
            /// Formule de quantité de base calculée.
            /// </summary>
            public string QuantiteFormule { get; set; }

            /// <summary>
            /// Prix unitaire.
            /// </summary>
            public decimal? PU { get; set; }

            /// <summary>
            /// Montant.
            /// </summary>
            public decimal? Montant { get; set; }

            /// <summary>
            /// Quantité sous-détail.
            /// </summary>
            public decimal? QuantiteSD { get; set; }

            /// <summary>
            /// Quantité formule sous-détail.
            /// </summary>
            public string QuantiteSDFormule { get; set; }

            /// <summary>
            /// Commentaire.
            /// </summary>
            public string Commentaire { get; set; }

            /// <summary>
            /// Identifiant de l'unité
            /// </summary>
            public int? UniteId { get; set; }
        }

        /// <summary>
        /// Représente une tâche 4 du détail du budget source.
        /// </summary>
        public class BudgetSourceTache4Dao
        {
            /// <summary>
            /// Le code de la tâche.
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Le libellé de la tâche.
            /// </summary>
            public string Libelle { get; set; }

            /// <summary>
            /// Indique si la tâche est active.
            /// </summary>
            public bool Active { get; set; } = true;

            /// <summary>
            /// Le type de la tâche.
            /// </summary>
            public int TacheType { get; set; }
        }

        /// <summary>
        /// Représente un item de la bibliothèque des prix.
        /// </summary>
        public class BibliothequePrixItemDao
        {
            /// <summary>
            /// Prix.
            /// </summary>
            public decimal? Prix { get; set; }

            /// <summary>
            /// Identifiant de l'unité.
            /// </summary>
            public int? UniteId { get; set; }

            /// <summary>
            /// Identifiant de la ressource.
            /// </summary>
            public int RessourceId { get; set; }

            /// <summary>
            /// Selector permettant de constuire ce DAO a partir d'une entité.
            /// </summary>
            public Expression<Func<BudgetBibliothequePrixItemEnt, BibliothequePrixItemDao>> Selector
            {
                get
                {
                    return item => new BibliothequePrixItemDao
                    {
                        Prix = item.Prix,
                        UniteId = item.UniteId,
                        RessourceId = item.RessourceId
                    };
                }
            }
        }

        /// <summary>
        /// Représente une tâche cible.
        /// </summary>
        public class BudgetCibleTacheDao
        {
            /// <summary>
            /// Selector permettant de constuire ce DTO à partir d'une tâche.
            /// </summary>
            public Expression<Func<TacheEnt, BudgetCibleTacheDao>> Selector
            {
                get
                {
                    return tacheEnt => new BudgetCibleTacheDao
                    {
                        TacheId = tacheEnt.TacheId,
                        Code = tacheEnt.Code,
                    };
                }
            }

            /// <summary>
            /// L'identifiant de la tache.
            /// </summary>
            public int TacheId { get; set; }

            /// <summary>
            /// Le code de la tâche.
            /// </summary>
            public string Code { get; set; }
        }
    }
}
