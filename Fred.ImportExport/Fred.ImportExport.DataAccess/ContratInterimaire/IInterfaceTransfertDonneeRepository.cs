using System.Collections.Generic;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.Entities;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.DataAccess.ContratInterimaire
{
    public interface IInterfaceTransfertDonneeRepository : IRepository<InterfaceTransfertDonneeEnt>
    {
        /// <summary>
        /// Add a list of InterfaceTransfertDonneeEnt
        /// </summary>
        /// <param name="list">List of InterfaceTransfertDonneeEnt</param>
        void AddList(IEnumerable<InterfaceTransfertDonneeEnt> list);

        /// <summary>
        /// Récupére la liste des contrat à traiter avec le statut 0 et 2
        /// </summary>
        /// <param name="codeInterface">Code interface</param>
        /// <param name="codeOrganisation">Code organisation</param>
        /// <param name="donneeType">Donnee type</param>
        /// <returns>List des interface transfert de données</returns>
        List<InterfaceTransfertDonneeEnt> GetContratAtraiter(string codeInterface, string codeOrganisation, string donneeType);

        /// <summary>
        /// Update interface transfert donnée
        /// </summary>
        /// <param name="interfaceTransfertDonnee">L'entité interface transfert donnée à modifier</param>
        /// <returns>L'entité interface transfert donnée à modifié</returns>
        InterfaceTransfertDonneeEnt InterfaceTransfertDonneeUpdate(InterfaceTransfertDonneeEnt interfaceTransfertDonnee);
    }
}
