using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.CI;
using Fred.Entities.Common.CommonSearchExpression;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Search;
using LinqKit;

namespace Fred.Entities.Commande
{
    [Serializable] // necessaire pour les favoris
    public class SearchReceivableOrdersFilter : AbstractSearch // necessaire pour les favoris
    {
        private readonly List<string> statutCodes;

        public SearchReceivableOrdersFilter()
        {
            statutCodes = new List<string>()
            {
                StatutCommandeEnt.CommandeStatutVA,
                StatutCommandeEnt.CommandeStatutMVA
            };
        }

        /// <summary>
        ///   Identifiant Fournisseur recherché
        /// </summary>
        public int? FournisseurId { get; set; }

        public FournisseurLightEnt Fournisseur { get; set; } // necessaire pour les favoris

        /// <summary>
        ///   Identifiant de l'agence recherchée
        /// </summary>
        public int? AgenceId { get; set; }

        public AgenceLightEnt Agence { get; set; } // necessaire pour les favoris

        /// <summary>
        ///   Identifiant CI recherché
        /// </summary>
        public int? CiId { get; set; }

        public CILightEnt CI { get; set; } // necessaire pour les favoris

        /// <summary>
        ///     Identifiant de la ressource
        /// </summary>
        public int? RessourceId { get; set; }

        public RessourceLightEnt Ressource { get; set; } // necessaire pour les favoris

        /// <summary>
        ///     Identifiant de la tache
        /// </summary>
        public int? TacheId { get; set; }

        public TacheLightEnt Tache { get; set; } // necessaire pour les favoris

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Date min
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Date max
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Identifiant du création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        public PersonnelLightEnt AuteurCreation { get; set; } // necessaire pour les favoris

        /// <summary>
        ///     Liste des codes type commande
        /// </summary>
        public List<string> TypeCodes { get; set; } = new List<string>();

        /// <summary>
        ///   Obtient ou définit le booléen commande abonnement
        /// </summary>
        public bool IsAbonnement { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande contient un matériel externe à pointer ou non 
        /// </summary>
        public bool IsMaterielAPointer { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est energie
        /// </summary>
        public bool IsEnergie { get; set; }

        /// <summary>
        ///     Obtient ou définit si on filtre par commande abonnement
        /// </summary>
        public bool IsSoldee { get; set; }

        /// <summary>
        ///     Liste des identifiants de CI
        /// </summary>
        public List<int> CiIds { get; set; } = new List<int>();


        /// <summary>
        ///   si on selectionne les commandes avec au moins une ligne vérouillée
        /// </summary>
        public bool OnlyCommandeWithAtLeastOneCommandeLigneLocked { get; set; }

        /// <summary>
        /// Determine si l'utilisateur courrant a la fonctionnalité de
        /// verrouillage/deverrouillage d'une ligne de commande
        /// </summary>
        public bool CurrentUserHasFeatureLockUnLockCommandeLigne { get; set; }

        /// <summary>
        /// Filter
        /// </summary>
        /// <returns>Expression</returns>
        /// <summary>
        ///   Permet de récupérer le prédicat de recherche des commandes.
        /// </summary>
        /// <returns>Retourne la condition de recherche des commandes</returns>
        public Expression<Func<CommandeEnt, bool>> GetCommandeForReceptionPredicateWhere()
        {

            if (TypeCodes.Count == 0)
            {
                //Si aucun type n'est spéciifé il faut charger uniquement les Fournitures, les Locations et les Prestations
                TypeCodes = new List<string>() {
                    CommandeTypeEnt.CommandeTypeF,
                    CommandeTypeEnt.CommandeTypeL,
                    CommandeTypeEnt.CommandeTypeP
                };
            }

            //filtre obligatoires inititalisation
            Expression<Func<CommandeEnt, bool>> outer = c => c.CiId.HasValue && CiIds.Contains(c.CiId.Value) && !c.DateCloture.HasValue
                && !c.DateSuppression.HasValue;

            outer = outer.And(c => statutCodes.Contains(c.StatutCommande.Code));

            if (!string.IsNullOrEmpty(ValueText))
            {
                outer = outer.And(IsInTextExpression());
            }

            AddPredicateFournisseurAndAgence(ref outer);

            if (this.CiId.HasValue)
            {
                outer = outer.And(c => c.CiId == CiId);
            }
            if (RessourceId.HasValue)
            {
                outer = outer.And(c => c.Lignes.Any(x => x.RessourceId == RessourceId.Value));
            }
            if (TacheId.HasValue)
            {
                outer = outer.And(c => c.Lignes.Any(x => x.TacheId == TacheId.Value));
            }
            AddPredicateDate(ref outer);

            if (AuteurCreationId != null)
            {
                outer = outer.And(c => c.AuteurCreationId == AuteurCreationId);
            }
            if (TypeCodes.Count != 0)
            {
                outer = outer.And(c => TypeCodes.Contains(c.Type.Code));
            }
            if (this.IsAbonnement)
            {
                outer = outer.And(c => c.IsAbonnement);
            }
            if (this.IsMaterielAPointer)
            {
                outer = outer.And(c => c.IsMaterielAPointer);
            }
            if (this.IsEnergie)
            {
                outer = outer.And(c => c.IsEnergie);
            }
            if (!IsSoldee)
            {
                var newSearchExpression = new SearchExpression();

                outer = outer.And(newSearchExpression.GetCommandeReceivableExpression());
            }

            if (this.CurrentUserHasFeatureLockUnLockCommandeLigne)
            {
                if (this.OnlyCommandeWithAtLeastOneCommandeLigneLocked)
                {
                    outer = outer.And(c => c.Lignes.Any(cl => cl.IsVerrou));
                }
                else
                {
                    outer = outer.And(c => c.Lignes.Any(cl => !cl.IsVerrou));
                }
            }
            return outer;
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <returns>Expression</returns>
        /// <summary>
        ///   Permet de récupérer le prédicat de recherche des commandes lignes.
        /// </summary>
        /// <returns>Retourne la condition de recherche des commandes lignes</returns>
        public Expression<Func<CommandeLigneEnt, bool>> GetCommandeLigneForReceptionPredicateWhere()
        {
            Expression<Func<CommandeLigneEnt, bool>> outer = c => c.CommandeLigneId > 0;
            if (RessourceId.HasValue)
            {
                outer = outer.And(x => x.RessourceId != RessourceId.Value);
            }
            if (TacheId.HasValue)
            {
                outer = outer.And(x => x.TacheId != TacheId);
            }
            return outer;
        }

        private void AddPredicateFournisseurAndAgence(ref Expression<Func<CommandeEnt, bool>> outer)
        {
            if (FournisseurId.HasValue)
            {
                outer = outer.And(c => c.FournisseurId == FournisseurId.Value);
                if (AgenceId.HasValue)
                {
                    outer = outer.And(c => c.AgenceId == AgenceId.Value);
                }
            }
        }

        private Expression<Func<CommandeEnt, bool>> IsInTextExpression()
        {
            return c => ((c.CI.Code ?? string.Empty) + " - " + (c.CI.Libelle ?? string.Empty)).Contains(ValueText)
                       || ((c.Fournisseur.Code ?? string.Empty) + " - " + (c.Fournisseur.Libelle ?? string.Empty)).Contains(ValueText)
                       || ((c.AuteurCreation.Personnel == null ? (c.AuteurCreation.Personnel.Prenom ?? string.Empty) : string.Empty) + " "
                        + (c.AuteurCreation.Personnel == null ? (c.AuteurCreation.Personnel.Nom ?? string.Empty) : string.Empty)).Contains(ValueText)
                       || c.Numero.Contains(ValueText)
                       || (c.NumeroCommandeExterne != null && c.NumeroCommandeExterne.Contains(ValueText))
                       || c.Libelle.Contains(ValueText)
                       || (c.Lignes.Count > 0 ? c.Lignes.Sum(l => l.Quantite * l.PUHT) : 0).ToString().Contains(ValueText);
        }


        /// <summary>
        /// predicate pour les dates
        /// </summary>
        /// <param name="predicate">un predicate</param>
        private void AddPredicateDate(ref Expression<Func<CommandeEnt, bool>> predicate)
        {
            if (DateFrom.HasValue)
            {
                var date = new DateTime(DateFrom.Value.Year, DateFrom.Value.Month, DateFrom.Value.Day, 0, 0, 0);
                predicate = predicate.And(p => p.DateCreation.Value >= date);
            }

            if (DateTo.HasValue)
            {
                var date = new DateTime(DateTo.Value.Year, DateTo.Value.Month, DateTo.Value.Day, 23, 59, 59);
                predicate = predicate.And(p => p.DateCreation.Value <= date);
            }
        }

    }
}
