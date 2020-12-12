using System;
using System.Collections.Generic;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Search;
using Fred.Web.Shared.Models.Personnel.Light;

namespace Fred.Web.Models.Rapport
{
    /// <summary>
    /// Classe représentant les critères de recherches des rapports
    /// </summary>
    public class SearchRapportModel : ISearchValueModel
    {
        public string ValueText { get; set; }

        /// <summary>
        /// Identifiant de l'utilisateur ayant effectué la recherche
        /// </summary>
        public int DemandeurId { get; set; }

        #region Champs de recherche
        /// <summary>
        /// Obtient ou défini une valeur indiquant si on recherche sur le code du CI
        /// </summary>
        public bool CiCode { get; set; }

        /// <summary>
        /// Obtient ou défini une valeur indiquant si on recherche sur le Libelle du CI
        /// </summary>
        public bool CiLibelle { get; set; }

        /// <summary>
        /// Obtient ou défini une valeur indiquant si on recherche sur le nom du rédacteur
        /// </summary>
        public bool Redacteur { get; set; }

        /// <summary>
        /// Obtient ou défini une valeur indiquant si on recherche sur nom du valideur
        /// </summary>
        public bool Valideur { get; set; }

        #endregion

        #region Critères
        /// <summary>
        /// Obtient ou défini une valeur indiquant l'identifiant du statut des rapport à afficher
        /// </summary>
        public string StatutCode { get; set; }

        /// <summary>
        /// Obtient ou définit le critère définissant que les rapports de tout statut doivent être récupéré
        /// </summary>
        public bool ToutStatut { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports de statut en cours doivent être récupéré
        /// </summary>
        public bool StatutEnCours { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports de statut valide doivent être récupéré
        /// </summary>
        public bool StatutValide1 { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports de statut valide doivent être récupéré
        /// </summary>
        public bool StatutValide2 { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports de statut valide doivent être récupéré
        /// </summary>
        public bool StatutValide3 { get; set; } = false;

        /// <summary>
        /// Obtient ou définit le critère définissant que les rapports de statut vérouillé doivent être récupéré
        /// </summary>
        public bool StatutVerrouille { get; set; }

        /// <summary>
        /// Obtient ou définit le critère définissant que les rapports non vérouillés doivent être récupéré
        /// </summary>
        public bool StatutNonVerrouille { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports contenant des personnels ouvriers doivent être récupérés
        /// </summary>
        public bool StatutOuvrier { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports contenant des personnels etam doivent être récupérés
        /// </summary>
        public bool StatutEtam { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports contenant des personnels cadre doivent être récupérés
        /// </summary>
        public bool StatutCadre { get; set; }

        /// <summary>
        ///   Obtient ou définit l'organisation
        /// </summary>
        public OrganisationLightModel Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de organisation
        /// </summary>
        public int? OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant la date comptable
        /// </summary>
        public DateTime? DateComptable { get; set; }

        /// <summary>
        /// Obtient ou défini une valeur indiquant la date de rapport minimum 
        /// </summary>
        public DateTime? DateChantierMin { get; set; }
        /// <summary>
        /// Obtient ou défini une valeur indiquant la date de rapport maximum
        /// </summary>
        public DateTime? DateChantierMax { get; set; }

        /// <summary>
        /// Obtient ou définit une variable indiquant si le rôle de l'utilisateur connecté est gestionnaire de paye
        /// </summary>
        public bool IsGSP { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des identifiants unique d'établissement de paie 
        /// </summary>
        public List<int?> EtablissementPaieIdList { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel auteur du rapport
        /// </summary>
        public PersonnelLightForPickListModel AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel valideur de niveau 1 du rapport
        /// </summary>
        public PersonnelLightForPickListModel Valideur1 { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel valideur de niveau 2 du rapport
        /// </summary>
        public PersonnelLightForPickListModel Valideur2 { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel valideur de niveau 3 du rapport
        /// </summary>
        public PersonnelLightForPickListModel Valideur3 { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel verrouilleur du rapport
        /// </summary>
        public PersonnelLightForPickListModel Verrouilleur { get; set; }
        #endregion

        #region Tris
        /// <summary>
        /// Obtient ou définit une valeur indiquant l'ordre du tri sur le code du CI
        /// </summary>
        public bool? CiCodeAsc { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant l'ordre du tri sur le numéro du rapport
        /// </summary>
        public bool? NumeroRapportAsc { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant l'ordre du tri sur le statut des rapports
        /// </summary>
        public bool? StatutAsc { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant l'ordre du tri sur les dates des rapports
        /// </summary>
        public bool? DateChantierAsc { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des champs sur lequel s'applique le tri
        /// </summary>
        public IDictionary<string, bool?> SortFields { get; set; }
        #endregion

        #region Rapports à ne pas verrouiller ou déverrouiler

        /// <summary>
        ///     Liste des identifiants des rapports à ne pas verrouiller/déverrouiller lorsque l'on clique sur "Tout sélectionner"
        /// </summary>
        public List<int> UnselectedRapports { get; set; } = new List<int>();

        #endregion
    }
}
