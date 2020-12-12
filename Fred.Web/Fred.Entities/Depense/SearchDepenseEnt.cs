using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Organisation;
using Fred.Entities.Search;
using LinqKit;

namespace Fred.Entities.Depense
{
    /// <summary>
    ///   Classe de recherche des dépenses
    /// </summary>
    [Serializable]
    public class SearchDepenseEnt : AbstractSearchEnt<DepenseAchatEnt>
    {
        #region Scope de recherche

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : Code / Libellé du CI de la dépense
        /// </summary>
        public bool CICodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   le Scope : Libellé de la dépense
        /// </summary>
        public bool Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   le Scope : Numéro / Libellé de la commande de la dépense
        /// </summary>
        public bool CommandeNumeroLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : Code / Libellé du fournisseur de la dépense
        /// </summary>
        public bool FournisseurCodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : Numéro du bon de livraison de la dépense
        /// </summary>
        public bool NumeroBL { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   le  Scope : Code / Libellé de la ressource de la dépense
        /// </summary>
        public bool RessourceCodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : Code / Libellé de la tâche de la dépense
        /// </summary>
        public bool TacheCodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : Commentaire de la dépense
        /// </summary>
        public bool Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : Auteur de la saisie de la dépense
        /// </summary>
        public bool AuteurCreationNomPrenom { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : Auteur de la modification de la dépense
        /// </summary>
        public bool AuteurModificationNomPrenom { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : Unité de la dépense
        /// </summary>
        public bool Unite { get; set; }

        /// <summary>
        /// Obtient ou définit l'Organisation
        /// </summary>
        public OrganisationLightEnt Organisation { get; set; }

        #endregion

        #region Critères

        /// <summary>
        ///   Obtient ou définit le Critère de recherche : Date min
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le Critère de recherche : Date max
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        ///   Obtient ou définit le Critère de recherche : Quantité min de la dépense
        /// </summary>
        public decimal? QuantiteFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le Critère de recherche : Quantité max de la dépense
        /// </summary>
        public decimal? QuantiteTo { get; set; }

        /// <summary>
        ///   Obtient ou définit le Critère de recherche : PUHT min de la dépense
        /// </summary>
        public decimal? PUHTFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le Critère de recherche : PUHT max de la dépense
        /// </summary>
        public decimal? PUHTTo { get; set; }

        /// <summary>
        ///   Obtient ou définit le Critère de recherche : Montant min de la dépense
        /// </summary>
        public decimal? MontantHTFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le Critère de recherche : Montant max de la dépense
        /// </summary>
        public decimal? MontantHTTo { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du type de dépense achat (Reception, Facture, FactureEcart)
        /// </summary>
        public int? DepenseTypeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'organisation
        /// </summary>
        public int? OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des CI
        /// </summary>
        public ICollection<int> Cis { get; set; }


        /// <summary>
        /// Inclure les réceptions intérimaires dans le résultat
        /// </summary>
        public bool IncludeReceptionInterimaire { get; set; }

        #endregion

        #region Spécifique aux DepenseEnt de type Réception

        /// <summary>
        ///   Obtient ou définit un booléen déterminant si l'on veut filtrer sur les DepenseEnt à viser
        /// </summary>
        public bool AViser { get; set; }

        /// <summary>
        ///   Obtient ou définit un booléen déterminant si l'on veut filtrer sur les DepenseEnt à viser
        /// </summary>
        public bool Visees { get; set; }

        /// <summary>
        ///   Obtient ou définit un booléen déterminant si l'on veut filtrer sur les DepenseEnt à viser
        /// </summary>
        public bool Far { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est une commande Abonnement ou non
        /// </summary>    
        public bool IsAbonnement { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est une commande avec matériel à pointer ou non
        /// </summary>    
        public bool IsMaterielAPointer { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est énergie
        /// </summary>    
        public bool IsEnergie { get; set; }

        /// <summary>
        /// Seulement les réceptions ayant au moins une pièce jointe attachée
        /// </summary>
        public bool OnlyReceptionWithPiecesJointes { get; set; }

        #endregion

        #region Tris

        /// <summary>
        ///   Obtient ou définit le Tri : Code du CI de la commande
        /// </summary>
        public bool? CICodeAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Date de la commande
        /// </summary>
        public bool? DateAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Libellé de la commande
        /// </summary>
        public bool? LibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Numéro / Libellé de la commande de la devise
        /// </summary>
        public bool? CommandeNumeroLibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Code / Libellé du fournisseur de la commande
        /// </summary>
        public bool? FournisseurCodeLibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Scope : Numéro du bon de livraison de la dépense
        /// </summary>
        public bool? NumeroBLAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Scope : Code / Libellé de la ressource de la dépense
        /// </summary>
        public bool? RessourceCodeLibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Scope : Code / Libellé de la tâche de la dépense
        /// </summary>
        public bool? TacheCodeLibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Commentaire de la dépense
        /// </summary>
        public bool? CommentaireAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Devise de la dépense
        /// </summary>
        public bool? DeviseAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Nom / Prénom du créateur de la commande
        /// </summary>
        public bool? AuteurCreationNomPrenomAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Nom / Prénom du valideur de la commande
        /// </summary>
        public bool? AuteurModificationNomPrenomAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Unité de la dépense
        /// </summary>
        public bool? UniteAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Quantité d'unités de la dépense
        /// </summary>
        public bool? QuantiteAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : PUHT de la dépense
        /// </summary>
        public bool? PUHTAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Montant HT de la commande
        /// </summary>
        public bool? MontantHTAsc { get; set; }

        #endregion

        #region Génération de prédicat de recherche

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche des dépenses.
        /// </summary>
        /// <returns>Retourne la condition de recherche des dépenses</returns>
        public override Expression<Func<DepenseAchatEnt, bool>> GetPredicateWhere()
        {
            return p =>
              (!DateFrom.HasValue || p.Date >= DateFrom.Value)
              && (!DateTo.HasValue || p.Date <= DateTo.Value)
              && (!QuantiteFrom.HasValue || p.Quantite >= QuantiteFrom.Value)
              && (!QuantiteTo.HasValue || p.Quantite <= QuantiteTo.Value)
              && (!PUHTFrom.HasValue || p.PUHT >= PUHTFrom.Value)
              && (!PUHTTo.HasValue || p.PUHT <= PUHTTo.Value)
              && (!MontantHTFrom.HasValue || (p.Quantite * p.PUHT) >= MontantHTFrom.Value)
              && (!MontantHTTo.HasValue || (p.Quantite * p.PUHT) <= MontantHTTo.Value)
              && (!DepenseTypeId.HasValue || p.DepenseTypeId == DepenseTypeId);
        }


        /// <summary>
        ///   Permet de récupérer le prédicat de rechercher des Dépenses Achat de type Réception
        /// </summary>
        /// <returns>Retourne la condition de recherche</returns>
        public Expression<Func<DepenseAchatEnt, bool>> GetReceptionPredicateWhere()
        {

            var outer = PredicateBuilder.New<DepenseAchatEnt>();

            //filtre obligatoires 
            outer = outer.And(c => c.CiId.HasValue); // il faut obligatoirement qu'une reception est un ci
            outer = outer.And(p => Cis.Contains(p.CiId.Value)); // il faut obligatoirement que le ci de la reception appartienne aux cis de l'utilisateur         
            outer = outer.And(c => !c.DateSuppression.HasValue);// il faut obligatoirement que la reception soit non supprimée

            if (!string.IsNullOrEmpty(ValueText))
            {
                outer = outer.And(IsInTextExpression());
            }
            if (DateFrom.HasValue)
            {
                outer = outer.And(p => ((p.DateComptable.Value.Year * 100) + p.DateComptable.Value.Month >= (DateFrom.Value.Year * 100) + DateFrom.Value.Month));
            }
            if (DateTo.HasValue)
            {
                outer = outer.And(p => ((p.DateComptable.Value.Year * 100) + p.DateComptable.Value.Month <= (DateTo.Value.Year * 100) + DateTo.Value.Month));
            }
            if (DepenseTypeId.HasValue)
            {
                outer = outer.And(p => p.DepenseTypeId == DepenseTypeId);
            }
            outer = AddPredicateAViser(outer);
            outer = AddPredicateVisees(outer);
            if (Far)
            {
                outer = outer.And(p => ((p.PUHT * p.Quantite) > 0));
            }
            if (IsAbonnement)
            {
                outer = outer.And(p => p.CommandeLigne.Commande.IsAbonnement);
            }
            if (IsMaterielAPointer)
            {
                outer = outer.And(p => p.CommandeLigne.Commande.IsMaterielAPointer);
            }
            if (IsEnergie)
            {
                outer = outer.And(p => p.CommandeLigne.Commande.IsEnergie);
            }

            outer.And(p => ((IncludeReceptionInterimaire && (p.Tache.Code == Constantes.TacheSysteme.CodeTacheEcartInterim || p.Tache.Code != Constantes.TacheSysteme.CodeTacheEcartInterim))
                             || (!IncludeReceptionInterimaire && p.Tache.Code != Constantes.TacheSysteme.CodeTacheEcartInterim)));

            // Construction des prédicats liés aux pièces jointes
            AddPredicatePcsJointes(ref outer);

            return outer;
        }

        private ExpressionStarter<DepenseAchatEnt> AddPredicateAViser(ExpressionStarter<DepenseAchatEnt> outer)
        {
            if (AViser)
            {
                outer = outer.And(x => x.DateVisaReception == null || x.HangfireJobId == null);
            }
            return outer;
        }

        private ExpressionStarter<DepenseAchatEnt> AddPredicateVisees(ExpressionStarter<DepenseAchatEnt> outer)
        {
            if (Visees)
            {
                outer = outer.And(x => !(x.DateVisaReception == null || x.HangfireJobId == null));
            }
            return outer;
        }


        /// <summary>
        /// Predicate pour les pièces jointes
        /// </summary>
        /// <param name="outer">Predicat à utiliser</param>
        private void AddPredicatePcsJointes(ref ExpressionStarter<DepenseAchatEnt> outer)
        {
            if (OnlyReceptionWithPiecesJointes)
            {
                outer = outer.And(p => p.PiecesJointesReception.Any());
            }
        }

        private Expression<Func<DepenseAchatEnt, bool>> IsInTextExpression()
        {
            var valueTextLower = ValueText?.ToLower();

            return p => (string.IsNullOrEmpty(ValueText)
                         || p.CommandeLigne.Commande.Fournisseur.Code.Contains(valueTextLower)
                         || p.CommandeLigne.Commande.Fournisseur.Libelle.Contains(valueTextLower)
                         || p.CommandeLigne.Commande.CI.Code.Contains(valueTextLower)
                         || p.CommandeLigne.Commande.CI.Libelle.Contains(valueTextLower)
                         || p.CommandeLigne.Commande.Libelle.Contains(valueTextLower)
                         || p.CommandeLigne.Commande.Numero.Contains(valueTextLower)
                         || (p.CommandeLigne.Commande.NumeroCommandeExterne != null && p.CommandeLigne.Commande.NumeroCommandeExterne.Contains(valueTextLower))
                         || p.Libelle.Contains(valueTextLower)
                         || p.Ressource.Code.Contains(valueTextLower)
                         || p.Ressource.Libelle.Contains(valueTextLower)
                         || p.Tache.Code.Contains(valueTextLower)
                         || p.Tache.Libelle.Contains(valueTextLower)
                         || p.NumeroBL.Contains(valueTextLower)
                         || p.Commentaire.Contains(valueTextLower));
        }

        /// <summary>
        ///   Permet de récupérer une ordre de dépense par défaut.
        /// </summary>
        /// <returns>Retourne un odre de dépense par défaut</returns>
        protected override IOrderer<DepenseAchatEnt> GetDefaultOrderBy()
        {
            if (!DateAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, DateTime?>(d => d.Date, false);
            }

            return null;
        }

        /// <summary>
        ///   Permet de récupérer un ordre de dépense par une de ses valeurs.
        /// </summary>
        /// <returns>Retourne un odre de dépense par une de ses valeurs</returns>
        protected override IOrderer<DepenseAchatEnt> GetUserOrderBy()
        {
            if (CICodeAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, string>(d => d.CI.Code, CICodeAsc.Value);
            }
            if (DateAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, DateTime?>(d => d.Date, DateAsc.Value);
            }
            if (LibelleAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, string>(d => d.Libelle, LibelleAsc.Value);
            }
            if (CommandeNumeroLibelleAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, string>(d => d.CommandeLigne.NumeroLibelle, CommandeNumeroLibelleAsc.Value);
            }
            if (FournisseurCodeLibelleAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, object>(new List<Expression<Func<DepenseAchatEnt, object>>> { d => d.Fournisseur.Code, d => d.Fournisseur.Libelle }, FournisseurCodeLibelleAsc.Value);
            }
            if (NumeroBLAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, string>(d => d.NumeroBL, NumeroBLAsc.Value);
            }
            if (RessourceCodeLibelleAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, object>(new List<Expression<Func<DepenseAchatEnt, object>>> { d => d.Ressource.Code, d => d.Ressource.Libelle }, RessourceCodeLibelleAsc.Value);
            }
            if (TacheCodeLibelleAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, object>(new List<Expression<Func<DepenseAchatEnt, object>>> { d => d.Tache.Code, d => d.Tache.Libelle }, TacheCodeLibelleAsc.Value);
            }
            if (CommentaireAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, string>(d => d.Commentaire, CommentaireAsc.Value);
            }
            if (AuteurCreationNomPrenomAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, object>(new List<Expression<Func<DepenseAchatEnt, object>>> { d => d.AuteurCreation.Personnel.Prenom, d => d.AuteurCreation.Personnel.Nom }, AuteurCreationNomPrenomAsc.Value);
            }
            if (AuteurModificationNomPrenomAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, object>(new List<Expression<Func<DepenseAchatEnt, object>>> { d => d.AuteurModification.Personnel.Prenom, d => d.AuteurModification.Personnel.Nom }, AuteurModificationNomPrenomAsc.Value);
            }
            if (UniteAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, string>(d => d.Unite.Code, UniteAsc.Value);
            }
            if (QuantiteAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, decimal>(d => d.Quantite, QuantiteAsc.Value);
            }
            if (PUHTAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, decimal>(d => d.PUHT, PUHTAsc.Value);
            }
            if (MontantHTAsc.HasValue)
            {
                return new Orderer<DepenseAchatEnt, decimal>(d => d.MontantHT, MontantHTAsc.Value);
            }

            return GetDefaultOrderBy();
        }

        #endregion
    }
}
