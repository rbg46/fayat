using System;
using System.Collections.Generic;

using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ValidationPointage;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.ValidationPointage
{
    /// <summary>
    ///   Gestionnaire des Lots de pointage
    /// </summary>
    public class LotPointageManager : Manager<LotPointageEnt, ILotPointageRepository>, ILotPointageManager
    {
        private readonly IUtilisateurManager userManager;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="LotPointageManager" />
        /// </summary>
        /// <param name="uow"> Unit Of Work </param>
        /// <param name="lotPointageRepository"></param>
        /// <param name="validator">Valideur des lots de pointage</param>
        /// <param name="userManager">Gestionnaire des utilisateurs</param>
        public LotPointageManager(IUnitOfWork uow, ILotPointageRepository lotPointageRepository, ILotPointageValidator validator, IUtilisateurManager userManager)
          : base(uow, lotPointageRepository, validator)
        {
            this.userManager = userManager;
        }

        /// <inheritdoc />
        public LotPointageEnt Get(int utilisateurId, DateTime periode)
        {
            return Repository.Get(utilisateurId, periode);
        }

        private LotPointageEnt GetWithoutLines(int utilisateurId, DateTime periode)
        {
            return Repository.GetWithoutLines(utilisateurId, periode);
        }

        /// <summary>
        /// Obtient le lot de pointage selon les paramètres spécifiés, ou le crée s'il n'existe pas
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur créateur du lot de pointage</param>
        /// <param name="periode">Date de la période du lot de pointage</param>
        /// <returns>Lot de pointage</returns>
        public LotPointageEnt GetorCreate(int utilisateurId, DateTime periode)
        {
            LotPointageEnt lotPointage = this.GetWithoutLines(utilisateurId, periode);
            if (lotPointage == null)
            {
                lotPointage = new LotPointageEnt { Periode = periode, AuteurCreationId = utilisateurId };
                lotPointage = this.AddLotPointage(lotPointage, utilisateurId);
            }

            return lotPointage;
        }

        /// <inheritdoc />
        public int? GetLotPointageId(int utilisateurId, DateTime periode)
        {
            return Repository.GetLotPointageId(utilisateurId, periode);
        }

        /// <summary>
        /// Récupération des lots de pointages d'un utilisateur sur plusieurs périodes
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="periodes">Liste de périodes (string yyyyMM)</param>
        /// <returns>Dictionnaire (string periode yyyyMM, lotPointageId)</returns>
        public Dictionary<string, int?> GetLotPointageId(int utilisateurId, List<string> periodes)
        {
            Dictionary<string, int?> dico = new Dictionary<string, int?>();

            List<LotPointageEnt> allLotPointage = Repository.Query().Filter(x => x.AuteurCreationId == utilisateurId)
                                                                    .Get()
                                                                    .AsNoTracking()
                                                                    .ToList();

            allLotPointage = allLotPointage.Where(x => periodes.Contains(x.Periode.ToString("yyyyMM"))).ToList();

            foreach (var d in periodes)
            {
                dico.Add(d, allLotPointage.Find(x => x.Periode.ToString("yyyyMM") == d)?.LotPointageId);
            }

            return dico;
        }

        /// <inheritdoc />
        public LotPointageEnt AddLotPointage(LotPointageEnt lotPointage, int utilisateurId)
        {
            BusinessValidation(lotPointage);
            return Repository.AddLotPointage(lotPointage, utilisateurId);
        }

        /// <inheritdoc />
        public void DeleteLotPointage(int lotPointageId)
        {
            Repository.DeleteLotPointage(lotPointageId);
            Save();
        }

        /// <inheritdoc />
        public LotPointageEnt Get(int lotPointageId)
        {
            return Repository.Get(lotPointageId);
        }

        public async Task<LotPointageEnt> FindByIdNotTrackedAsync(int id)
        {
            return await Repository.FindByIdNotTrackedAsync(id);
        }

        /// <summary>
        ///   Récupère un Lot de Pointage en fonction de son Identifiant
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>
        /// <param name="includes">Includes lors du get</param>
        /// <returns>Lot de pointage</returns>
        public LotPointageEnt Get(int lotPointageId, List<Expression<Func<LotPointageEnt, object>>> includes)
        {
            return Repository.Get(lotPointageId, includes);
        }

        /// <inheritdoc />
        public IEnumerable<LotPointageEnt> GetAll()
        {
            return Repository.GetAll();
        }

        /// <inheritdoc />
        public IEnumerable<LotPointageEnt> GetList(int auteurCreationId)
        {
            return Repository.GetList(auteurCreationId);
        }

        /// <inheritdoc />
        public IEnumerable<LotPointageEnt> GetList(DateTime periode)
        {
            return Repository.GetList(periode);
        }

        /// <inheritdoc />
        public LotPointageEnt UpdateLotPointage(LotPointageEnt lotPointage, int utilisateurId)
        {
            this.BusinessValidation(lotPointage);
            return Repository.UpdateLotPointage(lotPointage, utilisateurId);
        }

        /// <inheritdoc />
        public LotPointageEnt SignLotPointage(int lotPointageId)
        {
            // Le visa marque la fin des vérifications sur le lot par le Correspondant
            // Vérifier s'il y a eu un Contrôle Chantier = OK, un Contrôle Vrac = OK

            int utilisateurId = this.userManager.GetContextUtilisateurId();

            LotPointageEnt lotPointage = Get(lotPointageId);
            lotPointage.AuteurVisaId = utilisateurId;
            lotPointage.DateVisa = DateTime.UtcNow;

            UpdateLotPointage(lotPointage, utilisateurId);

            return Get(lotPointageId);
        }

        /// <summary>
        /// Get lot pointage by userId and periode
        /// </summary>
        /// <param name="utilisateursIds">List des utilisateurs ids</param>
        /// <param name="periode">Période</param>
        /// <returns>Lot de pointage</returns>
        public IEnumerable<LotPointageEnt> GetLotPointageByListUserIdAndPeriode(List<int> utilisateursIds, DateTime periode)
        {
            IEnumerable<LotPointageEnt> listLotPointage = Repository.GetLotPointageByListUserIdAndPeriode(utilisateursIds, periode);
            foreach (LotPointageEnt lotPointage in listLotPointage)
            {
                lotPointage.RapportLignes = lotPointage.RapportLignes.Where(rl => !rl.DateSuppression.HasValue).ToList();
            }

            return listLotPointage;
        }
    }
}
