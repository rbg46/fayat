using Fred.Entities.ActivitySummary;

namespace Fred.Business.Email.ActivitySummary.Builders
{

    /// <summary>
    /// Extension de ColorJalon
    /// </summary>
    public static class ColorJalonExt
    {
        /// <summary>
        /// Transforme un ColorActivity en rgb
        /// </summary>
        /// <param name="colorActivity">colorActivity</param>
        /// <returns>un rgb</returns>
        public static string ToRgb(this ColorJalon colorActivity)
        {
            if (colorActivity == ColorJalon.ColorOrange)
            {
                return "rgb(255,230,153)";
            }
            if (colorActivity == ColorJalon.ColorRed)
            {
                return "rgb(254,176,132)";
            }
            if (colorActivity == ColorJalon.ColorBlue)
            {
                return "rgb(221,235,247)";
            }
            if (colorActivity == ColorJalon.ColorGreen)
            {
                return "rgb(226,239,218)";
            }
            return "rgb(255,255,255)";
        }
    }
}
