using Fred.Framework.Exceptions;

namespace Fred.Business.Budget.Helpers.Extensions
{
    public static class AxeTypeExtension
    {
        /// <summary>
        /// Retourne 
        /// </summary>
        /// <param name="axe"></param>
        /// <returns></returns>
        public static string ToFriendlyName(this AxeTypes axe)
        {
            switch (axe)
            {
                case AxeTypes.T1:
                    return "T1";
                case AxeTypes.T2:
                    return "T2";
                case AxeTypes.T3:
                    return "T3";
                case AxeTypes.Chapitre:
                    return "R1";
                case AxeTypes.SousChapitre:
                    return "R2";
                case AxeTypes.Ressource:
                    return "R3";
                default:
                    throw new FredTechnicalException("Type d'axe inconnu " + axe.ToString());

            }
        }
    }
}
