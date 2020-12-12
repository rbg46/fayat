using System;
using Fred.Entities.ActivitySummary;

namespace Fred.Business.Email.ActivitySummary.Builders
{
    /// <summary>
    /// Fournie la couleur des jalons
    /// </summary>
    public class JalonColorProvider
    {

        /// <summary>
        /// Retourne la couleur du Jalon en fonction de la date courrante passée en parametre
        /// M est la valeur du mois en cours, N est la valeur du mois à afficher dans le tableau :
        ///        Si N >= M – 1, mettre le fond de la case en vert.
        ///        Si N = M – 2, mettre le fond de la case en orange.
        ///        Si N inferieur a M - 2, mettre le fond de la case en rouge.
        /// Exemple : on est le 11 janvier 2019 (M). Toutes les cases dont la valeur (N) est égale à « Décembre 2018 » ou « Janvier 2019 » sont en vert,
        /// toutes les cases dont la valeur est égale à « Novembre 2018 » sont en orange,
        /// et toutes les cases dont la valeur est inférieure ou égale à « Octobre 2018 » sont en rouge.
        /// </summary>
        /// <param name="now">date courante</param>
        /// <param name="dateN">Date pour laquelle on cherche la couleur </param>
        /// <returns>La couleur</returns>
        public ColorJalon GetJalonColor(DateTime now, DateTime? dateN)
        {
            if (!dateN.HasValue)
            {
                return ColorJalon.ColorBlue;
            }

            var dateToCheck = dateN.Value;

            var firstDayOfdateToCheck = new DateTime(dateToCheck.Year, dateToCheck.Month, 1);

            var unMoisAvant = new DateTime(now.Year, now.Month, 1).AddMonths(-1);

            var deuxMoisAvant = new DateTime(now.Year, now.Month, 1).AddMonths(-2);

            if (dateToCheck >= unMoisAvant)
            {
                return ColorJalon.ColorGreen;
            }
            if (firstDayOfdateToCheck == deuxMoisAvant)
            {
                return ColorJalon.ColorOrange;
            }
            if (firstDayOfdateToCheck < deuxMoisAvant)
            {
                return ColorJalon.ColorRed;
            }
            return ColorJalon.ColorBlue;
        }
    }

}
