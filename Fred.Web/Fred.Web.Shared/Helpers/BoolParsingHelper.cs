using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Helpers
{
    public class BoolParsingHelper
    {
        static readonly HashSet<string> booleanTrueStrings = new HashSet<string> { "ENABLED", "TRUE", "VRAI", "YES", "1", "OUI" };
        static readonly HashSet<string> booleanFalseStrings = new HashSet<string> { "DISABLED", "FALSE", "FAUX", "NO", "0", "NON" };
        
        public bool LooseParse(string value)
        {
            value = value.Trim();
            value = value.ToUpperInvariant();
            if (booleanTrueStrings.Contains(value)) return true;
            if (booleanFalseStrings.Contains(value)) return false;

            throw new ArgumentException("Unexpected Boolean Format");
        }
    }
}
