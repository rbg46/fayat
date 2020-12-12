using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Rapport;
using Fred.DataAccess.ExternalService.FredImportExport.Materiel;
using Fred.Entities;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;

namespace Fred.Business.ExternalService.Materiel
{
    /// <summary>
    /// Gestionnaire externe des matériels
    /// </summary>
    public class MaterielManagerExterne : IMaterielManagerExterne
    {
        private readonly IMaterielRepositoryExterne materielRepo;
        protected readonly IRapportManager rapportMgr;

        public MaterielManagerExterne(IMaterielRepositoryExterne materielRepo, IRapportManager rapportMgr)
        {
            this.materielRepo = materielRepo;
            this.rapportMgr = rapportMgr;
        }

        /// <inheritdoc />
        public async Task ExportPointageMaterielToStormAsync(int rapportId)
        {
            try
            {
                // On envoie le rapport à SAP et (à faire DMU : on sauvegarde l'idenfiant du job Hangfire).
                await materielRepo.ExportPointageMaterielToStormAsync(rapportId);
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        /// <inheritdoc />
        public async Task ExportPointageMaterielToStormAsync(int rapportId, UtilisateurEnt currentUser)
        {
            // pas d'envoi à STORM pour FES 
            if (currentUser.Personnel.Societe.Groupe.Code != Constantes.CodeGroupeFES)
            {
                await materielRepo.ExportPointageMaterielToStormAsync(rapportId);
            }
        }

        /// <inheritdoc />
        public async Task ExportPointageMaterielToStormAsync(List<int> rapportIds)
        {
            try
            {
                if (rapportIds.Any())
                {
                    // On envoie la liste des identifiants rapport à SAP et (à faire DMU : on sauvegarde l'idenfiant du job Hangfire).
                    await materielRepo.ExportPointageMaterielToStormAsync(rapportIds);
                }
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        /// <inheritdoc />
        public async Task ExportPointageMaterielToStormAsync(List<int> rapportIds, UtilisateurEnt currentUser)
        {
            // pas d'envoi à STORM pour FES 
            if (currentUser.Personnel.Societe.Groupe.Code != Constantes.CodeGroupeFES && rapportIds.Any())
            {
                // On envoie la liste des identifiants rapport à SAP et (à faire DMU : on sauvegarde l'idenfiant du job Hangfire).
                await materielRepo.ExportPointageMaterielToStormAsync(rapportIds);
            }
        }
    }
}
