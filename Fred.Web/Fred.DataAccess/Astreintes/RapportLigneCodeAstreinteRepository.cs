using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;

namespace Fred.DataAccess.Astreintes
{
    /// <summary>
    /// Rapport Ligne Code Astreinte Repository  Class
    /// </summary>
    public class RapportLigneCodeAstreinteRepository : FredRepository<RapportLigneCodeAstreinteEnt>, IRapportLigneCodeAstreinteRepository
    {
        public RapportLigneCodeAstreinteRepository(FredDbContext context)
              : base(context)
        {
        }

        /// <summary>
        /// Ajouter rapport ligne code astreinte
        /// </summary>
        /// <param name="rapportLigneId">rapportLigneId</param>
        /// <param name="codeAstreinteId">codeAstreinteId</param>
        public void AddRapportLigneAstreintes(int rapportLigneId, int codeAstreinteId)
        {
            RapportLigneCodeAstreinteEnt rapportLigneAstreinte = new RapportLigneCodeAstreinteEnt
            {
                RapportLigneId = rapportLigneId,
                CodeAstreinteId = codeAstreinteId
            };

            Context.RapportLigneCodeAstreintes.Add(rapportLigneAstreinte);
            Context.SaveChanges();
        }

        /// <summary>
        /// Ajouter rapport ligne code astreinte
        /// </summary>
        /// <param name="rapportLigneId">rapportLigneId</param>
        /// <param name="codeAstreinteId">codeAstreinteId</param>
        /// <param name="rapportLigneAstreinteId">Rapport ligne astreinte id</param>
        /// <param name="isPrimeNuit">True if is prime pour heures nuit</param>
        /// <returns>Rapport ligne code astreinre</returns>
        public RapportLigneCodeAstreinteEnt AddRapportLigneAstreintes(int rapportLigneId, int codeAstreinteId, int rapportLigneAstreinteId, bool isPrimeNuit)
        {
            RapportLigneCodeAstreinteEnt rapportLigneAstreinte = new RapportLigneCodeAstreinteEnt
            {
                RapportLigneId = rapportLigneId,
                CodeAstreinteId = codeAstreinteId,
                RapportLigneAstreinteId = rapportLigneAstreinteId,
                IsPrimeNuit = isPrimeNuit
            };

            rapportLigneAstreinte = Context.RapportLigneCodeAstreintes.Add(rapportLigneAstreinte).Entity;
            Context.SaveChanges();
            return rapportLigneAstreinte;
        }

        /// <summary>
        /// Delete prime astreinte by astreinte id
        /// </summary>
        /// <param name="astreinteId">Astreinte identifier</param>
        public void DeletePrimesAstreinteByAstreinteId(int astreinteId)
        {
            List<RapportLigneCodeAstreinteEnt> rapportLigneCodeAstreintes = Context.RapportLigneCodeAstreintes.Where(x => x.RapportLigneAstreinte.AstreinteId == astreinteId).ToList();
            if (rapportLigneCodeAstreintes != null && rapportLigneCodeAstreintes.Any())
            {
                foreach (RapportLigneCodeAstreinteEnt rapportLigneCodeAstreinte in rapportLigneCodeAstreintes)
                {
                    DeleteById(rapportLigneCodeAstreinte.RapportLigneCodeAstreinteId);
                    Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete prime astreinte by rapport Ligne Astreinte Id
        /// </summary>
        /// <param name="rapportLigneAstreinteId">rapport Ligne Astreinte Id</param>
        public void DeletePrimesAstreinteByLigneAstreinteId(int rapportLigneAstreinteId)
        {
            List<RapportLigneCodeAstreinteEnt> rapportLigneCodeAstreintes = Context.RapportLigneCodeAstreintes.Where(x => x.RapportLigneAstreinteId.Value == rapportLigneAstreinteId).ToList();
            if (rapportLigneCodeAstreintes != null && rapportLigneCodeAstreintes.Any())
            {
                foreach (RapportLigneCodeAstreinteEnt rapportLigneCodeAstreinte in rapportLigneCodeAstreintes)
                {
                    DeleteById(rapportLigneCodeAstreinte.RapportLigneCodeAstreinteId);
                    Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Get rapport ligne code astreinte
        /// </summary>
        /// <param name="rapportLigneAstreinteId">Rapport ligne astreinte id</param>
        /// <param name="isPrimeNuit">si l'astreinte est  nuit</param>
        /// <returns>Rapport ligne code astreinre</returns>
        public RapportLigneCodeAstreinteEnt GetRapportLigneCodeAstreinteEnt(int rapportLigneAstreinteId, bool isPrimeNuit)
        {
            return Context.RapportLigneCodeAstreintes.FirstOrDefault(x => x.RapportLigneAstreinteId.Value == rapportLigneAstreinteId && x.IsPrimeNuit == isPrimeNuit);
        }

        /// <summary>
        /// Delete prime astreinte by rapport Ligne Astreinte Id liste
        /// </summary>
        /// <param name="rapportLigneAstreinteIdList">rapport Ligne Astreinte Id liste</param>
        public void DeletePrimesAstreinteByLigneAstreinteList(List<int> rapportLigneAstreinteIdList)
        {
            List<RapportLigneCodeAstreinteEnt> rapportLigneCodeAstreintes = Context.RapportLigneCodeAstreintes.Where(x => rapportLigneAstreinteIdList.Contains(x.RapportLigneAstreinteId.Value)).ToList();
            if (rapportLigneCodeAstreintes != null && rapportLigneCodeAstreintes.Any())
            {
                foreach (RapportLigneCodeAstreinteEnt rapportLigneCodeAstreinte in rapportLigneCodeAstreintes)
                {
                    DeleteById(rapportLigneCodeAstreinte.RapportLigneCodeAstreinteId);
                }
            }
        }
    }
}
