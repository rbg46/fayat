using System;
using System.Collections.Generic;

namespace Fred.Framework.Comparers
{
    /// <summary>
    ///     Compare une liste contenant des chaines de caractères dont certaines sont numériques        
    /// </summary>
    /// <example>
    ///     input = ["80", "A", "95", "K", "C", "100", "F", "00", "010", "01", "3000", "060", "ED10", "A10", "A11", "B01", "B010", "60"]
    ///     output = ["A", "A10", "A11", "B01", "B010", "C", "ED10", "F", "K", "00", "01", "010", "060", "60", "80", "95", "100", "3000"]
    /// </example>
    public class CustomAlphanumericComparer : IComparer<string>
    {
        /// <summary>
        ///     Comparaison de deux chaines
        /// </summary>
        /// <param name="x">Chaine 1</param>
        /// <param name="y">Chaine 2</param>
        /// <returns>Ordre</returns>
        public int Compare(string x, string y)
        {
            int iX, iY;
            bool isXInt = int.TryParse(x, out iX);
            bool isYInt = int.TryParse(y, out iY);

            if (isXInt)
            {
                if (isYInt)
                {
                    if (Convert.ToInt32(x) > Convert.ToInt32(y))
                    {
                        return 1;
                    }

                    if (Convert.ToInt32(x) < Convert.ToInt32(y))
                    {
                        return -1;
                    }

                    if (Convert.ToInt32(x) == Convert.ToInt32(y))
                    {
                        return 0;
                    }
                }
                else
                {
                    return 1;
                }
            }
            else if (isYInt)
            {
                return -1;
            }

            return string.Compare(x, y, true);
        }
    }
}
