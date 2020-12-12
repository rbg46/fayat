using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.VerificationPointage.Model
{
    /// <summary>
    /// Modelde groupage des donner de verification pointage (Personnel or Materiel)  -->CI--->Etat Materiel
    /// </summary>
    public class ChekingPointingGroup
    {
        /// <summary>
        /// Modelde groupage des donner de verification pointage (Personnel or Materiel)  -->CI--->Etat Materiel
        /// </summary>
        /// 
        public List<ChekingPointingGroup> Lignepointing { get; set; }

        /// <summary>
        /// information sur le donnée à afficher
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// nombre des heures travaillés par jours de 1 à 31
        /// </summary>
        public Dictionary<int, double> DayWorks { get; set; }
    }
}
