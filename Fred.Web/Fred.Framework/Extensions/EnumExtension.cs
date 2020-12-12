using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Fred.Framework.Extensions
{
    /// <summary>
    ///   Permet d'afficher certaines informations pour une énumération
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        ///   Convert the value of an enum to a string
        /// </summary>
        /// <example>enum.element=1 => return "1";</example>
        /// <param name="value">énumération</param>
        /// <returns>valeur de l'enum en chaine</returns>
        public static string ToStringValue(this Enum value)
        {
            return ToIntValue(value).ToString();
        }


        /// <summary>
        ///   Return the int value of an enum
        /// </summary>
        /// <example>enum.element=1 => return 1;</example>
        /// <param name="value">enumération</param>
        /// <returns>valeur entière de l'enum</returns>
        public static int ToIntValue(this Enum value)
        {
            object item = Enum.Parse(value.GetType(), value.ToString());
            return (int)item;
        }


        /// <summary>
        ///   Convert an int to an enum.
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>enumération de type T</returns>
        public static T ToEnum<T>(this int enumValue)
        {
            T value = (T)Enum.ToObject(typeof(T), enumValue);
            return value;
        }

        /// <summary>
        ///   Convert an short to an enum.
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>énumération de type T</returns>
        public static T ToEnum<T>(this short enumValue)
        {
            T value = (T)Enum.ToObject(typeof(T), enumValue);
            return value;
        }

        /// <summary>
        ///   For each.
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException">soure is null</exception>
        /// <exception cref="System.ArgumentNullException">action is null</exception>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        ///   Parses a string to enum.
        /// </summary>
        /// <typeparam name="T">type of the enum</typeparam>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentException">T must be an enumerated type</exception>
        /// <returns>enumération</returns>
        public static T ParseEnum<T>(this string value) where T : IComparable, IFormattable
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        ///   Parses the enum.
        /// </summary>
        /// <typeparam name="T">type of the enum</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <exception cref="ArgumentException">T must be an enumerated type</exception>
        /// <returns>If foudn, return the string value converted to enum, else return the default value </returns>
        public static T ParseEnum<T>(this string value, T defaultValue) where T : IComparable, IFormattable
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (item.ToString().Equals(value.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return defaultValue;
        }

        /// <summary>
        ///   Retourne s'il existe, l'attribut <see cref="DisplayAttribute" /> d'une enumération
        /// </summary>
        /// <param name="value">enum with DisplayAttribute</param>
        /// <returns>
        ///   DisplayAttribute or null
        /// </returns>
        public static DisplayAttribute GetDisplayAttribute(Enum value)
        {
            DisplayAttribute attribut = value
              .GetType()
              .GetTypeInfo()
              .GetDeclaredField(value.ToString())
              .GetCustomAttribute<DisplayAttribute>();

            return attribut;
        }

        /// <summary>
        ///   Permet de récupérer la chaine du champ Description de l'attribut <see cref="DisplayAttribute" /> d'une énumération
        /// </summary>
        /// <param name="value">enum with DisplayAttribute</param>
        /// <returns>
        ///   string
        /// </returns>
        public static string GetDisplayDescription(this Enum value)
        {
            try
            {
                DisplayAttribute attribut = GetDisplayAttribute(value);
                return attribut.Description;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///   Permet de récupérer la chaine du champ Name de l'attribut <see cref="DisplayAttribute" /> d'une énumération
        /// </summary>
        /// <param name="value">enum with DisplayAttribute</param>
        /// <returns>
        ///   string
        /// </returns>
        public static string GetDisplayName(this Enum value)
        {
            try
            {
                DisplayAttribute attribut = GetDisplayAttribute(value);
                return attribut.Name;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
