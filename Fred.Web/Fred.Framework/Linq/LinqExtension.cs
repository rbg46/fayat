using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Framework.Linq
{
    /// <summary>
    /// Extension linq
    /// </summary>
    public static class LinqExtension
    {
        /// <summary>
        /// Creer un batch
        /// </summary>
        /// <typeparam name="T">type d'element</typeparam>
        /// <param name="items">les element</param>
        /// <param name="maxItems">nombre maximal du batch</param>
        /// <returns> une liste decoupé par batch</returns>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items,
                                                           int maxItems)
        {
            return items.Select((item, inx) => new { item, inx })
                        .GroupBy(x => x.inx / maxItems)
                        .Select(g => g.Select(x => x.item));
        }

        /// <summary>
        /// Execute une action sur une liste d'element
        /// </summary>
        /// <typeparam name="T">type d'element</typeparam>
        /// <param name="list">liste d'element</param>
        /// <param name="action">action a executer</param>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (T item in list)
            {
                action(item);
            }
        }
    }
}
