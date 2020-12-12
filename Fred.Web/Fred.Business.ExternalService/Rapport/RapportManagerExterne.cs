using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.ExternalService.FredImportExport.Rapport;
using Fred.Entities;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Helpers;
using Fred.Web.Shared.Models.Rapport;

namespace Fred.Business.ExternalService.Rapport
{
    public class RapportManagerExterne : IRapportManagerExterne
    {
        private readonly IRapportRepositoryExterne rapportRepositoryExterne;

        public RapportManagerExterne(IRapportRepositoryExterne rapportRepositoryExterne)
        {
            this.rapportRepositoryExterne = rapportRepositoryExterne;
        }

        /// <summary>
        /// API PRIVE FRED - FRED IE
        /// Permet d'exporter les pointages des PERSONNEL vers SAP.
        /// </summary>
        /// <param name="rapportId"> Id du rapport validé</param>
        public async Task ExportPointagePersonnelToSapAsync(int rapportId)
        {
            try
            {
                await rapportRepositoryExterne.ExportPointagePersonnelToSapAsync(rapportId);
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }


        /// <summary>
        /// API PRIVE FRED - FRED IE Async
        /// Permet d'exporter les pointages des PERSONNEL vers SAP en asynchrone.
        /// </summary>
        /// <param name="rapportId"> Id du rapport validé</param>
        /// <param name="currentUser">Utilisateur connecté</param>
        public async Task ExportPointagePersonnelToSapAsync(int rapportId, UtilisateurEnt currentUser)
        {
            // FAYAT TP UNIQUEMENT
            if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeFTP)
            {
                // Flux CAT2
                await rapportRepositoryExterne.ExportPointagePersonnelToSapAsync(rapportId);
            }
        }

        /// <summary>
        /// API PRIVE FRED - FRED IE
        /// Permet d'exporter les pointages des PERSONNEL de plusieurs rapports vers SAP.
        /// </summary>
        /// <param name="rapportIds">Liste d'Id de rapports validés</param>
        public async Task ExportPointagePersonnelToSapAsync(List<int> rapportIds)
        {
            try
            {
                if (rapportIds.Any())
                {
                    await rapportRepositoryExterne.ExportPointagePersonnelToSapAsync(rapportIds);
                }
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        /// <summary>
        /// API PRIVE FRED - FRED IE Async
        /// Permet d'exporter les pointages des PERSONNEL de plusieurs rapports vers SAP en asynchrone.
        /// </summary>
        /// <param name="rapportIds">Liste d'Id de rapports validés</param>
        /// <param name="currentUser">Utilisateur connecté</param>
        public async Task ExportPointagePersonnelToSapAsync(List<int> rapportIds, UtilisateurEnt currentUser)
        {
            // FAYAT TP UNIQUEMENT
            if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeFTP && rapportIds.Any())
            {
                await ExportPointagePersonnelToSapBatch(rapportIds);
            }
        }

        private async Task ExportPointagePersonnelToSapBatch(IEnumerable<int> listRapportIds)
        {
            int cat2LotFlux = int.Parse(ConfigurationManager.AppSettings["CAT2:LotFlux"]);
            IEnumerable<IEnumerable<int>> chunkedList = ChunkListHelper.Chunk<int>(listRapportIds, cat2LotFlux);
            foreach (IEnumerable<int> list in chunkedList)
            {
                await rapportRepositoryExterne.ExportPointagePersonnelToSapAsync(list.ToList());
            }
        }

        /// <summary>
        /// Permet d'envoyer les paramètres de selection à Tibco pour qu'il puisse appeler api Fred ie 
        /// </summary>
        /// <param name="filter">model de filtre</param>    
        public async Task ExportPointagePersonnelToTibcoAsync(ExportPointagePersonnelFilterModel filter)
        {
            await rapportRepositoryExterne.ExportPointagePersonnelToTibcoAsync(filter);
        }
    }
}
