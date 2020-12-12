using Fred.Entities.ActivitySummary;

namespace Fred.Business.Email.ActivitySummary.Builders
{

    /// <summary>
    ///  Extension de ColorActivity
    /// </summary>
    public static class ColorActivityExt
    {
        /// <summary>
        /// Transforme un ColorActivity en rgb
        /// </summary>
        /// <param name="colorActivity">colorActivity</param>
        /// <returns>un rgb</returns>
        public static string ToRgb(this ColorActivity colorActivity)
        {
            if (colorActivity == ColorActivity.ColorOrange)
            {
                return "rgb(255,230,153)";
            }
            if (colorActivity == ColorActivity.ColorRed)
            {
                return "rgb(254,176,132)";
            }
            if (colorActivity == ColorActivity.ColorBlue)
            {
                return "rgb(221,235,247)";
            }
            return "rgb(255,255,255)";
        }
    }
}
