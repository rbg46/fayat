using System;
using Fred.Framework.Extensions;

namespace Fred.Common.Tests.Helper
{
    /// <summary>
    /// Helper sur les guid
    /// </summary>
    public static class GuidEx
    {

        /// <summary>
        /// renvoie un vrai guid encodé en base 64, donc plus court : taille max = 22
        /// </summary>
        /// <returns>base 64 guid</returns>
        public static string CreateBase64Guid()
        {
            return Guid.NewGuid().ToBase64();
        }

        public static string CreateBase64Guid(int lenght)
        {
            return CreateBase64Guid().Left(lenght);
        }

        /// <summary>
        /// renvoie un vrai guid encodé en base 64, donc plus court : taille max = 22
        /// </summary>
        /// <param name="guid">un guid</param>
        /// <returns>base 64 guid</returns>
        public static string ToBase64(this Guid guid)
        {
            byte[] id = guid.ToByteArray();
            string id64 = Convert.ToBase64String(id).Replace("=", "");
            return id64;
        }


        /// <summary>
        /// Convertit un guid en base 64 vers un guid normal
        /// </summary>
        /// <param name="base64Guid">un guid en base 64 sans les == à la fin</param>
        /// <returns>résultat de la conversion</returns>
        public static Guid FromBase64(string base64Guid)
        {
            byte[] buffer = Convert.FromBase64String(base64Guid + "==");
            return new Guid(buffer);
        }

    }



}
