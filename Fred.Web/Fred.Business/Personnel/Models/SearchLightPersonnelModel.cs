using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.Budget.Helpers;
using Fred.Entities.Personnel;
using Microsoft.EntityFrameworkCore;
using static Fred.Entities.Constantes;

namespace Fred.Business.Personnel
{
    /// <summary>
    /// Model de recherche pour Lookup Personnel
    /// </summary>
    public class SearchLightPersonnelModel
    {
        // Tableau utilisé pour assimiler les types (ETAMChantier, ETAMBureau, ETAMArticle36) à ETAM (Statut = 2) . 
        private readonly string[] eTAMtypesValues = new string[] { TypePersonnel.ETAMChantier, TypePersonnel.ETAMBureau, TypePersonnel.ETAMArticle36 };

        #region Critères de recherche

        /// <summary>
        /// Texte recherché 
        /// </summary>
        public string Recherche { get; set; }

        /// <summary>
        /// Numéro de page
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Taille de la page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Identifiant du CI
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        /// Date du chantier / Date rapport
        /// </summary>
        public DateTime? DateChantier { get; set; }

        /// <summary>
        /// Periode du chantier / Date rapport
        /// </summary>
        public int? PeriodeChantier { get; set; }

        /// <summary>
        /// Texte 2 recherché
        /// </summary>
        public string Recherche2 { get; set; }

        /// <summary>
        /// Texte 3 recherché
        /// </summary>
        public string Recherche3 { get; set; }

        /// <summary>
        /// Seulement les personnels actifs à la date actuelle
        /// </summary>
        public bool ActifOnly { get; set; }

        /// <summary>
        /// Type d'auteur
        /// </summary>
        public string AuthorType { get; set; }

        /// <summary>
        /// Seulement la société gérante (dans le cas des SEP)
        /// </summary>
        public bool? OnlySocieteGerante { get; set; }

        /// <summary>
        /// Identifiant de la société
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Seulement les personnels utilisateur ou non
        /// </summary>
        public bool? OnlyUtilisateur { get; set; }

        /// <summary>
        /// Statut du personnel ETAM/IAC etc
        /// </summary>
        public string Statut { get; set; }

        /// <summary>
        ///   Obtient ou définit de statut de personnel
        /// </summary>
        public IEnumerable<string> StatutPersonnelList { get; set; } = new List<string>();

        /// <summary>
        /// Seulement pour les écrans affectations moyen
        /// </summary>
        public bool ForAffectationMoyen { get; set; }

        /// <summary>
        /// Indique si le résultat doit retourné les intérimaire
        /// </summary>
        public bool? IncludeInterimaire { get; set; }

        /// <summary>
        /// Identifiant du personnel
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        /// Indique si pour l'affectation CI
        /// </summary>
        public bool? IsForAffectationWithoutInterim { get; set; }
        #endregion

        #region Champs calculés

        /// <summary>
        /// Date du chantier / Date rapport
        /// </summary>
        public DateTime? DateDebutChantier
        {
            get
            {
                if (PeriodeChantier != null)
                {
                    return PeriodeHelper.ToFirstDayOfMonthDateTime((int)PeriodeChantier);
                }
                else if (DateChantier != null)
                {
                    return DateChantier;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Date du chantier / Date rapport
        /// </summary>
        public DateTime? DateFinChantier
        {
            get
            {
                if (PeriodeChantier != null)
                {
                    return PeriodeHelper.ToLastDayOfMonthDateTime((int)PeriodeChantier);
                }
                else if (DateChantier != null)
                {
                    return DateChantier;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Identifiant de l'établissement comptable
        /// </summary>
        public int? EtablissementComptableId { get; set; }

        /// <summary>
        /// Est un Sep ou non
        /// </summary>
        public bool IsSep { get; set; }

        /// <summary>
        /// Est pour GRZB ou non
        /// </summary>
        public bool IsGrzb { get; set; }

        /// <summary>
        /// Liste des identfiants de sociétés valides pour les SEP
        /// </summary>
        public List<int> SepEligibleSocieteIds { get; set; } = new List<int>();

        /// <summary>
        /// Liste des identfiants de sociétés valides pour les sociétés GRZB
        /// </summary>
        public List<int> GrzbEligibleSocieteIds { get; set; } = new List<int>();

        #endregion

        #region Prédicats

        /// <summary>
        /// Prédicat de recherche sur les infos personnels
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetSearchedTextPredicat()
        {
            return p => string.IsNullOrEmpty(Recherche)
                        || p.Matricule.Contains(Recherche)
                        || p.Nom.Contains(Recherche)
                        || p.Prenom.Contains(Recherche)
                        || p.Societe.Code.Contains(Recherche);
        }

        /// <summary>
        /// Prédicat de recherche sur les personnels actif seulement
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetActifOnlyPredicat()
        {
            return p => !ActifOnly || (p.DateSuppression == null || (p.DateSuppression.HasValue && p.DateSuppression.Value > DateTime.UtcNow))
                                                            && (p.DateSortie == null || (p.DateSortie.HasValue && p.DateSortie.Value > DateTime.UtcNow))
                                                            && p.DateEntree < DateTime.UtcNow;
        }

        /// <summary>
        /// Prédicat de recherche sur les personnels non-intérimaires GRZB
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetWithoutInterimairePredicat()
        {

            if (IsForAffectationWithoutInterim.HasValue && IsForAffectationWithoutInterim.Value)
            {
                return p => !p.IsInterimaire;
            }

            //laisse passer si
            // intérimaire
            // pas de date de chantier
            // si personnel actif à la date du chantier
            return p => p.IsInterimaire
                ||
                ((!DateDebutChantier.HasValue || (p.DateEntree.Value.Date <= DateDebutChantier.Value.Date && (!p.DateSortie.HasValue || p.DateSortie.Value.Date >= DateDebutChantier.Value.Date)))
                || (!DateFinChantier.HasValue || (p.DateEntree.Value.Date <= DateFinChantier.Value.Date && (!p.DateSortie.HasValue || p.DateSortie.Value.Date >= DateFinChantier.Value.Date)))
                || (DateDebutChantier.HasValue && DateFinChantier.HasValue &&
                    ((p.DateSortie.HasValue && p.DateEntree.Value.Date <= DateFinChantier.Value.Date && p.DateSortie.Value.Date >= DateDebutChantier.Value.Date && p.DateSortie.Value.Date <= DateFinChantier.Value.Date)
                      ||
                     (!p.DateSortie.HasValue && p.DateEntree.Value.Date <= DateFinChantier.Value.Date)))
                 )
                &&
                (!p.DateSuppression.HasValue || !DateChantier.HasValue || p.DateSuppression.Value > DateChantier.Value);
        }

        /// <summary>
        /// Prédicat de recherche sur les personnels intérimaires GRZB
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetWithInterimairePredicat()
        {

            if (IsForAffectationWithoutInterim.HasValue && IsForAffectationWithoutInterim.Value)
            {
                return p => !p.IsInterimaire;
            }

            //laisse passer si :
            // pas intérimaire
            // intémaire suivant 3 cas
            // 1 - pas date de chantier ou date de chantier compris dans contrats avec souplesse
            // 2 - pas de ciid ou ci identique à contrat.ci
            // 3 - pas etablissement ou etablissement dans zone travail du contrat
            if (DateDebutChantier.HasValue &&
                DateFinChantier.HasValue &&
                DateDebutChantier.Value.Date == DateFinChantier.Value.Date)
            {
                return p => !p.IsInterimaire ||
                  (p.ContratInterimaires
                    .Any(
                        c =>
                        ((DateDebutChantier.HasValue && DateFinChantier.HasValue && c.DateDebut.Date <= DateDebutChantier.Value.Date && DateFinChantier.Value.Date <= c.DateFin.AddDays(c.Souplesse).Date)
                        &&
                        ((!CiId.HasValue || c.CiId == CiId)
                        ||
                        (!EtablissementComptableId.HasValue || c.ZonesDeTravail.Any(z => z.EtablissementComptableId == EtablissementComptableId))))
                    )
                  );
            }
            else
            {
                return p => !p.IsInterimaire ||
                  (p.ContratInterimaires
                    .Any(
                        c =>
                        ((!DateDebutChantier.HasValue || (c.DateDebut.Date <= DateDebutChantier.Value.Date && DateDebutChantier.Value.Date <= c.DateFin.AddDays(c.Souplesse).Date))
                        || (!DateFinChantier.HasValue || (c.DateDebut.Date <= DateFinChantier.Value.Date && DateFinChantier.Value.Date <= c.DateFin.AddDays(c.Souplesse).Date))
                        || (DateDebutChantier.HasValue && DateFinChantier.HasValue && DateDebutChantier.Value.Date <= c.DateDebut.Date && c.DateFin.AddDays(c.Souplesse).Date <= DateFinChantier.Value.Date)
                        &&
                        ((!CiId.HasValue || c.CiId == CiId)
                        ||
                        (!EtablissementComptableId.HasValue || c.ZonesDeTravail.Any(z => z.EtablissementComptableId == EtablissementComptableId))))
                    )
                  );
            }
        }

        /// <summary>
        /// Prédicat de recherche sur les personnels intérimaires GFES
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetGfesInterimairePredicat()
        {
            return p => !p.IsInterimaire || !DateChantier.HasValue ||
            (p.DateEntree.Value.Date <= DateChantier.Value.Date
                && (p.DateSortie.Value.Date >= DateChantier.Value.Date)
                && (!p.DateSuppression.HasValue || p.DateSuppression.Value.Date >= DateChantier.Value.Date)
             );
        }

        /// <summary>
        /// Prédicat de recherche sur les les infos des personnels
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetImprovedSearchedTextPredicat()
        {
            return p => (string.IsNullOrEmpty(Recherche) || p.Nom.Contains(Recherche))
                        && (string.IsNullOrEmpty(Recherche2)
                           || p.Prenom.Contains(Recherche2))
                        && (string.IsNullOrEmpty(Recherche3)
                           || (p.Societe.Code.Contains(Recherche3)
                           || p.Societe.Libelle.Contains(Recherche3)
                           || p.Matricule.Contains(Recherche3)
                           || p.EtablissementPaie.Libelle.Contains(Recherche3)
                           || p.EtablissementPaie.Code.Contains(Recherche3)));
        }

        /// <summary>
        /// Prédicat de recherche sur les statuts des personnels
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetStatutPredicat()
        {
            return p => string.IsNullOrEmpty(Statut) || (eTAMtypesValues.Contains(p.Statut) ? TypePersonnel.ETAM : p.Statut) == Statut;
        }

        /// <summary>
        /// Prédicat de recherche sur les statuts des personnels
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetStatutPredicatFes()
        {
            return p => !StatutPersonnelList.Any() || StatutPersonnelList.Contains(p.Statut);
        }

        /// <summary>
        /// Prédicat de recherche sur les personnels du groupe Razel-Bec
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetGrzbPredicat()
        {
            return x => !IsGrzb || !CiId.HasValue || GrzbEligibleSocieteIds.Count == 0 || IsSep || (!IsSep && GrzbEligibleSocieteIds.Contains(x.SocieteId.Value));
        }

        /// <summary>
        /// Prédicat de recherche sur les personnels liés au SEP
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetSepPredicat()
        {
            return x => !IsSep || SepEligibleSocieteIds.Count == 0 || SepEligibleSocieteIds.Contains(x.SocieteId.Value);
        }

        /// <summary>
        /// Prédicat de recherche sur les personnels utilisateur
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetOnlyUtilisateurPredicat()
        {
            return p => !OnlyUtilisateur.HasValue || (OnlyUtilisateur.Value && p.Utilisateur != null) || (!OnlyUtilisateur.Value && p.Utilisateur == null);
        }

        /// <summary>
        /// Prédicat de recherch sur les personnels interimaire
        /// Si le champ IncludeInterimaire vaut true ou s'il est nul alors le le résultat devrat prendre en compte tous les personnels.
        /// Si le champ IncludeInterimaire vaut false alors il ne faudra pas inclure les intérimaires
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetFullPersonnel()
        {
            return p => !IncludeInterimaire.HasValue || ((IncludeInterimaire.Value && p.IsInterimaire) || (!IncludeInterimaire.Value && !p.IsInterimaire));
        }

        /// <summary>
        /// Prédicat de recherche des personnels du niveau hierarchique inférieur
        /// </summary>
        /// <returns>Expression</returns>
        public Expression<Func<PersonnelEnt, bool>> GetHierarchieNmoins1()
        {
            return p => (p.ManagerId.HasValue && PersonnelId.HasValue && p.ManagerId == PersonnelId
                        && p.ManagerPersonnels.Any() && p.PersonnelId != PersonnelId
                        && (p.DateEntree.HasValue && p.DateEntree.Value.CompareTo(DateTime.UtcNow) <= 0)
                        && (!p.DateSortie.HasValue || (PeriodeChantier.HasValue && (100 * p.DateSortie.Value.Year) + p.DateSortie.Value.Month >= PeriodeChantier) || p.DateSortie >= DateTime.UtcNow.Date)
                        && (!p.DateSuppression.HasValue || p.DateSuppression.Value.CompareTo(DateTime.UtcNow) >= 0));
        }

        #endregion
    }
}
