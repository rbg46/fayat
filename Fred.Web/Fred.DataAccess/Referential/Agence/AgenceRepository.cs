using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.Agence
{
    /// <summary>
    ///   Référentiel de données pour les agences fournisseurs.
    /// </summary>
    public class AgenceRepository : FredRepository<AgenceEnt>, IAgenceRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="IAgenceRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">Interface nitOfWork</param>
        public AgenceRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// envoie une angence suivant id
        /// </summary>
        /// <param name="codeAgence">Code agence</param>
        /// <returns>retourne une agence</returns>
        public AgenceEnt GetAgenceByCode(string codeAgence)
        {
            return Query()
                .Include(a => a.Adresse)
                .Filter(x => x.Code == codeAgence && !x.DateCloture.HasValue)
                .Get()
                .AsNoTracking()
                .Single();
        }

        /// <summary>
        /// Envoi la liste des agences par fournisseur
        /// </summary>
        /// <param name="fournisseurId">id fournisseur</param>
        /// <returns>liste des agences</returns>
        public IEnumerable<AgenceEnt> GetAgencesByFournisseur(int fournisseurId)
        {
            return Query()
                .Include(a => a.Adresse)
                .Filter(x => x.FournisseurId == fournisseurId)
                .Get()
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Envoi la liste des agences par fournisseur
        /// </summary>
        /// <param name="fournisseursIds">liste des ids fournisseur</param>
        /// <returns>liste des agences</returns>
        public IEnumerable<AgenceEnt> GetAgencesByFournisseurIds(List<int> fournisseursIds)
        {
            return Query()
                .Include(a => a.Adresse)
                .Filter(x => fournisseursIds.Any(p => p.Equals(x.FournisseurId)))
                .Get().AsNoTracking().ToList();
        }

        /// <summary>
        /// Mise à jour d'une Agence
        /// </summary>
        /// <param name="agenceId">id agence</param>
        /// <param name="agence">Agence à Mettre à jour</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        public void UpdateAgence(int agenceId, AgenceEnt agence, int userId)
        {
            DateTime dateMaj = DateTime.UtcNow;

            Update(GetAndUpdate(agenceId, agence, dateMaj, userId));

            AgenceEnt oldAgence = Get().Include(a => a.Adresse).Where(x => x.AgenceId == agenceId).AsNoTracking().FirstOrDefault();
            if (!oldAgence.Adresse.Equals(agence.Adresse))
            {
                agence.Adresse.AdresseId = oldAgence.AdresseId;

                // Maj meta data : Adresse
                agence.Adresse.AuteurModificationId = userId;
                agence.Adresse.DateModification = dateMaj;

                Context.Adresses.Update(agence.Adresse);
            }
        }

        /// <summary>
        /// Ajouter une nouvelle Agence
        /// </summary>
        /// <param name="agence">Nouvelle Agence</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        public void AddAgence(AgenceEnt agence, int userId)
        {
            DateTime dateCreation = DateTime.UtcNow;

            // Init meta data
            agence.AuteurCreationId = userId;
            agence.AuteurModificationId = userId;
            agence.DateCreation = dateCreation;
            agence.DateModification = dateCreation;

            // Maj meta data : Adresse
            if (agence.Adresse != null)
            {
                agence.Adresse.AuteurCreationId = userId;
                agence.Adresse.AuteurModificationId = userId;
                agence.Adresse.DateCreation = dateCreation;
                agence.Adresse.DateModification = dateCreation;
            }

            Insert(agence);
        }

        /// <summary>
        /// Mettre à jour four
        /// </summary>
        /// <param name="agenceId">id agence à mettre à jour</param>
        /// <param name="agencerUpdate">agence à mettre à jour</param>
        /// <param name="dateMaj">Date de la màj</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Metre à jour Fournisseurs</returns>
        private AgenceEnt GetAndUpdate(int agenceId, AgenceEnt agencerUpdate, DateTime dateMaj, int userId)
        {
            var entity = Context.Agences.Find(agenceId);
            entity.Libelle = agencerUpdate.Libelle;
            entity.Telephone = agencerUpdate.Telephone;
            entity.Fax = agencerUpdate.Fax;
            entity.Email = agencerUpdate.Email;
            entity.SIRET = agencerUpdate.SIRET;
            entity.DateModification = dateMaj;
            entity.AuteurModificationId = userId;
            return entity;
        }

        /// <summary>
        /// Clôturer une liste d'agences
        /// </summary>
        /// <param name="agencesIds">Liste des identifiants à clôturer</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        public void CloturerAgence(List<int> agencesIds, int userId)
        {

            DateTime dateCloture = DateTime.UtcNow;

            // Update date de clôture
            Context.Agences
              .Where(x => agencesIds.Any(y => y == x.AgenceId))
              .ToList()
              .ForEach(x =>
                {
                    x.DateCloture = dateCloture;
                    x.DateModification = dateCloture;
                    x.AuteurModificationId = userId;
                });
        }
    }
}
