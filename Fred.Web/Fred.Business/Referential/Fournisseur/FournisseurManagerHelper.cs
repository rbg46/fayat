using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Classe Helper fournisseur
    /// </summary>
    public static class FournisseurManagerHelper
    {
        private static readonly string Joker = "*";

        /// <summary>
        ///   Récupération requête final searchlight fournisseur
        /// </summary>
        /// <param name="query">Requête de base</param>    
        /// <param name="recherche">Texte recherché avec Recherche</param>
        /// <param name="recherche2">Texte recherché avec Recherche2</param>
        /// <returns>Liste fournisseur</returns>
        public static IEnumerable<FournisseurEnt> GetFiltered(IRepositoryQuery<FournisseurEnt> query, string recherche, string recherche2)
        {
            Expression<Func<FournisseurEnt, bool>> exprRecherche = BuildExpressionRecherche(recherche);
            Expression<Func<FournisseurEnt, bool>> exprRecherche2 = BuildExpressionRecherche2(recherche2);

            IEnumerable<FournisseurEnt> q = query.OrderBy(GetOrder).Get();

            if (!string.IsNullOrEmpty(recherche) && !string.IsNullOrEmpty(recherche2))
            {
                return q.Where(exprRecherche.Compile()).Where(exprRecherche2.Compile());
            }
            else if (!string.IsNullOrEmpty(recherche) && string.IsNullOrEmpty(recherche2))
            {
                return q.Where(exprRecherche.Compile());
            }
            else if (string.IsNullOrEmpty(recherche) && !string.IsNullOrEmpty(recherche2))
            {
                return q.Where(exprRecherche2.Compile());
            }

            return q;
        }

        /// <summary>
        ///   Construis le prédicat where de la recherche 1 basé sur le libellé du fournisseur
        /// </summary>
        /// <param name="recherche">Chaine de caractère</param>
        /// <example>
        /// - "sas" ou "sas*" => recherche sur "sas..." (le fait de rajouter une étoile à la fin ne fait pas de différence, c'est comme si elle était toujours là)
        /// - "*telephonie" => recherche sur "...telephonie..."
        /// - "sas*julien" => recherche sur "sas...julien..."    
        /// </example>
        /// <returns>Expression where</returns>
        private static Expression<Func<FournisseurEnt, bool>> BuildExpressionRecherche(string recherche)
        {
            Expression<Func<FournisseurEnt, bool>> exprRecherche = null;
            List<string> words = GetWords(recherche);
            if (words.Count > 0)
            {
                exprRecherche = !recherche.StartsWith(Joker) ? GetStartWithPredicat(words[0]) : GetPredicatRecherche(words[0]);
                ParameterExpression p = exprRecherche.Parameters.Single();

                foreach (var word in words)
                {
                    exprRecherche = Expression.Lambda<Func<FournisseurEnt, bool>>(Expression.And(exprRecherche.Body, Expression.Invoke(GetPredicatRecherche(word), p)), p);
                }
            }
            return exprRecherche;
        }

        /// <summary>
        ///   Construis le prédicat where de la recherche 2 (Code, Adresse, SIREN)
        /// </summary>
        /// <param name="recherche2">Chaine de caractères 2</param>
        /// <example>
        /// - "paris", "*paris" ou "paris*" => recherche sur "...paris..." (les étoiles au début ou à la fin ne font pas de différence)
        /// - "av*elysees" => recherche sur "...av...elysees..."
        /// </example>
        /// <returns>Expression where</returns>
        private static Expression<Func<FournisseurEnt, bool>> BuildExpressionRecherche2(string recherche2)
        {
            Expression<Func<FournisseurEnt, bool>> exprRecherche2 = null;
            List<string> words = GetWords(recherche2);

            exprRecherche2 = GetPredicatRecherche2(words[0]);
            ParameterExpression p1 = exprRecherche2.Parameters.Single();

            foreach (var word in words)
            {
                exprRecherche2 = Expression.Lambda<Func<FournisseurEnt, bool>>(Expression.And(exprRecherche2.Body, Expression.Invoke(GetPredicatRecherche2(word), p1)), p1);
            }

            return exprRecherche2;
        }

        private static List<string> GetWords(string text)
        {
            List<string> words = new List<string>();

            if (text.Contains(Joker))
            {
                words = text.Split(Convert.ToChar(Joker)).ToList();
            }
            else
            {
                words.Add(text);
            }
            return words;
        }

        private static IOrderedQueryable<FournisseurEnt> GetOrder(IQueryable<FournisseurEnt> o) => o.OrderBy(f => f.TypeSequence == "TIERS" ? 1 : f.TypeSequence == "TIERS2" ? 2 : f.TypeSequence == "GROUPE" ? 3 : 4).ThenBy(f => f.Code);

        private static Expression<Func<FournisseurEnt, bool>> GetPredicatRecherche(string word) => x => x.Libelle.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0;

        private static Expression<Func<FournisseurEnt, bool>> GetPredicatRecherche2(string word) =>
          x => (!string.IsNullOrEmpty(x.Code) && x.Code.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0)
               || (!string.IsNullOrEmpty(x.Adresse) && x.Adresse.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0)
               || (!string.IsNullOrEmpty(x.Ville) && x.Ville.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0)
               || (!string.IsNullOrEmpty(x.CodePostal) && x.CodePostal.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0)
               || (x.PaysId.HasValue && x.Pays.Code.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0)
               || (!string.IsNullOrEmpty(x.SIREN) && x.SIREN.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0);

        private static Expression<Func<FournisseurEnt, bool>> GetStartWithPredicat(string word) => x => x.Libelle.StartsWith(word, StringComparison.CurrentCultureIgnoreCase);
    }
}
