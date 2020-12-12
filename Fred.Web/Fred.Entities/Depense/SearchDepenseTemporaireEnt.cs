using Fred.Entities.Search;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fred.Entities.Depense
{
    /// <summary>
    ///   Classe de recherche des dépenses temporaires
    /// </summary>
    [Serializable]
    public class SearchDepenseTemporaireEnt : AbstractSearchEnt<DepenseTemporaireEnt>
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
        ///   Scope : Unité de la dépense
        /// </summary>
        public bool Unite { get; set; }

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
#pragma warning disable S3776
        /// <summary>
        ///   Permet de récupérer le prédicat de recherche des dépenses.
        /// </summary>
        /// <returns>Retourne la condition de recherche des dépenses</returns>
        public override Expression<Func<DepenseTemporaireEnt, bool>> GetPredicateWhere()
        {
            var valueTextLower = ValueText.ToLower();
            return p => (string.IsNullOrEmpty(ValueText)
                         || CICodeLibelle && (p.CI.Code.ToLower().Contains(valueTextLower) || p.CI.Libelle.ToLower().Contains(valueTextLower))
                         || Libelle && p.Libelle.ToLower().Contains(valueTextLower)
                         || CommandeNumeroLibelle && (p.CommandeLigne.Commande.Numero.ToLower().Contains(valueTextLower) || (p.CommandeLigne.Commande.NumeroCommandeExterne != null && p.CommandeLigne.Commande.NumeroCommandeExterne.ToLower().Contains(valueTextLower)) || p.CommandeLigne.Libelle.ToLower().Contains(valueTextLower))
                         || FournisseurCodeLibelle && (p.Fournisseur.Code.ToLower().Contains(valueTextLower) || p.Fournisseur.Libelle.ToLower().Contains(valueTextLower))
                         || NumeroBL && p.NumeroBL.ToLower().Contains(valueTextLower)
                         || RessourceCodeLibelle && (p.Ressource.Code.ToLower().Contains(valueTextLower) || p.Ressource.Libelle.ToLower().Contains(valueTextLower))
                         || TacheCodeLibelle && (p.Tache.Code.ToLower().Contains(valueTextLower) || p.Tache.Libelle.ToLower().Contains(valueTextLower))
                         || Commentaire && p.Commentaire.ToLower().Contains(valueTextLower)
                         || AuteurCreationNomPrenom && (p.AuteurCreation.Personnel.Prenom.ToLower().Contains(valueTextLower) || p.AuteurCreation.Personnel.Nom.ToLower().Contains(valueTextLower))
                         || Unite && p.UniteId.HasValue && p.Unite.Code.ToLower().Contains(valueTextLower))
                        && (!DateFrom.HasValue || p.Date >= DateFrom.Value)
                        && (!DateTo.HasValue || p.Date <= DateTo.Value)
                        && (!QuantiteFrom.HasValue || p.Quantite >= QuantiteFrom.Value)
                        && (!QuantiteTo.HasValue || p.Quantite <= QuantiteTo.Value)
                        && (!PUHTFrom.HasValue || p.PUHT >= PUHTFrom.Value)
                        && (!PUHTTo.HasValue || p.PUHT <= PUHTTo.Value)
                        && (!MontantHTFrom.HasValue || p.MontantHT >= MontantHTFrom.Value)
                        && (!MontantHTTo.HasValue || p.MontantHT <= MontantHTTo.Value);
        }
#pragma warning restore S3776
        /// <summary>
        ///   Permet de récupérer une ordre de dépense temporaire par défaut.
        /// </summary>
        /// <returns>Retourne un odre de dépense temporaire par défaut</returns>
        protected override IOrderer<DepenseTemporaireEnt> GetDefaultOrderBy()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Permet de récupérer un ordre de dépense temporaire par une de ses valeurs.
        /// </summary>
        /// <returns>Retourne un odre de dépense temporaire par une de ses valeurs</returns>
        protected override IOrderer<DepenseTemporaireEnt> GetUserOrderBy()
        {
            if (CICodeAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, string>(d => d.CI.Code, CICodeAsc.Value);
            }
            if (DateAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, DateTime?>(d => d.Date, DateAsc.Value);
            }
            if (LibelleAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, string>(d => d.Libelle, LibelleAsc.Value);
            }
            if (CommandeNumeroLibelleAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, object>(new List<Expression<Func<DepenseTemporaireEnt, object>>> { d => d.CommandeLigne.Commande.Numero, d => d.CommandeLigne.Libelle }, CommandeNumeroLibelleAsc.Value);
            }
            if (FournisseurCodeLibelleAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, object>(new List<Expression<Func<DepenseTemporaireEnt, object>>> { d => d.Fournisseur.Code, d => d.Fournisseur.Libelle }, FournisseurCodeLibelleAsc.Value);
            }
            if (NumeroBLAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, string>(d => d.NumeroBL, NumeroBLAsc.Value);
            }
            if (RessourceCodeLibelleAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, object>(new List<Expression<Func<DepenseTemporaireEnt, object>>> { d => d.Ressource.Code, d => d.Ressource.Libelle }, RessourceCodeLibelleAsc.Value);
            }
            if (TacheCodeLibelleAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, object>(new List<Expression<Func<DepenseTemporaireEnt, object>>> { d => d.Tache.Code, d => d.Tache.Libelle }, TacheCodeLibelleAsc.Value);
            }
            if (CommentaireAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, string>(d => d.Commentaire, CommentaireAsc.Value);
            }
            if (DeviseAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, string>(d => d.Devise.IsoCode, DeviseAsc.Value);
            }
            if (AuteurCreationNomPrenomAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, object>(new List<Expression<Func<DepenseTemporaireEnt, object>>> { d => d.AuteurCreation.Personnel.Prenom, d => d.AuteurCreation.Personnel.Nom }, AuteurCreationNomPrenomAsc.Value);
            }
            if (UniteAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, string>(d => d.Unite.Code, UniteAsc.Value);
            }
            if (QuantiteAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, decimal>(d => d.Quantite, QuantiteAsc.Value);
            }
            if (PUHTAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, decimal>(d => d.PUHT, PUHTAsc.Value);
            }
            if (MontantHTAsc.HasValue)
            {
                return new Orderer<DepenseTemporaireEnt, decimal>(d => d.MontantHT, MontantHTAsc.Value);
            }

            return GetDefaultOrderBy();
        }

        #endregion
    }
}
