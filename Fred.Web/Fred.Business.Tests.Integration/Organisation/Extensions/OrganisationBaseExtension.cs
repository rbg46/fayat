using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities;
using Fred.Entities.Organisation.Tree;

namespace Fred.Business.Tests.Integration.Organisation.Extensions
{
    public static class OrganisationBaseExtension
    {
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public static string ToStringTest(this OrganisationBase orgBase)
        {
            return $"{Truncate(orgBase.Libelle, 20).PadRight(25)} - OrganisationId = {orgBase.OrganisationId} PereId = {orgBase.PereId} {((OrganisationType)orgBase.TypeOrganisationId).ToFriendlyId() ?? "Id"} = {orgBase.Id}";
        }


        /// <summary>
        /// Tronquer une chaîne
        /// </summary>
        /// <param name="value">La chaine concernée</param>
        /// <param name="maxChars">Max nombre de caractères</param>
        /// <returns>Caîne tronquée</returns>
        public static string Truncate(string value, int maxChars)
        {
            if (value == null)
                return "";
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}
