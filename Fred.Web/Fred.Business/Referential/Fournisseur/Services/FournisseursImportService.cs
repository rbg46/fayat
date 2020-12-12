using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;

namespace Fred.Business.Referential.Service
{
    /// <summary>
    /// Manager Fournisseur Imports
    /// </summary>
    public class FournisseursImportService : IFournisseursImportService
    {
        private readonly IUnitOfWork uow;

        /// <summary>
        /// Repository des fournisseurs
        /// </summary>
        private readonly IFournisseurRepository fournisseurRepository;

        /// <summary>
        /// Repository des agences
        /// </summary>
        private readonly IAgenceRepository agenceRepository;

        public FournisseursImportService(IUnitOfWork uow, IFournisseurRepository fournisseurRepository, IAgenceRepository agenceRepository)
        {
            this.uow = uow;
            this.fournisseurRepository = fournisseurRepository;
            this.agenceRepository = agenceRepository;
        }

        /// <summary>
        /// Mise à jour de l'agence
        /// </summary>
        /// <param name="agence">Agence</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        public void UpdateAgence(AgenceEnt agence, int userId)
        {
            AgenceEnt oldagence = agenceRepository.GetAgenceByCode(agence.Code);
            agenceRepository.UpdateAgence((int)oldagence.AgenceId, agence, userId);
            Save();
        }

        /// <summary>
        /// Ajouter ou mettre à jour un fournisseur et les agences associées
        /// </summary>
        /// <param name="fournisseurFromSap">fournisseur à importer</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        public void AddOrUpdateFournisseurs(FournisseurEnt fournisseurFromSap, int userId)
        {
            var fournisseurFred = fournisseurRepository.GetFournisseurByCodeAndGroupeId(fournisseurFromSap.GroupeId, fournisseurFromSap.Code);

            if (fournisseurFred != null)
            {
                UpdateFournisseur(fournisseurFred, fournisseurFromSap, userId);
                Save();
            }
            else
            {
                fournisseurRepository.AddFournisseurWithoutSaving(fournisseurFromSap);
            }
            Save();
        }

        private void Save()
        {
            uow.Save();
        }

        /// <summary>
        /// Mettre à jour un fournisseur
        /// </summary>
        /// <param name="fournisseurFromFred">le fournisseur FRED</param>
        /// <param name="fournisseurToUpdate">Fournisseur à mettre à jour</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        private void UpdateFournisseur(FournisseurEnt fournisseurFromFred, FournisseurEnt fournisseurToUpdate, int userId)
        {
            // Gérer que les fournisseurs non clôturés
            if (!fournisseurFromFred.DateCloture.HasValue)
            {
                fournisseurToUpdate.FournisseurId = fournisseurFromFred.FournisseurId;

                // Si fournisseur trouvé
                if (!fournisseurFromFred.Equals(fournisseurToUpdate))
                {
                    fournisseurRepository.UpdateFournisseur(fournisseurToUpdate.FournisseurId, fournisseurToUpdate);
                }

                ManageAgence(fournisseurToUpdate, userId);
            }
        }

        private void ManageAgence(FournisseurEnt fournisseur, int userId)
        {
            if (fournisseur.Agences != null && fournisseur.Agences.Count != 0)
            {
                fournisseur.Agences.ForEach(x => x.FournisseurId = fournisseur.FournisseurId);
                UpdateAgences(fournisseur.Agences, userId);
            }
            else
            {
                // Clôturer toutes les agences dans FRED pour ce fournisseurId
                List<AgenceEnt> agencesDeleteFromFred = agenceRepository.GetAgencesByFournisseur(fournisseur.FournisseurId).ToList();
                if ((agencesDeleteFromFred != null) && (agencesDeleteFromFred.Count > 0))
                {
                    // Cloturer des agences // suppression soft
                    List<int> agencesIds = agencesDeleteFromFred.Where(x => !x.DateCloture.HasValue).Select(x => (int)x.AgenceId).ToList();

                    // Clôturer une liste d'agences 
                    agenceRepository.CloturerAgence(agencesIds, userId);
                }
            }
        }

        private void UpdateAgences(List<AgenceEnt> agencesFromSource, int userId)
        {
            List<AgenceEnt> agencesFromFred = agenceRepository.GetAgencesByFournisseur(agencesFromSource[0].FournisseurId).ToList();

            foreach (AgenceEnt agence in agencesFromSource)
            {
                if (agencesFromFred.Exists(a => a.Code == agence.Code))
                {
                    AgenceEnt oldAgence = agencesFromFred.Find(a => a.Code == agence.Code);
                    if (!oldAgence.Equals(agence))
                    {
                        agenceRepository.UpdateAgence((int)oldAgence.AgenceId, agence, userId);
                    }
                }
                else
                {
                    agenceRepository.AddAgence(agence, userId);
                }
            }

            List<AgenceEnt> agencesDeleteFromFred = agencesFromFred.Where(a => !agencesFromSource.Any(s => s.Code == a.Code)).ToList();
            if (agencesDeleteFromFred.Count > 0)
            {
                // Cloturer des agences // suppression soft
                List<int> agencesIds = agencesDeleteFromFred.Where(x => !x.DateCloture.HasValue).Select(x => (int)x.AgenceId).ToList();

                // Clôturer une liste d'agences 
                if (agencesIds != null && agencesIds.Count > 0)
                {
                    agenceRepository.CloturerAgence(agencesIds, userId);
                }
            }
        }
    }
}
