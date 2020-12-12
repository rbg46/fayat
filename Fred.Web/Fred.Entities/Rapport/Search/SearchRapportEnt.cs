using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.Organisation;
using Fred.Entities.Personnel;
using Fred.Entities.Search;

namespace Fred.Entities.Rapport.Search
{
#pragma warning disable S3776
    /// <summary>
    ///   Représente une recherche de rapport
    /// </summary>
    [Serializable]
    public class SearchRapportEnt : AbstractSearch
    {
        /// <summary>
        ///   Identifiant de l'utilisateur ayant effectué la recherche
        /// </summary>
        public int DemandeurId { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des Ci accessible pour l'utilisateur connecté
        /// </summary>
        public ICollection<int> Cis { get; set; }

        #region Critères

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports de tout statut doivent être récupéré
        /// </summary>
        public bool ToutStatut { get; set; } = false;

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
        ///   Obtient ou définit le critère définissant que les rapports de statut vérouillé doivent être récupéré
        /// </summary>
        public bool StatutVerrouille { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports contenant des personnels ouvriers doivent être récupérés
        /// </summary>
        public bool StatutOuvrier { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports contenant des personnels etam doivent être récupérés
        /// </summary>
        public bool StatutEtam { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports contenant des personnels cadre doivent être récupérés
        /// </summary>
        public bool StatutCadre { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit le critère définissant que les rapports non vérouillés doivent être récupéré
        /// </summary>
        public bool StatutNonVerrouille { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit l'organisation
        /// </summary>
        public OrganisationLightEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de organisation
        /// </summary>
        public int? OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant la date comptable
        /// </summary>
        public DateTime? DateComptable { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant la date de rapport minimum
        /// </summary>
        public DateTime? DateChantierMin { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant la date de rapport maximum
        /// </summary>
        public DateTime? DateChantierMax { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des identifiants unique d'établissement de paie 
        /// </summary>
        public List<int?> EtablissementPaieIdList { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel auteur du rapport
        /// </summary>
        public PersonnelLightForPickListEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel valideur de niveau 1 du rapport
        /// </summary>
        public PersonnelLightForPickListEnt Valideur1 { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel valideur de niveau 2 du rapport
        /// </summary>
        public PersonnelLightForPickListEnt Valideur2 { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel valideur de niveau 3 du rapport
        /// </summary>
        public PersonnelLightForPickListEnt Valideur3 { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel verrouilleur du rapport
        /// </summary>
        public PersonnelLightForPickListEnt Verrouilleur { get; set; }

        #endregion

        #region Tris

        /// <summary>
        ///   Obtient ou définit une valeur indiquant l'ordre du tri sur le code du CI
        /// </summary>
        public bool? CiCodeAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant l'ordre du tri sur le numéro du rapport
        /// </summary>
        public bool? NumeroRapportAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant l'ordre du tri sur le statut des rapports
        /// </summary>
        public bool? StatutAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant l'ordre du tri sur les dates des rapports
        /// </summary>
        public bool? DateChantierAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des champs sur lequel s'applique le tri
        /// </summary>
        public IDictionary<string, bool?> SortFields { get; set; }

        /// <summary>
        ///   Obtient ou définit une variable indiquant si le rôle de l'utilisateur connecté est gestionnaire de paye
        /// </summary>
        public bool IsGSP { get; set; }

        #endregion

        #region Rapports à ne pas verrouiller ou déverrouiler

        /// <summary>
        ///     Liste des identifiants des rapports à ne pas verrouiller/déverrouiller lorsque l'on clique sur "Tout sélectionner"
        /// </summary>
        public List<int> UnselectedRapports { get; set; } = new List<int>();

        /// <summary>
        /// Recupère l'id du filtre AuteurCreation : Nécessaire pour le prédicat de recherche, car il ne peut pas gérer des objets
        /// </summary>
        private int? AuteurCreationId => AuteurCreation?.PersonnelId;

        /// <summary>
        /// Recupère l'id du filtre Valideur1 : Nécessaire pour le prédicat de recherche, car il ne peut pas gérer des objets
        /// </summary>
        private int? Valideur1Id => Valideur1?.PersonnelId;

        /// <summary>
        /// Recupère l'id du filtre Valideur2 : Nécessaire pour le prédicat de recherche, car il ne peut pas gérer des objets
        /// </summary>
        private int? Valideur2Id => Valideur2?.PersonnelId;

        /// <summary>
        /// Recupère l'id du filtre Valideur3 : Nécessaire pour le prédicat de recherche, car il ne peut pas gérer des objets
        /// </summary>
        private int? Valideur3Id => Valideur3?.PersonnelId;

        /// <summary>
        /// Recupère l'id du filtre AuteurCreation : Nécessaire pour le prédicat de recherche, car il ne peut pas gérer des objets
        /// </summary>
        private int? VerrouilleurId => Verrouilleur?.PersonnelId;


        #endregion

        #region Predicate
        /// <summary>
        ///   Permet de récupérer le prédicat de recherche
        /// </summary>
        /// <returns>Retourne la condition de recherche</returns>
        public Expression<Func<RapportEnt, bool>> GetPredicateWhere()
        {
            string valueText = ValueText?.ToLower();
            var unselectedRapports = UnselectedRapports ?? Enumerable.Empty<int>();

#pragma warning disable RCS1155 // Use StringComparison when comparing strings.
            return p =>
                  ((AuteurCreationId == null || (AuteurCreationId != null && p.AuteurCreationId.HasValue && p.AuteurCreationId.Value == AuteurCreationId))
                  && (Valideur1Id == null || (Valideur1Id != null && p.ValideurCDCId.HasValue && p.ValideurCDCId.Value == Valideur1Id))
                  && (Valideur2Id == null || (Valideur2Id != null && p.ValideurCDTId.HasValue && p.ValideurCDTId.Value == Valideur2Id))
                  && (Valideur3Id == null || (Valideur3Id != null && p.ValideurDRCId.HasValue && p.ValideurDRCId.Value == Valideur3Id))
                  && (VerrouilleurId == null || (VerrouilleurId != null && p.AuteurVerrouId.HasValue && p.AuteurVerrouId.Value == VerrouilleurId)))
                  &&
                  (string.IsNullOrEmpty(valueText)
                  || p.CI.Code.ToLower().Equals(valueText)
                  || p.RapportId.ToString().Equals(valueText)
                  || p.CI.Libelle.ToLower().Contains(valueText)
                  || p.AuteurCreation.Personnel.Nom.ToLower().Contains(valueText)
                  || p.AuteurCreation.Personnel.Prenom.ToLower().Contains(valueText)
                  || (p.ValideurCDC != null && p.ValideurCDC.Personnel != null && (p.ValideurCDC.Personnel.Nom.ToLower().Contains(valueText) || p.ValideurCDC.Personnel.Prenom.ToLower().Contains(valueText)))
                  || (p.ValideurCDT != null && p.ValideurCDT.Personnel != null && (p.ValideurCDT.Personnel.Nom.ToLower().Contains(valueText) || p.ValideurCDT.Personnel.Prenom.ToLower().Contains(valueText)))
                  || (p.ValideurDRC != null && p.ValideurDRC.Personnel != null && (p.ValideurDRC.Personnel.Nom.ToLower().Contains(valueText) || p.ValideurDRC.Personnel.Prenom.ToLower().Contains(valueText))))
#pragma warning restore RCS1155 // Use StringComparison when comparing strings.
                        && ((!StatutEnCours && !StatutValide1 && !StatutValide2 && !StatutValide3 && !StatutVerrouille)
                     || (StatutValide1 && p.RapportStatutId == RapportStatutEnt.RapportStatutValide1.Key)
                     || (StatutValide2 && p.RapportStatutId == RapportStatutEnt.RapportStatutValide2.Key)
                     || (StatutValide3 && p.RapportStatutId == RapportStatutEnt.RapportStatutValide3.Key)
                     || (StatutVerrouille && p.RapportStatutId == RapportStatutEnt.RapportStatutVerrouille.Key)
                     || (StatutEnCours && p.RapportStatutId == RapportStatutEnt.RapportStatutEnCours.Key))
                    && ((!StatutOuvrier && !StatutEtam && !StatutCadre)
                     || (StatutOuvrier && p.ListLignes.Any(x => x.Personnel.Statut == Constantes.TypePersonnel.Ouvrier))
                     || (StatutEtam && p.ListLignes.Any(x => x.Personnel.Statut == Constantes.TypePersonnel.ETAM))
                     || (StatutCadre && p.ListLignes.Any(x => x.Personnel.Statut == Constantes.TypePersonnel.Cadre)))
                    && (!DateChantierMin.HasValue || p.DateChantier.Date >= DateChantierMin.Value.Date)
                    && (!DateChantierMax.HasValue || p.DateChantier.Date <= DateChantierMax.Value.Date)
                    && (!DateComptable.HasValue || (p.DateChantier.Month == DateComptable.Value.Month && p.DateChantier.Year == DateComptable.Value.Year))
                    && (Cis.Contains(p.CiId) || (EtablissementPaieIdList != null && EtablissementPaieIdList.Any()))
                    && !p.DateSuppression.HasValue
                    && !p.IsGenerated
                    && (!unselectedRapports.Any() || !unselectedRapports.Contains(p.RapportId));
        }

        /// <summary>
        ///   Permet de récupérer le prédicat d'ordonnacement ascendant
        /// </summary>
        /// <returns>prédicat d'ordonnacement ascendant</returns>
        public Expression<Func<RapportEnt, string>> GetPredicateOrderByASC()
        {
            Expression<Func<RapportEnt, string>> orderByAsc;
            orderByAsc = p => (CiCodeAsc.HasValue && CiCodeAsc.Value ? p.CI.Code : string.Empty) +
                              (StatutAsc.HasValue && StatutAsc.Value ? p.RapportStatut.Libelle : string.Empty) +
                              (NumeroRapportAsc.HasValue && NumeroRapportAsc.Value ? p.RapportId.ToString() : string.Empty) +
                              (DateChantierAsc.HasValue && DateChantierAsc.Value ? p.DateChantier.ToString() : string.Empty);
            return orderByAsc;
        }

        /// <summary>
        ///   Permet de récupérer le prédicat d'ordonnacement descendant
        /// </summary>
        /// <returns>prédicat d'ordonnacement descendant</returns>
        public Expression<Func<RapportEnt, string>> GetPredicateOrderByDESC()
        {
            Expression<Func<RapportEnt, string>> orderByDesc;
            orderByDesc = p => (CiCodeAsc.HasValue && !CiCodeAsc.Value ? p.CI.Code : string.Empty) +
                               (StatutAsc.HasValue && !StatutAsc.Value ? p.RapportStatut.Libelle : string.Empty) +
                               (NumeroRapportAsc.HasValue && !NumeroRapportAsc.Value ? p.RapportId.ToString() : string.Empty) +
                               (DateChantierAsc.HasValue && !DateChantierAsc.Value ? p.DateChantier.ToString() : string.Empty);
            return orderByDesc;
        }

        #endregion
#pragma warning restore S3776
    }
}
