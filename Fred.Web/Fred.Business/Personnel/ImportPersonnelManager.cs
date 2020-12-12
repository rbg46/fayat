using System.Collections.Generic;
using System.Linq;
using Fred.Business.ReferentielFixe;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.Personnel
{
    /// <summary>
    /// Manager pour l'importation du personnel Fes et Fon
    /// </summary>
    public class ImportPersonnelManager : IImportPersonnelManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ISocieteManager societeManager;
        private readonly IReferentielFixeManager referentielFixeManager;
        private readonly ISocieteRepository repoSocietes;
        private readonly IEtablissementPaieRepository etablissementPaieRepository;
        private readonly IEtablissementComptableRepository etablissementComptableRepository;
        private readonly IPersonnelRepository personnelRepository;
        private readonly ICIRepository cIRepository;
        private readonly IAffectationRepository affectationRepository;

        /// <summary>
        /// Manager pour l'importation du personnel FES
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>
        /// <param name="utilisateurManager">utilisateurManager</param>
        /// <param name="societeManager">societeManager</param>     
        /// <param name="referentielFixeManager">referentielFixeManager</param>
        public ImportPersonnelManager(IUnitOfWork unitOfWork,
                              IUtilisateurManager utilisateurManager,
                              ISocieteManager societeManager,
                              IReferentielFixeManager referentielFixeManager,
                              ISocieteRepository repoSocietes,
                              IEtablissementPaieRepository etablissementPaieRepository,
                              IEtablissementComptableRepository etablissementComptableRepository,
                              IPersonnelRepository personnelRepository,
                              ICIRepository cIRepository,
                              IAffectationRepository affectationRepository)
        {
            this.unitOfWork = unitOfWork;
            this.utilisateurManager = utilisateurManager;
            this.societeManager = societeManager;
            this.referentielFixeManager = referentielFixeManager;
            this.repoSocietes = repoSocietes;
            this.etablissementPaieRepository = etablissementPaieRepository;
            this.etablissementComptableRepository = etablissementComptableRepository;
            this.personnelRepository = personnelRepository;
            this.cIRepository = cIRepository;
            this.affectationRepository = affectationRepository;
        }


        /// <summary>
        ///   Retourne la société portant le code société paye indiqué.
        /// </summary>
        /// <param name="codeSocietePaye">Code de la société de paye dont l'identifiant est à retrouver.</param>
        /// <returns>Societe retrouvé, sinon nulle. Souleve une exception si plusieurs societe porte le meme codeSocietePaye</returns>
        public SocieteEnt GetSocieteByCodeSocietePaye(string codeSocietePaye)
        {
            var societes = repoSocietes.Query().Filter(s => s.CodeSocietePaye == codeSocietePaye && s.Active).Get().ToList();

            if (societes != null && societes.Count > 1)
            {
                throw new Fred.Framework.Exceptions.FredRepositoryException("2 sociétés on le meme CodeSocietePaye.");
            }

            return societes.FirstOrDefault();
        }


        /// <summary>
        /// Recupere les etablissements de paye pour plusieurs societes
        /// </summary>
        /// <param name="societeIds">id des societes</param>
        /// <returns>Liste EtablissementPaieEnt</returns>
        public IEnumerable<EtablissementPaieEnt> GetEtablissementPaiesBySocieteIds(List<int> societeIds)
        {
            var query = etablissementPaieRepository.Query()
                            .Filter(e => e.SocieteId.HasValue && societeIds.Contains(e.SocieteId.Value))
                            .Get()
                            .AsNoTracking();
            return query.ToList();
        }

        public IEnumerable<EtablissementComptableEnt> GetEtablissementComptablesBySocieteIds(List<int> societeIds)
        {
            return etablissementComptableRepository.Query()
                            .Filter(e => e.SocieteId.HasValue && societeIds.Contains(e.SocieteId.Value))
                            .Get()
                            .AsNoTracking()
                            .ToList();
        }

        /// <summary>
        /// Recupere la liste des ressources epour un groupe
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <returns>Liste des ressources pour un groupe</returns>
        public IEnumerable<RessourceEnt> GetRessourceListByGroupeId(int groupId)
        {
            return this.referentielFixeManager.GetRessourceListByGroupeId(groupId);
        }


        /// <summary>
        ///  Recupere les personnels  pour plusieurs societes
        /// </summary>
        /// <param name="societeIds">societeIds</param>
        /// <returns>les personnels  pour plusieurs societes</returns>
        public IEnumerable<PersonnelEnt> GetPersonnelListBySocieteIds(List<int> societeIds)
        {
            return personnelRepository.Query()
                    .Filter(x => x.SocieteId.HasValue && societeIds.Contains(x.SocieteId.Value))
                    .Get()
                    .AsNoTracking();
        }

        /// <summary>
        /// Recupere la liste des societes
        /// </summary>
        /// <returns> liste des societes</returns>
        public IEnumerable<SocieteEnt> GetSocieteList()
        {
            return societeManager.GetSocieteList();
        }

        /// <summary>
        /// Recupere les ci de fred.
        /// Le filtre sur la societe sera fait plutard, Il ne faut pas que l'unicite se fait sur le code et la societe.
        /// </summary>
        /// <param name="ciCodes">Liste des Code des CI a recuperes</param>
        /// <returns>Liste de ci</returns>
        public IEnumerable<CIEnt> GetCIsByCodes(IEnumerable<string> ciCodes)
        {
            return cIRepository.Query()
                .Filter(x => ciCodes.Contains(x.CodeExterne))
                .Get()
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Obtient toutes les affectations par default
        /// </summary>
        /// <returns>return toutes les affectations par default </returns>
        public IEnumerable<AffectationEnt> GetDefaultAffectations()
        {
            return affectationRepository.Get().AsNoTracking().ToList();
        }

        /// <summary>
        /// Recupere l'utilisateur FredIE
        /// </summary>
        /// <returns>fredIe</returns>
        public UtilisateurEnt GetFredIe()
        {
            return this.utilisateurManager.GetByLogin("fred_ie");
        }

    }
}
