using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Rapport.Pointage.Validation.Interfaces;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.Validation
{
    public class RapportLignesValidationDataProvider : IRapportLignesValidationDataProvider
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IPointageRepository pointageRepository;

        public RapportLignesValidationDataProvider(
            IUtilisateurManager utilisateurManager,
            IPointageRepository pointageRepository)
        {
            this.utilisateurManager = utilisateurManager;
            this.pointageRepository = pointageRepository;
        }

        /// <summary>
        /// Recupere les données necessaires pour faire la validation d'une liste de pointages. 
        /// </summary>
        /// <param name="rapportLignes">Les lignes de rapports Lignes</param>        
        /// <returns>Les données necessaires pour deterrminer les message d'erreurs</returns>
        public GlobalDataForValidationPointage GetDataForValidateRapportLignes(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            var personnelIdsOfPointages = rapportLignes.Where(o => !o.IsDeleted).Where(rl => rl.PersonnelId.HasValue).Select(rl => rl.PersonnelId.Value).ToList();
            return GetDataForValidateRapportLignes(rapportLignes, personnelIdsOfPointages);
        }

        /// <summary>
        /// Recupere les données necessaires pour faire la validation d'un pointage. 
        /// </summary>
        /// <param name="rapportLigne">Le rapportsLignes</param>        
        /// <returns>Les données necessaires pour deterrminer les message d'erreurs</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="rapportLigne"/> is <c>null</c>.</exception>
        public GlobalDataForValidationPointage GetDataForValidateRapportLignes(RapportLigneEnt rapportLigne)
        {
            if (rapportLigne == null)
            {
                throw new System.ArgumentNullException(nameof(rapportLigne));
            }

            var rapportLignes = new List<RapportLigneEnt>()
            {
                rapportLigne
            };

            if (rapportLigne.PersonnelId.HasValue)
            {
                var personnelIds = new List<int>()
                {
                    rapportLigne.PersonnelId.Value
                };
                return GetDataForValidateRapportLignes(rapportLignes, personnelIds);
            }
            return GetDataForValidateRapportLignes(rapportLignes, null);
        }

        /// <summary>
        /// Recupere les données necessaires pour faire la validation d'une liste de pointages et pour une liste de personnel. 
        /// </summary>
        /// <param name="listPointages">Les lignes de rapports</param>
        /// <param name="personnelIds">les id des personnels</param>    
        /// <returns>Les données necessaires pour calculer les champs caclulés</returns>
        private GlobalDataForValidationPointage GetDataForValidateRapportLignes(IEnumerable<RapportLigneEnt> listPointages, IEnumerable<int> personnelIds)
        {
            if (personnelIds == null)
            {
                personnelIds = new List<int>();
            }

            int currentUserId = utilisateurManager.GetContextUtilisateurId();
            List<int> personnelIdsDistincts = personnelIds.Distinct().ToList();
            List<int> ciIdsOfRapportLignes = listPointages.Select(p => p.CiId).Distinct().ToList();
            List<DateTime> datesPointagesOfPointages = listPointages.Select(p => p.DatePointage).Distinct().ToList();
            IEnumerable<RapportLigneEnt> rapportsLignesOnAllRapports = this.pointageRepository.GetRapportLignesForCalculateTotalHeuresNormalesAndMajorations(personnelIdsDistincts, ciIdsOfRapportLignes, datesPointagesOfPointages);
            IEnumerable<RapportLigneEnt> rapportsLignesWithPrimes = this.pointageRepository.GetRapportLignesWithPrimesForCalculatePrimeJournaliereAlreadyExists(personnelIdsDistincts, datesPointagesOfPointages);
            List<int> ciIdsOfPointagesWithRolePaieForCurrentUser = new List<int>();
            Dictionary<int, bool> ciIdIsRolePaie = utilisateurManager.IsRolePaie(currentUserId, ciIdsOfRapportLignes);

            foreach (var ciId in ciIdsOfRapportLignes)
            {
                if (ciIdIsRolePaie[ciId])
                {
                    ciIdsOfPointagesWithRolePaieForCurrentUser.Add(ciId);
                }
            }

            return new GlobalDataForValidationPointage()
            {
                RapportsLignesOnAllRapports = rapportsLignesOnAllRapports,
                RapportsLignesWithPrimes = rapportsLignesWithPrimes,
                CiIdsOfPointagesWithRolePaieForCurrentUser = ciIdsOfPointagesWithRolePaieForCurrentUser,
            };
        }
    }
}
