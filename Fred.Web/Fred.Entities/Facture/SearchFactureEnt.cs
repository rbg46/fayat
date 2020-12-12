using Fred.Entities.Search;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Entities.Facture
{
    /// <summary>
    ///   Classe de recherche des factures
    /// </summary>
    [Serializable]
    public class SearchFactureEnt : AbstractSearch
    {
        /// <summary>
        ///   Delegue pour le choix de l'ordre asc ou dsc
        /// </summary>
        /// <param name="oAsc">si tri ascendant true sinon false</param>
        /// <param name="s">chaine pour le tri</param>
        /// <returns>retourne la chaine valide pour le tri</returns>
        private delegate string DelegueSort(bool? oAsc, string s);

        #region Autre

        /// <summary>
        ///   Obtient ou définit le folio de l'utilisateur courant
        /// </summary>
        public string FolioUtilisateurCourant { get; set; }

        #endregion

        #region Scope de recherche

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : NoFMFI de la facture
        /// </summary>
        public bool NoFMFI { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : NoFacture de la facture
        /// </summary>
        public bool NoFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : JournalCode de la facture
        /// </summary>
        public bool JournalCode { get; set; }


        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : CompteGeneral de la facture
        /// </summary>
        public bool CompteGeneral { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : FournisseurCode de la facture
        /// </summary>
        public bool FournisseurCode { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : FournisseurLibelle de la facture
        /// </summary>
        public bool FournisseurLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : CI de la facture
        /// </summary>
        public bool CI { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : NoFactureFournisseur de la facture
        /// </summary>
        public bool NoFactureFournisseur { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : Devise (code iso pays) de la facture
        /// </summary>
        public bool DeviseCode { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : Folio de la facture
        /// </summary>
        public bool Folio { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : SocieteCode de la facture
        /// </summary>
        public bool SocieteCode { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si
        ///   Scope : EtablissementCode de la facture
        /// </summary>
        public bool EtablissementCode { get; set; }

        #endregion

        #region Critères

        /// <summary>
        ///   Obtient ou définit les factures si on recherche que celles de l'utilisateur
        ///   (affectation par CI)
        /// </summary>
        public bool SeulementMesFactures { get; set; }

        /// <summary>
        ///   Obtient ou définit les factures si on recherche celles rapprochées ou non
        /// </summary>
        public bool AfficherFacturesRapprochees { get; set; }

        /// <summary>
        ///   Obtient ou définit les factures si on recherche celles cachées ou non
        /// </summary>
        public bool AfficherFacturesCachees { get; set; }

        /// <summary>
        ///   Obtient ou définit l'orga pour laquelle on recherche des factures
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit la date comptable
        /// </summary>
        public DateTime? DateComptableFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit la date comptable
        /// </summary>
        public DateTime? DateComptableTo { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de gestion
        /// </summary>
        public DateTime? DateGestionFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de gestion
        /// </summary>
        public DateTime? DateGestionTo { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de facturation
        /// </summary>
        public DateTime? DateFactureFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de facturation
        /// </summary>
        public DateTime? DateFactureTo { get; set; }

        /// <summary>
        ///   Obtient ou définit la date d'échéance
        /// </summary>
        public DateTime? DateEcheanceFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit la date d'échéance
        /// </summary>
        public DateTime? DateEcheanceTo { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant HT
        /// </summary>
        public decimal? MontantHTFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant HT
        /// </summary>
        public decimal? MontantHTTo { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant de TVA
        /// </summary>
        public decimal? MontantTVAFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant de TVA
        /// </summary>
        public decimal? MontantTVATo { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant TTC
        /// </summary>
        public decimal? MontantTTCFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant TTC
        /// </summary>
        public decimal? MontantTTCTo { get; set; }

        #endregion

        #region Tris

        /// <summary>
        ///   Obtient ou définit le Tri : Numéro FMFI de la facture
        /// </summary>
        public bool? NoFMFIAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Numéro de la facture
        /// </summary>
        public bool? NoFactureAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Code du journal de la facture
        /// </summary>
        public bool? JournalCodeAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Login de l'utilisateur qui a créé la facture
        /// </summary>
        public bool? UtilisateurAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Compte général de la facture
        /// </summary>
        public bool? CompteGeneralAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Code du fournisseur de la facture
        /// </summary>
        public bool? FournisseurCodeAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Libellé du fournisseur de la facture
        /// </summary>
        public bool? FournisseurLibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Numéro de facture du fournisseur
        /// </summary>
        public bool? NoFactureFournisseurAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Montant HT de la facture
        /// </summary>
        public bool? MontantHTAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Montant avec TVA de la facture
        /// </summary>
        public bool? MontantTVAAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Montant TTC de la facture
        /// </summary>
        public bool? MontantTTCAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Code pays ISO de la devise de la facture
        /// </summary>
        public bool? DeviseCodeAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Code de l'établissement de la facture
        /// </summary>
        public bool? EtablissementCodeAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Code de la société de la facture
        /// </summary>
        public bool? SocieteCodeAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Date comptable de la facture
        /// </summary>
        public bool? DateComptableAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le Tri : Date de la facture
        /// </summary>
        public bool? DateFactureAsc { get; set; }

        #endregion

        #region Génération de prédicat de recherche

#pragma warning disable RCS1155 // Use StringComparison when comparing strings.

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche des dépenses.
        /// </summary>
        /// <returns>Retourne la condition de recherche des dépenses</returns>
        public Expression<Func<FactureEnt, bool>> GetPredicateWhere()
        {
            if (!string.IsNullOrEmpty(this.ValueText))
            {
                return p => (this.NoFMFI && p.NoFMFI != null && p.NoFMFI.ToLower().Contains(this.ValueText.ToLower())
                             || this.NoFactureFournisseur && p.NoFactureFournisseur != null && p.NoFactureFournisseur.ToLower().Contains(this.ValueText.ToLower())
                             || this.JournalCode && p.Journal != null && p.Journal.Code.ToLower().Contains(this.ValueText.ToLower())
                             || this.CompteGeneral && p.CompteGeneral != null && p.CompteGeneral.ToLower().Contains(this.ValueText.ToLower())
                             || this.FournisseurCode && p.Fournisseur != null && p.Fournisseur.Code.ToLower().Contains(this.ValueText.ToLower())
                             || this.FournisseurLibelle && p.Fournisseur != null && p.Fournisseur.Libelle.ToLower().Contains(this.ValueText.ToLower())
                             || this.Folio && p.Folio != null && p.Folio.ToLower().Contains(this.ValueText.ToLower())
                             || this.SocieteCode && p.Societe != null && p.Societe.Code.ToLower().Contains(this.ValueText.ToLower())
                             || this.EtablissementCode && p.Etablissement != null && p.Etablissement.Code.ToLower().Contains(this.ValueText.ToLower())
                             || this.CI && p.ListLigneFacture.Any(l => l.CI.Code.ToLower().Contains(ValueText.ToLower()))) // Filtre sur le code CI des lignes facture
                             && CheckBillingsCriteria(p)
                             && CheckDatesCriteria(p)
                             && CheckAmountsCriteria(p);
            }

            return p => CheckBillingsCriteria(p)
                            && CheckDatesCriteria(p)
                            && CheckAmountsCriteria(p);
        }

        private bool CheckBillingsCriteria(FactureEnt p)
        {
            return (!this.SeulementMesFactures || p.Folio == this.FolioUtilisateurCourant)
                && (!this.AfficherFacturesRapprochees || p.DateRapprochement == null)
                && (!this.AfficherFacturesCachees || p.Cachee == false);
        }

        private bool CheckDatesCriteria(FactureEnt p)
        {
            return (!DateComptableFrom.HasValue || p.DateComptable.HasValue && p.DateComptable.Value.Year == DateComptableFrom.Value.Year && p.DateComptable.Value.Month == DateComptableFrom.Value.Month)
                && (DateEcheanceFrom.HasValue || p.DateEcheance.Value.Date >= DateEcheanceFrom.Value.Date)
                && (DateEcheanceTo.HasValue || p.DateEcheance.Value.Date <= DateEcheanceTo.Value.Date)
                && (DateGestionFrom.HasValue || p.DateGestion.Value.Date >= DateGestionFrom.Value.Date)
                && (DateGestionTo.HasValue || p.DateGestion.Value.Date <= DateGestionTo.Value.Date)
                && (DateFactureFrom.HasValue || p.DateFacture.Value.Date >= DateFactureFrom.Value.Date)
                && (DateFactureTo.HasValue || p.DateFacture.Value.Date <= DateFactureTo.Value.Date);
        }

        private bool CheckAmountsCriteria(FactureEnt p)
        {
            return (!this.MontantHTFrom.HasValue || p.MontantHT >= this.MontantHTFrom.Value)
                && (!this.MontantHTTo.HasValue || p.MontantHT <= this.MontantHTTo.Value)
                && (!this.MontantTVAFrom.HasValue || p.MontantTVA >= this.MontantTVAFrom.Value)
                && (!this.MontantTVATo.HasValue || p.MontantTVA <= this.MontantTVATo.Value)
                && (!this.MontantTTCFrom.HasValue || p.MontantTTC >= this.MontantTTCFrom.Value)
                && (!this.MontantTTCTo.HasValue || p.MontantTTC <= this.MontantTTCTo.Value);
        }

#pragma warning restore RCS1155 // Use StringComparison when comparing strings.

        /// <summary>
        ///   Permet de récupérer le prédicat de tri Ascendant des depenses.
        /// </summary>
        /// <returns>Retourne la condition de tri Ascendant des depenses</returns>
        public Func<IQueryable<FactureEnt>, IOrderedQueryable<FactureEnt>> GetPredicateOrderByAsc()
        {
            return x => x.OrderBy(GetPredicateOrder(AscSort));
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de tri Descendant des depenses.
        /// </summary>
        /// <returns>Retourne la condition de tri Descendant des depenses</returns>
        public Func<IQueryable<FactureEnt>, IOrderedQueryable<FactureEnt>> GetPredicateOrderByDesc()
        {
            return x => x.OrderBy(GetPredicateOrder(DscSort));
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de tri des depenses.
        /// </summary>
        /// <param name="stringSort">AscSort ou DscSort</param>
        /// <returns>Retourne la condition de tri des depenses</returns>
#pragma warning disable S3776
        private Expression<Func<FactureEnt, string>> GetPredicateOrder(DelegueSort stringSort)
        {

            Expression<Func<FactureEnt, string>> sort = null;
            sort = d =>
              stringSort(NoFMFIAsc, d.NoFMFI ?? string.Empty) +
              stringSort(NoFactureAsc, d.NoFacture ?? string.Empty) +
              stringSort(JournalCodeAsc, d.Journal != null ? (d.Journal.Code ?? string.Empty) : string.Empty) +
              stringSort(UtilisateurAsc, d.UtilisateurCreation != null ? (d.UtilisateurCreation.Login ?? string.Empty) : string.Empty) +
              stringSort(CompteGeneralAsc, d.CompteGeneral ?? string.Empty) +
              stringSort(FournisseurCodeAsc, d.Fournisseur != null ? (d.Fournisseur.Code ?? string.Empty) : string.Empty) +
              stringSort(FournisseurLibelleAsc, d.Fournisseur != null ? (d.Fournisseur.Libelle ?? string.Empty) : string.Empty) +
              stringSort(NoFactureFournisseurAsc, d.NoFactureFournisseur ?? string.Empty) +
              stringSort(MontantHTAsc, d.MontantHT != null ? ((decimal)d.MontantHT).ToString("000000000.000000000") : string.Empty) +
              stringSort(MontantTVAAsc, d.MontantTVA != null ? ((decimal)d.MontantTVA).ToString("000000000.000000000") : string.Empty) +
              stringSort(MontantTTCAsc, d.MontantTTC.ToString("000000000.000000000")) +
              stringSort(DeviseCodeAsc, d.Devise != null ? (d.Devise.CodePaysIso ?? string.Empty) : string.Empty) +
              stringSort(EtablissementCodeAsc, d.Etablissement != null ? (d.Etablissement.Code ?? string.Empty) : string.Empty) +
              stringSort(SocieteCodeAsc, d.Societe != null ? (d.Societe.Code ?? string.Empty) : string.Empty) +
              stringSort(DateComptableAsc, d.DateComptable.HasValue ? ((DateTime)d.DateComptable).ToString("yyyyMMdd") : string.Empty) +
              stringSort(DateFactureAsc, d.DateFacture.HasValue ? ((DateTime)d.DateFacture).ToString("yyyyMMdd") : string.Empty);

            return sort;
#pragma warning restore S3776
        }

        /// <summary>
        ///   Fonction pour le choix de l'ordre dsc
        /// </summary>
        /// <param name="asc">si tri ascendant true sinon false</param>
        /// <param name="s">chaine pour le tri</param>
        /// <returns>retourne la chaine valide pour le tri</returns>
        private string AscSort(bool? asc, string s)
        {
            return asc.HasValue && asc.Value ? s : string.Empty;
        }

        /// <summary>
        ///   Fonction pour le choix de l'ordre asc
        /// </summary>
        /// <param name="asc">si tri ascendant true sinon false</param>
        /// <param name="s">chaine pour le tri</param>
        /// <returns>retourne la chaine valide pour le tri</returns>
        private string DscSort(bool? asc, string s)
        {
            return asc.HasValue && !asc.Value ? s : string.Empty;
        }

        #endregion
    }
}
