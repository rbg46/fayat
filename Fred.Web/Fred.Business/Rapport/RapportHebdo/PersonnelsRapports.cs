using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Rapport;
using MoreLinq;

namespace Fred.Business.Rapport.RapportHebdo
{
    /// <summary>
    /// Représente des rapports où des personnels sont pointés.
    /// </summary>
    public class PersonnelsRapports : IEnumerable<PersonnelRapports>
    {
        private readonly List<PersonnelRapports> personnelsRapports = new List<PersonnelRapports>();

        /// <summary>
        /// Ajoute un rapport où le personnel indiqué est pointé.
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel.</param>
        /// <param name="rapport">Le rapport où le personnel est pointé.</param>
        public void Add(int personnelId, RapportEnt rapport)
        {
            PersonnelRapports personnelRapports = GetPersonnelRapports(personnelId);
            if (!personnelRapports.Rapports.Any(r => r.CiId == rapport.CiId && r.DateChantier == rapport.DateChantier))
            {
                personnelRapports.Rapports.Add(rapport);
            }
        }

        /// <summary>
        /// Ajoute des rapports où le personnel indiqué est pointé.
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel.</param>
        /// <param name="rapports">Les rapports où le personnel est pointé.</param>
        public void Add(int personnelId, IEnumerable<RapportEnt> rapports)
        {
            var personnelRapports = GetPersonnelRapports(personnelId);
            foreach (var rapport in rapports)
            {
                if (!personnelRapports.Rapports.Any(r => r.CiId == rapport.CiId && r.DateChantier == rapport.DateChantier))
                {
                    personnelRapports.Rapports.Add(rapport);
                }
            }
        }

        /// <summary>
        /// Retourne les rapports utilisés.
        /// </summary>
        /// <returns>Les rapports utilisés.</returns>
        public IEnumerable<RapportEnt> GetUsedRapports()
        {
            return personnelsRapports
                .SelectMany(pr => pr.Rapports)
                .DistinctBy(r => new { r.CiId, r.DateChantier });
        }

        /// <inheritdoc/>
        public IEnumerator<PersonnelRapports> GetEnumerator()
        {
            return personnelsRapports.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return personnelsRapports.GetEnumerator();
        }

        /// <summary>
        /// Retourne les rapports où le personnel indiqué est pointé.
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel.</param>
        /// <returns>Les rapports où le personnel indiqué est pointé.</returns>
        private PersonnelRapports GetPersonnelRapports(int personnelId)
        {
            var ret = personnelsRapports.FirstOrDefault(pr => pr.PersonnelId == personnelId);
            if (ret == null)
            {
                ret = new PersonnelRapports(personnelId);
                personnelsRapports.Add(ret);
            }
            return ret;
        }
    }
}
