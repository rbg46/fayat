using System;
using System.Text.RegularExpressions;

namespace Fred.Framework.Extensions
{
    /// <summary>
    ///   Class d'extension pour les strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///   Methode permettant de détecter si une chaîne contient des caractères spéciaux
        /// </summary>
        /// <param name="str">Chaîne à vérifier</param>
        /// <returns>True si il y a des caractères spéciaux</returns>
        public static bool ContainsSpecialChar(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            Regex regexItem = new Regex("^[À-ž '&._-]*$");
            return regexItem.IsMatch(str);
        }

        /// <summary>
        /// Méthode permettant de mettre la première lettre d'une chaine de caractère en majuscule
        /// </summary>
        /// <param name="str">Chaîne de caractère</param>
        /// <returns>La chaîne de caractère avec la 1er letter en majuscule</returns>
        public static string ToUpperFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            char[] a = str.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }


        /// <summary>
        /// Determine si une chaine est null ou vide
        /// </summary>
        /// <param name="text">la chaine</param>
        /// <returns>True si la chaine est nulle ou vide</returns>
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        /// Permet de faire "chaine".IsNullOrWhiteSpace(), plutôt que String.IsNullOrWhiteSpace("chaine")
        /// </summary>
        /// <param name="str">chaine à vérifier</param>
        /// <returns>true if the str parameter is null or System.String.Empty, or if str consists exclusively of white-space characters</returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Tronque une chaine de charactere
        /// </summary>
        /// <param name="value">La chaine de charactere</param>
        /// <param name="maxLength">la longueur maximale</param>
        /// <returns>La chaione ou la chaine tronquée</returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }


        /// <summary>
        /// Retourne les n premiers caractères de str
        /// </summary>
        /// <param name="str">chaine</param>
        /// <param name="length">nombre de caractères à retourner depuis le début</param>
        /// <returns>les n premiers caractères de str</returns>
        public static string Left(this string str, int length)
        {
            return str?.Substring(0, Math.Min(length, str.Length));
        }



        /// <summary>
        /// Retourne les n derniers caractères de str
        /// </summary>
        /// <param name="str">chaine</param>
        /// <param name="length">nombre de caractères à retourner depuis la fin</param>
        /// <returns>les n derniers caractères de str</returns>
        public static string Right(this string str, int length)
        {
            return str?.Substring(str.Length - Math.Min(length, str.Length));
        }
    }
}
