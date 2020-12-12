using System.Collections.Generic;
using Fred.Entities.Affectation;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;

namespace Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel
{
    public class LigneAstreinteSelectModel
    {
        /// <summary>
        /// Obtient ou définit l'entité Astreinte
        /// </summary>
        public string AstreinteCode { get; set; }

        /// <summary>
        ///  Obtient ou définit la list des codes primes astreintes
        /// </summary>
        public double? HeureAstreinte { get; set; }

        /// <summary>
        /// Obtient ou définit la list des rapport ligne code astreinte
        /// </summary>
        public virtual ICollection<RapportLigneCodeAstreinteEnt> ListCodePrimeSortiesAstreintes { get; set; }
    }
}
