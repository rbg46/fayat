using System.Collections.Generic;
using System.Linq;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Database.ImportExport;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.DataAccess.ContratInterimaire
{
    public class InterfaceTransfertDonneeRepository : AbstractRepository<InterfaceTransfertDonneeEnt>, IInterfaceTransfertDonneeRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public InterfaceTransfertDonneeRepository(IUnitOfWork unitOfWork, ImportExportContext context)
            : base(context)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Add a list of InterfaceTransfertDonneeEnt
        /// </summary>
        /// <param name="list">List of InterfaceTransfertDonneeEnt</param>
        public void AddList(IEnumerable<InterfaceTransfertDonneeEnt> list)
        {
            bool itemAdded = false;
            foreach (var item in list)
            {
                if (!GetByInterfaceAndOrganisationAndTypeAndDonneeId(item.CodeInterface, item.CodeOrganisation, item.DonneeType, item.DonneeID))
                {
                    Add(item);
                    itemAdded = true;
                }
            }

            if (itemAdded)
            {
                unitOfWork.Save();
            }
        }

        private bool GetByInterfaceAndOrganisationAndTypeAndDonneeId(string codeInterface, string codeOrganisation, string donneeType, string donneeId)
        {
            return Get().Any(x => x.CodeInterface == codeInterface &&
                                    x.CodeOrganisation == codeOrganisation &&
                                    x.DonneeType == donneeType &&
                                    x.DonneeID == donneeId);
        }

        /// <summary>
        /// Récupére la liste des contrat à traiter 
        /// avec le statut 0 et 2
        /// </summary>
        /// <param name="codeInterface">Code interface</param>
        /// <param name="codeOrganisation">Code organisation</param>
        /// <param name="donneeType">Donnee type</param>
        /// <returns>List des interface transfert de données</returns>
        public List<InterfaceTransfertDonneeEnt> GetContratAtraiter(string codeInterface, string codeOrganisation, string donneeType)
        {
            return Get().Where(x => x.CodeInterface == codeInterface &&
                                   x.CodeOrganisation == codeOrganisation &&
                                   x.DonneeType == donneeType &&
                                   (x.Statut == 0 || x.Statut == 2))?
                        .OrderBy(x => x.DateCreation)
                        .ToList();
        }

        /// <summary>
        /// Update interface transfert donnée
        /// </summary>
        /// <param name="interfaceTransfertDonnee">L'entité interface transfert donnée à modifier</param>
        /// <returns>L'entité interface transfert donnée à modifié</returns>
        public InterfaceTransfertDonneeEnt InterfaceTransfertDonneeUpdate(InterfaceTransfertDonneeEnt interfaceTransfertDonnee)
        {
            Update(interfaceTransfertDonnee);
            unitOfWork.Save();
            return GetById(interfaceTransfertDonnee.InterfaceTransfertDonneeId);
        }
    }
}
