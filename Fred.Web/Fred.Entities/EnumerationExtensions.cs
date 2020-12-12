using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Fred.Entities
{
    /// <summary>
    /// Extensions sur les énumérations
    /// https://www.codementor.io/cerkit/giving-an-enum-a-string-value-using-the-description-attribute-6b4fwdle0
    /// </summary>
    public static class EnumerationExtensions
    {
        /// <summary>
        /// Récupérer la description liée à un enum
        /// </summary>
        /// <typeparam name="T">Type de l'enum</typeparam>
        /// <param name="e">Valeur de l'enum</param>
        /// <returns>Description de l'enum</returns>
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }
            return null;
        }
    }
}
