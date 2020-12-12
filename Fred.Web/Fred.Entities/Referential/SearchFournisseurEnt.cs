using Fred.Entities.Search;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Fred.Entities.Constantes;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Représente une recherche de Fournisseur
    /// </summary>
    [Serializable]
    public class SearchFournisseurEnt : AbstractSearchEnt<FournisseurEnt>
    {
        #region Critères

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche un code déplacement actif ou nonSIREN.
        /// </summary>
        public string SIREN { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche une Ville.
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche un Departement.
        /// </summary>
        public string Departement { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche un fournisseur locatier.
        /// </summary>
        public bool Locatier { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche un fournisseur ETT.
        /// </summary>
        public bool ETT { get; set; }

        #endregion

        #region Tri 
        /// <summary>
        ///   Permet de récupérer un tri par code par défaut.
        /// </summary>
        /// <returns>Retourne un odre de dépense par défaut</returns>
        protected override IOrderer<FournisseurEnt> GetDefaultOrderBy()
        {
            return new Orderer<FournisseurEnt, object>(new List<Expression<Func<FournisseurEnt, object>>> { f => f.Code, f => f.Libelle }, true);
        }
        #endregion

        #region Génération de prédicat de recherche

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche des fournisseurs.
        /// </summary>
        /// <returns>Retourne la condition de recherche des fournisseurs</returns>
#pragma warning disable S3776
        public override Expression<Func<FournisseurEnt, bool>> GetPredicateWhere()
        {
            if (string.IsNullOrEmpty(ValueText))
            {
                return p =>
                          (ETT && p.TypeTiers == TypeFournisseur.ETT
                          || Locatier && p.TypeTiers == TypeFournisseur.Locatier
                          || ETT && Locatier && (p.TypeTiers == TypeFournisseur.ETT || p.TypeTiers == TypeFournisseur.Locatier)
                          || !ETT && !Locatier)
                          && (string.IsNullOrEmpty(SIREN) || p.SIREN.ToLower().Contains(SIREN.ToLower()))
                          && (string.IsNullOrEmpty(Ville) || p.Ville.ToLower().Contains(Ville.ToLower()))
                          && (string.IsNullOrEmpty(Departement) || p.CodePostal.Substring(0, 2).Equals(Departement) || p.CodePostal.Substring(0, 3).Equals(Departement));
            }
            return p => (p.Code.ToLower().Contains(ValueText.ToLower()) || p.Libelle.ToLower().Contains(ValueText.ToLower()))
                       /* Critères */
                       && (ETT && p.TypeTiers == TypeFournisseur.ETT
                        || Locatier && p.TypeTiers == TypeFournisseur.Locatier
                        || ETT && Locatier && (p.TypeTiers == TypeFournisseur.ETT || p.TypeTiers == TypeFournisseur.Locatier)
                        || !ETT && !Locatier)
                       && (string.IsNullOrEmpty(SIREN) || p.SIREN.ToLower().Contains(SIREN.ToLower()))
                       && (string.IsNullOrEmpty(Ville) || p.Ville.ToLower().Contains(Ville.ToLower()))
                       && (string.IsNullOrEmpty(Departement) || p.CodePostal.Substring(0, 2).Equals(Departement) || p.CodePostal.Substring(0, 3).Equals(Departement));
        }
#pragma warning restore S3776
        /// <summary>
        /// Get user orderBy
        /// </summary>
        /// <returns>Orderby user</returns>
        protected override IOrderer<FournisseurEnt> GetUserOrderBy()
        {
            return GetDefaultOrderBy();
        }

        #endregion
    }
}
