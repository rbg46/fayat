using System;
using System.Collections.Generic;
using Fred.Web.Models.Referential.Light;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.ReferentielFixe.Light;
using Fred.Web.Models.Search;
using Fred.Web.Shared.Models;

namespace Fred.Web.Models
{
    /// <summary>
    /// Représente un modèle contenant les critères de filtrages des dépenses affichées dans l'explorateur des dépenses
    /// </summary>
    public class SearchExplorateurDepenseModel : ISearchValueModel
    {
        /// <summary>
        /// Obtient ou définit le champ de texte de recherche
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        /// Obtient ou définit la période de début (filtré sur date comptable des dépenses)
        /// </summary>
        public DateTime? PeriodeDebut { get; set; }

        /// <summary>
        /// Obtient ou définit la période de fin (filtré sur date comptable des dépenses)
        /// </summary>
        public DateTime? PeriodeFin { get; set; }

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
        /// Obtient ou définit la date de début de la facture
        /// </summary>
        public DateTime? DateFactureDebut { get; set; }

        /// <summary>
        /// Obtient ou définit la date de fin de la facture
        /// </summary>
        public DateTime? DateFactureFin { get; set; }

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
        /// Obtient ou définit la date rapprochement facture
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
        /// Obtient ou définit un booléen déterminant si on filtre uniquement sur les commandes de type energie
        /// </summary>
        public bool EnergieOnly { get; set; }

        /// <summary>
        /// Obtient ou définit le Fournisseur
        /// </summary>
        public FournisseurLightModel Fournisseur { get; set; }

        /// <summary>
        /// Obtient ou définit la Ressource
        /// </summary>
        public RessourceLightModel Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit la TypeRessource
        /// </summary>
        public TypeRessourceModel TypeRessource { get; set; }

        /// <summary>
        /// Obtient ou définit la Tache
        /// </summary>
        public TacheLightModel Tache { get; set; }

        /// <summary>
        /// Obtient ou définit le CI
        /// </summary>
        public CILightModel CI { get; set; }

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

        #region Tri

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

        #endregion

        #region Axes d'analyse

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
        public List<AxeModel> Axes { get; set; }

        #endregion
    }
}
