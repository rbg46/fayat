using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Framework.Tool
{
    /// <summary>
    /// Classe gérant les outils générique sur les strings
    /// </summary>
    public static class StringTool
    {
        /// <summary>
        /// Itération sur toutes les propriétés de type string afin de supprimer d'éventuelles espaces à la fin des chaînes.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entité</param>
        public static void StringPropertiesCleaner<T>(this T entity)  where T : class
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string) && property.CanWrite)
                {
                    string value = (string)property.GetValue(entity, null);
                    property.SetValue(entity, value?.TrimEnd());
                }
            }
        }

        /// <summary>
        /// Enlever les accents et les caractères spéciaux d'une chaine de caractère
        /// </summary>
        /// <param name="text">Chaine à traiter</param>
        /// <returns>Chaine sans accents et caractères spéciaux</returns>
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
