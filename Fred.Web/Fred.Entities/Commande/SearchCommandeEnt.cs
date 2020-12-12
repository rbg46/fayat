using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.CI;
using Fred.Entities.Import;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Search;
using LinqKit;

namespace Fred.Entities.Commande
{
    /// <summary>
    ///   Classe de recherche des commandes
    /// </summary>
    [Serializable]
    public class SearchCommandeEnt : AbstractSearchEnt<CommandeEnt>
    {
        #region Scope de recherche

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Numéro de la commande
        /// </summary>
        public bool Numero { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Numéro externe de la commande
        /// </summary>
        public bool NumeroCommandeExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Libellé de la commande
        /// </summary>
        public bool Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Code / Libellé du CI de la commande
        /// </summary>
        public bool CICodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si  le scope : Code / Libellé du fournisseur de la commande
        /// </summary>
        public bool FournisseurCodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Nom / Prénom du créateur de la commande
        /// </summary>
        public bool AuteurCreationNomPrenom { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Nom / Prénom du valideur de la commande
        /// </summary>
        public bool AuteurValidationNomPrenom { get; set; }

        #endregion

        #region Critères
        /// <summary>
        ///   Obtient ou définit le personnel auteur de la commande
        /// </summary>
        public PersonnelLightForPickListEnt Author { get; set; }

        /// <summary>
        ///   Obtient ou définit Type auteur de la commande
        /// </summary>
        public string AuthorType { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Date min
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Date max
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Montant min de la commande
        /// </summary>
        public decimal? MontantHTFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Montant max de la commande
        /// </summary>
        public decimal? MontantHTTo { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Type de la commande
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le model du type à préselectionner
        /// </summary>
        public string TypeModel { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de la commande recherchée
        /// </summary>
        public CommandeTypeEnt Type { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Identifiant du création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des types de commande disponibles pour le filtrage
        /// </summary>
        public IEnumerable<CommandeTypeEnt> Types { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Statut de la commande
        /// </summary>
        public int? StatutId { get; set; }

        /// <summary>
        /// Obtient ou définit le le libellé du statut
        /// </summary>
        public string StatutLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit le statut de la commande recherchée
        /// </summary>
        public StatutCommandeEnt Statut { get; set; }

        /// <summary>
        ///   Obtient ou définit le model du statut à préselectionner
        /// </summary>
        public string StatutModel { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des statuts de commande disponibles pour le filtrage
        /// </summary>
        public IEnumerable<StatutCommandeEnt> Statuts { get; set; }

        /// <summary>
        ///   Identifiant Fournisseur recherché
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        ///   Identifiant de l'agence recherchée
        /// </summary>
        public int? AgenceId { get; set; }

        /// <summary>
        ///   Fournisseur Recherché
        /// </summary>
        public FournisseurLightEnt Fournisseur { get; set; }

        /// <summary>
        ///   Agence Recherchée
        /// </summary>
        public AgenceLightEnt Agence { get; set; }

        /// <summary>
        ///   Identifiant CI recherché
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        ///   CI Recherché
        /// </summary>
        public CILightEnt CI { get; set; }

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
        /// Seulement les commandes ayant au moins une pièce jointe attachée
        /// </summary>
        public bool OnlyCommandeWithPiecesJointes { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Système externe
        /// </summary>
        public int? SystemeExterneId { get; set; }

        /// <summary>
        /// Obtient ou définit le model du système externe à préselectionner
        /// </summary>
        public string SystemeExterneModel { get; set; }

        /// <summary>
        ///   Obtient ou définit le système externe de la commande recherchée
        /// </summary>
        public SystemeExterneEnt SystemeExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des systèmes externes disponibles pour le filtrage
        /// </summary>
        public IEnumerable<SystemeExterneEnt> SystemeExternes { get; set; }

        /// <summary>
        ///     Obtient ou définit si on filtre par commande abonnement
        /// </summary>
        public bool IsSoldee { get; set; }

        /// <summary>
        ///     Liste des identifiants de CI
        /// </summary>
        public List<int> CiIds { get; set; } = new List<int>();

        /// <summary>
        ///     Liste des codes statut commande
        /// </summary>
        public List<string> StatutCodes { get; set; } = new List<string>();

        /// <summary>
        ///     Liste des codes type commande
        /// </summary>
        public List<string> TypeCodes { get; set; } = new List<string>();

        /// <summary>
        ///     Identifiant de la ressource
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        ///     Identifiant de la tache
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        ///     Identifiant du type dépense réception code
        /// </summary>
        public int? DepenseTypeReceptionCode { get; set; }

        #endregion

        #region Tris

        /// <summary>
        ///   Obtient ou définit le tri : Numéro de la commande
        /// </summary>
        public bool? NumeroAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Date de la commande
        /// </summary>
        public bool? DateAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Libellé de la commande
        /// </summary>
        public bool? LibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Code / Libellé du CI de la commande
        /// </summary>
        public bool? CICodeLibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Code / Libellé du fournisseur de la commande
        /// </summary>
        public bool? FournisseurCodeLibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Nom / Prénom du créateur de la commande
        /// </summary>
        public bool? AuteurCreationNomPrenomAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Nom / Prénom du valideur de la commande
        /// </summary>
        public bool? AuteurValidationNomPrenomAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Montant HT de la commande
        /// </summary>
        public bool? MontantHTAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur Mes commandes
        /// </summary>
        public bool MesCommandes { get; set; }

        /// <summary>
        /// Determine si l'utilisateur courrant a la fonctionnalité de
        /// verrouillage/deverrouillage d'une ligne de commande
        /// </summary>
        public bool CurrentUserHasFeatureLockUnLockCommandeLigne { get; set; }

        #endregion

        #region Génération de prédicat de recherche
        /// <summary>
        ///   Permet de récupérer le prédicat de recherche des commandes.
        /// </summary>
        /// <returns>Retourne la condition de recherche des commandes</returns>
        public override Expression<Func<CommandeEnt, bool>> GetPredicateWhere()
        {
            Expression<Func<CommandeEnt, bool>> predicate = (p => p.CiId.HasValue
                         && CiIds.Contains(p.CiId.Value)
                         && !p.DateSuppression.HasValue
                         && TypeCodes.Contains(p.Type.Code)
                         && p.StatutCommande.Code != StatutCommandeEnt.CommandeStatutPREP);
            //découper en plusieurs morceaux (problème du niveau de complexité)
            if (!string.IsNullOrEmpty(ValueText))
            {
                predicate = predicate.And(SearchTextPredicate());
            }
            if (AuthorType != null)
            {
                predicate = predicate.And(GetFilterByAuthorType());
            }
            AddPredicatePcsJointes(ref predicate);
            AddPredicateIdParams(ref predicate);
            AddPredicateDate(ref predicate);
            AddPredicatCritere(ref predicate);

            return predicate;
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




        /// <summary>
        ///   Permet de récupérer un ordre commandes par défaut.
        /// </summary>
        /// <returns>Retourne un odre de commande par défaut</returns>
        protected override IOrderer<CommandeEnt> GetDefaultOrderBy()
        {
            if (!NumeroAsc.HasValue)
            {
                return new Orderer<CommandeEnt, string>(c => c.Numero, true);
            }

            return null;
        }

        /// <summary>
        ///   Permet de récupérer un ordre commandes par une de ses valeurs.
        /// </summary>
        /// <returns>Retourne un odre de commande par une de ses valeurs</returns>
        protected override IOrderer<CommandeEnt> GetUserOrderBy()
        {
            if (NumeroAsc.HasValue)
            {
                return new Orderer<CommandeEnt, string>(c => c.Numero, NumeroAsc.Value);
            }
            if (DateAsc.HasValue)
            {
                return new Orderer<CommandeEnt, DateTime>(c => c.Date, DateAsc.Value);
            }
            if (LibelleAsc.HasValue)
            {
                return new Orderer<CommandeEnt, string>(c => c.Libelle, LibelleAsc.Value);
            }
            if (CICodeLibelleAsc.HasValue)
            {
                return new Orderer<CommandeEnt, object>(new List<Expression<Func<CommandeEnt, object>>> { c => c.CI.Code, c => c.CI.Libelle }, CICodeLibelleAsc.Value);
            }
            if (FournisseurCodeLibelleAsc.HasValue)
            {
                return new Orderer<CommandeEnt, object>(new List<Expression<Func<CommandeEnt, object>>> { c => c.Fournisseur.Code, c => c.Fournisseur.Libelle }, FournisseurCodeLibelleAsc.Value);
            }
            if (AuteurCreationNomPrenomAsc.HasValue)
            {
                return new Orderer<CommandeEnt, object>(new List<Expression<Func<CommandeEnt, object>>> { c => c.AuteurCreation.Personnel.Prenom, c => c.AuteurCreation.Personnel.Nom }, AuteurCreationNomPrenomAsc.Value);
            }
            if (AuteurValidationNomPrenomAsc.HasValue)
            {
                return new Orderer<CommandeEnt, object>(new List<Expression<Func<CommandeEnt, object>>> { c => c.Valideur.Personnel.Prenom, c => c.Valideur.Personnel.Nom }, AuteurValidationNomPrenomAsc.Value);
            }

            return GetDefaultOrderBy();
        }

        /// <summary>
        /// predicate pour texte  de recherche
        /// </summary>
        /// <returns>un predicate</returns>
        private Expression<Func<CommandeEnt, bool>> SearchTextPredicate()
        {
            Expression<Func<CommandeEnt, bool>> predicate = (p => Numero && p.Numero.Contains(ValueText));
            AddPredicateSearchText(ref predicate);
            if (AuteurCreationNomPrenom)
            {
                predicate = predicate.Or(AddPredicateAuthorCreator());
            }
            if (AuteurValidationNomPrenom)
            {
                predicate = predicate.Or(AddPredicateAuthorValidor());
            }
            return predicate;
        }

        /// <summary>
        /// ajouter des predicates pour texte de recherche
        /// </summary>
        /// <param name="predicate">un predicate</param>
        private void AddPredicateSearchText(ref Expression<Func<CommandeEnt, bool>> predicate)
        {
            if (NumeroCommandeExterne)
            {
                predicate = predicate.Or(p => p.NumeroCommandeExterne.Contains(ValueText));
            }

            if (Libelle)
            {
                predicate = predicate.Or(p => p.Libelle.Contains(ValueText));
            }

            if (CICodeLibelle)
            {
                predicate = predicate.Or(p => (p.CI.Code.Contains(ValueText) || p.CI.Libelle.Contains(ValueText)));
            }

            if (FournisseurCodeLibelle)
            {
                predicate = predicate.Or(p => (p.Fournisseur.Code.Contains(ValueText) || p.Fournisseur.Libelle.Contains(ValueText)));
            }
        }

        /// <summary>
        /// predicate pour les dates
        /// </summary>
        /// <param name="predicate">un predicate</param>
        private void AddPredicateDate(ref Expression<Func<CommandeEnt, bool>> predicate)
        {
            if (DateFrom.HasValue)
            {
                predicate = predicate.And(p => p.DateCreation.Value.Date >= DateFrom.Value.Date);
            }

            if (DateTo.HasValue)
            {
                predicate = predicate.And(p => p.DateCreation.Value.Date <= DateTo.Value.Date);
            }
        }

        /// <summary>
        /// predicate pour auteur de creation
        /// </summary>
        /// <returns>re tourne une predicate</returns>
        private Expression<Func<CommandeEnt, bool>> AddPredicateAuthorValidor()
        {
            return p => (p.Valideur.Personnel == null ? (p.Valideur.Personnel.Prenom ?? string.Empty) : string.Empty).Contains(ValueText)
                                                    || (p.Valideur.Personnel == null ? (p.Valideur.Personnel.Nom ?? string.Empty) : string.Empty).Contains(ValueText);
        }

        /// <summary>
        /// predicate pour auteur de creation
        /// </summary>
        /// <returns>re tourne une predicate</returns>
        private Expression<Func<CommandeEnt, bool>> AddPredicateAuthorCreator()
        {
            return p => (p.AuteurCreation.Personnel == null ? (p.AuteurCreation.Personnel.Prenom ?? string.Empty) : string.Empty).Contains(ValueText)
                                                 || (p.AuteurCreation.Personnel == null ? (p.AuteurCreation.Personnel.Nom ?? string.Empty) : string.Empty).Contains(ValueText);
        }

        /// <summary>
        /// predicate pour Critères de rechers
        /// </summary>
        /// <param name="predicate"> retourne un predicate</param>
        private void AddPredicatCritere(ref Expression<Func<CommandeEnt, bool>> predicate)
        {
            if (MesCommandes)
            {
                predicate = predicate.And(p => (MesCommandes && p.AuteurCreationId == AuteurCreationId));
            }
            if (IsAbonnement)
            {
                predicate = predicate.And(p => p.IsAbonnement);
            }
            if (IsMaterielAPointer)
            {
                predicate = predicate.And(p => p.IsMaterielAPointer);
            }
            if (IsEnergie)
            {
                predicate = predicate.And(p => p.IsEnergie);
            }
            if (MontantHTFrom.HasValue)
            {
                predicate = CheckMontantHTFrom(predicate);
            }
            if (MontantHTTo.HasValue)
            {
                predicate = CheckMontantHTo(predicate);
            }
        }

        private Expression<Func<CommandeEnt, bool>> CheckMontantHTFrom(Expression<Func<CommandeEnt, bool>> predicate)
        {
            return predicate.And(p => (p.Lignes.Count > 0 ? p.Lignes.Sum(l => (!(l.AvenantLigne != null && l.AvenantLigne.IsDiminution) ? l.Quantite : (-l.Quantite)) * l.PUHT) : 0) >= MontantHTFrom.Value);
        }

        private Expression<Func<CommandeEnt, bool>> CheckMontantHTo(Expression<Func<CommandeEnt, bool>> predicate)
        {
            return predicate.And(p => (p.Lignes.Count > 0 ? p.Lignes.Sum(l => (!(l.AvenantLigne != null && l.AvenantLigne.IsDiminution) ? l.Quantite : (-l.Quantite)) * l.PUHT) : 0) <= MontantHTTo.Value);
        }

        /// <summary>
        /// predicate pour les pièces jointes
        /// </summary>
        /// <param name="predicate"> retourne un predicate</param>
        private void AddPredicatePcsJointes(ref Expression<Func<CommandeEnt, bool>> predicate)
        {
            if (OnlyCommandeWithPiecesJointes)
            {
                predicate = predicate.And(p => (p.PiecesJointesCommande.Count > 0));
            }
        }

        /// <summary>
        /// predicate des filtres params
        /// </summary>
        /// <param name="predicate"> retourne un predicate</param>
        private void AddPredicateIdParams(ref Expression<Func<CommandeEnt, bool>> predicate)
        {
            AddPredicateFournisseurAndAgence(ref predicate);
            if (CiId.HasValue)
            {
                predicate = predicate.And(p => p.CiId == CiId.Value);
            }
            if (SystemeExterneId.HasValue)
            {
                predicate = predicate.And(p => p.SystemeExterneId == SystemeExterneId);
            }
            if (StatutId.HasValue)
            {
                predicate = predicate.And(p => p.StatutCommandeId == StatutId);
            }
            if (TypeId.HasValue)
            {
                predicate = predicate.And(p => p.TypeId == TypeId);
            }
        }

        /// <summary>
        /// Renvoi une expression de recherche lamda
        /// </summary>
        /// <returns>Filtre les commande suivants le type auteur</returns>
        private Expression<Func<CommandeEnt, bool>> GetFilterByAuthorType()
        {
            switch (AuthorType)
            {
                case "AuteurCreation":
                    return p => (p.AuteurCreation != null && p.AuteurCreationId == Author.PersonnelId);
                case "Valideur":
                    return p => (p.Valideur != null && p.Valideur.Personnel.PersonnelId == Author.PersonnelId);
                default:
                    return p => true;
            }
        }
        #endregion
    }
}
