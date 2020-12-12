using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Fred.Framework.Extensions
{
    /// <summary>
    /// Methodes d'extension des Expression
    /// </summary>
    public static class ExpressionExtension
    {
        /// <summary>
        /// Methode d'extension pour inclure des proprietes lors d'un appel a Entity framework
        /// </summary>
        /// <typeparam name="T">Type de l'entité</typeparam>
        /// <param name="includes">Liste d'includes precedamment crée</param>
        /// <param name="include">Expression pour chargé une propriété</param>
        /// <returns>Liste d'includes</returns>
        public static List<Expression<Func<T, object>>> Include<T>(this List<Expression<Func<T, object>>> includes, Expression<Func<T, object>> include) where T : class
        {
            includes.Add(include);
            return includes;
        }

        /// <summary>
        /// Methode d'extension permettant de convertir une expression en string utilable dans un include.
        /// Source : https://stackoverflow.com/questions/47052419/how-to-pass-lambda-include-with-multiple-levels-in-entity-framework-core/47063432#47063432
        /// </summary>
        /// <typeparam name="T">Type de l'entité</typeparam>
        /// <param name="expression">Expression à convertir.</param>
        /// <returns>Expression convertie en chaine de caractères.</returns>
        public static string AsPath<T>(this Expression<Func<T, object>> expression) where T : class
        {
            if (expression == null) return null;

            var exp = expression.Body;
            string path;
            TryParsePath(exp, out path);
            return path;
        }

        // This method is a slight modification of EF6 source code
        private static bool TryParsePath(Expression expression, out string path)
        {
            path = null;
            var withoutConvert = RemoveConvert(expression);
            var memberExpression = withoutConvert as MemberExpression;
            var callExpression = withoutConvert as MethodCallExpression;

            if (memberExpression != null)
            {
                var thisPart = memberExpression.Member.Name;
                string parentPart;
                if (!TryParsePath(memberExpression.Expression, out parentPart))
                {
                    return false;
                }
                path = parentPart == null ? thisPart : (parentPart + "." + thisPart);
            }
            else if (callExpression != null)
            {
                if (callExpression.Method.Name == "Select"
                    && callExpression.Arguments.Count == 2)
                {
                    string parentPart;
                    if (!TryParsePath(callExpression.Arguments[0], out parentPart))
                    {
                        return false;
                    }
                    if (parentPart != null)
                    {
                        var subExpression = callExpression.Arguments[1] as LambdaExpression;
                        if (subExpression != null)
                        {
                            string thisPart;
                            if (!TryParsePath(subExpression.Body, out thisPart))
                            {
                                return false;
                            }
                            if (thisPart != null)
                            {
                                path = parentPart + "." + thisPart;
                                return true;
                            }
                        }
                    }
                }
                else if (callExpression.Method.Name == "Where")
                {
                    throw new NotSupportedException("Filtering an Include expression is not supported");
                }
                else if (callExpression.Method.Name == "OrderBy" || callExpression.Method.Name == "OrderByDescending")
                {
                    throw new NotSupportedException("Ordering an Include expression is not supported");
                }
                return false;
            }

            return true;
        }

        // Removes boxing
        private static Expression RemoveConvert(Expression expression)
        {
            while (expression.NodeType == ExpressionType.Convert
                   || expression.NodeType == ExpressionType.ConvertChecked)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            return expression;
        }

        /// <summary>
        /// Retourne une expression de recherche.
        /// </summary>
        /// <typeparam name="TSource">Type des éléments de la source.</typeparam>
        /// <typeparam name="TSearch">Le type qui représente les champs où rechercher.</typeparam>
        /// <param name="searchor">L'expression de recherche.</param>
        /// <param name="search">La chaîne à rechercher.</param>
        /// <returns>L'expression de la recherche.</returns>
        internal static Expression<Func<TSource, bool>> GetSearchExpression<TSource, TSearch>(this Expression<Func<TSource, TSearch>> searchor, string search)
        {
            var newExpression = searchor?.Body as NewExpression;
            if (newExpression == null)
            {
                throw new ArgumentException(nameof(searchor));
            }

            var stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var stringToLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            var stringIsNullOrEmptyMethod = typeof(string).GetMethod("IsNullOrEmpty", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);

            var searchExp = Expression.Constant(search.ToLower());
            var nullExpression = Expression.Constant(null, typeof(object));
            var sourceParamExp = Expression.Parameter(typeof(TSource), "item");

            // string.IsNullOrEmpty(search)
            Expression fullExp = Expression.Call(stringIsNullOrEmptyMethod, searchExp);

            // Parcours les arguments du searchor
            foreach (var argument in newExpression.Arguments)
            {
                // Les arguments doivent être des membres de type string
                var member = argument as MemberExpression;
                if (member == null || member.Type != typeof(string))
                {
                    throw new ArgumentException($"L'élément \"{member}\" doit être de type string", nameof(searchor));
                }

                // Traite les propriétés imbriquées (searchor = a => new { a.b.c })
                var membersName = new List<string>() { member.Member.Name };
                while (member.Expression is MemberExpression)
                {
                    member = (MemberExpression)member.Expression;
                    membersName.Add(member.Member.Name);
                }

                // Vérifie la nullité des membres parent
                // ex : searchor = a =>  new { a.b.c.d } -> a.b != null AND a.b.c != null
                // Dans cet exemple, "a" n'est pas null et "d" est le membre cible testé plus loin
                Expression memberExp = sourceParamExp;
                Expression allParentMembersNotNullExp = null;
                for (var i = membersName.Count; i-- > 1;)
                {
                    memberExp = Expression.Property(memberExp, membersName[i]);
                    var parentMemberNotNullExp = Expression.NotEqual(memberExp, nullExpression);
                    allParentMembersNotNullExp = SetOrAndAlso(allParentMembersNotNullExp, parentMemberNotNullExp);
                }

                // a.b != null AND a.b.c != null AND !string.IsNullOrEmpty(a.b.c.d)
                var targetMemberExp = Expression.Property(memberExp, membersName[0]);
                var targetMemberIsNotNullOrEmptyExp = Expression.IsFalse(Expression.Call(stringIsNullOrEmptyMethod, targetMemberExp));
                var memberSearchExp = SetOrAndAlso(allParentMembersNotNullExp, targetMemberIsNotNullOrEmptyExp);

                // a.b != null AND a.b.c != null AND !string.IsNullOrEmpty(a.b.c.d) AND a.b.c.d.ToLower().Contains(search)
                var targetMemberToLowerExp = Expression.Call(targetMemberExp, stringToLowerMethod);
                var targetMemberToLowerContainsExp = Expression.Call(targetMemberToLowerExp, stringContainsMethod, searchExp);
                memberSearchExp = Expression.AndAlso(memberSearchExp, targetMemberToLowerContainsExp);

                // Expression complète
                fullExp = Expression.OrElse(fullExp, memberSearchExp);
            }

            return Expression.Lambda<Func<TSource, bool>>(fullExp, new ParameterExpression[] { sourceParamExp });
        }

        /// <summary>
        /// return right si left est null, sinon Expression.AndAlso(left, right)
        /// </summary>
        /// <param name="left">La partie gauche du And.</param>
        /// <param name="right">La partie droite du And.</param>
        /// <returns>right si left est null, sinon Expression.AndAlso(left, right)</returns>
        private static Expression SetOrAndAlso(Expression left, Expression right)
        {
            if (left != null)
            {
                return Expression.AndAlso(left, right);
            }
            return right;
        }
    }
}
