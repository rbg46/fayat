using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.RapportHebdo
{
    /// <summary>
    /// Représente des rapports où un personnel est pointé.
    /// </summary>
    public class PersonnelRapports
    {
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel.</param>
        public PersonnelRapports(int personnelId)
        {
            PersonnelId = personnelId;
            Rapports = new List<RapportEnt>();
        }

        /// <summary>
        /// Identifiant du personnel.
        /// </summary>
        public int PersonnelId { get; private set; }

        /// <summary>
        /// Les rapports où le personnel est pointé.
        /// </summary>
        public List<RapportEnt> Rapports { get; private set; }
    }
}
