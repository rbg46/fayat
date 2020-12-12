using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Permet de  garder les Affectation dejà traitees.
    /// </summary>
   public class AffectationMoyenRapportModel
    {
        /// <summary>
        /// Identifiant de l affectation moyen
        /// </summary>
        public int AffectationMoyenId { get; set; }

        /// <summary>
        /// Date de pointage
        /// </summary>
        public DateTime DatePointage { get; set; }

        /// <summary>
        /// CI Id
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// L'id Materiel
        /// </summary>
        public int MaterielId { get; set; }

        public bool IsExist(List<AffectationMoyenRapportModel> affectationMoyenRapportModel)
        {
            return affectationMoyenRapportModel.Any(x => x.AffectationMoyenId == this.AffectationMoyenId 
                                && x.DatePointage == this.DatePointage && x.CiId == this.CiId && x.MaterielId == this.MaterielId);  
        }
    }
}
