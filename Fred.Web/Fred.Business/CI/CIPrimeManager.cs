using System;
using System.Collections.Generic;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Framework.Exceptions;

namespace Fred.Business.CI
{
    /// <summary>
    ///   Gestionnaire des CIPrimes
    /// </summary>
    public class CIPrimeManager : Manager<CIPrimeEnt, ICIPrimeRepository>, ICIPrimeManager
    {
        private readonly IUtilisateurManager utilisateurMgr;

        public CIPrimeManager(IUnitOfWork uow, ICIPrimeRepository ciPrimeRepository, IUtilisateurManager utilisateurMgr)
          : base(uow, ciPrimeRepository)
        {
            this.utilisateurMgr = utilisateurMgr;
        }

        /// <inheritdoc />
        public IEnumerable<CIPrimeEnt> GetPrimes()
        {
            return Repository.Query().Get();
        }

        /// <inheritdoc />
        public IEnumerable<CIPrimeEnt> GetPrimesSync()
        {
            return Repository.GetCIPrimeSync();
        }

        /// <inheritdoc />
        public IEnumerable<CIPrimeEnt> GetSyncCIPrimes(DateTime lastModification)
        {
            try
            {
                //On récupère l'utilisateur courant.
                var currentUser = this.utilisateurMgr.GetContextUtilisateur();

                IEnumerable<CIPrimeEnt> ciPrimes = null;
                if (currentUser != null)
                {
                    ciPrimes = Repository.GetCIPrimePrivated(currentUser.Personnel.SocieteId, lastModification);
                }

                return ciPrimes;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
