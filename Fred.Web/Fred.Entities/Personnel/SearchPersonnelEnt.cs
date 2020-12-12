using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Referential;
using Fred.Entities.Search;
using Fred.Entities.Societe;

namespace Fred.Entities.Personnel
{
    /// <summary>
    ///   Représente un membre du personnel
    /// </summary>
    [Serializable]
    public class SearchPersonnelEnt : AbstractSearchEnt<PersonnelEnt>
    {
        #region Scope de recherche

        /// <summary>
        /// Valeur textuelle du nom recherché
        /// </summary>
        public string ValueTextNom { get; set; }

        /// <summary>
        /// Valeur textuelle du prénom recherché
        /// </summary>
        public string ValueTextPrenom { get; set; }

        /// <summary>
        /// Valeur textuelle du code etablissement paie recherché
        /// </summary>
        public EtablissementPaieLightEnt SearchEtab { get; set; }

        /// <summary>
        /// Valeur textuelle du code société recherché
        /// </summary>
        public SocieteLightEnt SearchSociete { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le nom du personnel.
        /// </summary>
        public bool Nom { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le prénom du personnel.
        /// </summary>
        public bool Prenom { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le libellé de la société.
        /// </summary>
        public bool SocieteCodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le matricule du personnel.
        /// </summary>
        public bool Matricule { get; set; }

        /// <summary>
        /// Obtient ou definit une valeur indiquant les personnels non pointables
        /// </summary>
        public bool IsPersonnelsNonPointables { get; set; }

        #endregion

        #region Critères

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Etablissement de paie
        /// </summary>
        public string EtablissementPaieCode { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Societe
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le personnel est interimaire ou non.
        /// </summary>
        public bool IsInterne { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le personnel est interne ou externe.
        /// </summary>
        public bool IsInterimaire { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le personnel est un utilisateur Fred.
        /// </summary>
        public bool IsUtilisateur { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le personnel est inactif.
        /// </summary>
        public bool IsActif { get; set; }

        #endregion

        #region Tris

        /// <summary>
        ///   Obtient ou définit le tri : Nom et prénom
        /// </summary>
        public bool? NomPrenomAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Libelle Societe
        /// </summary>
        public bool? SocieteAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Matricule
        /// </summary>
        public bool? MatriculeAsc { get; set; }

        #endregion

        #region Génération de prédicat de recherche

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche des personnels.
        /// </summary>
        /// <returns>Retourne la condition de recherche des personnels</returns>
        public override Expression<Func<PersonnelEnt, bool>> GetPredicateWhere()
        {
            return GetPredicateWhere(false);
        }


#pragma warning disable S3776
        /// <summary>
        /// Permet de récupérer le prédicat de recherche.
        /// </summary>
        /// <param name="withSplitFilters"> Permet d'appliquer la séparation des filtres</param>
        /// <returns>Retourne la condition de recherche</returns>
        public Expression<Func<PersonnelEnt, bool>> GetPredicateWhere(bool withSplitFilters)
        {
            DateTime now = DateTime.UtcNow.Date;

            if (withSplitFilters)
            {
                return p => (string.IsNullOrEmpty(ValueTextNom) || (Nom && p.Nom.ToLower().Contains(ValueTextNom.ToLower())))
                            && (string.IsNullOrEmpty(ValueTextPrenom) || (Prenom && p.Prenom.ToLower().Contains(ValueTextPrenom.ToLower())))
                            && (string.IsNullOrEmpty(ValueText) || ((SocieteCodeLibelle && (p.Societe.Code.ToLower().Contains(ValueText.ToLower())
                            || p.Societe.Libelle.ToLower().Contains(ValueText.ToLower())))
                            || (Matricule && p.Matricule.ToLower().Contains(ValueText.ToLower()))
                            || p.EtablissementPaie.Libelle.ToLower().Contains(ValueText.ToLower())
                            || p.EtablissementPaie.Code.ToLower().Contains(ValueText.ToLower())))
                            && (string.IsNullOrEmpty(EtablissementPaieCode) || EtablissementPaieCode.ToLower().Equals(p.EtablissementPaie.Code))
                            && (string.IsNullOrEmpty(SocieteCode) || SocieteCode.ToLower().Equals(p.Societe.Code))
                            // Critères -->
                            && (!IsInterne || p.IsInterne)
                            && (IsPersonnelsNonPointables ? p.IsPersonnelNonPointable == true : true)
                            && (!IsUtilisateur || (p.Utilisateur != null && p.Utilisateur.IsDeleted != true))
                            && (!IsInterimaire || p.IsInterimaire)
                            && (!IsActif || IsActif
                                && (
                                    (!p.DateSuppression.HasValue)
                                    && (!p.DateSortie.HasValue || p.DateSortie.Value.Date >= now)
                                    && (!p.DateEntree.HasValue || p.DateEntree.Value.Date <= now)
                                )
                            );
            }
            else
            {
                var valueText = ValueText.ToLower();
                return p => (string.IsNullOrEmpty(ValueText)
                    || Nom && p.Nom.ToLower().Contains(valueText)
                    || Prenom && p.Prenom.ToLower().Contains(valueText)
                    || SocieteCodeLibelle && (p.Societe.Code.ToLower().Contains(valueText)
                    || p.Societe.Libelle.ToLower().Contains(valueText))
                    || Matricule && p.Matricule.ToLower().Contains(valueText))

                   // Critères -->
                   && (!IsInterne || p.IsInterne)
                   && (IsPersonnelsNonPointables ? p.IsPersonnelNonPointable == true : true)
                   && (!IsUtilisateur || (p.Utilisateur != null
                   && p.Utilisateur.IsDeleted != true))
                   && (!IsInterimaire || p.IsInterimaire)
                   && (!IsActif || IsActif
                       && (
                            (!p.DateSuppression.HasValue)
                            && (!p.DateSortie.HasValue || p.DateSortie.Value.Date >= now)
                            && (!p.DateEntree.HasValue || p.DateEntree.Value.Date <= now)
                        ));
            }
#pragma warning restore S3776
        }

        /// <summary>
        ///   Permet de passer l'ordre d'un personnel par défaut.
        /// </summary>
        /// <returns>Retourne l'ordre des personnels par défaut</returns>
        protected override IOrderer<PersonnelEnt> GetDefaultOrderBy()
        {
            if (!NomPrenomAsc.HasValue)
            {
                return new Orderer<PersonnelEnt, string>(c => c.Matricule, true);
            }

            return null;
        }

        /// <summary>
        ///   Permet de passer l'ordre d'un personnel par ses valeurs.
        /// </summary>
        /// <returns>Retourne l'ordre des personnels par ses valeurs</returns>
        protected override IOrderer<PersonnelEnt> GetUserOrderBy()
        {
            if (SocieteAsc.HasValue && SocieteAsc.Value && MatriculeAsc.HasValue && MatriculeAsc.Value)
            {
                return new Orderer<PersonnelEnt, object>(new List<Expression<Func<PersonnelEnt, object>>> { c => c.Societe.Code, c => c.Societe.Libelle, c => c.Matricule }, SocieteAsc.Value);
            }

            if (NomPrenomAsc.HasValue)
            {
                return new Orderer<PersonnelEnt, string>(c => c.Nom, NomPrenomAsc.Value);
            }
            if (SocieteAsc.HasValue)
            {
                return new Orderer<PersonnelEnt, string>(c => c.Societe.CodeLibelle, SocieteAsc.Value);
            }
            if (MatriculeAsc.HasValue)
            {
                return new Orderer<PersonnelEnt, string>(c => c.Matricule, MatriculeAsc.Value);
            }

            return GetDefaultOrderBy();
        }

        #endregion
    }
}
