using Fred.Entities.ActivitySummary;

namespace Fred.Business.Email.ActivitySummary.Builders
{
    /// <summary>
    /// Determine la couleur d'une cellule pour une valeur donnée
    /// </summary>
    public class UserActivityColorProvider
    {

        /// <summary>
        /// Retourne la couleur du Jalon en fonction de la date courrante passée en parametre
        /// Si la valeur d’une case du tableau est =0, la laisser vide(ne pas afficher « 0 »).
        /// Si la valeur est >0 et inferieur à 5, afficher la valeur et laisser le fond de la case en blanc.
        /// Si la valeur est >= 5 et inferieur à 10, afficher la valeur et mettre le fond de la case en orange.
        /// Si la valeur est >=10, afficher la valeur et mettre le fond de la case en rouge.
        /// </summary>      
        /// <param name="value">Valeur dont on cherche la couleur correspondante</param>
        /// <returns>La couleur</returns>
        public ColorActivity GetActivityColor(int? value)
        {
            if (!value.HasValue)
            {
                return ColorActivity.ColorWhite;
            }

            if (value >= 10)
            {
                return ColorActivity.ColorRed;
            }

            if (value >= 5 && value < 10)
            {
                return ColorActivity.ColorOrange;
            }

            if (value > 0 && value < 5)
            {
                return ColorActivity.ColorWhite;
            }

            return ColorActivity.ColorBlue;
        }

    }
}
