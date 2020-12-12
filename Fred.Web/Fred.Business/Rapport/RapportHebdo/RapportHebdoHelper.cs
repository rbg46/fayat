using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Referential;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Rapport.RapportHebdo
{
    /// <summary>
    /// Helper pour les rapport hebdomadaire.
    /// </summary>
    public class RapportHebdoHelper : ManagersAccess
    {
        private readonly ICodeMajorationManager codeMajorationManager;
        private readonly IPrimeManager primeManager;
        private readonly List<Tuple<int, PrimeEnt>> lazyPrimes;
        private readonly List<Tuple<int, CodeMajorationEnt>> lazyMajorations;
        private readonly List<string> departementsIdf;

        /// <summary>
        /// Constructeur.
        /// </summary>
        public RapportHebdoHelper(ICodeMajorationManager codeMajorationManager, IPrimeManager primeManager)
        {
            this.codeMajorationManager = codeMajorationManager;
            this.primeManager = primeManager;
            lazyPrimes = new List<Tuple<int, PrimeEnt>>();
            lazyMajorations = new List<Tuple<int, CodeMajorationEnt>>();
            departementsIdf = new List<string>() { "75", "92", "93", "94" };
        }

        /// <summary>
        /// Indique si une ligne de rapport contient une prime active.
        /// </summary>
        /// <param name="ligne">La ligne de rapport concernée.</param>
        /// <param name="codePrime">Le code de la prime concernée.</param>
        /// <returns>True si la ligne de rapport contient la prime, sinon false.</returns>
        public bool HasPrime(RapportLigneEnt ligne, string codePrime)
        {
            foreach (var primeLigne in ligne.ListRapportLignePrimes.Where(lp => !lp.IsDeleted && lp.IsChecked))
            {
                // Note : parfois majorationLigne.CodeMajorationId = 0, mais majorationLigne.CodeMajoration n'est pas null...
                // Par précaution, on fait pareil avec les primes
                var primeId = primeLigne.PrimeId > 0
                    ? primeLigne.PrimeId
                    : primeLigne.Prime?.PrimeId;

                if (primeId.HasValue)
                {
                    var prime = GetPrime(primeId.Value);
                    if (prime != null && prime.Code == codePrime)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Récupère une prime en fonction de son identifiant.
        /// </summary>
        /// <param name="primeId">L'identifiant de la prime.</param>
        /// <returns>La prime, peut être null.</returns>
        public PrimeEnt GetPrime(int primeId)
        {
            var tuple = lazyPrimes.FirstOrDefault(p => p.Item1 == primeId);
            if (tuple == null)
            {
                var prime = primeManager.FindById(primeId);
                tuple = new Tuple<int, PrimeEnt>(primeId, prime);
                lazyPrimes.Add(tuple);
            }
            return tuple.Item2;
        }

        /// <summary>
        /// Indique si une ligne de rapport contient une majoration.
        /// </summary>
        /// <param name="ligne">La ligne de rapport concernée.</param>
        /// <param name="codeMajoration">Le code de la majoration concernée.</param>
        /// <returns>True si la ligne de rapport contient la majoration, sinon false.</returns>
        public bool HasMajoration(RapportLigneEnt ligne, string codeMajoration)
        {
            foreach (var majorationLigne in ligne.ListRapportLigneMajorations.Where(lm => !lm.IsDeleted && lm.HeureMajoration > 0))
            {
                // Note : parfois majorationLigne.CodeMajorationId = 0, mais majorationLigne.CodeMajoration n'est pas null...
                var codeMajorationId = majorationLigne.CodeMajorationId > 0
                    ? majorationLigne.CodeMajorationId
                    : majorationLigne.CodeMajoration?.CodeMajorationId;

                if (codeMajorationId.HasValue)
                {
                    var majoration = GetMajoration(codeMajorationId.Value);
                    if (majoration != null && majoration.Code == codeMajoration)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Récupère une majoration en fonction de son identifiant.
        /// </summary>
        /// <param name="majorationId">L'identifiant de la majoration.</param>
        /// <returns>La majoration, peut être null.</returns>
        public CodeMajorationEnt GetMajoration(int majorationId)
        {
            var tuple = lazyMajorations.FirstOrDefault(p => p.Item1 == majorationId);
            if (tuple == null)
            {
                var majoration = codeMajorationManager.FindById(majorationId);
                tuple = new Tuple<int, CodeMajorationEnt>(majorationId, majoration);
                lazyMajorations.Add(tuple);
            }
            return tuple.Item2;
        }

        /// <summary>
        /// Indique si un code postal est dans la zone Île-de-France.
        /// </summary>
        /// <param name="codePostal">Le code postal concerné.</param>
        /// <returns>True si le code postal est dans la zone Île-de-France, sinon false.</returns>
        public bool IsCodePostalIdf(string codePostal)
        {
            if (codePostal == null || codePostal.Length < 2)
            {
                throw new ArgumentException(FeatureRapportHebdo.RapportHebdoHelper_Code_Postal_Invalide, nameof(codePostal));
            }

            var departement = codePostal.Substring(0, 2);
            return departementsIdf.Contains(departement);
        }
    }
}
