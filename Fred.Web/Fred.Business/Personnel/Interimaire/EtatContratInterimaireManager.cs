using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel.Interimaire;
using Fred.Framework;

namespace Fred.Business.Personnel.Interimaire
{
    /// <summary>
    ///   Gestionnaire des états d'un contrats d'intérimaires
    /// </summary>
    public class EtatContratInterimaireManager : Manager<EtatContratInterimaireEnt, IEtatContratInterimaireRepository>, IEtatContratInterimaireManager
    {
        private const string EtatContratInterimaireCacheKey = "EtatContratInterimaires";


        private readonly ICacheManager cacheManager;

        public EtatContratInterimaireManager(IUnitOfWork uow, IEtatContratInterimaireRepository etatContratInterimaireRepository, ICacheManager cacheManager)
         : base(uow, etatContratInterimaireRepository)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// Récupére la liste des états d'un contrat intérimaire.
        /// </summary>
        /// <returns>List des état d'un contrat intérimaire.</returns>
        public IEnumerable<EtatContratInterimaireEnt> GetEtatContratInterimaireList()
        {
            return cacheManager.GetOrCreate(
                    EtatContratInterimaireCacheKey,
                    () => Repository.GetEtatContratInterimaireList(),
                    new TimeSpan(0, 6, 0, 0, 0));
        }

        /// <summary>
        /// Récupére un état contrat intérimaire pa code
        /// </summary>
        /// <param name="code">Code état contrat intérimaire</param>
        /// <returns>Létat du contrat intérimaire</returns>
        public EtatContratInterimaireEnt GetEtatContratInterimaireByCode(string code)
        {
            var list = GetEtatContratInterimaireList();
            return list.First(x => x.Code == code);
        }
    }
}
