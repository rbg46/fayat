using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Search;
using Fred.Web.Models.ReferentielFixe;

namespace Fred.Business.ExplorateurDepense
{
    /// <summary>
    /// Représente un modèle contenant les critères de filtrages des dépenses affichées dans l'explorateur des dépenses
    /// </summary>
    [Serializable]
    public class SearchExplorateurDepense : AbstractSearchEnt<ExplorateurDepenseGeneriqueModel>
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
        // TODO JVI #6000
        public bool TakeOd { get; set; }
        
        // TODO JVI if feature flipping OK alors
        //get 
        //{ 
        //    return TakeOdAch || TakeOdFg || TakeOdMi || TakeOdMit || TakeOdMo || TakeOdOth || TakeOdOthd || TakeOdRct; 
        //}        

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
        /// Indique si l'on doit remonté uniquement les energies
        /// </summary>
        public bool EnergieOnly { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur la famille OD de type Recettes
        /// </summary>
        public bool TakeOdRct { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur la famille OD de type Déboursé MO
        /// </summary>
        public bool TakeOdMo { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur la famille OD de type Déboursé achats
        /// </summary>
        public bool TakeOdAch { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur la famille OD de type Déboursé matériel Int
        /// </summary>
        public bool TakeOdMit { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur la famille OD de type Amortissement
        /// </summary>
        public bool TakeOdMi { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur la famille OD de type Autres dépenses
        /// </summary>
        public bool TakeOdOth { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur la famille OD de type Frais généraux
        /// </summary>
        public bool TakeOdFg { get; set; }

        /// <summary>
        /// Obtient ou définit un booléen déterminant si on filtre sur la famille OD de type Autres hors débours
        /// </summary>
        public bool TakeOdOthd { get; set; }

        /// <summary>
        /// Obtient ou définit le Fournisseur
        /// </summary>
        public FournisseurLightEnt Fournisseur { get; set; }

        /// <summary>
        /// Obtient ou définit la Ressource
        /// </summary>
        public RessourceLightEnt Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit la Tache
        /// </summary>
        public TacheLightEnt Tache { get; set; }

        /// <summary>
        /// Obtient ou définit le CI
        /// </summary>
        public CILightEnt CI { get; set; }

        /// <summary>
        /// Obtient ou définit la TypeRessource
        /// </summary>
        public TypeRessourceModel TypeRessource { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur la ressource
        /// </summary>
        public bool? RessourceAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur la tâche
        /// </summary>
        public bool? TacheAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur le libellé 1
        /// </summary>
        public bool? Libelle1Asc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur l'unité
        /// </summary>
        public bool? UniteAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur  la quantite
        /// </summary>
        public bool? QuantiteAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur le prix unitaire hors taxe
        /// </summary>
        public bool? PuHtAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur le montant hors taxe
        /// </summary>
        public bool? MontantHtAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur le code
        /// </summary>
        public bool? CodeAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur le libellé 2
        /// </summary>
        public bool? Libelle2Asc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur le commentaire
        /// </summary>
        public bool? CommentaireAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur la date dépense
        /// </summary>
        public bool? DateDepenseAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur la période
        /// </summary>
        public bool? PeriodeAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur la nature
        /// </summary>
        public bool? NatureAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur le type dépense
        /// </summary>
        public bool? TypeDepenseAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur le sous type dépense
        /// </summary>
        public bool? SousTypeDepenseAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur la période de facturation
        /// </summary>
        public bool? PeriodeFacturationAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur le numéro de facture
        /// </summary>
        public bool? NumeroFactureAsc { get; set; }

        /// <summary>
        /// Obtient ou définit l'ordre des axes analytique (Tache>Ressource ou Ressource>Tache)
        /// </summary>
        public int AxeAnalytique { get; set; }

        /// <summary>
        /// Obtient ou définit l'axe principal (T1,T2,T3) ou (Chapitre,SousChapitre,Ressource) : Utilisé pour la création de l'arborescence d'exploration
        /// </summary>
        public string[] AxePrincipal { get; set; }

        /// <summary>
        /// Obtient ou définit l'axe secondaire (T1,T2,T3) ou (Chapitre,SousChapitre,Ressource) : Utilisé pour la création de l'arborescence d'exploration
        /// </summary>
        public string[] AxeSecondaire { get; set; }

        /// <summary>
        /// Obtient ou définit la liste d'axes
        /// </summary>
        public List<Axe> Axes { get; set; }

        /// <summary>
        /// Détermine si le filtre sera utilisé lors d'un export excel ou pas
        /// </summary>
        public bool ForExport { get; set; }

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        /// <inheritdoc/>
        public override Expression<Func<ExplorateurDepenseGeneriqueModel, bool>> GetPredicateWhere()
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
                         && (!EnergieOnly || (EnergieOnly && x.IsEnergie)) && (!PeriodeDebut.HasValue || (100 * x.Periode.Year) + x.Periode.Month >= (100 * PeriodeDebut.Value.Year) + PeriodeDebut.Value.Month)
                         && (!PeriodeFin.HasValue || (100 * x.Periode.Year) + x.Periode.Month <= (100 * PeriodeFin.Value.Year) + PeriodeFin.Value.Month)
                         && (!DateDepenseDebut.HasValue || x.DateDepense.Date >= DateDepenseDebut.Value.Date)
                         && (!DateDepenseFin.HasValue || x.DateDepense.Date <= DateDepenseFin.Value.Date)
                         && (!DateFactureDebut.HasValue || (x.DateFacture.HasValue && x.DateFacture.Value.Date >= DateFactureDebut.Value.Date))
                         && (!DateFactureFin.HasValue || (x.DateFacture.HasValue && x.DateFacture.Value.Date <= DateFactureFin.Value.Date))
                         && (!FournisseurId.HasValue || (x.FournisseurId.HasValue && x.FournisseurId.Value == FournisseurId.Value))
                         && (!AgenceId.HasValue || (x.AgenceId.HasValue && x.AgenceId.Value == AgenceId.Value))
                         && (!RessourceId.HasValue || x.RessourceId == RessourceId.Value)
                         && (!TacheId.HasValue || x.TacheId == TacheId.Value)
                         && GetPredicationWhereForTypeDepense(x)
                         && GetPredicationWhereForSousTypeDepense(x)
                         && x.PUHT != 0
                         && (!MontantHTDebut.HasValue || x.MontantHT >= MontantHTDebut.Value)
                         && (!MontantHTFin.HasValue || x.MontantHT <= MontantHTFin.Value)
                         && (!DateRapprochement.HasValue || (x.DateRapprochement.HasValue && x.DateRapprochement.Value.Month == DateRapprochement.Value.Month && x.DateRapprochement.Value.Year == DateRapprochement.Value.Year))
                         && (!PersonnelInInsertion
                         || (PersonnelInInsertion && x.Personnel != null && x.Personnel.DateDebutInsertion.HasValue && x.Personnel.DateFinInsertion.HasValue
                         && x.Personnel.DateDebutInsertion <= PeriodeFin && (PeriodeDebut == null || x.Personnel.DateFinInsertion >= PeriodeDebut)))
                         && GetPredicateWhereForTypeRessource(x);
        
        }
        private bool GetPredicationWhereForTypeDepense(ExplorateurDepenseGeneriqueModel x)
        {
            return (!TakeReception && !TakeFacturation && !TakeValorisation && !TakeFar && !TakeOd)
                         || (TakeReception && !ForExport && x.TypeDepense == Constantes.DepenseType.Reception)
                         || (TakeReception && ForExport && (x.TypeDepense == Constantes.DepenseType.Reception || x.TypeDepense == Constantes.DepenseType.ExtourneFar))
                         || (TakeFacturation && x.TypeDepense == Constantes.DepenseType.Facturation)
                         || (TakeValorisation && x.TypeDepense == Constantes.DepenseType.Valorisation)
                         || (TakeFar && x.TypeDepense == Constantes.DepenseType.AjustementFar)
                         || TakeOd && x.TypeOd == Constantes.DepenseType.OD;
        }

        private bool GetPredicationWhereForSousTypeDepense(ExplorateurDepenseGeneriqueModel x)
        {
            return (!TakeEcart && !TakeAvoir && !TakeNonCommandee)
                         || (TakeEcart && x.SousTypeDepense == Constantes.DepenseSousType.Ecart)
                         || (TakeAvoir && x.SousTypeDepense == Constantes.DepenseSousType.Avoir)
                         || (TakeNonCommandee && x.SousTypeDepense == Constantes.DepenseSousType.NonCommandee);
        }

        private bool GetPredicateWhereForTypeRessource(ExplorateurDepenseGeneriqueModel x)
        {
            return TypeRessource == null 
                || (x.Ressource.TypeRessourceId.HasValue && x.Ressource.TypeRessourceId != 0 
                    && TypeRessource.TypeRessourceId != 0 
                    && x.Ressource.TypeRessourceId.Value == TypeRessource.TypeRessourceId);
        }

        public DateTime? PeriodeFacturation { get; set; }

        public int ValeurFiltreComplet { get; set; }

        public int? ValeurFiltreChap { get; set; }

        public int? ValeurFiltreT1 { get; set; }

        public int? ValeurFiltreT2 { get; set; }

        public int? ValeurFiltreSousChap { get; set; }

        public Func<ExplorateurDepenseGeneriqueModel, bool> GetPredicateWhereFor13C4C()
        {
            return x => (!PeriodeFin.HasValue || (100 * x.Periode.Year) + x.Periode.Month <= (100 * PeriodeFin.Value.Year) + PeriodeFin.Value.Month)
                          &&
                          (ValeurFiltreComplet == 1
                          || x.Ressource.SousChapitre.Chapitre.ChapitreId.Equals(ValeurFiltreChap)
                          || x.Ressource.SousChapitre.SousChapitreId.Equals(ValeurFiltreSousChap)
                          || x.Tache.Parent.Parent.TacheId.Equals(ValeurFiltreT1)
                          || x.Tache.Parent.TacheId == ValeurFiltreT2);
        }

        /// <inheritdoc/>
        protected override IOrderer<ExplorateurDepenseGeneriqueModel> GetDefaultOrderBy()
        {
            return new Orderer<ExplorateurDepenseGeneriqueModel, int>(c => c.DepenseId, true);
        }

        /// <inheritdoc/>
        protected override IOrderer<ExplorateurDepenseGeneriqueModel> GetUserOrderBy()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retourner le tri défini par l'utilisateur via l'interface web
        /// </summary>
        /// <returns>Le tri</returns>
        public IOrderer<ExplorateurDepenseGeneriqueModel> GetOrderBy()
        {
            if (RessourceAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, string>(c => $"{c.Ressource.Code} - {c.Ressource.Libelle}", RessourceAsc.Value);
            }
            if (TacheAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, string>(c => $"{c.Tache.Code} - {c.Tache.Libelle}", TacheAsc.Value);
            }
            if (Libelle1Asc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, string>(c => c.Libelle1, Libelle1Asc.Value);
            }
            if (UniteAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, string>(c => c.Unite.Code, UniteAsc.Value);
            }
            if (QuantiteAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, decimal?>(c => c.Quantite, QuantiteAsc.Value);
            }
            if (PuHtAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, decimal?>(c => c.PUHT, PuHtAsc.Value);
            }
            if (MontantHtAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, decimal>(c => c.MontantHT, MontantHtAsc.Value);
            }
            if (CodeAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, string>(c => c.Code, CodeAsc.Value);
            }
            if (Libelle2Asc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, string>(c => c.Libelle2, Libelle2Asc.Value);
            }

            return SecondPart();
        }

        private IOrderer<ExplorateurDepenseGeneriqueModel> SecondPart()
        {
            if (CommentaireAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, string>(c => c.Commentaire, CommentaireAsc.Value);
            }
            if (DateDepenseAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, DateTime>(c => c.DateDepense, DateDepenseAsc.Value);
            }
            if (PeriodeAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, DateTime>(c => c.Periode, PeriodeAsc.Value);
            }
            if (NatureAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, string>(c => $"{c.Nature.Code} - {c.Nature.Libelle}", NatureAsc.Value);
            }
            if (TypeDepenseAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, string>(c => c.TypeDepense, TypeDepenseAsc.Value);
            }
            if (SousTypeDepenseAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, string>(c => c.SousTypeDepense, SousTypeDepenseAsc.Value);
            }
            if (PeriodeFacturationAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, DateTime?>(c => c.DateFacture, PeriodeFacturationAsc.Value);
            }
            if (NumeroFactureAsc.HasValue)
            {
                return new Orderer<ExplorateurDepenseGeneriqueModel, string>(c => c.NumeroFacture, NumeroFactureAsc.Value);
            }

            return GetDefaultOrderBy();
        }
    }
}
