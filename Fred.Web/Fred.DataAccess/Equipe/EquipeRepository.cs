using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DataAccess.Personnel;
using Fred.Entities.Affectation;
using Fred.Entities.Personnel;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Equipe
{
    /// <summary>
    /// Equipe repository class
    /// </summary>
    public class EquipeRepository : FredRepository<EquipeEnt>, IEquipeRepository
    {
        #region constructor

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="PersonnelRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public EquipeRepository(FredDbContext context)
              : base(context)
        {
        }

        #endregion

        #region public method

        /// <summary>
        /// Creation d'une equipe
        /// </summary>
        /// <param name="equipe">Equipe entity </param>
        /// <returns>l'id de l'equipe si l'insertion est bien sinn 0</returns>
        public int? CreateEquipe(EquipeEnt equipe)
        {
            if (equipe != null)
            {
                Insert(equipe);

                return equipe.EquipeId;
            }

            return null;
        }

        /// <summary>
        /// Get une equipe par le proprietaire identifier
        /// </summary>
        /// <param name="proprietaireId">Proprietaire identifier</param>
        /// <returns>Aquipe entity</returns>
        public IEnumerable<PersonnelEnt> GetEquipePersonnelsByProprietaireId(int proprietaireId)
        {
            EquipeEnt equipe = Context.Equipe.FirstOrDefault(x => x.ProprietaireId == proprietaireId);
            if (equipe != null)
            {
                return Context.EquipePersonnel.Where(x => x.EquipePersoId == equipe.EquipeId).Select(x => x.Personnel).Include(x => x.Societe).ToList();
            }

            return new List<PersonnelEnt>();
        }

        /// <summary>
        /// Ajouter des personnels a une equipe
        /// </summary>
        /// <param name="equipeId">Equipe identifier</param>
        /// <param name="personnelsIds">List des personnels a ajouter dans l'equipe</param>
        public void AddPersonnelsToEquipeFavorite(int equipeId, IList<int> personnelsIds)
        {
            if (personnelsIds != null && personnelsIds.Any())
            {
                List<EquipePersonnelEnt> equipePersonnels = new List<EquipePersonnelEnt>();
                foreach (int id in personnelsIds)
                {
                    EquipePersonnelEnt personnelToAdd = new EquipePersonnelEnt
                    {
                        EquipePersoId = equipeId,
                        PersonnelId = id
                    };

                    equipePersonnels.Add(personnelToAdd);
                }

                Context.EquipePersonnel.AddRange(equipePersonnels);
            }
        }

        /// <summary>
        /// Suppression des personnels d'un equipe
        /// </summary>
        /// <param name="equipeId">Equipe identifier</param>
        /// <param name="equipePersonnels">List des personnels d'une equipe</param>
        public void DeletePersonnelsEquipe(int equipeId, IEnumerable<EquipePersonnelEnt> equipePersonnels)
        {
            if (equipePersonnels != null && equipePersonnels.Any())
            {
                foreach (EquipePersonnelEnt personnel in equipePersonnels)
                {
                    if (personnel != null)
                    {
                        Context.EquipePersonnel.Remove(personnel);
                    }
                }
            }
        }

        /// <summary>
        /// Get equipe personnel entity by equipe id aet personnel id 
        /// </summary>
        /// <param name="equipeId">Equipe identifier</param>
        /// <param name="peronnelId">Personnel identifier</param>
        /// <returns>Equipe personnel entity</returns>
        public EquipePersonnelEnt GetEquipePersonnel(int equipeId, int peronnelId)
        {
            return Context.EquipePersonnel.FirstOrDefault(x => x.EquipePersoId == equipeId && x.PersonnelId == peronnelId);
        }

        /// <summary>
        /// Get equipe by proprietaire identifier
        /// </summary>
        /// <param name="proprietaireId">Proprietaire identifier</param>
        /// <returns>Equipe entity object</returns>
        public EquipeEnt GetEquipeByProprietaireId(int proprietaireId)
        {
            return Context.Equipe.FirstOrDefault(x => x.ProprietaireId == proprietaireId);
        }

        /// <summary>
        /// Check if the personnel is in the team
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="equipeId">Equipe identifier</param>
        /// <returns>True if personnel already in the team</returns>
        public bool IsPersonnelInTeam(int personnelId, int equipeId)
        {
            return Context.EquipePersonnel.Any(x => x.EquipePersoId == equipeId && x.PersonnelId == personnelId);
        }

        #endregion
    }
}
