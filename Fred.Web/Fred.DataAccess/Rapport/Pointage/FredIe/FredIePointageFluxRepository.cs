using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Fred.DataAccess.Rapport.Pointage.FredIe
{
    /// <summary>
    ///  Repo FredWeb pour la gestion des exports de pointages de fred ie
    /// </summary>
    public class FredIePointageFluxRepository : IFredIePointageFluxRepository
    {
        private readonly FredDbContext fredDbContext;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>       
        public FredIePointageFluxRepository(IUnitOfWork unitOfWork)
        {

            this.fredDbContext = (unitOfWork as UnitOfWork).Context;
            //// this.fredDbContext.Database.Log = s => Debug.WriteLine(s) 
        }

        /// <summary>
        /// Permet de récupérer toutes les lignes de pointage d'un rapport avec les informations de personnel.
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>List de RapportLigneEnt</returns>
        public List<RapportLigneEnt> GetAllPointagesForPersonnelSap(int rapportId)
        {
            return fredDbContext.RapportLignes
                        .Include(x => x.Ci)
                        .Include(x => x.Rapport)
                        .Include(x => x.Personnel)
                        .Include(x => x.ListRapportLignePrimes).ThenInclude(p => p.Prime)
                        .Include(x => x.ListRapportLigneAstreintes)
                        .Include(x => x.ListRapportLigneTaches).ThenInclude(t => t.Tache)
                        .Include(x => x.CodeDeplacement)
                        .Include(x => x.CodeMajoration)
                        .Include(x => x.CodeAbsence)
                        .Where(x => x.RapportId == rapportId)
                        .AsNoTracking()
                        .ToList();
        }

        /// <summary>
        /// Récupération de liste des rapports en fonction d'une liste d'identifiants de rapport
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapport</param>
        /// <returns>Liste de rapport</returns>
        public List<RapportEnt> GetRapportList(IEnumerable<int> rapportIds)
        {
            return fredDbContext.Rapports.Where(x => rapportIds.Contains(x.RapportId)).AsNoTracking().ToList();
        }

        /// <summary>
        /// Recupere un rapport par Id
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>Un rapport</returns>
        public RapportEnt FindByRapportId(int rapportId)
        {
            return fredDbContext.Rapports.Find(rapportId);
        }

    }
}
