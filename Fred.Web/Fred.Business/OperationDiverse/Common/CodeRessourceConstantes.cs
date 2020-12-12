using System.Collections.Generic;

namespace Fred.Business.OperationDiverse.Common
{
    public static class CodeRessourceConstantes
    {
        public const string CodeRessourcePerso = "OD-PERSO-99";

        public const string CodeRessourceMat = "OD-MAT-99";

        public const string CodeRessourceAch = "OD-ACH-99";

        public const string CodeRessourceStt = "OD-STT-99";

        public const string CodeRessourcePrest = "OD-PREST-99";

        public const string CodeRessourceDivers = "OD-DIVERS-99";

        public const string CodeRessourceProv = "OD-PROV-99";

        public const string CodeSousChapitrePerso = "12-999";

        public const string CodeSousChapitreMat = "20-999";

        public const string CodeSousChapitreAch = "30-999";

        public const string CodeSousChapitreStt = "40-999";

        public const string CodeSousChapitrePrest = "50-999";

        public const string CodeSousChapitreDivers = "60-999";

        public const string CodeSousChapitreProv = "70-999";

        public static List<string> CodesSousChapitre = new List<string> { CodeSousChapitrePerso, CodeSousChapitreMat, CodeSousChapitreAch, CodeSousChapitreStt, CodeSousChapitrePrest, CodeSousChapitreDivers, CodeSousChapitreProv };
    }
}
