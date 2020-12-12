using System;
using System.Linq.Expressions;
using Fred.Entities.Search;

namespace Fred.Entities.Societe
{
    /// <summary>
    ///   Représente une recherche de société
    /// </summary>
    public class SearchSocieteEnt : AbstractSearchEnt<SocieteEnt>
    {
        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche sur le code condensé
        /// </summary>
        public bool Code { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche sur le code société paye
        /// </summary>
        public bool CodeSocietePaye { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche sur le code société comptable
        /// </summary>
        public bool CodeSocieteComptable { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche sur le libellé de la société
        /// </summary>
        public bool Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche sur l'Adresse d'une société
        /// </summary>
        public bool Adresse { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche sur la ville d'une société
        /// </summary>
        public bool Ville { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche sur le code postal d'une société
        /// </summary>
        public bool CodePostal { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche sur le numéro SIRET d'une société
        /// </summary>
        public bool SIRET { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche sur la valeur indiquant si une société est externe au groupe
        ///   ou non.
        /// </summary>
        public bool Externe { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si on recherche sur une valeur indiquant si une société est active ou non.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        ///   Get predicat
        /// </summary>
        /// <returns>predicat</returns>
        public override Expression<Func<SocieteEnt, bool>> GetPredicateWhere()
        {
            string searchedText = ValueText?.Trim();
            return p => (string.IsNullOrEmpty(searchedText)
                        || Code && p.Code.Contains(searchedText)
                        || Libelle && p.Libelle.Contains(searchedText)
                        || CodeSocietePaye && p.CodeSocietePaye.Contains(searchedText)
                        || CodeSocieteComptable && p.CodeSocieteComptable.Contains(searchedText)
                        || Adresse && p.Adresse.Contains(searchedText)
                        || Ville && p.Ville.Contains(searchedText)
                        || SIRET && p.SIREN.Contains(searchedText))
                       && (!Active || p.Active)
                       && (!Externe || p.Externe);
        }

        /// <summary>
        ///   Get order default by
        /// </summary>
        /// <returns>order by</returns>
        protected override IOrderer<SocieteEnt> GetDefaultOrderBy()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Get user order by
        /// </summary>
        /// <returns>order by</returns>
        protected override IOrderer<SocieteEnt> GetUserOrderBy()
        {
            throw new NotImplementedException();
        }
    }
}
