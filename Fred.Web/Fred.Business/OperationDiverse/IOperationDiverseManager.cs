using Fred.Business.DepenseGlobale;
using Fred.Entities.EcritureComptable;
using Fred.Entities.OperationDiverse;
using Fred.Entities.RepartitionEcart;
using Fred.Framework.Models;
using Fred.Web.Shared.Models.OperationDiverse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Fred.Business.OperationDiverse
{
    public interface IOperationDiverseManager : IManager<OperationDiverseEnt>
    {
        /// <summary>
        /// Renvoie l'OD liée au groupe de remplacement
        /// </summary>
        /// <param name="groupRemplacementId">L'id groupe de remplacement</param>
        /// <returns>OD liée au groupe de remplacement</returns>
        Task<OperationDiverseEnt> GetByGroupRemplacementIdAsync(int groupRemplacementId);

        /// <summary>
        /// Renvoie une OD
        /// </summary>
        /// <param name="odID">L'Id de l'OD</param>
        /// <returns>OD</returns>
        OperationDiverseEnt GetById(int odID);

        /// <summary>
        /// Sauvegarde une liste d'OperationDiverseEnt
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <param name="operationDiverses">operationDiverses</param>
        void Save(int ciId, DateTime dateComptable, IEnumerable<OperationDiverseEnt> operationDiverses);

        /// <summary>
        /// Mets à jour une opération diverse
        /// </summary>
        /// <param name="operationDiverse">Opération diverse à mettre à jour</param>
        /// <returns>Operation Diverse mise à jour </returns>
        Task<OperationDiverseEnt> UpdateAsync(OperationDiverseEnt operationDiverse);

        /// <summary>
        /// Mets à jour une liste d'opérations diverses
        /// </summary>
        /// <param name="operationsDiverses">Opérations diverses à mettre à jour</param>
        /// <returns>Liste de OperationDiverse mise à jour</returns>
        List<OperationDiverseEnt> Update(List<OperationDiverseEnt> operationsDiverses);

        /// <summary>
        /// Retourne la liste de tous les OperationDiverseEnt pour un ci et une dateComptable;
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <param name="withIncludes">inclut les objets sous jacents</param>
        /// <returns>liste de OperationDiverseEnt</returns>
        Task<IEnumerable<OperationDiverseEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptable, bool withIncludes = true);

        /// <summary>
        /// Retourne la liste de tous les OperationDiverseEnt pour un ci et une periode;
        /// </summary>
        /// <param name="ciId">CiId</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin </param>
        /// <returns>liste de OperationDiverseEnt</returns>
        Task<IEnumerable<OperationDiverseEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptableDebut, DateTime dateComptableFin);

        /// <summary>
        /// Remet les ods, qui ne sont pas d'ecart,a non cloturée et sans date de cloture 
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">Identifiant d'un CI</param>
        /// <param name="dateComptable">Date comptable</param>
        /// <param name="calculateRepartitionResult">Liste de <see cref="RepartitionEcartEnt"/></param>
        /// <returns>Result de string</returns>   
        Task<Result<string>> CloseOdsAsync(int societeId, int ciId, DateTime dateComptable, List<RepartitionEcartEnt> calculateRepartitionResult);

        /// <summary>
        /// Remet les ods, qui ne sont pas d'ecart,a non cloturée et sans date de cloture 
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant de CI</param>
        /// <param name="dateComptable">Date comptable</param>
        /// <param name="calulateResults">Liste de <see cref="RepartitionEcartEnt"/></param>
        /// <returns>Result de string </returns>
        Task<Result<string>> CloseOdsAsync(List<int> ciIds, DateTime dateComptable, List<RepartitionEcartEnt> calulateResults);

        /// <summary>
        /// Supprimes toutes le od d'ecarts
        /// </summary>
        /// <param name="ciId">Identifiant d'un CI</param>
        /// <param name="dateComptable">Date comptable</param>
        Task UnCloseOdsAsync(int ciId, DateTime dateComptable);

        /// <summary>
        /// Supprimes toutes le od d'ecarts
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant de CI</param>
        /// <param name="dateComptable">Date comptable</param>
        Task UnCloseOdsAsync(List<int> ciIds, DateTime dateComptable);

        /// <summary>
        /// Récupération de la liste des OD selon un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Liste des OD du CI</returns>
        Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListAsync(int ciId);

        /// <summary>
        /// Ajoute une Operation Diverse
        /// </summary>
        /// <param name="operationDiverseEnt">Opération diverse à ajouter</param>
        /// <returns>Operation Diverse</returns>
        OperationDiverseEnt AddOperationDiverse(OperationDiverseEnt operationDiverseEnt);

        /// <summary>
        /// Supprime une Opération diverse
        /// </summary>
        /// <param name="operationDiverseEnt">Opération diverse à supprimer</param>
        void Delete(OperationDiverseEnt operationDiverseEnt);

        /// <summary>
        /// Retourne la liste des opérations diverses selon les paramètres
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>    
        /// <param name="periodeDebut">Date periode début</param>
        /// <param name="periodeFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <returns>Liste d'opération diverse</returns>
        Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListAsync(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId);

        /// <summary>
        /// Retourne la liste des opérations diverses selon les paramètres
        /// </summary>
        /// <param name="filtres">Liste de <see cref="DepenseGlobaleFiltre"/></param>
        /// <returns>Liste de <see cref="OperationDiverseEnt"/></returns>
        Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListAsync(List<DepenseGlobaleFiltre> filtres);

        Task<byte[]> GetFichierExempleChargementODAsync(int ciId, DateTime dateComptable);

        /// <summary>
        /// Importation des Operations Diverses 
        /// </summary>
        /// <param name="dateComptable">date comptable recupéré de l'écran Rapprochement Comta/Gestion</param>
        /// <param name="stream">stream</param>
        /// <returns><see cref="ImportODResult"/></returns>
        ImportODResult ImportOperationDiverses(DateTime dateComptable, Stream stream);

        /// <summary>
        /// Génére une  liste d'OD en fonction d'un model
        /// </summary>
        /// <param name="operationDiverses"><see cref="OperationDiverseCommandeFtpModel"/></param>
        void GenerateOperationDiverse(IEnumerable<OperationDiverseCommandeFtpModel> operationDiverses);

        /// <summary>
        /// Génére une liste d'OD inverse à partir d'une liste d'écriture comptable
        /// </summary>
        /// <param name="ecritureComptables">Liste d'écriture comptable</param>
        /// <remarks>Les écritures comptable doivent avoir des OD associées</remarks>
        Task GenerateRevertedOperationDiverseAsync(List<EcritureComptableEnt> ecritureComptables);

        /// <summary>
        /// Retourne la liste des opérations diverses en fonction d'une liste d'identifiant de commande
        /// </summary>
        /// <param name="commandeIds">Liste d'identifiant de commande</param>
        /// <returns>Liste d'operation diverse</returns>
        Task<IReadOnlyList<OperationDiverseEnt>> GetOperationDiverseListAsync(List<int> commandeIds);

        /// <summary>
        /// Traite les Dépenses de Type OD qui n'ont pas la bonne Tache (car Tache liée à un mauvais CI)
        /// </summary>
        /// <param name="ods">Liste des ODs non modifiée</param>
        /// <returns><see cref="OperationDiverseEnt"/>Liste mise à jour</returns>
        IEnumerable<OperationDiverseEnt> ComputeOdsWithoutCorrectTache(IEnumerable<OperationDiverseEnt> ods);

        /// <summary>
        /// Récupération de la liste des OD selon un CI, en incluant la Nature via l'EcritureComptable
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Liste des OD du CI</returns>
        Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListWithNatureAsync(int ciId);

        PreFillingOperationDiverseModel GetPreFillingOD(int ciId, int? ecritureComptableId, int familleOdId);
    }
}
