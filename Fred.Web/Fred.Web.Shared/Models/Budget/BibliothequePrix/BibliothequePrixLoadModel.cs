using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Web.Shared.Models.Budget.BibliothequePrix
{
    /// <summary>
    /// Modèle de chargement d'une bibliothèque de prix.
    /// </summary>
    public class BibliothequePrixLoad
    {
        #region ResultModel

        /// <summary>
        /// Résultat du chargement d'une bibliothèque de prix.
        /// </summary>
        public class ResultModel : ErreurResultModel
        {
            /// <summary>
            /// L'identifiant de la société.
            /// </summary>
            public int SocieteId { get; set; }

            /// <summary>
            /// Liste des chapitres.
            /// </summary>
            public List<ChapitreModel> Chapitres { get; set; }

            /// <summary>
            /// Liste des organisations.
            /// </summary>
            public List<OrganisationModel> Organisations { get; private set; } = new List<OrganisationModel>();

            /// <summary>
            /// Liste des unités disponibles.
            /// </summary>
            public List<UniteModel> Unites { get; set; }

            /// <summary>
            /// Liste des devises disponibles.
            /// </summary>
            public List<DeviseModel> Devises { get; private set; } = new List<DeviseModel>();

            /// <summary>
            /// Identifiant de la devise utilisée.</param>
            /// </summary>
            public int? DeviseId { get; set; }
        }

        #endregion
        #region ReferentielModelBase

        /// <summary>
        /// Base des éléments du référentiel.
        /// </summary>
        public abstract class ReferentielModelBase : ResultModelBase
        {
            /// <summary>
            /// Le code.
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Le libellé.
            /// </summary>
            public string Libelle { get; set; }
        }

        #endregion
        #region ChapitreModel

        /// <summary>
        /// Représente un chapitre.
        /// </summary>
        [DebuggerDisplay("Chapitre {Code} - {Libelle}")]
        public class ChapitreModel : ReferentielModelBase
        {
            /// <summary>
            /// Selector permettant de constuire ce modèle a partir d'un chapitre.
            /// Exceptionnellement le Selector n'est pas une Expression mais simplement une Func car il est appelé 
            /// après l'exécution de la requete dans le repository
            /// </summary>
            public static Func<ChapitreEnt, ChapitreModel> Selector
            {
                get
                {
                    return chapitre => new ChapitreModel
                    {
                        Code = chapitre.Code,
                        Libelle = chapitre.Libelle,
                        SousChapitres = chapitre.SousChapitres.Select(SousChapitreModel.Selector).ToList()
                    };
                }
            }

            /// <summary>
            /// Liste des sous-chapitres.
            /// </summary>
            public IEnumerable<SousChapitreModel> SousChapitres { get; set; }
        }

        #endregion
        #region SousChapitreModel

        /// <summary>
        /// Représente un sous-chapitre.
        /// </summary>
        [DebuggerDisplay("Sous-chapitre {Code} - {Libelle}")]
        public class SousChapitreModel : ReferentielModelBase
        {
            /// <summary>
            /// Selector permettant de construire ce modèle a partir d'un sous-chapitre.
            /// </summary>
            public static Func<SousChapitreEnt, SousChapitreModel> Selector
            {
                get
                {
                    return sousChapitre => new SousChapitreModel
                    {
                        Code = sousChapitre.Code,
                        Libelle = sousChapitre.Libelle,
                        Ressources = sousChapitre.Ressources.Select(RessourceModel.Selector).ToList()
                    };
                }
            }

            /// <summary>
            /// Liste des ressources.
            /// </summary>
            public IEnumerable<RessourceModel> Ressources { get; set; }
        }

        #endregion
        #region RessourceModel

        /// <summary>
        /// Représente une ressource.
        /// </summary>
        [DebuggerDisplay("Ressource {Code} - {Libelle}")]
        public class RessourceModel : ReferentielModelBase
        {
            /// <summary>
            /// Selector permettant de construire ce model à partir d'une ressource.
            /// </summary>
            public static Func<RessourceEnt, RessourceModel> Selector
            {
                get
                {
                    return ressource => new RessourceModel
                    {
                        Code = ressource.Code,
                        Libelle = ressource.Libelle,
                        RessourceId = ressource.RessourceId
                    };
                }
            }

            /// <summary>
            /// L'identifiant de la ressource.
            /// </summary>
            public int RessourceId { get; set; }

        }

        #endregion
        #region OrganisationModel

        /// <summary>
        /// Représente les données d'une organisation.
        /// </summary>
        [DebuggerDisplay("Organisation {Code} - {TypeCode}")]
        public class OrganisationModel
        {
            /// <summary>
            /// L'identifiant de l'organisation.
            /// </summary>
            public int OrganisationId { get; set; }

            /// <summary>
            /// L'identifiant de la cible (comme CiId par exemple).
            /// </summary>
            public int TargetId { get; set; }

            /// <summary>
            /// Le type de l'organisation.
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// Le code de l'organisation.
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Le code du type de l'organisation.
            /// </summary>
            public string TypeCode { get; set; }

            /// <summary>
            /// Liste des éléments de la bibliothèque des prix.
            /// </summary>
            public List<ItemModel> Items { get; set; }
        }

        #endregion
        #region ItemModel

        /// <summary>
        /// Représente les données d'un élément de la bibliothèque des prix.
        /// </summary>
        public class ItemModel
        {
            /// <summary>
            /// Selector permettant de constuire ce modèle a partir d'un élément d'une bibliothèque des prix.
            /// </summary>
            public static Expression<Func<BudgetBibliothequePrixItemEnt, ItemModel>> Selector
            {
                get
                {
                    return item => new ItemModel
                    {
                        RessourceId = item.RessourceId,
                        Prix = item.Prix,
                        UniteId = item.UniteId,
                        DateCreation = item.DateCreation,
                        DateModification = item.DateModification,
                    };
                }
            }

            /// <summary>
            /// L'identifiant de la ressource.
            /// </summary>
            public int RessourceId { get; set; }

            /// <summary>
            /// Le prix de l'élément de la bibliothèque des prix.
            /// </summary>
            public decimal? Prix { get; set; }

            /// <summary>
            /// L'identifiant de l'unité de l'élément de la bibliothèque des prix.
            /// </summary>
            public int? UniteId { get; set; }

            /// <summary>
            /// Dater a laquelle la valeur a été créée
            /// </summary>
            public DateTime? DateCreation { get; set; }

            /// <summary>
            /// Date a laquelle les valeurs données sont devenues effectives
            /// </summary>
            public DateTime? DateModification { get; set; }
        }

        #endregion
        #region UniteModel

        /// <summary>
        /// Représente une unité.
        /// </summary>
        [DebuggerDisplay("Unité {Code} - {Libelle}")]
        public class UniteModel
        {
            /// <summary>
            /// Selector permettant de constuire ce modèle a partir d'une unité.
            /// </summary>
            public static Expression<Func<UniteEnt, UniteModel>> Selector
            {
                get
                {
                    return (unite) => new UniteModel
                    {
                        UniteId = unite.UniteId,
                        Code = unite.Code,
                        Libelle = unite.Libelle
                    };
                }
            }

            /// <summary>
            /// L'identifiant de l'unité.
            /// </summary>
            public int UniteId { get; set; }

            /// <summary>
            /// Le code de l'unité.
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Le libellé de l'unité.
            /// </summary>
            public string Libelle { get; set; }

            /// <summary>
            /// Indique si l'unité est disponible dans la société.
            /// </summary>
            public bool UniteSociete { get; set; }
        }

        #endregion
        #region DeviseModel

        /// <summary>
        /// Représente une devise.
        /// </summary>
        [DebuggerDisplay("Devise {Symbole} - {Libelle}")]
        public class DeviseModel
        {
            /// <summary>
            /// L'identifiant de la devise.
            /// </summary>
            public int DeviseId { get; set; }

            /// <summary>
            /// Le symbole de la devise.
            /// Correspond au symbole s'il est définit, sinon au code ISO.
            /// </summary>
            public string Symbole { get; set; }

            /// <summary>
            /// Le libellé de la devise.
            /// </summary>
            public string Libelle { get; set; }
        }

        #endregion
    }
}
