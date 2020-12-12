using System;
using System.Linq.Expressions;
using Fred.Entities;
using Fred.Entities.Search;
using Fred.Web.Shared.Models.Depense;

namespace Fred.Business.Depense
{
    /// <summary>
    /// Représente un modèle contenant les critères de filtrages des dépenses
    /// </summary>
    [Serializable]
    public class SearchDepense : AbstractSearchEnt<DepenseExhibition>
    {   
        /// <summary>
        /// Obtient ou définit la période de début (filtré sur date comptable des dépenses)
        /// </summary>
        public DateTime? PeriodeDebut { get; set; }

        /// <summary>
        /// Obtient ou définit la période de fin (filtré sur date comptable des dépenses)
        /// </summary>
        public DateTime? PeriodeFin { get; set; }

        /// <summary>
        /// Obtient ou définit la date de début de la facture
        /// </summary>
        public DateTime? DateFactureDebut { get; set; }

        /// <summary>
        /// Obtient ou définit la date de fin de la facture
        /// </summary>
        public DateTime? DateFactureFin { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du CI
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou définit la date de début de la dépense
        /// </summary>
        public DateTime? DateDepenseDebut { get; set; }

        /// <summary>
        /// Obtient ou définit la date de fin de la dépense
        /// </summary>
        public DateTime? DateDepenseFin { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du fournisseur
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        /// Identifiant de l'agence recherchée
        /// </summary>
        public int? AgenceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la ressource
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la tâche
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        /// Obtient ou définit le montant HT inférieur
        /// </summary>
        public decimal? MontantHTDebut { get; set; }

        /// <summary>
        /// Obtient ou définit le montant HT supérieur
        /// </summary>
        public decimal? MontantHTFin { get; set; }

        /// <summary>
        /// Obtient ou définit la date de rapprochement facture
        /// </summary>
        public DateTime? DateRapprochement { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les réceptions
        /// </summary>
        public bool TakeReception { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les valorisations
        /// </summary>
        public bool TakeValorisation { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les facturations
        /// </summary>
        public bool TakeFacturation { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les ajustement far
        /// </summary>
        public bool TakeFar { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les od
        /// </summary>
        public bool TakeOd { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les ecarts
        /// </summary>
        public bool TakeEcart { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les non commandées
        /// </summary>
        public bool TakeNonCommandee { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les avoirs
        /// </summary>
        public bool TakeAvoir { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les MO int.
        /// </summary>
        public bool TakeMOInt { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les MO intérim.
        /// </summary>
        public bool TakeMOInterim { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les Materiaux Int.
        /// </summary>
        public bool TakeMaterielInt { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les Materiaux Ext.
        /// </summary>
        public bool TakeMaterielExt { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur les personnels en insertion
        /// </summary>
        public bool PersonnelInInsertion { get; set; }

        /// <summary>
        /// Détermine si le filtre sera utilisé lors d'un export excel ou pas
        /// </summary>
        public bool ForExport { get; set; }

        /// <summary>
        ///  Permet de récupérer le prédicat de recherche.
        /// </summary>
        /// <returns>Retourne la condition de recherche</returns>
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        public override Expression<Func<DepenseExhibition, bool>> GetPredicateWhere()
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            return x => (string.IsNullOrEmpty(ValueText)
                         || x.Ressource.SousChapitre.Chapitre.Code.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || x.Ressource.SousChapitre.Chapitre.Libelle.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || x.Ressource.SousChapitre.Code.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || x.Ressource.SousChapitre.Libelle.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || x.Ressource.Code.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || x.Ressource.Libelle.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || x.Tache.Parent.Parent.Code.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || x.Tache.Parent.Parent.Libelle.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || x.Tache.Parent.Code.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || x.Tache.Parent.Libelle.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || x.Tache.Code.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || x.Tache.Libelle.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0
                         || (!string.IsNullOrEmpty(x.Libelle1) && x.Libelle1.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                         || (!string.IsNullOrEmpty(x.Code) && x.Code.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                         || (!string.IsNullOrEmpty(x.Libelle2) && x.Libelle2.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                         || (!string.IsNullOrEmpty(x.Commentaire) && x.Commentaire.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                         || (x.Nature != null && x.Nature.Code.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                         || (x.Nature != null && x.Nature.Libelle.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                         || (!string.IsNullOrEmpty(x.NumeroFacture) && x.NumeroFacture.IndexOf(ValueText, StringComparison.CurrentCultureIgnoreCase) >= 0))
                         && (!PeriodeDebut.HasValue || (100 * x.Periode.Year) + x.Periode.Month >= (100 * PeriodeDebut.Value.Year) + PeriodeDebut.Value.Month)
                         && (!PeriodeFin.HasValue || (100 * x.Periode.Year) + x.Periode.Month <= (100 * PeriodeFin.Value.Year) + PeriodeFin.Value.Month)
                         && (!DateDepenseDebut.HasValue || x.DateDepense.Date >= DateDepenseDebut.Value.Date)
                         && (!DateDepenseFin.HasValue || x.DateDepense.Date <= DateDepenseFin.Value.Date)
                         && (!DateFactureDebut.HasValue || (x.DateFacture.HasValue && x.DateFacture.Value.Date >= DateFactureDebut.Value.Date))
                         && (!DateFactureFin.HasValue || (x.DateFacture.HasValue && x.DateFacture.Value.Date <= DateFactureFin.Value.Date))
                         && (!FournisseurId.HasValue || (x.FournisseurId.HasValue && x.FournisseurId.Value == FournisseurId.Value))
                         && (!AgenceId.HasValue || (x.AgenceId.HasValue && x.AgenceId.Value == AgenceId.Value))
                         && (!RessourceId.HasValue || x.RessourceId == RessourceId.Value)
                         && (!TacheId.HasValue || x.TacheId == TacheId.Value)
                         && ((!TakeReception && !TakeFacturation && !TakeValorisation && !TakeFar && !TakeOd)
                         || (TakeReception && !ForExport && x.TypeDepense == Constantes.DepenseType.Reception)
                         || (TakeReception && ForExport && (x.TypeDepense == Constantes.DepenseType.Reception || x.TypeDepense == Constantes.DepenseType.ExtourneFar))
                         || (TakeFacturation && x.TypeDepense == Constantes.DepenseType.Facturation)
                         || (TakeValorisation && x.TypeDepense == Constantes.DepenseType.Valorisation)
                         || (TakeFar && x.TypeDepense == Constantes.DepenseType.AjustementFar)
                         || (TakeOd && x.TypeDepense == Constantes.DepenseType.OD)
                         && ((!TakeEcart && !TakeAvoir && !TakeNonCommandee)
                         || (TakeEcart && x.SousTypeDepense == Constantes.DepenseSousType.Ecart)
                         || (TakeAvoir && x.SousTypeDepense == Constantes.DepenseSousType.Avoir)
                         || (TakeNonCommandee && x.SousTypeDepense == Constantes.DepenseSousType.NonCommandee))
                         && (!MontantHTDebut.HasValue || x.MontantHT >= MontantHTDebut.Value)
                         && (!MontantHTFin.HasValue || x.MontantHT <= MontantHTFin.Value)
                         && (!DateRapprochement.HasValue || (x.DateRapprochement.HasValue && x.DateRapprochement.Value.Month == DateRapprochement.Value.Month && x.DateRapprochement.Value.Year == DateRapprochement.Value.Year))
                         && (x.PUHT != 0)
                         && (!PersonnelInInsertion
                         || (PersonnelInInsertion && x.Personnel != null && x.Personnel.DateDebutInsertion.HasValue && x.Personnel.DateFinInsertion.HasValue
                         && x.Personnel.DateDebutInsertion <= PeriodeFin && x.Personnel.DateFinInsertion >= PeriodeDebut)));
        }

        /// <summary>
        /// Retourner le tri par défaut (tri interne).
        /// </summary>
        /// <returns>Le tri</returns>
        protected override IOrderer<DepenseExhibition> GetDefaultOrderBy()
        {
            return new Orderer<DepenseExhibition, int>(c => c.DepenseId, true);
        }

        /// <summary>
        /// Retourner le tri défini par l'utilisateur.
        /// </summary>
        /// <returns>Le tri</returns>
        protected override IOrderer<DepenseExhibition> GetUserOrderBy()
        {
            throw new NotImplementedException();
        }
    }
}
