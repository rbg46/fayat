using System;

namespace Fred.Business.Moyen.Common
{
    /// <summary>
    /// Modle class pour extraire les heures pointés d'un personnel
    /// </summary>
    public class PersonnelPointage
    {
        /// <summary>
        /// Personnel id
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Date de pointage
        /// </summary>
        public DateTime DatePointage { get; set; }

        /// <summary>
        /// Heures d'astreinte pointés
        /// </summary>
        public double TotalHeuresAstreintes { get; set; }

        /// <summary>
        /// La somme des heures des taches pointées
        /// </summary>
        public double TotalHeuresTaches { get; set; }

        /// <summary>
        /// La somme des heure de majoration pointées
        /// </summary>
        public double TotalHeuresMajorations { get; set; }

        /// <summary>
        /// La somme des heures pointées
        /// </summary>
        public double TotalHeures
        {
            get
            {
                return TotalHeuresAstreintes + TotalHeuresTaches + TotalHeuresMajorations;
            }
        }
    }
}
