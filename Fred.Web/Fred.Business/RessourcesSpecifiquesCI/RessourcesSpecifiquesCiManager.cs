using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;

namespace Fred.Business.RessourcesSpecifiquesCI
{
    /// <summary>
    /// Gestionnaire des ressources spécifiques CI
    /// </summary>
    public class RessourcesSpecifiquesCiManager : Manager<RessourceEnt, IRessourceRepository>, IRessourcesSpecifiquesCiManager
    {
        private readonly IReferentielEtenduRepository referentielEtenduRepository;
        private readonly IUtilisateurManager userManager;
        private readonly ISepService sepService;

        #region Ctors

        public RessourcesSpecifiquesCiManager(
            IUnitOfWork uow,
            IRessourceRepository ressourceRepository,
            IUtilisateurManager userManager,
            IReferentielEtenduRepository referentielEtenduRepository,
            ISepService sepService)
            : base(uow, ressourceRepository)
        {
            this.referentielEtenduRepository = referentielEtenduRepository;
            this.userManager = userManager;
            this.sepService = sepService;
        }

        #endregion

        #region IRessourcesSpecifiquesCiManager

        /// <summary>
        ///   Retourne la liste des referentielEtendus pour une ci spécifique.
        /// </summary>
        /// <param name="ciId">Identifiant de la CI</param>
        /// <returns>Liste des referentielEtendus.</returns>
        public IEnumerable<ChapitreEnt> GetAllReferentielEtenduAsChapitreList(int ciId)
        {
            List<ReferentielEtenduEnt> ressourcesEtendus;

            if (sepService.IsSep(ciId))
            {
                int societeGeranteId = sepService.GetSocieteGeranteForSep(ciId).SocieteId;
                ressourcesEtendus = referentielEtenduRepository.GetReferentielsEtendusBySocieteId(societeGeranteId).ToList();
            }
            else
            {
                ressourcesEtendus = referentielEtenduRepository.GetReferentielsEtendusByCi(ciId).ToList();
            }

            referentielEtenduRepository.AddRessourcesSpecifiqueInReferentielEtendu(ciId, ref ressourcesEtendus);

            var listeChapitre = ressourcesEtendus.GroupBy(c => c.Ressource.SousChapitre)
                       .GroupBy(s => s.Key.Chapitre)
                       .Select(c => c.Key)
                       .ToList();

            return listeChapitre;
        }

        /// <summary>
        ///   Ajoute une ressource specifique ci
        /// </summary>
        /// <param name="ressource">ressource créée</param>
        /// <returns>Ressource ajoutée</returns>
        public RessourceEnt Add(RessourceEnt ressource)
        {
            Repository.AddRessource(ressource);
            Save();

            return ressource;
        }

        /// <summary>
        /// Permet de supprimer une ressource à partir de son identifiant
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <returns>Ressource déactivée avec date de suppression</returns> 
        public RessourceEnt DeleteById(int ressourceId)
        {
            var ressource = Repository.FindById(ressourceId);
            ressource.AuteurSuppressionId = userManager.GetContextUtilisateurId();
            ressource.DateSuppression = DateTime.UtcNow;
            ressource.Active = false;
            Repository.UpdateRessource(ressource);
            Save();
            return ressource;
        }

        /// <summary>
        /// Permet d'obtenir un code incrementé pour la nouvelle ressource créée
        /// </summary>
        /// <param name="ressourceRattachementId"> Identifiant de la ressource rattaché</param>
        /// <returns>le code incrémenté</returns>
        public string GetNextRessourceCode(int ressourceRattachementId)
        {
            //recherche du prefixe
            var prefixe = Repository.Query().Filter(r => ressourceRattachementId.Equals(r.RessourceId)).Get().Select(re => re.Code).FirstOrDefault();
            var numMax = referentielEtenduRepository.Query().Filter(r => ressourceRattachementId.Equals(r.ReferentielEtenduId)).Get().Count() - 1;
            var newCode = string.Empty;
            do
            {
                numMax++;
                newCode = string.Format("{0}-{1}", prefixe, numMax);
            }
            while (Repository.Query().Get().Any(re => newCode.Equals(re.Code)));
            return newCode;
        }

        #endregion
    }
}
