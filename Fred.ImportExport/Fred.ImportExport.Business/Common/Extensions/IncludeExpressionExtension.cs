using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fred.ImportExport.Business.Common.Extensions
{
    public static class IncludeExpressionExtension
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
    }
}
